using ErrorOr;

namespace Prod.Domain.Common.Errors;

public static partial class Errors
{
    public static class Country
    {
        public static Error InvalidRegions()
            => Error.Validation(
                code: "Countries.InvalidRegions",
                description: "Invalid regions in request");

        public static Error NotFoundRegion()
            => Error.NotFound(
                code: "Countries.NotFoundRegion",
                description: "Region not found");
    }
}