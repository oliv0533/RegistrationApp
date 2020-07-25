using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RegistrationApp.Messaging.Models;
using RegistrationAppDAL.Data;

namespace RegistrationApp.Messaging.Queries.GetAllUsersFromDatabase
{
    public class GetAllUsersWithLevelQueryHandler : IRequestHandler<GetAllUsersWithLevelQuery, List<UserResponseModel>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllUsersWithLevelQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<UserResponseModel>> Handle(GetAllUsersWithLevelQuery request, CancellationToken cancellationToken)
        {
            return _context.Users
                .Where(x => x.Level == request.Level)
                .Select(x =>
                    new UserResponseModel(
                        x.Id, 
                        x.NormalizedUserName, 
                        x.Email, 
                        x.PhoneNumber))
                .ToListAsync(cancellationToken);

        }
    }
}