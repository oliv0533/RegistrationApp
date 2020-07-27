using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RegistrationAppDAL.Data;

namespace RegistrationApp.Messaging.Commands.SetAttendingToNull
{
    public class SetAttendingToNullCommandHandler : IRequestHandler<SetAttendingToNullCommand>
    {
        private readonly ApplicationDbContext _context;

        public Task<Unit> Handle(SetAttendingToNullCommand request, CancellationToken cancellationToken)
        {
            var user = _context.Users.Find(request.Id);

            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            user.Attending = null;

            return Unit.Task;
        }

        public SetAttendingToNullCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}