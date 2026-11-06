using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.UIElements
{
	// Token: 0x02000280 RID: 640
	[NativeHeader("ModuleOverrides/com.unity.ui/Core/Native/Renderer/UIRMeshBuilder.bindings.h")]
	internal static class MeshBuilderNative
	{
		// Token: 0x0600121A RID: 4634 RVA: 0x00041210 File Offset: 0x0003F410
		public static MeshWriteDataInterface MakeBorder(MeshBuilderNative.NativeBorderParams borderParams, float posZ)
		{
			MeshWriteDataInterface meshWriteDataInterface;
			MeshBuilderNative.MakeBorder_Injected(ref borderParams, posZ, out meshWriteDataInterface);
			return meshWriteDataInterface;
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x00041228 File Offset: 0x0003F428
		public static MeshWriteDataInterface MakeSolidRect(MeshBuilderNative.NativeRectParams rectParams, float posZ)
		{
			MeshWriteDataInterface meshWriteDataInterface;
			MeshBuilderNative.MakeSolidRect_Injected(ref rectParams, posZ, out meshWriteDataInterface);
			return meshWriteDataInterface;
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x00041240 File Offset: 0x0003F440
		public static MeshWriteDataInterface MakeTexturedRect(MeshBuilderNative.NativeRectParams rectParams, float posZ)
		{
			MeshWriteDataInterface meshWriteDataInterface;
			MeshBuilderNative.MakeTexturedRect_Injected(ref rectParams, posZ, out meshWriteDataInterface);
			return meshWriteDataInterface;
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x00041258 File Offset: 0x0003F458
		public static MeshWriteDataInterface MakeVectorGraphicsStretchBackground(Vertex[] svgVertices, ushort[] svgIndices, float svgWidth, float svgHeight, Rect targetRect, Rect sourceUV, ScaleMode scaleMode, Color tint, MeshBuilderNative.NativeColorPage colorPage, int settingIndexOffset, ref int finalVertexCount, ref int finalIndexCount)
		{
			MeshWriteDataInterface meshWriteDataInterface;
			MeshBuilderNative.MakeVectorGraphicsStretchBackground_Injected(svgVertices, svgIndices, svgWidth, svgHeight, ref targetRect, ref sourceUV, scaleMode, ref tint, ref colorPage, settingIndexOffset, ref finalVertexCount, ref finalIndexCount, out meshWriteDataInterface);
			return meshWriteDataInterface;
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x00041284 File Offset: 0x0003F484
		public static MeshWriteDataInterface MakeVectorGraphics9SliceBackground(Vertex[] svgVertices, ushort[] svgIndices, float svgWidth, float svgHeight, Rect targetRect, Vector4 sliceLTRB, Color tint, MeshBuilderNative.NativeColorPage colorPage, int settingIndexOffset)
		{
			MeshWriteDataInterface meshWriteDataInterface;
			MeshBuilderNative.MakeVectorGraphics9SliceBackground_Injected(svgVertices, svgIndices, svgWidth, svgHeight, ref targetRect, ref sliceLTRB, ref tint, ref colorPage, settingIndexOffset, out meshWriteDataInterface);
			return meshWriteDataInterface;
		}

		// Token: 0x0600121F RID: 4639
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void MakeBorder_Injected(ref MeshBuilderNative.NativeBorderParams borderParams, float posZ, out MeshWriteDataInterface ret);

		// Token: 0x06001220 RID: 4640
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void MakeSolidRect_Injected(ref MeshBuilderNative.NativeRectParams rectParams, float posZ, out MeshWriteDataInterface ret);

		// Token: 0x06001221 RID: 4641
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void MakeTexturedRect_Injected(ref MeshBuilderNative.NativeRectParams rectParams, float posZ, out MeshWriteDataInterface ret);

		// Token: 0x06001222 RID: 4642
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void MakeVectorGraphicsStretchBackground_Injected(Vertex[] svgVertices, ushort[] svgIndices, float svgWidth, float svgHeight, ref Rect targetRect, ref Rect sourceUV, ScaleMode scaleMode, ref Color tint, ref MeshBuilderNative.NativeColorPage colorPage, int settingIndexOffset, ref int finalVertexCount, ref int finalIndexCount, out MeshWriteDataInterface ret);

		// Token: 0x06001223 RID: 4643
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void MakeVectorGraphics9SliceBackground_Injected(Vertex[] svgVertices, ushort[] svgIndices, float svgWidth, float svgHeight, ref Rect targetRect, ref Vector4 sliceLTRB, ref Color tint, ref MeshBuilderNative.NativeColorPage colorPage, int settingIndexOffset, out MeshWriteDataInterface ret);

		// Token: 0x04000802 RID: 2050
		public const float kEpsilon = 0.001f;

		// Token: 0x02000281 RID: 641
		public struct NativeColorPage
		{
			// Token: 0x04000803 RID: 2051
			public int isValid;

			// Token: 0x04000804 RID: 2052
			public Color32 pageAndID;
		}

		// Token: 0x02000282 RID: 642
		public struct NativeBorderParams
		{
			// Token: 0x04000805 RID: 2053
			public Rect rect;

			// Token: 0x04000806 RID: 2054
			public Color leftColor;

			// Token: 0x04000807 RID: 2055
			public Color topColor;

			// Token: 0x04000808 RID: 2056
			public Color rightColor;

			// Token: 0x04000809 RID: 2057
			public Color bottomColor;

			// Token: 0x0400080A RID: 2058
			public float leftWidth;

			// Token: 0x0400080B RID: 2059
			public float topWidth;

			// Token: 0x0400080C RID: 2060
			public float rightWidth;

			// Token: 0x0400080D RID: 2061
			public float bottomWidth;

			// Token: 0x0400080E RID: 2062
			public Vector2 topLeftRadius;

			// Token: 0x0400080F RID: 2063
			public Vector2 topRightRadius;

			// Token: 0x04000810 RID: 2064
			public Vector2 bottomRightRadius;

			// Token: 0x04000811 RID: 2065
			public Vector2 bottomLeftRadius;

			// Token: 0x04000812 RID: 2066
			internal MeshBuilderNative.NativeColorPage leftColorPage;

			// Token: 0x04000813 RID: 2067
			internal MeshBuilderNative.NativeColorPage topColorPage;

			// Token: 0x04000814 RID: 2068
			internal MeshBuilderNative.NativeColorPage rightColorPage;

			// Token: 0x04000815 RID: 2069
			internal MeshBuilderNative.NativeColorPage bottomColorPage;
		}

		// Token: 0x02000283 RID: 643
		public struct NativeRectParams
		{
			// Token: 0x04000816 RID: 2070
			public Rect rect;

			// Token: 0x04000817 RID: 2071
			public Rect subRect;

			// Token: 0x04000818 RID: 2072
			public Rect uv;

			// Token: 0x04000819 RID: 2073
			public Rect uvRegion;

			// Token: 0x0400081A RID: 2074
			public Color color;

			// Token: 0x0400081B RID: 2075
			public ScaleMode scaleMode;

			// Token: 0x0400081C RID: 2076
			public Vector2 topLeftRadius;

			// Token: 0x0400081D RID: 2077
			public Vector2 topRightRadius;

			// Token: 0x0400081E RID: 2078
			public Vector2 bottomRightRadius;

			// Token: 0x0400081F RID: 2079
			public Vector2 bottomLeftRadius;

			// Token: 0x04000820 RID: 2080
			public Rect backgroundRepeatRect;

			// Token: 0x04000821 RID: 2081
			public Vector2 contentSize;

			// Token: 0x04000822 RID: 2082
			public Vector2 textureSize;

			// Token: 0x04000823 RID: 2083
			public float texturePixelsPerPoint;

			// Token: 0x04000824 RID: 2084
			public int leftSlice;

			// Token: 0x04000825 RID: 2085
			public int topSlice;

			// Token: 0x04000826 RID: 2086
			public int rightSlice;

			// Token: 0x04000827 RID: 2087
			public int bottomSlice;

			// Token: 0x04000828 RID: 2088
			public float sliceScale;

			// Token: 0x04000829 RID: 2089
			public Vector4 rectInset;

			// Token: 0x0400082A RID: 2090
			public MeshBuilderNative.NativeColorPage colorPage;
		}
	}
}
