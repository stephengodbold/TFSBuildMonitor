TFSBuildMonitor
===============

A fork of the original TFS Build Monitor from http://buildmonitor.codeplex.com/

Changes made:

* Moved monitor to a windows service host
* Enabled F5 execution (add /i debug parameter)
* Event logging to default application log
* Install script added in root
* Added project filtering to configuration

Install Guide 

* Run PowerShell as Administrator
* Run the Install-BuildMonitor.ps1 script providing repository root path and install path
* Enter what you're asked for (credentials for the service)
* Update your config file to the settings you want (intend to fold this into the setup script)

That should be it. 

Troubleshooting

* Ensure logging is enabled in your configuration
* Open Event Viewer
* Check the Application Log for information and error messages for TFSBuildMonitor

If you are having problems with the Delcom assembly, try the 32-bit image. I've had issues with the 64-bit not working previously.
