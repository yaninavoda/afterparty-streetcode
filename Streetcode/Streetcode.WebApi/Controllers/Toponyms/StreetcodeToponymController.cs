using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.StreetcodeToponym;
using Streetcode.BLL.MediatR.StreetcodeToponym;

namespace Streetcode.WebApi.Controllers.Toponyms;

public class StreetcodeToponymController : BaseApiController
{
    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteStreetcodeToponymRequestDto deleteRequest)
    {
        return HandleResult(await Mediator.Send(new DeleteStreetcodeToponymCommand(deleteRequest)));
    }
}