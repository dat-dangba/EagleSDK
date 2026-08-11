namespace Eagle
{
    public interface IService
    {
    }

    public interface IInitializable : IService
    {
        void Initialize();
    }

    public interface IAsyncInitializable : IService
    {
        System.Threading.Tasks.Task InitializeAsync();
    }

    public interface ITickable : IService
    {
        void Tick();
    }

    public interface IDisposable : IService
    {
        void Dispose();
    }
}
