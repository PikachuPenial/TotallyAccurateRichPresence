using System;
using System.Collections;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using TARP.Utilities;

namespace TARP
{
    [BepInPlugin("com.penial.totallyaccuraterichpresence", "Totally Accurate Rich Presence", "1.1.0")]
    [BepInProcess("TotallyAccurateBattlegrounds.exe")]
    public class TARP : BaseUnityPlugin
    {
        public const string version = "1.1";
        internal static DiscordRPC.RichPresence presence;
        private float presenceCheckInterval;
        private float presenceUpdateCooldown;
        private long previousTimestamp;

        private string sceneActive;
        private string sceneOnPresence;

        public int level;
        public int xp;
        public int kills;
        public int alive;

        public const bool allowLogging = true;

        private void Awake()
        {
            var harmony = new Harmony("com.penial.totallyaccuraterichpresence");
            harmony.PatchAll();

            presenceCheckInterval = 5;
            presenceUpdateCooldown = presenceCheckInterval;

            DiscordRPC.CallBacks eventHandlers = default;
            eventHandlers.readyCallback = (DiscordRPC.ReadyCallback)Delegate.Combine(eventHandlers.readyCallback, new DiscordRPC.ReadyCallback(ReadyCallback));
            eventHandlers.disconnectedCallback = (DiscordRPC.DisconnectedCallback)Delegate.Combine(eventHandlers.disconnectedCallback, new DiscordRPC.DisconnectedCallback(DisconnectedCallback));
            eventHandlers.errorCallback = (DiscordRPC.ErrorCallback)Delegate.Combine(eventHandlers.errorCallback, new DiscordRPC.ErrorCallback(ErrorCallback));

            DiscordRPC.Initialize("1152021847400005722", ref eventHandlers, true, "823130");
            presence = default;
            //DiscordRPC.UpdatePresence(ref presence);
        }

        private static void ErrorCallback(int errorCode, string message)
        {
            TARPDebug.Log($"RPErrorCallback: {errorCode}: {message}");
        }

        private static void DisconnectedCallback(int errorCode, string message)
        {
            TARPDebug.Log($"RPDisconnectedCallback: {errorCode}: {message}");
        }

        private static void ReadyCallback()
        {
            TARPDebug.Log("Discord Rich Presence has been successfully initialized");
        }

        void Update()
        {
            if (presenceUpdateCooldown > 0)
            {
                presenceUpdateCooldown -= Time.deltaTime;
            }
            else
            {
                presenceUpdateCooldown = presenceCheckInterval;
                UpdatePresence();
            }
        }

        void UpdatePresence()
        {
            Scene scene = SceneManager.GetActiveScene();
            sceneActive = scene.name;

            switch (sceneActive)
            {
                case "MainMenu": // Main Menu
                    level = LevelProgressionUI.PLAYER_LEVEL;
                    xp = LevelProgressionUI.PLAYER_XP;
                    presence.details = "Browsing the Main Menu";
                    presence.state = "LVL." + level + " (" + xp + " XP)";
                    presence.largeImageKey = "mainmenuimage";
                    break;
                case "MainWorld_Base": // In Game
                    presence.details = "In a match";
                    presence.state = "99 kills | 99 alive";
                    presence.largeImageKey = "matchimage1";
                    break;
                case "WilhelmTest": // Shooting Range
                    presence.details = "In the Shooting Range";
                    presence.state = "99 dummies killed";
                    presence.largeImageKey = "shootingrangeimage";
                    break;
                default: // Failsafe if a unsupported scene is loaded
                    presence.details = "Browsing the Main Menu";
                    presence.state = "LVL." + level + " (" + xp + " XP)";
                    presence.largeImageKey = "mainmenuimage";
                    break;
            }

            long timestamp = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
            sceneOnPresence = sceneActive;

            presence.largeImageText = "TARP, created by Penial";
            presence.startTimestamp = timestamp;
            DiscordRPC.UpdatePresence(ref presence);
        }
    }
}