

namespace DAL.UnitOfWork.Interface
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}
