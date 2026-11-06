using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

namespace ProjektSumperk
{
	// Token: 0x020003F4 RID: 1012
	public class MessageManager : MonoBehaviour
	{
		// Token: 0x060019B0 RID: 6576 RVA: 0x000A2DD4 File Offset: 0x000A0FD4
		private void Awake()
		{
			this.dir = Application.dataPath + "/CompanionToolkit/Common/Data/JSON";
			this.path = Application.dataPath + "/CompanionToolkit/Common/Data/JSON/Message.json";
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

		// Token: 0x060019B1 RID: 6577 RVA: 0x000A2E48 File Offset: 0x000A1048
		private void Start()
		{
			this.LoadMessageData();
			this.InstantiateMessagePrefabs();
		}

		// Token: 0x060019B2 RID: 6578 RVA: 0x000A2E58 File Offset: 0x000A1058
		private void LoadMessageData()
		{
			if (File.Exists(this.path))
			{
				MessageManager.MessageDataList messageDataList = JsonUtility.FromJson<MessageManager.MessageDataList>(File.ReadAllText(this.path));
				this.messages = ((messageDataList != null && messageDataList.messages != null) ? messageDataList.messages : new List<MessageManager.MessageData>());
				return;
			}
			Debug.LogError("Message data file not found: " + this.path);
		}

		// Token: 0x060019B3 RID: 6579 RVA: 0x000A2EB8 File Offset: 0x000A10B8
		private void SaveMessageData()
		{
			string text = JsonUtility.ToJson(new MessageManager.MessageDataList
			{
				messages = this.messages
			}, true);
			try
			{
				File.WriteAllText(this.path, text);
			}
			catch (Exception ex)
			{
				Debug.LogError("Error saving updated message data: " + ex.Message);
			}
		}

		// Token: 0x060019B4 RID: 6580 RVA: 0x000A2F14 File Offset: 0x000A1114
		private void InstantiateMessagePrefabs()
		{
			foreach (MessageManager.MessageData messageData in this.messages)
			{
				if (this.messagePrefab != null)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.messagePrefab, this.messageParent);
					gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = messageData.name;
					gameObject.transform.GetChild(2).GetComponent<TMP_Text>().text = messageData.msg;
					gameObject.transform.GetChild(3).GetComponent<TMP_Text>().text = messageData.date;
				}
				else
				{
					Debug.LogWarning("Message prefab not assigned.");
				}
			}
		}

		// Token: 0x060019B5 RID: 6581 RVA: 0x000A2FE4 File Offset: 0x000A11E4
		public void CreateNewMessage(string senderName, string messageContent, string messageDate)
		{
			MessageManager.MessageData messageData = new MessageManager.MessageData
			{
				name = senderName,
				msg = messageContent,
				date = messageDate
			};
			this.messages.Add(messageData);
			if (this.messagePrefab != null)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.messagePrefab, this.messageParent);
				gameObject.transform.SetAsFirstSibling();
				gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = messageData.name;
				gameObject.transform.GetChild(2).GetComponent<TMP_Text>().text = messageData.msg;
				gameObject.transform.GetChild(3).GetComponent<TMP_Text>().text = messageData.date;
			}
			this.messages.Insert(0, messageData);
			this.SaveMessageData();
		}

		// Token: 0x04001ADF RID: 6879
		public List<MessageManager.MessageData> messages;

		// Token: 0x04001AE0 RID: 6880
		public Transform messageParent;

		// Token: 0x04001AE1 RID: 6881
		public GameObject messagePrefab;

		// Token: 0x04001AE2 RID: 6882
		private string path = "";

		// Token: 0x04001AE3 RID: 6883
		private string dir = "";

		// Token: 0x020003F5 RID: 1013
		[Serializable]
		public class MessageData
		{
			// Token: 0x04001AE4 RID: 6884
			public string name;

			// Token: 0x04001AE5 RID: 6885
			public string msg;

			// Token: 0x04001AE6 RID: 6886
			public string date;
		}

		// Token: 0x020003F6 RID: 1014
		[Serializable]
		public class MessageDataList
		{
			// Token: 0x04001AE7 RID: 6887
			public List<MessageManager.MessageData> messages;
		}
	}
}
