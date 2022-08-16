using System;

namespace User.API.Entities;

public class UserProfile
{
    public Guid Id { get; set; }
    public DateTime Birthday { get; set; }
    public string? Gender { get; set; }
    public string? FacebookAccount { get; set; }
    public string? GoogleAccount { get; set; }
    public string? Avatar { get; set; }
    public string? Background { get; set; }
    public string? Location { get; set; }
    public string? DisplayName { get; set; }
    public ApplicationUser ApplicationUser { get; set; } = null!;
    public string ApplicationUserId { get; set; } = string.Empty;
}