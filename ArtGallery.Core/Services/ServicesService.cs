using ArtGallery.Core.Common;
using ArtGallery.Core.Contracts;
using ArtGallery.Core.Models;
using ArtGallery.Infrastructure.Data;
using ArtGallery.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Core.Services
{
    public class ServicesService : DataService, IServicesService
    {
        public ServicesService(IApplicationDbRepository repo)
            : base(repo)
        {
        }

        public async Task<IEnumerable<ServiceModel>> GetServicesAsync()
        {
            return await this.repo.All<Service>()
                .OrderByDescending(s => s.Name)
                .ThenBy(s => s.Price)
                .Select(s => new ServiceModel
                {
                    Id = s.Id.ToString(),
                    Name = s.Name,
                    Price = s.Price,
                    Description = s.Description
                }).ToArrayAsync();
        }

        public async Task<bool> CreateServiceAsync(string name, string description, decimal price)
        {
            try
            {
                Guard.AgainstNullOrWhiteSpaceString(name);
                Guard.AgainstNullOrWhiteSpaceString(description);
                if (price <= 0) throw new ArgumentException("Price cannot be less than 0");

                var service = new Service
                {
                    Name = name,
                    Price = price,
                    Description = description
                };

                await this.repo.AddAsync(service);
                await this.repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> RemoveServiceAsync(string id)
        {
            try
            {
                Guard.AgainstNullOrWhiteSpaceString(id);

                await this.repo.DeleteAsync<Service>(Guid.Parse(id));
                await this.repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
