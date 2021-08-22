// TNOB
// Truck nuts on boats
// 
// File:    TNOB.cs
// Project: TNOB

using System.Collections.Generic;
using System.IO;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using Jotunn.Managers;
using Jotunn.Utils;
using TNOB.Scripts;
using UnityEngine;

namespace TNOB
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    //[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class Mod : BaseUnityPlugin
    {
        public const string PluginGUID = "com.meatwaremonster.TNOB";
        public const string PluginName = "TNOB";
        public const string PluginVersion = "1.0.0";

        public static string ModLocation = Path.GetDirectoryName(typeof(Mod).Assembly.Location);

        private readonly Harmony harmony = new Harmony(PluginGUID);

        private AssetBundle _embeddedResourceBundle;

        public static ConfigEntry<bool> EnableMenuLogo;
        public static ConfigEntry<bool> EnableRGB;

        private void Awake()
        {
            EnableMenuLogo = Config.Bind("TNOB", "Menu Logo", false, new ConfigDescription("Enable new menu logo"));
            EnableRGB = Config.Bind("TNOB", "RGB", false, new ConfigDescription("Enable RGB color cycling"));

            LoadAssetBundle();

            PrefabManager.OnPrefabsRegistered += () =>
            {
                var vikingShip = PrefabManager.Instance.GetPrefab("VikingShip");

                var vikingShipNuts = _embeddedResourceBundle.LoadAsset<GameObject>("Assets/CustomItems/VikingShipNuts.prefab");
                vikingShipNuts.transform.localScale += new Vector3(7, 7, 7);
                vikingShipNuts.transform.position = vikingShip.transform.position + new Vector3(0.2f, 2.2f, -11);
                vikingShipNuts.transform.parent = vikingShip.transform;
                var vikingShipRigidbody = vikingShipNuts.GetComponent<Rigidbody>();

                var vikingShipJoint = vikingShip.AddComponent<CharacterJoint>();
                vikingShipJoint.anchor = new Vector3(0.2f, 3.2f, -11);
                vikingShipJoint.connectedBody = vikingShipRigidbody;
                vikingShipJoint.enableCollision = false;

                var karve = PrefabManager.Instance.GetPrefab("Karve");

                var karveNuts = _embeddedResourceBundle.LoadAsset<GameObject>("Assets/CustomItems/KarveNuts.prefab");
                karveNuts.transform.localScale += new Vector3(5, 5, 5);
                karveNuts.transform.position = karve.transform.position + new Vector3(0.2f, 1.4f, -5);
                karveNuts.transform.parent = karve.transform;
                var karveRigidbody = karveNuts.GetComponent<Rigidbody>();

                var karveJoint = karve.AddComponent<CharacterJoint>();
                karveJoint.anchor = new Vector3(0.2f, 2.5f, -5);
                karveJoint.connectedBody = karveRigidbody;
                karveJoint.enableCollision = false;

                if (EnableRGB.Value)
                {
                    var game = Game.m_instance;
                    var rgb = (RGB)game.gameObject.AddComponent(typeof(RGB));
                    rgb.Renderers = new List<Renderer>
                    {
                        vikingShipNuts.GetComponent<Renderer>(),
                        karveNuts.GetComponent<Renderer>()
                    };
                }

                UnloadAssetBundle();
            };

            harmony.PatchAll();
        }

        private void LoadAssetBundle()
        {
            // Load asset bundle from embedded resources
            Jotunn.Logger.LogInfo($"Embedded resources: {string.Join(",", typeof(Mod).Assembly.GetManifestResourceNames())}");
            _embeddedResourceBundle = AssetUtils.LoadAssetBundleFromResources("boatnuts", typeof(Mod).Assembly);
        }

        private void UnloadAssetBundle()
        {
            _embeddedResourceBundle.Unload(false);
        }

        //#if DEBUG
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F6))
            { // Set a breakpoint here to break on F6 key press
            }
        }
        //#endif
    }
}