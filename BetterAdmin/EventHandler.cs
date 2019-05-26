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
			if (!adminquery.Admin.GetAuthToken().Contains("Bypass bans: YES"))
			{
				string AQuery = adminquery.Query.ToLower();
				string[] AAQuery = AQuery.Split(' ');
				string ARank = adminquery.Admin.GetRankName();

				switch (AAQuery[0])
				{
					#region Give/ForceClass blockers and Flash/Grenade blocker
					case "give":
						if (plugin.ItemBlocker)
						{
							if (plugin.ItemBlockerItems.Contains(AAQuery[2]) && (!plugin.ItemBlockerRanks.Contains(ARank) && !plugin.OverrideRanks.Contains(ARank)))
							{
								FailedGive(adminquery, AAQuery[2]);
							}
						}
						break;

					case "fc":
					case "forceclass":
						if (plugin.RoleBlocker)
						{
							if (plugin.RoleBlockerRoles.Contains(AAQuery[2]) && (!plugin.RoleBlockerRanks.Contains(ARank) && !plugin.OverrideRanks.Contains(ARank)))
							{
								FailedForceClass(adminquery, AAQuery[2]);
							}
						}
						break;

					case "flash":
					case "grenade":
						if (plugin.GrenadeFlash && (!plugin.GrenadeFlashRanks.Contains(ARank) && !plugin.OverrideRanks.Contains(ARank)))
						{
							FailedAdminQuery(adminquery, AAQuery[0]);
						}
						break;
					#endregion

					#region Ban handler
					case "ban":
						int.TryParse(AAQuery[2], out int Banlength);
						if (Banlength > 10079 && Banlength < 20160 && plugin.SevenDaysRanks.Contains(ARank) && !plugin.OverrideRanks.Contains(ARank))
						{
							adminquery.Handled = true;
							adminquery.Successful = false;
							adminquery.Admin.PersonalBroadcast(3, "<color=#ff0000ff>You are not permitted to ban for longer than 7 days!</color>", false);
						}
						if (Banlength > 20159 && Banlength < 43200 && plugin.FourteenDaysRanks.Contains(ARank) && !plugin.OverrideRanks.Contains(ARank))
						{
							adminquery.Handled = true;
							adminquery.Successful = false;
							adminquery.Admin.PersonalBroadcast(3, "<color=#ff0000ff>You are not permitted to ban for longer than 14 days!</color>", false);
						}
						if (Banlength > 43199 && plugin.ThirtyDaysRanks.Contains(ARank) && !plugin.OverrideRanks.Contains(ARank))
						{
							adminquery.Handled = true;
							adminquery.Successful = false;
							adminquery.Admin.PersonalBroadcast(3, "<color=#ff0000ff>You are not permitted to ban for longer than 30 days!</color>", false);
						}
						break;
					#endregion

					//Facility Management Permissions
					#region Broadcast and Cassie
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
						if (plugin.Broadcast && !plugin.BroadcastRanks.Contains(ARank) && !plugin.FacilityOverride.Contains(ARank) && !plugin.OverrideRanks.Contains(ARank))
						{
							FailedAdminQuery(adminquery, AAQuery[0]);
						}
						break;

					case "cassie":
						if (plugin.Cassie && !plugin.CassieRanks.Contains(ARank) && !plugin.FacilityOverride.Contains(ARank) && !plugin.OverrideRanks.Contains(ARank))
						{
							FailedAdminQuery(adminquery, AAQuery[0]);
						}
						break;
					#endregion

					#region Mutes
					case "mute":
					case "unmute":
						if (plugin.DisableMuting || (plugin.Mute && !plugin.MuteRanks.Contains(ARank) && !plugin.FacilityOverride.Contains(ARank) && !plugin.OverrideRanks.Contains(ARank)))
						{
							FailedAdminQuery(adminquery, AAQuery[0]);
						}
						break;

					case "imute":
					case "iunmute":
						if (plugin.Imute && (!plugin.ImuteRanks.Contains(ARank) && !plugin.FacilityOverride.Contains(ARank) && !plugin.OverrideRanks.Contains(ARank)))
						{
							FailedAdminQuery(adminquery, AAQuery[0]);
						}
						break;
					#endregion

					#region Door controls
					case "o":
					case "open":
					case "c":
					case "close":
						if (plugin.Doors && (!plugin.OpenCloseRanks.Contains(ARank) && !plugin.FacilityOverride.Contains(ARank) && !plugin.OverrideRanks.Contains(ARank)))
						{
							FailedAdminQuery(adminquery, AAQuery[0]);
						}
						break;

					case "l":
					case "lock":
					case "ul":
					case "unlock":
						if (plugin.Doors && (!plugin.LockUnlockRanks.Contains(ARank) && !plugin.FacilityOverride.Contains(ARank) && !plugin.OverrideRanks.Contains(ARank)))
						{
							FailedAdminQuery(adminquery, AAQuery[0]);
						}
						break;

					case "destroy":
						if (plugin.Doors && (!plugin.DestroyRanks.Contains(ARank) && !plugin.FacilityOverride.Contains(ARank) && !plugin.OverrideRanks.Contains(ARank)))
						{
							FailedAdminQuery(adminquery, AAQuery[0]);
						}
						break;
					#endregion

					#region Bypass Lockdown and Heal
					case "bm":
					case "bypass":
						if (plugin.Bypass && (!plugin.BypassRanks.Contains(ARank) && !plugin.FacilityOverride.Contains(ARank) && !plugin.OverrideRanks.Contains(ARank)))
						{
							FailedAdminQuery(adminquery, AAQuery[0]);
						}
						break;

					case "ld":
					case "lockdown":
						if (plugin.Lockdown && (!plugin.LockdownRanks.Contains(ARank) && !plugin.FacilityOverride.Contains(ARank) && !plugin.OverrideRanks.Contains(ARank)))
						{
							FailedAdminQuery(adminquery, AAQuery[0]);
						}
						break;

					case "heal":
					case "hp":
						if (plugin.Heal && (!plugin.HealRanks.Contains(ARank) && !plugin.FacilityOverride.Contains(ARank) && !plugin.OverrideRanks.Contains(ARank)))
						{
							FailedAdminQuery(adminquery, AAQuery[0]);
						}
						break;
					#endregion

					default:
						break;
				}
			}
		}

		public void OnPlayerJoin(PlayerJoinEvent player)
		{
			if (plugin.GmodResSlot && player.Player.GetAuthToken().Contains("Bypass bans: YES") && !ReservedSlot.ContainsSteamID(player.Player.SteamId))
			{
				plugin.Info("Added " + player.Player.Name + " to the reserved slots");
				new ReservedSlot(player.Player.IpAddress, player.Player.SteamId, " Studio Global Security - " + player.Player.Name).AppendToFile();
				player.Player.SendConsoleMessage("You've been given an automatic reserved slot via BetterAdmin - Global Security");
			}
			else if (plugin.StaffResSlot && player.Player.GetAuthToken().Contains("Bypass geo restrictions: YES") && !ReservedSlot.ContainsSteamID(player.Player.SteamId))
			{
				plugin.Info("Added " + player.Player.Name + " to the reserved slots");
				new ReservedSlot(player.Player.IpAddress, player.Player.SteamId, " Studio Staff - " + player.Player.Name).AppendToFile();
				player.Player.SendConsoleMessage("You've been given an automatic reserved slot via BetterAdmin - Studio Staff");
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
				Smod2.API.Door surface = doors.First(x => x.Name == "NUKE_SURFACE");
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
				Smod2.API.Door surface = doors.First(x => x.Name == "NUKE_SURFACE");
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


		void FailedGive(AdminQueryEvent adminquery, string item)
		{
			adminquery.Handled = true;
			adminquery.Successful = false;
			adminquery.Admin.PersonalBroadcast(3, $"<color=#ff0000ff>You are not permitted to give players item {item}!</color>", false);
		}
		void FailedForceClass(AdminQueryEvent adminquery, string role)
		{
			adminquery.Handled = true;
			adminquery.Successful = false;
			adminquery.Admin.PersonalBroadcast(3, $"<color=#ff0000ff>You are not permitted to forceclass players to role {role}!</color>", false);
		}
		void FailedAdminQuery(AdminQueryEvent adminquery, string Command)
		{
			adminquery.Handled = true;
			adminquery.Successful = false;
			adminquery.Admin.PersonalBroadcast(3, $"<color=#ff0000ff>You are not permitted to use {Command}!</color>", false);
		}
	}
}
