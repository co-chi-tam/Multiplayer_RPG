using UnityEngine;
using System;
using System.Collections;

namespace SurvivalTest {
	public class CBattlableComponent : CComponent {

		private int m_TotalDamage;
		private int m_TotalPhysicDamage;
		private int m_TotalMagicDamage;
		private int m_TotalHealthBuff;
		private int m_TotalManaBuff;

		private IStatus m_Target;

		public CBattlableComponent (IStatus target) : base ()
		{
			m_TotalDamage 		= 0;
			m_TotalPhysicDamage = 0;
			m_TotalMagicDamage 	= 0;
			m_TotalHealthBuff 	= 0;
			m_TotalManaBuff 	= 0;
			m_Target 			= target;
		}

		public void ApplyDamage(int damage, CEnum.EAttackType damageType) {
			switch (damageType) {
			default:
			case CEnum.EAttackType.Pure:
				m_TotalDamage += damage;
				break;
			case CEnum.EAttackType.Physic:
				m_TotalPhysicDamage += damage - m_Target.GetPhysicDefend();
				m_TotalPhysicDamage = Mathf.Max (1, m_TotalPhysicDamage);
				break;
			case CEnum.EAttackType.Magic:
				m_TotalMagicDamage += damage - m_Target.GetMagicDefend();
				m_TotalMagicDamage = Mathf.Max (1, m_TotalMagicDamage);
				break;
			}
		}

		public void ApplyBuff(int buff, CEnum.EStatusType statusType) {
			switch (statusType) {
			default:
			case CEnum.EStatusType.Health:
				m_TotalHealthBuff += buff;
				break;
			case CEnum.EStatusType.Mana:
				m_TotalManaBuff += buff;
				break;
			case CEnum.EStatusType.Lucky:
				// TODO
				break;
			}
		}

		public bool CalculateHealth(int current, out int result) {
			var needCalculate = false;
			result = current;
			if (m_TotalDamage != 0 
				|| m_TotalPhysicDamage != 0
				|| m_TotalMagicDamage != 0) {
				var total = m_TotalDamage + m_TotalPhysicDamage + m_TotalMagicDamage;
				result = result - total;
				m_TotalDamage 		= 0;
				m_TotalPhysicDamage = 0;
				m_TotalMagicDamage 	= 0;
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

		public bool CalculateMana(int current, out int result) {
			var needCalculate = false;
			result = current;
			if (m_TotalManaBuff != 0) {
				result = result + m_TotalManaBuff;
				m_TotalManaBuff = 0;
				needCalculate |= true;
			}
			result = Mathf.Clamp (result, 0, m_Target.GetMaxMana ());
			return needCalculate;
		}

		public override void Clear ()
		{
			base.Clear ();
			m_TotalDamage 		= 0;
			m_TotalPhysicDamage = 0;
			m_TotalMagicDamage 	= 0;
			m_TotalHealthBuff 	= 0;
			m_TotalManaBuff 	= 0;
		}

	}
}