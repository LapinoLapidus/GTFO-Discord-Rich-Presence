using System;
using BepInEx;
using BepInEx.IL2CPP;
using CellMenu;
using HarmonyLib;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace GTFO_DRP
{
    [BepInPlugin("ovh.lapinolapidus.gtfo_drp", "Discord Rich Presence", "0.0.1")]
    public class BepInExLoader : BasePlugin
    {
        public override void Load()
        {
            Harmony harmony = new Harmony("ovh.lapinolapidus.gtfo_drp");
            harmony.PatchAll();
            ClassInjector.RegisterTypeInIl2Cpp<RichPresence>();
        }
    }

    [HarmonyPatch(typeof(CM_PageRundown_New), "Setup")]
    public class CM_PageRundown_Setup
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            var go = new GameObject();
            go.AddComponent<RichPresence>();
            UnityEngine.Object.DontDestroyOnLoad(go);
        }
    }
}