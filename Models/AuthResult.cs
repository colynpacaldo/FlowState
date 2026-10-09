namespace FlowState.Models;

/// <summary>Outcome of a register/sign-in attempt.</summary>
public record AuthResult(bool Ok, string? Error = null);
