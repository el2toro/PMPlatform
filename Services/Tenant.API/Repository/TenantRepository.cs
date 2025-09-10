using Tenant.API.Data;

namespace Tenant.API.Repository;

public interface ITenantRepository
{
    Task CreateTenant(string name, string description, Guid ownerId, CancellationToken cancellationToken);
}

public class TenantRepository : ITenantRepository
{
    private readonly TenantDbContext _tenantDbContext;
    public TenantRepository(TenantDbContext tenantDbContext)
    {
        _tenantDbContext = tenantDbContext;
    }
    public async Task CreateTenant(string name, string description, Guid ownerId, CancellationToken cancellationToken)
    {
        _tenantDbContext.Tenants.Add(new Models.Tenant
        {
            Name = name,
            Description = description,
            OwnerId = ownerId
        });

        await _tenantDbContext.SaveChangesAsync(cancellationToken);
    }
}
