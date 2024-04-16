using ErrorOr;
using MediatR;
using Prod.Application.Common.Interfaces.Persistence;
using Prod.Domain.Common.Errors;
using Prod.Domain.Entities;

namespace Prod.Application.Countries.Queries;

public class FilterCountriesHandler(
    ICountryRepository countryRepository)
    : IRequestHandler<FilterCountriesQuery, ErrorOr<IEnumerable<Country>>>
{
    public async Task<ErrorOr<IEnumerable<Country>>> Handle(
        FilterCountriesQuery query,
        CancellationToken cancellationToken)
    {
        if (query.Regions.Count == 0)
            return Errors.Country.InvalidRegions();
        
        var countries = (await countryRepository
            .GetAllByRegions(query.Regions, cancellationToken)).ToList();
        
        if (countries.Count == 0)
            return Errors.Country.InvalidRegions();
        
        return countries
            .OrderBy(c => c.Alpha2)
            .ToList();
    }
}

public record FilterCountriesQuery(
    List<string> Regions)
    : IRequest<ErrorOr<IEnumerable<Country>>>;