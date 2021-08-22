using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace TNOB.Patches
{
    class FejdStartup_Patch
    {
        [HarmonyPatch(typeof(FejdStartup), nameof(FejdStartup.Awake))]
        class FejdStartup_Awake_Patch
        {
            private static void Postfix(FejdStartup __instance)
            {
                if (!Mod.EnableMenuLogo.Value) return;

                var path = $"{Mod.ModLocation}/Assets/Logo.png";
                byte[] bytes = System.IO.File.ReadAllBytes(path);
                Texture2D texture = new Texture2D(1, 1);
                texture.LoadImage(bytes);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                var mainMenu = __instance.m_mainMenu;
                var logo = mainMenu.transform.Find("LOGO");
                var logoImage = logo.GetComponent<Image>();
                logoImage.sprite = sprite;
            }
        }
    }
}
