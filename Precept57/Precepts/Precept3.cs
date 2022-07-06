using Modding;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Precept57
{
    internal class Precept3 : Precept
    {

        public static readonly Precept3 Instance = new();

        private Precept3() { }

        public override string SpriteFileName => "Precept9.png";

        public override string Name => "Precept 3";

        public override string Id => "Precept_3";

        public override string ShopDescription => "You must rest at each bench you pass.";

        public override string Take => "Following";

        public override string Press => "Tap A";

        public override string DescOne => "Always Be Rested";

        public override string DescTwo => "Fighting and adventuring take their toll on your body.";

        public override string Scene => "Tutorial_01";

        public override float X => 47.4f;

        public override float Y => 11.4f;

        public override PreceptSettings Settings(SaveSettings s) => s.Precept3;


        public override void Hook()
        {
            ModHooks.SceneChanged += UpdateBench;
        }

        private static void UpdateBench(string obj)
        {
            var bench = GameObject.Find("_Props/RestBench");
            if (bench == null) return;
            var hitbox = bench.GetComponent<BoxCollider2D>();
            hitbox.size = new Vector2(10f, 1f);
        }

    }
}
