using MediatR;

namespace RegistrationApp.Messaging.Commands.SetAttendingToNull
{
    public class SetAttendingToNullCommand : IRequest
    {
        public string Id { get; }

        public SetAttendingToNullCommand(string id)
        {
            Id = id;
        }
    }
}