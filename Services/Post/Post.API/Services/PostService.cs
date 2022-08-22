using Microsoft.AspNetCore.Mvc;
using MyMicroservice.PostAPI.DTOs;
using MyMicroservice.PostAPI.Entities;
using MyMicroservice.PostAPI.Intefaces;

namespace MyMicroservice.PostAPI.Services;

public class PostService : IPostService
{
    public ActionResult CreatePost(CreatePostDto createPostDto)
    {
        throw new NotImplementedException();
    }

    public ActionResult DeletePost(string id, string? userId)
    {
        throw new NotImplementedException();
    }

    public ActionResult GetPostById(string id)
    {
        throw new NotImplementedException();
    }

    public List<Post> GetPostsByCurrentUserId(string? applicationUserId)
    {
        throw new NotImplementedException();
    }

    public List<Post> GetPostsByIds(List<string> ids)
    {
        throw new NotImplementedException();
    }

    public List<Post> GetPostsByUserId(string? applicationUserId)
    {
        throw new NotImplementedException();
    }
}