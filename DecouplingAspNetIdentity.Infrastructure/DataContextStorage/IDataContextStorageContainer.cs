
namespace DecouplingAspNetIdentity.Infrastructure.DataContextStorage
{
    public interface IDataContextStorageContainer<T>
    {
        T GetDataContext();

        void Store(T objectContext);

        void Clear();
    }
}
