namespace Eagle
{
    public class SdkBootstrap : BaseBootstrap
    {
        protected override ServiceContainer Container { get; } = ServiceLocator.Sdk;
    }
}
