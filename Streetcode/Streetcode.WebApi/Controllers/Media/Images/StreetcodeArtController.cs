using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.MediatR.Media.StreetcodeArt.Delete;
using Streetcode.BLL.MediatR.Media.StreetcodeArt.GetByStreetcodeId;

namespace Streetcode.WebApi.Controllers.Media.Images;

public class StreetcodeArtController : BaseApiController
{
    [HttpGet("{streetcodeId:int}")]
    public async Task<IActionResult> GetByStreetcodeId([FromRoute] int streetcodeId)
    {
        return HandleResult(await Mediator.Send(new GetStreetcodeArtByStreetcodeIdQuery(streetcodeId)));
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteStreetcodeArtRequestDto deleteArtRequestDto)
    {
        return HandleResult(await Mediator.Send(new DeleteStreetcodeArtCommand(deleteArtRequestDto)));
    }
}
