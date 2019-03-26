using Smod2;
using Smod2.API;
using Smod2.Attributes;
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
        version = "1.1.0.0",
        SmodMajor = 3,
        SmodMinor = 3,
        SmodRevision = 0
        )]
    class Betteradmin : Plugin
    {
        public int[] Itemblacklist { get; private set; }
        public int[] Roleblacklist { get; private set; }
        public string[] Itemblacklistranks { get; private set; }
        public string[] Roleblacklistranks { get; private set; }

        public bool Anticamp106 { get; private set; }
        public bool Anticamp079 { get; private set; }
        public bool Anticampnuke { get; private set; }

        //public bool ConnectSpam { get; private set; }
        //public int ConnectSpamLength { get; private set; }

        public bool Idisable { get; private set; }
        public bool Rdisable { get; private set; }

        public bool Staffresslot { get; private set; }
        public bool Gmodresslot { get; private set; }

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
            this.AddEventHandlers(new EventHandler(this));

            this.AddConfig(new ConfigSetting("ba_items_blacklist", new[] { 25 }, SettingType.NUMERIC_LIST, true, "List of items that should not be able to be spawned"));
            this.AddConfig(new ConfigSetting("ba_roles_blacklist", new[] { 14 }, SettingType.NUMERIC_LIST, true, "List of roles that should not be able to be spawned"));
            this.AddConfig(new ConfigSetting("ba_items_blacklist_ranks", new[] { "owner" }, SettingType.LIST, true, "List of ranks that bypass the item blacklist"));
            this.AddConfig(new ConfigSetting("ba_roles_blacklist_ranks", new[] { "owner" }, SettingType.LIST, true, "List of ranks that bypass the role blacklist"));

            this.AddConfig(new ConfigSetting("ba_anticamp_106", true, SettingType.BOOL, true, "Locks the 106 chamber doors open upon 106 being recontained"));
            this.AddConfig(new ConfigSetting("ba_anticamp_079", true, SettingType.BOOL, true, "Locks the 079 chamber doors open upon activating all 5 generators"));
            this.AddConfig(new ConfigSetting("ba_anticamp_nuke", true, SettingType.BOOL, true, "Locks the nuke surface doors open upon the nuke being canceled"));

            //this.AddConfig(new ConfigSetting("ba_connectspam_ban", true, SettingType.BOOL, true, "Enable the connection spam ban setting, which bans anyone who tries to connect to a full server"));
            //this.AddConfig(new ConfigSetting("ba_connectspam_ban_length", 1, SettingType.NUMERIC, true, "Length of the ban given upon connecting to a full server"));

            this.AddConfig(new ConfigSetting("ba_staff_resslot", false, SettingType.BOOL, true, "Automatically creates a reserved slot for studio staff who join the server"));
            this.AddConfig(new ConfigSetting("ba_gmod_resslot", true, SettingType.BOOL, true, "Automatically creates a reserved slot for global moderators who join the server"));

            this.AddConfig(new ConfigSetting("ba_item_disable", false, SettingType.BOOL, true, "Disables the BetterAdmin item blocker"));
            this.AddConfig(new ConfigSetting("ba_role_disable", false, SettingType.BOOL, true, "Disables the BetterAdmin role blocker"));
        }

        public void RefreshConfig()
        {
            Itemblacklist = GetConfigIntList("ba_items_blacklist");
            Roleblacklist = GetConfigIntList("ba_roles_blacklist");
            Itemblacklistranks = GetConfigList("ba_items_blacklist_ranks");
            Roleblacklistranks = GetConfigList("ba_roles_blacklist_ranks");

            Anticamp106 = GetConfigBool("ba_anticamp_106");
            Anticamp079 = GetConfigBool("ba_anticamp_079");
            Anticampnuke = GetConfigBool("ba_anticamp_nuke");

            //ConnectSpam = GetConfigBool("ba_connectspam_ban");
            //ConnectSpamLength = GetConfigInt("ba_connectspam_ban_length");

            Idisable = GetConfigBool("ba_item_disable");
            Rdisable = GetConfigBool("ba_role_disable");

            Staffresslot = GetConfigBool("ba_staff_resslot");
            Gmodresslot = GetConfigBool("ba_gmod_resslot");
        }
	}
}
