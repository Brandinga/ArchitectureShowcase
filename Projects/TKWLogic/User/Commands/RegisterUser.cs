using System.ComponentModel.DataAnnotations;
using TKWData;
using TKWData.Models;
using TKWLogic;



public class UserPermission : Attribute
{
    public UserPermission(UserPermissions permission)
    {
    }
}

[UserPermission(UserPermissions.Guest)]
public class RegisterUserCommand : AbstractCommand
{
    [Required]
    [MaxLength(50)]
    public required string Name { get; set; }
    [Required]
    [MaxLength(25)]
    public required string FirstName { get; set; }

    [Required]
    [MaxLength(50)]
    public required string Password { get; set; }

}

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand>, ICommandValidation<RegisterUserCommand>
{
    private TKWDbContext _context;
    //private ILogger _logger;
    private TKWAppContext _t04Context;
    private IPostCommitRegistrator _postCommit;

    /// <summary>
    /// Initializes a new instance of the ResetPasswordCommandHandler class.
    /// </summary>
    public RegisterUserCommandHandler(TKWDbContext context, Func<TKWAppContext> t04Context, IPostCommitRegistrator postCommit)
    {
        _context = context;
        //_logger = logger;
        _t04Context = t04Context();
        _postCommit = postCommit;
    }

    public void Handle(RegisterUserCommand command)
    {

        var user = new User
        {
            Id = new Guid(),
            Name = command.Name,
            Password = command.Password,
        };

        _context.User.Add(user);

        _postCommit.Committed += () =>
        {
            // TODO: send email
        };
    }

    public IEnumerable<ValidationResult> ValidateObject(RegisterUserCommand command)
    {
        return new List<ValidationResult>();
    }
}