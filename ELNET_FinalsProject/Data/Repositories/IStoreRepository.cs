using ELNET_FinalsProject.Models;

namespace ELNET_FinalsProject.Data.Repositories
{
    public interface IStoreRepository
    {
        IQueryable<Menu> Menus { get; }
    }
}
