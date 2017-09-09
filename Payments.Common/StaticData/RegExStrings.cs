namespace Payments.Common.StaticData
{
    // validation regex string
    public static class RegExStrings
    {
        public const string Email = @"^[a-zA-Z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$";
        public const string Password = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!_@#$%^&'])[^ ]{8,}$";
        public const string Name = @"^[A-Z][A-Za-z- ]*$";
        public const string Phone = @"^380[\d]{9}$";
        public const string HolderName = @"^([A-Z][a-z]+ ?){2,}$";
        public const string SumNumber = @"^\d+(\.\d{1,2})?$";
        public const string Recipient = @"^\d{1,10}$";
    }
}