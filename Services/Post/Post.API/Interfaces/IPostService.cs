using Microsoft.AspNetCore.Mvc;
using MyMicroservice.PostAPI.DTOs;
using MyMicroservice.PostAPI.Entities;

namespace MyMicroservice.PostAPI.Intefaces;

public interface IPostService
{
    public ActionResult CreatePost(CreatePostDto createPostDto);
    public ActionResult GetPostById(string id);
    public ActionResult DeletePost(string id, string? userId);
    List<Post> GetPostsByCurrentUserId(string? applicationUserId);
    List<Post> GetPostsByUserId(string? applicationUserId);
    List<Post> GetPostsByIds(List<string> ids);
}