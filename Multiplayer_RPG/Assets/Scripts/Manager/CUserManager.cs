using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SurvivalTest {
	public class CUserManager : CMonoSingleton<CUserManager> {

		[SerializeField]	private CUserData m_CurrentUser;

		public string m_UserName;
		private string m_Password;

		private string m_URLLogin = "https://tamco-tinygame.rhcloud.com/login";
		private CWWW m_WWW;

		protected override void Awake ()
		{
			base.Awake ();
			DontDestroyOnLoad (this.gameObject);
			m_WWW = new CWWW ();
		}

		public void Login() {
			this.Login (m_UserName, m_Password);
		}

		public void UpdateUserName(InputField value) {
			m_UserName = value.text;
		}

		public void UpdatePassword(InputField value) {
			m_Password = value.text;
		}

		public virtual void OnClientLoginComplete(CSuccessResponse<CUserData> responce) {
			m_CurrentUser = responce.resultContent [0];
			CLog.Debug ("OnClientLoginComplete " + m_CurrentUser.token);
			CNetworkManager.Instance.OnClientLoginComplete (m_CurrentUser);
		}

		public virtual void OnClientLoginFail(CErrorResponse responce) {
			CLog.Error ("OnClientLoginFail " + responce.errorContent);
		}

		public virtual void Login(string userName, string userPassword) {
			if (string.IsNullOrEmpty (userName) == true || string.IsNullOrEmpty (userPassword)) {
				OnClientLoginFail (new CErrorResponse (0, "Field not empty."));
				return;
			}
			var fields = new Dictionary<string, string> ();
			fields.Add ("uname", userName);
			fields.Add ("upass", userPassword);
			fields.Add ("macadd", SystemInfo.deviceUniqueIdentifier);
			m_WWW.Post (m_URLLogin, fields, null, (result) => {
				if (result.IndexOf ("errorCode") != -1) {
					var errorResponse = TinyJSON.JSON.Load(result).Make<CErrorResponse>();
					OnClientLoginFail (errorResponse);
				} else {
					var successResponse = TinyJSON.JSON.Load(result).Make<CSuccessResponse<CUserData>>();
					OnClientLoginComplete (successResponse);
				}
			}, (error) => {
				if (error.IndexOf ("errorCode") != -1) {
					OnClientLoginFail (new CErrorResponse (0, error));
				} else {
					var errorResponse = TinyJSON.JSON.Load(error).Make<CErrorResponse>();
					OnClientLoginFail (errorResponse);
				}
			});
		}
	
	}
}
