using ErolFinal.DAL.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ErolFinal.BL.Services;

public class TrainerService
{
    protected AppDbContext _context;

    public TrainerService(AppDbContext context)
    {
        _context = context;
    }

    #region Create
    public async Task CreateAsync(TrainerModel model)
    {
        await _context.Trainers.AddAsync(model);
        await _context.SaveChangesAsync();
    }
    #endregion

    #region Read
    public async Task<TrainerModel?> GetByIdAsync(int id) => await _context.Trainers.FindAsync(id);
    public async Task<List<TrainerModel>> GetAllAsync() => await _context.Trainers.ToListAsync();
    #endregion

    #region Update
    public async Task UpdateAsync(int id, TrainerModel model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }
        if (id != model.Id)
        {
            throw new ArgumentException("The provided id as argument must match with the model id");
        }
        if (await GetByIdAsync(id) != model)
        {
            throw new ArgumentException("The provided model must be tracked by the database context. Consider creating models by GetByIdAsync() instead!");
        }

        await _context.SaveChangesAsync();
    }
    #endregion

    #region Delete
    public async Task DeleteAsync(int id)
    {
        var existingModel = await GetByIdAsync(id);
        if (existingModel == null) return;
        _context.Trainers.Remove(existingModel);
        await _context.SaveChangesAsync();
    }
    #endregion
}
