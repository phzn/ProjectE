using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020000D6 RID: 214
	public static class FSRUtils
	{
		// Token: 0x0600073E RID: 1854 RVA: 0x00023888 File Offset: 0x00021A88
		public static void SetEasuConstants(CommandBuffer cmd, Vector2 inputViewportSizeInPixels, Vector2 inputImageSizeInPixels, Vector2 outputImageSizeInPixels)
		{
			Vector4 vector;
			vector.x = inputViewportSizeInPixels.x / outputImageSizeInPixels.x;
			vector.y = inputViewportSizeInPixels.y / outputImageSizeInPixels.y;
			vector.z = 0.5f * inputViewportSizeInPixels.x / outputImageSizeInPixels.x - 0.5f;
			vector.w = 0.5f * inputViewportSizeInPixels.y / outputImageSizeInPixels.y - 0.5f;
			Vector4 vector2;
			vector2.x = 1f / inputImageSizeInPixels.x;
			vector2.y = 1f / inputImageSizeInPixels.y;
			vector2.z = 1f / inputImageSizeInPixels.x;
			vector2.w = -1f / inputImageSizeInPixels.y;
			Vector4 vector3;
			vector3.x = -1f / inputImageSizeInPixels.x;
			vector3.y = 2f / inputImageSizeInPixels.y;
			vector3.z = 1f / inputImageSizeInPixels.x;
			vector3.w = 2f / inputImageSizeInPixels.y;
			Vector4 vector4;
			vector4.x = 0f / inputImageSizeInPixels.x;
			vector4.y = 4f / inputImageSizeInPixels.y;
			vector4.z = 0f;
			vector4.w = 0f;
			cmd.SetGlobalVector(FSRUtils.ShaderConstants._FsrEasuConstants0, vector);
			cmd.SetGlobalVector(FSRUtils.ShaderConstants._FsrEasuConstants1, vector2);
			cmd.SetGlobalVector(FSRUtils.ShaderConstants._FsrEasuConstants2, vector3);
			cmd.SetGlobalVector(FSRUtils.ShaderConstants._FsrEasuConstants3, vector4);
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x00023A04 File Offset: 0x00021C04
		public static void SetRcasConstants(CommandBuffer cmd, float sharpnessStops = 0.2f)
		{
			float num = Mathf.Pow(2f, -sharpnessStops);
			ushort num2 = Mathf.FloatToHalf(num);
			float num3 = BitConverter.Int32BitsToSingle((int)num2 | ((int)num2 << 16));
			Vector4 vector;
			vector.x = num;
			vector.y = num3;
			vector.z = 0f;
			vector.w = 0f;
			cmd.SetGlobalVector(FSRUtils.ShaderConstants._FsrRcasConstants, vector);
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x00023A64 File Offset: 0x00021C64
		public static void SetRcasConstantsLinear(CommandBuffer cmd, float sharpnessLinear = 0.92f)
		{
			float num = (1f - sharpnessLinear) * 2.5f;
			FSRUtils.SetRcasConstants(cmd, num);
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x00023A86 File Offset: 0x00021C86
		public static bool IsSupported()
		{
			return SystemInfo.graphicsShaderLevel >= 45;
		}

		// Token: 0x04000492 RID: 1170
		internal const float kMaxSharpnessStops = 2.5f;

		// Token: 0x04000493 RID: 1171
		public const float kDefaultSharpnessStops = 0.2f;

		// Token: 0x04000494 RID: 1172
		public const float kDefaultSharpnessLinear = 0.92f;

		// Token: 0x020001CC RID: 460
		private static class ShaderConstants
		{
			// Token: 0x04000778 RID: 1912
			public static readonly int _FsrEasuConstants0 = Shader.PropertyToID("_FsrEasuConstants0");

			// Token: 0x04000779 RID: 1913
			public static readonly int _FsrEasuConstants1 = Shader.PropertyToID("_FsrEasuConstants1");

			// Token: 0x0400077A RID: 1914
			public static readonly int _FsrEasuConstants2 = Shader.PropertyToID("_FsrEasuConstants2");

			// Token: 0x0400077B RID: 1915
			public static readonly int _FsrEasuConstants3 = Shader.PropertyToID("_FsrEasuConstants3");

			// Token: 0x0400077C RID: 1916
			public static readonly int _FsrRcasConstants = Shader.PropertyToID("_FsrRcasConstants");
		}
	}
}
