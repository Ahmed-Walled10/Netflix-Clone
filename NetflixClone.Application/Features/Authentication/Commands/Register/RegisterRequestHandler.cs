using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using NetflixClone.Application.Contracts;
using NetflixClone.Application.Contracts.Infrasructure;
using NetflixClone.Domain.Entities.Identity;

namespace NetflixClone.Application.Features.Authentication.Commands.Register
{
    public class RegisterRequestHandler : IRequestHandler<RegisterRequest, RegisterResponce>
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOtpService _otpService;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IJwtTokenGeneration _jwtTokenGeneration;
        public RegisterRequestHandler(IJwtTokenGeneration jwtTokenGeneration, 
                                      UserManager<ApplicationUser> userManager,
                                      IOtpService otpService,
                                      IEmailService emailService,
                                      IMapper mapper)
        {
            _jwtTokenGeneration = jwtTokenGeneration;
            _userManager = userManager;
            _mapper = mapper;
            _otpService = otpService;
            _emailService = emailService;

        }

        public async Task<RegisterResponce> Handle(RegisterRequest request, CancellationToken cancellationToken)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new Exception($"Email '{request.Email}' is already registered.");
            }

            var otp = _otpService.GenerateOtp();
            var user = _mapper.Map<ApplicationUser>(request);
            user.EmailConfirmationOtp = otp;
            user.EmailConfirmationOtpExpiration = DateTime.UtcNow.AddMinutes(20);

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new Exception($"Registration failed: {errors}");
            }

            await _userManager.AddToRoleAsync(user, "NotSubscriber");
            await _emailService.SendEmailConfirmationOtpAsync(
                user.Email!,
                user.FirstName,
                otp);

            var token = _jwtTokenGeneration.GenerateJwtToken(user, new List<string> { "NotSubscriber" });

            return new RegisterResponce
            {
                Token = token,
                Email = user.Email!,
                FullName = $"{user.FirstName} {user.LastName}",
                Roles = new List<string> { "NotSubscriber" }
            };
        }
    }
}
