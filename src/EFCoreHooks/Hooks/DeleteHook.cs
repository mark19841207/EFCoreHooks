using Microsoft.EntityFrameworkCore;

namespace EFCoreHooks.Hooks
{
    public abstract class DeleteHook<TEntity> : ActionHook<TEntity>
    {
        public override EntityState HookStates
        {
            get { return EntityState.Deleted; }
        }
    }
}
