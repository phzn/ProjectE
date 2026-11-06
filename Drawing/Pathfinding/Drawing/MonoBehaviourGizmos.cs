using System;
using UnityEngine;

namespace Pathfinding.Drawing
{
	// Token: 0x02000053 RID: 83
	public abstract class MonoBehaviourGizmos : MonoBehaviour, IDrawGizmos
	{
		// Token: 0x0600029F RID: 671 RVA: 0x0000E733 File Offset: 0x0000C933
		public MonoBehaviourGizmos()
		{
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x00002104 File Offset: 0x00000304
		private void OnDrawGizmosSelected()
		{
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00002104 File Offset: 0x00000304
		public virtual void DrawGizmos()
		{
		}
	}
}
