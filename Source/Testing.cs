using Urho;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Testing
{
	public abstract class Test : UrhoObject {
		TaskCompletionSource<bool> testFinished;

        protected Test(IntPtr handle) : base(handle)
        {
        }

        protected Test() : base(UrhoObjectFlag.Empty)
        {
        }

		public Scene Scene{get;protected set;}

		

        public virtual Task OnStart(){
			testFinished = new TaskCompletionSource<bool>();
			return testFinished.Task;
		}

		public void Finish(){
			LogSharp.Debug($"Finished:{GetType()}");
			testFinished.TrySetResult(true);
		}
		
		public abstract void OnUpdate(float dt);
		public abstract void OnStop();

		protected Vector3 center = Vector3.Zero;
		public virtual Vector3 GetCenter(){
			return center;
		}
	}

    public class ParticleTest : Test
    {

        public ParticleTest() : base()
        {
			
        }

		ParticleEmitter p1 = null;
        public override Task OnStart()
        {
			Scene = new Scene ();
			Scene.LoadXmlFromCache(Testing.I.ResourceCache, Assets.Data.scenes.ParticleTest_xml.path);
			var cube = Scene.GetNode(Assets.scenes.ParticleTest.all_obj.Empty.id);
			p1 = cube.GetComponent<ParticleEmitter>();
			center = cube.Position;

			return base.OnStart();
        }

		bool particle1Active = true;

        public override void OnUpdate(float dt)
        {
            if (Testing.I.Input.GetKeyPress(Key.Space)){
				particle1Active = !particle1Active;
				p1.Enabled = particle1Active;
			}
			
			LogSharp.Debug($"A:{dt}");
        }

        public override void OnStop()
        {

        }
    }

    public class Testing : Sample
	{
		public static Testing I = null;

		public static List<Test> tests = new List<Test>(){
			new TestAnimations(), new ParticleTest(),new TestText()
		};

		Camera camera;
		Scene scene;

		Test currentTest = null;

		[Preserve]
		public Testing() : base(new ApplicationOptions(assetsFolder: "Data;CoreData")) { }

		protected override void Start ()
		{
			var exporter = new Blender.DotNetComponentExporter();
			I = this;
			base.Start ();
			CreateScene ();
			SimpleCreateInstructionsWithWasd ();
			Input.SetMouseVisible(true);
		}


		async Task RunTest(Test test) {
			currentTest = test;
			var testFinished = test.OnStart();
			scene = test.Scene;
			camera = scene.GetComponent<Camera>(true);
			if (camera == null) {
				CameraNode = scene.CreateChild("cn");
				camera = CameraNode.CreateComponent<Camera>();
			} else {
				CameraNode = camera.Node;
			}
			var center = currentTest.GetCenter();
			CameraNode.LookAt(center,Vector3.Up);
			var camRot = CameraNode.Rotation.ToEulerAngles();
			Yaw = camRot.Y;
			Pitch = camRot.X;
			SetupViewport();
			await testFinished;
			test.OnStop();
			currentTest = null;
		}

		async void CreateScene ()
		{
			int testNr = 0;
			while (true){
				var test = tests[testNr++];
				await RunTest(test);
				if (testNr >= tests.Count){
					testNr = 0;
				}
			}
		}

		void CreateParticleTest(){
			
			// Create a scene node for the camera, which we will move around
    		// The camera will use default settings (1000 far clip distance, 45 degrees FOV, set aspect ratio automatically)

			
			// Set an initial position for the camera scene node above the plane
			CameraNode.Position = new Vector3 (0, 5, 0);
		}
		
		void SetupViewport ()
		{
			// Set up a viewport to the Renderer subsystem so that the 3D scene can be seen. We need to define the scene and the camera
			// at minimum. Additionally we could configure the viewport screen size and the rendering path (eg. forward / deferred) to
			// use, but now we just use full screen and default render path configured in the engine command line options
			Renderer.SetViewport (0, new Viewport (Context, scene, camera, null));
		}

		protected override void OnUpdate(float timeStep)
		{
			base.OnUpdate(timeStep);
			currentTest?.OnUpdate(timeStep);
			SimpleMoveCamera3D(timeStep);

			if (Input.GetKeyPress(Key.Tab)){
				currentTest?.Finish();
			}		

			Input.SetMouseVisible(!Input.GetMouseButtonDown(MouseButton.Right));
		}
	}
}