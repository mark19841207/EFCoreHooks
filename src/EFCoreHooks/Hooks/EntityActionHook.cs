using EFCoreHooks.Hooks.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EFCoreHooks.Hooks
{
    internal class EntityActionHook : Hook
    {
        private readonly IEnumerable<IActionHook> actionHooks;

        public EntityActionHook(IEnumerable<IActionHook> actionHooks)
        {
            this.actionHooks = actionHooks;
        }

        public override Task OnSaving(IEnumerable<HookedEntityEntry> entities,
                                      DbContext dbContext,
                                      CancellationToken cancellationToken = default)
        {

            if (dbContext is null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (!dbContext.ChangeTracker.HasChanges())
            {
                return base.OnSaving(entities, dbContext, cancellationToken);
            }

            var tasks = entities.Select(
                    x =>
                        actionHooks.FirstOrDefault(h => h.HookStates.Equals(x.State)).OnSavingAsync(x.Entity, x.State, dbContext)
                );

            cancellationToken.ThrowIfCancellationRequested();

            return Task.WhenAll(tasks);
        }
    }
}
