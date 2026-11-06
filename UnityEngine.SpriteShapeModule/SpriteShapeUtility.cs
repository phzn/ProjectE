using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.U2D
{
	// Token: 0x02000009 RID: 9
	[MovedFrom("UnityEngine.Experimental.U2D")]
	[NativeHeader("Modules/SpriteShape/Public/SpriteShapeUtility.h")]
	public class SpriteShapeUtility
	{
		// Token: 0x06000025 RID: 37 RVA: 0x00002415 File Offset: 0x00000615
		[FreeFunction("SpriteShapeUtility::Generate")]
		[NativeThrows]
		public static int[] Generate(Mesh mesh, SpriteShapeParameters shapeParams, ShapeControlPoint[] points, SpriteShapeMetaData[] metaData, AngleRangeInfo[] angleRange, Sprite[] sprites, Sprite[] corners)
		{
			return SpriteShapeUtility.Generate_Injected(mesh, ref shapeParams, points, metaData, angleRange, sprites, corners);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002427 File Offset: 0x00000627
		[FreeFunction("SpriteShapeUtility::GenerateSpriteShape")]
		[NativeThrows]
		public static void GenerateSpriteShape(SpriteShapeRenderer renderer, SpriteShapeParameters shapeParams, ShapeControlPoint[] points, SpriteShapeMetaData[] metaData, AngleRangeInfo[] angleRange, Sprite[] sprites, Sprite[] corners)
		{
			SpriteShapeUtility.GenerateSpriteShape_Injected(renderer, ref shapeParams, points, metaData, angleRange, sprites, corners);
		}

		// Token: 0x06000028 RID: 40
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int[] Generate_Injected(Mesh mesh, ref SpriteShapeParameters shapeParams, ShapeControlPoint[] points, SpriteShapeMetaData[] metaData, AngleRangeInfo[] angleRange, Sprite[] sprites, Sprite[] corners);

		// Token: 0x06000029 RID: 41
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GenerateSpriteShape_Injected(SpriteShapeRenderer renderer, ref SpriteShapeParameters shapeParams, ShapeControlPoint[] points, SpriteShapeMetaData[] metaData, AngleRangeInfo[] angleRange, Sprite[] sprites, Sprite[] corners);
	}
}
