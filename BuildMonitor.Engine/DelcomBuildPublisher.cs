using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.Build.Client;
using System.Threading;

namespace BuildMonitor.Engine
{
    public class DelcomBuildPublisher
    {
        private readonly StringBuilder deviceName = new StringBuilder(DelcomBuildIndicator.MAXDEVICENAMELEN);
        private readonly ILog log;
        private readonly BuildWatcher watcher;
        private readonly IDictionary<string, BuildStatus> BuildStatuses = new Dictionary<string, BuildStatus>();

        public DelcomBuildPublisher(ILog log, BuildWatcher buildWatcher)
        {
            watcher = buildWatcher;
            watcher.BuildCompletionEvent += watcher_BuildCompletionEvent;
            watcher.BuildQualityChangeEvent += watcher_BuildQualityChangeEvent;
            watcher.BuildWatcherInitializing += Init;
            watcher.BuildWatcherStopping += Close;
            this.log = log;
        }

        public void Init(object sender, IEnumerable<BuildStoreEventArgs> events)
        {
            Close(null, null); //ensure the LEDS are all off

            if (!events.Any()) return;

            foreach (var buildEvent in events)
            {
                BuildStatuses[buildEvent.Data.BuildDefinition.Name] = buildEvent.Data.Status;
                log.Information(string.Format("Build {2} {1}: {0}", buildEvent.Data.Status,
                                              buildEvent.Data.LabelName, buildEvent.Data.BuildDefinition.TeamProject));
            }
            if (BuildStatuses.Any(b => b.Value == BuildStatus.Failed))
            {
                SetLED(DelcomBuildIndicator.REDLED, true, false);
            }
            else if (Properties.Settings.Default.EnableDelcomLightColourChangeWhileBuilding &&
                     BuildStatuses.Any(b => b.Value == BuildStatus.InProgress))
            {
                SetLED(DelcomBuildIndicator.GREENLED, false, false);
                SetLED(DelcomBuildIndicator.BLUELED, true,
                       Properties.Settings.Default.EnableDelcomFlashingWhileBuilding);
                SetLED(DelcomBuildIndicator.REDLED, false, false);
            }
            else if (BuildStatuses.Any(b => b.Value == BuildStatus.PartiallySucceeded))
            {
                SetLED(DelcomBuildIndicator.REDLED, true, false);
                SetLED(DelcomBuildIndicator.GREENLED, true, false);
            }
            else if (BuildStatuses.All(b => b.Value == BuildStatus.Succeeded))
            {
                SetLED(DelcomBuildIndicator.GREENLED, true, false);
            }
        }

        private void watcher_BuildQualityChangeEvent(object sender, BuildStoreEventArgs buildWatcherEventArgs)
        {
            BuildQualityChangeEvent(buildWatcherEventArgs);
        }

        private void watcher_BuildCompletionEvent(object sender, BuildStoreEventArgs buildWatcherEventArgs)
        {
            BuildCompletionEvent(buildWatcherEventArgs);
        }

        private uint GetDelcomDeviceHandle()
        {
            if (string.IsNullOrEmpty(deviceName.ToString()))
            {
                int Result = 0;
                try
                {
                    // Search for the first match USB device, For USB IO Chips use USBIODS
                    Result = DelcomBuildIndicator.DelcomGetNthDevice(DelcomBuildIndicator.USBDELVI, 0, deviceName);
                }
                catch (Exception)
                {
                    log.Error("Delcom device not found!");
                }
                if (Result == 0)
                {
                    log.Error("Delcom device not found!");
                    return 0;
                }
            }

            uint hUSB = DelcomBuildIndicator.DelcomOpenDevice(deviceName, 0); // open the device
            log.Information("Delcom device connected.");
            return hUSB;
        }

        private void BuildCompletionEvent(BuildStoreEventArgs buildStoreEvent)
        {
            var status = buildStoreEvent.Data.Status;
            log.Information(string.Format("[Build Event] {0} : {1} : {2}", buildStoreEvent.Data.Status,
                                          buildStoreEvent.Data.BuildDefinition, buildStoreEvent.Data.Quality));

            BuildStatuses[buildStoreEvent.Data.BuildDefinition.Name] = status;
            var checkAllBuildStatuses = !Properties.Settings.Default.ShowStatusOfLastBuildOnly;

            bool flashOnStatusChange = Properties.Settings.Default.FlashLightOnBuildStatusChange;
            int? flashDuration = Properties.Settings.Default.FlashLightOnBuildStatusChange ? 5 : new int?();

            if ((checkAllBuildStatuses && BuildStatuses.All(b => b.Value == BuildStatus.Succeeded)) ||
                (!checkAllBuildStatuses && status == BuildStatus.Succeeded))
            {
                SetLED(DelcomBuildIndicator.GREENLED, true, flashOnStatusChange, flashDuration);
                SetLED(DelcomBuildIndicator.BLUELED, false, false);
                SetLED(DelcomBuildIndicator.REDLED, false, false);
            }
            if (status == BuildStatus.NotStarted || status == BuildStatus.InProgress)
            {
                if (Properties.Settings.Default.EnableDelcomLightColourChangeWhileBuilding)
                {
                    SetLED(DelcomBuildIndicator.GREENLED, false, false);
                    SetLED(DelcomBuildIndicator.BLUELED, true,
                           Properties.Settings.Default.EnableDelcomFlashingWhileBuilding);
                    SetLED(DelcomBuildIndicator.REDLED, false, false);
                }
                else if (flashOnStatusChange)
                {
                    SetLED(DelcomBuildIndicator.BLUELED, true, true, flashDuration, true);
                }
            }
            if (status == BuildStatus.PartiallySucceeded)
            {
                SetLED(DelcomBuildIndicator.GREENLED, true, false);
                SetLED(DelcomBuildIndicator.BLUELED, false, false);
                SetLED(DelcomBuildIndicator.REDLED, true, flashOnStatusChange, flashDuration);
            }
            if (status == BuildStatus.Stopped)
            {
                // Don't care
            }
            if (status == BuildStatus.Failed)
            {
                SetLED(DelcomBuildIndicator.REDLED, true, flashOnStatusChange, flashDuration);
                SetLED(DelcomBuildIndicator.GREENLED, false, false);
                SetLED(DelcomBuildIndicator.BLUELED, false, false);
            }
        }

        public void BuildQualityChangeEvent(BuildStoreEventArgs buildStoreEvent)
        {
            log.Information("[Quality Change Event]");
        }

        public void Close(object sender, EventArgs eventArgs)
        {
            SetLED(DelcomBuildIndicator.GREENLED, false, false);
            SetLED(DelcomBuildIndicator.BLUELED, false, false);
            SetLED(DelcomBuildIndicator.REDLED, false, false);
        }

        private void SetLED(byte led, bool turnItOn, bool flashIt)
        {
            SetLED(led, turnItOn, flashIt, null, false);
        }

        private void SetLED(byte led, bool turnItOn, bool flashIt, int? flashDurationInSeconds)
        {
            SetLED(led, turnItOn, flashIt, flashDurationInSeconds, false);
        }

        private void SetLED(byte led, bool turnItOn, bool flashIt, int? flashDurationInSeconds,
                            bool turnOffAfterFlashing)
        {
            try
            {
                uint hUSB = GetDelcomDeviceHandle(); // open the device
                if (hUSB == 0) return;
                if (turnItOn)
                {
                    if (flashIt)
                    {
                        DelcomBuildIndicator.DelcomLEDControl(hUSB, led, DelcomBuildIndicator.LEDFLASH);
                        if (flashDurationInSeconds.HasValue)
                        {
                            Thread.Sleep(flashDurationInSeconds.Value*1000);
                            byte ledStatus = turnOffAfterFlashing
                                                 ? DelcomBuildIndicator.LEDOFF
                                                 : DelcomBuildIndicator.LEDON;
                            DelcomBuildIndicator.DelcomLEDControl(hUSB, led, ledStatus);
                        }
                    }
                    else
                    {
                        DelcomBuildIndicator.DelcomLEDControl(hUSB, led, DelcomBuildIndicator.LEDON);
                    }
                }
                else
                {
                    DelcomBuildIndicator.DelcomLEDControl(hUSB, led, DelcomBuildIndicator.LEDOFF);
                }
                DelcomBuildIndicator.DelcomCloseDevice(hUSB);
            }
            catch (Exception)
            {
                log.Error("Delcom device communication failed.");
            }
        }
    }
}