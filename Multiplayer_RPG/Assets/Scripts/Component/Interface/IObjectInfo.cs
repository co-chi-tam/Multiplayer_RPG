using UnityEngine;
using System;
using System.Collections;

namespace SurvivalTest {
	public interface IObjectInfo {

		string GetID();
		void SetID(string value);

		void SetObjectType (CEnum.EObjectType value);
		CEnum.EObjectType GetObjectType ();

		void SetClassType (CEnum.EClassType value);
		CEnum.EClassType GetClassType ();

		void SetName(string value);
		string GetName();

		void SetAvatar(string value);
		string GetAvatar();

		float GetSize();
		float GetHeight();

		int GetCurrentHealth();
		int GetMaxHealth();
		void SetCurrentHealth(int value);

		int GetCurrentSanity();
		int GetMaxSanity();
		void SetCurrentSanity (int value);

		int GetCurrentHunger();
		int GetMaxHunger();
		void SetCurrentHunger (int value);

		float GetAttackSpeed();
		int GetAttackDamage ();
		int GetPhysicDefend ();

		float GetMoveSpeed();

		float GetSeekRadius();

		object GetController ();

	}
}

