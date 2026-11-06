using System;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x02000038 RID: 56
	[Serializable]
	public struct GPUITransformData : IEquatable<GPUITransformData>
	{
		// Token: 0x06000224 RID: 548 RVA: 0x0000B690 File Offset: 0x00009890
		public void SetFromMatrix(Matrix4x4 matrix)
		{
			this.position.x = matrix.m03;
			this.position.y = matrix.m13;
			this.position.z = matrix.m23;
			this.rotation = matrix.rotation;
			this.scale = matrix.lossyScale;
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000B6EA File Offset: 0x000098EA
		public Matrix4x4 ToMatrix()
		{
			return Matrix4x4.TRS(this.position, this.rotation, this.scale);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000B703 File Offset: 0x00009903
		public void SetToTransform(Transform transform)
		{
			transform.SetPositionAndRotation(this.position, this.rotation);
			transform.localScale = this.scale;
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000B724 File Offset: 0x00009924
		public void SetTransformRelativeToParent(Matrix4x4 parentLTW, Matrix4x4 childLTW)
		{
			float m = parentLTW.m00;
			float m2 = parentLTW.m01;
			float m3 = parentLTW.m02;
			float m4 = parentLTW.m10;
			float m5 = parentLTW.m11;
			float m6 = parentLTW.m12;
			float m7 = parentLTW.m20;
			float m8 = parentLTW.m21;
			float m9 = parentLTW.m22;
			float num = (float)Math.Sqrt((double)(m * m + m4 * m4 + m7 * m7));
			float num2 = (float)Math.Sqrt((double)(m2 * m2 + m5 * m5 + m8 * m8));
			float num3 = (float)Math.Sqrt((double)(m3 * m3 + m6 * m6 + m9 * m9));
			float num4 = 1f / num;
			float num5 = 1f / num2;
			float num6 = 1f / num3;
			float num7 = m * num4;
			float num8 = m2 * num5;
			float num9 = m3 * num6;
			float num10 = m4 * num4;
			float num11 = m5 * num5;
			float num12 = m6 * num6;
			float num13 = m7 * num4;
			float num14 = m8 * num5;
			float num15 = m9 * num6;
			float m10 = childLTW.m00;
			float m11 = childLTW.m01;
			float m12 = childLTW.m02;
			float m13 = childLTW.m10;
			float m14 = childLTW.m11;
			float m15 = childLTW.m12;
			float m16 = childLTW.m20;
			float m17 = childLTW.m21;
			float m18 = childLTW.m22;
			float num16 = (float)Math.Sqrt((double)(m10 * m10 + m13 * m13 + m16 * m16));
			float num17 = (float)Math.Sqrt((double)(m11 * m11 + m14 * m14 + m17 * m17));
			float num18 = (float)Math.Sqrt((double)(m12 * m12 + m15 * m15 + m18 * m18));
			float num19 = 1f / num16;
			float num20 = 1f / num17;
			float num21 = 1f / num18;
			this.scale.x = num16 * num4;
			this.scale.y = num17 * num5;
			this.scale.z = num18 * num6;
			float num22 = m10 * num19;
			float num23 = m11 * num20;
			float num24 = m12 * num21;
			float num25 = m13 * num19;
			float num26 = m14 * num20;
			float num27 = m15 * num21;
			float num28 = m16 * num19;
			float num29 = m17 * num20;
			float num30 = m18 * num21;
			float num31 = num7;
			float num32 = num10;
			float num33 = num13;
			float num34 = num8;
			float num35 = num11;
			float num36 = num14;
			float num37 = num9;
			float num38 = num12;
			float num39 = num15;
			float num40 = num31 * num22 + num32 * num25 + num33 * num28;
			float num41 = num31 * num23 + num32 * num26 + num33 * num29;
			float num42 = num31 * num24 + num32 * num27 + num33 * num30;
			float num43 = num34 * num22 + num35 * num25 + num36 * num28;
			float num44 = num34 * num23 + num35 * num26 + num36 * num29;
			float num45 = num34 * num24 + num35 * num27 + num36 * num30;
			float num46 = num37 * num22 + num38 * num25 + num39 * num28;
			float num47 = num37 * num23 + num38 * num26 + num39 * num29;
			float num48 = num37 * num24 + num38 * num27 + num39 * num30;
			float num49 = num40 + num44 + num48;
			float num52;
			float num53;
			float num54;
			float num55;
			if (num49 > 0f)
			{
				float num50 = (float)Math.Sqrt((double)(num49 + 1f)) * 0.5f;
				float num51 = 0.25f / num50;
				num52 = num50;
				num53 = (num47 - num45) * num51;
				num54 = (num42 - num46) * num51;
				num55 = (num43 - num41) * num51;
			}
			else if (num40 > num44 && num40 > num48)
			{
				float num56 = (float)Math.Sqrt((double)(1f + num40 - num44 - num48)) * 0.5f;
				float num57 = 0.25f / num56;
				num52 = (num47 - num45) * num57;
				num53 = num56;
				num54 = (num41 + num43) * num57;
				num55 = (num42 + num46) * num57;
			}
			else if (num44 > num48)
			{
				float num58 = (float)Math.Sqrt((double)(1f + num44 - num40 - num48)) * 0.5f;
				float num59 = 0.25f / num58;
				num52 = (num42 - num46) * num59;
				num53 = (num41 + num43) * num59;
				num54 = num58;
				num55 = (num45 + num47) * num59;
			}
			else
			{
				float num60 = (float)Math.Sqrt((double)(1f + num48 - num40 - num44)) * 0.5f;
				float num61 = 0.25f / num60;
				num52 = (num43 - num41) * num61;
				num53 = (num42 + num46) * num61;
				num54 = (num45 + num47) * num61;
				num55 = num60;
			}
			this.rotation.x = num53;
			this.rotation.y = num54;
			this.rotation.z = num55;
			this.rotation.w = num52;
			float num62 = childLTW.m03 - parentLTW.m03;
			float num63 = childLTW.m13 - parentLTW.m13;
			float num64 = childLTW.m23 - parentLTW.m23;
			this.position.x = (num31 * num62 + num32 * num63 + num33 * num64) * num4;
			this.position.y = (num34 * num62 + num35 * num63 + num36 * num64) * num5;
			this.position.z = (num37 * num62 + num38 * num63 + num39 * num64) * num6;
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000BC07 File Offset: 0x00009E07
		public bool Equals(GPUITransformData other)
		{
			return this.position == other.position && this.rotation == other.rotation && this.scale == other.scale;
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000BC44 File Offset: 0x00009E44
		public override bool Equals(object obj)
		{
			if (obj is GPUITransformData)
			{
				GPUITransformData gpuitransformData = (GPUITransformData)obj;
				return this.Equals(gpuitransformData);
			}
			return base.Equals(obj);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000BC7C File Offset: 0x00009E7C
		public override int GetHashCode()
		{
			return GPUIUtility.GenerateHash(new int[]
			{
				this.position.GetHashCode(),
				this.rotation.GetHashCode(),
				this.scale.GetHashCode()
			});
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000BCD0 File Offset: 0x00009ED0
		public static bool operator ==(GPUITransformData left, GPUITransformData right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000BCDA File Offset: 0x00009EDA
		public static bool operator !=(GPUITransformData left, GPUITransformData right)
		{
			return !(left == right);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000BCE6 File Offset: 0x00009EE6
		public override string ToString()
		{
			return string.Format(GPUITransformData.TO_STRING_TEXT, this.position.ToString("F4"), this.rotation.ToString("F4"), this.scale.ToString("F4"));
		}

		// Token: 0x04000167 RID: 359
		public Vector3 position;

		// Token: 0x04000168 RID: 360
		public Quaternion rotation;

		// Token: 0x04000169 RID: 361
		public Vector3 scale;

		// Token: 0x0400016A RID: 362
		public const int STRIDE = 40;

		// Token: 0x0400016B RID: 363
		private static readonly string TO_STRING_TEXT = "Position: {0}, Rotation: {1}, Scale: {2}";
	}
}
