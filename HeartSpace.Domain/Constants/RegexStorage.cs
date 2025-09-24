namespace HeartSpace.Domain.Constants
{
    public interface RegexStorage
    {
        //email pattern
        public const string EMAIL_PATTERN = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        //vietnamese phone number pattern
        public const string PHONE_VN_PATTERN = @"^(\+84|84|0)((3[2-9])|(5[2689])|(7[06-9])|(8[1-9])|(9[0-46-9]))\d{7}$";

    }
}
