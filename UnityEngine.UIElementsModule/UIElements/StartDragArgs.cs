using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x02000170 RID: 368
	internal struct StartDragArgs
	{
		// Token: 0x06000BDD RID: 3037 RVA: 0x0002E74B File Offset: 0x0002C94B
		public StartDragArgs(string title, DragVisualMode visualMode)
		{
			this.title = title;
			this.visualMode = visualMode;
			this.genericData = null;
			this.unityObjectReferences = null;
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x0002E76C File Offset: 0x0002C96C
		internal StartDragArgs(string title, object target)
		{
			this.title = title;
			this.visualMode = DragVisualMode.Move;
			this.genericData = null;
			this.unityObjectReferences = null;
			this.SetGenericData("__unity-drag-and-drop__source-view", target);
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000BDF RID: 3039 RVA: 0x0002E79A File Offset: 0x0002C99A
		public readonly string title { get; }

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000BE0 RID: 3040 RVA: 0x0002E7A2 File Offset: 0x0002C9A2
		public readonly DragVisualMode visualMode { get; }

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000BE1 RID: 3041 RVA: 0x0002E7AA File Offset: 0x0002C9AA
		// (set) Token: 0x06000BE2 RID: 3042 RVA: 0x0002E7B2 File Offset: 0x0002C9B2
		internal Hashtable genericData { readonly get; private set; }

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000BE3 RID: 3043 RVA: 0x0002E7BB File Offset: 0x0002C9BB
		// (set) Token: 0x06000BE4 RID: 3044 RVA: 0x0002E7C3 File Offset: 0x0002C9C3
		internal IEnumerable<Object> unityObjectReferences { readonly get; private set; }

		// Token: 0x06000BE5 RID: 3045 RVA: 0x0002E7CC File Offset: 0x0002C9CC
		public void SetGenericData(string key, object data)
		{
			if (this.genericData == null)
			{
				this.genericData = new Hashtable();
			}
			this.genericData[key] = data;
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x0002E7FE File Offset: 0x0002C9FE
		public void SetUnityObjectReferences(IEnumerable<Object> references)
		{
			this.unityObjectReferences = references;
		}
	}
}
