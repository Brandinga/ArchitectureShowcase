using Autofac;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TKWData;
using TKWLogic;

public class AutofacModule
{
    public static IContainer BuildContainer(SqliteConnection _sqliteConnection)
    {
        ContainerBuilder builder = new ContainerBuilder();
        // The generic ILogger<TCategoryName> service was added to the ServiceCollection by ASP.NET Core.
        // It was then registered with Autofac using the Populate method. All of this starts
        // with the services.AddAutofac() that happens in Program and registers Autofac
        // as the service provider.
        // builder.Register(c => new ValuesService(c.Resolve<ILogger<ValuesService>>()))
        //     .As<IValuesService>()
        //     .InstancePerLifetimeScope();


        builder.RegisterType<DummyCommandHandlerResolver>().As<ICommandHandlerResolver>().SingleInstance();

        var options = new DbContextOptionsBuilder<TKWDbContext>()
                                .UseSqlite(_sqliteConnection)
                                .Options;

        builder.RegisterType<TKWDbContext>().AsSelf().WithParameter("options", options).InstancePerLifetimeScope();

        builder.RegisterType<PostCommitRegistratorImpl>().AsSelf().As<IPostCommitRegistrator>().InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies()).As(t => t.GetInterfaces().Where(a => a.IsClosedTypeOf(typeof(ICommandHandler<>))));
        builder.RegisterGenericDecorator(typeof(DummyTransactionalCommandHandlerDecorator<>), typeof(ICommandHandler<>));
        builder.RegisterGenericDecorator(typeof(PostCommitCommandHandlerDecorator<>), typeof(ICommandHandler<>));

        return builder.Build();
    }
}