using Modding;
using System;
using System.Linq;
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

        public override string DescTwo => "Littering is bad. You'll be charged for all uncollected geo.";

        public override string Scene => "GG_Waterways";

        public override float X => 61.0f;

        public override float Y => 12.4f;

        public override PreceptSettings Settings(SaveSettings s) => s.Precept9;

        private int tax;

        public override void Hook()
        {
            ModHooks.BeforeSceneLoadHook += CalculateLitterTax;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += ApplyLitterTax;
        }

        private int numActiveGeo(string geoObjName)
        {
            GameObject[] geos = Resources.FindObjectsOfTypeAll<GameObject>()
                    .Where(obj => obj.name.StartsWith(geoObjName))
                    .Where(geo => geo.activeSelf)
                    .ToArray<GameObject>();
            return geos.Length;
        }

        private string CalculateLitterTax(string new_scene)
        {
            if (Equipped())
            {
                int numSmallGeos = numActiveGeo("Geo Small(Clone)");
                int numMediumGeos = numActiveGeo("Geo Med(Clone)");
                int numLargeGeos = numActiveGeo("Geo Large(Clone)");
                tax = numSmallGeos + 5 * numMediumGeos + 25 * numLargeGeos;
                Log($"LITTER TAX: {tax}");
                
            }
            return new_scene;
        }

        private void ApplyLitterTax(Scene prevScene, Scene newScene)
        {
            if (Equipped() && tax > 0)
            {
                int player_geo = PlayerData.instance.GetInt("geo");
                ItemChanger.GeoCost cost = new(tax < player_geo ? tax : player_geo);
                cost.OnPay();
            }
        }
    }
}
