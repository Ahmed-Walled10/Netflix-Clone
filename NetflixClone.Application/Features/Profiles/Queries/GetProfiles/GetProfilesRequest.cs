using MediatR;

namespace NetflixClone.Application.Features.Profiles.Queries.GetProfiles;

public class GetProfilesRequest : IRequest<List<GetProfilesResponse>>
{
}
