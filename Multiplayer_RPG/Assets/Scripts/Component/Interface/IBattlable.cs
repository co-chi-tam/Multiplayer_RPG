using UnityEngine;
using System;
using System.Collections;

namespace SurvivalTest {
	public interface IBattlable {

		string GetID();
		void ApplyDamage(IBattlable attacker, int damage, CEnum.EAttackType damageType);
		void ApplyBuff(IBattlable buffer, int buff, CEnum.EStatusType statusType);

		bool GetActive();
		void SetActive(bool value);

		CEnum.EObjectType GetObjectType();
		void SetObjectType(CEnum.EObjectType objectType);

	}
}
