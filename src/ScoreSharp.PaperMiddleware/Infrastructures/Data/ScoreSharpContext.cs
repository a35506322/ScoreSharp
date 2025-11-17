using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ScoreSharp.PaperMiddleware.Infrastructures.Data.Entities;

namespace ScoreSharp.PaperMiddleware.Infrastructures.Data;

public partial class ScoreSharpContext : DbContext
{
    public ScoreSharpContext(DbContextOptions<ScoreSharpContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ReviewerPedding_PaperApplyCardCheckJob> ReviewerPedding_PaperApplyCardCheckJob { get; set; }

    public virtual DbSet<Reviewer_ApplyCreditCardInfoFile> Reviewer_ApplyCreditCardInfoFile { get; set; }

    public virtual DbSet<Reviewer_ApplyCreditCardInfoHandle> Reviewer_ApplyCreditCardInfoHandle { get; set; }

    public virtual DbSet<Reviewer_ApplyCreditCardInfoMain> Reviewer_ApplyCreditCardInfoMain { get; set; }

    public virtual DbSet<Reviewer_ApplyCreditCardInfoProcess> Reviewer_ApplyCreditCardInfoProcess { get; set; }

    public virtual DbSet<Reviewer_ApplyCreditCardInfoSupplementary> Reviewer_ApplyCreditCardInfoSupplementary { get; set; }

    public virtual DbSet<Reviewer_ApplyNote> Reviewer_ApplyNote { get; set; }

    public virtual DbSet<Reviewer_BankTrace> Reviewer_BankTrace { get; set; }

    public virtual DbSet<Reviewer_CardRecord> Reviewer_CardRecord { get; set; }

    public virtual DbSet<Reviewer_FinanceCheckInfo> Reviewer_FinanceCheckInfo { get; set; }

    public virtual DbSet<Reviewer_InternalCommunicate> Reviewer_InternalCommunicate { get; set; }

    public virtual DbSet<Reviewer_OutsideBankInfo> Reviewer_OutsideBankInfo { get; set; }

    public virtual DbSet<SetUp_AMLProfession> SetUp_AMLProfession { get; set; }

    public virtual DbSet<SetUp_AddressInfo> SetUp_AddressInfo { get; set; }

    public virtual DbSet<SetUp_CardPromotion> SetUp_CardPromotion { get; set; }

    public virtual DbSet<SysParamManage_SysParam> SysParamManage_SysParam { get; set; }

    public virtual DbSet<System_ErrorLog> System_ErrorLog { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReviewerPedding_PaperApplyCardCheckJob>(entity =>
        {
            entity.HasKey(e => e.ApplyNo);

            entity.ToTable(tb => tb.HasComment("徵審待辦_紙本申請信用卡"));

            entity.Property(e => e.ApplyNo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasComment("申請書編號\r\nYYYYMMDD+1-2碼數字為信用卡+4碼流水號");
            entity.Property(e => e.AddTime)
                .HasComment("創建時間")
                .HasColumnType("datetime");
            entity.Property(e => e.Check929LastTime)
                .HasComment("查敦陽姓名檢核最後時間")
                .HasColumnType("datetime");
            entity.Property(e => e.CheckFocusLastTime)
                .HasComment("發查關注名單最後時間")
                .HasColumnType("datetime");
            entity.Property(e => e.CheckInternalEmailLastTime)
                .HasComment("發查行內Email重複最後時間")
                .HasColumnType("datetime");
            entity.Property(e => e.CheckInternalMobileLastTime)
                .HasComment("發查行內手機重複最後時間")
                .HasColumnType("datetime");
            entity.Property(e => e.CheckNameLastTime)
                .HasComment("查敦陽姓名檢核最後時間")
                .HasColumnType("datetime");
            entity.Property(e => e.CheckRepeatApplyLastTime)
                .HasComment("檢查重覆進件最後時間")
                .HasColumnType("datetime");
            entity.Property(e => e.CheckShortTimeIDLastTime)
                .HasComment("檢查短時間ID相同最後時間")
                .HasColumnType("datetime");
            entity.Property(e => e.ErrorCount).HasComment("錯誤次數\r\n預設 0 \r\n上述檢核有錯誤記一次\r\n如果成功則變為0");
            entity.Property(e => e.IsCheck929).HasComment("是否發查929\r\n需檢核_未完成 = 1\r\n需檢核_成功 = 2\r\n需檢核_失敗 = 3\r\n不需檢核 = 4");
            entity.Property(e => e.IsCheckFocus).HasComment("是否發查關注名單\r\n需檢核_未完成 = 1\r\n需檢核_成功 = 2\r\n需檢核_失敗 = 3\r\n不需檢核 = 4");
            entity.Property(e => e.IsCheckInternalEmail).HasComment("是否發查行內Email重複\r\n需檢核_未完成 = 1\r\n需檢核_成功 = 2\r\n需檢核_失敗 = 3\r\n不需檢核 = 4");
            entity.Property(e => e.IsCheckInternalMobile).HasComment("是否發查行內手機重複\r\n需檢核_未完成 = 1\r\n需檢核_成功 = 2\r\n需檢核_失敗 = 3\r\n不需檢核 = 4");
            entity.Property(e => e.IsCheckName).HasComment("是否發查敦陽姓名檢核\r\n需檢核_未完成 = 1\r\n需檢核_成功 = 2\r\n需檢核_失敗 = 3\r\n不需檢核 = 4");
            entity.Property(e => e.IsCheckRepeatApply).HasComment("是否檢查重覆進件");
            entity.Property(e => e.IsCheckShortTimeID).HasComment("是否檢查短時間ID相同\r\n需檢核_未完成 = 1\r\n需檢核_成功 = 2\r\n需檢核_失敗 = 3\r\n不需檢核 = 4");
            entity.Property(e => e.IsChecked).HasComment("是否檢驗完畢\r\n完成 = 1\r\n未完成 = 2");
            entity.Property(e => e.IsQueryBranchInfo).HasComment("是否查詢分行資訊\r\n需檢核_未完成 = 1\r\n需檢核_成功 = 2\r\n需檢核_失敗 = 3\r\n不需檢核 = 4");
            entity.Property(e => e.IsQueryOriginalCardholderData).HasComment("是否查詢原持卡人\r\n需檢核_未完成 = 1\r\n需檢核_成功 = 2\r\n需檢核_失敗 = 3\r\n不需檢核 = 4");
            entity.Property(e => e.QueryBranchInfoLastTime)
                .HasComment("查詢分行資訊最後時間")
                .HasColumnType("datetime");
            entity.Property(e => e.QueryOriginalCardholderDataLastTime)
                .HasComment("查詢原持卡人最後時間")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Reviewer_ApplyCreditCardInfoFile>(entity =>
        {
            entity.HasKey(e => e.SeqNo);

            entity.HasIndex(e => e.ApplyNo, "NonClusteredIndex-ApplyNo");

            entity.Property(e => e.SeqNo).HasComment("PK");
            entity.Property(e => e.AddTime)
                .HasComment("新增時間")
                .HasColumnType("datetime");
            entity.Property(e => e.AddUserId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("新增員工，系統執行為SYSTEM");
            entity.Property(e => e.ApplyNo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasComment("申請書編號");
            entity.Property(e => e.DBName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("資料庫檔案名稱");
            entity.Property(e => e.FileId).HasComment("檔案 GUID\r\n\r\n關聯Reviewer_ApplyFile");
            entity.Property(e => e.IsHistory)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N")
                .HasComment("Y/N，預設N");
            entity.Property(e => e.Note)
                .HasMaxLength(100)
                .HasComment("備註");
            entity.Property(e => e.Page).HasComment("頁碼\r\n可以與 Reviewer_ApplyFileLog 與對應");
            entity.Property(e => e.Process)
                .HasMaxLength(50)
                .HasComment("狀態或者動作");
        });

        modelBuilder.Entity<Reviewer_ApplyCreditCardInfoHandle>(entity =>
        {
            entity.HasKey(e => e.SeqNo);

            entity.HasIndex(e => e.ApplyNo, "NonClusteredIndex-ApplyNo");

            entity.HasIndex(e => new { e.ID, e.CardStatus }, "NonClusteredIndex-ID_CardStatus");

            entity.Property(e => e.SeqNo)
                .HasMaxLength(26)
                .IsUnicode(false)
                .HasComment("PK\r\nULID");
            entity.Property(e => e.ApplyCardKind).HasComment("申請卡種 ( 1: 實體, 2: 數位, 3: 實體+數位)");
            entity.Property(e => e.ApplyCardType)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("申請卡別：以'/'串接，如JA00/JC00，關聯　SetUp_Card");
            entity.Property(e => e.ApplyNo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasComment("申請書編號\r\n");
            entity.Property(e => e.ApproveTime)
                .HasComment("核准時間\r\n\r\n- 執行核退撤時間")
                .HasColumnType("datetime");
            entity.Property(e => e.ApproveUserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("核准人員員編\r\n\r\n- 案件最終的核准人員，如案件為系統核准，核准人員為SYSTEM\r\n- 核件、退件、撤件最後押上\r\n- 核准人員在多張卡片會不同\r\n");
            entity.Property(e => e.BatchRejectionStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("排程_退件報表狀態\r\nY (完成)、N (等待)\r\nNull 代表未要執行 ");
            entity.Property(e => e.BatchRejectiontTime)
                .HasComment("排程_退件報表時間")
                .HasColumnType("datetime");
            entity.Property(e => e.BatchSupplementStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("排程_補件報表狀態\r\n\r\n- Y (完成)、N (等待)\r\n- Null 代表未要執行 \r\n");
            entity.Property(e => e.BatchSupplementTime)
                .HasComment("排程_補件報表時間")
                .HasColumnType("datetime");
            entity.Property(e => e.CardLimit).HasComment("核卡額度");
            entity.Property(e => e.CardPromotionCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasComment("優惠辦法代碼");
            entity.Property(e => e.CardStatus).HasComment("卡片狀態，查看附件-卡片狀態碼");
            entity.Property(e => e.CardStep).HasComment("卡片階段\r\n1.月收入確認\r\n2.人工徵審");
            entity.Property(e => e.CaseChangeAction).HasComment("案件異動動作\r\n權限內\r\n 1.補件\r\n 2.撤件\r\n 3.退件\r\n 4.核卡 (人工徵信中才有)\r\n權限外 (人工徵信中才有)\r\n 5.排入補件\r\n 6.排入撤件\r\n 7.排入退件\r\n 8.排入核卡\r\n");
            entity.Property(e => e.CreditCheckCode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasComment("徵信代碼\r\n1. 當前最新徵信代碼\r\n2. 關聯 SetUp_CreditCheckCode\r\n3. 提供進行月收入確認時使用，是否僅能在月收入確認");
            entity.Property(e => e.CreditLimit_RatingAdvice).HasComment("評分結果\r\n\r\n- 授信政策科\r\n- 舊系統有*不用這個\r\n");
            entity.Property(e => e.HandleNote)
                .HasMaxLength(100)
                .HasComment("處理備註");
            entity.Property(e => e.ID)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasComment("身分證字號");
            entity.Property(e => e.IsForceCard)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("是否強制發卡\r\nY / N");
            entity.Property(e => e.IsOriginCardholderSameCardLimit)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("建議額度與原持卡人額度相同\r\nY / N");
            entity.Property(e => e.IsPrintSMSAndPaper)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("是否列印簡訊、紙本通知函，Y｜N");
            entity.Property(e => e.MonthlyIncomeCheckUserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("月收入確認人員\r\n\r\n- 完成月收入確認時更改,沒有點完全月收入確認就不用\r\n- 完成月收入確認人員皆相同");
            entity.Property(e => e.MonthlyIncomeTime)
                .HasComment("月收入確認時間")
                .HasColumnType("datetime");
            entity.Property(e => e.NuclearCardNote)
                .HasMaxLength(100)
                .HasComment("核卡註記");
            entity.Property(e => e.OriginCardholderJCICNotes)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("原持卡人JCIC補充註記\r\n\r\n- 現由文彬依聯徵查回結果提供\r\n- Y 、N");
            entity.Property(e => e.OtherRejectionReason)
                .HasMaxLength(100)
                .HasComment("其他退件原因");
            entity.Property(e => e.OtherSupplementReason)
                .HasMaxLength(100)
                .HasComment("其他補件原因");
            entity.Property(e => e.RejectionNote)
                .HasMaxLength(100)
                .HasComment("退件註記");
            entity.Property(e => e.RejectionReasonCode)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("退件原因代碼\r\n關聯 SetUp_RejectionReason");
            entity.Property(e => e.RejectionSendCardAddr).HasComment("退件寄送地址\r\n1. 帳單地址\r\n2. 戶籍地址\r\n3. 公司地址\r\n4. 居住地址\r\n需檢驗選擇的地址完整性\r\n查看 Reviewer_ApplyCreditCardInfoAddress 完整地址");
            entity.Property(e => e.ReviewerTime)
                .HasComment("審查時間")
                .HasColumnType("datetime");
            entity.Property(e => e.ReviewerUserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("審查人員編號\r\n\r\n- 未完成案件於人工徵審派案的第一手權限內人員\r\n- 調撥案件或派案時更改\r\n- 審查人員皆相同\r\n");
            entity.Property(e => e.SupplementNote)
                .HasMaxLength(100)
                .HasComment("補件註記");
            entity.Property(e => e.SupplementReasonCode)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("補件原因代碼\r\n關聯 SetUp_SupplementReason");
            entity.Property(e => e.SupplementSendCardAddr).HasComment("補件寄送地址\r\n1. 帳單地址\r\n2. 戶籍地址\r\n3. 公司地址\r\n4. 居住地址\r\n需檢驗選擇的地址完整性\r\n查看 Reviewer_ApplyCreditCardInfoAddress 完整地址");
            entity.Property(e => e.UserType).HasComment("使用者類型\r\n1 = 正卡人 \r\n2 =  附卡人");
            entity.Property(e => e.WithdrawalNote)
                .HasMaxLength(100)
                .HasComment("撤件註記");
        });

        modelBuilder.Entity<Reviewer_ApplyCreditCardInfoMain>(entity =>
        {
            entity.HasKey(e => e.ApplyNo);

            entity.HasIndex(e => e.CurrentHandleUserId, "NonClusteredIndex-CurrentHandleUserId");

            entity.Property(e => e.ApplyNo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasComment("申請書編號");
            entity.Property(e => e.AMLJobLevelCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasComment("AML職級別：關聯 SetUp_AMLJobLevel");
            entity.Property(e => e.AMLProfessionCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasComment("AML職業別：關聯  SetUp_AMLProfession");
            entity.Property(e => e.AMLProfessionCode_Version)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasComment("AML職業別版本、關聯SetUp_AMLProfession");
            entity.Property(e => e.AMLProfessionOther)
                .HasMaxLength(50)
                .HasComment("AML職業別_其他");
            entity.Property(e => e.AMLRiskLevel)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasComment("洗防風險等級：由AML-KYC提供風險等級");
            entity.Property(e => e.AcceptType).HasComment("進件方式\r\n\r\n1. 親訪親簽\r\n2. 親訪未見親簽\r\n3. 設攤親簽\r\n4. 設為未見親簽\r\n5. 自來件\r\n6. 電話行銷\r\n");
            entity.Property(e => e.AnliNo)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("安麗直銷商編號");
            entity.Property(e => e.AnnualFeePaymentType)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasComment("年費收取方式\r\n關聯SetUp_AnnualFeeCollectionMethod 年費收取方式設定");
            entity.Property(e => e.ApplyDate)
                .HasComment("申請日期")
                .HasColumnType("datetime");
            entity.Property(e => e.AutoDeductionBankAccount)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("自動扣繳帳戶-銀行存摺帳號");
            entity.Property(e => e.AutoDeductionPayType).HasComment("自動扣繳帳戶-扣款方式 (10,最低 20,全額)");
            entity.Property(e => e.B68UnsecuredCreditAmount).HasComment("B68無擔保掛帳金額");
            entity.Property(e => e.BillAddressType).HasComment("帳單地址類型\r\n\r\n1. 同戶籍地址\r\n2. 同居住地址\r\n3. 同寄卡地址\r\n4. 同公司地址\r\n5. 其他\r\n");
            entity.Property(e => e.BillType).HasComment("帳單形式 (1：電子帳單、2：簡訊帳單、3：紙本帳單、4：LINE帳單\r\n)");
            entity.Property(e => e.Bill_Alley)
                .HasMaxLength(30)
                .HasComment("帳單_弄");
            entity.Property(e => e.Bill_City)
                .HasMaxLength(30)
                .HasComment("帳單_縣市");
            entity.Property(e => e.Bill_District)
                .HasMaxLength(30)
                .HasComment("帳單_區域");
            entity.Property(e => e.Bill_Floor)
                .HasMaxLength(30)
                .HasComment("帳單_樓層");
            entity.Property(e => e.Bill_FullAddr)
                .HasMaxLength(120)
                .HasComment("帳單_完整地址");
            entity.Property(e => e.Bill_Lane)
                .HasMaxLength(30)
                .HasComment("帳單_巷");
            entity.Property(e => e.Bill_Number)
                .HasMaxLength(30)
                .HasComment("帳單_號");
            entity.Property(e => e.Bill_Other)
                .HasMaxLength(120)
                .HasComment("帳單_其他");
            entity.Property(e => e.Bill_Road)
                .HasMaxLength(30)
                .HasComment("帳單_路");
            entity.Property(e => e.Bill_SubNumber)
                .HasMaxLength(30)
                .HasComment("帳單_之號");
            entity.Property(e => e.Bill_ZipCode)
                .HasMaxLength(30)
                .HasComment("帳單_郵遞區號");
            entity.Property(e => e.BirthCitizenshipCode).HasComment("出生地國籍：1. 中華民國 2. 其他");
            entity.Property(e => e.BirthCitizenshipCodeOther)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasComment("出生地其他\r\n出生地 = 其他為必填\r\n需符合徵審系統「國籍設定」。");
            entity.Property(e => e.BirthDay)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasComment("生日：民國格式為 YYYMMDD");
            entity.Property(e => e.BlackListNote)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("黑名單註記\r\n\r\n- 查詢paperless內建資料庫是否有資料相符情形\r\n- Y 、N\r\n- 於月收入確認完進行黑名單查詢\r\n");
            entity.Property(e => e.CHName)
                .HasMaxLength(30)
                .HasComment("中文姓名");
            entity.Property(e => e.CardAppId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("申請書ID\r\nE-CARD提供於[eCard_file].[dbo].[ApplyFile]關聯");
            entity.Property(e => e.CardOwner).HasComment("正附卡\r\n依客人申請帶出，現網路申請僅能申請正卡\r\n\r\n1. 正卡\r\n2. 附卡\r\n3. 正卡+附卡\r\n4. 附卡2\r\n5. 正卡+附卡2\r\n");
            entity.Property(e => e.CaseType).HasComment("案件種類\r\n1. 一般件\r\n2. 急件\r\n3. 緊急製卡");
            entity.Property(e => e.ChildrenCount).HasComment("子女人數");
            entity.Property(e => e.CitizenshipCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasComment("國籍：關聯 SetUp_Citizenship");
            entity.Property(e => e.CompID)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasComment("公司統一編號");
            entity.Property(e => e.CompJobLevel).HasComment("公司職級別\r\n\r\n駕駛人員 = 1\r\n服務生/門市人員 = 2\r\n專業人員 = 3\r\n專業技工 = 4\r\n業務人員 = 5\r\n一般職員 = 6\r\n主管階層 = 7\r\n股東/董事/負責人 = 8\r\n家管/其他 = 9\r\n\r\n");
            entity.Property(e => e.CompJobTitle)
                .HasMaxLength(30)
                .HasComment("公司職稱");
            entity.Property(e => e.CompName)
                .HasMaxLength(30)
                .HasComment("公司名稱");
            entity.Property(e => e.CompPhone)
                .HasMaxLength(21)
                .IsUnicode(false)
                .HasComment("公司電話\r\n範例020-28572463#55555\r\n3碼-七八碼#5碼分機");
            entity.Property(e => e.CompSeniority).HasComment("公司年資");
            entity.Property(e => e.CompTrade).HasComment("公司行業別\r\n\r\n金融業 = 1\r\n公務機關 = 2\r\n營造/製造/運輸業 =3\r\n一般商業 = 4\r\n休閒 / 娛樂 / 服務業 = 5\r\n軍警消防業 = 6\r\n非營利團體 = 7\r\n學生 = 8\r\n自由業_其他 = 9\r\n\r\n");
            entity.Property(e => e.Comp_Alley)
                .HasMaxLength(30)
                .HasComment("公司_弄");
            entity.Property(e => e.Comp_City)
                .HasMaxLength(30)
                .HasComment("公司_縣市");
            entity.Property(e => e.Comp_District)
                .HasMaxLength(30)
                .HasComment("公司_區域");
            entity.Property(e => e.Comp_Floor)
                .HasMaxLength(30)
                .HasComment("公司_樓層");
            entity.Property(e => e.Comp_FullAddr)
                .HasMaxLength(120)
                .HasComment("公司_完整地址");
            entity.Property(e => e.Comp_Lane)
                .HasMaxLength(30)
                .HasComment("公司_巷");
            entity.Property(e => e.Comp_Number)
                .HasMaxLength(30)
                .HasComment("公司_號");
            entity.Property(e => e.Comp_Other)
                .HasMaxLength(120)
                .HasComment("公司_其他");
            entity.Property(e => e.Comp_Road)
                .HasMaxLength(30)
                .HasComment("公司_街道");
            entity.Property(e => e.Comp_SubNumber)
                .HasMaxLength(30)
                .HasComment("公司_之號");
            entity.Property(e => e.Comp_ZipCode)
                .HasMaxLength(30)
                .HasComment("公司_郵遞區號");
            entity.Property(e => e.CurrentHandleUserId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("當前狀態處理經辦\r\n系統派案指定\r\n有此員編才能在個人工作清單查到");
            entity.Property(e => e.CurrentMonthIncome).HasComment("現職月收入(元)");
            entity.Property(e => e.CustomerServiceNotes)
                .HasMaxLength(100)
                .HasComment("客服備註");
            entity.Property(e => e.CustomerSpecialNotes)
                .HasMaxLength(100)
                .HasComment("客戶特殊註記");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(30)
                .HasComment("部門名稱\r\n【商務卡】及【國旅卡】的紙本申請書有此欄位，新徵審系統需增加此欄位，欄位位置規劃中\r\n自行填寫");
            entity.Property(e => e.ECard_AppendixIsException)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("ECard申請書附件是否異常\r\nY 、N\r\n紙本件固定為Null\r\n原卡友會固定N");
            entity.Property(e => e.EMail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("E-MAIL");
            entity.Property(e => e.ENName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("英文名稱");
            entity.Property(e => e.EcardAttachmentNotes).HasComment("Ecard附件註記\r\n1. 附件異常\r\n2.  MYDATA後補");
            entity.Property(e => e.Education).HasComment("教育程度：1. 博士, 2. 碩士, 3. 大學, 4. 專科, 5. 高中職, 6. 其他");
            entity.Property(e => e.ElecCodeId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("電子化約定條款\r\n範例：202007");
            entity.Property(e => e.EmploymentDate)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasComment("到職日期\r\n民國年 1090101");
            entity.Property(e => e.ExpatValidityPeriod)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasComment("外籍人士指定效期 (格式: YYYYMM)");
            entity.Property(e => e.FirstBrushingGiftCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("首刷禮代碼\r\nE-CARD提供\r\n原系統是使用家庭成員6年齡");
            entity.Property(e => e.GraduatedElementarySchool)
                .HasMaxLength(20)
                .HasComment("畢業國小");
            entity.Property(e => e.HouseRegPhone)
                .HasMaxLength(18)
                .IsUnicode(false)
                .HasComment("戶籍電話\r\n範例020-28572463\r\n3碼-七八碼");
            entity.Property(e => e.ID)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasComment("身份證字號");
            entity.Property(e => e.IDCardRenewalLocationCode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasComment("身分證發證地點：關聯 SetUp_IDCardRenewalLocation");
            entity.Property(e => e.IDIssueDate)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasComment("身分證發證日期：民國格式為 YYYMMDD");
            entity.Property(e => e.IDTakeStatus).HasComment("身分證請領狀態：1. 初發, 2. 補發, 3. 換發");
            entity.Property(e => e.IDType).HasComment("身份別\r\n1. 新戶\r\n2. 原持卡人\r\n3. 卡友\r\n4. 存戶\r\n5. 持他行卡\r\n6. 自然人憑證\r\n\r\nE-CARD提供-卡友、存戶、持他行卡、自然人憑證");
            entity.Property(e => e.ISRCAForCurrentPEP)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("當前或曾為PEP身分 (Y/N)，敦陽系統黑名單是否相符= Y 和 姓名檢核 = Y 需填寫");
            entity.Property(e => e.IsAcceptEasyCardDefaultBonus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("是否同意悠遊卡自動加值預設設定 (Y: 是, N: 否)");
            entity.Property(e => e.IsAgreeDataOpen)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("本人是否同意提供資料予聯名認同集團 (Y: 是, N: 否)");
            entity.Property(e => e.IsAgreeMarketing)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("是否同意提供資料供本行進行行銷 (Y: 是, N: 否)");
            entity.Property(e => e.IsApplyAutoDeduction)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("是否申請自動扣款 (Y: 是, N: 否)");
            entity.Property(e => e.IsApplyDigtalCard)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("是否申請數位卡(Y：是、N：否)");
            entity.Property(e => e.IsBranchCustomer)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("是否為分行客戶\r\n\r\n- Y / N");
            entity.Property(e => e.IsConvertCard)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("是否轉換卡別\r\nY：是、N：否\r\n紙本案件，申請書上可幫客人轉換卡別\r\n對應紙本案件欄位正卡_換發別種卡別");
            entity.Property(e => e.IsCurrentPositionRelatedPEPPosition)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("現任職位是否與PEP職位相關 (Y/N)，敦陽系統黑名單是否相符= Y 和 姓名檢核 = Y 需填寫");
            entity.Property(e => e.IsDunyangBlackList)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("敦陽系統黑名單是否相符（Y：是、N：否）\r\n由行員確認");
            entity.Property(e => e.IsFATCAIdentity)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("是否FATCA身份 (Y/N)，當國籍 = 美國時候預設為 Y");
            entity.Property(e => e.IsForeverResidencePermit)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("是否為永久居留證\r\n1. 配合法遵部政策，新增欄位。\r\n2. 欄位用拉霸方式選擇，Y/N，預設為N。\r\n3. 如為{N}，則須鍵入外籍人士指定效期。\r\n");
            entity.Property(e => e.IsHistory)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N")
                .HasComment("Y/N，預設N");
            entity.Property(e => e.IsHoldingBankCard)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("是否持有本行卡\r\nY 、N");
            entity.Property(e => e.IsKYCChange)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("是否變更KYC，Y｜N");
            entity.Property(e => e.IsOriginalCardholder)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("是否原持卡人\r\n- Y:是/N:否\r\n- 查詢 MW3 原持卡人如有資料則為Y則為原持卡人");
            entity.Property(e => e.IsPayNoticeBind)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("是否綁定消費通知 (Y: 是, N: 否)");
            entity.Property(e => e.IsRepeatApply)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("是否重複申請件\r\n- Y:是/N:否\r\n- 查詢 MW3 原持卡人如有資料則為Y且徵審系統有資料則為重複進件\r\n");
            entity.Property(e => e.IsStudent)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("是否學生身份 (Y: 是, N: 否)，經審查，有可能從學生改成非學生，也有可能從非學生改成學生。");
            entity.Property(e => e.LastUpdateTime)
                .HasComment("最後處理時間")
                .HasColumnType("datetime");
            entity.Property(e => e.LastUpdateUserId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("最後修改資料人員");
            entity.Property(e => e.LineBankUUID)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasComment("LineBankUUID");
            entity.Property(e => e.LiveAddressType).HasComment("居住地址類型，主要用於前端顯示\r\n\r\n1. 同戶籍地址\r\n2. 同帳單地址\r\n3. 同寄卡地址\r\n4. 同公司地址\r\n5. 其他\r\n");
            entity.Property(e => e.LiveOwner).HasComment("居住地所有權人：1. 本人, 2. 配偶, 3. 父母, 4. 子女, 5. 親戚, 6. 朋友, 7. 其他");
            entity.Property(e => e.LivePhone)
                .HasMaxLength(18)
                .IsUnicode(false)
                .HasComment("居住電話\r\n範例020-28572463\r\n3碼-七八碼");
            entity.Property(e => e.LiveYear).HasComment("居住年數");
            entity.Property(e => e.Live_Alley)
                .HasMaxLength(30)
                .HasComment("居住_弄");
            entity.Property(e => e.Live_City)
                .HasMaxLength(30)
                .HasComment("居住_縣市");
            entity.Property(e => e.Live_District)
                .HasMaxLength(30)
                .HasComment("居住_區域");
            entity.Property(e => e.Live_Floor)
                .HasMaxLength(30)
                .HasComment("居住_樓層");
            entity.Property(e => e.Live_FullAddr)
                .HasMaxLength(120)
                .HasComment("居住_完整地址");
            entity.Property(e => e.Live_Lane)
                .HasMaxLength(30)
                .HasComment("居住_巷");
            entity.Property(e => e.Live_Number)
                .HasMaxLength(30)
                .HasComment("居住_號");
            entity.Property(e => e.Live_Other)
                .HasMaxLength(120)
                .HasComment("居住_其他");
            entity.Property(e => e.Live_Road)
                .HasMaxLength(30)
                .HasComment("居住_街道");
            entity.Property(e => e.Live_SubNumber)
                .HasMaxLength(30)
                .HasComment("居住_之號");
            entity.Property(e => e.Live_ZipCode)
                .HasMaxLength(30)
                .HasComment("居住_郵遞區號");
            entity.Property(e => e.LongTerm)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("長循分期戶\r\n\r\nY 、N");
            entity.Property(e => e.MainIncomeAndFundCodes)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("所得及資金來源 (請參考Setup_MainIncomeAndFund表格、需符合Paperless「主要所得及資金來源勾選」，多選選項以逗號(,)區隔)");
            entity.Property(e => e.MainIncomeAndFundOther)
                .HasMaxLength(30)
                .HasComment("主要收入_其他");
            entity.Property(e => e.MarriageState).HasComment("婚姻狀況：1. 已婚, 2. 未婚, 3. 其他");
            entity.Property(e => e.Mobile)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("行動電話");
            entity.Property(e => e.MyDataCaseNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("MyData案件編號\r\n當附件註記為2：MYDATA後補時，本欄位必定有值\r\nE-CARD提供");
            entity.Property(e => e.NameChecked)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("姓名檢核\r\n\r\n- 敦陽系統之姓名檢核\r\n- Y 、N");
            entity.Property(e => e.NameCheckedReasonCodes)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("姓名檢核理由代碼，可為複數以「,」分割，敦陽系統黑名單是否相符= Y 和 姓名檢核 = Y 要有理由碼\r\n1. PEP   \r\n2. 要再問\r\n3. 黑名單\r\n4. 負面新聞\r\n5. 無\r\n6. RCA\r\n7. 國內PEP\r\n8. 國外PEP\r\n9. 國際組織PEP\r\n10. 卸任PEP\r\n");
            entity.Property(e => e.OTPMobile)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("OTP手機號碼");
            entity.Property(e => e.OTPTime)
                .HasComment("OTP時間")
                .HasColumnType("datetime");
            entity.Property(e => e.OldCertificateVerified)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("舊照查驗\r\nY：是、N：否");
            entity.Property(e => e.PEPRange).HasComment("擔任PEP範圍:\r\n不滿2年\r\n滿兩年但不滿四年\r\n滿4年 / 敦陽系統黑名單是否相符= Y 和 姓名檢核 = Y 需填寫");
            entity.Property(e => e.ParentLiveAddressType).HasComment("家長地址類型\r\n\r\n1. 同正卡戶籍地址\r\n2. 同正卡居住地址\r\n3. 其他\r\n");
            entity.Property(e => e.ParentLive_Alley)
                .HasMaxLength(30)
                .HasComment("家長_弄");
            entity.Property(e => e.ParentLive_City)
                .HasMaxLength(30)
                .HasComment("家長_縣市");
            entity.Property(e => e.ParentLive_District)
                .HasMaxLength(30)
                .HasComment("家長_區域");
            entity.Property(e => e.ParentLive_Floor)
                .HasMaxLength(30)
                .HasComment("家長_樓層");
            entity.Property(e => e.ParentLive_FullAddr)
                .HasMaxLength(120)
                .HasComment("家長_完整地址");
            entity.Property(e => e.ParentLive_Lane)
                .HasMaxLength(30)
                .HasComment("家長_巷");
            entity.Property(e => e.ParentLive_Number)
                .HasMaxLength(30)
                .HasComment("家長_號");
            entity.Property(e => e.ParentLive_Other)
                .HasMaxLength(120)
                .HasComment("家長_其他");
            entity.Property(e => e.ParentLive_Road)
                .HasMaxLength(30)
                .HasComment("家長_路");
            entity.Property(e => e.ParentLive_SubNumber)
                .HasMaxLength(30)
                .HasComment("家長_之號");
            entity.Property(e => e.ParentLive_ZipCode)
                .HasMaxLength(30)
                .HasComment("家長_郵遞區號");
            entity.Property(e => e.ParentName)
                .HasMaxLength(30)
                .HasComment("家長姓名");
            entity.Property(e => e.ParentPhone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasComment("家長電話\r\n可以行動電話或者家電或公司電話\r\n家電範例020-28572463，3碼-七八碼\r\n公司範例020-28572463#55555，3碼-七八碼#5碼分機\r\n手機範例0978822811");
            entity.Property(e => e.PassportDate)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasComment("護照日期 (格式: YYYYMMDD)");
            entity.Property(e => e.PassportNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasComment("護照號碼");
            entity.Property(e => e.PreviousHandleUserId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("前手經辦\r\n\r\n- 未分案清單+調撥案件時指定\r\n- 用於退回申請時指定人員\r\n");
            entity.Property(e => e.ProjectCode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasComment("專案代號\r\n\r\n- E-CARD提供\r\n- 得知由文彬組使用，只要來源端提供照存即可\r\n");
            entity.Property(e => e.PromotionUnit)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("推廣單位");
            entity.Property(e => e.PromotionUser)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("推廣人員");
            entity.Property(e => e.Reg_Alley)
                .HasMaxLength(30)
                .HasComment("戶籍_弄");
            entity.Property(e => e.Reg_City)
                .HasMaxLength(30)
                .HasComment("戶籍_縣市");
            entity.Property(e => e.Reg_District)
                .HasMaxLength(30)
                .HasComment("戶籍_區域");
            entity.Property(e => e.Reg_Floor)
                .HasMaxLength(30)
                .HasComment("戶籍_樓層");
            entity.Property(e => e.Reg_FullAddr)
                .HasMaxLength(120)
                .HasComment("戶籍_完整地址");
            entity.Property(e => e.Reg_Lane)
                .HasMaxLength(30)
                .HasComment("戶籍_巷");
            entity.Property(e => e.Reg_Number)
                .HasMaxLength(30)
                .HasComment("戶籍_號");
            entity.Property(e => e.Reg_Other)
                .HasMaxLength(120)
                .HasComment("戶籍_其他");
            entity.Property(e => e.Reg_Road)
                .HasMaxLength(30)
                .HasComment("戶籍_路");
            entity.Property(e => e.Reg_SubNumber)
                .HasMaxLength(30)
                .HasComment("戶籍_之號");
            entity.Property(e => e.Reg_ZipCode)
                .HasMaxLength(30)
                .HasComment("戶籍_郵遞區號");
            entity.Property(e => e.ResidencePermitBackendNum)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("居留證背面號碼：前兩碼大寫英文 + 8 碼數字，範例ＹＺ80000001");
            entity.Property(e => e.ResidencePermitDeadline)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasComment("居留證期限：格式 YYYYMMDD");
            entity.Property(e => e.ResidencePermitIssueDate)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasComment("居留證發證日期 (格式: YYYYMMDD)");
            entity.Property(e => e.ResignPEPKind).HasComment("卸任PEP種類：1. 國內 2.國外 3.國際組織 / 敦陽系統黑名單是否相符= Y 和 姓名檢核 = Y 需填寫");
            entity.Property(e => e.SendCardAddressType).HasComment("寄卡地址類型，主要用於前端\r\n\r\n1. 同戶籍地址\r\n2. 同居住地址\r\n3. 同帳單地址\r\n4. 同公司地址\r\n5. 親領\r\n6. 其他\r\n");
            entity.Property(e => e.SendCard_Alley)
                .HasMaxLength(30)
                .HasComment("寄卡_弄");
            entity.Property(e => e.SendCard_City)
                .HasMaxLength(30)
                .HasComment("寄卡_縣市");
            entity.Property(e => e.SendCard_District)
                .HasMaxLength(30)
                .HasComment("寄卡_區域");
            entity.Property(e => e.SendCard_Floor)
                .HasMaxLength(30)
                .HasComment("寄卡_樓層");
            entity.Property(e => e.SendCard_FullAddr)
                .HasMaxLength(120)
                .HasComment("寄卡_完整地址");
            entity.Property(e => e.SendCard_Lane)
                .HasMaxLength(30)
                .HasComment("寄卡_巷");
            entity.Property(e => e.SendCard_Number)
                .HasMaxLength(30)
                .HasComment("寄卡_號");
            entity.Property(e => e.SendCard_Other)
                .HasMaxLength(120)
                .HasComment("寄卡_其他");
            entity.Property(e => e.SendCard_Road)
                .HasMaxLength(30)
                .HasComment("寄卡_街道");
            entity.Property(e => e.SendCard_SubNumber)
                .HasMaxLength(30)
                .HasComment("寄卡_之號");
            entity.Property(e => e.SendCard_ZipCode)
                .HasMaxLength(30)
                .HasComment("寄卡_郵遞區號");
            entity.Property(e => e.Sex).HasComment("性別：1.男生 2. 女生");
            entity.Property(e => e.SocialSecurityCode)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("社會安全號碼，FATCA身份=Y，徵審人員會去跟客人要此值");
            entity.Property(e => e.Source).HasComment("來源\r\nE-CARD提供Ecard、APP\r\n\r\n1. ECARD\r\n2. APP\r\n3. 紙本");
            entity.Property(e => e.StudScheduledGraduationDate)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasComment("學生預定畢業日期 (IsStudent = Y 才有值, 格式: YYYMMDD)");
            entity.Property(e => e.StudSchool)
                .HasMaxLength(30)
                .HasComment("學生就讀學校 (IsStudent = Y 才有值)");
            entity.Property(e => e.StudentApplicantRelationship).HasComment("學生申請人與本人關係 (1: 父母, 2: 學生)");
            entity.Property(e => e.UserSourceIP)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasComment("使用者來源IP位置");
            entity.Property(e => e.UserType).HasComment("固定 1 = 正卡人");
        });

        modelBuilder.Entity<Reviewer_ApplyCreditCardInfoProcess>(entity =>
        {
            entity.HasKey(e => e.SeqNo);

            entity.HasIndex(e => e.ApplyNo, "NonClusteredIndex-ApplyNo");

            entity.Property(e => e.SeqNo).HasComment("PK");
            entity.Property(e => e.ApplyNo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasComment("申請書編號");
            entity.Property(e => e.EndTime)
                .HasComment("結束時間")
                .HasColumnType("datetime");
            entity.Property(e => e.Notes)
                .HasMaxLength(500)
                .HasComment("備註");
            entity.Property(e => e.Process)
                .HasMaxLength(100)
                .HasComment("案件狀態跟執行動作");
            entity.Property(e => e.ProcessUserId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("處理人員\r\n系統人員 =  SYSTEM");
            entity.Property(e => e.StartTime)
                .HasComment("開始時間")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Reviewer_ApplyCreditCardInfoSupplementary>(entity =>
        {
            entity.HasKey(e => new { e.ApplyNo, e.ID }).HasName("PK_Reviewer_ApplyCreditCardInfoSupplementary_1");

            entity.HasIndex(e => e.ApplyNo, "NonClusteredIndex-ApplyNo");

            entity.Property(e => e.ApplyNo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasComment("申請書編號\r\nPK");
            entity.Property(e => e.ID)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasComment("身分證字號\r\nPK");
            entity.Property(e => e.ApplicantRelationship).HasComment("與正卡人關係\r\n1. 配偶\r\n2. 父母\r\n3. 子女\r\n4. 兄弟姊妹\r\n7. 配偶父母");
            entity.Property(e => e.BirthCitizenshipCode).HasComment("出生地國籍\r\n1. 中華民國\r\n2. 其他");
            entity.Property(e => e.BirthCitizenshipCodeOther)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasComment("出生地其他\r\nBirthCitizenshipCode = 2(其他)為必填\r\n關聯 SetUp_Citizenship");
            entity.Property(e => e.BirthDay)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasComment("出生年月日\r\n目前系統是請他提供YYYMMDD");
            entity.Property(e => e.CHName)
                .HasMaxLength(30)
                .HasComment("中文姓名");
            entity.Property(e => e.CitizenshipCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasComment("國籍\r\n由徵審系統提供值給E-CARD\r\n關聯 SetUp_Citizenship");
            entity.Property(e => e.CompJobTitle)
                .HasMaxLength(30)
                .HasComment("職稱");
            entity.Property(e => e.CompName)
                .HasMaxLength(30)
                .HasComment("公司名稱");
            entity.Property(e => e.CompPhone)
                .HasMaxLength(21)
                .IsUnicode(false)
                .HasComment("公司電話\r\n範例020-28572463#55555，3碼-七八碼#多碼分機");
            entity.Property(e => e.ENName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("英文姓名");
            entity.Property(e => e.ExpatValidityPeriod)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasComment("外籍人士指定效期\r\nYYYYMM");
            entity.Property(e => e.IDCardRenewalLocationCode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasComment("身分證發證地點\r\n關聯 SetUp_IDCardRenewalLocation");
            entity.Property(e => e.IDIssueDate)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasComment("身分證發證日期\r\n民國YYYMMDD");
            entity.Property(e => e.IDTakeStatus).HasComment("身分證請領狀態\r\n1.初發\r\n2.補發\r\n3.換發");
            entity.Property(e => e.ISRCAForCurrentPEP)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("當前或曾為PEP身分 (Y/N)，敦陽系統黑名單是否相符= Y 和 姓名檢核 = Y 需填寫");
            entity.Property(e => e.IsCurrentPositionRelatedPEPPosition)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("現任職位是否與PEP職位相關 (Y/N)，敦陽系統黑名單是否相符= Y 和 姓名檢核 = Y 需填寫");
            entity.Property(e => e.IsDunyangBlackList)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("敦陽系統黑名單是否相符 \r\nY：是、N：否\r\n由行員確認");
            entity.Property(e => e.IsFATCAIdentity)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("是否FATCA身份\r\nY/N");
            entity.Property(e => e.IsForeverResidencePermit)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("是否為永久居留證\r\n配合法遵部政策，新增欄位。\r\n欄位用拉霸方式選擇，Y/N，預設為N。\r\n如為{N}，則須鍵入外籍人士指定效期。\r\n");
            entity.Property(e => e.IsOriginalCardholder)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("是否原持卡人\r\n- Y:是/N:否\r\n- 查詢 MW3 原持卡人如有資料則為Y則為原持卡人");
            entity.Property(e => e.IsRepeatApply)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("是否重複申請件\r\n- Y:是/N:否\r\n- 查詢 MW3 原持卡人如有資料則為Y且徵審系統有資料則為重複進件\r\n");
            entity.Property(e => e.LivePhone)
                .HasMaxLength(18)
                .IsUnicode(false)
                .HasComment("居住電話\r\n範例020-28572463\r\n3碼-七八碼");
            entity.Property(e => e.Live_Alley)
                .HasMaxLength(30)
                .HasComment("居住_弄");
            entity.Property(e => e.Live_City)
                .HasMaxLength(30)
                .HasComment("居住_縣市");
            entity.Property(e => e.Live_District)
                .HasMaxLength(30)
                .HasComment("居住_區域");
            entity.Property(e => e.Live_Floor)
                .HasMaxLength(30)
                .HasComment("居住_樓層");
            entity.Property(e => e.Live_FullAddr)
                .HasMaxLength(120)
                .HasComment("居住_完整地址");
            entity.Property(e => e.Live_Lane)
                .HasMaxLength(30)
                .HasComment("居住_巷");
            entity.Property(e => e.Live_Number)
                .HasMaxLength(30)
                .HasComment("居住_號");
            entity.Property(e => e.Live_Other)
                .HasMaxLength(120)
                .HasComment("居住_其他");
            entity.Property(e => e.Live_Road)
                .HasMaxLength(30)
                .HasComment("居住_街道");
            entity.Property(e => e.Live_SubNumber)
                .HasMaxLength(30)
                .HasComment("居住_之號");
            entity.Property(e => e.Live_ZipCode)
                .HasMaxLength(30)
                .HasComment("居住_郵遞區號");
            entity.Property(e => e.MarriageState).HasComment("婚姻狀況\r\n1.已婚\r\n2.未婚\r\n3.其他");
            entity.Property(e => e.Mobile)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("行動電話");
            entity.Property(e => e.NameChecked)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("姓名檢核\r\n\r\n- 敦陽系統之姓名檢核\r\n- Y 、N");
            entity.Property(e => e.NameCheckedReasonCodes)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("姓名檢核理由代碼\r\n可為複數以「,」分割，姓名檢核 = Y 要有理由碼\r\n1. PEP\r\n2. 要再問\r\n3. 黑名單\r\n4. 負面新聞\r\n5. 無\r\n6. RCA\r\n7. 國內PEP\r\n8. 國外PEP\r\n9. 國際組織PEP\r\n10. 卸任PEP");
            entity.Property(e => e.OldCertificateVerified)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("舊照查驗\r\nY：是、N：否");
            entity.Property(e => e.PEPRange).HasComment("擔任PEP範圍:\r\n不滿2年\r\n滿兩年但不滿四年\r\n滿4年 / 敦陽系統黑名單是否相符= Y 和 姓名檢核 = Y 需填寫");
            entity.Property(e => e.PassportDate)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasComment("護照日期\r\nYYYYMMDD");
            entity.Property(e => e.PassportNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasComment("護照號碼");
            entity.Property(e => e.ResidencePermitBackendNum)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("居留證背面號碼\r\n前兩碼大寫英文 + 8 碼數字，範例ＹＺ80000001");
            entity.Property(e => e.ResidencePermitDeadline)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasComment("居留證期限\r\n格式 YYYYMMDD");
            entity.Property(e => e.ResidencePermitIssueDate)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasComment("居留證發證日期 \r\nYYYYMMDD");
            entity.Property(e => e.ResidenceType).HasComment("居住地址類型\r\n同正卡居住地址 = 1\r\n其他 = 2\r\n主要用於前端顯示");
            entity.Property(e => e.ResignPEPKind).HasComment("卸任PEP種類：1. 國內 2.國外 3.國際組織 / 敦陽系統黑名單是否相符= Y 和 姓名檢核 = Y 需填寫");
            entity.Property(e => e.SendCard_Alley)
                .HasMaxLength(30)
                .HasComment("寄卡_弄");
            entity.Property(e => e.SendCard_City)
                .HasMaxLength(30)
                .HasComment("寄卡_縣市");
            entity.Property(e => e.SendCard_District)
                .HasMaxLength(30)
                .HasComment("寄卡_區域");
            entity.Property(e => e.SendCard_Floor)
                .HasMaxLength(30)
                .HasComment("寄卡_樓層");
            entity.Property(e => e.SendCard_FullAddr)
                .HasMaxLength(120)
                .HasComment("寄卡_完整地址");
            entity.Property(e => e.SendCard_Lane)
                .HasMaxLength(30)
                .HasComment("寄卡_巷");
            entity.Property(e => e.SendCard_Number)
                .HasMaxLength(30)
                .HasComment("寄卡_號");
            entity.Property(e => e.SendCard_Other)
                .HasMaxLength(120)
                .HasComment("寄卡_其他");
            entity.Property(e => e.SendCard_Road)
                .HasMaxLength(30)
                .HasComment("寄卡_街道");
            entity.Property(e => e.SendCard_SubNumber)
                .HasMaxLength(30)
                .HasComment("寄卡_之號");
            entity.Property(e => e.SendCard_ZipCode)
                .HasMaxLength(30)
                .HasComment("寄卡_郵遞區號");
            entity.Property(e => e.Sex).HasComment("性別\r\n1 = 男\r\n2 = 女 ");
            entity.Property(e => e.ShippingCardAddressType).HasComment("寄卡地址類型\r\n同正卡寄卡地址 = 1\r\n親領 = 2\r\n主要用於前端顯示");
            entity.Property(e => e.SocialSecurityCode)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("社會安全碼\r\nFATCA身份=Y，徵審人員會去跟客人要此值");
            entity.Property(e => e.UserType).HasComment("使用者類型\r\nPK\r\n固定 2 = 附卡人");
        });

        modelBuilder.Entity<Reviewer_ApplyNote>(entity =>
        {
            entity.HasKey(e => e.SeqNo);

            entity.ToTable(tb => tb.HasComment("申請書備註"));

            entity.Property(e => e.SeqNo).HasComment("PK");
            entity.Property(e => e.ApplyNo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasComment("申請書編號");
            entity.Property(e => e.ID)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasComment("身分證字號");
            entity.Property(e => e.Note)
                .HasComment("備註\r\n用於徵審人員備註資料")
                .HasColumnType("ntext");
            entity.Property(e => e.UpdateTime)
                .HasComment("修正時間")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateUserId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("修正員工");
            entity.Property(e => e.UserType).HasComment("使用者類型\r\n1.正卡人\r\n2.附卡人");
        });

        modelBuilder.Entity<Reviewer_BankTrace>(entity =>
        {
            entity.HasKey(e => new { e.ApplyNo, e.ID, e.UserType }).HasName("PK_Reviewer_DigistCheckInfo");

            entity.ToTable(tb => tb.HasComment("徵審_銀行追蹤"));

            entity.Property(e => e.ApplyNo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasComment("申請書編號，PK");
            entity.Property(e => e.ID)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasComment("身分證字號，PK");
            entity.Property(e => e.UserType).HasComment("使用者類型，PK\r\n1. 正卡人\r\n2. 附卡人\r\n");
            entity.Property(e => e.EqualInternalIP_CheckRecord)
                .HasMaxLength(100)
                .HasComment("確認紀錄\r\n於月收入確認簽核時，當 EqualInternalIP_Flag =Y，需填寫原因");
            entity.Property(e => e.EqualInternalIP_Flag)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("行內IP相同，Y｜N");
            entity.Property(e => e.EqualInternalIP_IsError)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("是否異常，Y｜N");
            entity.Property(e => e.EqualInternalIP_UpdateTime)
                .HasComment("確認時間")
                .HasColumnType("datetime");
            entity.Property(e => e.EqualInternalIP_UpdateUserId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("確認人員");
            entity.Property(e => e.InternalEmailSame_CheckRecord)
                .HasMaxLength(100)
                .HasComment("行內Email相同_確認紀錄\r\nInternalEmailSame_Flag =Y，需填寫原因");
            entity.Property(e => e.InternalEmailSame_Flag)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("行內Email相同\r\nY/N\r\n於進件時檢核，當 InternalEmailSame_Flag =Y，在執行系統審查時會直接進入人工審核，需填寫原因");
            entity.Property(e => e.InternalEmailSame_IsError)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("行內Email相同_是否異常\r\nY/N\r\nInternalEmailSame_Flag =Y，需填寫是否異常");
            entity.Property(e => e.InternalEmailSame_Relation).HasComment("行內Email相同_確認關係\r\n1.  父母\r\n2. 子女\r\n3. 配偶\r\n4. 兄弟姊妹\r\n5. 配偶父母\r\n6. 其他關係\r\n7. 無關係");
            entity.Property(e => e.InternalEmailSame_UpdateTime)
                .HasComment("行內Email相同_確認時間")
                .HasColumnType("datetime");
            entity.Property(e => e.InternalEmailSame_UpdateUserId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("行內Email相同_確認人員");
            entity.Property(e => e.InternalMobileSame_CheckRecord)
                .HasMaxLength(100)
                .HasComment("行內手機相同_確認紀錄\r\nInternalMobileSame_Flag =Y，需填寫原因");
            entity.Property(e => e.InternalMobileSame_Flag)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("行內手機相同\r\nY/N\r\n於進件時檢核，當 InternalMobileSame_Flag =Y，在執行系統審查時會直接進入人工審核，需填寫原因");
            entity.Property(e => e.InternalMobileSame_IsError)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("行內手機相同_是否異常\r\nY/N\r\nInternalMobileSame_Flag =Y，需填寫是否異常");
            entity.Property(e => e.InternalMobileSame_Relation).HasComment("行內手機相同_確認關係\r\n1.  父母\r\n2. 子女\r\n3. 配偶\r\n4. 兄弟姊妹\r\n5. 配偶父母\r\n6. 其他關係\r\n7. 無關係");
            entity.Property(e => e.InternalMobileSame_UpdateTime)
                .HasComment("行內手機相同_確認時間")
                .HasColumnType("datetime");
            entity.Property(e => e.InternalMobileSame_UpdateUserId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("行內手機相同_確認人員");
            entity.Property(e => e.SameEmail_CheckRecord)
                .HasMaxLength(100)
                .HasComment("電子信箱確認紀錄\r\n於月收入確認簽核時，當 SameEmail_Falg =Y，需填寫原因");
            entity.Property(e => e.SameEmail_Flag)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("電子信箱是否相同，Y｜N");
            entity.Property(e => e.SameEmail_IsError)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("電子信箱是否異常，Y｜N");
            entity.Property(e => e.SameEmail_UpdateTime)
                .HasComment("電子信箱確認時間")
                .HasColumnType("datetime");
            entity.Property(e => e.SameEmail_UpdateUserId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("電子信箱確認人員");
            entity.Property(e => e.SameIP_CheckRecord)
                .HasMaxLength(100)
                .HasComment("確認紀錄\r\n於月收入確認簽核時，當 SameIP_Flag =Y，需填寫原因");
            entity.Property(e => e.SameIP_Flag)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("相同IP比對，Y｜N");
            entity.Property(e => e.SameIP_IsError)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("IP是否異常，Y｜N");
            entity.Property(e => e.SameIP_UpdateTime)
                .HasComment("確認時間")
                .HasColumnType("datetime");
            entity.Property(e => e.SameIP_UpdateUserId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("確認人員");
            entity.Property(e => e.SameMobile_CheckRecord)
                .HasMaxLength(100)
                .HasComment("電話確認紀錄\r\n於月收入確認簽核時，當 SameMobile_Flag =Y，需填寫原因");
            entity.Property(e => e.SameMobile_Flag)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("電話是否相同，Y｜N");
            entity.Property(e => e.SameMobile_IsError)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("電話是否異常，Y｜N");
            entity.Property(e => e.SameMobile_UpdateTime)
                .HasComment("電話確認時間")
                .HasColumnType("datetime");
            entity.Property(e => e.SameMobile_UpdateUserId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("電話確認人員");
            entity.Property(e => e.ShortTimeID_CheckRecord)
                .HasMaxLength(100)
                .HasComment("短時間ID頻繁_確認紀錄\r\nShortTimeID_Flag =Y，需填寫原因");
            entity.Property(e => e.ShortTimeID_Flag)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("短時間ID頻繁申請相同\r\nY/N\r\n於進件時檢核，當 ShortTimeID_Flag =Y，在執行系統審查時會直接進入人工審核，需填寫原因");
            entity.Property(e => e.ShortTimeID_IsError)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("短時間ID頻繁_是否異常\r\nY/N\r\nInternalEmailSame_Flag =Y，需填寫是否異常");
            entity.Property(e => e.ShortTimeID_UpdateTime)
                .HasComment("短時間ID頻繁_確認時間")
                .HasColumnType("datetime");
            entity.Property(e => e.ShortTimeID_UpdateUserId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("短時間ID頻繁_確認人員");
        });

        modelBuilder.Entity<Reviewer_CardRecord>(entity =>
        {
            entity.HasKey(e => e.SeqNo);

            entity.ToTable(tb => tb.HasComment("卡別紀錄"));

            entity.Property(e => e.SeqNo).HasComment("PK\r\n自增");
            entity.Property(e => e.Action).HasComment("執行動作\r\n1.權限內\r\n2.權限外 (轉交)\r\n3.退回重審");
            entity.Property(e => e.AddTime)
                .HasComment("新增時間")
                .HasColumnType("datetime");
            entity.Property(e => e.ApplyNo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasComment("申請書編號\r\n非叢集索引");
            entity.Property(e => e.ApproveUserId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("核准員工");
            entity.Property(e => e.CardLimit).HasComment("核卡額度");
            entity.Property(e => e.CardStatus).HasComment("卡片狀態");
            entity.Property(e => e.ForwardedToUserId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("轉交員工\r\n當執行權限外時會出現此值，被轉交員工會掛於此");
            entity.Property(e => e.HandleNote)
                .HasMaxLength(100)
                .HasComment("處理備註");
            entity.Property(e => e.HandleSeqNo)
                .HasMaxLength(26)
                .IsUnicode(false)
                .HasComment("FK\r\n關聯 Reviewer_ApplyCreditCardInfoHandle\r\nULID");
        });

        modelBuilder.Entity<Reviewer_FinanceCheckInfo>(entity =>
        {
            entity.HasKey(e => new { e.ApplyNo, e.ID, e.UserType });

            entity.ToTable(tb => tb.HasComment("徵審_金融檢核"));

            entity.Property(e => e.ApplyNo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasComment("申請書編號\r\n");
            entity.Property(e => e.ID)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasComment("身分證字號");
            entity.Property(e => e.UserType).HasComment("使用者類型\r\n1 = 正卡人 \r\n2 =  附卡人");
            entity.Property(e => e.AMLRiskLevel)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasComment("洗防風險等級");
            entity.Property(e => e.BranchCus_QueryTime)
                .HasComment("分行客戶_查詢時間")
                .HasColumnType("datetime");
            entity.Property(e => e.BranchCus_RtnCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasComment("分行客戶_回傳代碼");
            entity.Property(e => e.BranchCus_RtnMsg)
                .HasMaxLength(50)
                .HasComment("分行客戶_回傳訊息");
            entity.Property(e => e.Checked929)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("929檢核\r\n\r\n- 用API方式以身分證字號查詢資料庫，檢核是否命中情形，資料庫來源為業務管理部\r\n- 入月收入確認前會先發查一次\r\n- 完成月收入確認時，如與落月收入確認發查日非為同一天，會再發查一次\r\n- 核卡的日期，如前一次發查929的日期非同一日，會再次發查。\r\n\r\n");
            entity.Property(e => e.FamilyMessageChecked)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("家事法訊息\r\n\r\n- 資料來源總行業管部自建資料庫\r\n- Y 、N");
            entity.Property(e => e.Focus1Check)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("關注名單1\r\n\r\n- Y/N");
            entity.Property(e => e.Focus1Hit)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("命中關注名單1\r\n\r\n- 命中的話會以代號串接使用、分割");
            entity.Property(e => e.Focus1_QueryTime)
                .HasComment("關注名單1_查詢時間")
                .HasColumnType("datetime");
            entity.Property(e => e.Focus1_RtnCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasComment("關注名單1_回傳代碼");
            entity.Property(e => e.Focus1_RtnMsg)
                .HasMaxLength(50)
                .HasComment("關注名單1_回傳訊息");
            entity.Property(e => e.Focus1_TraceId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("關注名單1_TraceId");
            entity.Property(e => e.Focus2Check)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("關注名單2\r\n\r\n- Y/N");
            entity.Property(e => e.Focus2Hit)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("命中關注名單2\r\n\r\n- 命中的話會以代號串接使用、分割");
            entity.Property(e => e.Focus2_QueryTime)
                .HasComment("關注名單2_查詢時間")
                .HasColumnType("datetime");
            entity.Property(e => e.Focus2_RtnCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasComment("關注名單2_回傳代碼");
            entity.Property(e => e.Focus2_RtnMsg)
                .HasMaxLength(50)
                .HasComment("關注名單2_回傳訊息");
            entity.Property(e => e.Focus2_TraceId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("關注名單2_TraceId");
            entity.Property(e => e.IDCheckResultChecked)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("身分證查驗結果\r\n\r\n- 資料來源總行業管部自建資料庫\r\n- Y 、N");
            entity.Property(e => e.IsBranchCustomer)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("是否為分行客戶\r\n\r\n- Y / N");
            entity.Property(e => e.KYC_Handler)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("KYC_經辦");
            entity.Property(e => e.KYC_Handler_SignTime)
                .HasComment("KYC_經辦_簽核時間")
                .HasColumnType("datetime");
            entity.Property(e => e.KYC_KYCManager)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("KYC_防洗錢主管");
            entity.Property(e => e.KYC_KYCManager_SignTime)
                .HasComment("KYC_防洗錢主管_簽核時間")
                .HasColumnType("datetime");
            entity.Property(e => e.KYC_Message)
                .HasMaxLength(500)
                .HasComment("KYC 回傳訊息");
            entity.Property(e => e.KYC_QueryTime)
                .HasComment("KYC_查詢時間")
                .HasColumnType("datetime");
            entity.Property(e => e.KYC_Reviewer)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("KYC_覆核");
            entity.Property(e => e.KYC_Reviewer_SignTime)
                .HasComment("KYC_覆核_簽核時間")
                .HasColumnType("datetime");
            entity.Property(e => e.KYC_RtnCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("KYC_回傳代碼");
            entity.Property(e => e.KYC_StrongReDetailJson).HasComment("KYC_加強審核詳細資訊Json");
            entity.Property(e => e.KYC_StrongReStatus).HasComment("加強審核執行狀態\r\n\r\n- 狀態為 不需檢核 或 核准,才能核卡，否則就需要做加強審核簽核才行\r\n\r\n定義值\r\n1. 未送審 (經辦)\r\n2. 送審中 (主管) \r\n3. 核准 (可核卡) (經辦) \r\n4. 駁回 (經辦) \r\n5. 不需檢核\r\n");
            entity.Property(e => e.KYC_StrongReVersion)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasComment("KYC加強審核版本\r\n\r\n- 利用此欄位決定KYC加強審核版本主要用於套印加強審核檔案\r\n- 由系統參數決定");
            entity.Property(e => e.KYC_Suggestion)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComment("KYC_建議核准\r\n\r\n- Y/N");
            entity.Property(e => e.Q929_QueryTime)
                .HasComment("929_查詢時間")
                .HasColumnType("datetime");
            entity.Property(e => e.Q929_RtnCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasComment("929_回傳代碼");
            entity.Property(e => e.Q929_RtnMsg)
                .HasMaxLength(50)
                .HasComment("929_回傳訊息");
        });

        modelBuilder.Entity<Reviewer_InternalCommunicate>(entity =>
        {
            entity.HasKey(e => e.ApplyNo);

            entity.ToTable(tb => tb.HasComment("徵審內部溝通"));

            entity.Property(e => e.ApplyNo)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.CommunicationNotes)
                .HasComment("溝通註記\r\n當核卡完成時清空")
                .HasColumnType("ntext");
            entity.Property(e => e.SupplementContactRecords_Result).HasComment("補聯繫紀錄_結果");
            entity.Property(e => e.SupplementContactRecords_Summary)
                .HasComment("補聯繫紀錄_摘要")
                .HasColumnType("ntext");
            entity.Property(e => e.SupplementContactRecords_Type).HasComment("補聯繫紀錄_類別");
            entity.Property(e => e.SupplementContactRecords_UserId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("補聯繫紀錄_登錄人員");
        });

        modelBuilder.Entity<Reviewer_OutsideBankInfo>(entity =>
        {
            entity.HasKey(e => new { e.ApplyNo, e.ID, e.UserType }).HasName("PK_Reviewer_OutsideBankInfo_1");

            entity.HasIndex(e => e.ApplyNo, "NonClusteredIndex-20241127-150700");

            entity.Property(e => e.ApplyNo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasComment("申請書編號，PK");
            entity.Property(e => e.ID)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasComment("身分證字號，PK");
            entity.Property(e => e.UserType).HasComment("使用者類型，PK\r\n1.正卡人\r\n2.附卡人");
            entity.Property(e => e.BalanceUpdateDate)
                .HasComment("餘額更新日期")
                .HasColumnType("datetime");
            entity.Property(e => e.DINGCUN_Balance).HasComment("定存目前餘額");
            entity.Property(e => e.DINGCUN_Balance_90).HasComment("定存90天平均餘額");
            entity.Property(e => e.HUOCUN_Balance).HasComment("活存目前餘額");
            entity.Property(e => e.HUOCUN_Balance_90).HasComment("活存90天平均餘額");
        });

        modelBuilder.Entity<SetUp_AMLProfession>(entity =>
        {
            entity.HasKey(e => e.SeqNo).HasName("PK_SetUp_AMLProfession_1");

            entity.ToTable(tb => tb.HasComment("AML職業別"));

            entity.Property(e => e.SeqNo)
                .HasMaxLength(26)
                .IsUnicode(false)
                .HasComment("PK");
            entity.Property(e => e.AMLProfessionCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasComment("AML職業別代碼");
            entity.Property(e => e.AMLProfessionCompareResult)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasComment("AML職業別比對結果，Y | N");
            entity.Property(e => e.AMLProfessionName)
                .HasMaxLength(50)
                .HasComment("AML職業別名稱");
            entity.Property(e => e.AddTime)
                .HasComment("新增時間")
                .HasColumnType("datetime");
            entity.Property(e => e.AddUserId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("新增員工");
            entity.Property(e => e.IsActive)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("是否啟用，Y | N");
            entity.Property(e => e.UpdateTime)
                .HasComment("修正時間")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateUserId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("修正員工");
            entity.Property(e => e.Version)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasComment("版本，範例: 20240102，用於切分上線時間點");
        });

        modelBuilder.Entity<SetUp_AddressInfo>(entity =>
        {
            entity.HasKey(e => e.SeqNo);

            entity.HasIndex(e => new { e.City, e.Area, e.Road }, "NonClusteredIndex-City_Area_Road");

            entity.Property(e => e.SeqNo)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasComment("PK");
            entity.Property(e => e.AddTime)
                .HasComment("新增時間")
                .HasColumnType("datetime");
            entity.Property(e => e.AddUserId)
                .HasMaxLength(30)
                .HasComment("新增員工");
            entity.Property(e => e.Area)
                .HasMaxLength(5)
                .HasComment("區域，例如蘆洲區");
            entity.Property(e => e.City)
                .HasMaxLength(5)
                .HasComment("縣市，例如新北市");
            entity.Property(e => e.IsActive)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Road)
                .HasMaxLength(20)
                .HasComment("街道");
            entity.Property(e => e.Scope)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("號判斷郵遞區號規則");
            entity.Property(e => e.UpdateTime)
                .HasComment("修正時間")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateUserId)
                .HasMaxLength(30)
                .HasComment("修正員工");
            entity.Property(e => e.ZIPCode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasComment("郵遞區號");
        });

        modelBuilder.Entity<SetUp_CardPromotion>(entity =>
        {
            entity.HasKey(e => e.CardPromotionCode);

            entity.ToTable(tb => tb.HasComment("信用卡優惠辦法"));

            entity.Property(e => e.CardPromotionCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasComment("優惠辦法代碼，範例 : 0001");
            entity.Property(e => e.AddTime)
                .HasComment("新增時間")
                .HasColumnType("datetime");
            entity.Property(e => e.AddUserId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("新增員工");
            entity.Property(e => e.CardPromotionName)
                .HasMaxLength(50)
                .HasComment("優惠辦法名稱");
            entity.Property(e => e.InterestRate)
                .HasComment("利率，範例 : 12.22")
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.IsActive)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("是否啟用，Y | N");
            entity.Property(e => e.PrimaryCardReservedPOT)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasComment("正卡預留POT，範例 : 01");
            entity.Property(e => e.PrimaryCardUsedPOT)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasComment("正卡使用POT，範例 : 01");
            entity.Property(e => e.ReservePromotionPeriod)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasComment("預留優惠期限(月)，範例 : 01");
            entity.Property(e => e.SupplementaryCardReservedPOT)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasComment("附卡預留POT，範例 : 01");
            entity.Property(e => e.SupplementaryCardUsedPOT)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasComment("附卡使用POT，範例 : 01");
            entity.Property(e => e.UpdateTime)
                .HasComment("修正時間")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateUserId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("修正員工");
            entity.Property(e => e.UsedPOTExpiryMonth)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasComment("使用POT截止月份，範例 : 01");
        });

        modelBuilder.Entity<SysParamManage_SysParam>(entity =>
        {
            entity.HasKey(e => e.SeqNo);

            entity.ToTable(tb => tb.HasComment("系統參數設定"));

            entity.Property(e => e.SeqNo).HasComment("PK");
            entity.Property(e => e.AMLProfessionCode_Version)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasComment("AML職業別版本\r\n利用此版本對應 AML職業別\r\n關聯SetUp_AMLProfession");
            entity.Property(e => e.IPCompareHour).HasComment("IP比對時間(小時)");
            entity.Property(e => e.IPMatchCount).HasComment("IP比對吻合次數");
            entity.Property(e => e.KYCFixEndTime)
                .HasComment("KYC 維護結束時間")
                .HasColumnType("datetime");
            entity.Property(e => e.KYCFixStartTime)
                .HasComment("KYC 維護開始時間")
                .HasColumnType("datetime");
            entity.Property(e => e.KYC_StrongReVersion)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasComment("KYC加強審核版本\r\n\r\n- 利用此欄位決定KYC加強審核版本主要用於套印加強審核檔案\r\n- 關聯 Reviewer_FinanceCheckInfo\r\n\r\n");
            entity.Property(e => e.QueryHisDataDayRange).HasComment("往前推歷史資料幾天");
            entity.Property(e => e.ShortTimeIDCompareHour).HasComment("短時間ID比對時間(小時)");
            entity.Property(e => e.ShortTimeIDMatchCount).HasComment("短時間ID比對吻合次數");
            entity.Property(e => e.WebCaseEmailCompareHour).HasComment("網路件相同EMail比對時間(小時)");
            entity.Property(e => e.WebCaseEmailMatchCount).HasComment("網路件相同EMail比對吻合次數");
            entity.Property(e => e.WebCaseMobileCompareHour).HasComment("網路件相同手機比對時間(小時)");
            entity.Property(e => e.WebCaseMobileMatchCount).HasComment("網路件相同手機比對吻合次數");
        });

        modelBuilder.Entity<System_ErrorLog>(entity =>
        {
            entity.HasKey(e => e.SeqNo).HasName("PK_Reviewer3rd_ErrorLog");

            entity.HasIndex(e => e.ApplyNo, "NonClusteredIndex-ApplyNo");

            entity.Property(e => e.SeqNo).HasComment("PK");
            entity.Property(e => e.AddTime)
                .HasComment("創建時間")
                .HasColumnType("datetime");
            entity.Property(e => e.ApplyNo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasComment("申請書編號");
            entity.Property(e => e.ErrorDetail).HasComment("錯誤詳細資訊");
            entity.Property(e => e.ErrorMessage).HasComment("錯誤訊息");
            entity.Property(e => e.FailLog).HasComment("錯誤訊息");
            entity.Property(e => e.Note).HasComment("備註");
            entity.Property(e => e.Project)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasComment("專案\r\n- API\r\n- BATCH\r\n- Middlewave");
            entity.Property(e => e.Request).HasComment("可用於放置參數，例如呼叫第三方API");
            entity.Property(e => e.Response).HasComment("可用於放置回應，例如呼叫第三方API");
            entity.Property(e => e.SendEmailTime)
                .HasComment("寄信時間")
                .HasColumnType("datetime");
            entity.Property(e => e.SendStatus).HasComment("寄信狀態");
            entity.Property(e => e.Source)
                .HasMaxLength(50)
                .HasComment("來源\r\n範例:如非卡友檢核排程");
            entity.Property(e => e.Type)
                .HasMaxLength(30)
                .HasComment("類型\r\n\r\n第三方API呼叫\r\n系統錯誤");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
