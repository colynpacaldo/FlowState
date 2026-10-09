namespace FlowState.Models;

/// <summary>A locally stored account (salted, iterated SHA-256 hash).</summary>
public record UserAccount(string Username, string Email, string Salt, string Hash);
