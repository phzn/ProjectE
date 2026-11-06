using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020000C6 RID: 198
	[Obsolete("BatchRendererGroupGlobals and associated cbuffer are now set automatically by Unity. Setting it manually is no longer necessary or supported.")]
	[Serializable]
	public struct BatchRendererGroupGlobals : IEquatable<BatchRendererGroupGlobals>
	{
		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000626 RID: 1574 RVA: 0x0001F730 File Offset: 0x0001D930
		public static BatchRendererGroupGlobals Default
		{
			get
			{
				BatchRendererGroupGlobals batchRendererGroupGlobals = default(BatchRendererGroupGlobals);
				batchRendererGroupGlobals.ProbesOcclusion = Vector4.one;
				batchRendererGroupGlobals.SpecCube0_HDR = ReflectionProbe.defaultTextureHDRDecodeValues;
				batchRendererGroupGlobals.SpecCube1_HDR = batchRendererGroupGlobals.SpecCube0_HDR;
				batchRendererGroupGlobals.SHCoefficients = new SHCoefficients(RenderSettings.ambientProbe);
				return batchRendererGroupGlobals;
			}
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x0001F77C File Offset: 0x0001D97C
		public bool Equals(BatchRendererGroupGlobals other)
		{
			return this.ProbesOcclusion.Equals(other.ProbesOcclusion) && this.SpecCube0_HDR.Equals(other.SpecCube0_HDR) && this.SpecCube1_HDR.Equals(other.SpecCube1_HDR) && this.SHCoefficients.Equals(other.SHCoefficients);
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x0001F7D8 File Offset: 0x0001D9D8
		public override bool Equals(object obj)
		{
			if (obj is BatchRendererGroupGlobals)
			{
				BatchRendererGroupGlobals batchRendererGroupGlobals = (BatchRendererGroupGlobals)obj;
				return this.Equals(batchRendererGroupGlobals);
			}
			return false;
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x0001F7FD File Offset: 0x0001D9FD
		public override int GetHashCode()
		{
			return HashCode.Combine<Vector4, Vector4, Vector4, SHCoefficients>(this.ProbesOcclusion, this.SpecCube0_HDR, this.SpecCube1_HDR, this.SHCoefficients);
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x0001F81C File Offset: 0x0001DA1C
		public static bool operator ==(BatchRendererGroupGlobals left, BatchRendererGroupGlobals right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x0001F826 File Offset: 0x0001DA26
		public static bool operator !=(BatchRendererGroupGlobals left, BatchRendererGroupGlobals right)
		{
			return !left.Equals(right);
		}

		// Token: 0x0400045F RID: 1119
		public const string kGlobalsPropertyName = "unity_DOTSInstanceGlobalValues";

		// Token: 0x04000460 RID: 1120
		public static readonly int kGlobalsPropertyId = Shader.PropertyToID("unity_DOTSInstanceGlobalValues");

		// Token: 0x04000461 RID: 1121
		public Vector4 ProbesOcclusion;

		// Token: 0x04000462 RID: 1122
		public Vector4 SpecCube0_HDR;

		// Token: 0x04000463 RID: 1123
		public Vector4 SpecCube1_HDR;

		// Token: 0x04000464 RID: 1124
		public SHCoefficients SHCoefficients;
	}
}
