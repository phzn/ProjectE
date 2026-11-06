using System;
using System.Collections.Generic;
using UnityEngine;

namespace Invector.vCharacterController.AI
{
	// Token: 0x020006C7 RID: 1735
	[Serializable]
	public class vWaypoint : vPoint
	{
		// Token: 0x06002BC6 RID: 11206 RVA: 0x000F3D10 File Offset: 0x000F1F10
		public Vector3 GetRandomSubPoint()
		{
			int num = new Random(100).Next(0, this.subPoints.Count - 1);
			return this.GetSubPoint(num);
		}

		// Token: 0x06002BC7 RID: 11207 RVA: 0x000F3D40 File Offset: 0x000F1F40
		public Vector3 GetSubPoint(int index)
		{
			if (this.subPoints != null && this.subPoints.Count > 0 && index < this.subPoints.Count)
			{
				return this.subPoints[index].position;
			}
			return base.transform.position;
		}

		// Token: 0x04002B10 RID: 11024
		public List<vPoint> subPoints;

		// Token: 0x04002B11 RID: 11025
		public bool randomPatrolPoint;

		// Token: 0x04002B12 RID: 11026
		public bool rotateTo = true;
	}
}
