using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ScoreSharp.API.Infrastructures.Data.EFCoreInterceptors;

public class AuditInterceptor : SaveChangesInterceptor
{
    private readonly IServiceProvider _services;

    public AuditInterceptor(IServiceProvider services)
    {
        _services = services;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
    )
    {
        if (eventData.Context is not null)
        {
            this.UpdateAuditableEntities(eventData.Context);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateAuditableEntities(DbContext context)
    {
        DateTime now = DateTime.Now;
        using (var scope = _services.CreateScope())
        {
            var _jwtTProfilerHelper = scope.ServiceProvider.GetRequiredService<IJWTProfilerHelper>();
            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    if (entry.Metadata.FindProperty("AddUserId") != null)
                    {
                        entry.Property("AddUserId").CurrentValue = _jwtTProfilerHelper.UserId;
                    }

                    if (entry.Metadata.FindProperty("AddTime") != null)
                    {
                        entry.Property("AddTime").CurrentValue = now;
                    }
                }
                else if (entry.State == EntityState.Modified)
                {
                    if (entry.Metadata.FindProperty("UpdateUserId") != null)
                    {
                        entry.Property("UpdateUserId").CurrentValue = _jwtTProfilerHelper.UserId;
                    }

                    if (entry.Metadata.FindProperty("UpdateTime") != null)
                    {
                        entry.Property("UpdateTime").CurrentValue = now;
                    }
                }
                else if (entry.State == EntityState.Deleted)
                {
                    // 在刪除前保存原有資料到 System_DeleteLog
                    var deletedEntity = entry.Entity;
                    var deletedLog = new System_DeleteLog
                    {
                        TableName = entry.Metadata.GetTableName(),
                        OriginData = JsonSerializer.Serialize(deletedEntity),
                        DeleteUserId = _jwtTProfilerHelper.UserId,
                        DeleteTime = now,
                    };

                    // 添加 deletedLog 到上下文中
                    context.Set<System_DeleteLog>().Add(deletedLog);
                }
            }
        }
    }
}
