using Microsoft.AspNetCore.Mvc;
using MyMicroservice.PostAPI.DTOs;

namespace MyMicroservice.PostAPI.Intefaces;

public interface IPostService
{
    public ActionResult CreatePost(CreatePostDto createPostDto);
    public ActionResult GetPostById(string id);
    public ActionResult DeletePost(string id, string? userId);
}