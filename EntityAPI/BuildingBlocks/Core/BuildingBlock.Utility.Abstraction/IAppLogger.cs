namespace BuildingBlock.Utility.Abstraction
{
    public interface IAppLogger<T>
    {
        void LogInformation(string info);
        void LogWarning(string warning);
        void LogError(string info);
    }
}