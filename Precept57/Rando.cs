using System.Collections.Generic;
using ItemChanger;
using Modding;
using RandomizerMod;
using RandomizerMod.Menu;
using RandomizerMod.Settings;
using RandomizerMod.Logging;
using RandomizerMod.RandomizerData;
using RandomizerMod.RC;
using RandomizerCore;
using RandomizerCore.Logic;
using RandomizerCore.LogicItems;
using SFCore;
using RequestBuilder = RandomizerMod.RC.RequestBuilder;

namespace Precept57
{
    internal class Rando : Loggable
    {
        public bool IsActive() =>
            (IsInstalled() && RandomizerMod.RandomizerMod.RS?.GenerationSettings != null);
        
        public bool IsInstalled() =>
            (ModHooks.GetMod("Randomizer 4") is Mod);

        public void HookRandomizer()
        {
            RequestBuilder.OnUpdate.Subscribe(-498, DefinePreceptsForRando);
            RequestBuilder.OnUpdate.Subscribe(50, AddPreceptsToPool);
            RCData.RuntimeLogicOverride.Subscribe(50, DefineLogicItems);
            RCData.RuntimeLogicOverride.Subscribe(49, DefineLogicLocations);
        }

        public void DefineLogicLocations(GenerationSettings gs, LogicManagerBuilder lmb)
        {
            foreach (var precept in Precept57.Precepts)
            {
                lmb.AddLogicDef(new RawLogicDef(name: precept.Id, logic: precept.Scene));
            }
        }

        private void DefineLogicItems(GenerationSettings gs, LogicManagerBuilder lmb)
        {
            foreach (var precept in Precept57.Precepts)
            {
                var name = precept.Id;
                var term = lmb.GetOrAddTerm(name);
                var oneOf = new TermValue(term, 1);
                lmb.AddItem(new CappedItem(name, new TermValue[]
                {
                    oneOf,
                    new TermValue(lmb.GetTerm("CHARMS"), 1)
                }, oneOf));
            }
        }

        private void AddPreceptsToPool(RequestBuilder rb)
        {
            foreach (var precept in Precept57.Precepts)
            {
                rb.AddItemByName((precept.Id));
                Log($"Location Added for {precept.Id}");
                rb.AddLocationByName(precept.Id);
            }
        }

        private void DefinePreceptsForRando(RequestBuilder rb)
        {
            foreach (var precept in Precept57.Precepts) //Need a method of getting the precepts list her
            {
                var name = precept.Id;
                rb.EditItemRequest(name, info =>
                {
                    info.getItemDef = () => new()
                    {
                        Name = name,
                        Pool = "Precept",
                        MajorItem = false,
                        PriceCap = 420
                    };
                });
                
                rb.EditLocationRequest(name, info =>
                {
                    info.getLocationDef = () => new()
                    {
                        Name = name,
                        SceneName = precept.Scene,
                        FlexibleCount = false,
                        AdditionalProgressionPenalty = false
                    };
                });
            }
        }
    }
}