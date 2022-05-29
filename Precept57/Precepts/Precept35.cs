using Modding;
using UnityEngine;
using UnityEngine.UI;

namespace Precept57
{
    internal class Precept35 : Precept
    {
        public static readonly Precept35 Instance = new();

        private Precept35() {}

        public override string Sprite => "Precept35.png";
        public override string Name => "Precept 35";
        public override string Description => "Add UP and DOWN text to the top and bottom of the screen.";
        public override string Scene => "Crossroads_20"; //TODO: Figure out which scene to put it in
        public override float X => 3.0f; //TODO: Figure out X coordinate in scene to put it at
        public override float Y => 3.0f; //TODO: Figure out Y coordinate in scene to put it at

        public override PreceptSettings Settings(SaveSettings s) => s.Precept35;

        public override void Hook()
        {
            ModHooks.HeroUpdateHook += AddText;
        }

        public void AddText()
        {
            var canvas = CanvasUtil.CreateCanvas(RenderMode.ScreenSpaceOverlay, new Vector2(1920, 1080));
            var rectSizeDelta = new Vector2(600, 600);
            var rectAnchorPosition = new Vector2(600, 600);
            var rectMin = new Vector2(0.5f, 0f);
            var rectMax = new Vector2(0.5f, 0f);
            var rectPivot = new Vector2(0.5f, 0.5f);
            var rectData = new CanvasUtil.RectData(rectSizeDelta, rectAnchorPosition, rectMin, rectPivot);
            var text = CanvasUtil.CreateTextPanel(canvas, "UP", 12, TextAnchor.UpperCenter, rectData, true)
                .GetComponent<Text>();
        }
    }
}