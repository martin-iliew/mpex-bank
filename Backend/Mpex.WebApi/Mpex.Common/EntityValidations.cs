namespace Mpex.Common
{
    public static class EntityValidations
    {
        public static class AppUser
        {
            public const int FirstNameMinLength = 2;
            public const int FirstNameMaxLength = 50;

            public const int LastNameMinLength = 2;
            public const int LastNameMaxLength = 100;

            public const int ImageUrlMaxLength = 2048;

        }
        public static class GeneralApplicationConstants
        {
            public const string AdminRoleName = "Administrator";
            public const string DevelopmentAdminEmail = "admin@boardtschek.com";

            public const string AdminAreaName = "Admin";
        }
    }
}
