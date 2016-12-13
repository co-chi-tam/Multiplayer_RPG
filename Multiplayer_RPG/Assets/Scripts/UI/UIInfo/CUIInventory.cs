using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CUIInventory : CBaseMonoBehaviour {

		[SerializeField]	private CUIItemInfo[] m_Items;

		public void LoadItems(IItem[] items) {
			for (int i = 0; i < m_Items.Length; i++) {
				if (items[i] != null) {
					m_Items [i].SetAmountText (items [i].GetCurrentAmount ().ToString());
					m_Items [i].SetImage (items [i].GetAvatar ());
				}
			}
		}
	
	}
}
