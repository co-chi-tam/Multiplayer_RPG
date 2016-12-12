using UnityEngine;
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

		void OnBecameVisible();
		void OnBecameInvisible();
		bool GetIsVisible ();

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
		void SetFSMStateName(string value);
		string GetFSMName();

		float GetSize();
		float GetHeight();

		int GetCurrentHealth();
		int GetMaxHealth();
		void SetCurrentHealth(int value);

		float GetAttackSpeed();
		int GetAttackDamage ();
		int GetPhysicDefend ();

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

		Vector3 GetOriginTouchPoint ();
		void SetOriginTouchPoint(Vector3 position);
		Vector3 GetDirectionTouchPoint ();
		void SetDirectionTouchPoint(Vector3 position);

		CEnum.EAnimation GetCurrentSkill();
		void SetCurrentSkill(CEnum.EAnimation value);

		void UpdateFSM (float dt);
		void UpdateTouchInput (float dt);
		void UpdateSkillInput (CEnum.EAnimation skill);
		void UpdateSelectionObject (Vector3 originPoint, Vector3 directionPoint);
		void OnDestroyObject ();

		string GetToken ();
		void SetToken(string value);

		object GetController ();
		CObjectController GetTargetInteract();
		void SetTargetInteract(CObjectController value);

		void Chat(string value);
		void SetChat (string value);
		string GetChat();

		void ShowEmotion(string value);
		void SetEmotion (string value);
		string GetEmotion();
	}
}
