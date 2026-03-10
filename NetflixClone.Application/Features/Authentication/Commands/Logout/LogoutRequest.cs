using MediatR;

namespace NetflixClone.Application.Features.Authentication.Commands.Logout
{
    public class LogoutRequest : IRequest<bool>
    {
    }
}
