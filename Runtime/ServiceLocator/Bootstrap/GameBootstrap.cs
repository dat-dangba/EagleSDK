namespace Eagle
{
    public class GameBootstrap : BaseBootstrap
    {
        protected override ServiceContainer Container => ServiceLocator.Game;
    }
}
