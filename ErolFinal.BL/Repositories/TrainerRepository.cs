using ErolFinal.DAL.Contexts;

namespace ErolFinal.BL.Repositories;

public class TrainerRepository
{
    protected AppDbContext _context;

    public TrainerRepository(AppDbContext context)
    {
        _context = context;
    }

    #region Create
    public async Task CreateTrainerAsync(TrainerModel model)
    {
        await _context.Trainers.AddAsync(model);
        await _context.SaveChangesAsync();
    }
    #endregion

    #region Read
    public async Task<TrainerModel?> GetTrainerByIdAsync(int id) => await _context.Trainers.FindAsync(id);
    public async Task<List<TrainerModel>> GetAllTrainersAsync() => await _context.Trainers.ToListAsync();
    #endregion

    #region Update
    public async Task SaveTrainerAsync(TrainerModel model)
    {
        _context.Entry(model).State = EntityState.Added;
        await _context.SaveChangesAsync();
    }
    #endregion

    #region Delete
    public async Task DeleteTrainerAsync(int id)
    {
        var existingTrainer = await GetTrainerByIdAsync(id);
        if (existingTrainer == null) return;
        _context.Trainers.Remove(existingTrainer);
        await _context.SaveChangesAsync();
    }
    #endregion
}
