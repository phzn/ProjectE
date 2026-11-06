using System;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace UnityEngine.ResourceManagement.Exceptions
{
	// Token: 0x02000011 RID: 17
	public class ProviderException : OperationException
	{
		// Token: 0x060000AA RID: 170 RVA: 0x00004125 File Offset: 0x00002325
		public ProviderException(string message, IResourceLocation location = null, Exception innerException = null)
			: base(message, innerException)
		{
			this.Location = location;
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00004136 File Offset: 0x00002336
		public IResourceLocation Location { get; }
	}
}
