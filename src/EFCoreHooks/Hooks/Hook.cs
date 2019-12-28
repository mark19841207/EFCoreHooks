using EFCoreHooks.Hooks.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EFCoreHooks.Hooks
{
    public abstract class Hook : IHook
    {
        public virtual Task OnSaved(IEnumerable<HookedEntityEntry> entities,
                                    DbContext dbContext,
                                    CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        public virtual Task OnSaving(IEnumerable<HookedEntityEntry> entities,
                                     DbContext dbContext,
                                     CancellationToken cancellationToken = default)
            => Task.CompletedTask;
    }
}
