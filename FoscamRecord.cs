using System;
using System.Collections.Generic;
using System.Text;
using MLS.ZWave.Service.Rules;
using MLS.ZWave.BusinessObjects;
using System.Diagnostics;
using MLS.HA.DeviceController.Common.Device;

public class Max80 : ScriptBase, ScriptInterface {
    
    /// <summary>
    /// This script finds any node that is above 80% and sets it to 80%.
    /// 
    /// ALWAYS MAKE COPIES OF SCRIPTS YOU INTEND TO CUSTOMIZE OR YOUR CHANGES 
    /// COULD BE LOST.
    /// </summary>
    public void runScript() {
        try {

            // *******************************************************
            // ******* Change these to fit your system ***************
            var vlcPath = @"y:\program files (x86)\VideoLan\VLC\vlc.exe";

            // This is where the video will be saved. Be sure it has a trailing slash at the end (\)
            var videoOutputPath = @"Y:\SecurityVids\";
            
            // This is the camera's short id (double click a device inside InControl to find this id)
            var deviceShortId = 152;

            // How long the recording should go
            var recordTimeSeconds = 30;
            // *******************************************************
            // *******************************************************

            var device = getNodeByShortId(deviceShortId) as CameraDevice;
            if (device != null) {

                var vidfileName = string.Format("{0}.asf", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                var asfUrl = string.Format("http://{0}/videostream.asf?user={1}&pwd={2}", device.ip, device.userName, device.password);

                var psi = new ProcessStartInfo();
                psi.FileName = vlcPath;

                //var args = string.Format("{0} --qt-start-minimized --no-qt-notification --run-time=15 :demux=dump :demuxdump-file={1}{2} vlc://quit", device.providerDeviceId, videoOutputPath, fileName);
                //var args = string.Format("\"{0}\" --run-time={3} :demux=dump :demuxdump-file=\"{1}{2}\"", asfUrl, videoOutputPath, vidfileName, recordTimeSeconds);
                //var args = string.Format("-I dummy --dummy-quiet \"{0}\" --run-time={3} --sout=#transcode{{vcodec=h264,vb=1024,acodec=mp3,ab=128,channels=2,samplerate=44100}}:std{{access=file,mux=avi,dst=\"{1}{2}\"}} vlc://quit", asfUrl, videoOutputPath, vidfileName, recordTimeSeconds);
                //var args = string.Format("\"{0}\" --qt-start-minimized --no-qt-notification --run-time={3} :sout=#duplicate{{dst=file{{dst=Y:\\\\SecurityVids\\\\test.asf}},dst=display}} :sout-keep vlc://quit", asfUrl, videoOutputPath, vidfileName, recordTimeSeconds);
                //var args = string.Format("\"{0}\" --qt-start-minimized --no-qt-notification --run-time={3} :sout=#duplicate{{dst=file{{dst={1}{2}}}}} vlc://quit", asfUrl, videoOutputPath, vidfileName, recordTimeSeconds);

                // Known to work best with VLC 2.1.3
                var args = string.Format("\"{0}\" --qt-start-minimized --run-time={3} --sout=\"#duplicate{{dst=std{{access=file,mux=asf,dst='{1}{2}'}},dst=nodisplay}}\" vlc://quit", asfUrl, videoOutputPath, vidfileName, recordTimeSeconds);
                                                                                       
                writeFileLog("CamRecord:" + args);
                psi.Arguments = args;
                psi.CreateNoWindow = true;
                psi.UseShellExecute = false;
                System.Diagnostics.Process.Start(psi);
            }

        } catch (Exception ex) {
            // Log the exception here
            var message = ex.Message;
            writeFileLog("CamRecord:" + message);
        }
    }
}

