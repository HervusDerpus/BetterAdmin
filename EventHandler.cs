using Smod2;
using Smod2.API;
using Smod2.EventHandlers;
using Smod2.Events;
using System.Collections.Generic;
using System.Linq;

namespace BetterAdmin
{
    class EventHandler : IEventHandlerAdminQuery, IEventHandlerWarheadStopCountdown, IEventHandlerWarheadStartCountdown, IEventHandlerContain106, IEventHandlerGeneratorFinish, IEventHandlerWaitingForPlayers
    {
        private readonly Betteradmin plugin;

        //public string CrtItem;
        public string ARank;
        public string AQuery;
        public int generatorPowerups = 0;
        public int itemrole;

        public EventHandler(Betteradmin plugin) => this.plugin = plugin; //Expression bodies can also be used

        //public void OnShoot(PlayerShootEvent ev)
        //{
        //    if (ev.Player.TeamRole.Role == Role.TUTORIAL)
        //    {
        //        ev.Player.ThrowGrenade(0, true, Smod2.API.Vector.Zero, false, ev.Player.GetPosition(), true, 10);
        //    }
        //}

        public void OnAdminQuery(AdminQueryEvent ev)
        {
            AQuery = ev.Query.ToLower();
            string[] AAQuery = AQuery.Split(' ');
            ARank = ev.Admin.GetRankName();
            if (AAQuery[0].Contains("give") && plugin.idisable == false)
            {
                int.TryParse(AAQuery[2], out itemrole);
                plugin.Info(itemrole.ToString());
                if (plugin.itemblacklist.Contains(itemrole) && !plugin.itemblacklistranks.Contains(ARank))
                {
                    ev.Handled = true;
                    ev.Successful = false;
                }
            }
            else if (AAQuery[0].Contains("forceclass") && plugin.rdisable == false)
            {
                int.TryParse(AAQuery[2], out itemrole);
                if (plugin.roleblacklist.Contains(itemrole) && !plugin.roleblacklistranks.Contains(ARank))
                {
                    ev.Handled = true;
                    ev.Successful = false;
                }
            }
        }

        public void OnContain106(PlayerContain106Event ev)
        {
            if (plugin.anticamp106 == true)
            {
                List<Smod2.API.Door> doors = plugin.Server.Map.GetDoors();
                doors.First(x => x.Name == "106_PRIMARY").Locked = true;
                doors.First(x => x.Name == "106_SECONDARY").Locked = true;
                doors.First(x => x.Name == "106_BOTTOM").Locked = true;
                doors.First(x => x.Name == "106_PRIMARY").Open = true;
                doors.First(x => x.Name == "106_SECONDARY").Open = true;
                doors.First(x => x.Name == "106_BOTTOM").Open = true;
            }
        }

        public void OnGeneratorFinish(GeneratorFinishEvent ev)
        {
            generatorPowerups++; ;

            if (generatorPowerups == 5 && plugin.anticamp079 == true)
            {
                List<Smod2.API.Door> doors = plugin.Server.Map.GetDoors();
                doors.First(x => x.Name == "079_FIRST").Locked = true;
                doors.First(x => x.Name == "079_SECOND").Locked = true;
                doors.First(x => x.Name == "079_FIRST").Open = true;
                doors.First(x => x.Name == "079_SECOND").Open = true;
            }
        }

        public void OnStartCountdown(WarheadStartEvent ev)
        {
            if (plugin.anticampnuke == true)
            {
                List<Smod2.API.Door> doors = plugin.Server.Map.GetDoors();
                doors.First(x => x.Name == "NUKE_SURFACE").Locked = false;
                doors.First(x => x.Name == "NUKE_SURFACE").Open = false;
            }
        }

        public void OnStopCountdown(WarheadStopEvent ev)
        {
            if (plugin.anticampnuke == true)
            {
                List<Smod2.API.Door> doors = plugin.Server.Map.GetDoors();
                doors.First(x => x.Name == "NUKE_SURFACE").Locked = true;
                doors.First(x => x.Name == "NUKE_SURFACE").Open = true;
            }
        }

        public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
        {
            plugin.RefreshConfig();
            generatorPowerups = 0;
        }
    }
}
