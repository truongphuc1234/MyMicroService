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
        _logger.LogInformation("received a delete request for post id {} from user {}", id, User.GetName());
        var applicationUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        _postService.DeletePost(id, applicationUserId);
        return Ok();
    }

@GetMapping("/posts/me")
    public ResponseEntity<?> FindCurrentUserPosts()
{
    log.info("retrieving posts for user {}", principal.getName());

    List<Post> posts = postService.postsByUsername(principal.getName());
    log.info("found {} posts for user {}", posts.size(), principal.getName());

    return ResponseEntity.ok(posts);
}

@GetMapping("/posts/{username}")
    public ResponseEntity<?> findUserPosts(@PathVariable("username") String username) {
        log.info("retrieving posts for user {}", username);

        List<Post> posts = postService.postsByUsername(username);
log.info("found {} posts for user {}", posts.size(), username);

        return ResponseEntity.ok(posts);
    }

    @PostMapping("/posts/in")
    public ResponseEntity<?> findPostsByIdIn(@RequestBody List<String> ids)
{
    log.info("retrieving posts for {} ids", ids.size());

    List<Post> posts = postService.postsByIdIn(ids);
    log.info("found {} posts", posts.size());

    return ResponseEntity.ok(posts);
}
}