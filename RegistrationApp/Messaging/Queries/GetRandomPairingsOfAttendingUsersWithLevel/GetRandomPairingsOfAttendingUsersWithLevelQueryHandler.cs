using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RegistrationApp.Messaging.Commands.SetPartnerForCouple;
using RegistrationApp.Messaging.Models;
using RegistrationApp.Messaging.Queries.GetAllAttendingUsersWithLevel;
using RegistrationAppDAL.Models;

namespace RegistrationApp.Messaging.Queries.GetRandomPairingsOfAttendingUsersWithLevel
{
    public class GetRandomPairingsOfAttendingUsersWithLevelQueryHandler : IRequestHandler<GetRandomPairingsOfAttendingUsersWithLevelQuery, List<UserResponseModel>>
    {
        private readonly IMediator _mediator;

        private List<ApplicationUser> _attendingMen;
        private List<ApplicationUser> _attendingWomen;

        public async Task<List<UserResponseModel>> Handle(GetRandomPairingsOfAttendingUsersWithLevelQuery request, CancellationToken cancellationToken)
        {
            var attendingUsers = await _mediator.Send(new GetAllAttendingUsersWithLevelQuery(request.Levels), cancellationToken);
            _attendingMen = GetAttendingUsersByGenderOrderedByDate(DanceGender.Male, attendingUsers);
            _attendingWomen = GetAttendingUsersByGenderOrderedByDate(DanceGender.Female, attendingUsers);
            
            LimitAmountOfAttendeesAndSortByMatches();

            FindPreferences(_attendingMen, _attendingWomen, out var malePreferenceModels);
            FindPreferences(_attendingWomen, _attendingMen, out var femalePreferenceModels);

            var maleResponseModels = GetResponseModelsForGender(_attendingMen);
            var femaleResponseModels = GetResponseModelsForGender(_attendingWomen);


            FindBestMatchesForAttendees(maleResponseModels, malePreferenceModels, femaleResponseModels, femalePreferenceModels);

            await SetPartnersForNewlyMatchedDancers(cancellationToken, maleResponseModels);

            return maleResponseModels.Concat(femaleResponseModels).ToList();
        }

        private async Task SetPartnersForNewlyMatchedDancers(CancellationToken cancellationToken, List<UserResponseModel> maleResponseModels)
        {
            foreach (var maleResponseModel in maleResponseModels)
            {
                var femaleResponseModel = maleResponseModel.Match;
                if (femaleResponseModel?.Match == null)
                {
                    throw new InvalidOperationException("The partner does not exists on one of the models");
                }

                await _mediator.Send(
                    new SetPartnerForCoupleCommand(
                        maleResponseModel.Id,
                        femaleResponseModel.Id),
                    cancellationToken);
            }
        }

        private static void FindBestMatchesForAttendees(
            IReadOnlyCollection<UserResponseModel> maleResponseModels, 
            IReadOnlyCollection<PreferenceModel> malePreferenceModels,
            IReadOnlyCollection<UserResponseModel> femaleResponseModels, 
            IReadOnlyCollection<PreferenceModel> femalePreferenceModels)
        {
            var freeCount = maleResponseModels.Count;

            while (freeCount > 0)
            {
                var freeMan = maleResponseModels
                    .FirstOrDefault(t => t.Match == null);

                for (var i = 0; i < maleResponseModels.Count && freeMan != null && freeMan.Match == null; i++)
                {
                    var preferredFemaleId = malePreferenceModels
                        .First(x => x.Id == freeMan.Id)
                        .PreferenceOrder[i];

                    var femaleModel = femaleResponseModels.First(x => x.Id == preferredFemaleId);

                    if (femaleModel.Match == null)
                    {
                        femaleModel.Match = freeMan;
                        freeMan.Match = femaleModel;
                        freeCount--;
                    }
                    else
                    {
                        var currentPartner = femaleModel.Match;

                        var preferredOrderForWoman = femalePreferenceModels
                            .First(x => x.Id == femaleModel.Id)
                            .PreferenceOrder;

                        if (preferredOrderForWoman.IndexOf(currentPartner.Id) <=
                            preferredOrderForWoman.IndexOf(freeMan.Id)) continue;
                        femaleModel.Match = freeMan;
                        currentPartner.Match = null;
                        freeMan.Match = femaleModel;
                    }
                }
            }
        }

        private static List<UserResponseModel> GetResponseModelsForGender(IEnumerable<ApplicationUser> users)
        {
            return users.Select(x =>
                new UserResponseModel(
                    x.Id, 
                    x.NormalizedUserName, 
                    x.Email, 
                    x.PhoneNumber)).ToList();
        }

        private static void FindPreferences(
            IEnumerable<ApplicationUser> genderToFindPreferencesFor, 
            IEnumerable<ApplicationUser> genderToPreference, 
            out List<PreferenceModel> preferenceModels)
        {
            preferenceModels = new List<PreferenceModel>();
            var partnerIds = genderToPreference.Select(x => x.Id).ToList();

            foreach (var dancer in genderToFindPreferencesFor)
            {
                var preferenceModel = new PreferenceModel(dancer.Id);
                var partnersNotYetDancedWith = partnerIds
                    .Where(x => !dancer.FormerMatches.Exists(y => y.PartnerId == x));
                foreach (var partner in partnersNotYetDancedWith)
                {
                    preferenceModel.PreferenceOrder.Add(partner);
                }

                var partnersDancedWithAscending =
                    dancer.FormerMatches
                        .GroupBy(x => x.PartnerId)
                        .Select(x => new {Id = x.Key, Count = x.Count()})
                        .OrderBy(x => x.Count);

                foreach (var partner in partnersDancedWithAscending)
                {
                    preferenceModel.PreferenceOrder.Add(partner.Id);
                }
                preferenceModels.Add(preferenceModel);
            }
        }

        private void LimitAmountOfAttendeesAndSortByMatches()
        {
            if (_attendingMen.Count < _attendingWomen.Count)
            {
                _attendingWomen = LimitAttendees(_attendingWomen, _attendingMen);
            }
            else if (_attendingMen.Count > _attendingWomen.Count)
            {
                _attendingMen = LimitAttendees(_attendingMen, _attendingWomen);
            }

            _attendingWomen = _attendingWomen.OrderByDescending(x => x.FormerMatches.Count).ToList();
            _attendingMen = _attendingMen.OrderByDescending(x => x.FormerMatches.Count).ToList();
        }

        private static List<ApplicationUser> LimitAttendees(IEnumerable<ApplicationUser> outnumberingGender, ICollection outnumberedGender)
        {
            return outnumberingGender
                .Take(outnumberedGender.Count)
                .OrderByDescending(x => x.FormerMatches.Count)
                .ToList();
        }

        private static List<ApplicationUser> GetAttendingUsersByGenderOrderedByDate(string gender, IEnumerable<ApplicationUser> attendingUsers)
        {
            return attendingUsers
                .Where(x => x.Gender == gender)
                .OrderBy(x => x.Attending)
                .ToList();
        } 

        public GetRandomPairingsOfAttendingUsersWithLevelQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
            _attendingWomen = new List<ApplicationUser>();
            _attendingMen = new List<ApplicationUser>();
        }
    }
}