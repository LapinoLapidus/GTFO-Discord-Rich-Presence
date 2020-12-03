using Player;

namespace GTFO_DRP
{
    public static class Utility
    {
        public static bool IsInExpedition() => PlayerManager.GetLocalPlayerAgent() != null;
    }
}