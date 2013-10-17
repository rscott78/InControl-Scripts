using MLS.ZWave.Service.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MLS.HA.DeviceController.Common.Device.ZWave;

namespace MLS.ZWave.Service.Scripts {
    public class SampleScript : ScriptBase, ScriptInterface {

        StringBuilder sbStatusMessage;

        /// <summary>
        /// This is called by the server when the script is executed.
        /// </summary>
        public void runScript() {

            sbStatusMessage = new StringBuilder();
            
            // *************************************************************
            // Customizations - it's safe to change values in this section
            // *************************************************************
            
            // Set the thermostat short id 
            // You can find the short id of a device by double clicking it
            var thermostatShortId = 14;

            // *************************************************************
            // Be careful about changes beyond this point
            // *************************************************************

            // Show a message to the user about the script starting up
            setStatus("Your script is starting...");
                        
            // Set the fan to auto and show a message if it didn't get set
            setStatus("Setting t-stat fan mode to on...");
            setAndValidateThermostatFanMode(thermostatShortId, HA.DeviceController.Common.ThermoFanMode.On);
            
            // Set the fan to on and show a message if it didn't get set
            setStatus("Setting t-stat fan mode to auto...");
            setAndValidateThermostatFanMode(thermostatShortId, HA.DeviceController.Common.ThermoFanMode.Auto);

            // Switch to Off
            setStatus("Setting t-stat mode to off...");
            setAndValidateSystemMode(thermostatShortId, HA.DeviceController.Common.ThermoSystemMode.Off);

            // Switch to cool
            setStatus("Setting t-stat mode to cool...");
            setAndValidateSystemMode(thermostatShortId, HA.DeviceController.Common.ThermoSystemMode.Cool);

            // Switch to heat
            setStatus("Setting t-stat mode to heat...");
            setAndValidateSystemMode(thermostatShortId, HA.DeviceController.Common.ThermoSystemMode.Heat);

            // Set the Heating1 setpoint to a value of 72
            setStatus("Setting t-stat Heating1 to value of 72...");
            setAndValidateThermostatSetpointValue(thermostatShortId, "Heating1", 72);

            // Set the Cooling1 setpoint to a value of 80
            setStatus("Setting t-stat Cooling1 to value of 85...");
            setAndValidateThermostatSetpointValue(thermostatShortId, "Cooling1", 85);

            setStatus("Finished Tests!");
        }

        /// <summary>
        /// Sets the system mode.
        /// </summary>
        /// <param name="thermostatShortId"></param>
        /// <param name="state"></param>
        private void setAndValidateSystemMode(int thermostatShortId, HA.DeviceController.Common.ThermoSystemMode state) {
            // Get the device associated with the thermostat
            var thermostatDevice = getNodeByShortId(thermostatShortId) as Thermostat;
            if (thermostatDevice != null) {
                dm.setZWaveThermostatSystemState((byte)thermostatDevice.providerDeviceId, state);

                // Re-poll the device to get the new values
                dm.pollDevice(thermostatDevice.deviceId);

                // This is a delay to wait for the thermostat to respond back to the poll attempt
                System.Threading.Thread.Sleep(4000);

                // Grab a fresh copy of the device
                thermostatDevice = getNodeByShortId(thermostatShortId) as Thermostat;

                if (thermostatDevice.thermostatSystemState != state) {
                    setStatus("...failed!");
                } else {
                    setStatus("...success!");
                }
            }
        }

        /// <summary>
        /// Sets a given thermostat setpoint to a value and verifies that it stuck.
        /// </summary>
        /// <param name="thermostatShortId"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        private void setAndValidateThermostatSetpointValue(int thermostatShortId, string setpointName, int targetValue) {
            // Get the device associated with the thermostat
            var thermostatDevice = getNodeByShortId(thermostatShortId) as Thermostat;

            if (thermostatDevice != null) {                
                // Set the setpoint
                dm.setZWaveThermostatSetPoint((byte)thermostatDevice.providerDeviceId, setpointName, targetValue);

                // Re-poll the device to get the new values
                dm.pollDevice(thermostatDevice.deviceId);

                // This is a delay to wait for the thermostat to respond back to the poll attempt
                System.Threading.Thread.Sleep(4000);

                // Grab a fresh copy of the device
                thermostatDevice = getNodeByShortId(thermostatShortId) as Thermostat;

                // Check the devie values to see if they stuck
                // Retrieve the setpoint
                var sp = thermostatDevice.thermostatSetPoints.Where(s=>s.pointName.ToLower() == setpointName.ToLower()).FirstOrDefault();

                if (sp != null) {
                    // The setpoint was found, now check the value to see that it matches
                    if (sp.temperature == targetValue) {
                        setStatus("... success!");
                    } else {
                        setStatus("... failed!");
                    }

                } else {
                    // The setpoint wasn't found
                    setStatus(setpointName + " does not appear to be a valid setpoint for this thermostat.");
                }
            } else {
                // Something is wrong - perhaps the shortId is wrong?
                // Notify the user that no thermostat was found
                setStatus("No thermostat with a short Id of " + thermostatShortId + " was found.");
            }
        }

        /// <summary>
        /// Sets the thermostat fan mode then checks to see if it was set properly.
        /// </summary>
        /// <param name="newMode"></param>
        private void setAndValidateThermostatFanMode(int thermostatShortId, MLS.HA.DeviceController.Common.ThermoFanMode newMode) {
            // Get the device associated with the thermostat
            var thermostatDevice = getNodeByShortId(thermostatShortId) as MLS.HA.DeviceController.Common.Device.ZWave.Thermostat;

            if (thermostatDevice != null) {
                // Set the thermostat fan mode to auto            
                dm.setZWaveThermostatFanMode(thermostatDevice.deviceId, newMode);

                // Force poll the thermostat to see if the value stuck
                dm.pollDevice(thermostatDevice.deviceId);

                // This is a delay to wait for the thermostat to respond back to the poll attempt
                System.Threading.Thread.Sleep(6000);

                // Grab a fresh copy of the device
                thermostatDevice = getNodeByShortId(thermostatShortId) as Thermostat;

                // Check the new thermostat fan mode
                if (thermostatDevice.thermostatFanMode == newMode) {
                    setStatus("... Success!");
                } else {
                    setStatus("... Failed!");
                }

            } else {
                // Something is wrong - perhaps the shortId is wrong?
                // Notify the user that no thermostat was found
                setStatus("No thermostat with a short Id of " + thermostatShortId + " was found.");
            }

            
        }

        private void setStatus(string message) {
            sbStatusMessage.AppendLine(message);
            showMessage(sbStatusMessage.ToString(), "Sample Script");
        }
    
    }
}
