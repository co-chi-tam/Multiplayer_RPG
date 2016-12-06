using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CUIObjectInfo : CBaseMonoBehaviour {

		[SerializeField]	private Text m_UIObjNameText;
		[SerializeField]	private Image m_UIObjHPImage; 
	
		public IStatus owner;

		protected override void LateUpdate ()
		{
			base.LateUpdate ();
			if (owner != null && owner.GetActive () && owner.GetIsVisible ()) {
				// Info
				this.name = "UIInfo-" + owner.GetName ();
				m_UIObjNameText.text = owner.GetName ();
				m_UIObjHPImage.fillAmount = (float)owner.GetCurrentHealth () / owner.GetMaxHealth ();
				// Position
				var ownerPosition = owner.GetPosition ();
				ownerPosition.y = owner.GetHeight () * 2f;
				var screenPosition = Camera.main.WorldToScreenPoint (ownerPosition);
				m_Transform.position = screenPosition;
			} else {
				DestroyImmediate (this.gameObject);
			}
		}
	}
}
