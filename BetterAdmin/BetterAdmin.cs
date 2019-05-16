using Smod2;
using Smod2.API;
using Smod2.Attributes;
using Smod2.Commands;
using Smod2.Config;
using System.Linq;

namespace BetterAdmin
{
	[PluginDetails(
		author = "Phoenix",
		name = "BetterAdmin",
		description = "Plugin to help with server administration",
		id = "phoenix.betteradmin",
		version = "1.3.0",
		configPrefix = "ba",
		SmodMajor = 3,
		SmodMinor = 4,
		SmodRevision = 0
		)]
	class Betteradmin : Plugin
	{
		[ConfigOption]
		//Automatic-stuffs
		public readonly bool StaffResSlot = false;
		public readonly bool GmodResSlot = true;
		public readonly bool Anticamp106 = true;
		public readonly bool Anticamp079 = true;
		public readonly bool AnticampNuke = true;

		//Smod command blockers
		public readonly bool GrenadeFlash = true;
		public readonly string[] GrenadeFlashRanks = { "owner" };

		//Item and Role Blockers
		public readonly bool ItemBlocker = true;
		public readonly string[] ItemBlockerItems = { "25" };
		public readonly string[] ItemBlockerRanks = { "owner" };

		public readonly bool RoleBlocker = true;
		public readonly string[] RoleBlockerRoles = { "14" };
		public readonly string[] RoleBlockerRanks = { "owner" };

		//Ban length blockers
		public readonly bool SevenDays = true;
		public readonly string[] SevenDaysRanks = { };

		public readonly bool FourteenDays = true;
		public readonly string[] FourteenDaysRanks = { "moderator" };

		public readonly bool ThirtyDays = true;
		public readonly string[] ThirtyDaysRanks = { "admin" };

		//Facility management
		public readonly string[] FacilityOverride = { "owner" };

		public readonly bool DisableMuting = true;

		public readonly bool Mute = false;
		public readonly string[] MuteRanks = { };

		public readonly bool Imute = false;
		public readonly string[] ImuteRanks = { "admin", "owner" };

		public readonly bool Cassie = false;
		public readonly string[] CassieRanks = { "owner" };

		public readonly bool Broadcast = false;
		public readonly string[] BroadcastRanks = { "admin", "owner" };

		public readonly bool Doors = false;
		public readonly string[] OpenCloseRanks = { "moderator", "admin", "owner" };
		public readonly string[] LockUnlockRanks = { "admin", "owner" };
		public readonly string[] DestroyRanks = { "owner" };

		public readonly bool Bypass = true;
		public readonly string[] BypassRanks = { "owner", "admin" };



		public readonly string[] RslotRanks = { "owner", "admin" };
		public readonly string[] OverrideRanks = { "owner" };


		public override void OnDisable()
		{
			this.Info(Details.name + " version " + Details.version + " was disabled");
		}

		public override void OnEnable()
		{
			this.Info(Details.name + " version " + Details.version + " was enabled");
		}

		public override void Register()
		{
			//Main event handler
			this.AddEventHandlers(new EventHandler(this));

			//Reserved slot command
			this.AddCommand("rslot", new RedSlotCommandHandler(this));

			//Misc.
			this.AddConfig(new ConfigSetting("ba_resslot_ranks", new[] { "admin", "owner" }, true, "List of ranks that should be able to use the RSLOT command"));
		}
	}

	internal class RedSlotCommandHandler : ICommandHandler
	{
		private Betteradmin betteradmin;

		public RedSlotCommandHandler(Betteradmin betteradmin)
		{
			this.betteradmin = betteradmin;
		}

		public string GetCommandDescription()
		{
			return "Grants a reserved slot to the user";
		}

		public string GetUsage()
		{
			return "rslot";
		}

		public string[] OnCall(ICommandSender sender, string[] args)
		{
			bool valid = sender is Server;
			Player admin = null;
			if (!valid)
			{
				admin = sender as Player;
				if (admin != null)
				{
					valid = this.betteradmin.RslotRanks.Contains(admin.GetRankName());
				}
			}

			if (valid)
			{
				foreach (Player target in this.betteradmin.Server.GetPlayers(args[0]))
				{
					if (target != null)
					{
						new ReservedSlot(target.IpAddress, target.SteamId, " " + target.Name + " granted by " + admin.Name).AppendToFile();
						return new[] { target.Name + " Has been given a reserved slot on this server!" };
					}
				}
			}
			else if (!valid)
			{
				return new[] { "You are not whitelisted to use this command" };
			}

			return new[] { GetUsage() };
		}
	}
}
