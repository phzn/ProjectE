using System;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.Util;

namespace UnityEngine.ResourceManagement.Exceptions
{
	// Token: 0x02000012 RID: 18
	public class RemoteProviderException : ProviderException
	{
		// Token: 0x060000AC RID: 172 RVA: 0x0000413E File Offset: 0x0000233E
		public RemoteProviderException(string message, IResourceLocation location = null, UnityWebRequestResult uwrResult = null, Exception innerException = null)
			: base(message, location, innerException)
		{
			this.WebRequestResult = uwrResult;
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00004151 File Offset: 0x00002351
		public override string Message
		{
			get
			{
				return this.ToString();
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00004159 File Offset: 0x00002359
		public UnityWebRequestResult WebRequestResult { get; }

		// Token: 0x060000AF RID: 175 RVA: 0x00004164 File Offset: 0x00002364
		public override string ToString()
		{
			if (this.WebRequestResult != null)
			{
				return string.Format("{0} : {1}\nUnityWebRequest result : {2}\n{3}", new object[]
				{
					base.GetType().Name,
					base.Message,
					this.WebRequestResult,
					base.InnerException
				});
			}
			return base.ToString();
		}
	}
}
