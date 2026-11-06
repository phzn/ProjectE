using System;
using UnityEngine;

namespace Pathfinding.RVO
{
	// Token: 0x020002DD RID: 733
	[AddComponentMenu("")]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/rvosquareobstacle.html")]
	public class RVOSquareObstacle : RVOObstacle
	{
		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06001234 RID: 4660 RVA: 0x0001AA07 File Offset: 0x00018C07
		protected override bool StaticObstacle
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06001235 RID: 4661 RVA: 0x00019DC2 File Offset: 0x00017FC2
		protected override bool ExecuteInEditor
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06001236 RID: 4662 RVA: 0x00019DC2 File Offset: 0x00017FC2
		protected override bool LocalCoordinates
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06001237 RID: 4663 RVA: 0x000703CB File Offset: 0x0006E5CB
		protected override float Height
		{
			get
			{
				return this.height;
			}
		}

		// Token: 0x04000D6E RID: 3438
		public float height = 1f;

		// Token: 0x04000D6F RID: 3439
		public Vector2 size = Vector3.one;

		// Token: 0x04000D70 RID: 3440
		public Vector2 center = Vector3.zero;
	}
}
