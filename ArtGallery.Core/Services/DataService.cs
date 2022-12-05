using ArtGallery.Infrastructure.Data.Repositories;

namespace ArtGallery.Core.Services
{
    public abstract class DataService
    {
        protected readonly IApplicationDbRepository repo;

        protected DataService(IApplicationDbRepository repo)
        {
            this.repo = repo;
        }
    }
}
