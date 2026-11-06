using System;
using Unity.Mathematics;
using UnityEngine.Splines.Interpolators;

namespace UnityEngine.Splines
{
	// Token: 0x0200000F RID: 15
	public static class InterpolatorUtility
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600005B RID: 91 RVA: 0x000037C8 File Offset: 0x000019C8
		public static IInterpolator<float> LerpFloat
		{
			get
			{
				return InterpolatorUtility.s_LerpFloat;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600005C RID: 92 RVA: 0x000037CF File Offset: 0x000019CF
		public static IInterpolator<float2> LerpFloat2
		{
			get
			{
				return InterpolatorUtility.s_LerpFloat2;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600005D RID: 93 RVA: 0x000037D6 File Offset: 0x000019D6
		public static IInterpolator<float3> LerpFloat3
		{
			get
			{
				return InterpolatorUtility.s_LerpFloat3;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600005E RID: 94 RVA: 0x000037DD File Offset: 0x000019DD
		public static IInterpolator<float4> LerpFloat4
		{
			get
			{
				return InterpolatorUtility.s_LerpFloat4;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600005F RID: 95 RVA: 0x000037E4 File Offset: 0x000019E4
		public static IInterpolator<float2> SlerpFloat2
		{
			get
			{
				return InterpolatorUtility.s_SlerpFloat2;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000060 RID: 96 RVA: 0x000037EB File Offset: 0x000019EB
		public static IInterpolator<float3> SlerpFloat3
		{
			get
			{
				return InterpolatorUtility.s_SlerpFloat3;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000061 RID: 97 RVA: 0x000037F2 File Offset: 0x000019F2
		public static IInterpolator<quaternion> LerpQuaternion
		{
			get
			{
				return InterpolatorUtility.s_LerpQuaternion;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000062 RID: 98 RVA: 0x000037F9 File Offset: 0x000019F9
		public static IInterpolator<Color> LerpColor
		{
			get
			{
				return InterpolatorUtility.s_LerpColor;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00003800 File Offset: 0x00001A00
		public static IInterpolator<float> SmoothStepFloat
		{
			get
			{
				return InterpolatorUtility.s_SmoothStepFloat;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00003807 File Offset: 0x00001A07
		public static IInterpolator<float2> SmoothStepFloat2
		{
			get
			{
				return InterpolatorUtility.s_SmoothStepFloat2;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000065 RID: 101 RVA: 0x0000380E File Offset: 0x00001A0E
		public static IInterpolator<float3> SmoothStepFloat3
		{
			get
			{
				return InterpolatorUtility.s_SmoothStepFloat3;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00003815 File Offset: 0x00001A15
		public static IInterpolator<float4> SmoothStepFloat4
		{
			get
			{
				return InterpolatorUtility.s_SmoothStepFloat4;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000067 RID: 103 RVA: 0x0000381C File Offset: 0x00001A1C
		public static IInterpolator<quaternion> SlerpQuaternion
		{
			get
			{
				return InterpolatorUtility.s_SlerpQuaternion;
			}
		}

		// Token: 0x04000020 RID: 32
		private static readonly IInterpolator<float> s_LerpFloat = default(LerpFloat);

		// Token: 0x04000021 RID: 33
		private static readonly IInterpolator<float2> s_LerpFloat2 = default(LerpFloat2);

		// Token: 0x04000022 RID: 34
		private static readonly IInterpolator<float3> s_LerpFloat3 = default(LerpFloat3);

		// Token: 0x04000023 RID: 35
		private static readonly IInterpolator<float4> s_LerpFloat4 = default(LerpFloat4);

		// Token: 0x04000024 RID: 36
		private static readonly IInterpolator<float2> s_SlerpFloat2 = default(SlerpFloat2);

		// Token: 0x04000025 RID: 37
		private static readonly IInterpolator<float3> s_SlerpFloat3 = default(SlerpFloat3);

		// Token: 0x04000026 RID: 38
		private static readonly IInterpolator<quaternion> s_LerpQuaternion = default(LerpQuaternion);

		// Token: 0x04000027 RID: 39
		private static readonly IInterpolator<Color> s_LerpColor = default(LerpColor);

		// Token: 0x04000028 RID: 40
		private static readonly IInterpolator<float> s_SmoothStepFloat = default(SmoothStepFloat);

		// Token: 0x04000029 RID: 41
		private static readonly IInterpolator<float2> s_SmoothStepFloat2 = default(SmoothStepFloat2);

		// Token: 0x0400002A RID: 42
		private static readonly IInterpolator<float3> s_SmoothStepFloat3 = default(SmoothStepFloat3);

		// Token: 0x0400002B RID: 43
		private static readonly IInterpolator<float4> s_SmoothStepFloat4 = default(SmoothStepFloat4);

		// Token: 0x0400002C RID: 44
		private static readonly IInterpolator<quaternion> s_SlerpQuaternion = default(SlerpQuaternion);
	}
}
