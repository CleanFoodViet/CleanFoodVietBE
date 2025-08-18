using System.Collections.Concurrent;

namespace CleanFoodVietAPI.Application.Utils
{
    public static class OtpStore
    {
        private static readonly ConcurrentDictionary<string, (string Otp, DateTime ExpireAt)> _store = new();

        public static void Set(string phoneNumber, string otp)
        {
            _store[phoneNumber] = (otp, DateTime.UtcNow.AddMinutes(5));
        }

        public static bool Validate(string phoneNumber, string otp)
        {
            if (_store.TryGetValue(phoneNumber, out var entry))
            {
                if (entry.ExpireAt > DateTime.UtcNow && entry.Otp == otp)
                {
                    _store.TryRemove(phoneNumber, out _);
                    return true;
                }
            }
            return false;
        }
    }
}
