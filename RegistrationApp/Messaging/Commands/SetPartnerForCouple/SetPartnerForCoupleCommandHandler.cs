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

        public async Task<Unit> Handle(SetPartnerForCoupleCommand request, CancellationToken cancellationToken)
        {
            var male = await _context.Users.FindAsync(request.MaleId, cancellationToken);
            var female = await _context.Users.FindAsync(request.FemaleId, cancellationToken);

            var today = DateTime.Today;

            if (male == null || female == null)
            {
                throw new InvalidOperationException("Male or female was null");
            }

            male.FormerMatches.Add(new FormerMatch(female.Id, today));
            female.FormerMatches.Add(new FormerMatch(male.Id, today));

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        public SetPartnerForCoupleCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}