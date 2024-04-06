using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entities;

namespace PassIn.Application.UseCases.Attendees.GetAllAttendee
{
    public class GetAllAttendeesByEventIdUseCase
    {
        private readonly PassInDbContext _dbContext;
        public GetAllAttendeesByEventIdUseCase()
        {
            _dbContext = new PassInDbContext();
        }

        public ResponseAllAttendeesjson Execute(Guid eventId)
        {
            var eventEntity = _dbContext.Events.Include(e => e.Attendees).ThenInclude(e => e.CheckIn).FirstOrDefault(e => e.Id.Equals(eventId));
            if (eventEntity == null)
                throw new NotFoundException("There's no event with this Id.");

            return new ResponseAllAttendeesjson()
            {
                Attendees = eventEntity.Attendees.Select(e => new ResponseAttendeeJson()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Email = e.Email,
                    CreatedAt = e.Created_At,
                    CheckedInAt = e.CheckIn?.Created_At
                }).ToList()
            };
        }
    }
}
