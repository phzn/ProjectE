using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.XR
{
	// Token: 0x02000030 RID: 48
	[NativeHeader("Modules/XR/XRPrefix.h")]
	[NativeHeader("Modules/XR/Subsystems/Meshing/XRMeshingSubsystem.h")]
	[NativeConditional("ENABLE_XR")]
	[UsedByNativeCode]
	public class XRMeshSubsystem : IntegratedSubsystem<XRMeshSubsystemDescriptor>
	{
		// Token: 0x06000169 RID: 361 RVA: 0x00004CEC File Offset: 0x00002EEC
		public bool TryGetMeshInfos(List<MeshInfo> meshInfosOut)
		{
			bool flag = meshInfosOut == null;
			if (flag)
			{
				throw new ArgumentNullException("meshInfosOut");
			}
			return this.GetMeshInfosAsList(meshInfosOut);
		}

		// Token: 0x0600016A RID: 362
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool GetMeshInfosAsList(List<MeshInfo> meshInfos);

		// Token: 0x0600016B RID: 363
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern MeshInfo[] GetMeshInfosAsFixedArray();

		// Token: 0x0600016C RID: 364 RVA: 0x00004D18 File Offset: 0x00002F18
		public void GenerateMeshAsync(MeshId meshId, Mesh mesh, MeshCollider meshCollider, MeshVertexAttributes attributes, Action<MeshGenerationResult> onMeshGenerationComplete)
		{
			this.GenerateMeshAsync(meshId, mesh, meshCollider, attributes, onMeshGenerationComplete, MeshGenerationOptions.None);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00004D2A File Offset: 0x00002F2A
		public void GenerateMeshAsync(MeshId meshId, Mesh mesh, MeshCollider meshCollider, MeshVertexAttributes attributes, Action<MeshGenerationResult> onMeshGenerationComplete, MeshGenerationOptions options)
		{
			this.GenerateMeshAsync_Injected(ref meshId, mesh, meshCollider, attributes, onMeshGenerationComplete, options);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00004D3C File Offset: 0x00002F3C
		[RequiredByNativeCode]
		private void InvokeMeshReadyDelegate(MeshGenerationResult result, Action<MeshGenerationResult> onMeshGenerationComplete)
		{
			bool flag = onMeshGenerationComplete != null;
			if (flag)
			{
				onMeshGenerationComplete(result);
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600016F RID: 367
		// (set) Token: 0x06000170 RID: 368
		public extern float meshDensity
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00004D5A File Offset: 0x00002F5A
		public bool SetBoundingVolume(Vector3 origin, Vector3 extents)
		{
			return this.SetBoundingVolume_Injected(ref origin, ref extents);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00004D68 File Offset: 0x00002F68
		public unsafe NativeArray<MeshTransform> GetUpdatedMeshTransforms(Allocator allocator)
		{
			NativeArray<MeshTransform> nativeArray2;
			using (XRMeshSubsystem.MeshTransformList meshTransformList = new XRMeshSubsystem.MeshTransformList(this.GetUpdatedMeshTransforms()))
			{
				NativeArray<MeshTransform> nativeArray = new NativeArray<MeshTransform>(meshTransformList.Count, allocator, NativeArrayOptions.UninitializedMemory);
				UnsafeUtility.MemCpy(nativeArray.GetUnsafePtr<MeshTransform>(), meshTransformList.Data.ToPointer(), (long)(meshTransformList.Count * sizeof(MeshTransform)));
				nativeArray2 = nativeArray;
			}
			return nativeArray2;
		}

		// Token: 0x06000173 RID: 371
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern IntPtr GetUpdatedMeshTransforms();

		// Token: 0x06000175 RID: 373
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GenerateMeshAsync_Injected(ref MeshId meshId, Mesh mesh, MeshCollider meshCollider, MeshVertexAttributes attributes, Action<MeshGenerationResult> onMeshGenerationComplete, MeshGenerationOptions options);

		// Token: 0x06000176 RID: 374
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool SetBoundingVolume_Injected(ref Vector3 origin, ref Vector3 extents);

		// Token: 0x02000031 RID: 49
		[NativeConditional("ENABLE_XR")]
		private readonly struct MeshTransformList : IDisposable
		{
			// Token: 0x06000177 RID: 375 RVA: 0x00004DED File Offset: 0x00002FED
			public MeshTransformList(IntPtr self)
			{
				this.m_Self = self;
			}

			// Token: 0x1700004B RID: 75
			// (get) Token: 0x06000178 RID: 376 RVA: 0x00004DF6 File Offset: 0x00002FF6
			public int Count
			{
				get
				{
					return XRMeshSubsystem.MeshTransformList.GetLength(this.m_Self);
				}
			}

			// Token: 0x1700004C RID: 76
			// (get) Token: 0x06000179 RID: 377 RVA: 0x00004E03 File Offset: 0x00003003
			public IntPtr Data
			{
				get
				{
					return XRMeshSubsystem.MeshTransformList.GetData(this.m_Self);
				}
			}

			// Token: 0x0600017A RID: 378 RVA: 0x00004E10 File Offset: 0x00003010
			public void Dispose()
			{
				XRMeshSubsystem.MeshTransformList.Dispose(this.m_Self);
			}

			// Token: 0x0600017B RID: 379
			[FreeFunction("UnityXRMeshTransformList_get_Length")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			private static extern int GetLength(IntPtr self);

			// Token: 0x0600017C RID: 380
			[FreeFunction("UnityXRMeshTransformList_get_Data")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			private static extern IntPtr GetData(IntPtr self);

			// Token: 0x0600017D RID: 381
			[FreeFunction("UnityXRMeshTransformList_Dispose")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			private static extern void Dispose(IntPtr self);

			// Token: 0x0400011A RID: 282
			private readonly IntPtr m_Self;
		}
	}
}
