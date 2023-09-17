using System;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TARP
{
    [BepInPlugin("com.penial.totallyaccuraterichpresence", "Totally Accurate Rich Presence", "1.1.0")]
    [BepInProcess("TotallyAccurateBattlegrounds.exe")]
    public class TARP : BaseUnityPlugin
    {
        internal static DiscordRPC.RichPresence presence;
        private Scene scene;
        private float presenceCheckInterval;
        private float presenceUpdateCooldown;
        private long timestamp;
        private long previousTimestamp;

        private string sceneActive;
        private string sceneOnPresence;

        public int level;
        public int xp;
        //public int kills;
        //public int alive;

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
        }

        private void ErrorCallback(int errorCode, string message)
        {
            Logger.LogInfo($"RPErrorCallback: {errorCode}: {message}");
        }

        private void DisconnectedCallback(int errorCode, string message)
        {
            Logger.LogInfo($"RPDisconnectedCallback: {errorCode}: {message}");
        }

        private void ReadyCallback()
        {
            Logger.LogInfo("Discord Rich Presence has been successfully initialized");
        }

        void Update()
        {
            if (presenceUpdateCooldown > 0)
            {
                presenceUpdateCooldown -= Time.unscaledDeltaTime;
            }
            else
            {
                presenceUpdateCooldown = presenceCheckInterval;
                UpdatePresence();
            }
        }

        void UpdatePresence()
        {
            scene = SceneManager.GetActiveScene();
            sceneActive = scene.name;

            switch (sceneActive)
            {
                case "MainMenu": // Main Menu
                    level = LevelProgressionUI.PLAYER_LEVEL;
                    xp = Mathf.RoundToInt((float)LevelProgressionUI.PLAYER_XP / (float)LevelProgressionUI.XPNeededAtLevel(level + 1)* 100);
                    presence.details = "Browsing the Main Menu";
                    presence.state = "";
                    presence.largeImageKey = "mainmenuimage";
                    break;
                case "MainWorld_Base": // In Game
                    presence.details = "In a match";
                    presence.state = "";
                    presence.largeImageKey = "matchimage1";
                    break;
                case "WilhelmTest": // Shooting Range
                    presence.details = "In the Shooting Range";
                    presence.state = "";
                    presence.largeImageKey = "shootingrangeimage";
                    break;
                default: // Failsafe if a unsupported scene is loaded
                    presence.details = "Browsing the Main Menu";
                    presence.state = "";
                    presence.largeImageKey = "mainmenuimage";
                    break;
            }
            presence.largeImageText = "LVL." + level + " (" + xp + "%)";

            if (sceneOnPresence != sceneActive)
            {
                timestamp = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
                presence.startTimestamp = timestamp;
            }

            DiscordRPC.UpdatePresence(ref presence);
            sceneOnPresence = sceneActive;
            previousTimestamp = timestamp;
        }
    }
}