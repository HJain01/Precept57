using Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Precept57
{
    internal class Precept2 : Precept
    {

        public static readonly Precept2 Instance = new();

        private Precept2() { }

        public override string SpriteFileName => "Precept9.png";

        public override string Name => "Precept 2";

        public override string Id => "Precept_2";

        public override string ShopDescription => "";

        public override string Take => "Following";

        public override string Press => "Tap A";

        public override string DescOne => "Never Let Them Laugh at You";

        public override string DescTwo => "Fools laugh at everything, even their superiors, even you";

        public override string Scene => "Tutorial_01";

        public override float X => 47.4f;

        public override float Y => 11.4f;

        public override PreceptSettings Settings(SaveSettings s) => s.Precept9;

        //private GameObject _precept2;
        private AudioSource _laugh;
        private AudioClip _laughClip;
        private bool _loaded;

        public override void Hook()
        {
            CreateHooks(0);
            ModHooks.SavegameLoadHook += CreateHooks;
        }

        private void CreateHooks(int obj)
        {
            LoadAudio();
            ModHooks.TakeDamageHook += LaughAtYouHazard;
            ModHooks.AfterPlayerDeadHook += () => { _laugh.Play(); }; 
        }

        private void LoadAudio()
        {
            if (_loaded) return;
            _laughClip = Resources.Load<AudioClip>("Audio/crowd_laugh.wav");
            _laugh = HeroController.instance.gameObject.AddComponent<AudioSource>();
            _laugh.clip = _laughClip;
            _loaded = true;
        }

        private int LaughAtYouHazard(ref int hazard, int damage)
        {
            if (hazard > 0) _laugh.Play();
            Log("Took hazard damage");
            return damage;
        }

    }
}
