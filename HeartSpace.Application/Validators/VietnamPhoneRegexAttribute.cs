using System.ComponentModel.DataAnnotations;

namespace HeartSpace.Application.Validators
{
    public class VietnamPhoneRegexAttribute : RegularExpressionAttribute
    {
        // Pattern cho số điện thoại VN - CHỈ 10 chữ số cho local format
        private const string Pattern = @"^0(3[2-9]|5[2689]|7[06-9]|8[1-9]|9[0-9]|1[2-9]|2[0-9])\d{7}$|^(\+84|84)(3[2-9]|5[2689]|7[06-9]|8[1-9]|9[0-9]|1[2-9]|2[0-9])\d{7}$";

        public VietnamPhoneRegexAttribute() : base(Pattern)
        {
            ErrorMessage = "Số điện thoại không đúng định dạng Việt Nam. Định dạng hợp lệ: 0987654321 (10 chữ số) hoặc +84987654321";
        }
    }
}
