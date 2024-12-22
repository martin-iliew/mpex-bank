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
        public static class Admin
        {
            public const string AdminRoleName = "Administrator";
            public const string AdminAreaName = "Admin";
            public const string NormalizedAreaName = "ADMIN";
            public const string DevelopmentAdminEmail = "admin@mpex.com";
            public const string NormalizedDevelopmentAdminEmail = "ADMIN@MPEX.COM";
        }
    }
}
