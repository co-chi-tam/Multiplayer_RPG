using UnityEngine;
using System;
using System.Collections;

namespace SurvivalTest {
	public class CInventoryComponent : CComponent {

		public Action<IItem> OnEventUpdateItem;

		// Owner
		protected IInventory m_Inventory;

		// Item
		protected IItem[] m_ItemEquipSlots;
		protected IItem[] m_ItemInventorySlots;

		public CInventoryComponent (IInventory inventory) : base ()
		{
			this.m_Inventory = inventory;

			// Item equipment
			this.m_ItemEquipSlots = new IItem[4];
			this.m_ItemEquipSlots [(int)CEnum.EItemSlot.Hand] 		= null; 
			this.m_ItemEquipSlots [(int)CEnum.EItemSlot.Back] 		= null; 
			this.m_ItemEquipSlots [(int)CEnum.EItemSlot.Hat]		= null; 
			this.m_ItemEquipSlots [(int)CEnum.EItemSlot.Necklet]	= null; 
			// Inventory
			this.m_ItemInventorySlots = new IItem[10];
		}

		public bool AddInventoryItem(IItem value, Action<IItem> onAddItem, Action<IItem> onUpdateItem) {
			var currentItem = FindItemSlot ((x) => {
				return x != null 
					&& x.GetName () == value.GetName () 
					&& x.GetCurrentAmount() + value.GetCurrentAmount() <= x.GetMaxAmount();
			});
			if (currentItem != null) {
				currentItem.SetCurrentAmount (currentItem.GetCurrentAmount () + value.GetCurrentAmount ());
				if (OnEventUpdateItem != null) {
					OnEventUpdateItem (value);
				}
				if (onUpdateItem != null) {
					onUpdateItem (value);
				}
				return true;
			} else {
				for (int i = 0; i < this.m_ItemInventorySlots.Length; i++) {
					if (this.m_ItemInventorySlots [i] == null) {
						this.m_ItemInventorySlots [i] = value;
						this.m_ItemInventorySlots [i].SetCurrentAmount (value.GetCurrentAmount());
						if (OnEventUpdateItem != null) {
							OnEventUpdateItem (value);
						}
						if (onAddItem != null) {
							onAddItem (value);
						}
						return true;
					}
				}
			}
			return false;
		}

		public void SwapInventoryItem (IItem origin, IItem end) {
			var originIndex = Array.IndexOf (this.m_ItemInventorySlots, origin);
			var endIndex = Array.IndexOf (this.m_ItemInventorySlots, end);
			if (originIndex != -1 && endIndex != -1) {
				this.m_ItemInventorySlots [originIndex] = end;
				this.m_ItemInventorySlots [endIndex] = origin;	
			}
		}

		public IItem FindItemSlot(Func<IItem, bool> onCondition) {
			for (int i = 0; i < this.m_ItemInventorySlots.Length; i++) {
				if (onCondition != null) {
					if (onCondition (this.m_ItemInventorySlots [i])) {
						return this.m_ItemInventorySlots [i];
					}
				}
			}
			return null;
		}

		public void SetInventoryItem(int slot, IItem value) {
			this.m_ItemInventorySlots [slot] = value;
		}

		public IItem[] GetInventoryItems() {
			return m_ItemInventorySlots;
		}

		public void SetEquipmentItem(CEnum.EItemSlot slot, IItem value) {
			this.m_ItemEquipSlots [(int)slot] = value;
		}

		public IItem[] GetEquipmentItems() {
			return m_ItemEquipSlots;
		}
	
	}
}
