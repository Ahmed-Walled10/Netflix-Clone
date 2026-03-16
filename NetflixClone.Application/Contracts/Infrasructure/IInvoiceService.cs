using NetflixClone.Domain.Entities.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetflixClone.Application.Contracts.Infrasructure
{
    public interface IInvoiceService
    {
        Task<Invoice> CreateInvoiceAsync(string userId, string subscriptionId, decimal amount);
    }
}
