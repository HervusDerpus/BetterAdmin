# BetterAdmin
Plugin for SCP: Secret Laboratory that allows for blocking item and role spawns
Also includes a anti-camp feature, that will lock open doors if specific requirements are met, to prevent players from camping inside

If you have any issues with the plugin, please post them [here](https://github.com/HervusDerpus/BetterAdmin/issues)
If you need any support, feel free to come and ask on my [discord](https://discord.gg/asVrGDm)

| Config Option | Value Type | Default Value | Description |
| :---: | :---: | :---: | ------------- |
| ba_items_blacklist  |  list | 25 | List of items that should not be able to be spawned  |
| ba_roles_blacklist  |  list | 14 | List of roles that should not be able to be spawned  |
| ba_items_blacklist_ranks  |  list | owner | List of ranks that bypass the item blacklist  |
| ba_roles_blacklist_ranks  |  list | owner | List of ranks that bypass the role blacklist  |
| ba_item_disable  |  Boolean | false | Disables the BetterAdmin item blocker  |
| ba_role_disable  |  Boolean | false | Disables the BetterAdmin role blocker  |
| ba_anticamp_106  |  Boolean | true | Locks the 106 chamber doors open upon 106 being recontained |
| ba_anticamp_079  |  Boolean | true | Locks the 079 chamber doors open upon activating all 5 generators |
| ba_anticamp_nuke  |  Boolean | true | Locks the nuke surface doors open upon the nuke being canceled |
| ba_staff_resslot  |  Boolean | false | Automatically creates a reserved slot for studio staff who join the server |
| ba_gmod_resslot  |  Boolean | true | Automatically creates a reserved slot for global moderators who join the server (It is highly advised you keep this enabled so that we don't have trouble with full servers) |