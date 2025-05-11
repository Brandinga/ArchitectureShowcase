using TKWData;

public class TKWIntegrationTestContextCreator : ITKWAppContextCreator
{
    RechteVerwaltung _userRights;

    /// <summary>
    /// Initializes a new instance of the AspxContext class.
    /// </summary>
    public TKWIntegrationTestContextCreator()
    {
        _userRights = new RechteVerwaltung();
    }

    public RechteVerwaltung SetUserRights()
    {
        return _userRights;
    }
}