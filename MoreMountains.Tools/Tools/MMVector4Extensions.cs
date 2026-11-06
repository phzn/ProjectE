using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000F3 RID: 243
	public static class MMVector4Extensions
	{
		// Token: 0x06000513 RID: 1299 RVA: 0x0001730B File Offset: 0x0001550B
		public static float SumComponents(this Vector4 vector)
		{
			return vector.x + vector.y + vector.z + vector.w;
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00017328 File Offset: 0x00015528
		public static Vector4 MMSetX(this Vector4 vector, float newValue)
		{
			vector.x = newValue;
			return vector;
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00017333 File Offset: 0x00015533
		public static Vector4 MMSetY(this Vector4 vector, float newValue)
		{
			vector.y = newValue;
			return vector;
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x0001733E File Offset: 0x0001553E
		public static Vector4 MMSetZ(this Vector4 vector, float newValue)
		{
			vector.z = newValue;
			return vector;
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x00017349 File Offset: 0x00015549
		public static Vector4 MMSetW(this Vector4 vector, float newValue)
		{
			vector.w = newValue;
			return vector;
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x00017354 File Offset: 0x00015554
		public static Vector4 MMInvert(this Vector4 newValue)
		{
			return new Vector4(1f / newValue.x, 1f / newValue.y, 1f / newValue.z, 1f / newValue.w);
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0001738B File Offset: 0x0001558B
		public static Vector4 MMProject(this Vector4 vector, Vector4 projectedVector)
		{
			return Vector4.Dot(vector, projectedVector) * projectedVector;
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x0001739A File Offset: 0x0001559A
		public static Vector4 MMReject(this Vector4 vector, Vector4 rejectedVector)
		{
			return vector - vector.MMProject(rejectedVector);
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x000173AC File Offset: 0x000155AC
		public static Vector4 MMRound(this Vector4 vector)
		{
			vector.x = Mathf.Round(vector.x);
			vector.y = Mathf.Round(vector.y);
			vector.z = Mathf.Round(vector.z);
			vector.w = Mathf.Round(vector.w);
			return vector;
		}
	}
}
