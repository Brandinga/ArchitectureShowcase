using Autofac;
using Autofac.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TKWData;
using TKWLogic;

public static class TestHelper
{
    //private static IContainer? _container;
    public static IDbContextTransaction? Transaction { get; private set; }
    public static ILifetimeScope Scope { get; private set; } = null!;
    private static readonly SqliteConnection _sqliteConnection = new("Filename=:memory:");
    //private static readonly SqliteConnection _sqliteConnection = new("Data Source=..\\TKWDatabase.db");
    public static void Initialize()
    {
        //var db = Services.GetRequiredService<IDbContextFactory<TodoDbContext>>().CreateDbContext();
        
        _sqliteConnection.Open();
        //DbContext.Database.EnsureCreated();
    }

    public static void InitializeTest()
    {
        if (Scope != null)
        {
            Scope.Dispose();
        }
        Scope = Container.BeginLifetimeScope();
        DbContext.Database.EnsureCreated();
        Transaction = DbContext.Database.BeginTransaction();
    }

    public static void CleanupTest()
    {
        if (Transaction != null)
        {
            Transaction.Rollback();
        }
        Transaction = null;
    }

    public static TKWAppContext CreateT04Context()
    {
        //return new TKWAppContext(new TKWIntegrationTestContextCreator(Context.Mitglieder.Where(m => m.Vorname.Equals(firstName) && m.Name.Equals(lastName)).First().PrimaryKey, Context, applicationType));
        return new TKWAppContext(new TKWIntegrationTestContextCreator());
    }

    public static void RefreshAllEntities()
    {
        var changedEntriesCopy = TestHelper.DbContext.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added ||
                        e.State == EntityState.Modified ||
                        e.State == EntityState.Deleted ||
                        e.State == EntityState.Unchanged)
            .ToList();

        foreach (var entry in changedEntriesCopy)
        {
            entry.Reload();
        }
    }

    public static void DetachAllEntities()
    {
        var changedEntriesCopy = TestHelper.DbContext.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added ||
                        e.State == EntityState.Modified ||
                        e.State == EntityState.Deleted ||
                        e.State == EntityState.Unchanged)
            .ToList();

        foreach (var entry in changedEntriesCopy)
            entry.State = EntityState.Detached;
    }

    public static TKWDbContext DbContext
    {
        get
        {
            return Scope.Resolve<TKWDbContext>();
        }
    }
    public static ICommandHandlerResolver Resolver
    {
        get
        {
            return Scope.Resolve<ICommandHandlerResolver>();
        }
    }

    public static IContainer Container
    {
        get
        {
            return AutofacModule.BuildContainer(_sqliteConnection);
        }
    }
}

