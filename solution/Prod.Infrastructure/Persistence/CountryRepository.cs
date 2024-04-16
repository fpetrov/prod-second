using Microsoft.EntityFrameworkCore;
using Prod.Application.Common.Interfaces.Persistence;
using Prod.Domain.Entities;

namespace Prod.Infrastructure.Persistence;

public class CountryRepository(
    ApplicationContext context)
    : ICountryRepository
{
    private readonly HashSet<string> _allowedRegions = 
        [ "Europe", "Africa", "Americas", "Oceania", "Asia" ];
    
    public async Task<IEnumerable<Country>> GetAllByRegions(
        List<string> regions,
        CancellationToken cancellationToken = default)
    {
        if (regions.Any(region => !_allowedRegions.Contains(region)))
            return Enumerable.Empty<Country>();
        
        return await context
            .Set<Country>()
            .Where(country => regions.Contains(country.Region))
            .ToListAsync(cancellationToken: cancellationToken);
    }
    
    public async Task<Country?> GetByAlpha2(
        string alpha2,
        CancellationToken cancellationToken)
    {
        return await context
            .Set<Country>()
            .FirstOrDefaultAsync(country => country.Alpha2 == alpha2, cancellationToken: cancellationToken);
    }
}