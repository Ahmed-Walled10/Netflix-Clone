using MediatR;
using Microsoft.AspNetCore.Identity;
using NetflixClone.Application.Contracts;
using NetflixClone.Application.Contracts.Infrasructure;
using NetflixClone.Domain.Entities.Identity;

namespace NetflixClone.Application.Features.Authentication.Commands.ForgotPassword
{
    public class ForgotPasswordRequestHandler : IRequestHandler<ForgotPasswordRequest, bool>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IOtpService _otpService;
        public ForgotPasswordRequestHandler(UserManager<ApplicationUser> userManager, IEmailService emailService , IOtpService otpService)
        {
            _userManager = userManager;
            _emailService = emailService;
            _otpService = otpService;
        }
        public async Task<bool> Handle(ForgotPasswordRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !user.EmailConfirmed)
            {
                // Return true regardless to prevent email enumeration attacks
                return true;
            }

            var otp = _otpService.GenerateOtp();
            user.PasswordResetOtp = otp;
            user.PasswordResetOtpExpiration = DateTime.UtcNow.AddMinutes(15);
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception("Failed to save password reset OTP. Please try again.");
            }
            await _emailService.SendPasswordResetOtpAsync(
                user.Email!,
                user.FirstName,
                otp);

            return true;

        }
    }
}
