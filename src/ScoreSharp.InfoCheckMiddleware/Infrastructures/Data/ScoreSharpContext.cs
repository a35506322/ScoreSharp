using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ScoreSharp.InfoCheckMiddleware.Infrastructures.Data.Entities;

namespace ScoreSharp.InfoCheckMiddleware.Infrastructures.Data;

public partial class ScoreSharpContext : DbContext
{
    public ScoreSharpContext(DbContextOptions<ScoreSharpContext> options)
        : base(options)
    {
    }

    public virtual DbSet<System_ErrorLog> System_ErrorLog { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
