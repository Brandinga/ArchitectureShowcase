using Autofac;
using TKWLogic;

public class CommandHandlerResolver : ICommandHandlerResolver
{
    private ILifetimeScope container;

    public CommandHandlerResolver(ILifetimeScope containerFactory)
    {
        this.container = containerFactory;
    }
    /// <summary>
    /// ruft die Handle Funktion vom Command vom Typ <T>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="command"></param>
    /// <param name="serviceName"></param>
    public void HandleCommand<T>(T command, TKWAppContext ctx) where T : AbstractCommand
    {
        var handlerInterface = typeof(ICommandHandler<>);
        var handlerType = handlerInterface.MakeGenericType(command.GetType());

        using (var scope = container.BeginLifetimeScope(builder =>
        {
            builder.RegisterInstance(ctx);
        }))
        {
            var handler = scope.Resolve(handlerType) as ICommandHandler<T>;

            if (handler != null)
            {
                handler.Handle(command);
            }
            else
            {
                throw new NullReferenceException("CommandHandlerResolver.HandleCommand: scope.Resolve(handlerType) as ICommandHandler<T> returned null");
            }
        }
    }
}