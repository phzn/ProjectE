using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Profiling;
using UnityEngine.Bindings;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x02000421 RID: 1057
	[NativeHeader("ModuleOverrides/com.unity.ui/Core/Native/Renderer/UIRendererUtility.h")]
	[VisibleToOtherModules(new string[] { "Unity.UIElements" })]
	internal class Utility
	{
		// Token: 0x06002199 RID: 8601 RVA: 0x0007F3B0 File Offset: 0x0007D5B0
		public static void SetVectorArray<T>(MaterialPropertyBlock props, int name, NativeSlice<T> vector4s) where T : struct
		{
			int num = vector4s.Length * vector4s.Stride / 16;
			Utility.SetVectorArray(props, name, new IntPtr(vector4s.GetUnsafePtr<T>()), num);
		}

		// Token: 0x14000042 RID: 66
		// (add) Token: 0x0600219A RID: 8602 RVA: 0x0007F3E8 File Offset: 0x0007D5E8
		// (remove) Token: 0x0600219B RID: 8603 RVA: 0x0007F41C File Offset: 0x0007D61C
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<bool> GraphicsResourcesRecreate;

		// Token: 0x14000043 RID: 67
		// (add) Token: 0x0600219C RID: 8604 RVA: 0x0007F450 File Offset: 0x0007D650
		// (remove) Token: 0x0600219D RID: 8605 RVA: 0x0007F484 File Offset: 0x0007D684
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action EngineUpdate;

		// Token: 0x14000044 RID: 68
		// (add) Token: 0x0600219E RID: 8606 RVA: 0x0007F4B8 File Offset: 0x0007D6B8
		// (remove) Token: 0x0600219F RID: 8607 RVA: 0x0007F4EC File Offset: 0x0007D6EC
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action FlushPendingResources;

		// Token: 0x14000045 RID: 69
		// (add) Token: 0x060021A0 RID: 8608 RVA: 0x0007F520 File Offset: 0x0007D720
		// (remove) Token: 0x060021A1 RID: 8609 RVA: 0x0007F554 File Offset: 0x0007D754
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<Camera> RegisterIntermediateRenderers;

		// Token: 0x14000046 RID: 70
		// (add) Token: 0x060021A2 RID: 8610 RVA: 0x0007F588 File Offset: 0x0007D788
		// (remove) Token: 0x060021A3 RID: 8611 RVA: 0x0007F5BC File Offset: 0x0007D7BC
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<IntPtr> RenderNodeAdd;

		// Token: 0x14000047 RID: 71
		// (add) Token: 0x060021A4 RID: 8612 RVA: 0x0007F5F0 File Offset: 0x0007D7F0
		// (remove) Token: 0x060021A5 RID: 8613 RVA: 0x0007F624 File Offset: 0x0007D824
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<IntPtr> RenderNodeExecute;

		// Token: 0x14000048 RID: 72
		// (add) Token: 0x060021A6 RID: 8614 RVA: 0x0007F658 File Offset: 0x0007D858
		// (remove) Token: 0x060021A7 RID: 8615 RVA: 0x0007F68C File Offset: 0x0007D88C
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<IntPtr> RenderNodeCleanup;

		// Token: 0x060021A8 RID: 8616 RVA: 0x0007F6BF File Offset: 0x0007D8BF
		[RequiredByNativeCode]
		internal static void RaiseGraphicsResourcesRecreate(bool recreate)
		{
			Action<bool> graphicsResourcesRecreate = Utility.GraphicsResourcesRecreate;
			if (graphicsResourcesRecreate != null)
			{
				graphicsResourcesRecreate(recreate);
			}
		}

		// Token: 0x060021A9 RID: 8617 RVA: 0x0007F6D4 File Offset: 0x0007D8D4
		[RequiredByNativeCode]
		internal static void RaiseEngineUpdate()
		{
			bool flag = Utility.EngineUpdate != null;
			if (flag)
			{
				Utility.EngineUpdate();
			}
		}

		// Token: 0x060021AA RID: 8618 RVA: 0x0007F6FB File Offset: 0x0007D8FB
		[RequiredByNativeCode]
		internal static void RaiseFlushPendingResources()
		{
			Action flushPendingResources = Utility.FlushPendingResources;
			if (flushPendingResources != null)
			{
				flushPendingResources();
			}
		}

		// Token: 0x060021AB RID: 8619 RVA: 0x0007F70F File Offset: 0x0007D90F
		[RequiredByNativeCode]
		internal static void RaiseRegisterIntermediateRenderers(Camera camera)
		{
			Action<Camera> registerIntermediateRenderers = Utility.RegisterIntermediateRenderers;
			if (registerIntermediateRenderers != null)
			{
				registerIntermediateRenderers(camera);
			}
		}

		// Token: 0x060021AC RID: 8620 RVA: 0x0007F724 File Offset: 0x0007D924
		[RequiredByNativeCode]
		internal static void RaiseRenderNodeAdd(IntPtr userData)
		{
			Action<IntPtr> renderNodeAdd = Utility.RenderNodeAdd;
			if (renderNodeAdd != null)
			{
				renderNodeAdd(userData);
			}
		}

		// Token: 0x060021AD RID: 8621 RVA: 0x0007F739 File Offset: 0x0007D939
		[RequiredByNativeCode]
		internal static void RaiseRenderNodeExecute(IntPtr userData)
		{
			Action<IntPtr> renderNodeExecute = Utility.RenderNodeExecute;
			if (renderNodeExecute != null)
			{
				renderNodeExecute(userData);
			}
		}

		// Token: 0x060021AE RID: 8622 RVA: 0x0007F74E File Offset: 0x0007D94E
		[RequiredByNativeCode]
		internal static void RaiseRenderNodeCleanup(IntPtr userData)
		{
			Action<IntPtr> renderNodeCleanup = Utility.RenderNodeCleanup;
			if (renderNodeCleanup != null)
			{
				renderNodeCleanup(userData);
			}
		}

		// Token: 0x060021AF RID: 8623
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr AllocateBuffer(int elementCount, int elementStride, bool vertexBuffer);

		// Token: 0x060021B0 RID: 8624
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void FreeBuffer(IntPtr buffer);

		// Token: 0x060021B1 RID: 8625
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void UpdateBufferRanges(IntPtr buffer, IntPtr ranges, int rangeCount, int writeRangeStart, int writeRangeEnd);

		// Token: 0x060021B2 RID: 8626
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetVectorArray(MaterialPropertyBlock props, int name, IntPtr vector4s, int count);

		// Token: 0x060021B3 RID: 8627
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr GetVertexDeclaration(VertexAttributeDescriptor[] vertexAttributes);

		// Token: 0x060021B4 RID: 8628 RVA: 0x0007F764 File Offset: 0x0007D964
		public static void RegisterIntermediateRenderer(Camera camera, Material material, Matrix4x4 transform, Bounds aabb, int renderLayer, int shadowCasting, bool receiveShadows, int sameDistanceSortPriority, ulong sceneCullingMask, int rendererCallbackFlags, IntPtr userData, int userDataSize)
		{
			Utility.RegisterIntermediateRenderer_Injected(camera, material, ref transform, ref aabb, renderLayer, shadowCasting, receiveShadows, sameDistanceSortPriority, sceneCullingMask, rendererCallbackFlags, userData, userDataSize);
		}

		// Token: 0x060021B5 RID: 8629
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public unsafe static extern void DrawRanges(IntPtr ib, IntPtr* vertexStreams, int streamCount, IntPtr ranges, int rangeCount, IntPtr vertexDecl);

		// Token: 0x060021B6 RID: 8630
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetPropertyBlock(MaterialPropertyBlock props);

		// Token: 0x060021B7 RID: 8631 RVA: 0x0007F78C File Offset: 0x0007D98C
		[ThreadSafe]
		public static void SetScissorRect(RectInt scissorRect)
		{
			Utility.SetScissorRect_Injected(ref scissorRect);
		}

		// Token: 0x060021B8 RID: 8632
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DisableScissor();

		// Token: 0x060021B9 RID: 8633
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsScissorEnabled();

		// Token: 0x060021BA RID: 8634 RVA: 0x0007F795 File Offset: 0x0007D995
		[ThreadSafe]
		public static IntPtr CreateStencilState(StencilState stencilState)
		{
			return Utility.CreateStencilState_Injected(ref stencilState);
		}

		// Token: 0x060021BB RID: 8635
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetStencilState(IntPtr stencilState, int stencilRef);

		// Token: 0x060021BC RID: 8636
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool HasMappedBufferRange();

		// Token: 0x060021BD RID: 8637
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern uint InsertCPUFence();

		// Token: 0x060021BE RID: 8638
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool CPUFencePassed(uint fence);

		// Token: 0x060021BF RID: 8639
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void WaitForCPUFencePassed(uint fence);

		// Token: 0x060021C0 RID: 8640
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SyncRenderThread();

		// Token: 0x060021C1 RID: 8641 RVA: 0x0007F7A0 File Offset: 0x0007D9A0
		[ThreadSafe]
		public static RectInt GetActiveViewport()
		{
			RectInt rectInt;
			Utility.GetActiveViewport_Injected(out rectInt);
			return rectInt;
		}

		// Token: 0x060021C2 RID: 8642
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ProfileDrawChainBegin();

		// Token: 0x060021C3 RID: 8643
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ProfileDrawChainEnd();

		// Token: 0x060021C4 RID: 8644
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void NotifyOfUIREvents(bool subscribe);

		// Token: 0x060021C5 RID: 8645 RVA: 0x0007F7B8 File Offset: 0x0007D9B8
		[ThreadSafe]
		public static Matrix4x4 GetUnityProjectionMatrix()
		{
			Matrix4x4 matrix4x;
			Utility.GetUnityProjectionMatrix_Injected(out matrix4x);
			return matrix4x;
		}

		// Token: 0x060021C6 RID: 8646 RVA: 0x0007F7D0 File Offset: 0x0007D9D0
		[ThreadSafe]
		public static Matrix4x4 GetDeviceProjectionMatrix()
		{
			Matrix4x4 matrix4x;
			Utility.GetDeviceProjectionMatrix_Injected(out matrix4x);
			return matrix4x;
		}

		// Token: 0x060021C7 RID: 8647
		[ThreadSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool DebugIsMainThread();

		// Token: 0x060021CA RID: 8650
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RegisterIntermediateRenderer_Injected(Camera camera, Material material, ref Matrix4x4 transform, ref Bounds aabb, int renderLayer, int shadowCasting, bool receiveShadows, int sameDistanceSortPriority, ulong sceneCullingMask, int rendererCallbackFlags, IntPtr userData, int userDataSize);

		// Token: 0x060021CB RID: 8651
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetScissorRect_Injected(ref RectInt scissorRect);

		// Token: 0x060021CC RID: 8652
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr CreateStencilState_Injected(ref StencilState stencilState);

		// Token: 0x060021CD RID: 8653
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetActiveViewport_Injected(out RectInt ret);

		// Token: 0x060021CE RID: 8654
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetUnityProjectionMatrix_Injected(out Matrix4x4 ret);

		// Token: 0x060021CF RID: 8655
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetDeviceProjectionMatrix_Injected(out Matrix4x4 ret);

		// Token: 0x04000E5C RID: 3676
		private static ProfilerMarker s_MarkerRaiseEngineUpdate = new ProfilerMarker("UIR.RaiseEngineUpdate");

		// Token: 0x02000422 RID: 1058
		[Flags]
		internal enum RendererCallbacks
		{
			// Token: 0x04000E5E RID: 3678
			RendererCallback_Init = 1,
			// Token: 0x04000E5F RID: 3679
			RendererCallback_Exec = 2,
			// Token: 0x04000E60 RID: 3680
			RendererCallback_Cleanup = 4
		}

		// Token: 0x02000423 RID: 1059
		internal enum GPUBufferType
		{
			// Token: 0x04000E62 RID: 3682
			Vertex,
			// Token: 0x04000E63 RID: 3683
			Index
		}

		// Token: 0x02000424 RID: 1060
		public class GPUBuffer<T> : IDisposable where T : struct
		{
			// Token: 0x060021D0 RID: 8656 RVA: 0x0007F7F6 File Offset: 0x0007D9F6
			public GPUBuffer(int elementCount, Utility.GPUBufferType type)
			{
				this.elemCount = elementCount;
				this.elemStride = UnsafeUtility.SizeOf<T>();
				this.buffer = Utility.AllocateBuffer(elementCount, this.elemStride, type == Utility.GPUBufferType.Vertex);
			}

			// Token: 0x060021D1 RID: 8657 RVA: 0x0007F828 File Offset: 0x0007DA28
			public void Dispose()
			{
				Utility.FreeBuffer(this.buffer);
			}

			// Token: 0x060021D2 RID: 8658 RVA: 0x0007F837 File Offset: 0x0007DA37
			public void UpdateRanges(NativeSlice<GfxUpdateBufferRange> ranges, int rangesMin, int rangesMax)
			{
				Utility.UpdateBufferRanges(this.buffer, new IntPtr(ranges.GetUnsafePtr<GfxUpdateBufferRange>()), ranges.Length, rangesMin, rangesMax);
			}

			// Token: 0x170007C3 RID: 1987
			// (get) Token: 0x060021D3 RID: 8659 RVA: 0x0007F85C File Offset: 0x0007DA5C
			public int ElementStride
			{
				get
				{
					return this.elemStride;
				}
			}

			// Token: 0x170007C4 RID: 1988
			// (get) Token: 0x060021D4 RID: 8660 RVA: 0x0007F874 File Offset: 0x0007DA74
			public int Count
			{
				get
				{
					return this.elemCount;
				}
			}

			// Token: 0x170007C5 RID: 1989
			// (get) Token: 0x060021D5 RID: 8661 RVA: 0x0007F88C File Offset: 0x0007DA8C
			internal IntPtr BufferPointer
			{
				get
				{
					return this.buffer;
				}
			}

			// Token: 0x04000E64 RID: 3684
			private IntPtr buffer;

			// Token: 0x04000E65 RID: 3685
			private int elemCount;

			// Token: 0x04000E66 RID: 3686
			private int elemStride;
		}
	}
}
