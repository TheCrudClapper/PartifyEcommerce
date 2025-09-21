namespace CSOS.Core.ResultTypes
{
    public sealed record Error(string ErrorCode, string Description)
    {
        public static readonly Error None = new(string.Empty, string.Empty);
        public static readonly Error NullValue = new("Error.NullValue", "Null value");
    }
}
