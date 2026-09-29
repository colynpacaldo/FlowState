using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.JSInterop;

namespace FlowState.Services;

public record UserAccount(string Username, string Email, string Salt, string Hash);
public record AuthResult(bool Ok, string? Error = null);

/// <summary>Pure validation rules, kept separate so they are easy to unit test.</summary>
public static class AuthRules
{
    public const int MinPassword = 8;

    static readonly Regex UserRx = new("^[A-Za-z0-9_]{3,20}$", RegexOptions.Compiled);

    public static string? Username(string? v) =>
        UserRx.IsMatch(v?.Trim() ?? "") ? null : "3–20 characters: letters, numbers, underscore.";

    public static string? Email(string? v)
    {
        var s = v?.Trim() ?? "";
        if (s.Length == 0 || s.Length > 254) return "Enter a valid email address.";
        try
        {
            var a = new MailAddress(s);
            // MailAddress accepts "Name <x@y>" forms; require the address to equal the input and have a dotted domain.
            return a.Address == s && a.Host.Contains('.') ? null : "Enter a valid email address.";
        }
        catch (FormatException) { return "Enter a valid email address."; }
    }

    public static string? Password(string? v) =>
        (v?.Length ?? 0) >= MinPassword ? null : $"Use at least {MinPassword} characters.";

    public static string? Confirm(string? pw, string? confirm) =>
        pw == confirm ? null : "Passwords don't match.";
}

/// <summary>
/// Client-side account store for the Blazor WASM app. Accounts live in the browser's
/// localStorage with salted, iterated SHA-256 hashes. This is fine for a local/demo app,
/// but it is NOT real security: swap this for a server API (and real password hashing)
/// before storing anything that matters.
/// </summary>
public class AuthService
{
    const string UsersKey = "flowstate.users", SessionKey = "flowstate.session";
    const int HashRounds = 20_000;

    readonly IJSRuntime _js;
    List<UserAccount> _users = new();
    bool _loaded;

    public AuthService(IJSRuntime js) => _js = js;

    public event Action? Changed;
    public UserAccount? Current { get; private set; }
    public bool IsSignedIn => Current is not null;

    /// <summary>Loads accounts and the saved session once. Safe to call repeatedly.</summary>
    public async Task InitAsync()
    {
        if (_loaded) return;
        _users = Read<List<UserAccount>>(await GetAsync(UsersKey)) ?? new();
        var email = await GetAsync(SessionKey);
        Current = email is null ? null : _users.FirstOrDefault(u => Same(u.Email, email));
        _loaded = true;
    }

    public async Task<AuthResult> RegisterAsync(string username, string email, string password, string confirm)
    {
        await InitAsync();
        username = username.Trim(); email = email.Trim();

        var err = AuthRules.Username(username) ?? AuthRules.Email(email)
               ?? AuthRules.Password(password) ?? AuthRules.Confirm(password, confirm);
        if (err is not null) return new(false, err);

        if (_users.Any(u => Same(u.Email, email))) return new(false, "An account with that email already exists.");
        if (_users.Any(u => Same(u.Username, username))) return new(false, "That username is taken.");

        var salt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
        var user = new UserAccount(username, email, salt, Hash(password, salt));
        _users.Add(user);
        await SetAsync(UsersKey, JsonSerializer.Serialize(_users));
        await StartSessionAsync(user);
        return new(true);
    }

    public async Task<AuthResult> SignInAsync(string email, string password)
    {
        await InitAsync();
        var user = _users.FirstOrDefault(u => Same(u.Email, email.Trim()));
        // Same message for unknown email and wrong password so accounts can't be enumerated.
        if (user is null || !FixedTimeEquals(user.Hash, Hash(password, user.Salt)))
            return new(false, "Incorrect email or password.");
        await StartSessionAsync(user);
        return new(true);
    }

    public async Task SignOutAsync()
    {
        Current = null;
        await _js.InvokeVoidAsync("localStorage.removeItem", SessionKey);
        Changed?.Invoke();
    }

    async Task StartSessionAsync(UserAccount user)
    {
        Current = user;
        await SetAsync(SessionKey, user.Email);
        Changed?.Invoke();
    }

    static bool Same(string a, string b) => string.Equals(a, b, StringComparison.OrdinalIgnoreCase);

    static string Hash(string password, string salt)
    {
        var data = SHA256.HashData(Encoding.UTF8.GetBytes(salt + password));
        for (var i = 0; i < HashRounds; i++) data = SHA256.HashData(data);
        return Convert.ToBase64String(data);
    }

    static bool FixedTimeEquals(string a, string b) =>
        CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(a), Encoding.UTF8.GetBytes(b));

    static T? Read<T>(string? json)
    {
        if (string.IsNullOrEmpty(json)) return default;
        try { return JsonSerializer.Deserialize<T>(json); } catch (JsonException) { return default; }
    }

    async Task<string?> GetAsync(string key) => await _js.InvokeAsync<string?>("localStorage.getItem", key);
    async Task SetAsync(string key, string value) => await _js.InvokeVoidAsync("localStorage.setItem", key, value);
}
