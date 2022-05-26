﻿using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace DecentM.Metrics
{
    // This class only exists so there's something to attach the inspector to
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class MetricsUI : UdonSharpBehaviour
    {
        // Autogenerated data
        public string builtAt = "";
        public string sdk = "";
        public string unity = "";
        public string sceneName = "";

        // Manually entered data
        public string worldName = "";
        public string worldAuthor = "";
        public int worldCapacity = 64;
        public int instanceCapacity = 64;
        public string metricsServerBaseUrl = "";
        public int minFps = 0;
        public int maxFps = 144;
    }
}
