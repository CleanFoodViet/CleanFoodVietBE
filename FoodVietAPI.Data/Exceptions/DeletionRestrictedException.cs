namespace CleanFoodVietAPI.Data.Exceptions
{
    public class DeletionRestrictedException : Exception
    {
        public DeletionRestrictedException(string message) : base(message)
        {
            
        }
    }
}
