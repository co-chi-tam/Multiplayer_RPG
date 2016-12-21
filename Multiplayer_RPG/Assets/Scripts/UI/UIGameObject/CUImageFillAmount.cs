using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	[RequireComponent(typeof(Image))]
	public class CUImageFillAmount : CBaseMonoBehaviour
	{
		public Image TargetImage;
		public Image SecondImage;

		public string ActionName;
		public IEventListener Target;

		protected override void Start ()
		{
			base.Start ();
			this.TargetImage.type = Image.Type.Filled;
			this.TargetImage.fillMethod = Image.FillMethod.Horizontal;
			this.TargetImage.fillOrigin = 0;
			this.TargetImage.fillAmount = 1f;
		}

		public void SetTarget(IEventListener target) {
			this.Target = target;
			this.Target.RemoveEventListener (ActionName, OnChangeStatus);
			this.Target.AddEventListener (ActionName, OnChangeStatus);
		}

		private void OnChangeStatus(object value) {
			this.TargetImage.fillAmount = (float)value;
			this.StopAllCoroutines ();
			this.StartCoroutine (HandleSecondImageFillAmount ((float)value));
		}

		private IEnumerator HandleSecondImageFillAmount(float value) {
			while (this.SecondImage.fillAmount != value) {
				this.SecondImage.fillAmount = Mathf.Lerp (this.SecondImage.fillAmount, value, 0.25f);
				yield return WaitHelper.WaitFixedUpdate;
			}
			this.SecondImage.fillAmount = value;
		}
	
	}
}

