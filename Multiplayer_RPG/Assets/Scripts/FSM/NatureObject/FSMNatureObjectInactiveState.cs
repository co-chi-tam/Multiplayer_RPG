using UnityEngine;
using System.Collections;
using FSM;

namespace SurvivalTest {
	public class FSMNatureObjectInactiveState : FSMCharacterInactiveState
	{
		public FSMNatureObjectInactiveState(IContext context) : base (context)
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