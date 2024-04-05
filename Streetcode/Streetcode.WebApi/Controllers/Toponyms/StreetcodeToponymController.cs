using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.StreetcodeToponym;
using Streetcode.BLL.MediatR.StreetcodeToponym.Create;

namespace Streetcode.WebApi.Controllers.Toponyms;

public class StreetcodeToponymController : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStreetcodeToponymRequestDto createRequest)
    {
        return HandleResult(await Mediator.Send(new CreateStreetcodeToponymCommand(createRequest)));
    }
}
