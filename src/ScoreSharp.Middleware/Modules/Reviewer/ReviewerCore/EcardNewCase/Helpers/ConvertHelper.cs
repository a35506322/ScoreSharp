using ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore.EcardNewCase.Models;

namespace ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore.EcardNewCase.Helpers;

public static class ConvertHelper
{
    public static CardStatus ConvertCardStatus(CaseContext caseContext)
    {
        var validCodes = new[]
        {
            回覆代碼.匯入成功,
            回覆代碼.申請書異常,
            回覆代碼.附件異常,
            回覆代碼.ECARD_FILE_DB_連線錯誤,
            回覆代碼.查無申請書附件檔案,
        };

        if (!validCodes.Contains(caseContext.ResultCode))
        {
            throw new Exception($"回覆代碼不在轉換案件狀態內: {caseContext.ResultCode}");
        }

        // 國旅先判斷
        if (caseContext.IsCITSCard && (caseContext.ResultCode == 回覆代碼.匯入成功 || caseContext.ResultCode == 回覆代碼.附件異常))
        {
            return CardStatus.國旅人事名冊確認;
        }
        else if (caseContext.IsCITSCard && caseContext.ResultCode == 回覆代碼.ECARD_FILE_DB_連線錯誤)
        {
            return CardStatus.網路件_非卡友_檔案連線異常;
        }
        else if (caseContext.IsCITSCard && caseContext.ResultCode == 回覆代碼.申請書異常)
        {
            return CardStatus.網路件_非卡友_申請書異常;
        }
        else if (caseContext.IsCITSCard && caseContext.ResultCode == 回覆代碼.查無申請書附件檔案)
        {
            return CardStatus.網路件_非卡友_查無申請書_附件;
        }

        bool isOriginalCardholder = caseContext.CaseType == 進件類型.原卡友;

        if (caseContext.ResultCode == 回覆代碼.ECARD_FILE_DB_連線錯誤)
        {
            return isOriginalCardholder ? CardStatus.網路件_卡友_檔案連線異常 : CardStatus.網路件_非卡友_檔案連線異常;
        }
        else if (caseContext.ResultCode == 回覆代碼.申請書異常)
        {
            return isOriginalCardholder ? CardStatus.網路件_卡友_申請書異常 : CardStatus.網路件_非卡友_申請書異常;
        }
        else if (caseContext.ResultCode == 回覆代碼.查無申請書附件檔案)
        {
            return isOriginalCardholder ? CardStatus.網路件_卡友_查無申請書_附件 : CardStatus.網路件_非卡友_查無申請書_附件;
        }
        else if (caseContext.ResultCode == 回覆代碼.附件異常)
        {
            return CardStatus.網路件_非卡友_待檢核;
        }

        if (caseContext.CaseType == 進件類型.原卡友)
        {
            return CardStatus.網路件_卡友_待檢核;
        }
        else
        {
            if (caseContext.IDType == IDType.存戶 || caseContext.IDType == IDType.持他行卡 || caseContext.IDType == IDType.自然人憑證)
            {
                if (string.IsNullOrEmpty(caseContext.MyDataCaseNo))
                {
                    return CardStatus.網路件_非卡友_待檢核;
                }
                else
                {
                    return CardStatus.網路件_等待MyData附件;
                }
            }
            // ! 此為小白件，主因數位峰之前談的規格問題..
            else if (caseContext.IDType == null)
            {
                if (string.IsNullOrEmpty(caseContext.MyDataCaseNo))
                {
                    return CardStatus.網路件_書面申請等待列印申請書及回郵信封;
                }
                else
                {
                    return CardStatus.網路件_書面申請等待MyData;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(caseContext.MyDataCaseNo))
                {
                    return CardStatus.網路件_非卡友_待檢核;
                }
                else
                {
                    return CardStatus.網路件_等待MyData附件;
                }
            }
        }
    }

    public static CardStatus ConvertRetryCardStatus(string returnCode) =>
        returnCode switch
        {
            "0005" => CardStatus.網路件初始_待重新發送_非定義值,
            "0006" => CardStatus.網路件初始_待重新發送_資料長度過長,
            "0007" => CardStatus.網路件初始_待重新發送_必要欄位不能為空值,
            "0013" => CardStatus.網路件初始_待重新發送_ECARD_FILE_DB_例外錯誤,
            "0014" => CardStatus.網路件初始_待重新發送_查無申請書附件檔案,
            _ => throw new Exception($"回覆代碼不在轉換案件狀態內: {returnCode}"),
        };

    public static 進件類型 ConvertCaseType(string idType, bool isCITSCard)
    {
        if (isCITSCard)
        {
            return 進件類型.國旅卡;
        }
        else if (idType == IDTypeConstant.卡友)
        {
            return 進件類型.原卡友;
        }
        else
        {
            return 進件類型.非卡友;
        }
    }

    public static IDType? ConvertIDType(string idType) =>
        idType switch
        {
            "存戶" => IDType.存戶,
            "卡友" => IDType.卡友,
            "持他行卡" => IDType.持他行卡,
            "自然人憑證" => IDType.自然人憑證,
            // ! 此為小白件，主因數位峰之前談的規格問題..
            "" => null,
            _ => throw new Exception($"IDType is not valid {idType}"),
        };

    public static BillAddressType? ConvertBillAddressType(string billAddress) =>
        billAddress switch
        {
            "1" => BillAddressType.同戶籍地址,
            "2" => BillAddressType.同居住地址,
            "3" => BillAddressType.同公司地址,
            "" => null,
            _ => throw new Exception($"BillAddressType is not valid {billAddress}"),
        };

    public static SendCardAddressType? ConvertSendCardAddressType(string sendCardAddress) =>
        sendCardAddress switch
        {
            "1" => SendCardAddressType.同戶籍地址,
            "2" => SendCardAddressType.同居住地址,
            "3" => SendCardAddressType.同公司地址,
            "" => null,
            _ => throw new Exception($"SendCardAddressType is not valid {sendCardAddress}"),
        };

    public static CardOwner? ConvertCardOwner(string cardOwner) =>
        cardOwner switch
        {
            "1" => CardOwner.正卡,
            "2" => CardOwner.附卡,
            "3" => CardOwner.正卡_附卡,

            "" => null,
            _ => throw new Exception($"CardOwner is not valid {cardOwner}"),
        };

    public static Sex? ConvertSex(string sex) =>
        sex switch
        {
            "1" => Sex.男,
            "2" => Sex.女,
            "" => null,
            _ => throw new Exception($"Sex is not valid {sex}"),
        };

    public static IDTakeStatus? ConvertIDTakeStatus(string idTakeStatus) =>
        idTakeStatus switch
        {
            "1" => IDTakeStatus.初發,
            "2" => IDTakeStatus.補發,
            "3" => IDTakeStatus.換發,
            "" => null,
            _ => throw new Exception($"IDTakeStatus is not valid {idTakeStatus}"),
        };

    public static BillType? ConvertBillType(string billType) =>
        billType switch
        {
            "1" => BillType.電子帳單,
            "2" => BillType.簡訊帳單,
            "3" => BillType.紙本帳單,
            "4" => BillType.LINE帳單,
            "" => null,
            _ => throw new Exception($"BillType is not valid {billType}"),
        };

    public static string ConvertYN(string code) =>
        code switch
        {
            "1" => "Y",
            "0" => "N",
            "" => "",
            _ => throw new Exception($"YesNoCode is not valid {code}"),
        };

    public static Education? ConvertEducation(string education) =>
        education switch
        {
            "1" => Education.博士,
            "2" => Education.碩士,
            "3" => Education.大學,
            "4" => Education.專科,
            "5" => Education.高中高職,
            "6" => Education.其他,
            "" => null,
            _ => throw new Exception($"Education is not valid {education}"),
        };

    public static LiveOwner? ConvertLiveOwner(string liveOwner) =>
        liveOwner switch
        {
            "1" => LiveOwner.本人,
            "2" => LiveOwner.配偶,
            "3" => LiveOwner.父母親,
            "4" => LiveOwner.親屬,
            "5" => LiveOwner.宿舍,
            "6" => LiveOwner.租貸,
            "7" => LiveOwner.其他,
            "" => null,
            _ => throw new Exception($"LiveOwner is not valid {liveOwner}"),
        };

    public static AutoDeductionPayType? ConvertAutoDeductionPayType(string autoDeductionPayType) =>
        autoDeductionPayType switch
        {
            "10" => AutoDeductionPayType.最低,
            "20" => AutoDeductionPayType.全額,
            "" => null,
            _ => throw new Exception($"AutoDeductionPayType is not valid {autoDeductionPayType}"),
        };

    public static Source? ConvertSource(string source) =>
        source switch
        {
            "ECARD" => Source.ECARD,
            "APP" => Source.APP,
            "紙本" => Source.紙本,
            "" => null,
            _ => throw new Exception($"Source is not valid {source}"),
        };

    public static ApplyCardKind? ConvertApplyCardKind(string applyCardKind) =>
        applyCardKind switch
        {
            "1" => ApplyCardKind.實體,
            "2" => ApplyCardKind.數位,
            "3" => ApplyCardKind.實體_數位,
            "" => null,
            _ => throw new Exception($"ApplyCardKind is not valid {applyCardKind}"),
        };

    public static AttachmentNotes? ConvertAttachmentNotes(string attachmentNotes) =>
        attachmentNotes switch
        {
            "1" => AttachmentNotes.附件異常,
            "2" => AttachmentNotes.MYDATA後補,
            "" => null,
            _ => throw new Exception($"AttachmentNotes is not valid {attachmentNotes}"),
        };
}
