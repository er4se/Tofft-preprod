using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tofft_preprod.DbContext;
using Tofft_preprod.Models;

namespace Tofft_preprod.Repositories;

public class BoardRepository : IRepository<Board>{
    private readonly ApplicationDbContext _context;
    
    public BoardRepository (ApplicationDbContext context){
        this._context = context;
    }
    
    public async Task<IEnumerable<Board>> GetAllAsync()
    {
        try
        {
            var entities = await _context.Boards.ToListAsync();
            return entities;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<Board> GetByIdAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
            throw new ArgumentNullException(nameof(id), "ID не может быть пустым.");

        var entity = await _context.Boards
            .FirstOrDefaultAsync(b => b.Id == id);

        if (entity == null)
            throw new KeyNotFoundException($"Доска с ID {id} не найдена.");

        return entity;
    }

    public async Task CreateAsync(Board entity)
    {
        try
        {   
            entity.Id = Guid.NewGuid().ToString();

            var userToBoard = new UserToBoard
            {
                UserId = entity.AdminId,
                BoardId = entity.Id,
                UserLocalSpeciality = "Administrator",
                Role = BoardRole.Admin
            };

            await _context.Boards.AddAsync(entity);
            await _context.UserToBoards.AddAsync(userToBoard);

            await _context.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task UpdateAsync(Board entity)
    {
        try
        {
            _context.Boards.Update(entity);
            await _context.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        
    }

    public async Task DeleteAsync(Board entity)
    {
        try
        {
            _context.Boards.Remove(entity);
            await _context.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}