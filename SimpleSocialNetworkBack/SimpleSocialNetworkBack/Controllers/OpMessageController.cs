using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Models;
using Business.Models.Requests;
using Business.Models.Responses;
using Business.Services;
using Business.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SimpleSocialNetworkBack.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("/api/[controller]")]
    public class OpMessageController : ControllerBase
    {
        private readonly IOpMessageService _opMessageService;

        public OpMessageController(IOpMessageService opMessageService)
        {
            _opMessageService = opMessageService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<int>> CreateOpMessage([FromBody] CreateOpMessageModel opMessage)
        {
            var username = User.Identity.Name!;

            var post = await _opMessageService.MakeAPost(username, opMessage);
            return Ok(post);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<OpMessageModel>> GetOpMessage(int id)
        {
            return Ok(await _opMessageService.GetById(id));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<OpMessageModel>>> GetOpMessages()
        {
            return Ok(await _opMessageService.GetAll());
        }

        [HttpGet("comments/{postId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CommentModel>>> GetComments(int postId)
        {
            return Ok(await _opMessageService.GetComments(postId));
        }
    }
}