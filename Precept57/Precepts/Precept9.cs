using Modding;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Precept57
{
    internal class Precept9 : Precept
    {

        public static readonly Precept9 Instance = new();

        private Precept9() { }

        public override string SpriteFileName => "Precept9.png";

        public override string Name => "Precept 9";

        public override string Id => "Precept_9";

        public override string ShopDescription => "Get charged for all uncollected geo.";

        public override string Take => "Following";

        public override string Press => "Tap A";

        public override string DescOne => "Keep Your Home Tidy";

        public override string DescTwo => "Littering is bad. Always clean up after yourself.";

        public override string Scene => "Tutorial_01";

        public override float X => 45.5f;

        public override float Y => 11.4f;

        public override PreceptSettings Settings(SaveSettings s) => s.Precept9;

        public override void Hook()
        {
            ModHooks.BeforeSceneLoadHook += ApplyLitterTax;
        }

        private string ApplyLitterTax(string new_scene)
        {
            Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            if (Equipped())
            {
                Log($"Scene: {scene.name}");
                GameObject[] objs = scene.GetRootGameObjects();
                foreach (GameObject obj in objs)
                {
                    Log($"Obj: {obj.name}");
                }
            }
            return new_scene;
        }
    }
}
