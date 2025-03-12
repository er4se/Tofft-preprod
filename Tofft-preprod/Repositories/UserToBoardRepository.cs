using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tofft_preprod.DbContext;
using Tofft_preprod.Models;

namespace Tofft_preprod.Repositories;

public class UserToBoardRepository : IRepository<UserToBoard>{
    private readonly ApplicationDbContext _context;
    
    public UserToBoardRepository (ApplicationDbContext context){
        this._context = context;
    }
    
    public async Task<IEnumerable<UserToBoard>> GetAllAsync()
    {
        try
        {
            var entities = await _context.UserToBoards.ToListAsync();
            return entities;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
    /// <summary>
    
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Не рекомендуется к использованию</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    public async Task<UserToBoard> GetByIdAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
            throw new ArgumentNullException(nameof(id), "ID не может быть пустым.");

        var entity = await _context.UserToBoards
            .FirstOrDefaultAsync(b => b.UserId == id);

        if (entity == null)
            throw new KeyNotFoundException($"Доска с ID {id} не найдена.");

        return entity;
    }

    public async Task<IEnumerable<UserToBoard>> GetAllBoardsByUser(string id)
    {
        try
        {
            var entities = await _context.UserToBoards
                .Where(b => b.UserId == id)
                .ToListAsync();

            return entities;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<IEnumerable<UserToBoard>> GetAllUsersByBoard(string id)
    {
        try
        {
            var entities = await _context.UserToBoards
                .Where(b => b.BoardId == id)
                .ToListAsync();
                
            return entities;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task CreateAsync(UserToBoard entity)
    {
        try
        {
            await _context.UserToBoards.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task UpdateAsync(UserToBoard entity)
    {
        try
        {
            _context.UserToBoards.Update(entity);
            await _context.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        
    }

    public async Task DeleteAsync(UserToBoard entity)
    {
        try
        {
            _context.UserToBoards.Remove(entity);
            await _context.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}