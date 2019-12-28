using Microsoft.EntityFrameworkCore;

namespace EFCoreHooks.Hooks
{
    public sealed class HookedEntityEntry
    {
        public object Entity { get; set; }

        public EntityState State { get; set; }
    }
}
