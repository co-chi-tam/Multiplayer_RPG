using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CUIItemInfo : CBaseMonoBehaviour {

		[SerializeField]	private Text m_UIItemAmountText;
		[SerializeField]	private Image m_UIItemImage;

		private IItem m_Item;

		public void ExecuteObject() {
			if (m_Item != null) {
				m_Item.ExecuteObject ();
			}
		}
	
		public void SetItem(IItem value, Action<object> onExecuteObject) {
			m_Item = value;
			if (value != null) {
				m_Item.RemoveAllEventListener ("ExecuteObject");
				m_Item.AddEventListener ("ExecuteObject", onExecuteObject);
				m_UIItemAmountText.text = value.GetCurrentAmount ().ToString ();
				m_UIItemImage.sprite = CUtil.FindSprite (value.GetAvatar ());
			} else {
				m_UIItemAmountText.text = string.Empty;
				m_UIItemImage.sprite = CUtil.FindSprite ("UISprite");
			}
		}

	}
}
