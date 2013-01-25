TFSBuildMonitor
===============

A fork of the original TFS Build Monitor from http://buildmonitor.codeplex.com/

Changes made:

* Moved monitor to a windows service host
* Enabled F5 execution 
* Event logging to default application log
* Install script added in root

Install Guide 

* Run PowerShell as Administrator
* Run the Install-BuildMonitor.ps1 script providing repository root path and install path
* Enter what you're asked for (credentials for the service)
* Update your config file to the settings you want (intend to fold this into the setup script)

That should be it. Watch for the light to come on, and check your event log for errors - they'll end up in the application log.

This was hacked together in around 20 minutes. Improvement welcome, the fork button is up next to the pull request :)
