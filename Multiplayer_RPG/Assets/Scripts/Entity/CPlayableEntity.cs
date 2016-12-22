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
			m_ObjectSyn.AddEventListener ("ExecuteInventoryItem", OnClientExecuteInventoryItem);
			m_ObjectSyn.AddEventListener ("TouchScreenInput", OnClientTouchScreenInput);
			m_ObjectSyn.AddEventListener ("SkillInput", OnClientSkillInput);
			m_ObjectSyn.AddEventListener ("ChatInput", OnClientChatInput);
			m_ObjectSyn.AddEventListener ("EmotionInput", OnClientEmotionInput);
		}

		// Active On local and is client
		public override void OnClientLoadedObject ()
		{
			base.OnClientLoadedObject ();
			m_ObjectSyn.SetUnderControl (false);
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
		}

		[ClientCallback]
		public virtual void OnClientSkillInput(object value) {
			var skill = (int)value;
			CmdUpdateSkillInput (skill);
		}

		[ClientCallback]
		public virtual void OnClientExecuteInventoryItem(object value) {
			var item = value as IItem;
			CmdOnClientExecuteInventoryitem (item.GetID ());
		}

		[ClientCallback]
		public virtual void OnClientTouchScreenInput(object value) {
			var touchPoints = value as Vector3[];
			CmdUpdateSelectionObject (touchPoints[0], touchPoints[1]);
		}

		[ClientCallback]
		public virtual void OnClientChatInput(object value) {
			var chatInput = (string)value;
			CmdUpdateChat (chatInput);
		}

		[ClientCallback]
		public virtual void OnClientEmotionInput(object value) {
			var emotionInput = (string)value;
			CmdUpdateEmotion (emotionInput);
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
			m_ObjectSyn.UpdateSkillInput ((CEnum.EAnimation)animSkill);
			m_ObjectSyn.SetCurrentSkill ((CEnum.EAnimation)animSkill);
		}

		[Command]
		internal virtual void CmdUpdateChat(string chat) {
			m_ObjectSyn.SetChat (chat);
			RpcUpdateChat (chat);
		}

		[Command]
		internal virtual void CmdUpdateEmotion(string emotion) {
			m_ObjectSyn.SetEmotion (emotion);
			RpcUpdateEmotion (emotion);
		}

		[Command]
		internal virtual void CmdUpdateSelectionObject(Vector3 originPoint, Vector3 directionPoint) {
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
			if (m_ObjectSyn == null)
				return;
			m_ObjectSyn.SetChat (chat);
		}

		[ClientRpc]
		internal virtual void RpcUpdateEmotion(string emotion) {
			if (m_ObjectSyn == null)
				return;
			m_ObjectSyn.SetEmotion (emotion);
		}

		#endregion

	}

}
