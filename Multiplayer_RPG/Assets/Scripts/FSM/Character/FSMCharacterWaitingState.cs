using UnityEngine;
using System.Collections;
using FSM;

namespace SurvivalTest {
	public class FSMCharacterWaitingState : FSMBaseControllerState
	{
		public FSMCharacterWaitingState(IContext context) : base (context)
		{

		}

		public override void StartState()
		{
			base.StartState ();
			m_Controller.SetAnimation (CEnum.EAnimation.Idle);
		}

		public override void UpdateState(float dt)
		{
			base.UpdateState (dt);
			m_Controller.UpdateMoveInput (dt);
		}

		public override void ExitState()
		{
			base.ExitState ();
		}
	}
}