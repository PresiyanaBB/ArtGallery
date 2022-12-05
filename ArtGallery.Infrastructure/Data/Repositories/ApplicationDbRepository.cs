using ArtGallery.Infrastructure.Data.Common;
using ArtGallery.Infrastructure.Data;
using ArtGallery.Infrastructure.Data.Repositories;

namespace ArtGallery.Infrastructure.Data.Repositories
{
    public class ApplicationDbRepository : Repository, IApplicationDbRepository
    {
        public ApplicationDbRepository(ApplicationDbContext context)
        {
            this.Context = context;
        }
    }
}