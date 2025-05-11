using Microsoft.EntityFrameworkCore;
using TKWData;
using TKWLogic;

public class DummyTransactionalCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : AbstractCommand
{
    private readonly TKWDbContext _context;
    private readonly ICommandHandler<TCommand> _decoratedCommand;

    public bool SaveChangesWasCalled { get; private set; } = false;

    /// <summary>
    /// Initializes a new instance of the TransactionalCommandHandlerDecorator class.
    /// </summary>
    /// <param name="decoratedCommand"></param>
    public DummyTransactionalCommandHandlerDecorator(ICommandHandler<TCommand> decoratedCommand, TKWDbContext context)
    {

        _decoratedCommand = decoratedCommand;
        _context = context;
    }

    public void Handle(TCommand command)
    {
        try
        {
            _decoratedCommand.Handle(command);
            _context.SaveChanges();
        }
        catch (Exception)
        {
            throw;
        }
    }
}