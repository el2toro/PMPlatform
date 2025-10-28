using Auth.API.Auth.Login;
using Auth.API.Interfaces;
using Auth.API.Models;
using Auth.API.Services;
using Moq;

namespace Auth.API.Tests.Auth.Handlers;

public class LoginHandlerTests
{
    private Mock<IAuthRepository> _authRepository;
    private Mock<IUserRepository> _userRepository;
    private Mock<IJwtTokenService> _jwtTokenService;
    private LoginHandler _loginHandler;
    public LoginHandlerTests()
    {
        _authRepository = new Mock<IAuthRepository>();
        _userRepository = new Mock<IUserRepository>();
        _jwtTokenService = new Mock<IJwtTokenService>();

        _loginHandler = new LoginHandler(_authRepository.Object, _userRepository.Object, _jwtTokenService.Object);
    }

    [Fact]
    public async Task Login_ShouldAuthenticateTheUser_Return_LoginResult()
    {
        var user = new User
        {
            Id = Guid.Parse("f6a2b3b5-2c3f-4d8f-8b0b-1a4e6b2e9a3c"),
            FirstName = "Alice",
            LastName = "Johnson",
            PasswordHash = "$2b$10$rM3Jy6CZFDUbg8P2igFiAedhg9FAwzDRmTpsNXuAbzQZk8vUvnPOO",
            Email = "alice.johnson@example.com",
            CreatedAt = new DateTime(2024, 5, 12, 14, 30, 0),
            UpdatedAt = DateTime.Now,
            UserTenants = new List<UserTenant>
    {
        new UserTenant
        {
            UserId = Guid.Parse("f6a2b3b5-2c3f-4d8f-8b0b-1a4e6b2e9a3c"),
            TenantId = Guid.NewGuid(),
            Role = Enums.TenantRole.Admin
        },
    }
        };

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            CreatedAt = DateTime.Now,
            RevokedAt = null,
            ExpiresAt = DateTime.Now.AddDays(7),
            Token = "token"
        };


        _userRepository.Setup(x => x.GetUserByEmail(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _authRepository.Setup(x => x.Login(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()));
        _jwtTokenService.Setup(x => x.GenerateToken(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<Guid>()))
            .Returns("token_123");

        _jwtTokenService.Setup(x => x.GenerateRefreshToken(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .Returns(refreshToken);

        LoginCommand loginCommand = new("alice.johnson@example.com", "Password123");

        var result = await _loginHandler.Handle(loginCommand, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(result.Email, loginCommand.Email);
    }
}
