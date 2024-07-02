namespace GymManagement.Domain.Common;

public abstract class Entity
{
    protected Entity(Guid id)
    {
        Id = id;
    }

    protected Entity()
    {
    }
    
    public Guid Id { get; set; }

    protected readonly List<IDomainEvent> _domainEvents = [];
}