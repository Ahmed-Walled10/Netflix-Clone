using MediatR;
using Microsoft.AspNetCore.Identity;
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
            var User = await _userManager.FindByEmailAsync(request.Email);
            if (User == null || !User.EmailConfirmed)
            {
                return true;
            }

            var otp = _otpService.GenerateOtp();
            User.PasswordResetOtp = otp;
            User.PasswordResetOtpExpiration = DateTime.UtcNow.AddMinutes(15);
            var result = await _userManager.UpdateAsync(User);
            if (!result.Succeeded)
            {
                throw new Exception("Failed to update user with password reset OTP.");
            }
            await _emailService.SendPasswordResetOtpAsync(
                User.Email!,
                User.FirstName,
                otp);

            return true;

        }
    }
}
