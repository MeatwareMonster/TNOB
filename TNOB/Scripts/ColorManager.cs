using System.Collections.Generic;
using UnityEngine;

namespace TNOB.Scripts
{
    public class ColorManager
    {
        private Color[] RGBColors = new Color[] {
            new Color(0.5f, 0, 0),
            new Color(0, 0.5f, 0),
            new Color(0, 0, 0.5f),
        };
        private Color TastefulGray = new Color(0.33f, 0.34f, 0.35f);
        private Color BaseColor = new Color(1f, 0.71f, 0.63f);

        private int CurrentIndex = 0;
        private int NextIndex = 1;
        public float ChangeColourTime = 2.0f;
        private float Timer = 0.0f;
        //private float lastChange = 0.0f;

        private List<Renderer> Renderers;

        public ColorManager(List<Renderer> renderers)
        {
            Renderers = renderers;

            Mod.EnableTastefulGrayColor.SettingChanged += (sender, args) => UpdateColors();
            Mod.EnableCustomColor.SettingChanged += (sender, args) => UpdateColors();
            Mod.Red.SettingChanged += (sender, args) => UpdateColors();
            Mod.Green.SettingChanged += (sender, args) => UpdateColors();
            Mod.Blue.SettingChanged += (sender, args) => UpdateColors();
            Mod.EnableRGB.SettingChanged += (sender, args) => UpdateColors();

            UpdateColors();
        }

        private void UpdateColors()
        {
            if (Mod.EnableRGB.Value) return;

            if (Mod.EnableCustomColor.Value)
            {
                var customColor = new Color(Mod.Red.Value, Mod.Green.Value, Mod.Blue.Value);
                SetRendererColors(customColor);
            }
            else if (Mod.EnableTastefulGrayColor.Value)
            {
                SetRendererColors(TastefulGray);
            }
            else
            {
                SetRendererColors(BaseColor);
            }
        }

        private void SetRendererColors(Color color)
        {
            foreach (var renderer in Renderers)
            {
                renderer.material.color = color;
            }
        }

        public void UpdateRGB()
        {
            Timer += Time.deltaTime;

            if (Timer > ChangeColourTime)
            {
                CurrentIndex = (CurrentIndex + 1) % RGBColors.Length;
                NextIndex = (CurrentIndex + 1) % RGBColors.Length;
                Timer = 0.0f;
            }

            foreach (var renderer in Renderers)
            {
                renderer.material.color = Color.Lerp(RGBColors[CurrentIndex], RGBColors[NextIndex], Timer / ChangeColourTime);
            }
        }
    }
}
