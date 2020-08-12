using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RegistrationAppDAL.Data;
using RegistrationAppDAL.Models;

namespace RegistrationApp.Messaging.Commands.SetUserAttending
{
    public class SetUserAttendingCommandHandler : IRequestHandler<SetUserAttendingCommand>
    {
        private readonly ApplicationDbContext _context;

        public async Task<Unit> Handle(SetUserAttendingCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(request.UserId, cancellationToken);

            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }
            
            user.Attending = request.Attending;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        public SetUserAttendingCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}