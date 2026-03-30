namespace NetflixClone.Application.Contracts.Infrasructure
{
    public interface IOtpService
    {
        string GenerateOtp();
        bool ValidateOtp(string providedOtp, string storedOtp, DateTime? expiresAt);

    }
}
