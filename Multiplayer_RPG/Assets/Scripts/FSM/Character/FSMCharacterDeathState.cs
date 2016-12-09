using UnityEngine;
using System.Collections;
using FSM;

namespace SurvivalTest {
	public class FSMCharacterDeathState : FSMBaseControllerState
	{

		public FSMCharacterDeathState(IContext context) : base (context)
		{

		}

		public override void StartState()
		{
			base.StartState ();
			m_Controller.SetAnimation (CEnum.EAnimation.Death);
			m_Controller.SetActive (false);
		}

		public override void UpdateState(float dt)
		{
			base.UpdateState (dt);
		}

		public override void ExitState()
		{
			base.ExitState ();
			m_Controller.ActiveObject ();
		}
	}
}