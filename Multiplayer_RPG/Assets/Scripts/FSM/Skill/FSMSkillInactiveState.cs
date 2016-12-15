using UnityEngine;
using System.Collections;
using FSM;

namespace SurvivalTest {
	public class FSMSkillInactiveState : FSMBaseControllerState
	{
		public FSMSkillInactiveState(IContext context) : base (context)
		{

		}

		public override void StartState()
		{
			base.StartState ();
			m_Controller.OnEndAction.Invoke (null);
			m_Controller.SetActive (false);
			m_Controller.OnReturnObjectManager ();
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