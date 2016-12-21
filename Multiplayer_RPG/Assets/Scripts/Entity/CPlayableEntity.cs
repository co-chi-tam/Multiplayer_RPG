using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CPlayableEntity : CEntity {

		#region Properties

		// User
		public CUserData userData;

		// Communicate
		protected string m_Chat = string.Empty;
		protected string m_Emotion = string.Empty;

		// Touch Point
		protected Vector3 m_OriginTouchPoint;
		protected Vector3 m_DirectionTouchPoint;

		// inventory Item
		protected string m_ExecuteItemId;

		#endregion

		#region Implementation MonoBehaviour 

		public override void Init ()
		{
			base.Init ();
			m_ObjectSyn.SetName (userData.displayName);
			m_ObjectSyn.SetToken (userData.token);
		}

		protected override void Awake ()
		{
			base.Awake ();
			userData = new CUserData ();
		}

		// Active on Server
		public override void OnServerLoadedObject ()
		{
			base.OnServerLoadedObject ();
			m_ObjectSyn.SetUnderControl (false);
			m_ObjectSyn.AddEventListener ("AddInventoryItem", OnServerUpdateInventoryItem);
			m_ObjectSyn.AddEventListener ("UpdateInventoryItem", OnServerUpdateInventoryItem);
		}

		// Active On local and is local player
		public override void OnLocalPlayerLoadedObject ()
		{
			base.OnLocalPlayerLoadedObject ();
			m_ObjectSyn.SetUnderControl (true);
		}

		// Active On local and is client
		public override void OnClientLoadedObject ()
		{
			base.OnClientLoadedObject ();
			m_ObjectSyn.SetUnderControl (false);
			m_ObjectSyn.AddEventListener ("ExecuteInventoryItem", OnClientExecuteInventoryItem);
		}

		#endregion

		#region Main methods

		#endregion

		#region Server

		[ServerCallback]
		public override void OnServerFixedUpdateSynData(float dt) {
			base.OnServerFixedUpdateSynData (dt);
			// Update Info
			RpcUpdateUserData (userData.displayName, userData.token);
			// Update Inventory
			RpcUpdateInventory();
		}

		[ServerCallback]
		public virtual void OnServerUpdateInventoryItem(object value) {
			var itemInterface = value as IItem;
			RpcOnClientAddInventoryItem (itemInterface.GetInventorySlot(), itemInterface.GetID (), itemInterface.GetCurrentAmount());
		}

		#endregion

		#region Client

		[ClientCallback]
		public override void OnClientFixedUpdateBaseTime(float dt) {
			if (m_ObjectSyn == null)
				return;
			base.OnClientFixedUpdateBaseTime (dt);
			// CMD current skill
			if (m_SkillInput != (int)m_ObjectSyn.GetCurrentSkill ()) {
				m_SkillInput = (int)m_ObjectSyn.GetCurrentSkill ();
				// Skill Input
				CmdUpdateSkillInput (m_SkillInput);
				// Reset
				m_SkillInput = (int)CEnum.EAnimation.Idle;
				m_ObjectSyn.SetCurrentSkill (CEnum.EAnimation.Idle);
			}
			// CMD Chat
			if (m_Chat != m_ObjectSyn.GetChat()) {
				m_Chat = m_ObjectSyn.GetChat ();
				CmdUpdateChat (m_ObjectSyn.GetChat ());
			}
			// CMD Emotion
			if (m_Emotion != m_ObjectSyn.GetEmotion()) {
				m_Emotion = m_ObjectSyn.GetEmotion ();
				CmdUpdateEmotion (m_ObjectSyn.GetEmotion ());
			}
			// Touch point
			if (m_OriginTouchPoint != m_ObjectSyn.GetOriginTouchPoint () || m_DirectionTouchPoint != m_ObjectSyn.GetDirectionTouchPoint ()) {
				m_OriginTouchPoint = m_ObjectSyn.GetOriginTouchPoint ();
				m_DirectionTouchPoint = m_ObjectSyn.GetDirectionTouchPoint ();
				// Update touch point
				CmdUpdateSelectionObject (m_ObjectSyn.GetOriginTouchPoint (), m_ObjectSyn.GetDirectionTouchPoint ());
				// Reset
				m_OriginTouchPoint = Vector3.zero;
				m_ObjectSyn.SetOriginTouchPoint (Vector3.zero);
			}
		}

		[ClientCallback]
		public virtual void OnClientExecuteInventoryItem(object value) {
			var item = value as IItem;
			CmdOnClientExecuteInventoryitem (item.GetID ());
		}

		#endregion

		#region Command

		[Command]
		internal virtual void CmdOnClientExecuteInventoryitem(string value) {
			var item = m_NetworkManager.FindEntity (value);
			var itemController = item.GetController () as CItemController;
			m_ObjectSyn.ExecuteInventoryItem (itemController.GetComponent<IItem> ());
		}

		[Command]
		internal virtual void CmdUpdateSkillInput(int animSkill) {
			m_SkillInput = animSkill;
			m_ObjectSyn.UpdateSkillInput ((CEnum.EAnimation)animSkill);
			m_ObjectSyn.SetCurrentSkill ((CEnum.EAnimation)animSkill);
		}

		[Command]
		internal virtual void CmdUpdateChat(string chat) {
			m_Chat = chat;
			m_ObjectSyn.SetChat (chat);
			RpcUpdateChat (chat);
		}

		[Command]
		internal virtual void CmdUpdateEmotion(string emotion) {
			m_Emotion = emotion;
			m_ObjectSyn.SetEmotion (emotion);
			RpcUpdateEmotion (emotion);
		}

		[Command]
		internal virtual void CmdUpdateSelectionObject(Vector3 originPoint, Vector3 directionPoint) {
			m_OriginTouchPoint = originPoint;
			m_DirectionTouchPoint = directionPoint;
			m_ObjectSyn.UpdateSelectionObject (originPoint, directionPoint);
		}

		#endregion

		#region RPC

		[ClientRpc]
		internal virtual void RpcOnClientAddInventoryItem(int slot, string item, int amount) {
			var itemEntity = m_NetworkManager.FindEntity (item);
			var itemInterface = itemEntity.GetController () as CItemController;
			if (itemInterface != null) {
				itemInterface.SetCurrentAmount (amount);
				m_ObjectSyn.SetInventoryItem (slot, itemInterface);
			} 
		}

		[ClientRpc]
		internal virtual void RpcUpdateUserData(string name, string token) {
			userData.displayName = name;
			userData.token = token;
			if (m_ObjectSyn == null)
				return;
			m_ObjectSyn.SetName (name);
			m_ObjectSyn.SetToken (token);
		}

		[ClientRpc]
		internal virtual void RpcUpdateChat(string chat) {
			m_Chat = chat;
			if (m_ObjectSyn == null)
				return;
			m_ObjectSyn.SetChat (chat);
		}

		[ClientRpc]
		internal virtual void RpcUpdateEmotion(string emotion) {
			m_Emotion = emotion;
			if (m_ObjectSyn == null)
				return;
			m_ObjectSyn.SetEmotion (emotion);
		}

		[ClientRpc]
		internal virtual void RpcUpdateInventory() {
			
		}

		#endregion

	}

}
