﻿using System;
using Doug.Commands;
using Doug.Controllers.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Doug.Controllers
{
    [Route("cmd/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly IStatsCommands _statsCommands;

        public StatsController(IStatsCommands statsCommands)
        {
            _statsCommands = statsCommands;
        }

        [HttpPost("profile")]
        public ActionResult Profile([FromForm]SlackCommandDto slackCommand)
        {
            try
            {
                var result = _statsCommands.Profile(slackCommand.ToCommand());
                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                return Ok(string.Format(DougMessages.DougError, ex.Message));
            }
        }

        [HttpPost("balance")]
        public ActionResult Balance([FromForm]SlackCommandDto slackCommand)
        {
            try
            {
                var result = _statsCommands.Balance(slackCommand.ToCommand());
                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                return Ok(string.Format(DougMessages.DougError, ex.Message));
            }
        }

        [HttpPost("health")]
        public ActionResult Health([FromForm]SlackCommandDto slackCommand)
        {
            try
            {
                var result = _statsCommands.Health(slackCommand.ToCommand());
                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                return Ok(string.Format(DougMessages.DougError, ex.Message));
            }
        }

        [HttpPost("energy")]
        public ActionResult Energy([FromForm]SlackCommandDto slackCommand)
        {
            try
            {
                var result = _statsCommands.Energy(slackCommand.ToCommand());
                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                return Ok(string.Format(DougMessages.DougError, ex.Message));
            }
        }

        [HttpPost("equipment")]
        public ActionResult Equipment([FromForm]SlackCommandDto slackCommand)
        {
            try
            {
                var result = _statsCommands.Equipment(slackCommand.ToCommand());
                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                return Ok(string.Format(DougMessages.DougError, ex.Message));
            }
        }

        [HttpPost("inventory")]
        public ActionResult Inventory([FromForm]SlackCommandDto slackCommand)
        {
            try
            {
                var result = _statsCommands.Inventory(slackCommand.ToCommand());
                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                return Ok(string.Format(DougMessages.DougError, ex.Message));
            }
        }
    }
}