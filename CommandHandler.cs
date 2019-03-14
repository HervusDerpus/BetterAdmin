using System.Collections.Generic;
using System.Linq;
using Smod2.EventHandlers;
using Smod2.Events;

namespace BetterAdmin
{
	class CommandHandler : IEventHandlerAdminQuery, IEventHandlerCallCommand
    {
		private readonly BetterAdmin plugin;
        public string Command;

        public CommandHandler(BetterAdmin plugin)
		{
			//Constructor passing plugin refrence to this class
			this.plugin = plugin;
		}

        public void OnAdminQuery(AdminQueryEvent ev)
        {
            ev.Query.ToString();
            ev.Handled = true;
            ev.Admin.SendConsoleMessage("Admin " + ev.Admin.Name + " Triggered " + ev.Query.ToString());
        }

        public void OnCallCommand(PlayerCallCommandEvent ev)
        {
            ev.Player.PersonalBroadcast(10, ev.Command, false);
        }
    }
}
