namespace Eagle
{
    public class SdkBootstrap : BaseBootstrap
    {
        protected override ServiceContainer Container => ServiceLocator.Sdk;
    }
}
