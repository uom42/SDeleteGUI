# SDeleteGUI
This is a simple GUI for a cool SDelete (Secure file deleting) tool made by Mark Russinovich (Sysinternals).
You can read more and download this tool from: https://docs.microsoft.com/en-us/sysinternals/downloads/sdelete

# After start:
You can specify the Disk Device (this disk must not contains any partitions!):
![](./Media/ScrShots/01_Main_SelectPhyDisk.png)

 Or Logical Disk (to clean free space on it):

 ![](./Media/ScrShots/02_Main_SelectLogDisk.png)
 
  Or Folder or file(s).
  You must have the appropriate permissions to these files and folders in order to modify them. This program itself does not change the access level, but uses the current one.

 # Cleaning process:
 After selecting object to clean and clean method, click on Start button.
 This will start Sysinternals SDelete tool with selected parameners to process cleaning.

![](./Media/ScrShots/03_Main_LogDisk_Inprocess.png)

 In the Output section you will see the output data of the running SDelete tool.

# When the cleaning process is completed:
 And when the cleaning process is completed, you will see something like this:

![](./Media/ScrShots/04_Main_LogDisk_Finished.png)

You can learn more about exactly how cleaning is done on the page of this amazing SDelete utility: https://docs.microsoft.com/en-us/sysinternals/downloads/sdelete

It's just simple. Enjoy!
