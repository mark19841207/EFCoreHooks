using EFCoreHooks.Hooks.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EFCoreHooks.Hooks
{
    public abstract class ActionHook<TEntity> : IActionHook
    {
        public Task OnSavingAsync(object entity, EntityState state, DbContext dbContext)
        {
            if (typeof(TEntity).IsAssignableFrom(entity.GetType()))
            {
                return Hook((TEntity)entity, dbContext);
            }

            return Task.CompletedTask;
        }

        public abstract Task Hook(TEntity entity, DbContext dbContext);

        public abstract EntityState HookStates { get; }
    }
}
