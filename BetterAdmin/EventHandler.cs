using Smod2;
using Smod2.API;
using Smod2.EventHandlers;
using Smod2.Events;
using System.Collections.Generic;
using System.Linq;

namespace BetterAdmin
{
	class EventHandler : IEventHandlerAdminQuery, IEventHandlerWarheadStopCountdown, IEventHandlerWarheadStartCountdown, IEventHandlerContain106, IEventHandlerGeneratorFinish, IEventHandlerWaitingForPlayers, IEventHandlerPlayerJoin
	{
		private readonly Betteradmin plugin;

		public string ARank;
		public string AQuery;
		public int GeneratorPowerups;
		public int Itemrole;

		public EventHandler(Betteradmin plugin) => this.plugin = plugin; 

		public void OnPlayerJoin(PlayerJoinEvent player)
		{
			if (plugin.Gmodresslot == true && player.Player.GetAuthToken().Contains("Bypass bans: YES") && !ReservedSlot.ContainsSteamID(player.Player.SteamId))
			{
				plugin.Info("Added " + player.Player.Name + " to the reserved slots");
				new ReservedSlot(player.Player.IpAddress, player.Player.SteamId, " Studio Global Security - " + player.Player.Name).AppendToFile();
			}
			else if (plugin.Staffresslot == true && player.Player.GetAuthToken().Contains("Bypass geo restrictions: YES") && !ReservedSlot.ContainsSteamID(player.Player.SteamId))
			{
				plugin.Info("Added " + player.Player.Name + " to the reserved slots");
				new ReservedSlot(player.Player.IpAddress, player.Player.SteamId, " Studio Staff - " + player.Player.Name).AppendToFile();
			}

			if(plugin.Latejoin == true && plugin.Server.Round.Duration >= 30)
			{
				player.Player.ChangeRole(Role.CLASSD, true, true, false, false);
			}
		}

		public void OnAdminQuery(AdminQueryEvent adminquery)
		{
			AQuery = adminquery.Query.ToLower();
			string[] AAQuery = AQuery.Split(' ');
			ARank = adminquery.Admin.GetRankName();
			if (AAQuery[0].Contains("give") && plugin.Idisable == false)
			{
				int.TryParse(AAQuery[2], out Itemrole);
				plugin.Info(Itemrole.ToString());
				if (plugin.Itemblacklist.Contains(Itemrole) && !plugin.Itemblacklistranks.Contains(ARank))
				{
					adminquery.Handled = true;
					adminquery.Successful = false;
				}
			}
			else if (AAQuery[0].Contains("forceclass") && plugin.Rdisable == false)
			{
				int.TryParse(AAQuery[2], out Itemrole);
				if (plugin.Roleblacklist.Contains(Itemrole) && !plugin.Roleblacklistranks.Contains(ARank))
				{
					adminquery.Handled = true;
					adminquery.Successful = false;
				}
			}
		}

		public void OnContain106(PlayerContain106Event contevent)
		{
			if (plugin.Anticamp106 == true)
			{
				List<Smod2.API.Door> doors = plugin.Server.Map.GetDoors();
				try { doors.First(x => x.Name == "106_PRIMARY").Locked = true; } catch { plugin.Warn("UNABLE TO OPEN 106_PRIMARY"); }
				try { doors.First(x => x.Name == "106_PRIMARY").Open = true; } catch { plugin.Warn("UNABLE TO LOCK 106_PRIMARY"); }
				try { doors.First(x => x.Name == "106_SECONDARY").Locked = true; } catch { plugin.Warn("UNABLE TO OPEN 106_SECONDARY"); }
				try { doors.First(x => x.Name == "106_SECONDARY").Open = true; } catch { plugin.Warn("UNABLE TO LOCK 106_SECONDARY"); }
				try { doors.First(x => x.Name == "106_BOTTOM").Locked = true; } catch { plugin.Warn("UNABLE TO OPEN 106_BOTTOM"); }
				try { doors.First(x => x.Name == "106_BOTTOM").Open = true; } catch { plugin.Warn("UNABLE TO LOCK 106_BOTTOM"); }
			}
		}

		public void OnGeneratorFinish(GeneratorFinishEvent genevent)
		{
			GeneratorPowerups++;

			if (GeneratorPowerups == 5 && plugin.Anticamp079 == true)
			{
				List<Smod2.API.Door> doors = plugin.Server.Map.GetDoors();
				doors.First(x => x.Name == "079_FIRST").Locked = true;
				doors.First(x => x.Name == "079_FIRST").Open = true;
				doors.First(x => x.Name == "079_SECOND").Locked = true;
				doors.First(x => x.Name == "079_SECOND").Open = true;
			}
		}

		public void OnStartCountdown(WarheadStartEvent whstart)
		{
			if (plugin.Anticampnuke == true)
			{
				List<Smod2.API.Door> doors = plugin.Server.Map.GetDoors();
				doors.First(x => x.Name == "NUKE_SURFACE").Locked = false;
				doors.First(x => x.Name == "NUKE_SURFACE").Open = false;

			}
		}

		public void OnStopCountdown(WarheadStopEvent whstop)
		{
			if (plugin.Anticampnuke == true)
			{
				List<Smod2.API.Door> doors = plugin.Server.Map.GetDoors();
				doors.First(x => x.Name == "NUKE_SURFACE").Locked = true;
				doors.First(x => x.Name == "NUKE_SURFACE").Open = true;
			}
		}

		public void OnWaitingForPlayers(WaitingForPlayersEvent wfp)
		{
			plugin.RefreshConfig();
			GeneratorPowerups = 0;
		}
    }
}
