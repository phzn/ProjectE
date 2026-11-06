using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

namespace ProjektSumperk
{
	// Token: 0x02000401 RID: 1025
	public class QuestManager : MonoBehaviour
	{
		// Token: 0x06001A0A RID: 6666 RVA: 0x000A5370 File Offset: 0x000A3570
		private void Awake()
		{
			this.dir = Application.dataPath + "/CompanionToolkit/Common/Data/JSON";
			this.path = Application.dataPath + "/CompanionToolkit/Common/Data/JSON/Quests.json";
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

		// Token: 0x06001A0B RID: 6667 RVA: 0x000A53E4 File Offset: 0x000A35E4
		private void Start()
		{
			this.LoadQuestData();
			this.InstantiateQuestPrefabs();
		}

		// Token: 0x06001A0C RID: 6668 RVA: 0x000A53F4 File Offset: 0x000A35F4
		private void LoadQuestData()
		{
			if (File.Exists(this.path))
			{
				QuestManager.QuestDataList questDataList = JsonUtility.FromJson<QuestManager.QuestDataList>(File.ReadAllText(this.path));
				this.quests = ((questDataList != null && questDataList.quests != null) ? questDataList.quests : new List<QuestManager.QuestData>());
				return;
			}
			Debug.LogError("Quest data file not found: " + this.path);
		}

		// Token: 0x06001A0D RID: 6669 RVA: 0x000A5454 File Offset: 0x000A3654
		private void SaveQuestData()
		{
			string text = JsonUtility.ToJson(new QuestManager.QuestDataList
			{
				quests = this.quests
			}, true);
			try
			{
				File.WriteAllText(this.path, text);
			}
			catch (Exception ex)
			{
				Debug.LogError("Error saving updated quest data: " + ex.Message);
			}
		}

		// Token: 0x06001A0E RID: 6670 RVA: 0x000A54B0 File Offset: 0x000A36B0
		private QuestManager.QuestData FindQuestByTitle(string title)
		{
			foreach (QuestManager.QuestData questData in this.quests)
			{
				if (questData.title == title)
				{
					return questData;
				}
			}
			return null;
		}

		// Token: 0x06001A0F RID: 6671 RVA: 0x000A5514 File Offset: 0x000A3714
		public void UpdateQuestStatus(string title, string newStatus)
		{
			QuestManager.QuestData questData = this.FindQuestByTitle(title);
			if (questData != null)
			{
				questData.status = newStatus;
				this.SaveQuestData();
				return;
			}
			Debug.LogWarning("Quest with title " + title + " not found.");
		}

		// Token: 0x06001A10 RID: 6672 RVA: 0x00004F31 File Offset: 0x00003131
		public void UpdateQuest()
		{
		}

		// Token: 0x06001A11 RID: 6673 RVA: 0x000A5550 File Offset: 0x000A3750
		private void InstantiateQuestPrefabs()
		{
			foreach (QuestManager.QuestData questData in this.quests)
			{
				if (this.questPrefab != null)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.questPrefab, this.questParent);
					gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = questData.title;
					gameObject.transform.GetChild(1).GetComponent<TMP_Text>().text = questData.type;
					gameObject.transform.GetChild(3).GetComponent<TMP_Text>().text = questData.location;
					gameObject.transform.GetChild(4).GetComponent<TMP_Text>().text = questData.description;
					gameObject.transform.GetChild(5).GetComponent<TMP_Text>().text = "Reward " + questData.xpReward.ToString() + " XP";
					string status = questData.status;
					if (!(status == "Completed"))
					{
						if (!(status == "Tracking"))
						{
							if (status == "Not Started")
							{
								gameObject.transform.GetChild(6).gameObject.SetActive(true);
								gameObject.transform.GetChild(7).gameObject.SetActive(false);
								gameObject.transform.GetChild(8).gameObject.SetActive(false);
							}
						}
						else
						{
							gameObject.transform.GetChild(6).gameObject.SetActive(false);
							gameObject.transform.GetChild(7).gameObject.SetActive(true);
							gameObject.transform.GetChild(8).gameObject.SetActive(false);
						}
					}
					else
					{
						gameObject.transform.GetChild(6).gameObject.SetActive(false);
						gameObject.transform.GetChild(7).gameObject.SetActive(false);
						gameObject.transform.GetChild(8).gameObject.SetActive(true);
					}
				}
				else
				{
					Debug.LogWarning("Quest prefab not assigned.");
				}
			}
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x000A578C File Offset: 0x000A398C
		public void CreateNewQuest(string title, string type, string description, int xpReward, string location)
		{
			QuestManager.QuestData questData = new QuestManager.QuestData
			{
				title = title,
				type = type,
				description = description,
				status = "Not Started",
				xpReward = xpReward,
				location = location
			};
			this.quests.Add(questData);
			if (this.questPrefab != null)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.questPrefab, this.questParent);
				gameObject.transform.SetAsFirstSibling();
				gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = questData.title;
				gameObject.transform.GetChild(1).GetComponent<TMP_Text>().text = questData.type;
				gameObject.transform.GetChild(3).GetComponent<TMP_Text>().text = questData.location;
				gameObject.transform.GetChild(4).GetComponent<TMP_Text>().text = questData.description;
				gameObject.transform.GetChild(5).GetComponent<TMP_Text>().text = "Reward " + questData.xpReward.ToString() + " XP";
				gameObject.transform.GetChild(6).gameObject.SetActive(true);
				gameObject.transform.GetChild(7).gameObject.SetActive(false);
				gameObject.transform.GetChild(8).gameObject.SetActive(false);
			}
			this.quests.Insert(0, questData);
			this.SaveQuestData();
		}

		// Token: 0x04001B2E RID: 6958
		public List<QuestManager.QuestData> quests;

		// Token: 0x04001B2F RID: 6959
		public Transform questParent;

		// Token: 0x04001B30 RID: 6960
		public GameObject questPrefab;

		// Token: 0x04001B31 RID: 6961
		private string path = "";

		// Token: 0x04001B32 RID: 6962
		private string dir = "";

		// Token: 0x02000402 RID: 1026
		[Serializable]
		public class QuestData
		{
			// Token: 0x04001B33 RID: 6963
			public string title;

			// Token: 0x04001B34 RID: 6964
			public string type;

			// Token: 0x04001B35 RID: 6965
			public string description;

			// Token: 0x04001B36 RID: 6966
			public string status;

			// Token: 0x04001B37 RID: 6967
			public int xpReward;

			// Token: 0x04001B38 RID: 6968
			public string location;
		}

		// Token: 0x02000403 RID: 1027
		[Serializable]
		public class QuestDataList
		{
			// Token: 0x04001B39 RID: 6969
			public List<QuestManager.QuestData> quests;
		}
	}
}
