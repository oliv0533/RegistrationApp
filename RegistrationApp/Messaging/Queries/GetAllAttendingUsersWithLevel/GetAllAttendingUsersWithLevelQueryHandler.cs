using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RegistrationApp.Messaging.Models;
using RegistrationAppDAL.Data;
using RegistrationAppDAL.Models;

namespace RegistrationApp.Messaging.Queries.GetAllAttendingUsersWithLevel
{
    public class GetAllAttendingUsersWithLevelQueryHandler : IRequestHandler<GetAllAttendingUsersWithLevelQuery, List<ApplicationUser>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllAttendingUsersWithLevelQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<ApplicationUser>> Handle(GetAllAttendingUsersWithLevelQuery request, CancellationToken cancellationToken)
        {
            var users = _context.Users
                .Where(x => x.Attending != null && x.Attending.Levels.All(request.Levels.Contains));
            return users.ToListAsync(cancellationToken);
        }
    }
} 