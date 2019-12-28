using Microsoft.EntityFrameworkCore;

namespace EFCoreHooks.Hooks
{
    public abstract class InsertHook<TEntity> : ActionHook<TEntity>
    {
        public override EntityState HookStates
        {
            get { return EntityState.Added; }
        }
    }
}
