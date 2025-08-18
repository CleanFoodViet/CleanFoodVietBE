namespace CleanFoodVietAPI.Data.Exceptions
{
    public class UpdateRestrictedException : Exception
    {
        public UpdateRestrictedException()
        {
        }

        public UpdateRestrictedException(string? message) : base(message)
        {
        }
    }
}
