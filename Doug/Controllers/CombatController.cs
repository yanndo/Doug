﻿using System.Threading.Tasks;
using Doug.Commands;
using Doug.Controllers.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Doug.Controllers
{
    [Route("cmd/[controller]")]
    [ApiController]
    public class CombatController : ControllerBase
    {
        private readonly ICombatCommands _combatCommands;

        public CombatController(ICombatCommands combatCommands)
        {
            _combatCommands = combatCommands;
        }

        [HttpPost("steal")]
        public async Task<ActionResult> Steal([FromForm]SlackCommandDto slackCommand)
        {
            var result = await _combatCommands.Steal(slackCommand.ToCommand());
            return Ok(result.Message);
        }

        [HttpPost("attack")]
        public async Task<ActionResult> Attack([FromForm]SlackCommandDto slackCommand)
        {
            var result = await _combatCommands.Attack(slackCommand.ToCommand());
            return Ok(result.Message);
        }

        [HttpPost("revolution")]
        public async Task<ActionResult> Revolution([FromForm]SlackCommandDto slackCommand)
        {
            var result = await _combatCommands.Revolution(slackCommand.ToCommand());
            return Ok(result.Message);
        }

        [HttpPost("monsters")]
        public async Task<ActionResult> Monsters([FromForm]SlackCommandDto slackCommand)
        {
            var result = await _combatCommands.ListMonsters(slackCommand.ToCommand());
            return Ok(result.Message);
        }

        [HttpPost("skill")]
        public ActionResult Skill([FromForm]SlackCommandDto slackCommand)
        {
            var result = _combatCommands.Skill(slackCommand.ToCommand());
            return Ok(result.Message);
        }
    }
}