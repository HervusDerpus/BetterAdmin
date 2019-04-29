using Smod2;
using Smod2.API;
using Smod2.Attributes;
using Smod2.Commands;
using Smod2.Config;
using Smod2.EventHandlers;
using Smod2.Events;
using System.Linq;

namespace BetterAdmin
{
	[PluginDetails(
		author = "Phoenix",
		name = "BetterAdmin",
		description = "Plugin to help with server administration",
		id = "phoenix.betteradmin",
		version = "1.2.0",
		SmodMajor = 3,
		SmodMinor = 4,
		SmodRevision = 0
		)]
	class Betteradmin : Plugin
	{
		//Item and role blocker
		public int[] Itemblacklist { get; private set; }
		public int[] Roleblacklist { get; private set; }
		public string[] Itemblacklistranks { get; private set; }
		public string[] Roleblacklistranks { get; private set; }
		public bool IBlock { get; private set; }
		public bool RBlock { get; private set; }

		//Anti-camp
		public bool Anticamp106 { get; private set; }
		public bool Anticamp079 { get; private set; }
		public bool Anticampnuke { get; private set; }

		//Automatic reserved slot
		public bool Staffresslot { get; private set; }
		public bool Gmodresslot { get; private set; }

		//Grenade and flash command
		public bool Grenadeflash { get; private set; }
		public string[] Grenadeflashranks { get; private set; }

		//Latejoin options
		public bool Latejoin { get; private set; }
		public int Latejoinduration { get; private set; }

		//ban length blocker
		public string[] Sevendays { get; private set; }
		public string[] Fourteendays { get; private set; }
		public string[] Thirtydays { get; private set; }

		//Misc.
		public string[] RSlotRanks { get; private set; }

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

			//Item and role blocker config options
			this.AddConfig(new ConfigSetting("ba_items_blacklist", new[] { 25 }, true, "List of items that should not be able to be spawned"));
			this.AddConfig(new ConfigSetting("ba_roles_blacklist", new[] { 14 }, true, "List of roles that should not be able to be spawned"));
			this.AddConfig(new ConfigSetting("ba_items_blacklist_ranks", new[] { "owner" }, true, "List of ranks that bypass the item blacklist"));
			this.AddConfig(new ConfigSetting("ba_roles_blacklist_ranks", new[] { "owner" }, true, "List of ranks that bypass the role blacklist"));
			this.AddConfig(new ConfigSetting("ba_item", true, true, "Enables the BetterAdmin item blocker"));
			this.AddConfig(new ConfigSetting("ba_role", true, true, "Enables the BetterAdmin role blocker"));

			//Anti-camp options
			this.AddConfig(new ConfigSetting("ba_anticamp_106", true, true, "Locks the 106 chamber doors open upon 106 being recontained"));
			this.AddConfig(new ConfigSetting("ba_anticamp_079", true, true, "Locks the 079 chamber doors open upon activating all 5 generators"));
			this.AddConfig(new ConfigSetting("ba_anticamp_nuke", true, true, "Locks the nuke surface doors open upon the nuke being canceled"));

			//Automatic reserved slot options
			this.AddConfig(new ConfigSetting("ba_staff_resslot", false, true, "Automatically creates a reserved slot for studio staff who join the server"));
			this.AddConfig(new ConfigSetting("ba_gmod_resslot", true, true, "Automatically creates a reserved slot for global moderators who join the server"));

			//Grenade and flash command blocker
			this.AddConfig(new ConfigSetting("ba_grenade_flash", true, true, "Enables the blocker of the grenade and flash commands"));
			this.AddConfig(new ConfigSetting("ba_grenade_flash_ranks", new[] { "owner" }, true, "List of ranks that can use the grenade and flash commands"));

			//Latejoin options
			this.AddConfig(new ConfigSetting("ba_latejoin", true, true, "Enables the late join feature of BetterAdmin"));
			this.AddConfig(new ConfigSetting("ba_latejoin_duration", 30, true, "List of ranks that can use the grenade and flash commands"));

			//Ban length blocker
			this.AddConfig(new ConfigSetting("ba_7d_ranks", new[] { "" }, true, "List of ranks that should not be allowed to ban for more than 7 days"));
			this.AddConfig(new ConfigSetting("ba_14d_ranks", new[] { "moderator" }, true, "List of ranks that should not be allowed to ban for more than 14 days"));
			this.AddConfig(new ConfigSetting("ba_30d_ranks", new[] { "admin" }, true, "List of ranks that should not be allowed to ban for more than 30 days"));

			//Misc.
			this.AddConfig(new ConfigSetting("ba_resslot_ranks", new[] { "admin", "owner" }, true, "List of ranks that should be able to use the RSLOT command"));
		}
		public void RefreshConfig()
		{
			//Item and role blocker config
			Itemblacklist = GetConfigIntList("ba_items_blacklist");
			Roleblacklist = GetConfigIntList("ba_roles_blacklist");
			Itemblacklistranks = GetConfigList("ba_items_blacklist_ranks");
			Roleblacklistranks = GetConfigList("ba_roles_blacklist_ranks");
			IBlock = GetConfigBool("ba_item");
			RBlock = GetConfigBool("ba_role");

			//Anti-camp
			Anticamp106 = GetConfigBool("ba_anticamp_106");
			Anticamp079 = GetConfigBool("ba_anticamp_079");
			Anticampnuke = GetConfigBool("ba_anticamp_nuke");

			//Automatic reserved slot
			Staffresslot = GetConfigBool("ba_staff_resslot");
			Gmodresslot = GetConfigBool("ba_gmod_resslot");

			//Grenade and flash command
			Grenadeflash = GetConfigBool("ba_grenade_flash");
			Grenadeflashranks = GetConfigList("ba_grenade_flash_ranks");

			//Latejoin
			Latejoin = GetConfigBool("ba_latejoin");
			Latejoinduration = GetConfigInt("ba_latejoin_duration");

			//ban length blocker
			Sevendays = GetConfigList("ba_7d_ranks");
			Fourteendays = GetConfigList("ba_14d_ranks");
			Thirtydays = GetConfigList("ba_30d_ranks");

			//Misc.
			RSlotRanks = GetConfigList("ba_resslot_ranks");
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
					valid = this.betteradmin.RSlotRanks.Contains(admin.GetRankName());
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
