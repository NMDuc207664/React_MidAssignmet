using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Library_API_2._0.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Library_API_2._0.Infrastructure.Persistence.Interceptors
{
    public class ProtectedEntitiesInterceptor : SaveChangesInterceptor
    {
        private static readonly Guid[] _protectedGenreIds = new[]
        {
            Guid.Parse("9BD6855A-634C-4248-8641-F7FEF2DC97C3"),
            Guid.Parse("639de03f-7876-4fff-96ec-37f8bd3bf180"),
            Guid.Parse("45deb9d6-c1ae-44a6-a03b-c9a5cfc15f2f"),
            Guid.Parse("B625C683-D11B-4B3D-B440-CFDD51C26E92"),
        };

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            CheckProtectedEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            CheckProtectedEntities(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void CheckProtectedEntities(DbContext context)
        {
            if (context == null) return;

            var entries = context.ChangeTracker
                .Entries()
                .Where(e => e.State == EntityState.Modified || e.State == EntityState.Deleted);

            foreach (var entry in entries)
            {
                Guid? entityId = GetEntityId(entry.Entity);

                if (entityId.HasValue && _protectedGenreIds.Contains(entityId.Value))
                {
                    throw new InvalidOperationException(
                        $"Cannot modify or delete protected entity: {entry.Entity.GetType().Name} with ID {entityId}");
                }
            }
        }

        private Guid? GetEntityId(object entity)
        {
            return entity switch
            {
                Author author => author.Id,
                Genre genre => genre.Id,
                IdentityRole role => Guid.TryParse(role.Id, out var roleId) ? roleId : null,
                _ => null
            };
        }
    }
}