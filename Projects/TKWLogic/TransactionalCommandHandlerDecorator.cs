using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TKWData;
using TKWLogic;

public class TransactionalCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : AbstractCommand
{
    private readonly DbContext _context;
    private readonly ICommandHandler<TCommand> _decoratedCommand;
    private readonly Func<TKWAppContext> _TKWContext;
    private readonly IPostCommitRegistrator _postCommit;

    //select * from master.dbo.sysmessages
    public const int SqlServerViolationOfConstraint = 547;

    /// <summary>
    /// Initializes a new instance of the TransactionalCommandHandlerDecorator class.
    /// </summary>
    /// <param name="decoratedCommand"></param>
    public TransactionalCommandHandlerDecorator(ICommandHandler<TCommand> decoratedCommand, TKWDbContext context, Func<TKWAppContext> t04Context, IPostCommitRegistrator postCommit)
    {

        _decoratedCommand = decoratedCommand;
        _context = context;
        _TKWContext = t04Context;
        _postCommit = postCommit;
    }

    public void Handle(TCommand command)
    {
        // HINT: if we dont throw/rethrow exceptions, postCommit actions and so on are getting executed, be carefull!
        using (var dbContextTransaction = _context.Database.BeginTransaction())
        {
            try
            {
                _decoratedCommand.Handle(command);

                _context.SaveChanges();

                dbContextTransaction.Commit();
            }
            catch (DbUpdateConcurrencyException)
            {

                //var changedEntites = ex.Entries.GetChangedEntitiesByPropertyAsJson(_context);
                throw;
            }
            catch (DbUpdateException ex)
            {
                SqliteException? sqlEx = ex?.InnerException?.InnerException as SqliteException;
                if (sqlEx != null)
                {
                    //take care! the same error number can be used for various erros
                    //zb.: Die INSERT-Anweisung steht in Konflikt mit der CHECK-Einschränkung 'chk_BuchungAvoidDuplicates'. Der Konflikt trat in der tennis04-Datenbank, Tabelle 'dbo.Buchung' auf.
                    //The %ls statement conflicted with the %ls constraint "%.*ls". The conflict occurred in database "%.*ls", table "%.*ls"%ls%.*ls%ls.
                    //--> error number 547 --> only reference exceptions should be handled as "known" error, not CHECK constraints
                    //                    if (sqlEx.Number == SqlServerViolationOfConstraint && sqlEx.Message.Contains("REFERENCE"))
                    if (sqlEx.Message.Contains("REFERENCE"))
                    {
                        throw;
                    }

                    // spezific database constraint
                    if (sqlEx.Message.Contains("chk_BuchungAvoidDuplicates"))
                    {

                        throw;
                    }
                }

                throw;
            }
            // catch (DbEntityValidationException ex)
            // {
            //     var newException = new FormattedDbEntityValidationException(ex);
            //     throw newException;
            // }
            catch (Exception)
            {
                // if no connection exists the rollback fails with: The underlying provider failed on Rollback. --> "Value cannot be null. Parameter name: connection"
                // this occurs e.g. if a deadlock gets detected or we already called Commit()[nice for testing!] on this transaction
                // if (dbContextTransaction.UnderlyingTransaction.Connection != null)
                // {
                //     dbContextTransaction.Rollback();
                // }

                dbContextTransaction.Rollback();
                throw;

            }
            // not practicle for cleanup/exceute always, because if a exception is thrown within a catch, finally is not called
            // finally { }

        }
    }
}