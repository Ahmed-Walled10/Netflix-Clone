using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetflixClone.Application.Features.Authentication.Commands.Logout;
using NetflixClone.Application.Features.Authentication.Confirmations.EmailConfirmations;
using NetflixClone.Application.Features.Authentication.Confirmations.ResendEmailConfirmationOtp;
using NetflixClone.Application.Features.Authentication.Commands.ForgotPassword;
using NetflixClone.Application.Features.Authentication.Commands.ResetPassword;
using NetflixClone.Application.Features.Authentication.Commands.Register;
using NetflixClone.Application.Features.Authentication.Commands.Login;

namespace NetflixClone.Api.Controller
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            var result = await _mediator.Send(registerRequest);

            return Ok(new
            {
                message = "Registration successful! Please check your email for OTP to confirm your account.",
                data = result
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var result = await _mediator.Send(loginRequest);
            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(LogoutRequest logoutRequest)
        {
            var result = await _mediator.Send(logoutRequest);
            return Ok(result);
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmationRequest emailConfirmationRequest)
        {
            var result = await _mediator.Send(emailConfirmationRequest);
            if (!result)
            {
                return BadRequest(new { message = "Invalid Or Expired Otp , Please try again" });
            }

            return Ok();
        }

        [HttpPost("resend-confirmation-otp")]
        public async Task<IActionResult> ResendConfirmationOtp(ResendEmailConfirmationOtpRequest
            resendEmailConfirmationOtpRequest)
        {
            var result = await _mediator.Send(resendEmailConfirmationOtpRequest);
            if (!result)
            {
                return BadRequest(new
                { message = "Could not resend OTP. Email may already be confirmed or does not exist." });
            }

            return Ok(new { message = "OTP resent successfully! Please check your email." });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest forgotPasswordRequest)
        {
            var result = await _mediator.Send(forgotPasswordRequest);
            if (!result)
            {
                return BadRequest(new { message = "Could not forgot password. Please try again." });
            }

            return Ok(new { message = "If your email exists, you will receive a password reset OTP shortly." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest resetPasswordRequest)
        {
            var result = await _mediator.Send(resetPasswordRequest);

            if (!result)
            {
                return BadRequest(new
                { message = "Invalid or expired OTP. Please try again or request a new password reset." });
            }

            return Ok(new { message = "Password reset successfully! You can now login with your new password." });
        }


    }
}
