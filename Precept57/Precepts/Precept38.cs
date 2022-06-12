using Modding;

namespace Precept57
{ 
    internal class Precept38 : Precept
    { 
        public static readonly Precept38 Instance = new();

        private Precept38() {}

        public override string SpriteFileName => "Precept35.png";
        public override string Name => "Precept 38";
        public override string Id => "Precept_38";
        public override string ShopDescription => "A mysterious force bears down on us from above, pushing us downwards. If you spend too long in the air, the force will crush you against the ground and destroy you. Beware!";
        public override string Take => "Following";
        public override string Press => "Tap A";
        public override string DescOne => "Beware the Mysterious Force";

        public override string DescTwo => "You now take damage after significant falls.";
        public override string Scene => "Tutorial_01";
        public override float X => 45.5f;
        public override float Y => 11.4f;

        public override PreceptSettings Settings(SaveSettings s) => s.Precept38;

        private float FALL_DAMAGE_MIN_TIME = 0.0f;
        private float FALL_DAMAGE_MAX_TIME = 0.0f;
        private float _maxFallTime = 0.0f;
        private bool _startFall = false;

        public override void Hook()
        {
            ModHooks.HeroUpdateHook += FallDamage;
        }

        private void FallDamage()
        {
            if (Equipped())
            { 
                if (HeroController.instance.cState.falling && HeroController.instance.cState.willHardLand)
                {
                    if (!_startFall)
                        _startFall = true;
                    if (HeroController.instance.fallTimer > _maxFallTime)
                    {
                        _maxFallTime = HeroController.instance.fallTimer;
                    }
                }
                else if (_startFall) 
                {
                    Log("Fell for " + _maxFallTime + " time");
                    _startFall = false;
                    _maxFallTime = 0.0f;
                }
            }
        }
    } 
}
