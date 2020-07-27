using MediatR;
using Microsoft.VisualBasic.FileIO;

namespace RegistrationApp.Messaging.Commands.SetPartnerForCouple
{
    public class SetPartnerForCoupleCommand : IRequest
    {
        public string MaleId { get; }
        public string FemaleId { get; }

        public SetPartnerForCoupleCommand(string maleId, string femaleId)
        {
            MaleId = maleId;
            FemaleId = femaleId;
        }
    }
}