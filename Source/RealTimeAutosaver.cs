using System;
using System.Threading.Tasks;
using System.Timers;
using Verse;

namespace RealTimeAutoSave
{
    public static class RealTimeAutosaver
    {
        private const int MONITORING_INTERVAL = 5000;

        private static Timer timer = new Timer();

        static RealTimeAutosaver()
        {
            timer.Elapsed += new ElapsedEventHandler(delegate (object source, ElapsedEventArgs e) {
                TryAutosave();
            });
        }

        public static void ScheduleAutosave(int? delay = null)
        {
            timer.Stop();
            timer.Interval = delay != null ? (int) delay : RealTimeAutoSaveSettings.RealTimeInterval * 1000 * 60;
            timer.Start();
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
                if (Current.Game != null && !Find.WindowStack.WindowsForcePause && !LongEventHandler.ForcePause)
                {
                    LongEventHandler.QueueLongEvent(new Action(Find.Autosaver.DoAutosave), "Autosaving", false, null, true);
                    delay = null;
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                return false;
            }
            finally
            {
                if (RealTimeAutoSaveSettings.AutoSaveMode == AutoSaveMode.RealTime)
                {
                    ScheduleAutosave(delay);
                }
            }
        }
    }
}
