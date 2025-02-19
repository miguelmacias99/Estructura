namespace Plantilla.Entidad.Modelo.Identity
{
    public enum TwoFactorType
    {
        None = 0,          // Sin 2FA
        Sms = 1,           // Autenticación vía SMS
        Email = 2,         // Autenticación vía correo electrónico
        Authenticator = 3,  // Autenticación vía aplicación de autenticación (Google Authenticator, etc.)
        HardwareToken = 4, // Autenticación vía token hardware
        Biometric = 5      // Autenticación biométrica (huella dactilar, reconocimiento facial)
    }
}