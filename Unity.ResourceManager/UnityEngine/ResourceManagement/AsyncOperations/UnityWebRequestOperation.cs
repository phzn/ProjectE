using System;
using UnityEngine.Networking;

namespace UnityEngine.ResourceManagement.AsyncOperations
{
	// Token: 0x02000060 RID: 96
	internal class UnityWebRequestOperation : AsyncOperationBase<UnityWebRequest>
	{
		// Token: 0x060002F5 RID: 757 RVA: 0x00009F2B File Offset: 0x0000812B
		public UnityWebRequestOperation(UnityWebRequest webRequest)
		{
			this.m_UWR = webRequest;
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x00009F3A File Offset: 0x0000813A
		protected override void Execute()
		{
			this.m_UWR.SendWebRequest().completed += delegate(AsyncOperation request)
			{
				base.Complete(this.m_UWR, string.IsNullOrEmpty(this.m_UWR.error), this.m_UWR.error);
			};
		}

		// Token: 0x0400011E RID: 286
		private UnityWebRequest m_UWR;
	}
}
