using UnityEngine;
using System;
using System.Collections;

namespace SurvivalTest {
	public class CBattlableComponent : CComponent {

		private int m_TotalDamage;
		private int m_TotalSanity;
		private int m_TotalHunger;
		private int m_TotalHealthBuff;

		private IObjectInfo m_Target;

		public CBattlableComponent (IObjectInfo target) : base ()
		{
			this.m_TotalDamage 		= 0;
			this.m_TotalHealthBuff 	= 0;
			this.m_TotalSanity 		= 0;
			this.m_TotalHunger 		= 0;
			this.m_Target 			= target;
		}

		public void ApplyDamage(int value, CEnum.EElementType damageType) {
			switch (damageType) {
			default:
			case CEnum.EElementType.Pure:
				m_TotalDamage += value;
				break;
			}
		}

		public void ApplySanity(int value) {
			m_TotalSanity += value;
		}

		public void ApplyHunger(int value) {
			m_TotalHunger += value;
		}

		public void ApplyBuff(int value, CEnum.EStatusType statusType) {
			switch (statusType) {
			default:
			case CEnum.EStatusType.Health:
				m_TotalHealthBuff += value;
				break;
			}
		}

		public bool CalculateHealth(int current, out int result) {
			var needCalculate = false;
			result = current;
			if (m_TotalDamage != 0 ) {
				result = result - m_TotalDamage;
				m_TotalDamage = 0;
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

		public bool CalculateSanity(int current, out int result) {
			var needCalculate = false;
			result = current;
			if (m_TotalSanity != 0) {
				result = result + m_TotalSanity;
				m_TotalSanity = 0;
				needCalculate = true;
			}
			return needCalculate;
		}

		public bool CalculateHunger(int current, out int result) {
			var needCalculate = false;
			result = current;
			if (m_TotalHunger != 0) {
				result = result + m_TotalHunger;
				m_TotalHunger = 0;
				needCalculate = true;
			}
			return needCalculate;
		}

		public override void Clear ()
		{
			base.Clear ();
			this.m_TotalDamage 		= 0;
			this.m_TotalSanity 		= 0;
			this.m_TotalHunger 		= 0;
			this.m_TotalHealthBuff 	= 0;
		}

	}
}