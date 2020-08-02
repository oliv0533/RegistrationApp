using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RegistrationAppDAL.Data;

namespace RegistrationApp.Messaging.Commands.SetAllAttendingToNull
{
    public class AllAttendingToNullCommandHandler : IRequestHandler<AllAttendingToNullCommand, Unit>
    {
        private readonly ApplicationDbContext _context;

        public Task<Unit> Handle(AllAttendingToNullCommand request, CancellationToken cancellationToken)
        {
            foreach (var applicationUser in _context.Users)
            {
                applicationUser.Attending = null;
            }

            _context.SaveChangesAsync(cancellationToken);
            return Task.FromResult(Unit.Value);
        }

        public AllAttendingToNullCommandHandler(ApplicationDbContext context)
        {
            this._context = context;
        }
    }
}