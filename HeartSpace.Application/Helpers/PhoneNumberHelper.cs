namespace HeartSpace.Application.Helpers
{
    public static class PhoneNumberHelper
    {
        public static string NormalizePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new ArgumentException("Phone number cannot be null or empty.", nameof(phoneNumber));
            }
            // Remove all non-digit characters
            var digitsOnly = new string(phoneNumber.Where(char.IsDigit).ToArray());
            // Handle country code (assuming +84 for Vietnam as an example)
            if (digitsOnly.StartsWith("84"))
            {
                digitsOnly = "0" + digitsOnly.Substring(2);
            }
            else if (digitsOnly.StartsWith("+84"))
            {
                digitsOnly = "0" + digitsOnly.Substring(3);
            }
            else if (!digitsOnly.StartsWith("0"))
            {
                // If it doesn't start with 0, assume it's a local number and prepend 0
                digitsOnly = "0" + digitsOnly;
            }
            // Validate length (assuming standard length of 10 or 11 digits for local numbers)
            if (digitsOnly.Length < 10 || digitsOnly.Length > 11)
            {
                throw new ArgumentException("Invalid phone number length after normalization.", nameof(phoneNumber));
            }
            return digitsOnly;
        }
    }
}
