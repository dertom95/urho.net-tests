using System;
using System.Threading.Tasks;
using Urho.Gui;

namespace Testing {
    public class TestText : Test
    {
        public TestText() : base(){}

        private Text text;
        private UIElement elem;
        private Button btn;
        private float time = 0;

        public override Task OnStart()
        {
            var ui = Testing.I.UI;
            //ui.Clear();
            Scene = new Urho.Scene();
            Scene.LoadXmlFromCache(Testing.I.ResourceCache,Assets.Data.scenes.ParticleTest_xml.path);

            btn = new Button(){
                HorizontalAlignment=HorizontalAlignment.Center,
                VerticalAlignment=VerticalAlignment.Top,
                Size=new Urho.IntVector2(800,100),
                BlendMode=Urho.BlendMode.Alpha,
                Border=new Urho.IntRect(30,30,30,30),
            };
            btn.Click += (args)=>{
                var btn = args.Element;
                Urho.LogSharp.Debug($"Clicked:{btn}");
                
            };
            //win.SetStyle("Window",Testing.I.ResourceCache.GetXmlFile("UI/DefaultStyle.xml"));
            btn.Texture=Testing.I.ResourceCache.GetTexture2D(Assets.Data.textures.all.window_png.path);
            ui.Root.AddChild(btn);

            elem = new UIElement(){
                HorizontalAlignment=HorizontalAlignment.Center,
                VerticalAlignment=VerticalAlignment.Top,
                Size=new Urho.IntVector2(200,200),
                LayoutMode=LayoutMode.Vertical,
                Position=new Urho.IntVector2(0,15)
            };
            btn.AddChild(elem);
            text = new Text(){
                Value="Fortuna",
                // HorizontalAlignment=HorizontalAlignment.Center,
                // VerticalAlignment=VerticalAlignment.Top,
            };
            text.SetFont(Testing.I.ResourceCache.GetFont(Assets.Data.fonts.BlueHighway_ttf.path),16);
            
            var text2 = new Text(){
                Value="Fortuna2",
                HorizontalAlignment=HorizontalAlignment.Center,
                VerticalAlignment=VerticalAlignment.Top
            };
            text2.SetFont(Testing.I.ResourceCache.GetFont(Assets.Data.fonts.BlueHighway_ttf.path),16);

            elem.AddChild(text);
            elem.AddChild(text2);
            return base.OnStart();
        }


        public override void OnStop()
        {
            btn.Remove();
        }

        public override void OnUpdate(float dt)
        {
            time += dt;
            text.Value=$"Time : {time}";
            

        }
    }
}
