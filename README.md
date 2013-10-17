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

```
   var device = getNodeByShortId(10);
```

- `Object Provider Id`: This is an Id assigned by the controller; it may not always be guaranteed to be unique if you use multiple controllers.

```
   var device = getNode(18);
```

- `Device Id`: This is a unique UUID assigned to each device by InControl. This id is not exposed in the InControl UI anywhere, but can be queried by a script if desired.

```
   var gUid = Guid.Parse("F9168C5E-CEB2-4faa-B6BF-329BF39FA1E4");
   var device = getNode(gUid);
```

#### Standard Device Commands

##### Set Device Level

To set the level of a device to 75, you would first need to retrieve the device then use it's device Id to issue the command. 

```
   var device = getNodeByShortId(10);
   dm.setLevel(device.deviceId, 75);
```

##### Set Device Power

To power a device on or off, you would first need to retrieve the device then use it's device id to issue the command.

```
   var device = getNodeByShortId(10);
   dm.setPower(device.deviceId, true);
```

#### Thermostat Control

##### Fan Mode

```
   var device = getNodeByShortId(10);
   
   // Turns the fan to auto
   dm.setZWaveThermostatFanMode(device.deviceId, HA.DeviceController.Common.ThermoFanMode.Auto);
   
   // Turns the fan to on
   dm.setZWaveThermostatFanMode(device.deviceId, HA.DeviceController.Common.ThermoFanMode.On);
```

##### System Mode

```
   var device = getNodeByShortId(10);
   dm.setZWaveThermostatSystemState(device.deviceId, HA.DeviceController.Common.ThermoSystemMode.Heat);
```

These are the valid modes -- note that some thermostats may not support all of these modes:

        Off
        Heat
        Cool 
        Auto
        AuxEmergency 
        Resume
        Fan
        Furnace
        DryAir
        MoistAir
        AutoChange
        EnergySaveHeat
        EnergySaveCool
        Away

##### Setpoints

To change a setpoint, you need to know the name of the manufacture created setpoints. Generally these are always the same and you 
usually use these values:

- To change heating, use a name of "Heating1"
- To change cooling, use a name of "Cooling1"

```
   var device = getNodeByShortId(10);
   dm.setZWaveThermostatSetPoint(device.deviceId, "Heating1", 72);
```

#### Other Commands

##### Showing Messages

This command will show a message in the UI if it's running. If the UI is not running, the command is ignored.

```
   showMessage("Your message here", "The title of your message");
```

#### Writing to the Logfile.txt

The logfile.txt can be found in your InControl's installation folder.

```
   writeFileLog("Message to append to the logfile");
```
