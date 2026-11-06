using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEngine.VFX
{
	// Token: 0x02000015 RID: 21
	[StaticAccessor("GetVFXManager()", StaticAccessorType.Dot)]
	[NativeHeader("Modules/VFX/Public/ScriptBindings/VFXManagerBindings.h")]
	[NativeHeader("Modules/VFX/Public/VFXManager.h")]
	[RequiredByNativeCode]
	public static class VFXManager
	{
		// Token: 0x06000068 RID: 104
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern VisualEffect[] GetComponents();

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000069 RID: 105
		internal static extern ScriptableObject runtimeResources
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600006A RID: 106
		// (set) Token: 0x0600006B RID: 107
		public static extern float fixedTimeStep
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600006C RID: 108
		// (set) Token: 0x0600006D RID: 109
		public static extern float maxDeltaTime
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600006E RID: 110
		// (set) Token: 0x0600006F RID: 111
		internal static extern float maxScrubTime
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000070 RID: 112
		internal static extern string renderPipeSettingsPath
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000071 RID: 113
		// (set) Token: 0x06000072 RID: 114
		internal static extern uint batchEmptyLifetime
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06000073 RID: 115
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void CleanupEmptyBatches(bool force = false);

		// Token: 0x06000074 RID: 116 RVA: 0x0000274A File Offset: 0x0000094A
		public static void FlushEmptyBatches()
		{
			VFXManager.CleanupEmptyBatches(true);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00002754 File Offset: 0x00000954
		public static VFXBatchedEffectInfo GetBatchedEffectInfo([NotNull("NullExceptionObject")] VisualEffectAsset vfx)
		{
			VFXBatchedEffectInfo vfxbatchedEffectInfo;
			VFXManager.GetBatchedEffectInfo_Injected(vfx, out vfxbatchedEffectInfo);
			return vfxbatchedEffectInfo;
		}

		// Token: 0x06000076 RID: 118
		[FreeFunction(Name = "VFXManagerBindings::GetBatchedEffectInfos", HasExplicitThis = false)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void GetBatchedEffectInfos([NotNull("NullExceptionObject")] List<VFXBatchedEffectInfo> infos);

		// Token: 0x06000077 RID: 119 RVA: 0x0000276C File Offset: 0x0000096C
		internal static VFXBatchInfo GetBatchInfo(VisualEffectAsset vfx, uint batchIndex)
		{
			VFXBatchInfo vfxbatchInfo;
			VFXManager.GetBatchInfo_Injected(vfx, batchIndex, out vfxbatchInfo);
			return vfxbatchInfo;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00002783 File Offset: 0x00000983
		[Obsolete("Use explicit PrepareCamera and ProcessCameraCommand instead")]
		public static void ProcessCamera(Camera cam)
		{
			VFXManager.PrepareCamera(cam, VFXManager.kDefaultCameraXRSettings);
			VFXManager.Internal_ProcessCameraCommand(cam, null, VFXManager.kDefaultCameraXRSettings, IntPtr.Zero);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000027A4 File Offset: 0x000009A4
		public static void PrepareCamera(Camera cam)
		{
			VFXManager.PrepareCamera(cam, VFXManager.kDefaultCameraXRSettings);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000027B3 File Offset: 0x000009B3
		public static void PrepareCamera([NotNull("NullExceptionObject")] Camera cam, VFXCameraXRSettings camXRSettings)
		{
			VFXManager.PrepareCamera_Injected(cam, ref camXRSettings);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000027BD File Offset: 0x000009BD
		[Obsolete("Use ProcessCameraCommand with CullingResults to allow culling of VFX per camera")]
		public static void ProcessCameraCommand(Camera cam, CommandBuffer cmd)
		{
			VFXManager.Internal_ProcessCameraCommand(cam, cmd, VFXManager.kDefaultCameraXRSettings, IntPtr.Zero);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000027D2 File Offset: 0x000009D2
		[Obsolete("Use ProcessCameraCommand with CullingResults to allow culling of VFX per camera")]
		public static void ProcessCameraCommand(Camera cam, CommandBuffer cmd, VFXCameraXRSettings camXRSettings)
		{
			VFXManager.Internal_ProcessCameraCommand(cam, cmd, camXRSettings, IntPtr.Zero);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000027E3 File Offset: 0x000009E3
		public static void ProcessCameraCommand(Camera cam, CommandBuffer cmd, VFXCameraXRSettings camXRSettings, CullingResults results)
		{
			VFXManager.Internal_ProcessCameraCommand(cam, cmd, camXRSettings, results.ptr);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000027F5 File Offset: 0x000009F5
		private static void Internal_ProcessCameraCommand([NotNull("NullExceptionObject")] Camera cam, CommandBuffer cmd, VFXCameraXRSettings camXRSettings, IntPtr cullResults)
		{
			VFXManager.Internal_ProcessCameraCommand_Injected(cam, cmd, ref camXRSettings, cullResults);
		}

		// Token: 0x0600007F RID: 127
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern VFXCameraBufferTypes IsCameraBufferNeeded([NotNull("NullExceptionObject")] Camera cam);

		// Token: 0x06000080 RID: 128
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetCameraBuffer([NotNull("NullExceptionObject")] Camera cam, VFXCameraBufferTypes type, Texture buffer, int x, int y, int width, int height);

		// Token: 0x06000082 RID: 130
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetBatchedEffectInfo_Injected(VisualEffectAsset vfx, out VFXBatchedEffectInfo ret);

		// Token: 0x06000083 RID: 131
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetBatchInfo_Injected(VisualEffectAsset vfx, uint batchIndex, out VFXBatchInfo ret);

		// Token: 0x06000084 RID: 132
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void PrepareCamera_Injected(Camera cam, ref VFXCameraXRSettings camXRSettings);

		// Token: 0x06000085 RID: 133
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_ProcessCameraCommand_Injected(Camera cam, CommandBuffer cmd, ref VFXCameraXRSettings camXRSettings, IntPtr cullResults);

		// Token: 0x04000115 RID: 277
		private static readonly VFXCameraXRSettings kDefaultCameraXRSettings = new VFXCameraXRSettings
		{
			viewTotal = 1U,
			viewCount = 1U,
			viewOffset = 0U
		};
	}
}
