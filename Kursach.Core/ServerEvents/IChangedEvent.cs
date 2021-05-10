namespace ISTraining_Part.Core.ServerEvents
{
    public interface IChangedEvent<T>
    {
        void OnChanged(DbChangeStatus status, T arg);
    }
}
