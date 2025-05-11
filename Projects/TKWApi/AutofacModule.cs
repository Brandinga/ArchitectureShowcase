using Autofac;
using Microsoft.EntityFrameworkCore;
using TKWData;
using TKWLogic;

public class AutofacModule : Module
{

    protected override void Load(ContainerBuilder builder)
    {
        // The generic ILogger<TCategoryName> service was added to the ServiceCollection by ASP.NET Core.
        // It was then registered with Autofac using the Populate method. All of this starts
        // with the services.AddAutofac() that happens in Program and registers Autofac
        // as the service provider.
        // builder.Register(c => new ValuesService(c.Resolve<ILogger<ValuesService>>()))
        //     .As<IValuesService>()
        //     .InstancePerLifetimeScope();

        builder.RegisterType<CommandHandlerResolver>().As<ICommandHandlerResolver>().SingleInstance();

        // TODO: do not hardcode connectionstring
        var options = new DbContextOptionsBuilder<TKWDbContext>()
                                .UseSqlite("Data Source=..\\TKWDatabase.db")
                                .Options;

        builder.RegisterType<TKWDbContext>().AsSelf().WithParameter("options", options).InstancePerLifetimeScope();


        builder.RegisterType<PostCommitRegistratorImpl>().AsSelf().As<IPostCommitRegistrator>().InstancePerLifetimeScope();

        //     builder.RegisterAssemblyTypes(logicAssembly).As(t => t.GetInterfaces().Where(a => a.IsClosedTypeOf(typeof(ICommandValidation<>))));

        //     //hat nicht immer funktioniert - jetzt direkt laden

        builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies()).As(t => t.GetInterfaces().Where(a => a.IsClosedTypeOf(typeof(ICommandHandler<>))));
        //builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies()).As(t => t.GetInterfaces().Where(a => a.IsClosedTypeOf(typeof(ICommandHandler<>))).Select(a => new KeyedService("commandHandler", a)));
        //     builder.RegisterAssemblyTypes(logicAssembly).As(t => t.GetInterfaces().Where(a => a.IsClosedTypeOf(typeof(ICommandHandler<>))).Select(a => new KeyedService("commandHandler", a)));
        //     builder.RegisterGenericDecorator(typeof(AspectsCommandHandlerDecorator<>), typeof(ICommandHandler<>), fromKey: "commandHandler").Keyed("aspectsDecorator", typeof(ICommandHandler<>));
        //builder.RegisterGenericDecorator(typeof(TransactionalCommandHandlerDecorator<>), typeof(ICommandHandler<>), fromKey: "aspectsDecorator").Keyed("transactionDecorator", typeof(ICommandHandler<>));
        //builder.RegisterGenericDecorator(typeof(PermissionCommandHandlerDecorator<>), typeof(ICommandHandler<>), fromKey: "transactionDecorator").Keyed("permissionDecorator", typeof(ICommandHandler<>));
        //     builder.RegisterGenericDecorator(typeof(InfoMessagesCommandHandlerDecorator<>), typeof(ICommandHandler<>), fromKey: "permissionDecorator").Keyed("infoMessageDecorator", typeof(ICommandHandler<>));
        //     builder.RegisterGenericDecorator(typeof(PostCommitCommandHandlerDecorator<>), typeof(ICommandHandler<>), fromKey: "infoMessageDecorator").Keyed("postCommitDecorator", typeof(ICommandHandler<>));
        //     builder.RegisterGenericDecorator(typeof(ValidationCommandHandlerDecorator<>), typeof(ICommandHandler<>), fromKey: "postCommitDecorator");


        builder.RegisterGenericDecorator(typeof(TransactionalCommandHandlerDecorator<>), typeof(ICommandHandler<>));
        builder.RegisterGenericDecorator(typeof(PostCommitCommandHandlerDecorator<>), typeof(ICommandHandler<>));

        //builder.RegisterGenericDecorator(typeof(TransactionalCommandHandlerDecorator<>), typeof(ICommandHandler<>), fromKey: "aspectsDecorator").Keyed("transactionDecorator", typeof(ICommandHandler<>));
        //     builder.RegisterType<CommandValidator>().AsSelf().As<IValidator>().InstancePerLifetimeScope();
    }
}

// public static ContainerBuilder GetContainerBuilder(bool isWcf)
// {
//     var modelAssembly = Assembly.Load("Tennis.Model");
//     var logicAssembly = Assembly.Load("Tennis.Logic");

//     var builder = new ContainerBuilder();

//     builder.RegisterType<CommandHandlerResolver>().As<ICommandHandlerResolver>().SingleInstance();
//     builder.RegisterType<QueryProcessor>().As<IQueryProcessor>().SingleInstance();

//     //Extrem wichtig damit immer selber dbContext verwendet wird!!! -> sonst funktionieren die Transaktion nicht
//     //muss perRequest sein oder mit Named könnte es auch realisiert werden, damit die einzelnen decorator die selbe Instanz von dbContext erhalten.
//     if (!isWcf)
//         builder.RegisterType<DbContextTennis04>().AsSelf().As<IDbContext>().InstancePerLifetimeScope();
//     else
//         // wcf funktioniert anders hier wird sowieso bei jedem request ein neuer Scoper erzeugt
//         builder.RegisterType<DbContextTennis04>().AsSelf().As<IDbContext>().InstancePerLifetimeScope();

//     builder.RegisterType<Logger>().AsSelf().As<ILogger>().InstancePerDependency();

//     builder.RegisterGeneric(typeof(CommandPermissionChecker<>)).As(typeof(CommandPermissionChecker<>));

//     builder.RegisterType<UserPermissionChecker>().As<IUserPermissionChecker>();

//     // register all defined aspects in logic assembly as self and as implemented interface exluding IAspect, so we can use the defined interfaces in Tennis.Logic to resolve aspects
//     builder.RegisterAssemblyTypes(logicAssembly)
//         .Where(x => x.IsAssignableTo<IAspect>() && x.IsClass && !x.IsAbstract)
//         .AsSelf()
//         .As(x => x.GetInterfaces().Where(y => y != typeof(IAspect))).PropertiesAutowired().InstancePerLifetimeScope();
//     // register aspects context and add aspects to context
//     builder.RegisterType<AspectsContext>().AsSelf().As<IAspectsContext>().InstancePerLifetimeScope()
//         .OnActivated(x =>
//         {
//             x.Instance.AddAfterCommandAspect((IAspect)x.Context.Resolve(logicAssembly.GetTypes().Where(y => y.IsAssignableTo<IAspect>() && y.IsInterface && y.Name == "IInvoiceDocumentAspect").Single()), 1);
//             x.Instance.AddAfterCommandAspect((IAspect)x.Context.Resolve(logicAssembly.GetTypes().Where(y => y.IsAssignableTo<IAspect>() && y.IsInterface && y.Name == "IJournalStatusAspect").Single()), 2);
//             // bookingStatusAspect should be called after journal because bookings rely on journalstate -> user order parameter
//             x.Instance.AddAfterCommandAspect((IAspect)x.Context.Resolve(logicAssembly.GetTypes().Where(y => y.IsAssignableTo<IAspect>() && y.IsInterface && y.Name == "IBookingStatusAspect").Single()), 3);
//         });

//     builder.RegisterType<PostCommitRegistratorImpl>().AsSelf().As<IPostCommitRegistrator>().InstancePerLifetimeScope();

//     builder.RegisterAssemblyTypes(logicAssembly).As(t => t.GetInterfaces().Where(a => a.IsClosedTypeOf(typeof(ICommandValidation<>))));

//     //hat nicht immer funktioniert - jetzt direkt laden
//     //builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies()).As(t => t.GetInterfaces().Where(a => a.IsClosedTypeOf(typeof(ICommandHandler<>))).Select(a => new KeyedService("commandHandler", a)));
//     builder.RegisterAssemblyTypes(logicAssembly).As(t => t.GetInterfaces().Where(a => a.IsClosedTypeOf(typeof(ICommandHandler<>))).Select(a => new KeyedService("commandHandler", a)));
//     builder.RegisterGenericDecorator(typeof(AspectsCommandHandlerDecorator<>), typeof(ICommandHandler<>), fromKey: "commandHandler").Keyed("aspectsDecorator", typeof(ICommandHandler<>));
//     builder.RegisterGenericDecorator(typeof(TransactionalCommandHandlerDecorator<>), typeof(ICommandHandler<>), fromKey: "aspectsDecorator").Keyed("transactionDecorator", typeof(ICommandHandler<>));
//     builder.RegisterGenericDecorator(typeof(PermissionCommandHandlerDecorator<>), typeof(ICommandHandler<>), fromKey: "transactionDecorator").Keyed("permissionDecorator", typeof(ICommandHandler<>));
//     builder.RegisterGenericDecorator(typeof(InfoMessagesCommandHandlerDecorator<>), typeof(ICommandHandler<>), fromKey: "permissionDecorator").Keyed("infoMessageDecorator", typeof(ICommandHandler<>));
//     builder.RegisterGenericDecorator(typeof(PostCommitCommandHandlerDecorator<>), typeof(ICommandHandler<>), fromKey: "infoMessageDecorator").Keyed("postCommitDecorator", typeof(ICommandHandler<>));
//     builder.RegisterGenericDecorator(typeof(ValidationCommandHandlerDecorator<>), typeof(ICommandHandler<>), fromKey: "postCommitDecorator");

//     builder.RegisterType<CommandValidator>().AsSelf().As<IValidator>().InstancePerLifetimeScope();

//     //builder.RegisterAssemblyTypes(Assembly.Load("Tennis.Logic"))
//     //    .As(t => t.GetInterfaces()
//     //        .Where(a => a.IsClosedTypeOf(typeof(IQueryHandler<,>)))
//     //        .Select(a => new KeyedService("queryHandler", a))
//     //    );

//     // TODO: refactor these "StartsWith" registration to use registration using IBaseQueryHandler instead
//     Type typeGetAllQueryHandler = logicAssembly.GetTypes().Where(t => t.Name.StartsWith("GetAllQueryHandler")).SingleOrDefault();
//     if (typeGetAllQueryHandler != null)
//     {
//         builder.RegisterGeneric(typeGetAllQueryHandler)
//             .As(typeof(IQueryHandler<,,>));
//     }

//     Type typeGetByIDQueryHandler = logicAssembly.GetTypes().Where(t => t.Name.StartsWith("GetByIDQueryHandler")).SingleOrDefault();
//     if (typeGetByIDQueryHandler != null)
//     {
//         builder.RegisterGeneric(typeGetByIDQueryHandler)
//             .As(typeof(IQueryHandler<,,>));
//     }

//     Type typeGetMappedByIDQueryHandler = logicAssembly.GetTypes().Where(t => t.Name.StartsWith("GetMappedByIDQueryHandler")).SingleOrDefault();
//     if (typeGetMappedByIDQueryHandler != null)
//     {
//         builder.RegisterGeneric(typeGetMappedByIDQueryHandler)
//             .As(typeof(IQueryHandler<,,>));
//     }

//     foreach (var queryType in logicAssembly.GetTypes().Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IBaseQueryHandler<,,>))))
//     {
//         builder.RegisterGeneric(queryType)
//                 .As(typeof(IQueryHandler<,,>));
//     }

//     builder.RegisterAssemblyTypes(logicAssembly)
//         .AsClosedTypesOf(typeof(IQueryHandler<,,>))
//         .AsImplementedInterfaces();

//     builder.RegisterType<PayPalProvider>().Keyed<IPaymentProvider>(GlobalDefinitions.OnlinePaymentProvider.PayPal).AsSelf();
//     builder.RegisterType<SofortProvider>().Keyed<IPaymentProvider>(GlobalDefinitions.OnlinePaymentProvider.Sofort).AsSelf();
//     builder.RegisterType<StripeProvider>().Keyed<IPaymentProvider>(GlobalDefinitions.OnlinePaymentProvider.Stripe).AsSelf();

//     // TODO: Generic injection of Services and Facades
//     builder.RegisterType<PlannerService>().AsSelf();
//     builder.RegisterType<PlannerFinalizeService>().AsSelf();
//     builder.RegisterType<PlannerConfirmationService>().AsSelf();
//     builder.RegisterType<AppointmentSeriesFacade>().AsSelf();

//     builder.RegisterAssemblyTypes(Assembly.Load("Tennis.CrossCutting"))
//         .Where(x => x.GetInterfaces().Contains(typeof(ITextBlockDecorator)))
//         .Where(x => x.GetCustomAttribute<TextBlockTypeAttribute>() != null)
//         .Keyed<ITextBlockDecorator>(x => x.GetCustomAttribute<TextBlockTypeAttribute>().Type);

//     return builder;
// }