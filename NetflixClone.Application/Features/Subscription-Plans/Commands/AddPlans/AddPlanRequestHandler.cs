using AutoMapper;
using MediatR;
using NetflixClone.Application.Persistence;
using NetflixClone.Domain.Entities.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetflixClone.Application.Features.Subscription_Plans.Commands.AddPlans
{
    public class AddPlanRequestHandler : IRequestHandler<AddPlanRequest, AddPlanResponse>
    {
        private readonly IBaseRepository<Plan> _planRepo;
        private readonly IMapper _mapper;
        public AddPlanRequestHandler(IBaseRepository<Plan> planRepo, IMapper mapper)
        {
            _planRepo = planRepo;
            _mapper = mapper;
        }
        public async Task<AddPlanResponse> Handle(AddPlanRequest request, CancellationToken cancellationToken)
        {
            var plan = _mapper.Map<Plan>(request);

            await _planRepo.AddAsync(plan, cancellationToken);
            await _planRepo.SaveChangesAsync(cancellationToken);

            return _mapper.Map<AddPlanResponse>(plan);

        }
    }
}
