namespace Prod.Contracts.Countries;

public record FilterCountriesRequest(
    List<string> Regions);