using Prod.Domain.Entities;

namespace Prod.Application.Common.Interfaces.Persistence;

public interface ICountryRepository
{
    public Task<IEnumerable<Country>> GetAllByRegions(List<string> regions, CancellationToken cancellationToken = default);
    public Task<Country?> GetByAlpha2(string alpha2, CancellationToken cancellationToken = default);
}