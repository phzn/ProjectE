using System;
using UnityEngine;

namespace ProjektSumperk
{
	// Token: 0x020003E5 RID: 997
	public class GetDialNumber : MonoBehaviour
	{
		// Token: 0x06001975 RID: 6517 RVA: 0x000A129B File Offset: 0x0009F49B
		private void Start()
		{
			this.callManager = Object.FindObjectOfType<CallManager>();
		}

		// Token: 0x06001976 RID: 6518 RVA: 0x000A12A8 File Offset: 0x0009F4A8
		public void GetNumberOnClick()
		{
			this.callManager.GetSetNumberText(this.num);
		}

		// Token: 0x04001A95 RID: 6805
		public string num;

		// Token: 0x04001A96 RID: 6806
		private CallManager callManager;
	}
}
