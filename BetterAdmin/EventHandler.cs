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

		int GeneratorPowerups;

		public EventHandler(Betteradmin plugin) => this.plugin = plugin;

		public void OnAdminQuery(AdminQueryEvent adminquery)
		{
			string AQuery = adminquery.Query.ToLower();
			string[] AAQuery = AQuery.Split(' ');
			string ARank = adminquery.Admin.GetRankName();

			if (AAQuery[0].Contains("give") && plugin.IBlock)
			{
				int.TryParse(AAQuery[2], out int item);
				if (plugin.Itemblacklist.Contains(item) && !plugin.Itemblacklistranks.Contains(ARank))
				{
					adminquery.Handled = true;
					adminquery.Successful = false;
				}
			}
			else if (AAQuery[0].Contains("forceclass") && plugin.RBlock)
			{
				int.TryParse(AAQuery[2], out int role);
				if (plugin.Roleblacklist.Contains(role) && !plugin.Roleblacklistranks.Contains(ARank))
				{
					adminquery.Handled = true;
					adminquery.Successful = false;
				}
			}
			else if (AAQuery[0].Contains("grenade") && plugin.Grenadeflash && !plugin.Grenadeflashranks.Contains(ARank))
			{
				adminquery.Handled = true;
				adminquery.Successful = false;
			}
			else if (AAQuery[0].Contains("flash") && plugin.Grenadeflash && !plugin.Grenadeflashranks.Contains(ARank))
			{
				adminquery.Handled = true;
				adminquery.Successful = false;
			}
			else if (AAQuery[0].Contains("ban"))
			{
				int.TryParse(AAQuery[2], out int Banlength);
				if (Banlength > 10080 && plugin.Sevendays.Contains(ARank))
				{
					adminquery.Handled = true;
					adminquery.Successful = false;
				}
				if (Banlength > 20160 && plugin.Fourteendays.Contains(ARank))
				{
					adminquery.Handled = true;
					adminquery.Successful = false;
				}
				if (Banlength > 43200 && plugin.Thirtydays.Contains(ARank))
				{
					adminquery.Handled = true;
					adminquery.Successful = false;
				}
			}
		}

		public void OnPlayerJoin(PlayerJoinEvent player)
		{
			if (plugin.Gmodresslot && player.Player.GetAuthToken().Contains("Bypass bans: YES") && !ReservedSlot.ContainsSteamID(player.Player.SteamId))
			{
				plugin.Info("Added " + player.Player.Name + " to the reserved slots");
				new ReservedSlot(player.Player.IpAddress, player.Player.SteamId, " Studio Global Security - " + player.Player.Name).AppendToFile();
			}
			else if (plugin.Staffresslot && player.Player.GetAuthToken().Contains("Bypass geo restrictions: YES") && !ReservedSlot.ContainsSteamID(player.Player.SteamId))
			{
				plugin.Info("Added " + player.Player.Name + " to the reserved slots");
				new ReservedSlot(player.Player.IpAddress, player.Player.SteamId, " Studio Staff - " + player.Player.Name).AppendToFile();
			}

			if (plugin.Latejoin && plugin.Server.Round.Duration > 0 && plugin.Server.Round.Duration < plugin.Latejoinduration)
			{
				player.Player.ChangeRole(Role.CLASSD);
				player.Player.PersonalBroadcast(5, "You joined late, so was spawned as a D-class", false);
			}
		}
		public void OnContain106(PlayerContain106Event contevent)
		{
			if (plugin.Anticamp106)
			{
				List<Smod2.API.Door> doors = plugin.Server.Map.GetDoors();
				Smod2.API.Door primary = doors.First(x => x.Name == "106_PRIMARY");
				Smod2.API.Door secondary = doors.First(x => x.Name == "106_SECONDARY");
				Smod2.API.Door bottom = doors.First(x => x.Name == "106_BOTTOM");
				if (primary != null)
				{
					primary.Open = true;
					primary.Locked = true;
				}
				if (secondary != null)
				{
					secondary.Open = true;
					secondary.Locked = true;
				}
				if (bottom != null)
				{
					bottom.Open = true;
					bottom.Locked = true;
				}
			}
		}
		public void OnGeneratorFinish(GeneratorFinishEvent genevent)
		{
			GeneratorPowerups++;
			if (GeneratorPowerups == 5 && plugin.Anticamp079)
			{
				List<Smod2.API.Door> doors = plugin.Server.Map.GetDoors();
				Smod2.API.Door primary = doors.First(x => x.Name == "079_FIRST");
				Smod2.API.Door secondary = doors.First(x => x.Name == "079_SECOND");
				if (primary != null)
				{
					primary.Open = true;
					primary.Locked = true;
				}
				if (secondary != null)
				{
					secondary.Open = true;
					secondary.Locked = true;
				}
			}
		}
		public void OnStartCountdown(WarheadStartEvent whstart)
		{
			if (plugin.Anticampnuke)
			{
				List<Smod2.API.Door> doors = plugin.Server.Map.GetDoors();
				Smod2.API.Door surface = doors.First(x => x.Name == "079_FIRST");
				if (surface != null)
				{
					surface.Open = true;
					surface.Locked = false;
				}
			}
		}
		public void OnStopCountdown(WarheadStopEvent whstop)
		{
			if (plugin.Anticampnuke)
			{
				List<Smod2.API.Door> doors = plugin.Server.Map.GetDoors();
				Smod2.API.Door surface = doors.First(x => x.Name == "079_FIRST");
				if (surface != null)
				{
					surface.Open = true;
					surface.Locked = true;
				}
			}
		}
		public void OnWaitingForPlayers(WaitingForPlayersEvent wfp)
		{
			plugin.RefreshConfig();
			GeneratorPowerups = 0;
		}
	}
}
