using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020000DA RID: 218
	public static class HDROutputUtils
	{
		// Token: 0x06000760 RID: 1888 RVA: 0x000241EC File Offset: 0x000223EC
		public static bool GetColorSpaceForGamut(ColorGamut gamut, out int colorspace)
		{
			if (ColorGamutUtility.GetWhitePoint(gamut) != WhitePoint.D65)
			{
				Debug.LogWarningFormat("{0} white point is currently unsupported for outputting to HDR.", new object[] { gamut.ToString() });
				colorspace = -1;
				return false;
			}
			switch (ColorGamutUtility.GetColorPrimaries(gamut))
			{
			case ColorPrimaries.Rec709:
				colorspace = 0;
				return true;
			case ColorPrimaries.Rec2020:
				colorspace = 1;
				return true;
			case ColorPrimaries.P3:
				colorspace = 2;
				return true;
			default:
				Debug.LogWarningFormat("{0} color space is currently unsupported for outputting to HDR.", new object[] { gamut.ToString() });
				colorspace = -1;
				return false;
			}
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x00024274 File Offset: 0x00022474
		public static bool GetColorEncodingForGamut(ColorGamut gamut, out int encoding)
		{
			switch (ColorGamutUtility.GetTransferFunction(gamut))
			{
			case TransferFunction.sRGB:
				encoding = 0;
				return true;
			case TransferFunction.PQ:
				encoding = 2;
				return true;
			case TransferFunction.Linear:
				encoding = 3;
				return true;
			case TransferFunction.Gamma22:
				encoding = 4;
				return true;
			}
			Debug.LogWarningFormat("{0} color encoding is currently unsupported for outputting to HDR.", new object[] { gamut.ToString() });
			encoding = -1;
			return false;
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x000242DC File Offset: 0x000224DC
		public static void ConfigureHDROutput(Material material, ColorGamut gamut, HDROutputUtils.Operation operations)
		{
			int num;
			int num2;
			if (!HDROutputUtils.GetColorSpaceForGamut(gamut, out num) || !HDROutputUtils.GetColorEncodingForGamut(gamut, out num2))
			{
				return;
			}
			material.SetInteger(HDROutputUtils.ShaderPropertyId.hdrColorSpace, num);
			material.SetInteger(HDROutputUtils.ShaderPropertyId.hdrEncoding, num2);
			CoreUtils.SetKeyword(material, HDROutputUtils.ShaderKeywords.HDRColorSpaceConversionAndEncoding.name, operations.HasFlag(HDROutputUtils.Operation.ColorConversion) && operations.HasFlag(HDROutputUtils.Operation.ColorEncoding));
			CoreUtils.SetKeyword(material, HDROutputUtils.ShaderKeywords.HDREncoding.name, operations.HasFlag(HDROutputUtils.Operation.ColorEncoding) && !operations.HasFlag(HDROutputUtils.Operation.ColorConversion));
			CoreUtils.SetKeyword(material, HDROutputUtils.ShaderKeywords.HDRColorSpaceConversion.name, operations.HasFlag(HDROutputUtils.Operation.ColorConversion) && !operations.HasFlag(HDROutputUtils.Operation.ColorEncoding));
			CoreUtils.SetKeyword(material, HDROutputUtils.ShaderKeywords.HDRInput.name, operations == HDROutputUtils.Operation.None);
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x000243E4 File Offset: 0x000225E4
		public static void ConfigureHDROutput(MaterialPropertyBlock properties, ColorGamut gamut)
		{
			int num;
			int num2;
			if (!HDROutputUtils.GetColorSpaceForGamut(gamut, out num) || !HDROutputUtils.GetColorEncodingForGamut(gamut, out num2))
			{
				return;
			}
			properties.SetInteger(HDROutputUtils.ShaderPropertyId.hdrColorSpace, num);
			properties.SetInteger(HDROutputUtils.ShaderPropertyId.hdrEncoding, num2);
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x00024420 File Offset: 0x00022620
		public static void ConfigureHDROutput(Material material, HDROutputUtils.Operation operations)
		{
			CoreUtils.SetKeyword(material, HDROutputUtils.ShaderKeywords.HDRColorSpaceConversionAndEncoding.name, operations.HasFlag(HDROutputUtils.Operation.ColorConversion) && operations.HasFlag(HDROutputUtils.Operation.ColorEncoding));
			CoreUtils.SetKeyword(material, HDROutputUtils.ShaderKeywords.HDREncoding.name, operations.HasFlag(HDROutputUtils.Operation.ColorEncoding) && !operations.HasFlag(HDROutputUtils.Operation.ColorConversion));
			CoreUtils.SetKeyword(material, HDROutputUtils.ShaderKeywords.HDRColorSpaceConversion.name, operations.HasFlag(HDROutputUtils.Operation.ColorConversion) && !operations.HasFlag(HDROutputUtils.Operation.ColorEncoding));
			CoreUtils.SetKeyword(material, HDROutputUtils.ShaderKeywords.HDRInput.name, operations == HDROutputUtils.Operation.None);
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x000244F8 File Offset: 0x000226F8
		public static void ConfigureHDROutput(ComputeShader computeShader, ColorGamut gamut, HDROutputUtils.Operation operations)
		{
			int num;
			int num2;
			if (!HDROutputUtils.GetColorSpaceForGamut(gamut, out num) || !HDROutputUtils.GetColorEncodingForGamut(gamut, out num2))
			{
				return;
			}
			computeShader.SetInt(HDROutputUtils.ShaderPropertyId.hdrColorSpace, num);
			computeShader.SetInt(HDROutputUtils.ShaderPropertyId.hdrEncoding, num2);
			CoreUtils.SetKeyword(computeShader, HDROutputUtils.ShaderKeywords.HDRColorSpaceConversionAndEncoding.name, operations.HasFlag(HDROutputUtils.Operation.ColorConversion) && operations.HasFlag(HDROutputUtils.Operation.ColorEncoding));
			CoreUtils.SetKeyword(computeShader, HDROutputUtils.ShaderKeywords.HDREncoding.name, operations.HasFlag(HDROutputUtils.Operation.ColorEncoding) && !operations.HasFlag(HDROutputUtils.Operation.ColorConversion));
			CoreUtils.SetKeyword(computeShader, HDROutputUtils.ShaderKeywords.HDRColorSpaceConversion.name, operations.HasFlag(HDROutputUtils.Operation.ColorConversion) && !operations.HasFlag(HDROutputUtils.Operation.ColorEncoding));
			CoreUtils.SetKeyword(computeShader, HDROutputUtils.ShaderKeywords.HDRInput.name, operations == HDROutputUtils.Operation.None);
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x00024600 File Offset: 0x00022800
		public static bool IsShaderVariantValid(ShaderKeywordSet shaderKeywordSet, bool isHDREnabled)
		{
			bool flag = shaderKeywordSet.IsEnabled(HDROutputUtils.ShaderKeywords.HDREncoding) || shaderKeywordSet.IsEnabled(HDROutputUtils.ShaderKeywords.HDRColorSpaceConversion) || shaderKeywordSet.IsEnabled(HDROutputUtils.ShaderKeywords.HDRColorSpaceConversionAndEncoding) || shaderKeywordSet.IsEnabled(HDROutputUtils.ShaderKeywords.HDRInput);
			return isHDREnabled || !flag;
		}

		// Token: 0x020001D0 RID: 464
		[Flags]
		public enum Operation
		{
			// Token: 0x0400078D RID: 1933
			None = 0,
			// Token: 0x0400078E RID: 1934
			ColorConversion = 1,
			// Token: 0x0400078F RID: 1935
			ColorEncoding = 2
		}

		// Token: 0x020001D1 RID: 465
		public struct HDRDisplayInformation
		{
			// Token: 0x06000B69 RID: 2921 RVA: 0x00030193 File Offset: 0x0002E393
			public HDRDisplayInformation(int maxFullFrameToneMapLuminance, int maxToneMapLuminance, int minToneMapLuminance, float hdrPaperWhiteNits)
			{
				this.maxFullFrameToneMapLuminance = maxFullFrameToneMapLuminance;
				this.maxToneMapLuminance = maxToneMapLuminance;
				this.minToneMapLuminance = minToneMapLuminance;
				this.paperWhiteNits = hdrPaperWhiteNits;
			}

			// Token: 0x04000790 RID: 1936
			public int maxFullFrameToneMapLuminance;

			// Token: 0x04000791 RID: 1937
			public int maxToneMapLuminance;

			// Token: 0x04000792 RID: 1938
			public int minToneMapLuminance;

			// Token: 0x04000793 RID: 1939
			public float paperWhiteNits;
		}

		// Token: 0x020001D2 RID: 466
		public static class ShaderKeywords
		{
			// Token: 0x04000794 RID: 1940
			public const string HDR_COLORSPACE_CONVERSION = "HDR_COLORSPACE_CONVERSION";

			// Token: 0x04000795 RID: 1941
			public const string HDR_ENCODING = "HDR_ENCODING";

			// Token: 0x04000796 RID: 1942
			public const string HDR_COLORSPACE_CONVERSION_AND_ENCODING = "HDR_COLORSPACE_CONVERSION_AND_ENCODING";

			// Token: 0x04000797 RID: 1943
			public const string HDR_INPUT = "HDR_INPUT";

			// Token: 0x04000798 RID: 1944
			internal static readonly ShaderKeyword HDRColorSpaceConversion = new ShaderKeyword("HDR_COLORSPACE_CONVERSION");

			// Token: 0x04000799 RID: 1945
			internal static readonly ShaderKeyword HDREncoding = new ShaderKeyword("HDR_ENCODING");

			// Token: 0x0400079A RID: 1946
			internal static readonly ShaderKeyword HDRColorSpaceConversionAndEncoding = new ShaderKeyword("HDR_COLORSPACE_CONVERSION_AND_ENCODING");

			// Token: 0x0400079B RID: 1947
			internal static readonly ShaderKeyword HDRInput = new ShaderKeyword("HDR_INPUT");
		}

		// Token: 0x020001D3 RID: 467
		private static class ShaderPropertyId
		{
			// Token: 0x0400079C RID: 1948
			public static readonly int hdrColorSpace = Shader.PropertyToID("_HDRColorspace");

			// Token: 0x0400079D RID: 1949
			public static readonly int hdrEncoding = Shader.PropertyToID("_HDREncoding");
		}
	}
}
