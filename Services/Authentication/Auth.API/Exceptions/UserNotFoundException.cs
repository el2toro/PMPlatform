using Core.Exceptions;

namespace Auth.API.Exceptions;

public class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(string id) : base("User", id)
    {
    }
}
