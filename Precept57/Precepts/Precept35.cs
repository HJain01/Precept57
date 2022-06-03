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
        public override string Id => "Precept_35";
        public override string Description => "Add UP and DOWN text to the top and bottom of the screen.";
        public override string Scene => "Crossroads_36"; //TODO: Figure out which scene to put it in
        public override float X => 37.5f; //TODO: Figure out X coordinate in scene to put it at
        public override float Y => 29.1f; //TODO: Figure out Y coordinate in scene to put it at

        public override PreceptSettings Settings(SaveSettings s) => s.Precept35;

        public override void Hook()
        {
            ModHooks.HeroUpdateHook += AddText;
        }

        private void AddText()
        {
            if (Equipped())
            {
                AddUpText();
                AddDownText();
            }
        }

        private static void AddUpText()
        {
            GameObject canvas = CanvasUtil.CreateCanvas(RenderMode.ScreenSpaceOverlay, new Vector2(Screen.width, Screen.height));
            var rectSizeDelta = new Vector2(100, 100);
            var rectAnchorPosition = new Vector2(0, Screen.height - 150);
            var rectMin = new Vector2(0.5f, 0f);
            var rectMax = new Vector2(0.5f, 0f);
            var rectPivot = new Vector2(0.5f, 0.5f);
            var rectData = new CanvasUtil.RectData(rectSizeDelta, rectAnchorPosition, rectMin, rectMax, rectPivot);
            CanvasUtil.CreateTextPanel(canvas, "UP", 48, TextAnchor.UpperCenter, rectData)
                .GetComponent<Text>();
        }
        
        private static void AddDownText()
        {
            GameObject canvas = CanvasUtil.CreateCanvas(RenderMode.ScreenSpaceOverlay, new Vector2(2560, 1440));
            var rectSizeDelta = new Vector2(200, 200);
            var rectAnchorPosition = new Vector2(0, 150);
            var rectMin = new Vector2(0.5f, 0f);
            var rectMax = new Vector2(0.5f, 0f);
            var rectPivot = new Vector2(0.5f, 0.5f);
            var rectData = new CanvasUtil.RectData(rectSizeDelta, rectAnchorPosition, rectMin, rectMax, rectPivot);
            CanvasUtil.CreateTextPanel(canvas, "DOWN", 48, TextAnchor.LowerCenter, rectData, true)
                .GetComponent<Text>();
        }
    }
}