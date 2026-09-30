using Application.Features.Auth.Commands;
using Application.Features.Auth.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ISender _sender;

        public AuthController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponse>> Register(
            RegisterCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(
                command,
                cancellationToken);

            return Ok(result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponse>> Login(
            LoginCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(
                command,
                cancellationToken);

            return Ok(result);
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponse>> Refresh(
            RefreshTokenCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(
                command,
                cancellationToken);

            return Ok(result);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout(
            LogoutCommand command,
            CancellationToken cancellationToken)
        {
            await _sender.Send(
                command,
                cancellationToken);

            return NoContent();
        }
    }
}
