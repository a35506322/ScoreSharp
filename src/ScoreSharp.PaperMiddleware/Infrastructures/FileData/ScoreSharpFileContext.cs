using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ScoreSharp.PaperMiddleware.Infrastructures.FileData.Entities;

namespace ScoreSharp.PaperMiddleware.Infrastructures.FileData;

public partial class ScoreSharpFileContext : DbContext
{
    public ScoreSharpFileContext(DbContextOptions<ScoreSharpFileContext> options)
        : base(options) { }

    public virtual DbSet<Reviewer_ApplyFile> Reviewer_ApplyFile { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reviewer_ApplyFile>(entity =>
        {
            entity.HasKey(e => e.SeqNo);

            entity.HasIndex(e => new { e.ApplyNo, e.FileType }, "NonClusteredIndex-20250522-160414");

            entity.HasIndex(e => e.FileId, "UQ__Reviewer__6F0F98BECC1D2038").IsUnique();

            entity.Property(e => e.ApplyNo).HasMaxLength(15).IsUnicode(false).HasComment("申請書編號");
            entity.Property(e => e.FileContent).HasComment("檔案內容");
            entity.Property(e => e.FileId).HasComment("FileTable 所需");
            entity.Property(e => e.FileName).HasMaxLength(100).HasComment("檔案名稱\r\n\r\n- 包含附檔名\r\n- 申請書編號_檔案類型_時間戳_副檔名\r\n");
            entity.Property(e => e.FileType).HasComment("檔案類型\r\n\r\n1. 申請書相關\r\n2. 行員附件\r\n");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
