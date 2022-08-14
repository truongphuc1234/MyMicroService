using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using User.API.Data;
using User.API.DTOs;
using User.API.Entities;

namespace User.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class UserController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    public UserController(UserManager<ApplicationUser> userManager, IMapper mapper, ApplicationDbContext context)
    {

        _userManager = userManager;
        _mapper = mapper;
        _context = context;
    }

    [HttpPost("signup")]
    [AllowAnonymous]
    public async Task<ActionResult> SignUp(UserSignUpDto signUpDto)
    {
        if (await _userManager.Users.AnyAsync(u => u.NormalizedUserName == signUpDto.UserName.ToUpper()))
        {
            return BadRequest("Username is existed");
        }

        if (await _userManager.Users.AnyAsync(u => signUpDto.Email.ToUpper() == u.NormalizedEmail))
        {
            return BadRequest("Email is existed");
        }

        var user = _mapper.Map<ApplicationUser>(signUpDto);

        var result = await _userManager.CreateAsync(user, signUpDto.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok();
    }


    [HttpGet("profile")]
    public async Task<ActionResult<UserProfileDto>> GetUserProfileAsync()
    {
        var applicationUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        if (applicationUserId == null)
        {
            return BadRequest();
        }
        var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(x => x.ApplicationUserId  == applicationUserId);

        var result = _mapper.Map<UserProfileDto>(userProfile);

        return Ok(result);

    }
}
