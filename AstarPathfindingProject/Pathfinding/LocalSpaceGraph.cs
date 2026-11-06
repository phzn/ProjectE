using System;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000079 RID: 121
	[HelpURL("https://arongranberg.com/astar/documentation/stable/localspacegraph.html")]
	public class LocalSpaceGraph : VersionedMonoBehaviour
	{
		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000458 RID: 1112 RVA: 0x0001521D File Offset: 0x0001341D
		public GraphTransform transformation
		{
			get
			{
				return this.graphTransform;
			}
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00015225 File Offset: 0x00013425
		private void Start()
		{
			this.originalMatrix = base.transform.worldToLocalMatrix;
			base.transform.hasChanged = true;
			this.Refresh();
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0001524A File Offset: 0x0001344A
		public void Refresh()
		{
			if (base.transform.hasChanged)
			{
				this.graphTransform.SetMatrix(base.transform.localToWorldMatrix * this.originalMatrix);
				base.transform.hasChanged = false;
			}
		}

		// Token: 0x040002B4 RID: 692
		private Matrix4x4 originalMatrix;

		// Token: 0x040002B5 RID: 693
		private MutableGraphTransform graphTransform = new MutableGraphTransform(Matrix4x4.identity);
	}
}
