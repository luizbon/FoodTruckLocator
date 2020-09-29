using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FoodTruckLocator.Models;

namespace FoodTruckLocator.Providers
{
    public interface IStorageProvider
    {
        Task<IEnumerable<Permit>> ReadPermitsAsync();
    }

    public class StorageProvider:IStorageProvider
    {
        public Task<IEnumerable<Permit>> ReadPermitsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
