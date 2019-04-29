using Smod2;
using Smod2.API;
using Smod2.Attributes;
using Smod2.Commands;
using Smod2.Config;
using Smod2.EventHandlers;
using Smod2.Events;

namespace BetterAdmin
{
    [PluginDetails(
        author = "Phoenix",
        name = "BetterAdmin",
        description = "Plugin with a bunch of config options to help server administration",
        id = "phoenix.betteradmin",
        version = "1.1.1",
        SmodMajor = 3,
        SmodMinor = 3,
        SmodRevision = 0
        )]
    class Betteradmin : Plugin
    {
		//Item and role blocker
		public int[] Itemblacklist { get; private set; }
        public int[] Roleblacklist { get; private set; }
        public string[] Itemblacklistranks { get; private set; }
        public string[] Roleblacklistranks { get; private set; }
		public bool Idisable { get; private set; }
		public bool Rdisable { get; private set; }

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
            this.AddConfig(new ConfigSetting("ba_roles_blacklist", new[] { 14 },  true, "List of roles that should not be able to be spawned"));
            this.AddConfig(new ConfigSetting("ba_items_blacklist_ranks", new[] { "owner" }, true, "List of ranks that bypass the item blacklist"));
            this.AddConfig(new ConfigSetting("ba_roles_blacklist_ranks", new[] { "owner" }, true, "List of ranks that bypass the role blacklist"));
            this.AddConfig(new ConfigSetting("ba_item_disable", false, true, "Disables the BetterAdmin item blocker"));
            this.AddConfig(new ConfigSetting("ba_role_disable", false, true, "Disables the BetterAdmin role blocker"));

			//Anti-camp options
            this.AddConfig(new ConfigSetting("ba_anticamp_106", true, true, "Locks the 106 chamber doors open upon 106 being recontained"));
            this.AddConfig(new ConfigSetting("ba_anticamp_079", true, true, "Locks the 079 chamber doors open upon activating all 5 generators"));
            this.AddConfig(new ConfigSetting("ba_anticamp_nuke", true, true, "Locks the nuke surface doors open upon the nuke being canceled"));

			//Automatic reserved slot options
            this.AddConfig(new ConfigSetting("ba_staff_resslot", false, true, "Automatically creates a reserved slot for studio staff who join the server"));
            this.AddConfig(new ConfigSetting("ba_gmod_resslot", true, true, "Automatically creates a reserved slot for global moderators who join the server"));

			//Grenade and flash command blocker
			this.AddConfig(new ConfigSetting("ba_grenade_flash_disable", false, true, "Disables the blocker of the grenade and flash commands"));
			this.AddConfig(new ConfigSetting("ba_grenade_flash_ranks", new[] { "owner" }, true, "List of ranks that can use the grenade and flash commands"));

			//Latejoin options
			this.AddConfig(new ConfigSetting("ba_latejoin", true, true, "Enables the late join feature of BetterAdmin"));
			this.AddConfig(new ConfigSetting("ba_latejoin_duration", 30, true, "List of ranks that can use the grenade and flash commands"));

			//ban length blocker
			this.AddConfig(new ConfigSetting("ba_sevenday_ranks", new[] { "moderator" }, true, "List of ranks that should not be allowed to ban for more than 7 days"));
		}

        public void RefreshConfig()
        {
			//Item and role blocker config
			Itemblacklist = GetConfigIntList("ba_items_blacklist");
            Roleblacklist = GetConfigIntList("ba_roles_blacklist");
            Itemblacklistranks = GetConfigList("ba_items_blacklist_ranks");
            Roleblacklistranks = GetConfigList("ba_roles_blacklist_ranks");
			Idisable = GetConfigBool("ba_item_disable");
			Rdisable = GetConfigBool("ba_role_disable");

			//Anti-camp
			Anticamp106 = GetConfigBool("ba_anticamp_106");
            Anticamp079 = GetConfigBool("ba_anticamp_079");
            Anticampnuke = GetConfigBool("ba_anticamp_nuke");

			//Automatic reserved slot
            Staffresslot = GetConfigBool("ba_staff_resslot");
            Gmodresslot = GetConfigBool("ba_gmod_resslot");

			//Grenade and flash command
			Grenadeflash = GetConfigBool("ba_grenade_flash_disable");
			Grenadeflashranks = GetConfigList("ba_grenade_flash_ranks");

			//Latejoin
			Latejoin = GetConfigBool("ba_latejoin");
			Latejoinduration = GetConfigInt("ba_latejoin_duration");

			//ban length blocker
			Sevendays = GetConfigList("ba_sevenday_ranks");
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
			if (args.Length < 2) return new[] { "You need to state a PlayerID" };
			else
			{
				foreach (Player player in betteradmin.Server.GetPlayers(args[1]))
				{
					if(player != null)
					{ 
						betteradmin.Info(sender.ToString());
						new ReservedSlot(player.IpAddress, player.SteamId, " Slot given by " + sender.ToString());
						return new[] { "Reserved slot granted to " + player.Name.ToString() };
					}
					else { return new[] { "Sorry, but soemthing went wrong" }; }
				}
				return new[] { "Hello!" };
			}			
		}
	}
}
