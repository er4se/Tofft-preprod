using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tofft_preprod.DbContext;
using Tofft_preprod.Models;

namespace Tofft_preprod.Repositories;

public class MissionRepository : IRepository<Mission>
{
    private readonly ApplicationDbContext _context;
    
    public MissionRepository(ApplicationDbContext context){
        this._context = context;
    }
    
    public async Task<IEnumerable<Mission>> GetAllAsync()
    {
        try
        {
            var entities = await _context.Missions.ToListAsync();
            return entities;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<Mission> GetByIdAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
            throw new ArgumentNullException(nameof(id), "ID не может быть пустым.");

        var entity = await _context.Missions
            .FirstOrDefaultAsync(b => b.Id == id);

        if (entity == null)
            throw new KeyNotFoundException($"Доска с ID {id} не найдена.");

        return entity;
    }

    public async Task CreateAsync(Mission entity)
    {
        try
        {
            await _context.Missions.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task UpdateAsync(Mission entity)
    {
        try
        {
            _context.Missions.Update(entity);
            await _context.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        
    }

    public async Task DeleteAsync(Mission entity)
    {
        try
        {
            _context.Missions.Remove(entity);
            await _context.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}