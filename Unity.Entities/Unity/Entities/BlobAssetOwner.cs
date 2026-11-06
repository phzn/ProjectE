using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using UnityEngine.Scripting;

namespace Unity.Entities
{
	// Token: 0x0200001B RID: 27
	[BurstCompile]
	[StructLayout(LayoutKind.Explicit, Size = 8)]
	internal struct BlobAssetOwner : ISharedComponentData, IQueryTypeParameter, IRefCounted
	{
		// Token: 0x06000052 RID: 82 RVA: 0x00002CB3 File Offset: 0x00000EB3
		public unsafe BlobAssetOwner(void* buffer, int expectedTotalDataSize)
		{
			this.BlobAssetBatchPtr = BlobAssetBatch.CreateFromMemory(buffer, expectedTotalDataSize);
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00002CC2 File Offset: 0x00000EC2
		public bool IsCreated
		{
			get
			{
				return this.BlobAssetBatchPtr != null;
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002CD1 File Offset: 0x00000ED1
		[BurstCompile]
		public void Release()
		{
			if (this.BlobAssetBatchPtr != null)
			{
				BlobAssetBatch.Release(this.BlobAssetBatchPtr);
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002CE8 File Offset: 0x00000EE8
		[BurstCompile]
		public void Retain()
		{
			if (this.BlobAssetBatchPtr != null)
			{
				BlobAssetBatch.Retain(this.BlobAssetBatchPtr);
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002CFF File Offset: 0x00000EFF
		[BurstCompile]
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		[Preserve]
		public static void __codegen__Retain(IntPtr self)
		{
			BlobAssetOwner.__codegen__Retain_00000056$BurstDirectCall.Invoke(self);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002D07 File Offset: 0x00000F07
		[BurstCompile]
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		[Preserve]
		public static void __codegen__Release(IntPtr self)
		{
			BlobAssetOwner.__codegen__Release_00000057$BurstDirectCall.Invoke(self);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002D0F File Offset: 0x00000F0F
		[BurstCompile]
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		[Preserve]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void __codegen__Retain$BurstManaged(IntPtr self)
		{
			self.Retain();
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002D18 File Offset: 0x00000F18
		[BurstCompile]
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		[Preserve]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void __codegen__Release$BurstManaged(IntPtr self)
		{
			self.Release();
		}

		// Token: 0x04000022 RID: 34
		[FieldOffset(0)]
		public unsafe BlobAssetBatch* BlobAssetBatchPtr;

		// Token: 0x0200001C RID: 28
		// (Invoke) Token: 0x0600005B RID: 91
		internal delegate void __codegen__Retain_00000056$PostfixBurstDelegate(IntPtr self);

		// Token: 0x0200001D RID: 29
		internal static class __codegen__Retain_00000056$BurstDirectCall
		{
			// Token: 0x0600005E RID: 94 RVA: 0x00002D21 File Offset: 0x00000F21
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (BlobAssetOwner.__codegen__Retain_00000056$BurstDirectCall.Pointer == 0)
				{
					BlobAssetOwner.__codegen__Retain_00000056$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(BlobAssetOwner.__codegen__Retain_00000056$BurstDirectCall.DeferredCompilation, methodof(BlobAssetOwner.__codegen__Retain$BurstManaged(IntPtr)).MethodHandle, typeof(BlobAssetOwner.__codegen__Retain_00000056$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = BlobAssetOwner.__codegen__Retain_00000056$BurstDirectCall.Pointer;
			}

			// Token: 0x0600005F RID: 95 RVA: 0x00002D50 File Offset: 0x00000F50
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				BlobAssetOwner.__codegen__Retain_00000056$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000060 RID: 96 RVA: 0x00002D68 File Offset: 0x00000F68
			public static void Constructor()
			{
				BlobAssetOwner.__codegen__Retain_00000056$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(BlobAssetOwner.__codegen__Retain(IntPtr)).MethodHandle);
			}

			// Token: 0x06000061 RID: 97 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06000062 RID: 98 RVA: 0x00002D79 File Offset: 0x00000F79
			// Note: this type is marked as 'beforefieldinit'.
			static __codegen__Retain_00000056$BurstDirectCall()
			{
				BlobAssetOwner.__codegen__Retain_00000056$BurstDirectCall.Constructor();
			}

			// Token: 0x06000063 RID: 99 RVA: 0x00002D80 File Offset: 0x00000F80
			public static void Invoke(IntPtr self)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = BlobAssetOwner.__codegen__Retain_00000056$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(System.IntPtr), self, functionPointer);
						return;
					}
				}
				BlobAssetOwner.__codegen__Retain$BurstManaged(self);
			}

			// Token: 0x04000023 RID: 35
			private static IntPtr Pointer;

			// Token: 0x04000024 RID: 36
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200001E RID: 30
		// (Invoke) Token: 0x06000065 RID: 101
		internal delegate void __codegen__Release_00000057$PostfixBurstDelegate(IntPtr self);

		// Token: 0x0200001F RID: 31
		internal static class __codegen__Release_00000057$BurstDirectCall
		{
			// Token: 0x06000068 RID: 104 RVA: 0x00002DB1 File Offset: 0x00000FB1
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (BlobAssetOwner.__codegen__Release_00000057$BurstDirectCall.Pointer == 0)
				{
					BlobAssetOwner.__codegen__Release_00000057$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(BlobAssetOwner.__codegen__Release_00000057$BurstDirectCall.DeferredCompilation, methodof(BlobAssetOwner.__codegen__Release$BurstManaged(IntPtr)).MethodHandle, typeof(BlobAssetOwner.__codegen__Release_00000057$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = BlobAssetOwner.__codegen__Release_00000057$BurstDirectCall.Pointer;
			}

			// Token: 0x06000069 RID: 105 RVA: 0x00002DE0 File Offset: 0x00000FE0
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				BlobAssetOwner.__codegen__Release_00000057$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600006A RID: 106 RVA: 0x00002DF8 File Offset: 0x00000FF8
			public static void Constructor()
			{
				BlobAssetOwner.__codegen__Release_00000057$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(BlobAssetOwner.__codegen__Release(IntPtr)).MethodHandle);
			}

			// Token: 0x0600006B RID: 107 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x0600006C RID: 108 RVA: 0x00002E09 File Offset: 0x00001009
			// Note: this type is marked as 'beforefieldinit'.
			static __codegen__Release_00000057$BurstDirectCall()
			{
				BlobAssetOwner.__codegen__Release_00000057$BurstDirectCall.Constructor();
			}

			// Token: 0x0600006D RID: 109 RVA: 0x00002E10 File Offset: 0x00001010
			public static void Invoke(IntPtr self)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = BlobAssetOwner.__codegen__Release_00000057$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(System.IntPtr), self, functionPointer);
						return;
					}
				}
				BlobAssetOwner.__codegen__Release$BurstManaged(self);
			}

			// Token: 0x04000025 RID: 37
			private static IntPtr Pointer;

			// Token: 0x04000026 RID: 38
			private static IntPtr DeferredCompilation;
		}
	}
}
