using System;
using System.Collections.Generic;
using Tofft_preprod.DbContext;

namespace Tofft_preprod.Repositories;

public interface IRepository<T> where T : class{
    //ApplicationDbContext _context { get; }
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(string id);
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}