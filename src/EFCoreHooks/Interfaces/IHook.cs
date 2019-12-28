using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EFCoreHooks.Hooks.Interfaces
{
    public interface IHook
    {
        Task OnSaving(IEnumerable<HookedEntityEntry> entities, DbContext dbContext, CancellationToken cancellationToken = default);
        Task OnSaved(IEnumerable<HookedEntityEntry> entities, DbContext dbContext, CancellationToken cancellationToken = default);
    }
}
