using TKWLogic;

public sealed class PostCommitCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : AbstractCommand
    {
        private readonly ICommandHandler<TCommand> decorated;
        private readonly PostCommitRegistratorImpl registrator;
        private readonly Func<TKWAppContext> _TKWContext;

        public PostCommitCommandHandlerDecorator(
            ICommandHandler<TCommand> decorated, PostCommitRegistratorImpl registrator, Func<TKWAppContext> t04Context)
        {
            this.decorated = decorated;
            this.registrator = registrator;
            this._TKWContext = t04Context;
        }

        public void Handle(TCommand command)
        {

            decorated.Handle(command);

            try
            {
                this.registrator.ExecuteActions();
            }
            // catch (TKWException)
            // {
            //     throw;
            // }
            catch (Exception)
            {
                //LoggingHelper logHelper = LoggingHelper.GetInstance();
                //logHelper.WriteApplicationError(_t04Context().UserRights.OrganisationId, LogApplikationsBereich.PostCommitDecorator, ex, (Guid)_t04Context().UserRights.UserId);
            }
            finally
            {
                this.registrator.Reset();
            }
        }
    }

public interface IPostCommitRegistrator
{
    event Action Committed;
    void Reset();
}

public sealed class PostCommitRegistratorImpl : IPostCommitRegistrator
{
    private readonly object completedEventLock = new object();
    private event Action committed = () => { };
    public event Action Committed
    {
        add
        {
            lock (completedEventLock)
            {
                committed += value;
            }
        }
        remove
        {
            lock (completedEventLock)
            {
                committed -= value;
            }
        }
    }

    public void ExecuteActions()
    {
        this.committed();
    }

    public void Reset()
    {
        // Clears the list of actions.
        this.committed = () => { };
    }
}