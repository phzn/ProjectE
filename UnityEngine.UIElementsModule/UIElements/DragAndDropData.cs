using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x02000167 RID: 359
	internal abstract class DragAndDropData : IDragAndDropData
	{
		// Token: 0x06000BB3 RID: 2995
		public abstract object GetGenericData(string key);

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000BB4 RID: 2996 RVA: 0x0002DDB7 File Offset: 0x0002BFB7
		object IDragAndDropData.userData
		{
			get
			{
				return this.GetGenericData("__unity-drag-and-drop__source-view");
			}
		}

		// Token: 0x06000BB5 RID: 2997
		public abstract void SetGenericData(string key, object data);

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000BB6 RID: 2998
		public abstract object source { get; }

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000BB7 RID: 2999
		public abstract DragVisualMode visualMode { get; }

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000BB8 RID: 3000
		public abstract IEnumerable<Object> unityObjectReferences { get; }

		// Token: 0x0400057F RID: 1407
		internal const string dragSourceKey = "__unity-drag-and-drop__source-view";
	}
}
