InControl-Scripts
=================

This repository holds scripts and samples of scripts that can be executed inside InControl Home Automation

### Script Creation

To create a script, you simply need a text editor. Load up your favorite one (Notepad++ works great). 
Create a new file in your Scripts folder -- normally located at 
`C:\Program Files (X86)\Moonlit Software, LLC\InControl HA\Scripts`. Name the file with a .CS extension -- for example, "MyScript.cs"

All scripts should start with a basic framework. Here's a sample framework for you to start with. Copy and paste this into your new script:

```
using MLS.ZWave.Service.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MLS.HA.DeviceController.Common.Device.ZWave;

public class MyScript : ScriptBase, ScriptInterface {

   public void runScript() {
       // This is where the magic happens
       // Your code should go here
   }
   
}
```

### Commands

#### Retrieving a Device

Devices can be retrieved if you know one of three id's related to the device:

- `Short Id`: The Short Id can be found by double-clicking a device inside InControl. This is the best Id to use for retrieving a device.
- `Object Provider Id`: This is an Id assigned by the controller; it may not always be guaranteed to be unique if you use multiple controllers.
- `Device Id`: This is a unique UUID assigned to each device by InControl. This id is not exposed in the InControl UI anywhere, but can be queried by a script if desired.

##### Using a Short Id

Assuming your short id is 10, you could retrieve the device using this command:

```
   var device = getNodeByShortId(10);
```

##### Using a Object Provider Id:

```
   var device = getNode(18);
```

##### Using a Device Id

```
   var gUid = Guid.Parse("F9168C5E-CEB2-4faa-B6BF-329BF39FA1E4");
   var device = getNode(gUid);
```






