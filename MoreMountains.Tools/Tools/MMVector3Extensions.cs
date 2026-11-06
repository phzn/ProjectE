using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000F2 RID: 242
	public static class MMVector3Extensions
	{
		// Token: 0x0600050C RID: 1292 RVA: 0x00017268 File Offset: 0x00015468
		public static Vector3 MMSetX(this Vector3 vector, float newValue)
		{
			vector.x = newValue;
			return vector;
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x00017273 File Offset: 0x00015473
		public static Vector3 MMSetY(this Vector3 vector, float newValue)
		{
			vector.y = newValue;
			return vector;
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x0001727E File Offset: 0x0001547E
		public static Vector3 MMSetZ(this Vector3 vector, float newValue)
		{
			vector.z = newValue;
			return vector;
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x00017289 File Offset: 0x00015489
		public static Vector3 MMInvert(this Vector3 newValue)
		{
			return new Vector3(1f / newValue.x, 1f / newValue.y, 1f / newValue.z);
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x000172B4 File Offset: 0x000154B4
		public static Vector3 MMProject(this Vector3 vector, Vector3 projectedVector)
		{
			return Vector3.Dot(vector, projectedVector) * projectedVector;
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x000172C3 File Offset: 0x000154C3
		public static Vector3 MMReject(this Vector3 vector, Vector3 rejectedVector)
		{
			return vector - vector.MMProject(rejectedVector);
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x000172D2 File Offset: 0x000154D2
		public static Vector3 MMRound(this Vector3 vector)
		{
			vector.x = Mathf.Round(vector.x);
			vector.y = Mathf.Round(vector.y);
			vector.z = Mathf.Round(vector.z);
			return vector;
		}
	}
}
