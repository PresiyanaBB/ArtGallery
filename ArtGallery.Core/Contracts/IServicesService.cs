using ArtGallery.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Core.Contracts
{
    public interface IServicesService
    {
        public Task<IEnumerable<ServiceModel>> GetServicesAsync();
        Task<bool> CreateServiceAsync(string name, string description, decimal price);
        Task<bool> RemoveServiceAsync(string id);
    }
}
