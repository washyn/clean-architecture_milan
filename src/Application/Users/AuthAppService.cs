using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Domain.Users;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;

namespace Application.Users;

public class AuthAppService : IAuthAppService
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenProvider _tokenProvider;

    public AuthAppService(IApplicationDbContext context, 
        IPasswordHasher passwordHasher,
        ITokenProvider tokenProvider)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _tokenProvider = tokenProvider;
    }
    
    public async Task<LoginResultDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _context
            .Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Email == loginDto.Email);
        if (user is null)
        {
            throw new ApplicationException("User not found");
        }
        bool verified = _passwordHasher.Verify(loginDto.Password, user.PasswordHash);
        if (!verified)
        {
            throw new ApplicationException("Invalid password");
        }
        return new LoginResultDto
        {
            Token = _tokenProvider.Create(user)
        };
    }
    
    public async Task RegisterAsync(RegisterDto registerDto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
        {
            throw new ApplicationException("Email already exists");
        }
        var user = new User
        {
            Email = registerDto.Email,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            PasswordHash = _passwordHasher.Hash(registerDto.Password)
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }
}

public interface IAuthAppService : ITransientDependency
{
    Task<LoginResultDto> LoginAsync(LoginDto loginDto);
    Task RegisterAsync(RegisterDto registerDto);
}

public class LoginDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginResultDto
{
    public string Token { get; set; }
}

public class RegisterDto
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
}


internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterDto>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(c => c.FirstName).NotEmpty();
        RuleFor(c => c.LastName).NotEmpty();
        RuleFor(c => c.Email).NotEmpty().EmailAddress();
        RuleFor(c => c.Password).NotEmpty().MinimumLength(8);
    }
}
