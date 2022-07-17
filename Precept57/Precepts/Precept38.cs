using GlobalEnums;
using Modding;
using UnityEngine;

namespace Precept57
{ 
    internal class Precept38 : Precept
    { 
        public static readonly Precept38 Instance = new();

        private Precept38() {}

        public override string SpriteFileName => "Precept35.png";
        public override string Name => "Precept 38";
        public override string Id => "Precept_38";
        public override string ShopDescription => "If you spend too long in the air, the force will crush you against the ground and destroy you. Beware!";
        public override string Take => "Following";
        public override string Press => "Tap A";
        public override string DescOne => "Beware the Mysterious Force";

        public override string DescTwo => "You now take damage after significant falls.";
        // after Soul Master fight (& getting Desolate Dive)
        public override string Scene => "Ruins1_32";
        public override float X => 37.2f;
        public override float Y => 65.5f;

        public override PreceptSettings Settings(SaveSettings s) => s.Precept38;

        /*
         * Hard landings happen when fall timer > 1.1f (BIG_FALL_TIME).
         * Some hard landings are forced (scene transitions) and are ignored
         * if they are less than the fall timer limit. Falling from the top of
         * the elevator shaft from Resting Grounds to City of Tears ~ 8.28
         * fall time. Damage is scaled to take 8 masks of damage (1-max) after
         * a fall time of 7
         * Equation: https://www.desmos.com/calculator/uvygbscjot
         */
        private float FALL_DAMAGE_MIN_TIME = 1.1f;
        private float _fallTimer;

        public override void Hook()
        {
            ModHooks.HeroUpdateHook += FallDamage;
        }

        private void FallDamage()
        {
            if (HeroController.instance.fallTimer == 0f && _fallTimer > 0
                && HeroController.instance.hero_state != ActorStates.airborne
                && !HeroController.instance.cState.transitioning
                && !HeroController.instance.cState.bouncing
                && !HeroController.instance.cState.shroomBouncing
                && !HeroController.instance.cState.spellQuake)
            {
                if (_fallTimer < FALL_DAMAGE_MIN_TIME)
                    return;
                var damage = Mathf.FloorToInt(5.5f * Mathf.Sqrt(0.5f * _fallTimer - 0.25f) - 1.9f);
                if (damage >= PlayerData.instance.health + PlayerData.instance.healthBlue)
                    damage = PlayerData.instance.health + PlayerData.instance.healthBlue - 1;
                HeroController.instance.TakeDamage(null, GlobalEnums.CollisionSide.bottom, damage, 1);
                LogDebug("Fell for " + _fallTimer + "s & took " + damage + " masks");
                _fallTimer = 0.0f;
            }

            _fallTimer = HeroController.instance.fallTimer;
        }
    } 
}
