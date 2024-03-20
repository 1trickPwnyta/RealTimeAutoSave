using System;
using System.Threading.Tasks;
using System.Timers;
using Verse;

namespace RealTimeAutoSave
{
    public static class RealTimeAutosaver
    {
        // If autosave is skipped for some reason, we try again in 5 seconds
        private const int MONITORING_INTERVAL = 5000;

        private static Timer timer = new Timer();

        static RealTimeAutosaver()
        {
            timer.Elapsed += new ElapsedEventHandler(delegate (object source, ElapsedEventArgs e) {
                TryAutosave();
            });
        }

        // delay can be null, which means don't override the interval as configured in
        // the mod settings
        public static void ScheduleAutosave(int? delay = null)
        {
            // Don't do anything if the timer is already set to the correct interval and turned on
            if (delay == null && timer.Enabled && timer.Interval == RealTimeAutoSaveSettings.RealTimeInterval * 1000 * 60)
            {
                return;
            }

            timer.Stop();
            timer.Interval = delay != null ? (int) delay : RealTimeAutoSaveSettings.RealTimeInterval * 1000 * 60;
            timer.Start();
            Debug.Log($"Set timer for {timer.Interval}");
        }

        public static void CancelAutosave()
        {
            timer.Stop();
        }

        public static bool TryAutosave()
        {
            Debug.Log("TryAutosave called.");

            timer.Stop();
            int? delay = MONITORING_INTERVAL;

            try
            {
                // Only perform the autosave under certain conditions, otherwise skip it for now
                if (Current.Game != null && !Find.WindowStack.WindowsForcePause && !LongEventHandler.ForcePause)
                {
                    // Perform the autosave and set delay to null in order to use the interval in the mod settings
                    LongEventHandler.QueueLongEvent(new Action(Find.Autosaver.DoAutosave), "Autosaving", false, null, true);
                    delay = null;
                    Debug.Log("TryAutosave succeeded.");
                    return true;
                }

                // When skipped, we use the monitoring interval to try again in a short while
                Debug.Log("TryAutosave skipped.");
                return false;
            }
            catch (Exception e)
            {
                // This shouldn't happen
                Debug.Log(e.ToString());
                return false;
            }
            finally
            {
                // Whether there was an exception or not, if the mod is configured to use a real-time 
                // interval, schedule the next autosave attempt according to the delay we set above
                if (RealTimeAutoSaveSettings.AutoSaveMode == AutoSaveMode.RealTime)
                {
                    ScheduleAutosave(delay);
                }
            }
        }
    }
}
