namespace CleanFoodVietAPI.Data.Enums.ServiceFeatureEnums
{
    public enum ServiceFeatureStatusEnum
    {
        ACTIVE,
        INACTIVE,
        PENDING
    }

    public enum ServiceFeatureActionEnum
    {
        POST_QUOTA,      // Used for limiting the number of posts (quota)
        POST_PRIORITY,   // Used for sorting posts by priority
        IMAGE_LIMIT      // Used for limiting the max number of images per post
        // Add more actions as needed
    }
}
