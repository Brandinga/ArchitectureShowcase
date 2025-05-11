using System.ComponentModel.DataAnnotations;

namespace TKWLogic;

public interface ICommand
{
    // messages shown to a user after ending
    List<String> ReturnMessages { get; set; }
}

public class AbstractCommand : ICommand
{
    public List<String> ReturnMessages { get; set; } = new List<string>();
}


public interface ICommandValidation<in TCommand> where TCommand : AbstractCommand
{
    IEnumerable<ValidationResult> ValidateObject(TCommand command);
}

public interface ICommandHandler<in TCommand> where TCommand : AbstractCommand
{
    void Handle(TCommand command);
}

public interface ICommandHandlerResolver
{
    void HandleCommand<T>(T command, TKWAppContext ctx) where T : AbstractCommand;
}