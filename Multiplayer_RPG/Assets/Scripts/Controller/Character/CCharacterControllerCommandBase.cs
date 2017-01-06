using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public partial class CCharacterController {

		protected override void OnRegisterCommand ()
		{
			base.OnRegisterCommand ();
			this.m_EventComponent.AddEventListener ("EAT_BAD_FOOD", OnEatSomethingBad);
			this.m_EventComponent.AddEventListener ("EAT_GOOD_FOOD", OnEatSomethingGood);
		}

		public virtual void OnUpdateExecuteCommand() {
			var executeItem = this.m_InventoryComponent.ExecuteItem ((x) => {
				return x.GetCurrentAmount() > 0;				
			});
			if (executeItem != null && executeItem.GetOwner() == this) {
				this.InvokeCommand (executeItem.GetExecuteCommand (), executeItem);
			}
		}

		public override void ExecuteInventoryItem(object value) {
			base.ExecuteInventoryItem (value);
			if (this.GetAnimation() != CEnum.EAnimation.Idle)
				return;
			var item = value as IItem;
			if (item.GetOwner () == this) {
				this.m_InventoryComponent.AddExecuteItemList (item);
				// Only client call it.
				this.m_EventComponent.InvokeEventListener ("ExecuteInventoryItem", item);
			}
		}

		protected virtual void OnEatSomethingBad(object value) {
			var item = value as IItem;
			this.CalculateItemCommand (item, () => {
				if (this.GetDataUpdate ()) {
					this.ApplyDamage (null, 10, CEnum.EElementType.Pure);
				}
			});
		}

		protected virtual void OnEatSomethingGood(object value) {
			var item = value as IItem;
			this.CalculateItemCommand (item, () => {
				if (this.GetDataUpdate ()) {
					this.ApplyBuff (null, 10, CEnum.EStatusType.Health);
				}
			});
		}

		// Calculate item using amount
		private void CalculateItemCommand(IItem item, Action onExecuteItem) {
			if (item.GetCurrentAmount () > 0) {
				item.SetCurrentAmount (item.GetCurrentAmount () - 1);
				if (onExecuteItem != null) {
					onExecuteItem ();
				}
				this.m_UIManager.LoadInventoryItems (m_InventoryComponent.GetInventoryItems (), this.ExecuteInventoryItem);
			} 
			if (item.GetCurrentAmount () <= 0) {
				var itemController = item.GetController() as CObjectController;
				itemController.SetActive (false);
				itemController.DisableObject ("Inactive");
				this.m_ObjectManager.SetObject(itemController.GetName(), itemController); 
				if (this.m_InventoryComponent.RemoveInventoryItem (item)) {
					this.m_UIManager.LoadInventoryItems (m_InventoryComponent.GetInventoryItems (), this.ExecuteInventoryItem);
				};
			}
		}
	
	}
}

