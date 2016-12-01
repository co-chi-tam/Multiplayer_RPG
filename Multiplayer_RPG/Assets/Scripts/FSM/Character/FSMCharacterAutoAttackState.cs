using UnityEngine;
using System.Collections;
using FSM;

namespace SurvivalTest {
	public class FSMCharacterAutoAttackState : FSMBaseControllerState
	{
		public FSMCharacterAutoAttackState(IContext context) : base (context)
		{

		}

		public override void StartState()
		{
			base.StartState ();
			if (m_Controller.GetUnderControl ()) {
//			var random = (int)(Mathf.PerlinNoise(Time.time, Time.time) * 4);
//			m_Controller.SetActiveSkill((int)CEnum.EAnimation.Attack_1 + random);
				m_Controller.SetActiveSkill ((int)CEnum.EAnimation.Attack_3);
			}
		}

		public override void UpdateState(float dt)
		{
			base.UpdateState (dt);
			var target = m_Controller.GetTargetAttack ();
			if (target != null) { 
				m_Controller.LookAtTarget (target.GetPosition ());
			}
		}

		public override void ExitState()
		{
			base.ExitState ();
			m_Controller.SetDidAttack (false);
		}
	}
}