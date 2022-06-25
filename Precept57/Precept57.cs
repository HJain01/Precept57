using Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UObject = UnityEngine.Object;
using ItemChanger;
using ItemChanger.Items;
using ItemChanger.Locations;
using ItemChanger.Tags;
using ItemChanger.UIDefs;

namespace Precept57
{
    public class Precept57 : Mod, ILocalSettings<SaveSettings>
    {
        private static readonly List<Precept> Precepts = new()
        {
            Precept9.Instance,
            Precept35.Instance
            Precept38.Instance,
        };
        
        private static Precept57 Instance;

        private Dictionary<string, Func<bool, bool>> BoolGetters = new();
        private Dictionary<string, Action<bool>> BoolSetters = new();
        private Dictionary<(string Key, string Sheet), Func<string>> TextEdits = new();


        public Precept57() : base("Precept 57")
        {
        }
        
        public override string GetVersion() => "0.0.2";

        private SaveSettings saveSettings = new();
        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects) {
            Log("Initializing Precept 57");

            Instance = this;

            foreach (var precept in Precepts)
            {
                Log("Added " + precept.Name);
                EmbeddedSprite sprite = precept.Sprite;

                Func<SaveSettings, PreceptSettings> settings = precept.Settings;
                PopulateHookDictionaries(precept, settings);

                var preceptItem = CreatePrecept(precept, sprite);
                ConfigureMapMod(preceptItem);
            }
            DefineModHooks();

            Log("Initialized Precept 57");
        }
        
        private void DefineModHooks()
        {
            ModHooks.GetPlayerBoolHook += GetPreceptBools;
            ModHooks.SetPlayerBoolHook += SetPreceptBools;
            ModHooks.NewGameHook += PlacePreceptsAtFixedPositions;
            ModHooks.LanguageGetHook += GetCharmStrings;
        }

        private BoolItem CreatePrecept(Precept precept, EmbeddedSprite sprite)
        {
            var item = new BoolItem()
            {
                fieldName = precept.Id,
                name = precept.Id,
                UIDef = new BigUIDef()
                {
                    bigSprite = sprite,
                    take = new LanguageString("UI", $"{precept.Id}_TAKE"),
                    press = new LanguageString("UI", $"{precept.Id}_PRESS"),
                    descOne = new LanguageString("UI", $"{precept.Id}_DESC_ONE"),
                    descTwo = new LanguageString("UI", $"{precept.Id}_DESC_TWO"),
                    name = new LanguageString("UI", $"{precept.Id}_NAME"),
                    shopDesc = new LanguageString("UI", $"{precept.Id}_DESC"),
                    sprite = sprite
                }
            };

            Finder.DefineCustomItem(item);

            return item;
        }

        private void ConfigureMapMod(BoolItem item)
        {
            var mapmodTag = item.AddTag<InteropTag>();
            mapmodTag.Message = "PreceptSupplementalMetadata";
            mapmodTag.Properties["ModSource"] = GetName();
            mapmodTag.Properties["PoolGroup"] = "Items";
        }

        private void PopulateHookDictionaries(Precept precept, Func<SaveSettings, PreceptSettings> settings)
        {
            AddTextEdit($"{precept.Id}_NAME", "UI", precept.Name);
            AddTextEdit($"{precept.Id}_DESC", "UI", precept.ShopDescription);
            AddTextEdit($"{precept.Id}_TAKE", "UI", precept.Take);
            AddTextEdit($"{precept.Id}_PRESS", "UI", precept.Press);
            AddTextEdit($"{precept.Id}_DESC_ONE", "UI", $"\"{precept.DescOne}\"");
            AddTextEdit($"{precept.Id}_DESC_TWO", "UI", precept.DescTwo);
            BoolGetters[precept.Id] = _ => settings(saveSettings).Got;
            BoolSetters[precept.Id] = value => settings(saveSettings).Got = value;
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
            var precept = Precepts.FirstOrDefault(precept => precept.Id == name);
            if (BoolSetters.TryGetValue(name, out var f) && precept != null)
            {
                f(orig);
                precept.Hook();
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
        
        private string GetCharmStrings(string key, string sheetTitle, string orig)
        {
            if (TextEdits.TryGetValue((key, sheetTitle), out var text))
            {
                return text();
            }
            return orig;
        }
        
        internal void AddTextEdit(string key, string sheetName, string text)
        {
            TextEdits.Add((key, sheetName), () => text);
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
