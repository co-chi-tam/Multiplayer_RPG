using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using FSM;

namespace SurvivalTest {
	public class CItemController : CNeutralObjectController {

		protected CUIManager m_UIManager;

		protected CObjectController m_Owner;

		protected override void OnRegisterFSM ()
		{
			base.OnRegisterFSM ();

			var idleState = new FSMItemIdleState (this);
			var inativeState = new FSMItemInactiveState (this);

			m_FSMManager.RegisterState ("ItemIdleState", idleState);
			m_FSMManager.RegisterState ("ItemInactiveState", inativeState);
		}

		protected override void OnLoadData ()
		{
			base.OnLoadData ();
			if (this.GetDataUpdate()) {
				m_Data = TinyJSON.JSON.Load (m_DataText.text).Make<CItemData> ();
			}
		}

		public override void ApplyDamage (IBattlable attacker, int damage, CEnum.EElementType damageType)
		{
			if (this.GetOtherInteractive () == false)
				return;
			this.m_BattleComponent.ApplyDamage (1, CEnum.EElementType.Pure);
			this.SetOwner (attacker.GetController () as CObjectController);
		}

		public override void OnReturnObjectManager ()
		{
			// TODO
		}

		public override CEnum.EItemSlot GetItemSlot() {
			base.GetItemSlot ();
			return (m_Data as CItemData).itemSlot;
		}

		public override void SetItemSlot(CEnum.EItemSlot value) {
			base.SetItemSlot (value);
			(m_Data as CItemData).itemSlot = value;
		}

		public override int GetCurrentAmount() {
			base.GetCurrentAmount ();
			return (m_Data as CItemData).currentAmount;
		}

		public override void SetCurrentAmount(int value) {
			base.SetCurrentAmount (value);
			(m_Data as CItemData).currentAmount = value;
		}

		public override int GetMaxAmount() {
			base.GetMaxAmount ();
			return (m_Data as CItemData).maxAmount;
		}

		public override void SetOwner (CObjectController value)
		{
			base.SetOwner (value);
			this.m_Owner = value;
		}

		public override CObjectController GetOwner ()
		{
			base.GetOwner ();
			return this.m_Owner;
		}

	}
}
