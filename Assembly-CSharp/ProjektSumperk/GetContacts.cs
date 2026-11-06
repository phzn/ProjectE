using System;
using TMPro;
using UnityEngine;

namespace ProjektSumperk
{
	// Token: 0x020003E4 RID: 996
	public class GetContacts : MonoBehaviour
	{
		// Token: 0x06001972 RID: 6514 RVA: 0x000A125A File Offset: 0x0009F45A
		private void Start()
		{
			this.contactManager = Object.FindObjectOfType<ContactManager>();
		}

		// Token: 0x06001973 RID: 6515 RVA: 0x000A1267 File Offset: 0x0009F467
		public void GetContactOnClick()
		{
			this.names = base.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text;
			this.contactManager.GetContactName(this.names);
		}

		// Token: 0x04001A93 RID: 6803
		public string names;

		// Token: 0x04001A94 RID: 6804
		private ContactManager contactManager;
	}
}
