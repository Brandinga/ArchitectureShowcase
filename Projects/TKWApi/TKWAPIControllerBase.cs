using Microsoft.AspNetCore.Mvc;
using TKWLogic;

public class TKWAPIControllerBase : ControllerBase
{
    public ICommandHandlerResolver CommandDispatcher { get; set; }
    // TODO: nullable ... do i need the currentcommand really
    //       --> CommandDispatcher cant detect type if not assigned here??
    public ICommand CurrentCommand { get; set; } = null!;

    //public IQueryProcessor QueryProcessor { get; set; }

    private readonly Dictionary<String, Object> _messageProperties = new Dictionary<String, Object>();

    /// <summary>
    /// WICHTIG: es darf nicht T04Core verwendet werden!!! Grund: wenn ExternAuthorizeAttribute verwendet wird ist kein UserProfile vorhanden (es wird keine DB-Session erstellt)
    /// deshalb erstelle ich hier je nachdem ob der ExternAuthorizeFilter aktiv ist einen "Context" neu
    /// </summary>
    //public TKWControllerBase(ICommandHandlerResolver commandDispatcher, IQueryProcessor queryProcessor)
    public TKWAPIControllerBase(ICommandHandlerResolver commandDispatcher)
    {
        this.CommandDispatcher = commandDispatcher;
        //this.QueryProcessor = queryProcessor;
    }
}