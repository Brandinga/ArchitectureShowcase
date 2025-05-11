using TKWData.Models;

public class TKWAppContext
{
    public TKWAppContext(ITKWAppContextCreator creator)
    {
        UserRights = creator.SetUserRights();
    }
    public RechteVerwaltung UserRights { get; } = null!;
}

public interface ITKWAppContextCreator
{
    RechteVerwaltung SetUserRights();
}

public class RechteVerwaltung
{
    public int OrganisationId { get; private set; }
    public Guid? UserId { get; private set; }
    public UserPermissions Role { get; private set; } = UserPermissions.Guest;

    // TODO: should not use DataModels --> dont allow lazyLoading to be protected
    public RechteVerwaltung(User user)
    {
        UserId = user.Id;
    }

    public RechteVerwaltung()
    {

    }
}


public enum UserPermissions
{
    Guest = -1,
    Member = 0,
    Administrator = 1
}