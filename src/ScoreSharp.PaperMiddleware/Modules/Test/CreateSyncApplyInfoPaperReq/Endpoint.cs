using System.Globalization;
using System.Text.RegularExpressions;
using Bogus;
using Bogus.DataSets;
using hyjiacan.py4n;
using ScoreSharp.PaperMiddleware.Modules.Test.CreateSyncApplyInfoPaperReq;
using static hyjiacan.py4n.Pinyin4Net;

namespace ScoreSharp.PaperMiddleware.Modules.Test
{
    public partial class TestController
    {
        /// <summary>
        /// 創建紙本建檔同步案件Req
        /// </summary>
        [HttpPost]
        [OpenApiOperation("CreateSyncApplyInfoPaperReq")]
        public async Task<IResult> CreateSyncApplyInfoPaperReq([FromBody] CreateSyncApplyInfoPaperRequest request) =>
            Results.Ok(await _mediator.Send(new Command(request)));
    }
}

namespace ScoreSharp.PaperMiddleware.Modules.Test.CreateSyncApplyInfoPaperReq
{
    public record Command(CreateSyncApplyInfoPaperRequest createSyncApplyInfoPaperRequest) : IRequest<SyncApplyInfoPaperRequest>;

    public class Handler(ScoreSharpContext scoreSharpContext) : IRequestHandler<Command, SyncApplyInfoPaperRequest>
    {
        public async Task<SyncApplyInfoPaperRequest> Handle(Command request, CancellationToken cancellationToken)
        {
            var req = request.createSyncApplyInfoPaperRequest;

            var faker = new Faker<SyncApplyInfoPaperRequest>("zh_TW");

            var applyCreditCardInfoWithParams = await scoreSharpContext.Procedures.Usp_GetApplyCreditCardInfoWithParamsAsync();
            var allParams = applyCreditCardInfoWithParams.Where(x => x.IsActive == "Y").ToList();

            var 身分證換發地點多筆代碼 = allParams.Where(x => x.Type == "IDCardRenewalLocation").Select(x => x.StringValue).ToList();
            var AML職級別多筆代碼 = allParams.Where(x => x.Type == "AMLJobLevel").Select(x => x.StringValue).ToList();
            var AML職業別多筆代碼 = allParams.Where(x => x.Type == "AMLProfession").Select(x => x.StringValue).ToList();
            var 主要所得及資金來源多筆代碼 = allParams.Where(x => x.Type == "MainIncomeAndFund").Select(x => x.StringValue).ToList();
            var 卡片種類多筆代碼 = allParams.Where(x => x.Type == "Card").Select(x => x.StringValue).ToList();
            var 國籍多筆代碼 = allParams.Where(x => x.Type == "Citizenship").Select(x => x.StringValue).ToList();
            var 縣市多筆代碼 = allParams.Where(x => x.Type == "City").Select(x => x.StringValue).ToList();
            var 專案代號多筆代碼 = allParams.Where(x => x.Type == "ProjectCode").Select(x => x.StringValue).ToList();
            var 推廣單位多筆代碼 = allParams.Where(x => x.Type == "PromotionUnit").Select(x => x.StringValue).ToList();
            var 年費收取方式多筆代碼 = allParams.Where(x => x.Type == "AnnualFeeCollectionMethod").Select(x => x.StringValue).ToList();

            var 卡片優惠辦法多筆代碼 = await scoreSharpContext
                .SetUp_CardPromotion.Where(x => x.IsActive == "Y")
                .Select(x => x.CardPromotionCode)
                .ToListAsync();

            var 地址 = await scoreSharpContext.SetUp_AddressInfo.ToListAsync();
            var 多筆縣市 = 地址.Select(i => i.City).Distinct();
            var 縣市_區域 = 地址.GroupBy(x => x.City).ToDictionary(g => g.Key, g => g.Select(a => a.Area).Distinct());
            var 縣市_區域_街道 = 地址.GroupBy(i => new { i.City, i.Area })
                .ToDictionary(g => g.Key.City + g.Key.Area, g => g.Select(a => a.Road).Distinct());

            var 主要所得及資金來源_其他 = new[] { "兼職收入", "家庭支援", "海外收入", "藝術品買賣" };
            var AML職業別_其他 = new[] { "自由接案者", "短期補習班講師", "自營電商賣家", "直播主", "加密貨幣交易者" };
            var 公司名稱 = new[] { "大宇軟體", "迅捷資訊", "環宇工程", "綠能資源", "藍海設計" };
            var 職稱 = new[] { "專員", "設計師", "外送員", "司機", "工程師" };
            var 部門名稱 = new[] { "資訊部", "財務部", "行銷部", "人資部", "營運部" };

            faker
                .RuleFor(x => x.SyncStatus, f => req.SyncStatus)
                .RuleFor(x => x.SyncUserId, f => "SYSTEM")
                .RuleFor(x => x.ApplyNo, f => $"{DateTime.Now:yyyyMMdd}0{f.Random.Int(1, 9999):D4}")
                /* 正卡人資料 */
                .RuleFor(x => x.CardOwner, f => req.CardOwner)
                .RuleFor(x => x.M_Sex, f => f.PickRandom<Sex>())
                .RuleFor(
                    x => x.M_CHName,
                    (f, s) => $"{f.Name.LastName()}{f.Name.FirstName(s.M_Sex == Sex.女 ? Name.Gender.Female : Name.Gender.Male)}"
                )
                .RuleFor(x => x.M_ID, f => null)
                .RuleFor(
                    x => x.M_CitizenshipCode,
                    f =>
                    {
                        if (req.M_IsTaiwanNationality)
                            return "TW";
                        return f.PickRandom(國籍多筆代碼.Where(x => x != "TW"));
                    }
                )
                .RuleFor(
                    x => x.M_IDIssueDate,
                    (f, s) =>
                    {
                        if (s.M_CitizenshipCode != "TW")
                            return null;
                        var twCalender = new TaiwanCalendar();
                        var date = f.Date.Past(20);
                        var taiwanYear = twCalender.GetYear(date);
                        return $"{taiwanYear:D3}{date:MMdd}";
                    }
                )
                .RuleFor(
                    x => x.M_IDCardRenewalLocationCode,
                    (f, s) =>
                    {
                        if (s.M_CitizenshipCode != "TW")
                            return null;
                        return f.PickRandom(身分證換發地點多筆代碼);
                    }
                )
                .RuleFor(
                    x => x.M_IDTakeStatus,
                    (f, s) =>
                    {
                        if (s.M_CitizenshipCode != "TW")
                            return null;
                        return f.PickRandom<IDTakeStatus>();
                    }
                )
                .RuleFor(x => x.M_MarriageState, f => f.PickRandom<MarriageState>())
                .RuleFor(x => x.M_ChildrenCount, f => f.Random.Int(0, 4))
                .RuleFor(
                    x => x.M_BirthDay,
                    f =>
                    {
                        var twCalender = new TaiwanCalendar();
                        var date = f.Person.DateOfBirth;
                        var taiwanYear = twCalender.GetYear(date);
                        return $"{taiwanYear:D3}{date:MMdd}";
                    }
                )
                .RuleFor(x => x.M_ENName, (f, s) => GetPinyin(s.M_CHName))
                .RuleFor(
                    x => x.M_BirthCitizenshipCode,
                    f =>
                    {
                        if (req.M_IsBornInTaiwan)
                            return BirthCitizenshipCode.中華民國;
                        return BirthCitizenshipCode.其他;
                    }
                )
                .RuleFor(
                    x => x.M_BirthCitizenshipCodeOther,
                    (f, s) =>
                    {
                        if (s.M_BirthCitizenshipCode == BirthCitizenshipCode.中華民國)
                            return null;
                        return f.PickRandom(國籍多筆代碼);
                    }
                )
                .RuleFor(x => x.M_Education, f => f.PickRandom<Education>())
                .RuleFor(x => x.M_GraduatedElementarySchool, f => $"{f.Address.CityPrefix()}國小")
                // 戶籍地址
                .RuleFor(x => x.M_Reg_ZipCode, f => null)
                .RuleFor(x => x.M_Reg_City, f => f.PickRandom(多筆縣市))
                .RuleFor(x => x.M_Reg_District, (f, s) => f.PickRandom(縣市_區域[s.M_Reg_City]))
                .RuleFor(x => x.M_Reg_Road, (f, s) => f.PickRandom(縣市_區域_街道[s.M_Reg_City + s.M_Reg_District]))
                .RuleFor(x => x.M_Reg_Lane, (f, s) => f.Random.Int(1, 30).ToString())
                .RuleFor(x => x.M_Reg_Alley, (f, s) => f.Random.Int(1, 20).ToString())
                .RuleFor(x => x.M_Reg_Number, (f, s) => f.Random.Int(1, 300).ToString())
                .RuleFor(x => x.M_Reg_SubNumber, (f, s) => f.Random.Int(1, 20).ToString())
                .RuleFor(x => x.M_Reg_Floor, (f, s) => f.Random.Int(1, 15).ToString())
                .RuleFor(
                    x => x.M_Reg_Other,
                    f =>
                    {
                        var addressOther = new[]
                        {
                            $"{f.Phone.PhoneNumber("?棟")}{f.Random.Int(1, 9)}樓",
                            $"{f.Random.Int(1, 9)}{f.Random.Int(0, 9)}{f.Random.Int(0, 9)}室",
                            $"{f.Person.FirstName}{f.Phone.PhoneNumber("大樓?棟")}",
                            $"{f.Random.Int(1, 9)}F-{f.Random.Int(1, 9)}",
                            $"B{f.Random.Int(1, 3)}",
                        };

                        return f.PickRandom(addressOther);
                    }
                )
                // 通訊地址
                .RuleFor(x => x.M_Live_AddressType, f => LiveAddressType.其他)
                .RuleFor(x => x.M_Live_ZipCode, f => null)
                .RuleFor(x => x.M_Live_City, f => f.PickRandom(多筆縣市))
                .RuleFor(x => x.M_Live_District, (f, s) => f.PickRandom(縣市_區域[s.M_Live_City]))
                .RuleFor(x => x.M_Live_Road, (f, s) => f.PickRandom(縣市_區域_街道[s.M_Live_City + s.M_Live_District]))
                .RuleFor(x => x.M_Live_Lane, f => f.Random.Int(1, 30).ToString())
                .RuleFor(x => x.M_Live_Alley, f => f.Random.Int(1, 20).ToString())
                .RuleFor(x => x.M_Live_Number, f => f.Random.Int(1, 300).ToString())
                .RuleFor(x => x.M_Live_SubNumber, f => f.Random.Int(1, 20).ToString())
                .RuleFor(x => x.M_Live_Floor, f => f.Random.Int(1, 15).ToString())
                .RuleFor(
                    x => x.M_Live_Other,
                    f =>
                    {
                        var addressOther = new[]
                        {
                            $"{f.Phone.PhoneNumber("?棟")}{f.Random.Int(1, 9)}樓",
                            $"{f.Random.Int(1, 9)}{f.Random.Int(0, 9)}{f.Random.Int(0, 9)}室",
                            $"{f.Person.FirstName}{f.Phone.PhoneNumber("大樓?棟")}",
                            $"{f.Random.Int(1, 9)}F-{f.Random.Int(1, 9)}",
                            $"B{f.Random.Int(1, 3)}",
                        };

                        return f.PickRandom(addressOther);
                    }
                )
                // 帳單地址
                .RuleFor(x => x.M_Bill_AddressType, f => BillAddressType.其他)
                .RuleFor(x => x.M_Bill_ZipCode, f => null)
                .RuleFor(x => x.M_Bill_City, f => f.PickRandom(多筆縣市))
                .RuleFor(x => x.M_Bill_District, (f, s) => f.PickRandom(縣市_區域[s.M_Bill_City]))
                .RuleFor(x => x.M_Bill_Road, (f, s) => f.PickRandom(縣市_區域_街道[s.M_Bill_City + s.M_Bill_District]))
                .RuleFor(x => x.M_Bill_Lane, f => f.Random.Int(1, 30).ToString())
                .RuleFor(x => x.M_Bill_Alley, f => f.Random.Int(1, 20).ToString())
                .RuleFor(x => x.M_Bill_Number, f => f.Random.Int(1, 300).ToString())
                .RuleFor(x => x.M_Bill_SubNumber, f => f.Random.Int(1, 20).ToString())
                .RuleFor(x => x.M_Bill_Floor, f => f.Random.Int(1, 15).ToString())
                .RuleFor(
                    x => x.M_Bill_Other,
                    f =>
                    {
                        var addressOther = new[]
                        {
                            $"{f.Phone.PhoneNumber("?棟")}{f.Random.Int(1, 9)}樓",
                            $"{f.Random.Int(1, 9)}{f.Random.Int(0, 9)}{f.Random.Int(0, 9)}室",
                            $"{f.Person.FirstName}{f.Phone.PhoneNumber("大樓?棟")}",
                            $"{f.Random.Int(1, 9)}F-{f.Random.Int(1, 9)}",
                            $"B{f.Random.Int(1, 3)}",
                        };

                        return f.PickRandom(addressOther);
                    }
                )
                // 寄卡地址
                .RuleFor(x => x.M_SendCard_AddressType, f => SendCardAddressType.其他)
                .RuleFor(x => x.M_SendCard_ZipCode, f => null)
                .RuleFor(x => x.M_SendCard_City, f => f.PickRandom(多筆縣市))
                .RuleFor(x => x.M_SendCard_District, (f, s) => f.PickRandom(縣市_區域[s.M_SendCard_City]))
                .RuleFor(x => x.M_SendCard_Road, (f, s) => f.PickRandom(縣市_區域_街道[s.M_SendCard_City + s.M_SendCard_District]))
                .RuleFor(x => x.M_SendCard_Lane, f => f.Random.Int(1, 30).ToString())
                .RuleFor(x => x.M_SendCard_Alley, f => f.Random.Int(1, 20).ToString())
                .RuleFor(x => x.M_SendCard_Number, f => f.Random.Int(1, 300).ToString())
                .RuleFor(x => x.M_SendCard_SubNumber, f => f.Random.Int(1, 20).ToString())
                .RuleFor(x => x.M_SendCard_Floor, f => f.Random.Int(1, 15).ToString())
                .RuleFor(
                    x => x.M_SendCard_Other,
                    f =>
                    {
                        var addressOther = new[]
                        {
                            $"{f.Phone.PhoneNumber("?棟")}{f.Random.Int(1, 9)}樓",
                            $"{f.Random.Int(1, 9)}{f.Random.Int(0, 9)}{f.Random.Int(0, 9)}室",
                            $"{f.Person.FirstName}{f.Phone.PhoneNumber("大樓?棟")}",
                            $"{f.Random.Int(1, 9)}F-{f.Random.Int(1, 9)}",
                            $"B{f.Random.Int(1, 3)}",
                        };

                        return f.PickRandom(addressOther);
                    }
                )
                // 聯絡資訊
                .RuleFor(x => x.M_HouseRegPhone, f => f.Phone.PhoneNumber("0#-########"))
                .RuleFor(x => x.M_LivePhone, f => f.Phone.PhoneNumber("0#-########"))
                .RuleFor(x => x.M_Mobile, f => f.Phone.PhoneNumber("09########"))
                .RuleFor(x => x.M_LiveOwner, f => f.PickRandom<LiveOwner>())
                .RuleFor(x => x.M_LiveYear, f => f.Random.Int(1, 30))
                .RuleFor(x => x.M_EMail, (f, s) => $"{f.Phone.PhoneNumber("?????##")}@gmail.com")
                // 公司資訊
                .RuleFor(x => x.M_CompName, f => $"{f.PickRandom(公司名稱)}有限公司")
                .RuleFor(x => x.M_CompTrade, f => f.PickRandom<CompTrade>())
                .RuleFor(x => x.M_AMLProfessionCode, f => f.PickRandom(AML職業別多筆代碼))
                .RuleFor(
                    x => x.M_AMLProfessionOther,
                    (f, s) =>
                    {
                        var 其他代碼 = allParams.FirstOrDefault(x => x.Type == "AMLProfession" && x.Name.Contains("其他"))?.StringValue;
                        if (s.M_AMLProfessionCode != 其他代碼)
                            return null;
                        return f.PickRandom(AML職業別_其他);
                    }
                )
                .RuleFor(x => x.M_AMLJobLevelCode, f => f.PickRandom(AML職級別多筆代碼))
                .RuleFor(x => x.M_CompJobLevel, f => f.PickRandom<CompJobLevel>())
                .RuleFor(x => x.M_Comp_ZipCode, f => null)
                .RuleFor(x => x.M_Comp_City, f => f.PickRandom(多筆縣市))
                .RuleFor(x => x.M_Comp_District, (f, s) => f.PickRandom(縣市_區域[s.M_Comp_City]))
                .RuleFor(x => x.M_Comp_Road, (f, s) => f.PickRandom(縣市_區域_街道[s.M_Comp_City + s.M_Comp_District]))
                .RuleFor(x => x.M_Comp_Lane, f => f.Random.Int(1, 30).ToString())
                .RuleFor(x => x.M_Comp_Alley, f => f.Random.Int(1, 20).ToString())
                .RuleFor(x => x.M_Comp_Number, f => f.Random.Int(1, 300).ToString())
                .RuleFor(x => x.M_Comp_SubNumber, f => f.Random.Int(1, 20).ToString())
                .RuleFor(x => x.M_Comp_Floor, f => f.Random.Int(1, 15).ToString())
                .RuleFor(
                    x => x.M_Comp_Other,
                    f =>
                    {
                        var addressOther = new[]
                        {
                            $"{f.Phone.PhoneNumber("?棟")}{f.Random.Int(1, 9)}樓",
                            $"{f.Random.Int(1, 9)}{f.Random.Int(0, 9)}{f.Random.Int(0, 9)}室",
                            $"{f.Person.FirstName}{f.Phone.PhoneNumber("大樓?棟")}",
                            $"{f.Random.Int(1, 9)}F-{f.Random.Int(1, 9)}",
                            $"B{f.Random.Int(1, 3)}",
                        };

                        return f.PickRandom(addressOther);
                    }
                )
                .RuleFor(x => x.M_CompPhone, f => $"{f.Phone.PhoneNumber("0#-########")}#{f.Random.Number(1000, 9999)}")
                .RuleFor(x => x.M_CompID, f => f.Phone.PhoneNumber("########"))
                .RuleFor(x => x.M_CompJobTitle, f => f.PickRandom(職稱))
                .RuleFor(x => x.M_DepartmentName, f => f.PickRandom(部門名稱))
                .RuleFor(x => x.M_CurrentMonthIncome, f => f.Random.Int(35000, 1000000))
                .RuleFor(
                    x => x.M_EmploymentDate,
                    f =>
                    {
                        var date = f.Date.Past(20);
                        var taiwanYear = date.Year - 1911;
                        return $"{taiwanYear:D3}{date:MMdd}";
                    }
                )
                .RuleFor(x => x.M_CompSeniority, f => f.Random.Int(1, 30))
                // 學生資訊
                .RuleFor(x => x.M_ReissuedCardType, f => f.PickRandom(new[] { "Y", "N" }))
                .RuleFor(x => x.M_IsAgreeDataOpen, f => f.PickRandom(new[] { "Y", "N" }))
                .RuleFor(x => x.M_MainIncomeAndFundCodes, f => string.Join(",", f.PickRandom(主要所得及資金來源多筆代碼, f.Random.Int(1, 4))))
                .RuleFor(
                    x => x.M_MainIncomeAndFundOther,
                    (f, s) =>
                    {
                        var 其他代碼 = allParams.FirstOrDefault(x => x.Type == "MainIncomeAndFund" && x.Name == "其他")?.StringValue;

                        if (!s.M_MainIncomeAndFundCodes.Split(',').Contains(其他代碼))
                            return null;
                        return f.PickRandom(主要所得及資金來源_其他);
                    }
                )
                .RuleFor(x => x.ElecCodeId, f => f.Phone.PhoneNumber("##########"))
                .RuleFor(x => x.M_IsHoldingBankCard, f => f.PickRandom(new[] { "Y", "N" }))
                .RuleFor(x => x.FirstBrushingGiftCode, f => f.Random.Int(1, 15).ToString("D2"))
                .RuleFor(x => x.AnnualFeePaymentType, f => f.PickRandom(年費收取方式多筆代碼))
                .RuleFor(x => x.M_IsAcceptEasyCardDefaultBonus, f => f.PickRandom(new[] { "Y", "N" }))
                .RuleFor(x => x.IsAgreeMarketing, f => f.PickRandom(new[] { "Y", "N" }))
                .RuleFor(x => x.BillType, f => f.PickRandom<BillType>())
                /* 申請書其他相關資訊 */
                .RuleFor(x => x.ProjectCode, (f, s) => f.PickRandom(專案代號多筆代碼))
                .RuleFor(x => x.PromotionUnit, (f, s) => f.PickRandom(推廣單位多筆代碼))
                .RuleFor(x => x.PromotionUser, f => $"{f.Name.LastName()}{f.Name.FirstName()}")
                .RuleFor(x => x.AcceptType, f => f.PickRandom<AcceptType>())
                .RuleFor(x => x.AnliNo, f => $"{f.PickRandom(new[] { "2", "3", "8" })}{f.Random.ReplaceNumbers("########")}")
                .RuleFor(x => x.CaseType, f => f.PickRandom<CaseType>())
                /*卡片資訊*/
                .RuleFor(
                    x => x.CardInfo,
                    (f, s) =>
                    {
                        var cardInfos = new List<CardInfoDto>();
                        var selectedCardTypes = 卡片種類多筆代碼.ToList();

                        switch (req.CardOwner)
                        {
                            case CardOwner.正卡:
                            {
                                var mCardTypes = selectedCardTypes.Take(req.M_CardCount).ToList();
                                foreach (var card in mCardTypes)
                                {
                                    cardInfos.Add(
                                        new CardInfoDto
                                        {
                                            ID = s.M_ID,
                                            UserType = UserType.正卡人,
                                            ApplyCardType = card,
                                            CardStatus = req.CardStatus,
                                            ApplyCardKind = f.PickRandom<ApplyCardKind>(),
                                        }
                                    );
                                }

                                break;
                            }

                            case CardOwner.附卡:
                            {
                                var sCardTypes = selectedCardTypes.Take(req.S_CardCount).ToList();
                                foreach (var card in sCardTypes)
                                {
                                    cardInfos.Add(
                                        new CardInfoDto
                                        {
                                            ID = s.M_ID,
                                            UserType = UserType.附卡人,
                                            ApplyCardType = card,
                                            CardStatus = req.CardStatus,
                                            ApplyCardKind = f.PickRandom<ApplyCardKind>(),
                                        }
                                    );
                                }

                                break;
                            }

                            case CardOwner.正卡_附卡:
                            {
                                if (req.S_CardCount > req.M_CardCount)
                                    throw new ArgumentException("正附卡模式下，附卡數量不可大於正卡數量");

                                // 正卡先產生
                                var mCardTypes = selectedCardTypes.Take(req.M_CardCount).ToList();

                                foreach (var card in mCardTypes)
                                {
                                    cardInfos.Add(
                                        new CardInfoDto
                                        {
                                            ID = s.M_ID,
                                            UserType = UserType.正卡人,
                                            ApplyCardType = card,
                                            CardStatus = req.CardStatus,
                                            ApplyCardKind = f.PickRandom<ApplyCardKind>(),
                                        }
                                    );
                                }

                                // 附卡從正卡卡種中隨機選出 S_CardCount 種 (不重複)
                                var sCardTypes = mCardTypes.OrderBy(_ => f.Random.Int()).Take(req.S_CardCount).ToList();

                                foreach (var card in sCardTypes)
                                {
                                    cardInfos.Add(
                                        new CardInfoDto
                                        {
                                            ID = s.M_ID,
                                            UserType = UserType.附卡人,
                                            ApplyCardType = card, // ✅ 直接用已選的正卡卡種
                                            CardStatus = req.CardStatus,
                                            ApplyCardKind = f.PickRandom<ApplyCardKind>(),
                                        }
                                    );
                                }

                                break;
                            }

                            default:
                                throw new InvalidOperationException($"未知的 CardOwner 類型: {req.CardOwner}");
                        }

                        return cardInfos;
                    }
                )
                /*申請書歷程紀錄*/
                .RuleFor(
                    x => x.ApplyProcess,
                    (f, s) =>
                    {
                        var allStatuses = new[]
                        {
                            CardStatus.紙本件_初始,
                            CardStatus.紙本件_一次件檔中,
                            CardStatus.紙本件_二次件檔中,
                            CardStatus.紙本件_建檔審核中,
                        };

                        // 找到 req.CardStatus 在順序陣列中的 index
                        int currentIndex = Array.IndexOf(allStatuses, req.CardStatus);
                        if (currentIndex == -1)
                        {
                            throw new InvalidOperationException($"未定義的 CardStatus：{req.CardStatus}");
                        }

                        // 正確地產生所有歷程
                        var selectedStatuses = allStatuses.Take(currentIndex + 1);

                        var baseTime = f.Date.Between(DateTime.Today.AddDays(-30), DateTime.Today.AddDays(-1));
                        var applyProcesses = new List<ApplyProcessDto>();

                        foreach (var status in selectedStatuses)
                        {
                            var startTime = baseTime;
                            var endTime = startTime.AddSeconds(f.Random.Int(3, 10));
                            baseTime = endTime;

                            applyProcesses.Add(
                                new ApplyProcessDto
                                {
                                    Process = status.ToString(),
                                    StartTime = startTime,
                                    EndTime = endTime,
                                    Notes = f.Lorem.Sentence(),
                                    ProcessUserId = "SYSTEM",
                                }
                            );
                        }

                        return applyProcesses;
                    }
                );

            /*附卡人申請書資料*/
            if (req.CardOwner != CardOwner.正卡)
            {
                faker
                    .RuleFor(x => x.S1_ApplicantRelationship, f => f.PickRandom<ApplicantRelationship>())
                    .RuleFor(x => x.S1_CHName, f => $"{f.Name.LastName()}{f.Name.FirstName()}")
                    .RuleFor(x => x.S1_ID, f => null)
                    .RuleFor(
                        x => x.S1_IDIssueDate,
                        f =>
                        {
                            var twCalender = new TaiwanCalendar();
                            var date = f.Date.Past();
                            var taiwanYear = twCalender.GetYear(date);
                            return $"{taiwanYear:D3}{date:MMdd}";
                        }
                    )
                    .RuleFor(x => x.S1_IDCardRenewalLocationCode, f => f.PickRandom(身分證換發地點多筆代碼))
                    .RuleFor(x => x.S1_IDTakeStatus, f => f.PickRandom<IDTakeStatus>())
                    .RuleFor(x => x.S1_Sex, f => f.PickRandom<Sex>())
                    .RuleFor(x => x.S1_MarriageState, f => f.PickRandom<MarriageState>())
                    .RuleFor(
                        x => x.S1_BirthDay,
                        f =>
                        {
                            var twCalender = new TaiwanCalendar();
                            var date = f.Person.DateOfBirth;
                            var taiwanYear = twCalender.GetYear(date);
                            return $"{taiwanYear:D3}{date:MMdd}";
                        }
                    )
                    .RuleFor(x => x.S1_ENName, (f, s) => GetPinyin(s.S1_CHName))
                    .RuleFor(
                        x => x.S1_BirthCitizenshipCode,
                        f =>
                        {
                            if (req.S_IsBornInTaiwan)
                                return BirthCitizenshipCode.中華民國;
                            return BirthCitizenshipCode.其他;
                        }
                    )
                    .RuleFor(
                        x => x.S1_BirthCitizenshipCodeOther,
                        (f, s) =>
                        {
                            if (s.S1_BirthCitizenshipCode == BirthCitizenshipCode.中華民國)
                                return null;
                            return f.PickRandom(國籍多筆代碼);
                        }
                    )
                    .RuleFor(
                        x => x.S1_CitizenshipCode,
                        f =>
                        {
                            if (req.S_IsTaiwanNationality)
                                return "TW";
                            return f.PickRandom(國籍多筆代碼.Where(x => x != "TW"));
                        }
                    )
                    // 附卡人戶籍地址
                    .RuleFor(x => x.S1_Live_AddressType, f => ResidenceType.其他)
                    .RuleFor(x => x.S1_Live_ZipCode, f => null)
                    .RuleFor(x => x.S1_Live_City, f => f.PickRandom(多筆縣市))
                    .RuleFor(x => x.S1_Live_District, (f, s) => f.PickRandom(縣市_區域[s.S1_Live_City]))
                    .RuleFor(x => x.S1_Live_Road, (f, s) => f.PickRandom(縣市_區域_街道[s.S1_Live_City + s.S1_Live_District]))
                    .RuleFor(x => x.S1_Live_Lane, (f, s) => f.Random.Int(1, 30).ToString())
                    .RuleFor(x => x.S1_Live_Alley, (f, s) => f.Random.Int(1, 20).ToString())
                    .RuleFor(x => x.S1_Live_Number, (f, s) => f.Random.Int(1, 300).ToString())
                    .RuleFor(x => x.S1_Live_SubNumber, (f, s) => f.Random.Int(1, 20).ToString())
                    .RuleFor(x => x.S1_Live_Floor, (f, s) => f.Random.Int(1, 15).ToString())
                    .RuleFor(
                        x => x.S1_Live_Other,
                        f =>
                        {
                            var addressOther = new[]
                            {
                                $"{f.Phone.PhoneNumber("?棟")}{f.Random.Int(1, 9)}樓",
                                $"{f.Random.Int(1, 9)}{f.Random.Int(0, 9)}{f.Random.Int(0, 9)}室",
                                $"{f.Person.FirstName}{f.Phone.PhoneNumber("大樓?棟")}",
                                $"{f.Random.Int(1, 9)}F-{f.Random.Int(1, 9)}",
                                $"B{f.Random.Int(1, 3)}",
                            };

                            return f.PickRandom(addressOther);
                        }
                    )
                    // 附卡1寄卡地址
                    .RuleFor(x => x.S1_SendCard_AddressType, f => ShippingCardAddressType.同正卡寄卡地址)
                    .RuleFor(x => x.S1_SendCard_ZipCode, f => null)
                    .RuleFor(x => x.S1_SendCard_City, f => f.PickRandom(多筆縣市))
                    .RuleFor(x => x.S1_SendCard_District, (f, s) => f.PickRandom(縣市_區域[s.S1_SendCard_City]))
                    .RuleFor(x => x.S1_SendCard_Road, (f, s) => f.PickRandom(縣市_區域_街道[s.S1_SendCard_City + s.S1_SendCard_District]))
                    .RuleFor(x => x.S1_SendCard_Lane, f => f.Random.Int(1, 30).ToString())
                    .RuleFor(x => x.S1_SendCard_Alley, f => f.Random.Int(1, 20).ToString())
                    .RuleFor(x => x.S1_SendCard_Number, f => f.Random.Int(1, 300).ToString())
                    .RuleFor(x => x.S1_SendCard_SubNumber, f => f.Random.Int(1, 20).ToString())
                    .RuleFor(x => x.S1_SendCard_Floor, f => f.Random.Int(1, 15).ToString())
                    .RuleFor(
                        x => x.S1_SendCard_Other,
                        f =>
                        {
                            var addressOther = new[]
                            {
                                $"{f.Phone.PhoneNumber("?棟")}{f.Random.Int(1, 9)}樓",
                                $"{f.Random.Int(1, 9)}{f.Random.Int(0, 9)}{f.Random.Int(0, 9)}室",
                                $"{f.Person.FirstName}{f.Phone.PhoneNumber("大樓?棟")}",
                                $"{f.Random.Int(1, 9)}F-{f.Random.Int(1, 9)}",
                                $"B{f.Random.Int(1, 3)}",
                            };

                            return f.PickRandom(addressOther);
                        }
                    )
                    .RuleFor(x => x.S1_LivePhone, f => f.Phone.PhoneNumber("0#-########"))
                    .RuleFor(x => x.S1_Mobile, f => f.Phone.PhoneNumber("09########"))
                    .RuleFor(x => x.S1_CompPhone, f => $"{f.Phone.PhoneNumber("0#-########")}#{f.Random.Number(1000, 9999)}")
                    .RuleFor(x => x.S1_CompName, f => $"{f.PickRandom(公司名稱)}有限公司")
                    .RuleFor(x => x.S1_CompJobTitle, f => f.PickRandom(職稱));
            }

            var response = faker.Generate();

            return response;
        }

        private string GetPinyin(string chineseName)
        {
            PinyinFormat format = PinyinFormat.WITHOUT_TONE | PinyinFormat.UPPERCASE | PinyinFormat.WITH_U_AND_COLON;
            var formatName = GetPinyinArray(chineseName, format);
            var cleanPinyin = formatName.Select(item => item.FirstOrDefault() ?? string.Empty); // 取每個拼音項的第一個字
            string finalName = string.Join(" ", cleanPinyin);
            return finalName;
        }
    }
}
