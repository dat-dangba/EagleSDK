using System.Threading.Tasks;

namespace Eagle
{
    public abstract class BaseGameBootstrap : BaseBootstrap
    {
        protected override ServiceContainer Container => ServiceLocator.Game;

        protected abstract void ManualInitializeServices();

        protected override Task BootstrapAsync()
        {
            ManualInitializeServices();
            return base.BootstrapAsync();
        }
    }
}
