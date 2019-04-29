# BetterAdmin
Plugin for SCP: Secret Laboratory that allows for blocking item and role spawns
Also includes a anti-camp feature, that will lock open doors if specific requirements are met, to prevent players from camping inside

If you have any issues with the plugin, please post them [here](https://github.com/PhoenProject/BetterAdmin/issues)
If you need any support, feel free to come and ask on my [discord](https://discord.gg/asVrGDm)

| Config Option | Value Type | Default Value | Description |
| :---: | :---: | :---: | ------------- |
| ba_item  |  Boolean | true | Enables the BetterAdmin item blocker  |
| ba_items_blacklist  |  list | 25 | List of items that should not be able to be spawned  |
| ba_items_blacklist_ranks  |  list | owner | List of ranks that bypass the item blacklist  |
| ba_role  |  Boolean | true | Enables the BetterAdmin role blocker  |
| ba_roles_blacklist  |  list | 14 | List of roles that should not be able to be spawned  |
| ba_roles_blacklist_ranks  |  list | owner | List of ranks that bypass the role blacklist  |
| ba_anticamp_106  |  Boolean | true | Locks the 106 chamber doors open upon 106 being recontained |
| ba_anticamp_079  |  Boolean | true | Locks the 079 chamber doors open upon activating all 5 generators |
| ba_anticamp_nuke  |  Boolean | true | Locks the nuke surface doors open upon the nuke being canceled |
| ba_staff_resslot  |  Boolean | false | Automatically creates a reserved slot for studio staff who join the server |
| ba_gmod_resslot  |  Boolean | true | Automatically creates a reserved slot for global moderators who join the server (It is highly advised you keep this enabled so that we don't have trouble with full servers) |
| ba_latejoin |  Boolean | true | Enables the late join feature of BetterAdmin  |
| ba_latejoin_duration |  Int | 30 | Time before people will stop being spawned in after joining late  |
| ba_7d_ranks  |  List |  | List of ranks that should not be allowed to ban for more than 7 days  |
| ba_14d_ranks  |  List | moderator | List of ranks that should not be allowed to ban for more than 14 days  |
| ba_30d_ranks  |  List | admin | List of ranks that should not be allowed to ban for more than 30 days  |
| ba_resslot_ranks  |  list | admin, owner | List of ranks that should be able to use the RSLOT command  |

| Command | Value | Description |
| :---: | :---: | ------------- |
| rslot  |  <PlayerID> | Grants a reserved slot to the player |

