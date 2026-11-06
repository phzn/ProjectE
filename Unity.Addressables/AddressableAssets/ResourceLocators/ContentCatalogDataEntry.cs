using System;
using System.Collections.Generic;

namespace UnityEngine.AddressableAssets.ResourceLocators
{
	// Token: 0x02000020 RID: 32
	public class ContentCatalogDataEntry
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000193 RID: 403 RVA: 0x00007620 File Offset: 0x00005820
		// (set) Token: 0x06000194 RID: 404 RVA: 0x00007628 File Offset: 0x00005828
		public string InternalId { get; set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000195 RID: 405 RVA: 0x00007631 File Offset: 0x00005831
		// (set) Token: 0x06000196 RID: 406 RVA: 0x00007639 File Offset: 0x00005839
		public string Provider { get; private set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000197 RID: 407 RVA: 0x00007642 File Offset: 0x00005842
		// (set) Token: 0x06000198 RID: 408 RVA: 0x0000764A File Offset: 0x0000584A
		public List<object> Keys { get; private set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00007653 File Offset: 0x00005853
		// (set) Token: 0x0600019A RID: 410 RVA: 0x0000765B File Offset: 0x0000585B
		public List<object> Dependencies { get; private set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600019B RID: 411 RVA: 0x00007664 File Offset: 0x00005864
		// (set) Token: 0x0600019C RID: 412 RVA: 0x0000766C File Offset: 0x0000586C
		public object Data { get; set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600019D RID: 413 RVA: 0x00007675 File Offset: 0x00005875
		// (set) Token: 0x0600019E RID: 414 RVA: 0x0000767D File Offset: 0x0000587D
		public Type ResourceType { get; private set; }

		// Token: 0x0600019F RID: 415 RVA: 0x00007688 File Offset: 0x00005888
		public ContentCatalogDataEntry(Type type, string internalId, string provider, IEnumerable<object> keys, IEnumerable<object> dependencies = null, object extraData = null)
		{
			this.InternalId = internalId;
			this.Provider = provider;
			this.ResourceType = type;
			this.Keys = new List<object>(keys);
			this.Dependencies = ((dependencies == null) ? new List<object>() : new List<object>(dependencies));
			this.Data = extraData;
		}
	}
}
