using System;

namespace UnityEngine.AI
{
	// Token: 0x02000013 RID: 19
	public struct NavMeshDataInstance
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00002CA6 File Offset: 0x00000EA6
		public bool valid
		{
			get
			{
				return this.id != 0 && NavMesh.IsValidNavMeshDataHandle(this.id);
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00002CBE File Offset: 0x00000EBE
		// (set) Token: 0x0600010B RID: 267 RVA: 0x00002CC6 File Offset: 0x00000EC6
		internal int id { readonly get; set; }

		// Token: 0x0600010C RID: 268 RVA: 0x00002CCF File Offset: 0x00000ECF
		public void Remove()
		{
			NavMesh.RemoveNavMeshDataInternal(this.id);
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00002CE0 File Offset: 0x00000EE0
		// (set) Token: 0x0600010E RID: 270 RVA: 0x00002D00 File Offset: 0x00000F00
		public Object owner
		{
			get
			{
				return NavMesh.InternalGetOwner(this.id);
			}
			set
			{
				int num = ((value != null) ? value.GetInstanceID() : 0);
				bool flag = !NavMesh.InternalSetOwner(this.id, num);
				if (flag)
				{
					Debug.LogError("Cannot set 'owner' on an invalid NavMeshDataInstance");
				}
			}
		}
	}
}
