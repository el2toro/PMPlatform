namespace Auth.API.Exceptions;

public class RefreshTokenNotFoundException : NotFoundException
{
    public RefreshTokenNotFoundException(string message) : base("RefreshToken", message)
    {
    }
}
