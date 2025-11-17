using Bogus;
using ScoreSharp.PaperMiddleware.Modules.Test.CreateSyncApplyInfoWebWhiteReq;

namespace ScoreSharp.PaperMiddleware.Modules.Test
{
    public partial class TestController
    {
        /// <summary>
        /// Create SyncApplyInfoWebWhite Request
        /// </summary>
        [HttpPost]
        [OpenApiOperation("CreateSyncApplyInfoWebWhiteReq")]
        public async Task<IResult> CreateSyncApplyInfoWebWhiteReq([FromBody] CreateSyncApplyInfoWebWhiteReqRequest request) =>
            Results.Ok(await _mediator.Send(new Command(request)));
    }
}

namespace ScoreSharp.PaperMiddleware.Modules.Test.CreateSyncApplyInfoWebWhiteReq
{
    public record Command(CreateSyncApplyInfoWebWhiteReqRequest request) : IRequest<SyncApplyInfoWebWhiteRequest>;

    public class Handler(ScoreSharpContext scoreSharpContext) : IRequestHandler<Command, SyncApplyInfoWebWhiteRequest>
    {
        public async Task<SyncApplyInfoWebWhiteRequest> Handle(Command request, CancellationToken cancellationToken)
        {
            var applyNo = request.request.ApplyNo;
            var syncUserId = request.request.SyncUserId;

            // 讀取 Main Handle
            var main = await scoreSharpContext
                .Reviewer_ApplyCreditCardInfoMain.AsNoTracking()
                .FirstOrDefaultAsync(x => x.ApplyNo == applyNo, cancellationToken);

            if (main == null)
            {
                throw new NotFoundException($"找不到申請書編號: {applyNo}");
            }

            var handles = await scoreSharpContext
                .Reviewer_ApplyCreditCardInfoHandle.AsNoTracking()
                .Where(x => x.ApplyNo == applyNo)
                .ToListAsync(cancellationToken);

            if (
                handles.Any(x =>
                    x.CardStatus != CardStatus.網路件_書面申請等待列印申請書及回郵信封 && x.CardStatus != CardStatus.網路件_書面申請等待MyData
                )
            )
            {
                throw new BusinessBadRequestException($"申請書編號: {applyNo} 狀態不正確");
            }

            // Process 新增建檔審核中
            var process = new ApplyProcess
            {
                Process = "建檔審核中",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                Notes = "紙本同步網路小白件",
                ProcessUserId = syncUserId,
            };

            // Map to SyncApplyInfoWebWhiteRequest
            var syncRequest = new SyncApplyInfoWebWhiteRequest
            {
                ApplyNo = main.ApplyNo,
                SyncUserId = main.LastUpdateUserId ?? "SYSTEM",
                CardOwner = main.CardOwner ?? CardOwner.正卡,

                // 正卡基本資料
                M_CHName = main.CHName ?? string.Empty,
                M_ID = main.ID ?? string.Empty,
                M_Sex = main.Sex,
                M_BirthDay = main.BirthDay,
                M_ENName = main.ENName,
                M_BirthCitizenshipCode = main.BirthCitizenshipCode,
                M_BirthCitizenshipCodeOther = main.BirthCitizenshipCodeOther,
                M_CitizenshipCode = main.CitizenshipCode,
                M_IDIssueDate = main.IDIssueDate,
                M_IDCardRenewalLocationCode = main.IDCardRenewalLocationCode,
                M_IDTakeStatus = main.IDTakeStatus,

                // 戶籍地址
                M_Reg_ZipCode = main.Reg_ZipCode,
                M_Reg_City = main.Reg_City,
                M_Reg_District = main.Reg_District,
                M_Reg_Road = main.Reg_Road,
                M_Reg_Lane = main.Reg_Lane,
                M_Reg_Alley = main.Reg_Alley,
                M_Reg_Number = main.Reg_Number,
                M_Reg_SubNumber = main.Reg_SubNumber,
                M_Reg_Floor = main.Reg_Floor,
                M_Reg_Other = main.Reg_Other,

                // 居住地址
                M_Live_AddressType = main.LiveAddressType,
                M_Live_ZipCode = main.Live_ZipCode,
                M_Live_City = main.Live_City,
                M_Live_District = main.Live_District,
                M_Live_Road = main.Live_Road,
                M_Live_Lane = main.Live_Lane,
                M_Live_Alley = main.Live_Alley,
                M_Live_Number = main.Live_Number,
                M_Live_SubNumber = main.Live_SubNumber,
                M_Live_Floor = main.Live_Floor,
                M_Live_Other = main.Live_Other,

                // 帳單地址
                M_Bill_AddressType = main.BillAddressType,
                M_Bill_ZipCode = main.Bill_ZipCode,
                M_Bill_City = main.Bill_City,
                M_Bill_District = main.Bill_District,
                M_Bill_Road = main.Bill_Road,
                M_Bill_Lane = main.Bill_Lane,
                M_Bill_Alley = main.Bill_Alley,
                M_Bill_Number = main.Bill_Number,
                M_Bill_SubNumber = main.Bill_SubNumber,
                M_Bill_Floor = main.Bill_Floor,
                M_Bill_Other = main.Bill_Other,

                // 寄卡地址
                M_SendCard_AddressType = main.SendCardAddressType,
                M_SendCard_ZipCode = main.SendCard_ZipCode,
                M_SendCard_City = main.SendCard_City,
                M_SendCard_District = main.SendCard_District,
                M_SendCard_Road = main.SendCard_Road,
                M_SendCard_Lane = main.SendCard_Lane,
                M_SendCard_Alley = main.SendCard_Alley,
                M_SendCard_Number = main.SendCard_Number,
                M_SendCard_SubNumber = main.SendCard_SubNumber,
                M_SendCard_Floor = main.SendCard_Floor,
                M_SendCard_Other = main.SendCard_Other,

                // 聯絡資料
                M_Mobile = main.Mobile,
                M_EMail = main.EMail,
                M_HouseRegPhone = main.HouseRegPhone,
                M_LivePhone = main.LivePhone,

                // 教育程度
                M_Education = main.Education,
                M_GraduatedElementarySchool = main.GraduatedElementarySchool,

                // 職業資料
                M_AMLProfessionCode = main.AMLProfessionCode,
                M_AMLProfessionOther = main.AMLProfessionOther,
                M_AMLJobLevelCode = main.AMLJobLevelCode,
                M_CompName = main.CompName,
                M_CompPhone = main.CompPhone,
                M_CompID = main.CompID,
                M_CompJobTitle = main.CompJobTitle,
                M_CompSeniority = main.CompSeniority,

                // 公司地址
                M_Comp_ZipCode = main.Comp_ZipCode,
                M_Comp_City = main.Comp_City,
                M_Comp_District = main.Comp_District,
                M_Comp_Road = main.Comp_Road,
                M_Comp_Lane = main.Comp_Lane,
                M_Comp_Alley = main.Comp_Alley,
                M_Comp_Number = main.Comp_Number,
                M_Comp_SubNumber = main.Comp_SubNumber,
                M_Comp_Floor = main.Comp_Floor,
                M_Comp_Other = main.Comp_Other,

                // 收入資料
                M_CurrentMonthIncome = main.CurrentMonthIncome,
                M_MainIncomeAndFundCodes = main.MainIncomeAndFundCodes,
                M_MainIncomeAndFundOther = main.MainIncomeAndFundOther,

                // 推廣資料
                PromotionUnit = main.PromotionUnit,
                PromotionUser = main.PromotionUser,

                // 同意條款
                M_IsAgreeDataOpen = main.IsAgreeDataOpen,
                IsAgreeMarketing = main.IsAgreeMarketing,
                M_IsAcceptEasyCardDefaultBonus = main.IsAcceptEasyCardDefaultBonus,

                // 帳單與扣款設定
                BillType = main.BillType,
                LiveOwner = main.LiveOwner,
                AnliNo = main.AnliNo,
                FirstBrushingGiftCode = main.FirstBrushingGiftCode,
                ProjectCode = main.ProjectCode,
                // 卡片資訊陣列
                CardInfo = handles
                    .Select(h => new CardInfo
                    {
                        ID = h.ID,
                        UserType = h.UserType,
                        CardStatus = new Faker().PickRandom(
                            CardStatus.紙本件_初始,
                            CardStatus.紙本件_一次件檔中,
                            CardStatus.紙本件_二次件檔中,
                            CardStatus.紙本件_建檔審核中
                        ),
                        ApplyCardType = h.ApplyCardType,
                        ApplyCardKind = h.ApplyCardKind.Value,
                    })
                    .ToList(),

                // 申請流程陣列
                ApplyProcess = new List<ApplyProcess> { process },
            };

            return syncRequest;
        }
    }
}
