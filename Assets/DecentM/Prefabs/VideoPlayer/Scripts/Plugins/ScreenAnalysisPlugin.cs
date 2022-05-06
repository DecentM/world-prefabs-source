﻿using System;
using System.Collections;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace DecentM.VideoPlayer.Plugins
{
    public class ScreenAnalysisPlugin : VideoPlayerPlugin
    {
        public float targetFps = 30;
        public float historyLengthSeconds = 0.3333f;
        public int sampleSize = 40;
        public float sampleRandomisationFactor = 1f;

        private bool isRunning = false;

        public Texture2D fetchTexture;
        public Texture2D sampleVisualisationTexture;

        public Texture2D averageColourHistoryTexture;
        public Texture2D smoothedAverageColourHistoryTexture;

        public Texture2D mostVibrantColourHistoryTexture;
        public Texture2D smoothedMostVibrantColourHistoryTexture;

        public Texture2D brightestColourHistoryTexture;
        public Texture2D smoothedBrightestColourHistoryTexture;

        private float elapsed = 0;
        private float fps = 0;

        private new Camera camera;
        private bool enableSampleVisualisation = true;

        protected override void _Start()
        {
            this.camera = GetComponent<Camera>();

            if (this.fetchTexture == null)
            {
                Debug.LogError($"[ScreenAnalysisPlugin] Missing fetch texture!");
                this.enabled = false;
                return;
            }

            if (this.sampleVisualisationTexture != null && (this.sampleVisualisationTexture.width != this.camera.scaledPixelWidth || this.sampleVisualisationTexture.height != this.camera.scaledPixelHeight))
            {
                Debug.LogWarning($"[ScreenAnalysisPlugin] Sample visualisation texture size must match the camera. Expected {this.camera.scaledPixelWidth}x{this.camera.scaledPixelHeight}, got {this.sampleVisualisationTexture.width}x{this.sampleVisualisationTexture.height}.");
                this.enableSampleVisualisation = false;
            }

            this.camera.enabled = false;

            this.Reset();
        }

        public void Reset()
        {
            int length = Mathf.CeilToInt(this.targetFps * this.historyLengthSeconds);
            this.averageHistory = new Color[length];
            this.smoothedAverageHistory = new Color[length];
            this.mostVibrantHistory = new Color[length];
            this.smoothedMostVibrantHistory = new Color[length];
            this.brightestHistory = new Color[length];
            this.smoothedBrightestHistory = new Color[length];
            this.UpdateHistoryValues(new Color[] { Color.black });
        }

        private void LateUpdate()
        {
            if (!this.isRunning || !this.camera) return;

            this.elapsed += Time.unscaledDeltaTime;
            if (elapsed < 1f / this.fps) return;
            elapsed = 0;

            camera.Render();
        }

        private float GetVibrance(Color colour)
        {
            float rgDiff = Mathf.Abs(colour.r - colour.g);
            float rbDiff = Mathf.Abs(colour.r - colour.b);
            float gbDiff = Mathf.Abs(colour.g - colour.b);

            return (rgDiff + rbDiff + gbDiff) / 3;
        }

        private Color GetMostVibrant(Color[] colours)
        {
            Color result = Color.black;

            foreach (Color colour in colours)
            {
                float vibrance = GetVibrance(colour);
                if (vibrance > GetVibrance(result)) result = colour;
            }

            return result;
        }

        private float GetBrightness(Color colour)
        {
            return (colour.r + colour.g + colour.b) / 3;
        }

        private Color GetBrightest(Color[] colours)
        {
            Color result = Color.black;

            foreach (Color colour in colours)
            {
                float brightness = GetBrightness(colour);
                if (brightness > GetBrightness(result)) result = colour;
            }

            return result;
        }

        private Color[] AddColourHistory(Color[] history, Color colour)
        {
            Color[] tmp = new Color[history.Length];
            Array.Copy(history, 0, tmp, 1, history.Length - 1);
            tmp[0] = colour;
            return tmp;
        }

        private Color GetAverage(Color[] colors)
        {
            if (colors == null || colors.Length == 0) return Color.black;

            Color average = new Color();

            foreach (Color color in colors)
            {
                average.r += color.r;
                average.g += color.g;
                average.b += color.b;
            }

            average.r /= colors.Length;
            average.g /= colors.Length;
            average.b /= colors.Length;

            return average;
        }

        private Vector2Int GetCoordsForIndex(int index, int width)
        {
            Vector2Int result = new Vector2Int();

            result.x = index % width;
            result.y = Mathf.FloorToInt(index / width);

            return result;
        }

        private Color GenerateRandomColour()
        {
            Color result = new Color();

            result.r = UnityEngine.Random.Range(0.0f, 1.0f);
            result.g = UnityEngine.Random.Range(0.0f, 1.0f);
            result.b = UnityEngine.Random.Range(0.0f, 1.0f);

            return result;
        }

        private void ResizeFetchTexture(Texture2D outputTexture, Vector2Int oldSize)
        {
            float widthSkip = (float)oldSize.x / outputTexture.width;
            float heightSkip = (float)oldSize.y / outputTexture.height;

            for (int i = 0; i < outputTexture.width * outputTexture.height; i++)
            {
                int randomOffset = Mathf.FloorToInt(UnityEngine.Random.Range(0, Mathf.Min(oldSize.x, oldSize.y) / 2));
                Vector2Int center = new Vector2Int(oldSize.x / 2 + randomOffset, oldSize.y / 2 + randomOffset);
                Vector2Int writeCoords = GetCoordsForIndex(i, outputTexture.width);

                Vector2Int rawReadCoords = new Vector2Int(Mathf.FloorToInt(writeCoords.x * widthSkip), Mathf.FloorToInt(writeCoords.y * heightSkip));
                Vector2Int readCoords = new Vector2Int(rawReadCoords.x + (center.x - rawReadCoords.x) / outputTexture.width / 3, rawReadCoords.y + (center.y - rawReadCoords.y) / outputTexture.height / 3);

                fetchTexture.ReadPixels(new Rect(readCoords.x, readCoords.y, 1, 1), 0, 0);
                Color colour = fetchTexture.GetPixel(0, 0);

                outputTexture.SetPixel(writeCoords.x, outputTexture.height - writeCoords.y, colour == Color.black ? Color.red : colour);
            }
        }

        private Vector2Int[] GenerateSamplePoints(int sampleCount, Vector2Int size)
        {
            Vector2Int[] result = new Vector2Int[sampleCount];
            int skip = size.x * size.y / sampleCount;
            float twitchXMultiplier = size.x / sampleCount;
            float twitchYMultiplier = size.y / sampleCount;

            for (int i = 0; i < sampleCount; i++)
            {
                Vector2Int rawReadCoords = GetCoordsForIndex(i * skip, size.x);

                float twitchX = UnityEngine.Random.Range(-1, 1) * twitchXMultiplier * this.sampleRandomisationFactor;
                float twitchY = UnityEngine.Random.Range(-1, 1) * twitchYMultiplier * this.sampleRandomisationFactor;
                Vector2Int readCoords = new Vector2Int(
                    Mathf.Max(Mathf.Min(Mathf.RoundToInt(rawReadCoords.x + twitchX), size.x), 0),
                    Mathf.Max(Mathf.Min(Mathf.RoundToInt(rawReadCoords.y + twitchY), size.y), 0)
                );

                result[i] = readCoords;
            }

            return result;
        }

        private Color[] SampleFetchTexture(Vector2Int[] samplePoints)
        {
            Color[] result = new Color[samplePoints.Length];

            for (int i = 0; i < samplePoints.Length; i++)
            {
                Vector2Int readCoords = samplePoints[i];

                fetchTexture.ReadPixels(new Rect(readCoords.x, readCoords.y, 1, 1), 0, 0);
                Color colour = fetchTexture.GetPixel(0, 0);

                result[i] = colour;
            }

            return result;
        }

        private void UpdateTexture(Texture2D texture, Color[] colours)
        {
            // Create an output of all blacks in case there are less colours in the input
            Color[] output = new Color[texture.width * texture.height];
            for (int i = 0; i < output.Length; i++) output[i] = Color.black;

            // Copy all colours to the output array and throw away ones that wouldn't fit
            if (colours.Length <= output.Length) Array.Copy(colours, output, colours.Length);
            else Array.Copy(colours, output, output.Length);

            texture.SetPixels(output);
            texture.Apply();
        }

        private Color[] averageHistory;
        private Color[] smoothedAverageHistory;
        private Color[] brightestHistory;
        private Color[] smoothedBrightestHistory;
        private Color[] mostVibrantHistory;
        private Color[] smoothedMostVibrantHistory;

        private void OnPostRender()
        {
            Vector2Int[] samplePoints = this.GenerateSamplePoints(Mathf.Min(Mathf.Max(this.sampleSize, 1), 1000), new Vector2Int(this.camera.scaledPixelWidth, this.camera.scaledPixelHeight));
            Color[] colours = SampleFetchTexture(samplePoints);

            this.UpdateHistoryValues(colours);

            if (this.sampleVisualisationTexture != null && this.enableSampleVisualisation)
            {
                Color colour = this.GenerateRandomColour();

                foreach (Vector2Int point in samplePoints)
                {
                    this.sampleVisualisationTexture.SetPixel(point.x, point.y, colour);
                }

                this.sampleVisualisationTexture.Apply();
            }
        }

        private void UpdateHistoryValues(Color[] colours)
        {
            if (this.averageColourHistoryTexture != null)
            {
                this.averageHistory = this.AddColourHistory(this.averageHistory, this.GetAverage(colours));
                this.UpdateTexture(this.averageColourHistoryTexture, this.averageHistory);
            }

            if (this.smoothedAverageColourHistoryTexture != null)
            {
                this.smoothedAverageHistory = this.AddColourHistory(this.smoothedAverageHistory, this.GetAverage(this.averageHistory));
                this.UpdateTexture(this.smoothedAverageColourHistoryTexture, this.smoothedAverageHistory);
            }

            if (this.brightestColourHistoryTexture != null)
            {
                this.brightestHistory = this.AddColourHistory(this.brightestHistory, this.GetBrightest(colours));
                this.UpdateTexture(this.brightestColourHistoryTexture, this.brightestHistory);
            }

            if (this.smoothedBrightestColourHistoryTexture != null)
            {
                this.smoothedBrightestHistory = this.AddColourHistory(this.smoothedBrightestHistory, this.GetAverage(this.brightestHistory));
                this.UpdateTexture(this.smoothedBrightestColourHistoryTexture, this.smoothedBrightestHistory);
            }

            if (this.mostVibrantColourHistoryTexture != null)
            {
                this.mostVibrantHistory = this.AddColourHistory(this.mostVibrantHistory, this.GetMostVibrant(colours));
                this.UpdateTexture(this.mostVibrantColourHistoryTexture, this.mostVibrantHistory);
            }

            if (this.smoothedMostVibrantColourHistoryTexture != null)
            {
                this.smoothedMostVibrantHistory = this.AddColourHistory(this.smoothedMostVibrantHistory, this.GetAverage(this.mostVibrantHistory));
                this.UpdateTexture(this.smoothedMostVibrantColourHistoryTexture, this.smoothedMostVibrantHistory);
            }
        }

        protected override void OnMetadataChange(string title, string uploader, string siteName, int viewCount, int likeCount, string resolution, int fps, string description, string duration, string[][] subtitles)
        {
            this.fps = fps <= 0 ? this.targetFps : Mathf.Min(fps, targetFps);
        }

        protected override void OnPlaybackEnd()
        {
            this.isRunning = false;
            this.Reset();
        }

        protected override void OnLoadApproved(VRCUrl url)
        {
            this.isRunning = false;
            this.Reset();
        }

        protected override void OnPlaybackStart(float timestamp)
        {
            this.isRunning = true;
        }

        protected override void OnPlaybackStop(float timestamp)
        {
            this.isRunning = false;
        }

        protected override void OnUnload()
        {
            this.isRunning = false;
            this.Reset();
        }
    }
}

