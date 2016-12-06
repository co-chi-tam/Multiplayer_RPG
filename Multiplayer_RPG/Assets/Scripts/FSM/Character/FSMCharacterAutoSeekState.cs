using UnityEngine;
using System.Collections;
using FSM;

namespace SurvivalTest {
	public class FSMCharacterAutoSeekState : FSMBaseControllerState
	{
		public FSMCharacterAutoSeekState(IContext context) : base (context)
		{

		}

		public override void StartState()
		{
			base.StartState ();
			if (m_Controller.GetUnderControl ()) {
				m_Controller.SetAnimation (CEnum.EAnimation.Idle);
				// FIND RANDOM POSITION
				m_Controller.FindMovePosition();

				// FIND ENEMY BASE OBJECT TYPE
				m_Controller.FindTargetInteract();
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