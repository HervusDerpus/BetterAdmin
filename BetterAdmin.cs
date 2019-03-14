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
        description = "Plugin that improves administation capabilties",
        id = "phoenix.betteradmin",
        version = "1.0",
        SmodMajor = 3,
        SmodMinor = 3,
        SmodRevision = 0
        )]
    class Betteradmin : Plugin
    {
        public int[] itemblacklist { get; private set; }
        public int[] roleblacklist { get; private set; }
        public string[] itemblacklistranks { get; private set; }
        public string[] roleblacklistranks { get; private set; }

        public bool anticamp106 { get; private set; }
        public bool anticamp079 { get; private set; }
        public bool anticampnuke { get; private set; }

        public bool idisable { get; private set; }
        public bool rdisable { get; private set; }

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

            this.AddConfig(new ConfigSetting("ba_item_disable", false, SettingType.BOOL, true, "Disables the BetterAdmin item blocker"));
            this.AddConfig(new ConfigSetting("ba_role_disable", false, SettingType.BOOL, true, "Disables the BetterAdmin role blocker"));
        }

        public void RefreshConfig()
        {
            itemblacklist = GetConfigIntList("ba_items_blacklist");
            roleblacklist = GetConfigIntList("ba_roles_blacklist");
            itemblacklistranks = GetConfigList("ba_items_blacklist_ranks");
            roleblacklistranks = GetConfigList("ba_roles_blacklist_ranks");

            anticamp106 = GetConfigBool("ba_anticamp_106");
            anticamp079 = GetConfigBool("ba_anticamp_079");
            anticampnuke = GetConfigBool("ba_anticamp_nuke");

            idisable = GetConfigBool("ba_item_disable");
            rdisable = GetConfigBool("ba_role_disable");
        }
	}
}
