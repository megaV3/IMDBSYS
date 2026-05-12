using IMDBSYS.Models;

namespace IMDBSYS.Data.Repositories
{
    public interface IStoreRepository
    {
        IQueryable<Menu> Menus { get; }
    }
}
