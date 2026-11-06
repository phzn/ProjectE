using System;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000007 RID: 7
	public abstract class ClothBehaviour : MonoBehaviour
	{
		// Token: 0x0600000B RID: 11 RVA: 0x00002137 File Offset: 0x00000337
		public virtual int GetMagicaHashCode()
		{
			return 0;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000C RID: 12 RVA: 0x0000213A File Offset: 0x0000033A
		// (set) Token: 0x0600000D RID: 13 RVA: 0x00002142 File Offset: 0x00000342
		public bool IsGizmoVisible { get; set; }

		// Token: 0x0600000E RID: 14 RVA: 0x0000214B File Offset: 0x0000034B
		protected virtual void OnDrawGizmos()
		{
			this.IsGizmoVisible = true;
		}
	}
}
