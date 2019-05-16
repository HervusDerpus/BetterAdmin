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
		//Automatic-stuffs
		[ConfigOption] public readonly bool StaffResSlot = false;
		[ConfigOption] public readonly bool GmodResSlot = true;
		[ConfigOption] public readonly bool Anticamp106 = true;
		[ConfigOption] public readonly bool Anticamp079 = true;
		[ConfigOption] public readonly bool AnticampNuke = true;

		//Smod command blockers
		[ConfigOption] public readonly bool GrenadeFlash = true;
		[ConfigOption] public readonly string[] GrenadeFlashRanks = { "owner" };

		//Item and Role Blockers
		[ConfigOption] public readonly bool ItemBlocker = true;
		[ConfigOption] public readonly string[] ItemBlockerItems = { "25" };
		[ConfigOption] public readonly string[] ItemBlockerRanks = { "owner" };

		[ConfigOption] public readonly bool RoleBlocker = true;
		[ConfigOption] public readonly string[] RoleBlockerRoles = { "14" };
		[ConfigOption] public readonly string[] RoleBlockerRanks = { "owner" };

		//Ban length blockers
		[ConfigOption] public readonly bool SevenDays = true;
		[ConfigOption] public readonly string[] SevenDaysRanks = { };

		[ConfigOption] public readonly bool FourteenDays = true;
		[ConfigOption] public readonly string[] FourteenDaysRanks = { "moderator" };

		[ConfigOption] public readonly bool ThirtyDays = true;
		[ConfigOption] public readonly string[] ThirtyDaysRanks = { "admin" };

		//Facility management
		[ConfigOption] public readonly string[] FacilityOverride = { "owner" };

		[ConfigOption] public readonly bool DisableMuting = true;
		[ConfigOption] public readonly bool Mute = false;
		[ConfigOption] public readonly string[] MuteRanks = { };

		[ConfigOption] public readonly bool Imute = false;
		[ConfigOption] public readonly string[] ImuteRanks = { "admin", "owner" };

		[ConfigOption] public readonly bool Cassie = false;
		[ConfigOption] public readonly string[] CassieRanks = { "owner" };

		[ConfigOption] public readonly bool broadcast = false;
		[ConfigOption] public readonly string[] BroadcastRanks = { "admin", "owner" };

		[ConfigOption] public readonly bool Doors = false;
		[ConfigOption] public readonly string[] OpenCloseRanks = { "moderator", "admin", "owner" };
		[ConfigOption] public readonly string[] LockUnlockRanks = { "admin", "owner" };
		[ConfigOption] public readonly string[] DestroyRanks = { "owner" };

		[ConfigOption] public readonly bool Bypass = true;
		[ConfigOption] public readonly string[] BypassRanks = { "owner", "admin" };

		[ConfigOption] public readonly bool Lockdown = true;
		[ConfigOption] public readonly string[] LockdownRanks = { "owner", "admin" };

		[ConfigOption] public readonly string[] RslotRanks = { "owner", "admin" };
		[ConfigOption] public readonly string[] OverrideRanks = { "owner" };

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
