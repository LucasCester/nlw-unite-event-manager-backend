using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Events.GetById
{
    public class GetEventByIdUseCase
    {
        private readonly PassInDbContext _dbContext;

        public GetEventByIdUseCase()
        {
            _dbContext = new PassInDbContext();
        }

        public ResponseEventJson Execute(Guid id)
        {
            var entity = _dbContext.Events.Include(e => e.Attendees).FirstOrDefault(e => e.Id.Equals(id));
            if (entity == null)
                throw new NotFoundException("There's no event with this Id.");

            return new ResponseEventJson()
            {
                Id = entity.Id,
                Title = entity.Title,
                Details = entity.Details,
                MaximumAttendees = entity.Maximum_Attendees,
                AttendeesAmount = entity.Attendees.Count()
            };
        }
    }
}
