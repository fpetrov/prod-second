using ErrorOr;
using MediatR;
using Prod.Application.Common.Interfaces.Persistence;
using Prod.Domain.Common.Errors;
using Prod.Domain.Entities;

namespace Prod.Application.Countries.Queries;

public class GetCountryByCodeHandler(
    ICountryRepository countryRepository)
    : IRequestHandler<GetCountryByCodeQuery, ErrorOr<Country>>
{
    public async Task<ErrorOr<Country>> Handle(
        GetCountryByCodeQuery query,
        CancellationToken cancellationToken)
    {
        var alpha2 = query.Alpha2.ToUpper();
        
        var country = await countryRepository
            .GetByAlpha2(alpha2, cancellationToken);
        
        if (country is null)
            return Errors.Country.NotFoundRegion();
        
        return country;
    }
}

public record GetCountryByCodeQuery(string Alpha2)
    : IRequest<ErrorOr<Country>>;