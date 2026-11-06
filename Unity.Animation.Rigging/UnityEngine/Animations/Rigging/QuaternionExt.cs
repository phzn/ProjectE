using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000077 RID: 119
	public static class QuaternionExt
	{
		// Token: 0x06000304 RID: 772 RVA: 0x0000A230 File Offset: 0x00008430
		public static Quaternion FromToRotation(Vector3 from, Vector3 to)
		{
			float num = Vector3.Dot(from.normalized, to.normalized);
			if (num >= 1f)
			{
				return Quaternion.identity;
			}
			if (num <= -1f)
			{
				Vector3 vector = Vector3.Cross(from, Vector3.right);
				if (vector.sqrMagnitude == 0f)
				{
					vector = Vector3.Cross(from, Vector3.up);
				}
				return Quaternion.AngleAxis(180f, vector);
			}
			return Quaternion.AngleAxis(Mathf.Acos(num) * 57.29578f, Vector3.Cross(from, to).normalized);
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000A2BC File Offset: 0x000084BC
		public static Quaternion Add(Quaternion rhs, Quaternion lhs)
		{
			float num = Mathf.Sign(Quaternion.Dot(rhs, lhs));
			return new Quaternion(rhs.x + num * lhs.x, rhs.y + num * lhs.y, rhs.z + num * lhs.z, rhs.w + num * lhs.w);
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000A317 File Offset: 0x00008517
		public static Quaternion Scale(Quaternion q, float scale)
		{
			return new Quaternion(q.x * scale, q.y * scale, q.z * scale, q.w * scale);
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000A340 File Offset: 0x00008540
		public static Quaternion NormalizeSafe(Quaternion q)
		{
			float num = Quaternion.Dot(q, q);
			if (num > 1E-10f)
			{
				float num2 = 1f / Mathf.Sqrt(num);
				return new Quaternion(q.x * num2, q.y * num2, q.z * num2, q.w * num2);
			}
			return Quaternion.identity;
		}

		// Token: 0x0400013F RID: 319
		private const float k_FloatMin = 1E-10f;

		// Token: 0x04000140 RID: 320
		public static readonly Quaternion zero = new Quaternion(0f, 0f, 0f, 0f);
	}
}
