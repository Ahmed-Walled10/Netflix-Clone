using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NetflixClone.Application.Contracts;
using NetflixClone.Domain.Entities.Identity;

namespace NetflixClone.Application.Features.Authentication.Commands.ResetPassword
{
    public class ResetPasswordRequestHandler : IRequestHandler<ResetPasswordRequest, bool>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOtpService _otpService;
        public ResetPasswordRequestHandler(UserManager<ApplicationUser> userManager, IOtpService otpService)
        {
            _userManager = userManager;
            _otpService = otpService;
        }

        public async Task<bool> Handle(ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return false;

            if (user.OtpAttempts >= 10 &&
            user.LastOtpAttemptAt.HasValue &&
            DateTime.UtcNow < user.LastOtpAttemptAt.Value.AddMinutes(15))
            {
                return false;
            }

            if (!user.LastOtpAttemptAt.HasValue ||
                DateTime.UtcNow >= user.LastOtpAttemptAt.Value.AddMinutes(15))
            {
                user.OtpAttempts = 0;
            }

            user.OtpAttempts++;
            user.LastOtpAttemptAt = DateTime.UtcNow;

            if (!_otpService.ValidateOtp(request.Otp, user.PasswordResetOtp, user.PasswordResetOtpExpiration))
            {
                Console.WriteLine("OTP is Incorrect");
                await _userManager.UpdateAsync(user);
                return false;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);

            if (!result.Succeeded)
            {
                return false;
            }

            user.PasswordResetOtp = null;
            user.PasswordResetOtpExpiration = null;
            user.OtpAttempts = 0;
            user.LastOtpAttemptAt = null;
            await _userManager.UpdateAsync(user);
            return true;

        }
    }
}
