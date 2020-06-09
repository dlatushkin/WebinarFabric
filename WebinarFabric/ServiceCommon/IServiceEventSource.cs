namespace ServiceCommon
{
    public interface IServiceEventSource
    {
        void ServiceHostInitializationFailed(string toString);

        void ServiceTypeRegistered(int id, string name);
    }
}
