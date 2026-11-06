using System;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;

namespace ProjektSumperk
{
	// Token: 0x020003D6 RID: 982
	public class ContactManager : MonoBehaviour
	{
		// Token: 0x06001919 RID: 6425 RVA: 0x0009F638 File Offset: 0x0009D838
		private void Awake()
		{
			this.dir = Application.dataPath + "/CompanionToolkit/Common/Data/JSON";
			this.path = Application.dataPath + "/CompanionToolkit/Common/Data/JSON/Contacts.json";
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

		// Token: 0x0600191A RID: 6426 RVA: 0x0009F6AC File Offset: 0x0009D8AC
		private void Start()
		{
			byte[] array = File.ReadAllBytes(this.path);
			if (Encoding.UTF8.GetString(array) == "")
			{
				File.WriteAllText(this.path, "{}");
				this.contactData.name = "James";
				this.contactData.number = "8888888888";
				this.contactDatas.users.Add(this.contactData);
				string text = JsonUtility.ToJson(this.contactDatas);
				File.WriteAllText(this.path, text);
			}
			this.LoadList();
		}

		// Token: 0x0600191B RID: 6427 RVA: 0x0009E55C File Offset: 0x0009C75C
		public void LoadList()
		{
			base.Invoke("GetData", 0.1f);
		}

		// Token: 0x0600191C RID: 6428 RVA: 0x0009F740 File Offset: 0x0009D940
		private void GetData()
		{
			try
			{
				byte[] array = File.ReadAllBytes(this.path);
				string @string = Encoding.UTF8.GetString(array);
				this.contactDatas = JsonUtility.FromJson<ContactDatas>(@string);
				foreach (ContactData contactData in this.contactDatas.users)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.contactPrefab);
					gameObject.transform.SetParent(this.parent, false);
					gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
					gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
					gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = contactData.name;
					gameObject.transform.GetChild(1).GetComponent<TMP_Text>().text = contactData.number;
					gameObject.transform.GetChild(2).GetChild(0).GetComponent<TMP_Text>()
						.text = contactData.name.Substring(0, 1);
				}
			}
			catch
			{
				Debug.Log("Message Error");
			}
		}

		// Token: 0x0600191D RID: 6429 RVA: 0x0009F8AC File Offset: 0x0009DAAC
		public void GetContactName(string name)
		{
			this.Dialing.SetActive(false);
			Debug.Log(name);
			this.mCall.SetActive(true);
			this.Calling.SetActive(true);
			this.callManager.GetComponent<CallManager>().StartCallFromContacts(name);
			this.callManager.GetComponent<CallManager>().isCallFromContacts = true;
		}

		// Token: 0x04001A2E RID: 6702
		[SerializeField]
		private ContactData contactData = new ContactData();

		// Token: 0x04001A2F RID: 6703
		public ContactDatas contactDatas = new ContactDatas();

		// Token: 0x04001A30 RID: 6704
		public GameObject contactPrefab;

		// Token: 0x04001A31 RID: 6705
		public Transform parent;

		// Token: 0x04001A32 RID: 6706
		public GameObject callManager;

		// Token: 0x04001A33 RID: 6707
		public GameObject Calling;

		// Token: 0x04001A34 RID: 6708
		public GameObject Dialing;

		// Token: 0x04001A35 RID: 6709
		public GameObject mCall;

		// Token: 0x04001A36 RID: 6710
		private string path = "";

		// Token: 0x04001A37 RID: 6711
		private string dir = "";
	}
}
