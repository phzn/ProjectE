using System;
using System.Runtime.CompilerServices;
using Pathfinding.Collections;
using Pathfinding.Drawing;
using Unity.Burst;
using Unity.Mathematics;

namespace Pathfinding.ECS
{
	// Token: 0x02000323 RID: 803
	[BurstCompile]
	internal struct DrawGizmosJobUtils
	{
		// Token: 0x060013CA RID: 5066 RVA: 0x00075849 File Offset: 0x00073A49
		[BurstCompile]
		internal static void DrawPath(ref CommandBuilder draw, ref UnsafeSpan<float3> vertices, ref AgentCylinderShape shape)
		{
			DrawGizmosJobUtils.DrawPath_00001274$BurstDirectCall.Invoke(ref draw, ref vertices, ref shape);
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x00075854 File Offset: 0x00073A54
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void DrawPath$BurstManaged(ref CommandBuilder draw, ref UnsafeSpan<float3> vertices, ref AgentCylinderShape shape)
		{
			if (shape.radius > 0.01f)
			{
				CommandBuilder.PolylineWithSymbol polylineWithSymbol = new CommandBuilder.PolylineWithSymbol(CommandBuilder.SymbolDecoration.ArrowHead, shape.radius * 0.5f, shape.radius * 0f, shape.radius * 4f, true, 0f);
				for (int i = vertices.Length - 1; i >= 0; i--)
				{
					polylineWithSymbol.MoveTo(ref draw, *vertices[i]);
				}
				return;
			}
			for (int j = 0; j < vertices.Length - 1; j++)
			{
				draw.Line(*vertices[j], *vertices[j + 1]);
			}
		}

		// Token: 0x02000324 RID: 804
		// (Invoke) Token: 0x060013CD RID: 5069
		internal delegate void DrawPath_00001274$PostfixBurstDelegate(ref CommandBuilder draw, ref UnsafeSpan<float3> vertices, ref AgentCylinderShape shape);

		// Token: 0x02000325 RID: 805
		internal static class DrawPath_00001274$BurstDirectCall
		{
			// Token: 0x060013D0 RID: 5072 RVA: 0x000758FB File Offset: 0x00073AFB
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (DrawGizmosJobUtils.DrawPath_00001274$BurstDirectCall.Pointer == 0)
				{
					DrawGizmosJobUtils.DrawPath_00001274$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(DrawGizmosJobUtils.DrawPath_00001274$BurstDirectCall.DeferredCompilation, methodof(DrawGizmosJobUtils.DrawPath$BurstManaged(ref CommandBuilder, ref UnsafeSpan<float3>, ref AgentCylinderShape)).MethodHandle, typeof(DrawGizmosJobUtils.DrawPath_00001274$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = DrawGizmosJobUtils.DrawPath_00001274$BurstDirectCall.Pointer;
			}

			// Token: 0x060013D1 RID: 5073 RVA: 0x00075928 File Offset: 0x00073B28
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				DrawGizmosJobUtils.DrawPath_00001274$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x060013D2 RID: 5074 RVA: 0x00075940 File Offset: 0x00073B40
			public static void Constructor()
			{
				DrawGizmosJobUtils.DrawPath_00001274$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(DrawGizmosJobUtils.DrawPath(ref CommandBuilder, ref UnsafeSpan<float3>, ref AgentCylinderShape)).MethodHandle);
			}

			// Token: 0x060013D3 RID: 5075 RVA: 0x00003788 File Offset: 0x00001988
			public static void Initialize()
			{
			}

			// Token: 0x060013D4 RID: 5076 RVA: 0x00075951 File Offset: 0x00073B51
			// Note: this type is marked as 'beforefieldinit'.
			static DrawPath_00001274$BurstDirectCall()
			{
				DrawGizmosJobUtils.DrawPath_00001274$BurstDirectCall.Constructor();
			}

			// Token: 0x060013D5 RID: 5077 RVA: 0x00075958 File Offset: 0x00073B58
			public static void Invoke(ref CommandBuilder draw, ref UnsafeSpan<float3> vertices, ref AgentCylinderShape shape)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = DrawGizmosJobUtils.DrawPath_00001274$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Pathfinding.Drawing.CommandBuilder&,Pathfinding.Collections.UnsafeSpan`1<Unity.Mathematics.float3>&,Pathfinding.ECS.AgentCylinderShape&), ref draw, ref vertices, ref shape, functionPointer);
						return;
					}
				}
				DrawGizmosJobUtils.DrawPath$BurstManaged(ref draw, ref vertices, ref shape);
			}

			// Token: 0x04000E3F RID: 3647
			private static IntPtr Pointer;

			// Token: 0x04000E40 RID: 3648
			private static IntPtr DeferredCompilation;
		}
	}
}
