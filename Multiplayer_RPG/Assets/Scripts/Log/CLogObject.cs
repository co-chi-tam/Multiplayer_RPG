using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SurvivalTest;

namespace LogTest {
	public class CLogObject : CMonoSingleton<CLogObject> {

		public enum ELogType: int {
			Debug = 0,
			Warning = 1,
			Error = 2
		}

		protected const string LINE_FORMAT = "\n{0}.{1}-{2}:{3}";
		protected float m_WWidth;
		protected float m_WHeight;
		protected StringBuilder m_LogString;
		protected bool m_ShowWindow = false;

		private Rect m_WindowRect;
		private Vector2 m_ScrollPositionTextArea;

		protected override void Awake ()
		{
			base.Awake ();
			m_LogString = new StringBuilder ();
			m_WWidth = Screen.width - 40f;
			m_WHeight = Screen.height - 40f;
			m_WindowRect = new Rect (20, 20, m_WWidth, m_WHeight);

			Application.logMessageReceived += this.EventLog;
		}

		protected virtual void OnGUI() {
			if (m_ShowWindow) {
				m_WindowRect = GUILayout.Window (0, m_WindowRect, DrawWindow, "Debug Log");
			}
		}

		private void DrawWindow(int id) {
			GUILayout.BeginVertical ();
			GUILayout.BeginHorizontal ();
			m_ScrollPositionTextArea = GUILayout.BeginScrollView (m_ScrollPositionTextArea);
			GUILayout.TextArea (m_LogString.ToString());
			GUILayout.EndScrollView ();
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("OK")) {
				m_ShowWindow = false;
			}
			if (GUILayout.Button ("Clear")) {
				Clear ();
			}
			GUILayout.EndHorizontal ();
			GUILayout.EndVertical ();
			GUI.DragWindow ();
		}

		public void EventLog(string logString, string stackTrace, LogType type) {
			switch (type) {
			default:
			case LogType.Log:
				this.ShowLog (ELogType.Debug, stackTrace, logString);
				break;
			case LogType.Warning:
				this.ShowLog (ELogType.Warning, stackTrace, logString);
				break;
			case LogType.Error:
				this.ShowLog (ELogType.Error, stackTrace, logString);
				break;
			}
		}

		public void ShowLog(ELogType type, string callerName, string value) {
			m_LogString.Append (string.Format(LINE_FORMAT, DateTime.Now.ToString("dd/MM/yyyy HH:mm"), type.ToString(), callerName, value));
			m_ShowWindow = true;
		}

		public void ShowLogFormat(ELogType type, string pattern, string callerName, params object[] param) {
			this.ShowLog (type, callerName, string.Format(pattern, param));
		}

		public void Clear() {
			m_LogString = new StringBuilder ();
		}
	
	}
}


