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

## Retrieving a Device
