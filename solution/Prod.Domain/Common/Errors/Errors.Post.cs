using ErrorOr;

namespace Prod.Domain.Common.Errors;

public static partial class Errors
{
    public static class Post
    {
        public static Error NotFound
            => Error.NotFound(
                code: "Post.NotFound",
                description: "Post not found.");
    }
}