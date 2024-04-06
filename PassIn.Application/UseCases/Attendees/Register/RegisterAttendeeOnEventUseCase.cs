using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entities;
using System.Net.Mail;

namespace PassIn.Application.UseCases.Attendees.RegisterAttendee
{
    public class RegisterAttendeeOnEventUseCase
    {
        private readonly PassInDbContext _dbContext;

        public RegisterAttendeeOnEventUseCase()
        {
            _dbContext = new PassInDbContext();
        }

        public ResponseAttendeeJson Execute(Guid eventId, RequestRegisterEventJson request)
        {
            Validate(eventId, request);

            var entity = new Attendee()
            {
                Name = request.Name,
                Email = request.Email,
                Event_Id = eventId,
                Created_At = DateTime.UtcNow
            };

            _dbContext.Attendees.Add(entity);
            _dbContext.SaveChanges();

            return new ResponseAttendeeJson()
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                CreatedAt = entity.Created_At
            };
        }

        public void Validate(Guid eventId, RequestRegisterEventJson request)
        {
            var resultEvent = _dbContext.Events.FirstOrDefault(e => e.Id.Equals(eventId));

            if (resultEvent == null)
                throw new NotFoundException("There's no event with this Id.");

            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ErrorOnValidationException("Invalid Name.");

            if (string.IsNullOrWhiteSpace(request.Email) && !EmailIsValid(request.Email))
                throw new ErrorOnValidationException("Invalid Email.");

            if (_dbContext.Attendees.Any(e => e.Email.Equals(request.Email)))
                throw new ConflictException("There's already an attendee with this email.");

            if (_dbContext.Attendees.Count(e => e.Event_Id.Equals(eventId)) >= resultEvent.Maximum_Attendees)
                throw new NotFoundException("The limit of attendees was reached.");
        }

        private bool EmailIsValid(string email)
        {
            try
            {
                new MailAddress(email);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
