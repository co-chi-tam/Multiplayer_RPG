using UnityEngine;
using System.Collections;
using FSM;

namespace SurvivalTest {
	public class FSMItemInactiveState : FSMCharacterInactiveState
	{
		public FSMItemInactiveState(IContext context) : base (context)
		{

		}

		public override void StartState()
		{
			base.StartState ();
			var owner = m_Controller.GetOwner () as CCharacterController;
			if (owner != null) {
				owner.AddInventoryItem (m_Controller as CNeutralObjectController);
			}
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