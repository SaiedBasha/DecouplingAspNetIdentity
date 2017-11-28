using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DecouplingAspNetIdentity.Infrastructure.DataContextStorage
{
    public static class DataContextStorageFactory<T> where T : class
    {
        private static IDataContextStorageContainer<T> _dataContextStorageContainer;

        /// <summary>
        /// Create & return data context storage container
        /// </summary>
        /// <returns>Instance of IDataContextStorageContainer</returns>
        public static IDataContextStorageContainer<T> CreateStorageContainer()
        {
            if (HttpContext.Current == null)
            {
                if (_dataContextStorageContainer == null || _dataContextStorageContainer is HttpDataContextStorageContainer<T>)
                {
                    _dataContextStorageContainer = new ThreadDataContextStorageContainer<T>();
                }
            }
            else
            {
                if (_dataContextStorageContainer == null || _dataContextStorageContainer is ThreadDataContextStorageContainer<T>)
                {
                    _dataContextStorageContainer = new HttpDataContextStorageContainer<T>();
                }
            }

            return _dataContextStorageContainer;
        }
    }
}
