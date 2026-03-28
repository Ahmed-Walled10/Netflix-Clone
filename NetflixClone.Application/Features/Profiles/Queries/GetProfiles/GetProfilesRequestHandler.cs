using AutoMapper;
using MediatR;
using NetflixClone.Application.Contracts.Persistence;
using System.Security.Claims;

namespace NetflixClone.Application.Features.Profiles.Queries.GetProfiles;

public class GetProfilesRequestHandler : IRequestHandler<GetProfilesRequest, List<GetProfilesResponse>>
{
    private readonly IProfileRepository _profileRepository;
    private readonly IMapper _mapper;

    public GetProfilesRequestHandler(
        IProfileRepository profileRepository,
        IMapper mapper)
    {
        _profileRepository = profileRepository;
        _mapper = mapper;
    }

    public async Task<List<GetProfilesResponse>> Handle(
        GetProfilesRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _profileRepository.GetUserWithProfilesAsync(ClaimTypes.NameIdentifier);
        if (user is null)
            throw new UnauthorizedAccessException("User account not found.");

        return _mapper.Map<List<GetProfilesResponse>>(user.Profiles);
    }
}
