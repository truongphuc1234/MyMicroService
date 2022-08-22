using System.Security.Claims;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyMicroservice.PostAPI.DTOs;
using MyMicroservice.PostAPI.Entities;
using MyMicroservice.PostAPI.Intefaces;

namespace MyMicroservice.PostAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PostController : ControllerBase
{
    private ILogger<PostController> _logger;
    private IPostService _postService;

    public PostController(ILogger<PostController> logger, IPostService postService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _postService = postService ?? throw new ArgumentNullException(nameof(postService));
    }

    [HttpPost]
    public ActionResult CreatePost([FromBody] CreatePostDto postCreateDto)
    {
        _logger.LogInformation("Create new Post");
        _postService.CreatePost(postCreateDto);
        return Ok();
    }

    [HttpDelete("/{id}")]
    public ActionResult DeletePost(string id)
    {
        _logger.LogInformation($"Received a delete request for post id {id} from user {User.GetDisplayName()}");
        var applicationUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        _postService.DeletePost(id, applicationUserId);
        return Ok();
    }

    [HttpGet]
    public ActionResult FindCurrentUserPosts()
    {
        var applicationUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        _logger.LogInformation($"Retrieving posts for user {User.GetDisplayName()}");
        List<Post> posts = _postService.GetPostsByUserId(applicationUserId);
        _logger.LogInformation($"Found {posts.Count} posts for user {User.GetDisplayName()}");
        return Ok(posts);
    }

    [HttpGet("/user/{userId}")]
    public ActionResult FindUserPosts(string userId)
    {
        _logger.LogInformation($"Retrieving posts for user id {userId}");
        List<Post> posts = _postService.GetPostsByUserId(userId);
        _logger.LogInformation($"Found {posts.Count} posts for user {User.GetDisplayName()}");
        return Ok(posts);
    }

    [HttpPost("/postsByIds")]
    public ActionResult FindPostsByIds([FromBody] List<String> ids)
    {
        _logger.LogInformation($"Retrieving posts for {ids.Count} ids");
        List<Post> posts = _postService.GetPostsByIds(ids);
        _logger.LogInformation($"found {posts.Count} posts");
        return Ok(posts);
    }
}