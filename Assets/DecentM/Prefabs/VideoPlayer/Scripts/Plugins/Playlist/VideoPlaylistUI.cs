﻿using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace DecentM.VideoPlayer.Plugins
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class VideoPlaylistUI : UdonSharpBehaviour
    {
        public Transform itemsRoot;
        public GameObject itemRendererTemplate;
        public VideoPlaylist playlist;

        private PlaylistItemRenderer[] instances;

        private bool isInstantiating = false;
        private int instantiatingIndex = 0;

        private void FixedUpdate()
        {
            if (!this.isInstantiating) return;
            if (this.instantiatingIndex >= playlist.urls.Length)
            {
                this.isInstantiating = false;
                return;
            }

            object[] item = playlist.urls[this.instantiatingIndex];
            this.instantiatingIndex++;

            if (item == null) return;

            GameObject instance = Instantiate(this.itemRendererTemplate);
            PlaylistItemRenderer renderer = instance.GetComponent<PlaylistItemRenderer>();

            if (renderer == null) return;

            VRCUrl url = (VRCUrl)item[0];
            Sprite thumbnail = (Sprite)item[1];
            string title = (string)item[2];
            string uploader = (string)item[3];
            string platform = (string)item[4];
            int views = (int)item[5];
            int likes = (int)item[6];
            string resolution = (string)item[7];
            int fps = (int)item[8];
            string description = (string)item[9];
            string duration = (string)item[10];

            instance.transform.SetPositionAndRotation(this.itemRendererTemplate.transform.position, this.itemRendererTemplate.transform.rotation);
            instance.transform.SetParent(this.itemsRoot, true);
            instance.transform.localScale = this.itemRendererTemplate.transform.localScale;
            instance.name = $"{renderer.name}_{this.instantiatingIndex - 1}";
            renderer.SetData(this.instantiatingIndex - 1, url, thumbnail, title, uploader, platform, views, likes, resolution, fps, description, duration);
            instance.gameObject.SetActive(true);

            PlaylistItemRenderer[] tmp = new PlaylistItemRenderer[this.instances.Length + 1];
            Array.Copy(this.instances, tmp, this.instances.Length);
            tmp[tmp.Length - 1] = renderer;
            this.instances = tmp;
        }

        void Start()
        {
            this.instances = new PlaylistItemRenderer[playlist.urls.Length];
            this.itemRendererTemplate.SetActive(false);

            this.isInstantiating = true;
        }
    }
}
