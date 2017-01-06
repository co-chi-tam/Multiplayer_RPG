using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CInventoryComponent : CComponent {

		public Action<IItem> OnEventUpdateItem;

		// Owner
		protected IInventory m_Inventory;

		// Item
		protected IItem[] m_ItemEquipSlots;
		protected IItem[] m_ItemInventorySlots;

		// Execute item
		protected Queue<IItem> m_ExecuteItems;

		public CInventoryComponent (IInventory inventory, int slot) : base ()
		{
			this.m_Inventory = inventory;

			// Item equipment
			this.m_ItemEquipSlots = new IItem[4];
			this.m_ItemEquipSlots [(int)CEnum.EItemSlot.Hand] 		= null; 
			this.m_ItemEquipSlots [(int)CEnum.EItemSlot.Back] 		= null; 
			this.m_ItemEquipSlots [(int)CEnum.EItemSlot.Hat]		= null; 
			this.m_ItemEquipSlots [(int)CEnum.EItemSlot.Necklet]	= null; 
			// Inventory
			this.m_ItemInventorySlots = new IItem[slot];
			// Execute items
			this.m_ExecuteItems = new Queue<IItem> ();
		}

		public CInventoryComponent (IInventory inventory, IItem[] equipmentItems, IItem[] inventoryItems) : base ()
		{
			this.m_Inventory = inventory;
			// Item equipment
			this.m_ItemEquipSlots = equipmentItems; 
			// Inventory
			this.m_ItemInventorySlots = inventoryItems;
			// Execute items
			this.m_ExecuteItems = new Queue<IItem> ();
		}

		public void AddExecuteItemList(IItem item) {
			if (this.m_ExecuteItems.Contains (item) == false) {
				this.m_ExecuteItems.Enqueue (item);
			}
		}

		public IItem ExecuteItem(Func<IItem, bool> onExecuteItem) {
			if (this.m_ExecuteItems.Count <= 0)
				return null;
			var firstItem = this.m_ExecuteItems.Peek ();
			if (onExecuteItem != null) {
				if (onExecuteItem (firstItem)) {
					return this.m_ExecuteItems.Dequeue ();
				}
			}
			return firstItem;
		}

		public bool AddInventoryItem(IItem value, Action<IItem> onAddItem, Action<IItem> onUpdateItem) {
			var currentItem = FindItemSlot ((x) => {
				return x != null 
					&& x.GetName () == value.GetName () 
					&& x.GetCurrentAmount() + value.GetCurrentAmount() <= x.GetMaxAmount();
			});
			if (currentItem != null) {
				currentItem.SetInventorySlot (Array.IndexOf (this.m_ItemInventorySlots, currentItem));
				currentItem.SetCurrentAmount (currentItem.GetCurrentAmount () + value.GetCurrentAmount ());
				if (OnEventUpdateItem != null) {
					OnEventUpdateItem (currentItem);
				}
				if (onUpdateItem != null) {
					onUpdateItem (currentItem);
				}
				return true;
			} else {
				for (int i = 0; i < this.m_ItemInventorySlots.Length; i++) {
					if (this.m_ItemInventorySlots [i] == null) {
						this.SetInventoryItem (i, value);
						this.m_ItemInventorySlots [i].SetCurrentAmount (value.GetCurrentAmount());
						if (OnEventUpdateItem != null) {
							OnEventUpdateItem (this.m_ItemInventorySlots [i]);
						}
						if (onAddItem != null) {
							onAddItem (this.m_ItemInventorySlots [i]);
						}
						return true;
					}
				}
			}
			return false;
		}

		public bool RemoveInventoryItem(IItem value) {
			var indexItem = Array.IndexOf (m_ItemInventorySlots, value);
			if (indexItem != -1) {
				this.m_ItemInventorySlots [indexItem] = null;
				if (OnEventUpdateItem != null) {
					OnEventUpdateItem (value);
				}
				return true;
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
			this.m_ItemInventorySlots [slot].SetInventorySlot (slot);
		}

		public IItem[] GetInventoryItems() {
			return this.m_ItemInventorySlots;
		}

		public void SetEquipmentItem(CEnum.EItemSlot slot, IItem value) {
			this.m_ItemEquipSlots [(int)slot] = value;
		}

		public IItem[] GetEquipmentItems() {
			return this.m_ItemEquipSlots;
		}

		public IItem GetFirstExecuteItem() {
			if (this.m_ExecuteItems.Count <= 0)
				return null;
			return this.m_ExecuteItems.Peek ();
		}
	
	}
}
