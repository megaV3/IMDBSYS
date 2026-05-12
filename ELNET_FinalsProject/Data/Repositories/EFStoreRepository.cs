using IMDBSYS.Models;

namespace IMDBSYS.Data.Repositories
{
    public class EFStoreRepository : IStoreRepository
    {
        private AppDbContext context;

        public EFStoreRepository(AppDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Menu> Menus => context.Menus;
    }
}
