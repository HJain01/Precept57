using Modding;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Precept57
{
    internal class Precept35 : Precept
    {
        public static readonly Precept35 Instance = new();

        private Precept35() {}

        public override string SpriteFileName => "Precept35.png";
        public override string Name => "Precept 35";
        public override string Id => "Precept_35";
        public override string ShopDescription => "Add UP and DOWN text to the top and bottom of the screen.";
        public override string Take => "Following";
        public override string Press => "Tap A";
        public override string DescOne => "Up is Up, Down is Down";

        public override string DescTwo => "No matter how turned around you get, you'll know where is up and where is down";
        public override string Scene => "Crossroads_36";
        public override float X => 37.5f;
        public override float Y => 29.1f;

        public override PreceptSettings Settings(SaveSettings s) => s.Precept35;
        
        private GameObject canvas;


        public override void Hook()
        {
            AddUpAndDownText();
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += DestroyText;
            ModHooks.SavegameLoadHook += AddTextAfterLoad;
        }

        private void AddTextAfterLoad(int obj)
        {
            AddUpAndDownText();
        }

        private void DestroyText(Scene arg0, Scene scene)
        {
            if (scene.name == "Menu_Title")
            {
                Object.Destroy(canvas);
            }
        }

        private void AddUpAndDownText()
        {
            canvas = CanvasUtil.CreateCanvas(RenderMode.ScreenSpaceOverlay,
                new Vector2(Screen.width, Screen.height));
            AddUpText(canvas);
            AddDownText(canvas);
        }

        private void AddUpText(GameObject upCanvas)
        {
            Object.DontDestroyOnLoad(upCanvas);
            var rectSizeDelta = new Vector2(100, 100);
            var rectAnchorPosition = new Vector2(0, Screen.height - 150);
            var rectMin = new Vector2(0.5f, 0f);
            var rectMax = new Vector2(0.5f, 0f);
            var rectPivot = new Vector2(0.5f, 0.5f);
            var rectData = new CanvasUtil.RectData(rectSizeDelta, rectAnchorPosition, rectMin, rectMax, rectPivot);
            CanvasUtil.CreateTextPanel(upCanvas, "UP", 48, TextAnchor.UpperCenter, rectData)
                .GetComponent<Text>();
        }
        
        private void AddDownText(GameObject downCanvas)
        {
            Object.DontDestroyOnLoad(downCanvas);
            var rectSizeDelta = new Vector2(200, 200);
            var rectAnchorPosition = new Vector2(0, 150);
            var rectMin = new Vector2(0.5f, 0f);
            var rectMax = new Vector2(0.5f, 0f);
            var rectPivot = new Vector2(0.5f, 0.5f);
            var rectData = new CanvasUtil.RectData(rectSizeDelta, rectAnchorPosition, rectMin, rectMax, rectPivot);
            CanvasUtil.CreateTextPanel(downCanvas, "DOWN", 48, TextAnchor.LowerCenter, rectData)
                .GetComponent<Text>();
        }
    }
}