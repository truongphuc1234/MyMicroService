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

    [HttpPost("change-password")]
    public async Task<ActionResult> ChangePassword([FromBody] UpdatePasswordDto updatePasswordDto)
    {
        var applicationUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        var user = await _userManager.Users.FirstAsync(x => x.Id == applicationUserId);
        var result = await _userManager.ChangePasswordAsync(user, updatePasswordDto.CurrentPassword, updatePasswordDto.NewPassword);

        if (!result.Succeeded)
        {
            return Ok();
        }
        return BadRequest();
    }

    [HttpPost("change-email")]
    public async Task<ActionResult> ChangeEmail([FromBody] string email)
    {
        var applicationUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        var user = await _userManager.Users.FirstAsync(x => x.Id == applicationUserId);
        var result = await _userManager.SetEmailAsync(user, email);

        if (!result.Succeeded)
        {
            return Ok();
        }
        return BadRequest();
    }

    [HttpPost("change-phone")]
    public async Task<ActionResult> ChangePhone([FromBody] string phone)
    {
        var applicationUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        var user = await _userManager.Users.FirstAsync(x => x.Id == applicationUserId);
        var result = await _userManager.SetPhoneNumberAsync(user, phone);

        if (!result.Succeeded)
        {
            return Ok();
        }
        return BadRequest();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteUser()
    {
        var applicationUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        var user = await _userManager.Users.FirstAsync(x => x.Id == applicationUserId);
        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            return Ok();
        }
        return BadRequest();
    }


    [HttpGet("profile")]
    public async Task<ActionResult<UserProfileDto>> GetUserProfileAsync()
    {
        var applicationUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        if (applicationUserId == null)
        {
            return BadRequest();
        }
        var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(x => x.ApplicationUserId == applicationUserId);

        var result = _mapper.Map<UserProfileDto>(userProfile);

        return Ok(result);

    }

    [HttpGet("profile/{userId}")]
    public async Task<ActionResult<UserProfileDto>> GetUserProfileByUserIdAsync([FromQuery] string userId)
    {
        var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(x => x.ApplicationUserId == userId);
        var result = _mapper.Map<UserProfileDto>(userProfile);

        return Ok(result);
    }

    [HttpPut("profile")]
    public async Task<ActionResult<UserProfileDto>> UpdateUserProfile([FromBody] UserProfileDto userProfileDto)
    {
        var applicationUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        var profile = await _context.UserProfiles.Where(x => x.ApplicationUserId == applicationUserId).SingleOrDefaultAsync();
        if (profile == null)
        {
            return BadRequest();
        }

        var updateProfile = _mapper.Map<UserProfileDto, UserProfile>(userProfileDto, profile);
        await _context.SaveChangesAsync();
        return Ok();
    }
}
