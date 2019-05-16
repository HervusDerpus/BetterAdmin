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

			switch (AAQuery[0])
			{
				case "give":
					if (plugin.ItemBlocker)
					{
						if (plugin.ItemBlockerItems.Contains(AAQuery[2]) && !plugin.ItemBlockerRanks.Contains(ARank))
						{
							adminquery.Handled = true;
							adminquery.Successful = false;
						}
					}
					break;

				case "fc":
				case "forceclass":
					if (plugin.RoleBlocker)
					{
						if (plugin.RoleBlockerRoles.Contains(AAQuery[2]) && !plugin.RoleBlockerRanks.Contains(ARank))
						{
							adminquery.Handled = true;
							adminquery.Successful = false;
						}
					}
					break;

				case "flash":
				case "grenade":
					if(plugin.GrenadeFlash && !plugin.GrenadeFlashRanks.Contains(ARank))
					{
						adminquery.Handled = true;
						adminquery.Successful = false;
					}
					break;

				case "ban":
					int.TryParse(AAQuery[2], out int Banlength);
					if (Banlength > 10080 && plugin.SevenDaysRanks.Contains(ARank))
					{
						adminquery.Handled = true;
						adminquery.Successful = false;
					}
					if (Banlength > 20160 && plugin.FourteenDaysRanks.Contains(ARank))
					{
						adminquery.Handled = true;
						adminquery.Successful = false;
					}
					if (Banlength > 43200 && plugin.ThirtyDaysRanks.Contains(ARank))
					{
						adminquery.Handled = true;
						adminquery.Successful = false;
					}
					break;

				//Facility Management Permissions
				case "broadcast":
				case "bc":
				case "alert":
				case "broadcastmono":
				case "bcmono":
				case "alertmono":
				case "CLEARBC":
				case "BCCLEAR":
				case "CLEARALERT":
				case "ALERTCLEAR":
					if (plugin.Broadcast && !plugin.BroadcastRanks.Contains(ARank) || (plugin.FacilityOverride.Contains (ARank) || plugin.OverrideRanks.Contains(ARank)))
					{
						adminquery.Handled = true;
						adminquery.Successful = false;
					}
					break;

				case "mute":
				case "unmute":
					if(plugin.DisableMuting || (plugin.Mute && !plugin.MuteRanks.Contains(ARank) || (plugin.FacilityOverride.Contains(ARank) || plugin.OverrideRanks.Contains(ARank))))
					{
						adminquery.Handled = true;
						adminquery.Successful = false;
					}
					break;

				case "imute":
				case "iunmute":
					if (plugin.Imute && !plugin.ImuteRanks.Contains(ARank) || (plugin.FacilityOverride.Contains(ARank) || plugin.OverrideRanks.Contains(ARank)))
					{
						adminquery.Handled = true;
						adminquery.Successful = false;
					}
					break;

				case "o":
				case "open":
				case "c":
				case "close":
					if (plugin.Doors && !plugin.OpenCloseRanks.Contains(ARank) || (plugin.FacilityOverride.Contains(ARank) || plugin.OverrideRanks.Contains(ARank)))
					{
						adminquery.Handled = true;
						adminquery.Successful = false;
					}
					break;

				case "l":
				case "lock":
				case "ul":
				case "unlock":
					if (plugin.Doors && !plugin.LockUnlockRanks.Contains(ARank) || (plugin.FacilityOverride.Contains(ARank) || plugin.OverrideRanks.Contains(ARank)))
					{
						adminquery.Handled = true;
						adminquery.Successful = false;
					}
					break;

				case "destroy":
					if (plugin.Doors && !plugin.DestroyRanks.Contains(ARank) || (plugin.FacilityOverride.Contains(ARank) || plugin.OverrideRanks.Contains(ARank)))
					{
						adminquery.Handled = true;
						adminquery.Successful = false;
					}
					break;
				case "bm":
				case "bypass":
					if (plugin.Bypass && !plugin.BypassRanks.Contains(ARank) || (plugin.FacilityOverride.Contains(ARank) || plugin.OverrideRanks.Contains(ARank)))
					{
						adminquery.Handled = true;
						adminquery.Successful = false;
					}
					break;

				case "ld":
				case "lockdown":
					if (plugin.Lockdown && !plugin.LockdownRanks.Contains(ARank) || (plugin.FacilityOverride.Contains(ARank) || plugin.OverrideRanks.Contains(ARank)))
					{
						adminquery.Handled = true;
						adminquery.Successful = false;
					}
					break;

				case "cassie":
					if (plugin.Cassie && !plugin.CassieRanks.Contains(ARank) || (plugin.FacilityOverride.Contains(ARank) || plugin.OverrideRanks.Contains(ARank)))
					{
						adminquery.Handled = true;
						adminquery.Successful = false;
					}
					break;

				default:
					adminquery.Handled = true;
					adminquery.Successful = true;
					break;
			}
		}

		public void OnPlayerJoin(PlayerJoinEvent player)
		{
			if (plugin.GmodResSlot && player.Player.GetAuthToken().Contains("Bypass bans: YES") && !ReservedSlot.ContainsSteamID(player.Player.SteamId))
			{
				plugin.Info("Added " + player.Player.Name + " to the reserved slots");
				new ReservedSlot(player.Player.IpAddress, player.Player.SteamId, " Studio Global Security - " + player.Player.Name).AppendToFile();
				player.Player.SendConsoleMessage("You've been given a reserved slot - Global Security");
			}
			else if (plugin.StaffResSlot && player.Player.GetAuthToken().Contains("Bypass geo restrictions: YES") && !ReservedSlot.ContainsSteamID(player.Player.SteamId))
			{
				plugin.Info("Added " + player.Player.Name + " to the reserved slots");
				new ReservedSlot(player.Player.IpAddress, player.Player.SteamId, " Studio Staff - " + player.Player.Name).AppendToFile();
				player.Player.SendConsoleMessage("You've been given a reserved slot - Studio Staff");
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
			if (plugin.AnticampNuke)
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
			if (plugin.AnticampNuke)
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
			GeneratorPowerups = 0;
		}
	}
}
