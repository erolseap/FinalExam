namespace ErolFinal.BL.Exceptions;

public class RegisterException : Exception
{
    public List<IdentityError> Errors { get; }

    public RegisterException(List<IdentityError> errors)
    {
        Errors = errors;
    }
}
