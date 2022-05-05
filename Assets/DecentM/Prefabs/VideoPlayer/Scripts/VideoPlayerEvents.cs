﻿
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components.Video;
using DecentM.Pubsub;

namespace DecentM.VideoPlayer
{
    public enum VideoPlayerEvent
    {
        OnDebugLog,

        OnVideoPlayerInit,
        OnBrightnessChange,
        OnVolumeChange,
        OnMutedChange,
        OnFpsChange,
        OnScreenResolutionChange,
        OnScreenTextureChange,

        OnPlaybackStart,
        OnPlaybackStop,
        OnPlaybackEnd,
        OnProgress,

        OnUrlValidationFailed,
        OnLoadBegin,
        OnLoadReady,
        OnLoadError,
        OnUnload,
        OnLoadRequested,

        OnAutoRetry,
        OnAutoRetrySwitchPlayer,
        OnAutoRetryLoadTimeout,
        OnAutoRetryAbort,
        OnAutoRetryAllPlayersFailed,

        OnOwnershipChanged,
        OnOwnershipSecurityChanged,
        OnOwnershipRequested,

        OnRemotePlayerLoaded,

        OnUIVisibilityChange,

        OnMetadataChange,
        OnSubtitleLanguageOptionsChange,
        OnSubtitleLanguageRequested,
    }

    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public sealed class VideoPlayerEvents : PubsubHost
    {
        #region Core

        public void OnDebugLog(string message)
        {
            this.BroadcastEvent(VideoPlayerEvent.OnDebugLog, message);
        }

        public void OnVideoPlayerInit()
        {
            this.BroadcastEvent(VideoPlayerEvent.OnVideoPlayerInit);
        }

        public void OnBrightnessChange(float alpha)
        {
            this.BroadcastEvent(VideoPlayerEvent.OnBrightnessChange, alpha);
        }

        public void OnVolumeChange(float volume, bool muted)
        {
            this.BroadcastEvent(VideoPlayerEvent.OnVolumeChange, volume, muted);
        }

        public void OnMutedChange(bool muted, float volume)
        {
            this.BroadcastEvent(VideoPlayerEvent.OnMutedChange, muted, volume);
        }

        public void OnFpsChange(int fps)
        {
            this.BroadcastEvent(VideoPlayerEvent.OnFpsChange, fps);
        }

        public void OnScreenResolutionChange(Renderer screen, float width, float height)
        {
            this.BroadcastEvent(VideoPlayerEvent.OnScreenResolutionChange, screen, width, height);
        }

        public void OnPlaybackStart(float timestamp)
        {
            this.BroadcastEvent(VideoPlayerEvent.OnPlaybackStart, timestamp);
        }

        public void OnPlaybackStop(float timestamp)
        {
            this.BroadcastEvent(VideoPlayerEvent.OnPlaybackStop, timestamp);
        }

        public void OnProgress(float timestamp, float duration)
        {
            this.BroadcastEvent(VideoPlayerEvent.OnProgress, timestamp, duration);
        }

        public void OnUrlValidationFailed(VRCUrl url)
        {
            this.BroadcastEvent(VideoPlayerEvent.OnUrlValidationFailed, url);
        }

        public void OnPlaybackEnd()
        {
            this.BroadcastEvent(VideoPlayerEvent.OnPlaybackEnd);
        }

        public void OnLoadBegin(VRCUrl url)
        {
            this.BroadcastEvent(VideoPlayerEvent.OnLoadBegin, url);
        }

        public void OnLoadRequested(VRCUrl url)
        {
            this.BroadcastEvent(VideoPlayerEvent.OnLoadRequested, url);
        }

        public void OnLoadReady(float duration)
        {
            this.BroadcastEvent(VideoPlayerEvent.OnLoadReady, duration);
        }

        public void OnUnload()
        {
            this.BroadcastEvent(VideoPlayerEvent.OnUnload);
        }

        public void OnLoadError(VideoError videoError)
        {
            this.BroadcastEvent(VideoPlayerEvent.OnLoadError, videoError);
        }

        #endregion

        #region Plugins

        public void OnAutoRetry(int attempt)
        {
            this.BroadcastEvent(VideoPlayerEvent.OnAutoRetry, attempt);
        }

        public void OnAutoRetrySwitchPlayer()
        {
            this.BroadcastEvent(VideoPlayerEvent.OnAutoRetrySwitchPlayer);
        }

        public void OnAutoRetryLoadTimeout(int timeout)
        {
            this.BroadcastEvent(VideoPlayerEvent.OnAutoRetryLoadTimeout, timeout);
        }

        public void OnAutoRetryAbort()
        {
            this.BroadcastEvent(VideoPlayerEvent.OnAutoRetryAbort);
        }

        public void OnAutoRetryAllPlayersFailed()
        {
            this.BroadcastEvent(VideoPlayerEvent.OnAutoRetryAllPlayersFailed);
        }

        public void OnOwnershipChanged(int previousOwnerId, VRCPlayerApi nextOwner)
        {
            this.BroadcastEvent(VideoPlayerEvent.OnOwnershipChanged, previousOwnerId, nextOwner);
        }

        public void OnOwnershipSecurityChanged(bool locked)
        {
            this.BroadcastEvent(VideoPlayerEvent.OnOwnershipSecurityChanged, locked);
        }

        public void OnOwnershipRequested()
        {
            this.BroadcastEvent(VideoPlayerEvent.OnOwnershipRequested);
        }

        public void OnScreenTextureChange()
        {
            this.BroadcastEvent(VideoPlayerEvent.OnScreenTextureChange);
        }

        public void OnRemotePlayerLoaded(int[] loadedPlayers)
        {
            this.BroadcastEvent(VideoPlayerEvent.OnRemotePlayerLoaded, loadedPlayers);
        }

        public void OnUIVisibilityChange(bool visible)
        {
            this.BroadcastEvent(VideoPlayerEvent.OnUIVisibilityChange, visible);
        }

        public void OnMetadataChange(
            string title,
            string uploader,
            string siteName,
            int viewCount,
            int likeCount,
            string resolution,
            int fps,
            string description,
            string duration,
            string[][] subtitles
        )
        {
            this.BroadcastEvent(
                VideoPlayerEvent.OnMetadataChange,
                title,
                uploader,
                siteName,
                viewCount,
                likeCount,
                resolution,
                fps,
                description,
                duration,
                subtitles
            );
        }

        public void OnSubtitleLanguageOptionsChange(string[] newOptions)
        {
            this.BroadcastEvent(VideoPlayerEvent.OnSubtitleLanguageOptionsChange, newOptions);
        }

        public void OnSubtitleLanguageRequested(string language)
        {
            this.BroadcastEvent(VideoPlayerEvent.OnSubtitleLanguageRequested, language);
        }

        #endregion
    }
}
