using PulsarModLoader;

namespace EmergencyBeamOut
{
    public class Mod : PulsarMod
    {
        public override string Version => "0.0.2";

        public override string Author => "18107";

        public override string ShortDescription => "Teleports players off exploding ships";

        public override string Name => "Emergency Beam Out";

        public override string ModID => "emergencybeamout";

        public override string HarmonyIdentifier()
        {
            return "id107.emergencybeamout";
        }
    }
}
