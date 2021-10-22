// namespace Testing {
//     using global::Urho;
//     using global::Urho.Actions;
//     using Urho;
// 	public class ShapeKeyTo : FiniteTimeAction
// 	{
// 		public string ShapeKey {get;protected set;}
//         public float DestValue {get;protected set;}
//         public float InitalValue {get;protected set;}


// 		#region Constructors

// 		public ShapeKeyTo (float duration, string shapeKey, float destValue) : base (duration)
// 		{
// 			ShapeKey = shapeKey;
// 		}

// 		#endregion Constructors


// 		protected override ActionState StartAction(Node target)
// 		{
// 			return new ShapeKeyToState (this, target);
// 		}

// 		public override FiniteTimeAction Reverse ()
// 		{
// 			return new ShapeKeyTo (Duration, ShapeKey, destValue);
// 		}
// 	}

// 	public class ShapeKeyToState : FiniteTimeActionState
// 	{

// 		protected uint Times { get; set; }

// 		protected bool OriginalState { get; set; }

// 		public ShapeKeyToState (ShapeKeyTo action, Node target)
// 			: base (action, target)
// 		{ 
// 			Times = action.Times;
// 			OriginalState = target.Enabled;
// 		}

// 		public override void Update (float time)
// 		{
// 			if (Target != null && !IsDone)
// 			{
// 				float slice = 1.0f / Times;
// 				float m = time % slice;
// 				Target.Enabled = m > (slice / 2);
// 			}
// 		}

// 		protected  override void Stop ()
// 		{
// 			Target.Enabled = OriginalState;
// 			base.Stop ();
// 		}

// 	}
// }