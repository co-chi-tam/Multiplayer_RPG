using UnityEngine;
using System.Collections;
using FSM;

namespace SurvivalTest {
	public class FSMMapIdleState : FSMBaseState
	{
		public FSMMapIdleState(IContext context) : base (context)
		{
			
		}

		public override void StartState()
		{
			base.StartState ();
		}

		public override void UpdateState(float dt)
		{
			base.UpdateState (dt);
		}

		public override void ExitState()
		{
			base.ExitState ();
		}
	}
}