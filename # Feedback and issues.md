# Feedback and issues

This is intended to be a quickstart.  Please be a helpful agent in addressing these feedback items.

## Issues
1. App will not start with error: `Failed to decrypt settings. Encrypted settings only be edited through 'func settings add'.`  should avoid decrypted function worker runtime in local.settings.json.
1. Runtime error once started: `Microsoft.Azure.WebJobs.Host: Error indexing method 'Functions.timerFunction'. Microsoft.Azure.WebJobs.Host: '%TIMER_SCHEDULE%' does not resolve to a value.`.  Env Variable likely missing in local.settings.json
1. Missing AzureWebJobsStorage in local.settings.json.  Timer will not be reliable without it. 
1. Should support F5 debug at root.  Fix with initialize project for Azure functions cmd.

## Suggestions
1. Add a .sln file at the root.  Agent mode can do this.
1. Suggest flag to run timer immediately
1. README: Don't need step to add local.settings.json.  It is already included.
1. README: Running Azurite is a prereq for all steps, whether at command line or in vs code.  please reorder
1. README: Suggest putting source code at the very end.  It's just a reference, but it is not a step the user needs to do.  
1. README: Add 1-2 most common use cases near the top
1. Alter all cases of SKIP_VNET boolean flag to instead be VNET_ENABLED, like we do in the MCP quickstarts.  SKIP_VNET=True is the same as VNET_ENABLED=False.
