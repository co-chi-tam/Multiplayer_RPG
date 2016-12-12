using UnityEngine;
using System.Collections;
using FSM;

namespace SurvivalTest {
	public class FSMCharacterAttackState : FSMBaseControllerState
	{

		public FSMCharacterAttackState(IContext context) : base (context)
		{

		}

		public override void StartState()
		{
			base.StartState ();
			m_Controller.InteractAnObject ();
		}

		public override void UpdateState(float dt)
		{
			base.UpdateState (dt);
			m_Controller.UpdateTouchInput (dt);
			var target = m_Controller.GetTargetInteract ();
			if (target != null) { 
				m_Controller.LookAtTarget (target.GetPosition ());
			}
		}

		public override void ExitState()
		{
			base.ExitState ();
			m_Controller.SetTargetInteract (null);
			m_Controller.SetDidAttack (false);
		}
	}
}