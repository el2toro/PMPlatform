using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Tenant.API.Data;
using Tenant.API.Services;

namespace Tenant.API.Repository;

public interface ITenantRepository
{
    Task<Models.Tenant> CreateTenantAsync(Models.Tenant tenant, CancellationToken cancellationToken);
    Task<Models.Tenant> GetTenantByIdAsync(Guid tenantId, CancellationToken cancellationToken);
    Task DeleteTenantAsync(Guid tenantId, CancellationToken cancellationToken);
    Task<Models.Tenant?> GetTenantByNameAsync(string tenantName, CancellationToken cancellationToken);
}

public class TenantRepository : ITenantRepository
{
    private readonly TenantDbContext _tenantDbContext;
    private readonly AuthServiceClient _authServiceClient;
    public TenantRepository(TenantDbContext tenantDbContext, AuthServiceClient authServiceClient)
    {
        _tenantDbContext = tenantDbContext;
        _authServiceClient = authServiceClient;
    }

    public async Task<Models.Tenant> CreateTenantAsync(Models.Tenant tenant, CancellationToken cancellationToken)
    {
        var existingTenant = await _tenantDbContext.Tenants
            .FindAsync(tenant.Id, cancellationToken);

        if (existingTenant != null)
        {
            return new();
        };

        var newTenant = new Models.Tenant
        {
            Id = tenant.Id,
            Name = tenant.Name,
            Description = tenant.Description,
            OwnerId = tenant.OwnerId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };


        var createdTenant = _tenantDbContext.Tenants.Add(newTenant).Entity;
        await _tenantDbContext.SaveChangesAsync(cancellationToken);

        return createdTenant;
    }

    public async Task DeleteTenantAsync(Guid tenantId, CancellationToken cancellationToken)
    {
        var tenant = _tenantDbContext.Tenants.FirstOrDefault(t => t.Id == tenantId);

        ArgumentNullException.ThrowIfNull(tenant);

        _tenantDbContext.Tenants.Remove(tenant);
        await _tenantDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Models.Tenant> GetTenantByIdAsync(Guid tenantId, CancellationToken cancellationToken)
    {
        var tenant = await _tenantDbContext.Tenants.FirstOrDefaultAsync(t => t.Id == tenantId);

        ArgumentNullException.ThrowIfNull(tenant);

        return tenant;
    }

    public async Task<Models.Tenant?> GetTenantByNameAsync(string tenantName, CancellationToken cancellationToken)
    {
        var tenant = await _tenantDbContext.Tenants
             .FirstOrDefaultAsync(t => t.Name == tenantName, cancellationToken);

        return tenant;
    }
}
