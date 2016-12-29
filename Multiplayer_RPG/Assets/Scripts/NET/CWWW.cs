using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CWWW {

		#region Properties

		private bool m_Handling = false;
		private float m_TimeOut = 30f;
		private Dictionary<string, string> m_Header;

		#endregion

		#region Contructor

		public CWWW ()
		{
			m_Header = new Dictionary<string, string> ();
			m_Header.Add ("verify", "TinyGame");
		}

		#endregion

		#region Handle Coroutine

		public void HandleCoroutine(IEnumerator handleObject, Action complete = null) {
			CHandleEvent.Instance.AddEvent (handleObject, complete);
		}

		#endregion

		#region GET

		public void Get(string path, Action<float> processing, Action<string> complete, Action<string> error) {
			CHandleEvent.Instance.AddEvent (HandleWWW (path, null, processing, complete, error));
		}

		public void Get(string path, Action<string> complete, Action<string> error) {
			Get(path, null, complete, error);
		}

		public void Get(string path, Action<string> complete) {
			Get(path, null, complete, null);
		}

		#endregion

		#region POST

		public void Post(string path, Dictionary<string, string> param, Action<float> processing, Action<string> complete, Action<string> error) {
			var formWWW = new WWWForm ();
			foreach (var item in param) {
				formWWW.AddField (item.Key, item.Value);
			}
			CHandleEvent.Instance.AddEvent (HandleWWW (path, formWWW, processing, complete, error));
		}

		public void Post(string path, Dictionary<string, string> param, Action<string> complete, Action<string> error) {
			Post(path, null, complete, error);
		}

		public void Post(string path, Dictionary<string, string> param, Action<string> complete) {
			Post(path, null, complete, null);
		}
	
		#endregion

		#region WWW

		public IEnumerator HandleWWW(string path, WWWForm formWWW, Action<float> processing, Action<string> complete, Action<string> error) {
			if (m_Handling == true)
				yield break;
			m_Handling = true;
			WWW www = null;
			if (formWWW != null) {
				www = new WWW (path, formWWW.data, m_Header);
			} else {
				www = new WWW (path, null, m_Header);
			}
			var timeProcessing = 0f;
			while (www.isDone == false && timeProcessing <= m_TimeOut) {
				if (processing != null) {
					processing (timeProcessing);
				}
				timeProcessing += Time.deltaTime;
				yield return WaitHelper.WaitFixedUpdate;
			}
			if (www.isDone == false && timeProcessing >= m_TimeOut) {
				if (error != null) {
					error ("Request time out....");
				}
				m_Handling = false;
				yield break;
			}
			yield return www;
			if (string.IsNullOrEmpty (www.error) == false) {
				if (error != null) {
					error (www.error);
				}
			} else {
				if (complete != null) {
					complete (www.text);
				}
			}
			m_Handling = false;
			www.Dispose ();
		}

		#endregion

	}
}