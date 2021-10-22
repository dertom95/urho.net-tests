using System;
using System.Threading.Tasks;
using Urho.Gui;
using Urho;
using Urho.Actions;

namespace Testing {
    public class TestAnimations : Test
    {
        public TestAnimations() : base(){}

        private Node nodeObj;
        private Node nodeSuzanneMorph;

        private Node worm;

        public override Task OnStart()
        {
            var ui = Testing.I.UI;
            //ui.Clear();
            
            Scene = new Urho.Scene();
            Scene.LoadXmlFromCache(Testing.I.ResourceCache,Assets.Data.scenes.AnimationTest_xml.path);

            // ---- object anim ----
            nodeObj = Scene.GetChild(Assets.scenes.AnimationTest.all_obj.Cube.name,true);
            Animation objAnim = Testing.I.ResourceCache.GetAnimation(Assets.Data.animations.CubeAction_ani.path);
            var animControl = nodeObj.CreateComponent<AnimationController>();
            animControl.PlayExclusive(Assets.Data.animations.CubeAction_ani.path,0,true,0.1f);
            
            // ---- morph with actions
            nodeSuzanneMorph = Scene.GetChild(Assets.scenes.AnimationTest.all_obj.Suzanne_morph.name,true);
            var suzanneModel = nodeSuzanneMorph.GetComponent<AnimatedModel>();
            
            var actionSeq = new Sequence(
                new Urho.Actions.Parallel(
                    new JumpTo(1.0f,nodeSuzanneMorph.Position+new Vector3(2,0,2),1.0f,3),
                    new ActionTween(1.0f,"eyes",0.0f,1.0f,(value,key)=>{
                        suzanneModel.SetMorphWeight(key,value);
                    })
                ),
                new DelayTime(1.0f)
            );

            nodeSuzanneMorph.RunActions(new RepeatForever(
                actionSeq,
                actionSeq.Reverse()
            ));

            // --- skeletal animaion ----
            worm = Scene.GetChild(Assets.scenes.AnimationTest.all_obj.Worm.name,true);
            var wormAnimControl = worm.CreateComponent<AnimationController>();
            wormAnimControl.PlayExclusive(Assets.Data.animations.ArmatureAction_ani.path,0,true,0.1f);

            return base.OnStart();
        }


        public override void OnStop()
        {
        }

        public override void OnUpdate(float dt)
        {

           
        }
    }
}
