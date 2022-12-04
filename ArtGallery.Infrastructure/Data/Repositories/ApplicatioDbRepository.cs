using ArtGallery.Infrastructure.Data.Common;
using ArtGallery.Infrastructure.Data;
using ArtGallery.Infrastructure.Data.Repositories;

namespace ArtGallery.Infrastructure.Data.Repositories
{
    public class ApplicatioDbRepository : Repository, IApplicatioDbRepository
    {
        public ApplicatioDbRepository(ApplicationDbContext context)
        {
            this.Context = context;
        }
    }
}