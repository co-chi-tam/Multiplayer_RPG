﻿using UnityEngine;
using System.Collections;

namespace SurvivalTest {
	public interface IStatus {

		string GetID();
		void SetID(string value);

		void SetData (CObjectData value);
		CObjectData GetData();

		void SetAnimation (CEnum.EAnimation value);
		CEnum.EAnimation GetAnimation();

		void SetAnimationTime (float value);
		float GetAnimationTime();

		void SetDidAttack (bool value);
		bool GetDidAttack();

		void SetObjectType (CEnum.EObjectType value);
		CEnum.EObjectType GetObjectType ();

		bool GetActive();
		void SetActive(bool value);

		bool GetUnderControl();
		void SetUnderControl(bool value);

		bool GetLocalUpdate();
		void SetLocalUpdate(bool value);

		bool GetDataUpdate();
		void SetDataUpdate(bool value);

		bool GetOtherInteractive();
		void SetOtherInteractive(bool value);

		void SetName(string value);
		string GetName();

		string GetFSMStateName();
		string GetFSMName();

		int GetCurrentHealth();
		int GetMaxHealth();
		void SetCurrentHealth(int value);

		int GetCurrentMana();
		int GetMaxMana();
		void SetCurrentMana(int value);

		int GetPureDamage();

		int GetPhysicDefend ();
		int GetPhysicDamage ();

		int GetMagicDefend ();
		int GetMagicDamage ();

		float GetMoveSpeed();

		float GetSeekRadius();

		Vector3 GetPosition ();
		void SetPosition(Vector3 position);

		Vector3 GetRotation ();
		void SetRotation(Vector3 rotation);

		Vector3 GetMovePosition ();
		void SetMovePosition(Vector3 position);

		Vector3 GetStartPosition ();
		void SetStartPosition(Vector3 position);

		void UpdateFSM (float dt);
		void OnDestroyObject ();

		string GetToken ();
		void SetToken(string value);

		object GetController ();
		CObjectController GetTargetAttack();
		void SetTargetAttack(CObjectController value);
	}
}
