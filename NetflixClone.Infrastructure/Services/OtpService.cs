using NetflixClone.Application.Contracts.Infrasructure;
using System.Security.Cryptography;

namespace NetflixClone.Infrastructure.Services
{
    public class OtpService : IOtpService
    {
        public string GenerateOtp()
        {
            // Use a cryptographically secure RNG — predictable Random() is not safe for OTPs
            return RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
        }

        public bool ValidateOtp(string providedOtp, string storedOtp, DateTime? expiresAt)
        {
            if (string.IsNullOrEmpty(providedOtp) || string.IsNullOrEmpty(storedOtp))
                return false;

            if (expiresAt == null || DateTime.UtcNow > expiresAt)
                return false;

            return providedOtp == storedOtp;
        }
    }
}
