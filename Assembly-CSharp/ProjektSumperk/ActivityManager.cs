using System;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;

namespace ProjektSumperk
{
	// Token: 0x020003CD RID: 973
	public class ActivityManager : MonoBehaviour
	{
		// Token: 0x060018E8 RID: 6376 RVA: 0x0009E444 File Offset: 0x0009C644
		private void Awake()
		{
			this.dir = Application.dataPath + "/CompanionToolkit/Common/Data/JSON";
			this.file = Application.dataPath + "/CompanionToolkit/Common/Data/JSON/Activity.json";
			if (!Directory.Exists(this.dir))
			{
				Directory.CreateDirectory(this.dir);
			}
			if (!File.Exists(this.file))
			{
				File.Create(this.file);
			}
			Debug.Log(this.file);
		}

		// Token: 0x060018E9 RID: 6377 RVA: 0x0009E4B8 File Offset: 0x0009C6B8
		private void Start()
		{
			byte[] array = File.ReadAllBytes(this.file);
			if (Encoding.UTF8.GetString(array) == "")
			{
				File.WriteAllText(this.file, "{}");
				this.activityData.title = "James";
				this.activityData.msg = "Hello! How are you doing?";
				this.activityData.time = "6th Jun";
				this.activityDatas.activity.Add(this.activityData);
				string text = JsonUtility.ToJson(this.activityDatas);
				File.WriteAllText(this.file, text);
			}
			this.LoadList();
		}

		// Token: 0x060018EA RID: 6378 RVA: 0x0009E55C File Offset: 0x0009C75C
		public void LoadList()
		{
			base.Invoke("GetData", 0.1f);
		}

		// Token: 0x060018EB RID: 6379 RVA: 0x0009E570 File Offset: 0x0009C770
		private void GetData()
		{
			try
			{
				byte[] array = File.ReadAllBytes(this.file);
				string @string = Encoding.UTF8.GetString(array);
				this.activityDatas = JsonUtility.FromJson<ActivityDatas>(@string);
				this.activityDatas.activity.Reverse();
				foreach (ActivityData activityData in this.activityDatas.activity)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.activityPrefab);
					gameObject.transform.SetParent(this.parent, false);
					gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
					gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
					gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = activityData.title;
					gameObject.transform.GetChild(1).GetComponent<TMP_Text>().text = activityData.msg;
					gameObject.transform.GetChild(2).GetComponent<TMP_Text>().text = activityData.time;
				}
			}
			catch
			{
				Debug.Log("Message Error");
			}
		}

		// Token: 0x060018EC RID: 6380 RVA: 0x0009E6DC File Offset: 0x0009C8DC
		public void AddNewActivity(string title, string msg, string time)
		{
			this.SaveIntoJson(title, msg, time);
		}

		// Token: 0x060018ED RID: 6381 RVA: 0x0009E6E8 File Offset: 0x0009C8E8
		public void SaveIntoJson(string title, string msg, string time)
		{
			try
			{
				this.activityData.title = title;
				this.activityData.msg = msg;
				this.activityData.time = DateTime.Now.ToString();
				this.activityDatas.activity.Add(this.activityData);
				string text = JsonUtility.ToJson(this.activityDatas);
				File.WriteAllText(this.file, text);
				GameObject gameObject = Object.Instantiate<GameObject>(this.activityPrefab, base.transform.position, base.transform.rotation);
				gameObject.transform.SetParent(this.parent, false);
				gameObject.transform.SetAsFirstSibling();
				gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = title;
				gameObject.transform.GetChild(1).GetComponent<TMP_Text>().text = msg;
				gameObject.transform.GetChild(2).GetComponent<TMP_Text>().text = time;
				byte[] array = File.ReadAllBytes(this.file);
				string @string = Encoding.UTF8.GetString(array);
				this.activityDatas = JsonUtility.FromJson<ActivityDatas>(@string);
			}
			catch
			{
				Debug.Log("Message Error");
			}
		}

		// Token: 0x060018EE RID: 6382 RVA: 0x0009E824 File Offset: 0x0009CA24
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.B))
			{
				this.AddNewActivity("James", "Hello! James how are you? go collect some coins.", DateTime.Now.ToString());
			}
		}

		// Token: 0x040019E8 RID: 6632
		[SerializeField]
		private ActivityData activityData = new ActivityData();

		// Token: 0x040019E9 RID: 6633
		public ActivityDatas activityDatas = new ActivityDatas();

		// Token: 0x040019EA RID: 6634
		public GameObject activityPrefab;

		// Token: 0x040019EB RID: 6635
		public Transform parent;

		// Token: 0x040019EC RID: 6636
		private string file = "";

		// Token: 0x040019ED RID: 6637
		private string dir = "";
	}
}
