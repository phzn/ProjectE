using System;

namespace UnityEngine.AI
{
	// Token: 0x02000015 RID: 21
	public struct NavMeshLinkInstance
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600011D RID: 285 RVA: 0x00002E42 File Offset: 0x00001042
		public bool valid
		{
			get
			{
				return this.id != 0 && NavMesh.IsValidLinkHandle(this.id);
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00002E5A File Offset: 0x0000105A
		// (set) Token: 0x0600011F RID: 287 RVA: 0x00002E62 File Offset: 0x00001062
		internal int id { readonly get; set; }

		// Token: 0x06000120 RID: 288 RVA: 0x00002E6B File Offset: 0x0000106B
		public void Remove()
		{
			NavMesh.RemoveLinkInternal(this.id);
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000121 RID: 289 RVA: 0x00002E7C File Offset: 0x0000107C
		// (set) Token: 0x06000122 RID: 290 RVA: 0x00002E9C File Offset: 0x0000109C
		public Object owner
		{
			get
			{
				return NavMesh.InternalGetLinkOwner(this.id);
			}
			set
			{
				int num = ((value != null) ? value.GetInstanceID() : 0);
				bool flag = !NavMesh.InternalSetLinkOwner(this.id, num);
				if (flag)
				{
					Debug.LogError("Cannot set 'owner' on an invalid NavMeshLinkInstance");
				}
			}
		}
	}
}
