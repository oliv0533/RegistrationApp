using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RegistrationAppDAL.Data;
using RegistrationAppDAL.Models;

namespace RegistrationApp.Messaging.Commands.UpdateUser
{
    public class UpdateUserDetailsCommandHandler : IRequestHandler<UpdateUserDetailsCommand>
    {
        private readonly ApplicationDbContext _context;

        public async Task<Unit> Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.FindAsync<ApplicationUser>(request.UserId);
            user.Levels = request.Levels;
            user.Gender = request.Gender;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }


        public UpdateUserDetailsCommandHandler (ApplicationDbContext context)
        {
            _context = context;
        }
    }
}