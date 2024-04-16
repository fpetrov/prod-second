using Prod.Domain.Entities;

namespace Prod.Contracts.Countries;

public record FilterCountriesResponse(
    List<Country> Countries);