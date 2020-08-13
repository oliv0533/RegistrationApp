using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RegistrationApp.Messaging.Models;
using RegistrationApp.Messaging.Queries.GetRandomPairingsOfAttendingUsersWithLevel;
using RegistrationAppDAL.Models;

namespace RegistrationApp.Messaging.Queries.GetAllRandomPairingsForTheWeek
{
    public class GetAllRandomPairingsForTheWeekQueryHandler : IRequestHandler<GetAllRandomPairingsForTheWeekQuery, List<LevelPairingsModel>>
    {
        private readonly IMediator _mediator;

        public async Task<List<LevelPairingsModel>> Handle(GetAllRandomPairingsForTheWeekQuery request, CancellationToken cancellationToken)
        {
            var result = new List<LevelPairingsModel>();

            await AddDancersFromLevel(result, new List<string>{Level.Theme, Level.Advanced},  cancellationToken);
            await AddDancersFromLevel(result, new List<string> {Level.Theme}, cancellationToken);
            await AddDancersFromLevel(result, new List<string> { Level.Advanced }, cancellationToken);
            await AddDancersFromLevel(result, new List<string> { Level.Novice }, cancellationToken);
            await AddDancersFromLevel(result, new List<string> { Level.Beginner }, cancellationToken);

            return result;
        }

        private async Task AddDancersFromLevel(
            ICollection<LevelPairingsModel> result,
            List<string> levels,
            CancellationToken cancellationToken)
        {
            var dancers =
                await _mediator.Send(
                    new GetRandomPairingsOfAttendingUsersWithLevelQuery(levels),
                    cancellationToken);

            result.Add(dancers);
        }

        public GetAllRandomPairingsForTheWeekQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}