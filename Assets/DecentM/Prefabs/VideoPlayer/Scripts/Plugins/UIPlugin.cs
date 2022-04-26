﻿using System;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDK3.Components.Video;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;
using UnityEngine.UI;

namespace DecentM.VideoPlayer.Plugins
{
    public sealed class UIPlugin : VideoPlayerPlugin
    {
        public Slider progress;

        [Space]
        public Button playButton;
        public Button pauseButton;
        public Button stopButton;

        [Space]
        public TextMeshProUGUI status;
        public VRCUrlInputField urlInput;
        public Button enterButton;

        [Space]
        public Image brightnessImage;
        public Slider brightnessSlider;
        public Image volumeImage;
        public Slider volumeSlider;

        [Space]
        public Sprite volumeMutedIcon;
        public Sprite volumeLowIcon;
        public Sprite volumeMediumIcon;
        public Sprite volumeHighIcon;
        public Sprite brightnessLowIcon;
        public Sprite brightnessMediumIcon;
        public Sprite brightnessHighIcon;

        [Space]
        public TextMeshProUGUI info;

        [Space]
        public Button ownershipButton;
        public TextMeshProUGUI ownershipLabel;
        public Sprite lockIcon;
        public Sprite unlockIcon;
        public Sprite ownershipTransferIcon;

        [Space]
        public VRCUrl emptyUrl;

        private string HumanReadableTimestamp(float timestamp)
        {
            TimeSpan t = TimeSpan.FromSeconds(timestamp);

            if (t.Hours > 0) return string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);

            return string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
        }

        private string GetProgressIndicator(float timestamp, float duration)
        {
            if (float.IsInfinity(timestamp)) return "Live";
            if (float.IsInfinity(duration)) return $"{this.HumanReadableTimestamp(timestamp)} (Live)";

            return $"{this.HumanReadableTimestamp(timestamp)} / {this.HumanReadableTimestamp(duration)}";
        }

        #region Outputs

        protected override void OnProgress(float timestamp, float duration)
        {
            this.status.text = this.GetProgressIndicator(timestamp, duration);
            if (!float.IsInfinity(timestamp) && !float.IsInfinity(duration)) this.progress.SetValueWithoutNotify(Mathf.Max(timestamp / duration, 0.001f));
        }

        protected override void OnAutoRetry(int attempt)
        {
            this.status.text = $"Retrying (attempt {attempt + 1})...";
        }

        protected override void OnAutoRetrySwitchPlayer()
        {
            this.status.text = "Trying with a different player...";
        }

        protected override void OnAutoRetryLoadTimeout()
        {
            this.status.text = "Load timeout, retrying in 5 seconds...";
        }

        protected override void OnLoadRequested(VRCUrl url)
        {
            this.status.text = "Waiting for video player...";
        }

        protected override void OnLoadBegin()
        {
            this.status.text = "Loading...";
        }

        protected override void OnLoadBegin(VRCUrl url)
        {
            this.status.text = "Loading...";
            this.urlInput.SetUrl(url);
        }

        private void StoppedScreen(float duration)
        {
            this.playButton.interactable = true;
            this.pauseButton.interactable = false;
            this.stopButton.interactable = false;
            this.urlInput.gameObject.SetActive(true);
            this.enterButton.gameObject.SetActive(true);
            this.status.gameObject.SetActive(true);
            this.progress.gameObject.SetActive(false);
        }

        private void PausedScreen(float duration)
        {
            this.playButton.interactable = true;
            this.pauseButton.interactable = false;
            this.stopButton.interactable = true;
            this.urlInput.gameObject.SetActive(false);
            this.enterButton.gameObject.SetActive(false);
            this.status.gameObject.SetActive(true);
            this.progress.gameObject.SetActive(!float.IsInfinity(duration));
        }

        private void PlayingScreen(float duration)
        {
            this.playButton.interactable = false;
            this.pauseButton.interactable = true;
            this.stopButton.interactable = true;
            this.urlInput.gameObject.SetActive(false);
            this.enterButton.gameObject.SetActive(false);
            this.status.gameObject.SetActive(true);
            this.progress.gameObject.SetActive(!float.IsInfinity(duration));
        }

        protected override void OnLoadReady(float duration)
        {
            this.status.text = "Loaded, press play to begin";
            this.PausedScreen(duration);
        }

        protected override void OnLoadError(VideoError videoError)
        {
            switch (videoError)
            {
                case VideoError.InvalidURL:
                    this.status.text = "Invalid URL";
                    break;

                case VideoError.AccessDenied:
                    this.status.text = "Access denied";
                    break;

                // We don't care about the rest of the errors as they're handled by the AutoRetry plugin
            }
        }

        private Sprite GetBrightnessSprite(float alpha)
        {
            if (alpha < 1f / 3f) return this.brightnessLowIcon;
            if (alpha < 2f / 3f) return this.brightnessMediumIcon;

            return this.brightnessHighIcon;
        }

        protected override void OnBrightnessChange(float alpha)
        {
            this.brightnessSlider.SetValueWithoutNotify(alpha);
            this.brightnessImage.sprite = this.GetBrightnessSprite(alpha);
        }

        private Sprite GetVolumeSprite(float volume, bool muted)
        {
            if (muted || volume == 0) return this.volumeMutedIcon;

            if (volume < 1f / 3f) return this.volumeLowIcon;
            if (volume < 2f / 3f) return this.volumeMediumIcon;

            return this.volumeHighIcon;
        }

        protected override void OnVolumeChange(float volume, bool muted)
        {
            this.volumeSlider.SetValueWithoutNotify(volume);
            this.volumeImage.sprite = this.GetVolumeSprite(volume, muted);
        }

        protected override void OnMutedChange(bool muted, float volume)
        {
            this.volumeSlider.interactable = !muted;
            this.volumeImage.sprite = this.GetVolumeSprite(volume, muted);
        }

        protected override void OnPlaybackEnd()
        {
            this.status.text = "Playback ended";
            this.StoppedScreen(this.system.GetDuration());
        }

        protected override void OnPlaybackStart(float timestamp)
        {
            this.status.text = "Playing...";
            this.PlayingScreen(this.system.GetDuration());
        }

        protected override void OnPlaybackStop(float timestamp)
        {
            this.status.text = $"Paused - {this.GetProgressIndicator(timestamp, this.system.GetDuration())}";
            this.PausedScreen(this.system.GetDuration());
        }

        protected override void OnUnload()
        {
            this.status.text = "Stopped";
            this.StoppedScreen(this.system.GetDuration());
            this.urlInput.SetUrl(this.emptyUrl);
        }

        protected override void OnScreenResolutionChange(Renderer screen, float width, float height)
        {
            if (width / height != 1920f / 1080f)
            {
                this.info.text = $"{width}x{height}";
            } else
            {
                this.info.text = $"{height}p";
            }
        }

        private bool selfOwned = false;

        protected override void OnOwnershipChanged(int previousOwnerId, VRCPlayerApi nextOwner)
        {
            if (nextOwner == null || !nextOwner.IsValid()) return;

            if (nextOwner != Networking.LocalPlayer) this.ownershipButton.image.sprite = this.ownershipTransferIcon;
            else if (this.ownershipLocked) this.ownershipButton.image.sprite = this.lockIcon;
            else this.ownershipButton.image.sprite = this.unlockIcon;

            this.selfOwned = nextOwner == Networking.LocalPlayer;

            this.ownershipLabel.text = nextOwner.displayName;
        }

        private bool ownershipLocked = false;

        protected override void OnOwnershipSecurityChanged(bool locked)
        {
            this.ownershipLocked = locked;
            
            this.ownershipButton.image.sprite = locked ? this.lockIcon : this.unlockIcon;
        }

        private VRCPlayerApi[] GetUnloadedPlayers(int[] loadedPlayers)
        {
            VRCPlayerApi[] allPlayers = new VRCPlayerApi[64];
            VRCPlayerApi[] unloadedPlayers = new VRCPlayerApi[0];

            VRCPlayerApi.GetPlayers(allPlayers);

            foreach (VRCPlayerApi player in allPlayers)
            {
                if (player == null || !player.IsValid()) continue;

                bool found = false;

                // We always consider the local player as loaded
                if (player != Networking.LocalPlayer)
                {
                    foreach (int loadedPlayer in loadedPlayers)
                    {
                        if (loadedPlayer == player.playerId)
                        {
                            found = true;
                            break;
                        }
                    }
                }

                if (!found)
                {
                    VRCPlayerApi[] tmp = new VRCPlayerApi[unloadedPlayers.Length + 1];
                    Array.Copy(unloadedPlayers, tmp, unloadedPlayers.Length);
                    tmp[tmp.Length - 1] = player;
                    unloadedPlayers = tmp;
                }
            }

            return unloadedPlayers;
        }

        protected override void OnRemotePlayerLoaded(int[] loadedPlayers)
        {
            VRCPlayerApi[] unloadedPlayers = this.GetUnloadedPlayers(loadedPlayers);

            // int total = VRCPlayerApi.GetPlayerCount();
            // int waitingForCount = total - loadedPlayers.Length - 1;

            if (unloadedPlayers.Length == 1) this.status.text = $"Waiting for {unloadedPlayers[0]}";
            else this.status.text = $"Waiting for {unloadedPlayers.Length} players to load";
        }

        #endregion

        #region Inputs

        public void OnPlayButton()
        {
            this.system.StartPlayback();
        }

        public void OnPauseButton()
        {
            this.system.PausePlayback();
        }

        public void OnStopButton()
        {
            this.system.UnloadVideo();
        }

        public void OnBrightnessSlider()
        {
            float brightness = this.system.GetBrightness();

            // Ignore duplicate values
            if (this.brightnessSlider.value == brightness) return;

            this.system.SetBrightness(this.brightnessSlider.value);
        }

        public void OnVolumeSlider()
        {
            float volume = this.system.GetVolume();

            // Ignore duplicate values
            if (this.volumeSlider.value == volume) return;

            this.system.SetVolume(this.volumeSlider.value);
        }

        public void OnMuteButton()
        {
            this.system.SetMuted(!this.system.GetMuted());
        }

        public void OnProgressSlider()
        {
            this.system.Seek(this.progress.value * this.system.GetDuration());
        }

        public void OnUrlInput()
        {
            this.system.LoadVideo(this.urlInput.GetUrl());
        }

        public void OnOwnershipButton()
        {
            if (selfOwned)
            {
                this.events.OnOwnershipSecurityChanged(!this.ownershipLocked);
            } else
            {
                this.events.OnOwnershipRequested();
            }
        }

        #endregion
    }
}
