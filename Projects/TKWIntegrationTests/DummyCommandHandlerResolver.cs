using Autofac;
using TKWLogic;

public class DummyCommandHandlerResolver : ICommandHandlerResolver
    {
        private ILifetimeScope container;

        public DummyCommandHandlerResolver(ILifetimeScope containerFactory)
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

            using (var scope = container.BeginLifetimeScope(
                builder =>
                {
                    builder.RegisterInstance(ctx);
                }))
            {
                var handler = scope.Resolve(handlerType) as ICommandHandler<T>;

                handler.Handle(command);
            }

            // ef does not refresh entities with database values if the entity is already attached
            // this caused confusion (load entity X, execute command - mutate X, load X --> mutation not visible):
            // HINT: Without AsNoTracking the test fails in line 110: Assert.AreEqual(appointmentSeries.Single().PrimaryKey, shopOfferAfterCheckout.IDAbos);
            //       Test environment is buggy? Do we not have a new DbContext for each command and also Testhelper?
            // this pattern ClientWins(https://learn.microsoft.com/en-us/dotnet/api/system.data.objects.objectcontext.refresh?view=netframework-4.8) does not occur when using AsNoTracking
            // to overcome this issue, we refresh all already tracked entities after each command
            // !! but most important: our Asserts should always represent the value from the database, so we should not rely on adding AsNoTracking()
            TestHelper.RefreshAllEntities();
        }
    }