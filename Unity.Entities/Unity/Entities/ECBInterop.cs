using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;

namespace Unity.Entities
{
	// Token: 0x020000DD RID: 221
	[GenerateBurstMonoInterop("ECBInterop")]
	[BurstCompile]
	internal struct ECBInterop
	{
		// Token: 0x060003DC RID: 988 RVA: 0x00016D4C File Offset: 0x00014F4C
		[BurstMonoInteropMethod(false)]
		[BurstDiscard]
		internal unsafe static void _RemoveManagedReferences(EntityDataAccess* mgr, int* sharedIndex, int count)
		{
			try
			{
				for (int i = 0; i < count; i++)
				{
					int num = sharedIndex[i];
					if (EntityComponentStore.IsUnmanagedSharedComponentIndex(num))
					{
						mgr->EntityComponentStore->RemoveSharedComponentReference_Unmanaged(num, 1);
					}
					else
					{
						mgr->ManagedComponentStore.RemoveSharedComponentReference_Managed(num, 1);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
		}

		// Token: 0x060003DD RID: 989 RVA: 0x00016DAC File Offset: 0x00014FAC
		[BurstMonoInteropMethod(false)]
		[BurstDiscard]
		internal unsafe static void _CleanupManaged(EntityCommandBufferChain* chain)
		{
			for (EntityComponentGCNode* ptr = chain->m_Cleanup->CleanupList; ptr != null; ptr = ptr->Prev)
			{
				ptr->BoxedObject.Free();
			}
			chain->m_Cleanup->CleanupList = null;
		}

		// Token: 0x060003DE RID: 990 RVA: 0x00016DEB File Offset: 0x00014FEB
		[BurstMonoInteropMethod(false)]
		[BurstDiscard]
		internal unsafe static void _ProcessManagedCommand(void* processor, int processorType, BasicCommand* header)
		{
			if (processorType == 0)
			{
				EntityCommandBuffer.ProcessManagedCommand<EntityCommandBuffer.PlaybackProcessor>((EntityCommandBuffer.PlaybackProcessor*)processor, header);
				return;
			}
			if (processorType == 1)
			{
				EntityCommandBuffer.ProcessManagedCommand<EntityCommandBuffer.DebugViewProcessor>((EntityCommandBuffer.DebugViewProcessor*)processor, header);
				return;
			}
			if (processorType == 2)
			{
				EntityCommandBuffer.ProcessManagedCommand<EntityCommandBuffer.PlaybackWithTraceProcessor>((EntityCommandBuffer.PlaybackWithTraceProcessor*)processor, header);
				return;
			}
			if (processorType == 3)
			{
				EntityCommandBuffer.ProcessManagedCommand<EntityCommandBuffer.PrePlaybackValidationProcessor>((EntityCommandBuffer.PrePlaybackValidationProcessor*)processor, header);
			}
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00016E1C File Offset: 0x0001501C
		[BurstMonoInteropMethod(false)]
		internal unsafe static void _ProcessChainChunk(void* walker, int processorType, ECBChainPlaybackState* chainStates, int currentChain, int nextChain)
		{
			if (processorType == 0)
			{
				((EntityCommandBuffer.EcbWalker<EntityCommandBuffer.PlaybackProcessor>*)walker)->ProcessChain(chainStates, currentChain, nextChain);
				return;
			}
			if (processorType == 1)
			{
				((EntityCommandBuffer.EcbWalker<EntityCommandBuffer.DebugViewProcessor>*)walker)->ProcessChain(chainStates, currentChain, nextChain);
				return;
			}
			if (processorType == 2)
			{
				((EntityCommandBuffer.EcbWalker<EntityCommandBuffer.PlaybackWithTraceProcessor>*)walker)->ProcessChain(chainStates, currentChain, nextChain);
				return;
			}
			if (processorType == 3)
			{
				((EntityCommandBuffer.EcbWalker<EntityCommandBuffer.PrePlaybackValidationProcessor>*)walker)->ProcessChain(chainStates, currentChain, nextChain);
			}
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00016E6B File Offset: 0x0001506B
		[BurstDiscard]
		private static void CheckDelegate(ref bool useDelegate)
		{
			useDelegate = true;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x00016E70 File Offset: 0x00015070
		private static bool UseDelegate()
		{
			bool flag = false;
			ECBInterop.CheckDelegate(ref flag);
			return flag;
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00016E88 File Offset: 0x00015088
		[ExcludeFromBurstCompatTesting("Uses managed delegates")]
		internal unsafe static void Initialize()
		{
			if (ECBInterop.Managed._initialized)
			{
				return;
			}
			ECBInterop.Managed._initialized = true;
			ECBInterop.Managed._bfp_ProcessChainChunk = BurstCompiler.CompileFunctionPointer<ECBInterop.Managed._dlg_ProcessChainChunk>(new ECBInterop.Managed._dlg_ProcessChainChunk(ECBInterop._mono_to_burst_ProcessChainChunk)).Invoke;
			ECBInterop.Managed._dlg_RemoveManagedReferences dlg_RemoveManagedReferences = new ECBInterop.Managed._dlg_RemoveManagedReferences(ECBInterop._wrapper_RemoveManagedReferences);
			ECBInterop.Managed._gcDefeat_RemoveManagedReferences = dlg_RemoveManagedReferences;
			*ECBInterop._bfp_RemoveManagedReferences.Data = Marshal.GetFunctionPointerForDelegate<ECBInterop.Managed._dlg_RemoveManagedReferences>(dlg_RemoveManagedReferences);
			ECBInterop.Managed._dlg_CleanupManaged dlg_CleanupManaged = new ECBInterop.Managed._dlg_CleanupManaged(ECBInterop._wrapper_CleanupManaged);
			ECBInterop.Managed._gcDefeat_CleanupManaged = dlg_CleanupManaged;
			*ECBInterop._bfp_CleanupManaged.Data = Marshal.GetFunctionPointerForDelegate<ECBInterop.Managed._dlg_CleanupManaged>(dlg_CleanupManaged);
			ECBInterop.Managed._dlg_ProcessManagedCommand dlg_ProcessManagedCommand = new ECBInterop.Managed._dlg_ProcessManagedCommand(ECBInterop._wrapper_ProcessManagedCommand);
			ECBInterop.Managed._gcDefeat_ProcessManagedCommand = dlg_ProcessManagedCommand;
			*ECBInterop._bfp_ProcessManagedCommand.Data = Marshal.GetFunctionPointerForDelegate<ECBInterop.Managed._dlg_ProcessManagedCommand>(dlg_ProcessManagedCommand);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00016F2D File Offset: 0x0001512D
		internal unsafe static void ProcessChainChunk(void* walker, int processorType, ECBChainPlaybackState* chainStates, int currentChain, int nextChain)
		{
			if (ECBInterop.UseDelegate())
			{
				ECBInterop._forward_mono_ProcessChainChunk(walker, processorType, chainStates, currentChain, nextChain);
				return;
			}
			ECBInterop._ProcessChainChunk(walker, processorType, chainStates, currentChain, nextChain);
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00016F4D File Offset: 0x0001514D
		[BurstCompile]
		[MonoPInvokeCallback(typeof(ECBInterop.Managed._dlg_ProcessChainChunk))]
		private static void _mono_to_burst_ProcessChainChunk(IntPtr walker, int processorType, IntPtr chainStates, int currentChain, int nextChain)
		{
			ECBInterop._mono_to_burst_ProcessChainChunk_0000036B$BurstDirectCall.Invoke(walker, processorType, chainStates, currentChain, nextChain);
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x00016F5A File Offset: 0x0001515A
		[BurstDiscard]
		private unsafe static void _forward_mono_ProcessChainChunk(void* walker, int processorType, ECBChainPlaybackState* chainStates, int currentChain, int nextChain)
		{
			ECBInterop.Managed._bfp_ProcessChainChunk((IntPtr)walker, processorType, (IntPtr)((void*)chainStates), currentChain, nextChain);
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00016F78 File Offset: 0x00015178
		internal unsafe static void RemoveManagedReferences(EntityDataAccess* mgr, int* sharedIndex, int count)
		{
			if (!ECBInterop.UseDelegate())
			{
				FunctionPointer<ECBInterop.Managed._dlg_RemoveManagedReferences> functionPointer = new FunctionPointer<ECBInterop.Managed._dlg_RemoveManagedReferences>(*ECBInterop._bfp_RemoveManagedReferences.Data);
				functionPointer.Invoke((IntPtr)((void*)mgr), (IntPtr)((void*)sharedIndex), count);
				return;
			}
			ECBInterop._RemoveManagedReferences(mgr, sharedIndex, count);
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x00016FC0 File Offset: 0x000151C0
		[MonoPInvokeCallback(typeof(ECBInterop.Managed._dlg_RemoveManagedReferences))]
		private unsafe static void _wrapper_RemoveManagedReferences(IntPtr mgr, IntPtr sharedIndex, int count)
		{
			ECBInterop._RemoveManagedReferences((EntityDataAccess*)(void*)mgr, (int*)(void*)sharedIndex, count);
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x00016FD4 File Offset: 0x000151D4
		internal unsafe static void CleanupManaged(EntityCommandBufferChain* chain)
		{
			if (!ECBInterop.UseDelegate())
			{
				FunctionPointer<ECBInterop.Managed._dlg_CleanupManaged> functionPointer = new FunctionPointer<ECBInterop.Managed._dlg_CleanupManaged>(*ECBInterop._bfp_CleanupManaged.Data);
				functionPointer.Invoke((IntPtr)((void*)chain));
				return;
			}
			ECBInterop._CleanupManaged(chain);
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00017013 File Offset: 0x00015213
		[MonoPInvokeCallback(typeof(ECBInterop.Managed._dlg_CleanupManaged))]
		private unsafe static void _wrapper_CleanupManaged(IntPtr chain)
		{
			ECBInterop._CleanupManaged((EntityCommandBufferChain*)(void*)chain);
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x00017020 File Offset: 0x00015220
		internal unsafe static void ProcessManagedCommand(void* processor, int processorType, BasicCommand* header)
		{
			if (!ECBInterop.UseDelegate())
			{
				FunctionPointer<ECBInterop.Managed._dlg_ProcessManagedCommand> functionPointer = new FunctionPointer<ECBInterop.Managed._dlg_ProcessManagedCommand>(*ECBInterop._bfp_ProcessManagedCommand.Data);
				functionPointer.Invoke((IntPtr)processor, processorType, (IntPtr)((void*)header));
				return;
			}
			ECBInterop._ProcessManagedCommand(processor, processorType, header);
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00017068 File Offset: 0x00015268
		[MonoPInvokeCallback(typeof(ECBInterop.Managed._dlg_ProcessManagedCommand))]
		private unsafe static void _wrapper_ProcessManagedCommand(IntPtr processor, int processorType, IntPtr header)
		{
			ECBInterop._ProcessManagedCommand((void*)processor, processorType, (BasicCommand*)(void*)header);
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x000170E0 File Offset: 0x000152E0
		[BurstCompile]
		[MonoPInvokeCallback(typeof(ECBInterop.Managed._dlg_ProcessChainChunk))]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void _mono_to_burst_ProcessChainChunk$BurstManaged(IntPtr walker, int processorType, IntPtr chainStates, int currentChain, int nextChain)
		{
			ECBInterop._ProcessChainChunk((void*)walker, processorType, (ECBChainPlaybackState*)(void*)chainStates, currentChain, nextChain);
		}

		// Token: 0x04000253 RID: 595
		public static readonly SharedStatic<IntPtr> _bfp_RemoveManagedReferences = SharedStatic<IntPtr>.GetOrCreateUnsafe(0U, -7444679374746066657L, 0L);

		// Token: 0x04000254 RID: 596
		public static readonly SharedStatic<IntPtr> _bfp_CleanupManaged = SharedStatic<IntPtr>.GetOrCreateUnsafe(0U, -6741787120775434015L, 0L);

		// Token: 0x04000255 RID: 597
		public static readonly SharedStatic<IntPtr> _bfp_ProcessManagedCommand = SharedStatic<IntPtr>.GetOrCreateUnsafe(0U, -7891320699714491227L, 0L);

		// Token: 0x020000DE RID: 222
		private static class Managed
		{
			// Token: 0x04000256 RID: 598
			public static bool _initialized;

			// Token: 0x04000257 RID: 599
			public static ECBInterop.Managed._dlg_ProcessChainChunk _bfp_ProcessChainChunk;

			// Token: 0x04000258 RID: 600
			public static object _gcDefeat_RemoveManagedReferences;

			// Token: 0x04000259 RID: 601
			public static object _gcDefeat_CleanupManaged;

			// Token: 0x0400025A RID: 602
			public static object _gcDefeat_ProcessManagedCommand;

			// Token: 0x020000DF RID: 223
			// (Invoke) Token: 0x060003EF RID: 1007
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate void _dlg_ProcessChainChunk(IntPtr walker, int processorType, IntPtr chainStates, int currentChain, int nextChain);

			// Token: 0x020000E0 RID: 224
			// (Invoke) Token: 0x060003F3 RID: 1011
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate void _dlg_RemoveManagedReferences(IntPtr mgr, IntPtr sharedIndex, int count);

			// Token: 0x020000E1 RID: 225
			// (Invoke) Token: 0x060003F7 RID: 1015
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate void _dlg_CleanupManaged(IntPtr chain);

			// Token: 0x020000E2 RID: 226
			// (Invoke) Token: 0x060003FB RID: 1019
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate void _dlg_ProcessManagedCommand(IntPtr processor, int processorType, IntPtr header);
		}

		// Token: 0x020000E3 RID: 227
		private struct TagType_RemoveManagedReferences
		{
		}

		// Token: 0x020000E4 RID: 228
		private struct TagType_CleanupManaged
		{
		}

		// Token: 0x020000E5 RID: 229
		private struct TagType_ProcessManagedCommand
		{
		}

		// Token: 0x020000E6 RID: 230
		// (Invoke) Token: 0x060003FF RID: 1023
		internal delegate void _mono_to_burst_ProcessChainChunk_0000036B$PostfixBurstDelegate(IntPtr walker, int processorType, IntPtr chainStates, int currentChain, int nextChain);

		// Token: 0x020000E7 RID: 231
		internal static class _mono_to_burst_ProcessChainChunk_0000036B$BurstDirectCall
		{
			// Token: 0x06000402 RID: 1026 RVA: 0x000170F7 File Offset: 0x000152F7
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (ECBInterop._mono_to_burst_ProcessChainChunk_0000036B$BurstDirectCall.Pointer == 0)
				{
					ECBInterop._mono_to_burst_ProcessChainChunk_0000036B$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(ECBInterop._mono_to_burst_ProcessChainChunk_0000036B$BurstDirectCall.DeferredCompilation, methodof(ECBInterop._mono_to_burst_ProcessChainChunk$BurstManaged(IntPtr, int, IntPtr, int, int)).MethodHandle, typeof(ECBInterop._mono_to_burst_ProcessChainChunk_0000036B$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = ECBInterop._mono_to_burst_ProcessChainChunk_0000036B$BurstDirectCall.Pointer;
			}

			// Token: 0x06000403 RID: 1027 RVA: 0x00017124 File Offset: 0x00015324
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				ECBInterop._mono_to_burst_ProcessChainChunk_0000036B$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000404 RID: 1028 RVA: 0x0001713C File Offset: 0x0001533C
			public static void Constructor()
			{
				ECBInterop._mono_to_burst_ProcessChainChunk_0000036B$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(ECBInterop._mono_to_burst_ProcessChainChunk(IntPtr, int, IntPtr, int, int)).MethodHandle);
			}

			// Token: 0x06000405 RID: 1029 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06000406 RID: 1030 RVA: 0x0001714D File Offset: 0x0001534D
			// Note: this type is marked as 'beforefieldinit'.
			static _mono_to_burst_ProcessChainChunk_0000036B$BurstDirectCall()
			{
				ECBInterop._mono_to_burst_ProcessChainChunk_0000036B$BurstDirectCall.Constructor();
			}

			// Token: 0x06000407 RID: 1031 RVA: 0x00017154 File Offset: 0x00015354
			public static void Invoke(IntPtr walker, int processorType, IntPtr chainStates, int currentChain, int nextChain)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = ECBInterop._mono_to_burst_ProcessChainChunk_0000036B$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(System.IntPtr,System.Int32,System.IntPtr,System.Int32,System.Int32), walker, processorType, chainStates, currentChain, nextChain, functionPointer);
						return;
					}
				}
				ECBInterop._mono_to_burst_ProcessChainChunk$BurstManaged(walker, processorType, chainStates, currentChain, nextChain);
			}

			// Token: 0x0400025B RID: 603
			private static IntPtr Pointer;

			// Token: 0x0400025C RID: 604
			private static IntPtr DeferredCompilation;
		}
	}
}
