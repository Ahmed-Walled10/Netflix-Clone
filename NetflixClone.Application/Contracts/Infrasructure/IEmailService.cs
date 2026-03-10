namespace NetflixClone.Application.Contracts.Infrasructure;

public interface IEmailService
{
    Task SendEmailConfirmationOtpAsync(string email, string firstName, string otp);
    Task SendPasswordResetOtpAsync(string email, string firstName, string otp);
}