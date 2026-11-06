using System;
using System.ComponentModel;

namespace UnityEngine.ResourceManagement.ResourceProviders
{
	// Token: 0x02000040 RID: 64
	[DisplayName("JSON Asset Provider")]
	public class JsonAssetProvider : TextDataProvider
	{
		// Token: 0x0600019A RID: 410 RVA: 0x00006B9B File Offset: 0x00004D9B
		public override object Convert(Type type, string text)
		{
			return JsonUtility.FromJson(text, type);
		}
	}
}
