using HeartSpace.Application.Validators;
using System.ComponentModel.DataAnnotations;

namespace HeartSpace.Application.Helpers
{
    public static class LoginTypeHelper
    {
        private static readonly EmailAddressAttribute _emailValidator = new();
        private static readonly VietnamPhoneRegexAttribute _phoneValidator = new();

        public static LoginType GetLoginType(string keyLogin)
        {
            if (string.IsNullOrWhiteSpace(keyLogin))
                return LoginType.Unknown;

            keyLogin = keyLogin.Trim();

            // Check email first
            if (_emailValidator.IsValid(keyLogin))
                return LoginType.Email;

            // Then check phone
            if (_phoneValidator.IsValid(keyLogin))
                return LoginType.Phone;

            return LoginType.Unknown;
        }

        public static bool IsValidEmailOrPhone(string keyLogin)
        {
            return GetLoginType(keyLogin) != LoginType.Unknown;
        }

        public static string GetValidationMessage(string keyLogin)
        {
            var loginType = GetLoginType(keyLogin);
            return loginType switch
            {
                LoginType.Email => "Email hợp lệ",
                LoginType.Phone => "Số điện thoại hợp lệ",
                LoginType.Unknown => "Email hoặc số điện thoại không đúng định dạng",
                _ => "Không xác định được định dạng"
            };
        }
    }

    public enum LoginType
    {
        Email,
        Phone,
        Unknown
    }
}
