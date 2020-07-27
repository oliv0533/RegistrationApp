using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RegistrationAppDAL.Data;
using RegistrationAppDAL.Models;

namespace RegistrationApp.Messaging.Commands.SetPartnerForCouple
{
    public class SetPartnerForCoupleCommandHandler : IRequestHandler<SetPartnerForCoupleCommand>
    {
        private readonly ApplicationDbContext _context;

        public Task<Unit> Handle(SetPartnerForCoupleCommand request, CancellationToken cancellationToken)
        {
            var male = _context.Users.Find(request.MaleId);
            var female = _context.Users.Find(request.FemaleId);

            if (male == null || female == null)
            {
                throw new InvalidOperationException("Male or female was null");
            }

            male.FormerMatches.Add(new FormerMatches(female.Id, DateTime.Today));
            female.FormerMatches.Add(new FormerMatches(male.Id, DateTime.Today));

            return Unit.Task;
        }

        public SetPartnerForCoupleCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}