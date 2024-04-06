using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entities;

namespace PassIn.Application.UseCases.Checkins.DoCheckin
{
    public class DoAttendeeCheckinUseCase
    {
        private readonly PassInDbContext _dbContext;

        public DoAttendeeCheckinUseCase()
        {
            _dbContext = new PassInDbContext();    
        }

        public ResponseRegisteredJson Execute(Guid attendeeId)
        {
            Validate(attendeeId);

            var entity = new CheckIn
            {
                Attendee_Id = attendeeId,
                Created_At = DateTime.UtcNow,
            };

            _dbContext.CheckIns.Add(entity);
            _dbContext.SaveChanges();

            return new ResponseRegisteredJson()
            {
                Id = attendeeId,
            };
        }

        public void Validate(Guid attendeeId)
        {
            if (!_dbContext.Attendees.Any(e => e.Id.Equals(attendeeId)))
                throw new NotFoundException("There's no attendee with this Id.");

            if(_dbContext.CheckIns.Any(e => e.Attendee_Id.Equals(attendeeId)))
                throw new ConflictException("Attendee cannot do Check In twice.");
        }
    }
}
