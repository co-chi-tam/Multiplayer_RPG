using UnityEngine;
using System;
using System.Collections;

namespace SurvivalTest {
	public class CBattlableComponent : CComponent {

		private int m_TotalDamage;
		private int m_TotalHealthBuff;

		private IStatus m_Target;

		public CBattlableComponent (IStatus target) : base ()
		{
			m_TotalDamage 		= 0;
			m_TotalHealthBuff 	= 0;
			m_Target 			= target;
		}

		public void ApplyDamage(int damage, CEnum.EElementType damageType) {
			switch (damageType) {
			default:
			case CEnum.EElementType.Pure:
				m_TotalDamage += damage;
				break;
			}
		}

		public void ApplyBuff(int buff, CEnum.EStatusType statusType) {
			switch (statusType) {
			default:
			case CEnum.EStatusType.Health:
				m_TotalHealthBuff += buff;
				break;
			}
		}

		public bool CalculateHealth(int current, out int result) {
			var needCalculate = false;
			result = current;
			if (m_TotalDamage != 0 ) {
				var total = m_TotalDamage;
				result = result - total;
				m_TotalDamage 		= 0;
				needCalculate |= true;
			}
			if (m_TotalHealthBuff != 0) {
				result = result + m_TotalHealthBuff;
				m_TotalHealthBuff = 0;
				needCalculate |= true;
			}
			result = Mathf.Clamp (result, 0, m_Target.GetMaxHealth ());
			return needCalculate;
		}

		public override void Clear ()
		{
			base.Clear ();
			m_TotalDamage 		= 0;
			m_TotalHealthBuff 	= 0;
		}

	}
}