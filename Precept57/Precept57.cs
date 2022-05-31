﻿using Modding;
using System;
using System.Collections.Generic;
using UnityEngine;
using UObject = UnityEngine.Object;
using ItemChanger;
using ItemChanger.Items;
using ItemChanger.Locations;
using ItemChanger.Tags;
using ItemChanger.UIDefs;
using SFCore;

namespace Precept57
{
    public class Precept57 : Mod, ILocalSettings<SaveSettings>
    {
        private static readonly List<Precept> Precepts = new()
        {
            Precept35.Instance
        };
        
        private static Precept57 Instance;

        private Dictionary<string, Func<bool, bool>> BoolGetters = new();
        private Dictionary<string, Action<bool>> BoolSetters = new();

        public Precept57() : base("Precept 57")
        {
            // Instance = this;
        }
        
        public override string GetVersion() => "0.0.1";

        private SaveSettings saveSettings = new();
        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects) {
            Log("Initializing Precept 57");

            Instance = this;

            foreach (var precept in Precepts)
            {
                var sprite = EmbeddedSprites.Get(precept.Sprite);
                ItemHelper.AddNormalItem(sprite, "false", precept.Id, precept.Description);

                Func<SaveSettings, PreceptSettings> settings = precept.Settings;
                BoolGetters[precept.Id] = _ => settings(saveSettings).Got;
                BoolSetters[precept.Id] = value => settings(saveSettings).Got = value;

                precept.Hook();
                
                var item = new BoolItem() {
                    fieldName = precept.Id,
                    name = precept.Id,
                    UIDef = new MsgUIDef() { 
                        name = new LanguageString("UI", precept.Id),
                        shopDesc = new LanguageString("UI", precept.Description),
                        sprite = new EmbeddedSprite() { key = precept.Sprite }
                    }
                };
                
                var mapmodTag = item.AddTag<InteropTag>();
                mapmodTag.Message = "PreceptSupplementalMetadata";
                mapmodTag.Properties["ModSource"] = GetName();
                mapmodTag.Properties["PoolGroup"] = "Items";
                Finder.DefineCustomItem(item);
            }
            ModHooks.GetPlayerBoolHook += GetPreceptBools;
            ModHooks.SetPlayerBoolHook += SetPreceptBools;
            ModHooks.NewGameHook += PlacePreceptsAtFixedPositions;

            Log("Initialized Precept 57");
        }
        
        private bool GetPreceptBools(string name, bool orig)
        {
            if (BoolGetters.TryGetValue(name, out var f))
            {
                return f(orig);
            }
            return orig;
        }
        
        private bool SetPreceptBools(string name, bool orig)
        {
            if (BoolSetters.TryGetValue(name, out var f))
            {
                f(orig);
            }
            return orig;
        }

        private void PlacePreceptsAtFixedPositions()
        {
            ItemChangerMod.CreateSettingsProfile(overwrite: false, createDefaultModules: false);

            var placements = new List<AbstractPlacement>();
            foreach (var precept in Precepts)
            {
                var name = precept.Id;
                placements.Add(
                    new CoordinateLocation() { x = precept.X, y = precept.Y, elevation = 0, sceneName = precept.Scene, name = name }
                        .Wrap()
                        .Add(Finder.GetItem(name)));
            }
            ItemChangerMod.AddPlacements(placements, conflictResolution: PlacementConflictResolution.Ignore);
        }
        
        public void OnLoadLocal(SaveSettings s)
        {
            saveSettings = s;
        }

        public SaveSettings OnSaveLocal()
        {
            return saveSettings;
        }
    }
}