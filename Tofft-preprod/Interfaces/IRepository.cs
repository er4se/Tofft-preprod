using System;
using System.Collections.Generic;
using Tofft_preprod.DbContext;

namespace Tofft_preprod.Repositories;

public interface IRepository<T> where T : class{
    //ApplicationDbContext _context { get; }
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(string id);
    Task Create(T entity);
    Task Update(T entity);
    Task Delete(T entity);
}