namespace ErolFinal.BL.Exceptions;

public class LoginException : Exception
{
    public List<IdentityError> Errors { get; }

    public LoginException(List<IdentityError> errors)
    {
        Errors = errors;
    }
}
