namespace Payments.Common.StaticData
{
    public static class RegExStrings
    {
        public const string Email = @"^[a-zA-Z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$";
        public const string Password = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!_@#$%^&'])[^ ]{8,}$";
        public const string Name = @"^[A-Z][A-Za-z- ]*$";
        public const string Phone = @"^380[\d]{9}$";
    }
}