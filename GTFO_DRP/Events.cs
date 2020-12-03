using System;
using SNetwork;

namespace GTFO_DRP
{
    public static class Events
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="secret">SNet Lobby Identifier provided by Discord</param>
        public static void OnActivityJoin(string secret)
        {
            if (SNet.IsInLobby)
            {
                SNet.Lobbies.LeaveLobby();
            }
            SNet.Lobbies.JoinLobby(new SNet_LobbyIdentifier(Convert.ToUInt64(secret)));

        }
    }
}