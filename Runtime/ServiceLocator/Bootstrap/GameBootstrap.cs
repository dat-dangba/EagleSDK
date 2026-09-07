namespace Eagle
{
    public class GameBootstrap : BaseBootstrap
    {
        protected override ServiceContainer Container { get; } = ServiceLocator.Game;
    }
}
