using System;
using System.Collections.Generic;
using System.Text;
using MLS.ZWave.Service.Rules;
using MLS.ZWave.BusinessObjects;

/// <summary>
/// ALWAYS MAKE COPIES OF SCRIPTS YOU INTEND TO CUSTOMIZE OR YOUR CHANGES 
/// COULD BE LOST.
/// </summary>
public class TurnOnOffRandomSceneLight : ScriptBase, ScriptInterface {
    public void runScript() {
        try {

            // Get a random light (this will be either a dimmer or a binary switch)
            var light = getRandomSceneLight();

            // 1 in 5 chance of changing the light
            Random r = new Random(DateTime.Now.Millisecond);
            var rInt = r.Next(0, 4);

            if (rInt <= 1) {
                // Check the light level
                if (light.level > 0) {
                    // Light is on, so turn it off
                    dm.setPower(light.deviceId, false);
                } else {
                    // Light is off, so turn it on
                    dm.setPower(light.deviceId, true);
                }
            }


        } catch (Exception ex) {
            // Log the exception here            
            writeFileLog("TurnOnOffRandomSceneLight Error", ex);
        }
    }
}

