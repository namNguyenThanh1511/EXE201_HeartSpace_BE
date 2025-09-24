using System.ComponentModel.DataAnnotations;

namespace HeartSpace.Application.Validators
{
    public class EmailOrVietnamPhoneAttribute : ValidationAttribute
    {
        private readonly EmailAddressAttribute _emailValidator;
        private readonly VietnamPhoneRegexAttribute _phoneValidator;

        public EmailOrVietnamPhoneAttribute() : base("Vui lòng nhập email hợp lệ hoặc số điện thoại Việt Nam hợp lệ")
        {
            _emailValidator = new EmailAddressAttribute();
            _phoneValidator = new VietnamPhoneRegexAttribute();
        }

        public override bool IsValid(object? value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return false;

            string input = value.ToString()!.Trim();

            // Validate as email first
            bool isValidEmail = _emailValidator.IsValid(input);

            // If not valid email, try phone validation
            if (!isValidEmail)
            {
                bool isValidPhone = _phoneValidator.IsValid(input);
                return isValidPhone;
            }

            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return "Email hoặc số điện thoại không đúng định dạng. " +
                   "Email: example@domain.com, " +
                   "SĐT: 0987654321 hoặc +84987654321";
        }
    }
}
