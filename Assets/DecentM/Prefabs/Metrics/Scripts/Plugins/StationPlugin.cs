﻿using VRC.SDKBase;

namespace DecentM.Metrics.Plugins
{
    public class StationPlugin : IndividualTrackingPlugin
    {
        private bool locked = true;
        private bool reportedState = false;

        private void DoReport(bool state)
        {
            VRCUrl url = this.urlStore.GetStationUrl(this.metricName, state);
            if (url == null) return;

            this.system.RecordMetric(url, Metric.Station);
            this.reportedState = state;

            // Only set the lock after exiting the trigger, as we'd rather miss reporting a second entry than miss
            // reporting the exit due to the lock
            if (!state)
            {
                this.locked = true;
                this.SendCustomEventDelayedSeconds(nameof(Unlock), 5.2f);
            }
        }

        protected override void OnMetricsSystemInit()
        {
            this.Unlock();
        }

        public void Unlock()
        {
            this.locked = false;
        }

        public override void OnStationEntered(VRCPlayerApi player)
        {
            if (player == null || !player.IsValid() || player != Networking.LocalPlayer) return;
            if (this.locked || this.reportedState) return;

            this.DoReport(true);
        }

        public override void OnStationExited(VRCPlayerApi player)
        {
            if (player == null || !player.IsValid() || player != Networking.LocalPlayer) return;
            if (this.locked || !this.reportedState) return;

            this.DoReport(false);
        }
    }
}
