using HarmonyLib;

namespace EmergencyBeamOut
{
    [HarmonyPatch(typeof(PLShipInfo), "AboutToBeDestroyed")]
    internal class ShipInfo
    {
        static void Prefix(PLShipInfo __instance)
        {
            //If the exploding ship is the player ship, there is nowhere to teleport to
            if (__instance.GetIsPlayerShip() || __instance.MyTLI == null || PLEncounterManager.Instance.PlayerShip == null)
                return;


            PLPlayer localPlayer = PLNetworkManager.Instance.LocalPlayer;
            int playerShipTeleporterID = PLEncounterManager.Instance.PlayerShip.MyTLI.SubHubID;
            int explodingTeleporterID = __instance.MyTLI.SubHubID;
            PLInterior interior = PLEncounterManager.Instance.PlayerShip.MyTLI.AllTTIs[0].MyInterior;
            int playerShipinteriorID = interior != null ? interior.InteriorID : -1;

            //if the local player is the host
            if (PhotonNetwork.isMasterClient)
            {
                foreach (PLPlayer player in PLServer.Instance.AllPlayers)
                {
                    //if player is alive and on the exploding ship
                    if (player != null && player.TeamID == 0 && player.GetPawn() != null && player.SubHubID == explodingTeleporterID && player != localPlayer)
                    {
                        Teleport(player, playerShipTeleporterID, playerShipinteriorID);
                        PulsarModLoader.Utilities.Messaging.Notification("Emergency Beam Out", player);
                    }
                }
            }

            //if local player is on exploding ship
            if (localPlayer.SubHubID == explodingTeleporterID)
            {
                Teleport(localPlayer, playerShipTeleporterID, playerShipinteriorID);
            }
        }

        private static void Teleport(PLPlayer player, int playerShipTeleporterID, int interiorID)
        {
            //Teleport
            player.photonView.RPC("NetworkTeleportToSubHub", PhotonTargets.All, new object[]
            {
                playerShipTeleporterID,
                0
            });

            //Set interior
            player.photonView.RPC("SetInterior", PhotonTargets.All, new object[]
            {
                interiorID
            });
        }
    }
}
