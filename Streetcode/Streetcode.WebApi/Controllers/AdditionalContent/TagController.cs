using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetAll;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetById;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetByStreetcodeId;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetTagByTitle;

namespace Streetcode.WebApi.Controllers.AdditionalContent
{
    [AllowAnonymous]
    public class TagController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return HandleResult(await Mediator.Send(new GetAllTagsQuery()));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return HandleResult(await Mediator.Send(new GetTagByIdQuery(id)));
        }

        [HttpGet("{streetcodeId:int}")]
        public async Task<IActionResult> GetByStreetcodeId([FromRoute] int streetcodeId)
        {
            return HandleResult(await Mediator.Send(new GetTagByStreetcodeIdQuery(streetcodeId)));
        }

        [HttpGet("{title}")]
        public async Task<IActionResult> GetTagByTitle([FromRoute] string title)
        {
            return HandleResult(await Mediator.Send(new GetTagByTitleQuery(title)));
        }
    }
}
