using System;
using BepInEx.Logging;
using UnityEngine;
using Discord;
using GameData;
using SNetwork;
using Logger = BepInEx.Logging.Logger;

namespace GTFO_DRP
{
    public class RichPresence : MonoBehaviour
    {
        private static Discord.Discord _discord;
        private static ActivityManager _activityManager;
        public static ManualLogSource log;

        private static pActiveExpedition expPackage;

        private static DateTime _currentTime;

        public RichPresence(IntPtr intPtr) : base(intPtr)
        {
            log = Logger.CreateLogSource("Discord Rich Presence");
            _discord = new Discord.Discord(764433332330037249L, 0UL);
            _activityManager = _discord.GetActivityManager();
            // TODO: ?
            _activityManager.RegisterCommand("gtfo://run");
            _activityManager.RegisterSteam(493520);

            _currentTime = _currentTime.AddSeconds(10);

            _activityManager.OnActivityJoin += Events.OnActivityJoin;
            log.LogMessage("RichPresence created.");

            Activity activity = new Activity()
            {
                State = "Playing GTFO",
                Details = "Selecting an expedition."
            };

            SetActivity(activity);
        }

        private void Update()
        {
            _discord.RunCallbacks();
            if (_currentTime <= DateTime.Now && SNet.IsInLobby)
            {
                expPackage = RundownManager.GetActiveExpeditionData();

                SetActivity(GetActivity());
                // Recheck status every 10 seconds.
                _currentTime = DateTime.Now.AddSeconds(10);
            }
        }

        public static void SetActivity(Activity activity)
        {
            _activityManager.UpdateActivity(activity, (result =>
            {
                if (result == Result.Ok)
                    log.LogInfo("Success");
                else
                    log.LogInfo("Failed: " + result);
            }));
        }

        public static Activity GetActivity()
        {
            return new Activity()
            {
                State = "Playing GTFO",
                // TODO: Option to use name instead of RxYz
                // TODO: Get rundown version dynamically
                // TODO: Fix "In lobby" / "Somewhere in the darkness"
                Details = (Utility.IsInExpedition() ? "In lobby: " : "Somewhere in the darkness: ") + "R4" +
                          expPackage.tier.ToString().Replace("Tier", "") +
                          (expPackage.expeditionIndex + 1),
                Party =
                {
                    Id = "abcd",
                    Size =
                    {
                        CurrentSize = SNet.LobbyPlayers.Count,
                        MaxSize = SNet.LobbyPlayers.Capacity
                    }
                },
                Secrets =
                {
                    // TODO: Probably change this to be like a GUID or something
                    Match = "abcdefghijklmnopqrstuvwxyz",
                    Join = SNet.Lobby.Identifier.ID.ToString(),
                    Spectate = "null"
                }
            };
        }
    }
}