using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

namespace ProjektSumperk
{
	// Token: 0x020003E0 RID: 992
	public class EmailManager : MonoBehaviour
	{
		// Token: 0x06001966 RID: 6502 RVA: 0x000A0F3C File Offset: 0x0009F13C
		private void Awake()
		{
			this.dir = Application.dataPath + "/CompanionToolkit/Common/Data/JSON";
			this.path = Application.dataPath + "/CompanionToolkit/Common/Data/JSON/Email.json";
			if (!Directory.Exists(this.dir))
			{
				Directory.CreateDirectory(this.dir);
			}
			if (!File.Exists(this.path))
			{
				File.Create(this.path);
			}
			Debug.Log(this.path);
		}

		// Token: 0x06001967 RID: 6503 RVA: 0x000A0FB0 File Offset: 0x0009F1B0
		private void Start()
		{
			this.LoadEmailData();
			this.InstantiateEmailPrefabs();
		}

		// Token: 0x06001968 RID: 6504 RVA: 0x000A0FC0 File Offset: 0x0009F1C0
		private void LoadEmailData()
		{
			if (File.Exists(this.path))
			{
				EmailManager.EmailDataList emailDataList = JsonUtility.FromJson<EmailManager.EmailDataList>(File.ReadAllText(this.path));
				this.emails = ((emailDataList != null && emailDataList.emails != null) ? emailDataList.emails : new List<EmailManager.EmailData>());
				return;
			}
			Debug.LogError("Email data file not found: " + this.path);
		}

		// Token: 0x06001969 RID: 6505 RVA: 0x000A1020 File Offset: 0x0009F220
		private void SaveEmailData()
		{
			string text = JsonUtility.ToJson(new EmailManager.EmailDataList
			{
				emails = this.emails
			}, true);
			try
			{
				File.WriteAllText(this.path, text);
			}
			catch (Exception ex)
			{
				Debug.LogError("Error saving updated email data: " + ex.Message);
			}
		}

		// Token: 0x0600196A RID: 6506 RVA: 0x000A107C File Offset: 0x0009F27C
		private void InstantiateEmailPrefabs()
		{
			foreach (EmailManager.EmailData emailData in this.emails)
			{
				if (this.emailPrefab != null)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.emailPrefab, this.emailParent);
					gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = emailData.name;
					gameObject.transform.GetChild(2).GetComponent<TMP_Text>().text = emailData.msg;
					gameObject.transform.GetChild(3).GetComponent<TMP_Text>().text = emailData.date;
				}
				else
				{
					Debug.LogWarning("Email prefab not assigned.");
				}
			}
		}

		// Token: 0x0600196B RID: 6507 RVA: 0x000A114C File Offset: 0x0009F34C
		public void CreateNewEmail(string senderName, string emailContent, string emailDate)
		{
			EmailManager.EmailData emailData = new EmailManager.EmailData
			{
				name = senderName,
				msg = emailContent,
				date = emailDate
			};
			this.emails.Add(emailData);
			if (this.emailPrefab != null)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.emailPrefab, this.emailParent);
				gameObject.transform.SetAsFirstSibling();
				gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = emailData.name;
				gameObject.transform.GetChild(2).GetComponent<TMP_Text>().text = emailData.msg;
				gameObject.transform.GetChild(3).GetComponent<TMP_Text>().text = emailData.date;
			}
			this.emails.Insert(0, emailData);
			this.SaveEmailData();
		}

		// Token: 0x04001A88 RID: 6792
		public List<EmailManager.EmailData> emails;

		// Token: 0x04001A89 RID: 6793
		public Transform emailParent;

		// Token: 0x04001A8A RID: 6794
		public GameObject emailPrefab;

		// Token: 0x04001A8B RID: 6795
		private string path = "";

		// Token: 0x04001A8C RID: 6796
		private string dir = "";

		// Token: 0x020003E1 RID: 993
		[Serializable]
		public class EmailData
		{
			// Token: 0x04001A8D RID: 6797
			public string name;

			// Token: 0x04001A8E RID: 6798
			public string msg;

			// Token: 0x04001A8F RID: 6799
			public string date;
		}

		// Token: 0x020003E2 RID: 994
		[Serializable]
		public class EmailDataList
		{
			// Token: 0x04001A90 RID: 6800
			public List<EmailManager.EmailData> emails;
		}
	}
}
