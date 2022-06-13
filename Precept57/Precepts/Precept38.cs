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
        public override string Scene => "Tutorial_01";
        public override float X => 45.5f;
        public override float Y => 11.4f;

        public override PreceptSettings Settings(SaveSettings s) => s.Precept38;

        /*
         * Hard landings happen when fall timer > 1.1f (BIG_FALL_TIME).
         * Some hard landings are forced (scene transitions) and are ignored
         * if they are less than the fall timer limit. Falling from the top of
         * the elevator shaft from Resting Grounds to City of Tears ~ 8.28
         * fall time. Damage is scaled to take 8 masks of damage (1-max) after
         * a fall time of 7.5
         */
        private float FALL_DAMAGE_MIN_TIME = 1.1f;
        private float FALL_DAMAGE_MAX_TIME = 7.5f;
        private int MAX_MASK_DAMAGE = 8;
        private float _fallTimer;

        public override void Hook()
        {
            ModHooks.HeroUpdateHook += FallDamage;
        }

        private void FallDamage()
        {
            if (!Equipped()) return;
            if (HeroController.instance.fallTimer == 0f && _fallTimer > 0
                && HeroController.instance.hero_state != ActorStates.airborne
                && !HeroController.instance.cState.transitioning
                && !HeroController.instance.cState.bouncing
                && !HeroController.instance.cState.shroomBouncing
                && !HeroController.instance.cState.spellQuake)
            {
                if (_fallTimer < FALL_DAMAGE_MIN_TIME)
                    return;
                var damage =
                    Mathf.FloorToInt(((MAX_MASK_DAMAGE - 1) / (FALL_DAMAGE_MAX_TIME - FALL_DAMAGE_MIN_TIME)) * 
                        (_fallTimer - FALL_DAMAGE_MAX_TIME) + MAX_MASK_DAMAGE);
                if (damage >= PlayerData.instance.health)
                    damage = PlayerData.instance.health - 1;
                HeroController.instance.TakeDamage(null, GlobalEnums.CollisionSide.bottom, damage, 1);
                Log("Fell for " + _fallTimer + "t & took " + damage + " masks");
                _fallTimer = 0.0f;
            }

            _fallTimer = HeroController.instance.fallTimer;
        }
    } 
}
