using EFCoreHooks.Hooks.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EFCoreHooks.Hooks
{
    public abstract class HookDbContext : DbContext
    {
        protected HookDbContext(IEnumerable<IHook> hooks):base()
        {
            AddHooks(hooks);
        }

        protected HookDbContext(IEnumerable<IHook> hooks,DbContextOptions options) : base(options)
        {
            AddHooks(hooks);
        }

        private List<IHook> _hooks = new List<IHook>();

        public bool DisableHook { get; set; }

        private void AddHooks(IEnumerable<IHook> hooks)
        {
            _hooks = hooks.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IEnumerable<HookedEntityEntry> GetChangedEntities()
        {
            return ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted)
                .Select(
                    x =>
                        new HookedEntityEntry
                        {
                            Entity = x.Entity,
                            State = x.State
                        }
                );
        }

        private Task OnSaved(IEnumerable<HookedEntityEntry> entities,
                             CancellationToken cancellationToken = default)
        {
            return _hooks.Any()
                ? Task.WhenAll(_hooks.Select(p => p.OnSaved(entities, this, cancellationToken)))
                : Task.CompletedTask;
        }

        private Task OnSaving(IEnumerable<HookedEntityEntry> entities,
                              CancellationToken cancellationToken = default)
        {
            return _hooks.Any()
                ? Task.WhenAll(_hooks.Select(p => p.OnSaving(entities, this, cancellationToken)))
                : Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess"></param>
        /// <returns></returns>
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            if (DisableHook)
            {
                return base.SaveChanges(acceptAllChangesOnSuccess);
            }

            return SaveChangesWithHooks(acceptAllChangesOnSuccess);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess"></param>
        /// <returns></returns>
        internal int SaveChangesWithHooks(bool acceptAllChangesOnSuccess)
        {
            var entries = GetChangedEntities();

            OnSaving(entries).GetAwaiter().GetResult();
            var result = base.SaveChanges(acceptAllChangesOnSuccess);
            OnSaved(entries).GetAwaiter().GetResult();

            return result;
        }

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            if (DisableHook)
            {
                return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            }

            return SaveChangesWithHooksAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        internal Task<int> SaveChangesWithHooksAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken)
        {
            var entries = GetChangedEntities();

            OnSaving(entries, cancellationToken).ConfigureAwait(false);
            var result = base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            OnSaved(entries, cancellationToken).ConfigureAwait(false);

            return result;
        }
    }
}
