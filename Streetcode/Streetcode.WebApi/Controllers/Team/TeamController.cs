﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.MediatR.Team.GetAll;
using Streetcode.BLL.MediatR.Team.GetAllMain;
using Streetcode.BLL.MediatR.Team.GetById;

namespace Streetcode.WebApi.Controllers.Team
{
    [AllowAnonymous]
    public class TeamController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return HandleResult(await Mediator.Send(new GetAllTeamQuery()));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMain()
        {
            return HandleResult(await Mediator.Send(new GetAllMainTeamQuery()));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return HandleResult(await Mediator.Send(new GetByIdTeamQuery(id)));
        }
    }
}
