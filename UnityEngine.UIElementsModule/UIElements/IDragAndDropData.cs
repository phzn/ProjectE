using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x02000166 RID: 358
	internal interface IDragAndDropData
	{
		// Token: 0x06000BB0 RID: 2992
		object GetGenericData(string key);

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000BB1 RID: 2993
		object userData { get; }

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000BB2 RID: 2994
		IEnumerable<Object> unityObjectReferences { get; }
	}
}
