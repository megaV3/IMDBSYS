using ELNET_FinalsProject.Models;

namespace ELNET_FinalsProject.Data.Repositories
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
