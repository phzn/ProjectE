using System;
using UnityEngine.Networking;

namespace UnityEngine.ResourceManagement
{
	// Token: 0x0200000C RID: 12
	public class WebRequestQueueOperation
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00003D8C File Offset: 0x00001F8C
		public bool IsDone
		{
			get
			{
				return this.m_Completed || this.Result != null;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00003DA1 File Offset: 0x00001FA1
		// (set) Token: 0x0600008F RID: 143 RVA: 0x00003DA9 File Offset: 0x00001FA9
		public UnityWebRequest WebRequest
		{
			get
			{
				return this.m_WebRequest;
			}
			internal set
			{
				this.m_WebRequest = value;
			}
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003DB2 File Offset: 0x00001FB2
		public WebRequestQueueOperation(UnityWebRequest request)
		{
			this.m_WebRequest = request;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003DC1 File Offset: 0x00001FC1
		internal void Complete(UnityWebRequestAsyncOperation asyncOp)
		{
			this.m_Completed = true;
			this.Result = asyncOp;
			Action<UnityWebRequestAsyncOperation> onComplete = this.OnComplete;
			if (onComplete == null)
			{
				return;
			}
			onComplete(this.Result);
		}

		// Token: 0x04000034 RID: 52
		private bool m_Completed;

		// Token: 0x04000035 RID: 53
		public UnityWebRequestAsyncOperation Result;

		// Token: 0x04000036 RID: 54
		public Action<UnityWebRequestAsyncOperation> OnComplete;

		// Token: 0x04000037 RID: 55
		internal UnityWebRequest m_WebRequest;
	}
}
