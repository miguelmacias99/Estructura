namespace Plantilla.Infraestructura.Modelo.Configuracion
{
    public class ConfiguracionIdentity
    {
        public SignInSettings SignIn { get; set; } = null!;
        public UserSettings User { get; set; } = null!;
        public PasswordSettings Password { get; set; } = null!;
        public LockoutSettings Lockout { get; set; } = null!;
        public TokenSettings Tokens { get; set; } = null!;
        public int TokenLifespanMinutes { get; set; }

        public class SignInSettings
        {
            public bool RequireConfirmedEmail { get; set; }
        }

        public class UserSettings
        {
            public bool RequireUniqueEmail { get; set; }
        }

        public class PasswordSettings
        {
            public bool RequireDigit { get; set; }
            public bool RequireLowercase { get; set; }
            public bool RequireUppercase { get; set; }
            public bool RequireNonAlphanumeric { get; set; }
            public int RequiredLength { get; set; }
        }

        public class LockoutSettings
        {
            public int DefaultLockoutTimeSpanMinutes { get; set; }
            public int MaxFailedAccessAttempts { get; set; }
            public bool AllowedForNewUsers { get; set; }
        }

        public class TokenSettings
        {
            public string EmailConfirmationTokenProvider { get; set; } = string.Empty;
            public string PasswordResetTokenProvider { get; set; } = string.Empty;
            public string AuthenticatorTokenProvider { get; set; } = string.Empty;
        }
    }
}