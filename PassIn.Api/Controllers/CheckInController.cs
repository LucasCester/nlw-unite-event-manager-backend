using Microsoft.AspNetCore.Mvc;
using PassIn.Application.UseCases.Checkins.DoCheckin;
using PassIn.Communication.Responses;

namespace PassIn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckInController : ControllerBase
    {
        [HttpPost]
        [Route("{attendeeId}/CheckIn")]
        [ProducesResponseType(typeof(ResponseRegisteredJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status409Conflict)]
        public IActionResult CheckIn([FromRoute] Guid attendeeId)
        {
            var useCase = new DoAttendeeCheckinUseCase();
            var result = useCase.Execute(attendeeId);

            return Created(string.Empty, result);
        }
    }
}
