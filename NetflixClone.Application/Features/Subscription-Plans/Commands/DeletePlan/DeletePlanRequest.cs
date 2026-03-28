using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetflixClone.Application.Features.Subscription_Plans.Commands.DeletePlan
{
    public class DeletePlanRequest : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
