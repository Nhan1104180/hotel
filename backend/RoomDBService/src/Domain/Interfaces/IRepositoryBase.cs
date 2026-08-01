using System.Linq.Expressions;

namespace Domain.Interfaces;

public interface IRepositoryBase<T> where T : class
{
    // crud chung chung cơ bản
    Task<T> GetByIdAsync(int id);  //Lấy theo ID
    Task<IEnumerable<T>> GetAllAsync();   //Lấy danh sách
    Task AddAsync(T entity);   //Thêm
    Task UpdateAsync(T entity); //Cập nhật
    Task DeleteAsync(T entity); //Xóa
    Task<List<T>> WhereAsync(Expression<Func<T, bool>> predicate);
}