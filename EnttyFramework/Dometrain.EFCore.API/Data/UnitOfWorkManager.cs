namespace Dometrain.EFCore.API.Data;

public interface IUnitOfWorkManager
{
    void StartUnitOfWork();
    bool IsUnitOfWorkStarted { get; }
    
    Task<int> SaveChangesAsync();
}

public class UnitOfWorkManager(MoviesContext _context): IUnitOfWorkManager
{
    private bool _isUnitOfWorkStarted = false;
    
    public void StartUnitOfWork()
    {
        _isUnitOfWorkStarted = true;
    }

    public bool IsUnitOfWorkStarted => _isUnitOfWorkStarted;
    
    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}