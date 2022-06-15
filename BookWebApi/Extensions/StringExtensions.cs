namespace BookWebApi.Extensions;
public static class StringExtensions
{
    public static bool StartEndOrEqual(this string @this, string that)
    {
        return @this.ToLowerInvariant().StartsWith(that.ToLowerInvariant()) ||
            @this.ToLowerInvariant().EndsWith(that.ToLowerInvariant()) ||
            @this.Equals(that, StringComparison.InvariantCultureIgnoreCase);
    }
}