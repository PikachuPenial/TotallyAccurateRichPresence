using System;
using System.Collections;
using BepInEx;
using UnityEngine;
using UnityEngine.SceneManagement;
using TARP.Utilities;

namespace TARP
{
    [BepInPlugin("com.penial.totallyaccuraterichpresence", "Totally Accurate Rich Presence", "1.0.0")]
    [BepInProcess("TotallyAccurateBattlegrounds.exe")]

    public class TARP : BaseUnityPlugin
    {
        public const string version = "1.0";
        internal static DiscordRPC.RichPresence presence;
        private float presenceCheckInterval;
        private float presenceUpdateCooldown;
        private Scene curScene;
        private string curSceneName;
        private string prevSceneName;

        public const bool allowDebug = true;

        private void Awake()
        {
            presenceCheckInterval = 5;
            presenceUpdateCooldown = presenceCheckInterval;

            DiscordRPC.CallBacks eventHandlers = default;
            eventHandlers.readyCallback = (DiscordRPC.ReadyCallback)Delegate.Combine(eventHandlers.readyCallback, new DiscordRPC.ReadyCallback(ReadyCallback));
            eventHandlers.disconnectedCallback = (DiscordRPC.DisconnectedCallback)Delegate.Combine(eventHandlers.disconnectedCallback, new DiscordRPC.DisconnectedCallback(DisconnectedCallback));
            eventHandlers.errorCallback = (DiscordRPC.ErrorCallback)Delegate.Combine(eventHandlers.errorCallback, new DiscordRPC.ErrorCallback(ErrorCallback));

            DiscordRPC.Initialize("1152021847400005722", ref eventHandlers, true, "823130");
            presence = default;
            
            curScene = SceneManager.GetActiveScene();
            UpdatePresence("Browsing the Main Menu", "mainmenuimage");
            TARPDebug.Log("Rich Presence was switched to 'Browsing the Main Menu'");
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
                CheckStatus();
            }
        }

        void CheckStatus()
        {
            Scene curScene = SceneManager.GetActiveScene();
            curSceneName = curScene.name;

            if (curSceneName == prevSceneName)
            {
                return;
            }

            switch(curScene.name)
            {
                case "MainMenu":
                    prevSceneName = curSceneName;
                    UpdatePresence("Browsing the Main Menu", "mainmenuimage");
                    TARPDebug.Log("Rich Presence was switched to 'Browsing the Main Menu'");
                    break;
                case "WilhelmTest":
                    prevSceneName = curSceneName;
                    UpdatePresence("In the Shooting Range", "shootingrangeimage");
                    TARPDebug.Log("Rich Presence was switched to 'In the Shooting Range'");
                    break;
                case "MainWorld_Base":
                    System.Random rndMatchImg = new System.Random();
                    prevSceneName = curSceneName;
                    UpdatePresence("In a match", "matchimage" + rndMatchImg.Next(1, 5));
                    TARPDebug.Log("Rich Presence was switched to 'In a match'");
                    break;
                default:
                    prevSceneName = curSceneName;
                    UpdatePresence("Browsing the Main Menu", "mainmenuimage");
                    TARPDebug.Log("No compatible Scene was detected, setting scene to Main Menu as a failsafe");
                    break;
            }
        }

        void UpdatePresence(string detail, string key)
        {
            long timestamp = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
            presence.details = detail;
            presence.startTimestamp = timestamp;
            presence.largeImageKey = key;
            presence.largeImageText = "TARP " + version + ", created by Penial";
            DiscordRPC.UpdatePresence(ref presence);
        }
    }
}