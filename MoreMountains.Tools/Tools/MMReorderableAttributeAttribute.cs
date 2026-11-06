using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000130 RID: 304
	public class MMReorderableAttributeAttribute : PropertyAttribute
	{
		// Token: 0x060006D0 RID: 1744 RVA: 0x00021FE5 File Offset: 0x000201E5
		public MMReorderableAttributeAttribute()
			: this(null)
		{
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x00021FEE File Offset: 0x000201EE
		public MMReorderableAttributeAttribute(string elementNameProperty)
			: this(true, true, true, elementNameProperty, null, null)
		{
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x00021FFC File Offset: 0x000201FC
		public MMReorderableAttributeAttribute(string elementNameProperty, string elementIconPath)
			: this(true, true, true, elementNameProperty, null, elementIconPath)
		{
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x0002200A File Offset: 0x0002020A
		public MMReorderableAttributeAttribute(string elementNameProperty, string elementNameOverride, string elementIconPath)
			: this(true, true, true, elementNameProperty, elementNameOverride, elementIconPath)
		{
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x00022018 File Offset: 0x00020218
		public MMReorderableAttributeAttribute(bool add, bool remove, bool draggable, string elementNameProperty = null, string elementIconPath = null)
			: this(add, remove, draggable, elementNameProperty, null, elementIconPath)
		{
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x00022028 File Offset: 0x00020228
		public MMReorderableAttributeAttribute(bool add, bool remove, bool draggable, string elementNameProperty = null, string elementNameOverride = null, string elementIconPath = null)
		{
			this.add = add;
			this.remove = remove;
			this.draggable = draggable;
			this.elementNameProperty = elementNameProperty;
			this.elementNameOverride = elementNameOverride;
			this.elementIconPath = elementIconPath;
		}

		// Token: 0x0400087F RID: 2175
		public bool add;

		// Token: 0x04000880 RID: 2176
		public bool remove;

		// Token: 0x04000881 RID: 2177
		public bool draggable;

		// Token: 0x04000882 RID: 2178
		public bool singleLine;

		// Token: 0x04000883 RID: 2179
		public string elementNameProperty;

		// Token: 0x04000884 RID: 2180
		public string elementNameOverride;

		// Token: 0x04000885 RID: 2181
		public string elementIconPath;
	}
}
