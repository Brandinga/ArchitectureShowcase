using TKWData;

public class TKWWebApiContextCreator : ITKWAppContextCreator
{
    RechteVerwaltung _userRights;

    /// <summary>
    /// Initializes a new instance of the AspxContext class.
    /// </summary>
    public TKWWebApiContextCreator(TKWDbContext dbContext)
    {
        Guid sessionId = Guid.Empty;

        // var session = null; // dbContext.Session.Where(w => w.Id == sessionId).FirstOrDefault();

        // if (session != null)
        // {
        //     _userRights = new RechteVerwaltung(session.User);
        // } else
        // {
        //     _userRights = new RechteVerwaltung();
        // }

        _userRights = new RechteVerwaltung();
        // Model.Mitglied user = null;
        // URLRoutingDefinition url = null;
        // Permissions permission = Permissions.Guest;
        // var urlPath = HttpContextHelper.GetURLPathFromHttpHeader();

        // // über den URLPath wird die uiversion, kalenderversion, applicationtyp(online/touch) usw. ermittelt
        // // wenn kein HTTPHeader vorhanden ist wird die "default" route für die organisation aus der tabelle organisation.webadresse ermittelt
        // if (!string.IsNullOrEmpty(urlPath))
        // {
        //     url = GlobalApplicationState.GetURLRouting(urlPath);
        // }
        // else
        // {
        //     // 20190913: diesen fallback hinzugefügt
        //     var state = GlobalApplicationState.GetState(idOrg);
        //     if (state != null)
        //     {
        //         url = GlobalApplicationState.GetURLRouting(state.WebAdresseURLPath);
        //     }
        // }

        // // wenn noch immer keine url ermittelt werden konnte, wird ui version 3 & kalender version 2 in rechteverwaltung gesetzt
        // // es kann dann zwar die website geladen werden, das menü ist aber falsch --> es kann nicht navigiert werden --> da kein urlPath in der rechteverwaltung gesetzt ist
        // if (url == null)
        // {
        //     // 20190913
        //     // --> ich löse dieses Problem nun indem ich von der IDOrg die state.WebAdresseURLPath verwende
        //     // bei folgenden UserAgents wird mein HTTPHeader nicht mit versendet
        //     // 1) bingpreview bot
        //     // 2) applebot
        //     // 3) HTTP_X_FORWARDED_FOR --> 2-3mal in 2 Monaten aufgetreten --> User die hinter einem httpProxy sind
        //     LoggingHelper logHelper = LoggingHelper.GetInstance();
        //     logHelper.WriteApplicationError(idOrg, LogApplikationsBereich.WebApiContext, new ArgumentException("There should be an URLPath http-Header present"));
        // }

        // var org = T04Core.DbContext.Organisation
        //                 .Include(o => o.OrganisationBerechtigung)
        //                 .Include(o => o.OrganisationSettings)
        //                 .Where(u => u.PrimaryKey == idOrg).AsNoTracking().SingleOrDefault();

        // var userId = HttpContextHelper.GetUserIdFromClaim();
        // if (userId != null)
        // {
        //     user = T04Core.DbContext.Mitglieder.Where(u => u.PrimaryKey == userId).AsNoTracking().SingleOrDefault();
        //     //wenn ein spielerzusammengelegt wird, ist der spieler nicht mehr vorhanden
        //     permission = user == null ? Permissions.Guest : (Permissions)user?.MasterBlaster;
        // }

        // var courts = T04Core.DbContext.Tennisplatz.Include(i => i.PlatzGruppeNavigation).Include(i => i.PlatzGruppeNavigation.OptionenBuchungTouch).Where(m => m.IDOrg == idOrg).AsNoTracking().ToList();
        // var seasons = T04Core.DbContext.Saison.Where(m => m.IDOrg == idOrg).AsNoTracking().ToList();

        //_userRights = new RechteVerwaltung((int)permission, org, url, courts, seasons, user);
    }

    public RechteVerwaltung SetUserRights()
    {
        return _userRights;
    }
}