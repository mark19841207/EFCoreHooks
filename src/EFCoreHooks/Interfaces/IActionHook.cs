using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EFCoreHooks.Hooks.Interfaces
{
    public interface IActionHook
    {
        Task OnSavingAsync(object entity, EntityState state, DbContext dbContext);

        EntityState HookStates { get; }
    }
}
