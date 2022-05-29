using Modding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace Precept57
{
    public class Precept57 : Mod
    {
        internal static Precept57 Instance;

        //public override List<ValueTuple<string, string>> GetPreloadNames()
        //{
        //    return new List<ValueTuple<string, string>>
        //    {
        //        new ValueTuple<string, string>("White_Palace_18", "White Palace Fly")
        //    };
        //}

        //public Precept57() : base("Precept57")
        //{
        //    Instance = this;
        //}

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects) {
            Log("Initializing");

            Instance = this;

            Log("Initialized");
        }
    }
}