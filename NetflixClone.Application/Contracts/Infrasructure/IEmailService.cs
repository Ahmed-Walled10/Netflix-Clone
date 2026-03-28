namespace NetflixClone.Application.Contracts.Infrasructure;

public interface IEmailService
{
    Task SendEmailConfirmationOtpAsync(string email, string firstName, string otp);
    Task SendPasswordResetOtpAsync(string email, string firstName, string otp);
    Task SendInvoiceEmailAsync(string email, string firstName, string planName, decimal amount, string cardBrand, string cardLast4, DateTime paymentDate, DateTime periodEnd);
}