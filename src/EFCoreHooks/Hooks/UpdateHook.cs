using Microsoft.EntityFrameworkCore;

namespace EFCoreHooks.Hooks
{
    public abstract class UpdateHook<TEntity> : ActionHook<TEntity>
    {
        public override EntityState HookStates
        {
            get { return EntityState.Modified; }
        }
    }
}
