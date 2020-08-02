using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RegistrationApp.Messaging.Models;
using RegistrationAppDAL.Data;

namespace RegistrationApp.Messaging.Queries.GetPairingsForWeek
{
    public class GetPairingsForWeekQueryHandler : IRequestHandler<GetPairingsForWeekQuery, List<UserResponseModel>>
    {
        private readonly ApplicationDbContext _context;

        public async Task<List<UserResponseModel>> Handle(GetPairingsForWeekQuery request, CancellationToken cancellationToken)
        {
            var dancersAttended = await
                _context.Users.Where(x =>
                    x.FormerMatches.Exists(y =>
                        y.DateDanced >= request.Date.AddDays(-6) && y.DateDanced <= request.Date)).ToListAsync(cancellationToken);

            var result = new List<UserResponseModel>();

            foreach (var dancer in dancersAttended)
            {
                if (result.Exists(x => x.Id == dancer.Id || x.Match?.Id == dancer.Id))
                {
                    continue;
                }

                var formerMatch = dancer.FormerMatches
                        .Find(
                            x => x.DateDanced >= request.Date.AddDays(-6) && x.DateDanced <= request.Date)!
                    .PartnerId;

                var partner = await _context.Users.FindAsync(formerMatch, cancellationToken);

                if (partner == null)
                {
                    throw new InvalidOperationException("Partner not found in DB");
                }

                var dancerResponse = new UserResponseModel(
                    dancer.Id, 
                    dancer.NormalizedUserName, 
                    dancer.Email,
                    dancer.PhoneNumber);

                dancerResponse.Match = new UserResponseModel(
                    partner.Id, 
                    partner.NormalizedUserName, 
                    partner.Email,
                    partner.PhoneNumber)
                {
                    Match = dancerResponse
                };

                result.Add(dancerResponse);
            }
            return result;
        }

        public GetPairingsForWeekQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}