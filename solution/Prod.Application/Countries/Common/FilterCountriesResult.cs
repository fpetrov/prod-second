using Prod.Domain.Entities;

namespace Prod.Application.Countries.Common;

public record FilterCountriesResult(
    List<Country> Countries);