using DecouplingAspNetIdentity.Infrastructure.DataContextStorage;

namespace DecouplingAspNetIdentity.Repositories.EF
{
    public static class DataContextFactory
    {
        public static void Clear()
        {
            var dataContextStorageContainer = DataContextStorageFactory<ApplicationDbContext>.CreateStorageContainer();
            dataContextStorageContainer.Clear();
        }

        public static ApplicationDbContext GetDataContext()
        {
            var dataContextStorageContainer = DataContextStorageFactory<ApplicationDbContext>.CreateStorageContainer();
            var applicationContext = dataContextStorageContainer.GetDataContext();
            if (applicationContext != null)
            {
                return applicationContext;
            }

            applicationContext = new ApplicationDbContext();
            dataContextStorageContainer.Store(applicationContext);

            return applicationContext;
        }
    }
}