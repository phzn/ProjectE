using System;
using System.Collections.Generic;
using UnityEngine;

namespace Invector.vCharacterController.AI
{
	// Token: 0x020006C8 RID: 1736
	public class vWaypointArea : MonoBehaviour
	{
		// Token: 0x06002BC9 RID: 11209 RVA: 0x000F3DA0 File Offset: 0x000F1FA0
		public vWaypoint GetRandomWayPoint()
		{
			Random random = new Random(100);
			List<vWaypoint> validPoints = this.GetValidPoints(false);
			int num = random.Next(0, this.waypoints.Count - 1);
			if (validPoints != null && validPoints.Count > 0 && num < validPoints.Count)
			{
				return validPoints[num];
			}
			return null;
		}

		// Token: 0x06002BCA RID: 11210 RVA: 0x000F3DF0 File Offset: 0x000F1FF0
		public List<vWaypoint> GetValidPoints(bool reverse = false)
		{
			List<vWaypoint> list = this.waypoints.FindAll((vWaypoint node) => node.isValid);
			if (reverse)
			{
				list.Reverse();
			}
			return list;
		}

		// Token: 0x06002BCB RID: 11211 RVA: 0x000F3E34 File Offset: 0x000F2034
		public List<vPoint> GetValidSubPoints(vWaypoint waipoint, bool reverse = false)
		{
			List<vPoint> list = waipoint.subPoints.FindAll((vPoint node) => node.isValid);
			if (reverse)
			{
				list.Reverse();
			}
			return list;
		}

		// Token: 0x06002BCC RID: 11212 RVA: 0x000F3E78 File Offset: 0x000F2078
		public vWaypoint GetWayPoint(int index)
		{
			List<vWaypoint> validPoints = this.GetValidPoints(false);
			if (validPoints != null && validPoints.Count > 0 && index < validPoints.Count)
			{
				return validPoints[index];
			}
			return null;
		}

		// Token: 0x04002B13 RID: 11027
		public List<vWaypoint> waypoints;

		// Token: 0x04002B14 RID: 11028
		public bool randomWayPoint;
	}
}
