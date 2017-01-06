using UnityEngine;
using System.Collections;
using FSM;

namespace SurvivalTest {
	public class FSMCharacterAutoAvoidanceState : FSMBaseControllerState
	{

		public FSMCharacterAutoAvoidanceState(IContext context) : base (context)
		{

		}

		public override void StartState()
		{
			base.StartState ();
			m_Controller.SetAnimation (CEnum.EAnimation.Move);
		}

		public override void UpdateState(float dt)
		{
			base.UpdateState (dt);
			var target = m_Controller.GetTargetInteract ();
			if (target != null) { 
				var direction = m_Controller.GetPosition () - target.GetPosition ();
				var sqrtDirection = direction.normalized * m_Controller.GetSeekRadius();
				m_Controller.MoveToTarget (sqrtDirection, dt);
			}
		}

		public override void ExitState()
		{
			base.ExitState ();
		}
	}
}