using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AOT;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Core;
using Unity.Mathematics;

namespace Unity.Entities
{
	// Token: 0x0200035B RID: 859
	[BurstCompile]
	internal struct WorldUnmanagedImpl
	{
		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x060016F0 RID: 5872 RVA: 0x00056D8E File Offset: 0x00054F8E
		// (set) Token: 0x060016F1 RID: 5873 RVA: 0x00056D96 File Offset: 0x00054F96
		public int Version { readonly get; private set; }

		// Token: 0x060016F2 RID: 5874 RVA: 0x00056DA0 File Offset: 0x00054FA0
		internal void BumpVersion()
		{
			int version = this.Version;
			this.Version = version + 1;
		}

		// Token: 0x060016F3 RID: 5875 RVA: 0x00056DC0 File Offset: 0x00054FC0
		internal unsafe WorldUnmanagedImpl(World worldManaged, ulong sequenceNumber, WorldFlags flags, AllocatorHelper<AutoFreeAllocator> worldAllocatorHelper, string worldName)
		{
			this.m_WorldAllocatorHelper = worldAllocatorHelper;
			AllocatorManager.AllocatorHandle handle = worldAllocatorHelper.Allocator.Handle;
			this.CurrentTime = default(TimeData);
			this.Name = default(FixedString128Bytes);
			(ref this.Name).CopyFromTruncated<FixedString128Bytes>(worldName);
			this.SequenceNumber = sequenceNumber;
			this.MaximumDeltaTime = 0.33333334f;
			this.Flags = flags;
			this._unmanagedSlotByTypeHash = new UnsafeParallelMultiHashMap<long, ushort>(32, handle);
			this._stateMemory = default(WorldUnmanagedImpl.StateAllocator);
			this._stateMemory.Init();
			this.Version = 0;
			this.ExecutingSystem = default(SystemHandle);
			if (WorldUnmanagedImpl.s_UnmanagedUpdateFn == null)
			{
				WorldUnmanagedImpl.s_UnmanagedUpdateFn = BurstCompiler.CompileFunctionPointer<WorldUnmanagedImpl.UnmanagedUpdateSignature>(new WorldUnmanagedImpl.UnmanagedUpdateSignature(WorldUnmanagedImpl.UnmanagedUpdate)).Invoke;
			}
			this.GroupUpdateAllocators = (DoubleRewindableAllocators*)Memory.Unmanaged.Allocate((long)sizeof(DoubleRewindableAllocators), 64, handle);
			*this.GroupUpdateAllocators = new DoubleRewindableAllocators(handle, 131072);
			this.DoubleUpdateAllocators = this.GroupUpdateAllocators;
			this.m_SystemStatePtrMap = new NativeParallelHashMap<int, IntPtr>(32, Allocator.Persistent);
			this.sysHandlesInCreationOrder = new UnsafeList<PerWorldSystemInfo>(1, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			this.m_EntityManager = default(EntityManager);
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x060016F4 RID: 5876 RVA: 0x00056EE7 File Offset: 0x000550E7
		// (set) Token: 0x060016F5 RID: 5877 RVA: 0x00056EF4 File Offset: 0x000550F4
		internal unsafe bool UpdateAllocatorEnableBlockFree
		{
			get
			{
				return this.DoubleUpdateAllocators->EnableBlockFree;
			}
			set
			{
				this.DoubleUpdateAllocators->EnableBlockFree = value;
			}
		}

		// Token: 0x060016F6 RID: 5878 RVA: 0x00056F02 File Offset: 0x00055102
		internal unsafe void SetGroupAllocator(DoubleRewindableAllocators* newGroupAllocators)
		{
			if (newGroupAllocators != null)
			{
				this.DoubleUpdateAllocators = newGroupAllocators;
				return;
			}
		}

		// Token: 0x060016F7 RID: 5879 RVA: 0x00056F11 File Offset: 0x00055111
		internal unsafe void RestoreGroupAllocator(DoubleRewindableAllocators* oldGroupAllocators)
		{
			if (oldGroupAllocators != null)
			{
				if (this.DoubleUpdateAllocators != oldGroupAllocators)
				{
					this.DoubleUpdateAllocators->Update();
				}
				this.DoubleUpdateAllocators = oldGroupAllocators;
				return;
			}
		}

		// Token: 0x060016F8 RID: 5880 RVA: 0x00056F34 File Offset: 0x00055134
		[ExcludeFromBurstCompatTesting("DestroyInstance disposes managed lists")]
		internal unsafe void Dispose()
		{
			AllocatorManager.AllocatorHandle handle = this.m_WorldAllocatorHelper.Allocator.Handle;
			this.GroupUpdateAllocators->Dispose();
			Memory.Unmanaged.Free<DoubleRewindableAllocators>(this.GroupUpdateAllocators, handle);
			this.m_EntityManager.DestroyInstance();
			this.m_EntityManager = default(EntityManager);
			this._unmanagedSlotByTypeHash.Dispose();
			this._stateMemory.Dispose();
			this.m_SystemStatePtrMap.Dispose();
			this.sysHandlesInCreationOrder.Dispose();
		}

		// Token: 0x060016F9 RID: 5881 RVA: 0x00056FAC File Offset: 0x000551AC
		private unsafe void InvalidateSlot(ushort handle)
		{
			ushort num;
			WorldUnmanagedImpl.StateAllocator.IncVersion((ref this._stateMemory.GetBlock(handle, out num)->Version.FixedElementField) + (IntPtr)num * 2);
			int num2 = this.Version + 1;
			this.Version = num2;
		}

		// Token: 0x060016FA RID: 5882 RVA: 0x00056FEC File Offset: 0x000551EC
		internal unsafe void FreeSlotWithoutOnDestroy(ushort handle, SystemState* statePtr)
		{
			this.m_EntityManager.DestroyEntityInternal(&statePtr->SystemHandle.m_Entity, 1);
			Memory.Unmanaged.Free(statePtr->m_SystemPtr, Allocator.Persistent);
			this.m_SystemStatePtrMap.Remove(statePtr->m_SystemID);
			statePtr->Dispose();
			this._stateMemory.Free(handle);
		}

		// Token: 0x060016FB RID: 5883 RVA: 0x0005704C File Offset: 0x0005524C
		private unsafe void FreeSlot(ushort handle, SystemState* statePtr)
		{
			if (statePtr->m_SystemPtr != null)
			{
				WorldUnmanagedImpl.PreviousSystemGlobalState previousSystemGlobalState = new WorldUnmanagedImpl.PreviousSystemGlobalState(ref this, statePtr);
				try
				{
					if (statePtr->PreviouslyEnabled)
					{
						SystemBaseRegistry.CallOnStopRunning(statePtr);
					}
					SystemBaseRegistry.CallOnDestroy(statePtr);
					previousSystemGlobalState.Restore(ref this, statePtr);
					return;
				}
				finally
				{
					previousSystemGlobalState.Restore(ref this, statePtr);
					this.FreeSlotWithoutOnDestroy(handle, statePtr);
				}
			}
			this.FreeSlotWithoutOnDestroy(handle, statePtr);
		}

		// Token: 0x060016FC RID: 5884 RVA: 0x000570B4 File Offset: 0x000552B4
		private unsafe void FreeSlot(ushort handle)
		{
			SystemState* ptr = this._stateMemory.ResolveNoCheck(handle);
			this.FreeSlot(handle, ptr);
		}

		// Token: 0x060016FD RID: 5885 RVA: 0x000570D8 File Offset: 0x000552D8
		private unsafe SystemHandle GetExistingUnmanagedSystem(long typeHash)
		{
			ushort num;
			NativeParallelMultiHashMapIterator<long> nativeParallelMultiHashMapIterator;
			if (this._unmanagedSlotByTypeHash.TryGetFirstValue(typeHash, out num, out nativeParallelMultiHashMapIterator))
			{
				ushort num2;
				WorldUnmanagedImpl.StateAllocLevel1* block = this._stateMemory.GetBlock(num, out num2);
				return new SystemHandle(block->States[num2].SystemHandle.m_Entity, num, *((ref block->Version.FixedElementField) + (IntPtr)num2 * 2), (uint)this.SequenceNumber);
			}
			return SystemHandle.Null;
		}

		// Token: 0x060016FE RID: 5886 RVA: 0x00057144 File Offset: 0x00055344
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleSystem) })]
		internal SystemHandle GetExistingUnmanagedSystem<[global::System.Runtime.CompilerServices.IsUnmanaged] T>() where T : struct, ValueType, ISystem
		{
			return this.GetExistingUnmanagedSystem(Unity.Burst.BurstRuntime.GetHashCode64<T>());
		}

		// Token: 0x060016FF RID: 5887 RVA: 0x00057151 File Offset: 0x00055351
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleSystem) })]
		internal unsafe ref SystemState GetExistingSystemState<T>()
		{
			return ref *this.ResolveSystemState(this.GetExistingUnmanagedSystem(TypeManager.GetSystemTypeHash<T>()));
		}

		// Token: 0x06001700 RID: 5888 RVA: 0x00057164 File Offset: 0x00055364
		internal SystemHandle GetExistingUnmanagedSystem(Type t)
		{
			long hashCode = Unity.Burst.BurstRuntime.GetHashCode64(t);
			if (this._unmanagedSlotByTypeHash.ContainsKey(hashCode))
			{
				return this.GetExistingUnmanagedSystem(hashCode);
			}
			return SystemHandle.Null;
		}

		// Token: 0x06001701 RID: 5889 RVA: 0x00057194 File Offset: 0x00055394
		internal SystemHandle GetExistingUnmanagedSystem(SystemTypeIndex t)
		{
			long systemTypeHash = TypeManager.GetSystemTypeHash(t);
			if (this._unmanagedSlotByTypeHash.ContainsKey(systemTypeHash))
			{
				return this.GetExistingUnmanagedSystem(systemTypeHash);
			}
			return SystemHandle.Null;
		}

		// Token: 0x06001702 RID: 5890 RVA: 0x000571C3 File Offset: 0x000553C3
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckSysHandleVersion(SystemHandle sysHandle)
		{
			if (sysHandle.m_Version == 0)
			{
				throw new ArgumentException("sysHandle is invalid (default constructed)");
			}
		}

		// Token: 0x06001703 RID: 5891 RVA: 0x000571D8 File Offset: 0x000553D8
		internal void InvalidateSystemHandle(SystemHandle sysHandle)
		{
			long typeHashNoCheck = this._stateMemory.GetTypeHashNoCheck(sysHandle.m_Handle);
			this._unmanagedSlotByTypeHash.Remove<ushort>(typeHashNoCheck, sysHandle.m_Handle);
			this.InvalidateSlot(sysHandle.m_Handle);
		}

		// Token: 0x06001704 RID: 5892 RVA: 0x00057215 File Offset: 0x00055415
		internal unsafe void DestroyManagedSystem(SystemState* state)
		{
			this.InvalidateSystemHandle(state->m_Handle);
			this.FreeSlot(state->m_Handle.m_Handle, state);
		}

		// Token: 0x06001705 RID: 5893 RVA: 0x00057235 File Offset: 0x00055435
		internal void DestroyUnmanagedSystem(SystemHandle sysHandle)
		{
			this.InvalidateSystemHandle(sysHandle);
			this.FreeSlot(sysHandle.m_Handle);
		}

		// Token: 0x06001706 RID: 5894 RVA: 0x0005724C File Offset: 0x0005544C
		private unsafe SystemHandle CreateUnmanagedSystemInternal(World self, int structSize, long typeHash, int typeIndex, out void* systemPtr, bool callOnCreate)
		{
			int systemTypeMetaIndex = SystemBaseRegistry.GetSystemTypeMetaIndex(typeHash);
			if (systemTypeMetaIndex == -1)
			{
				throw new ArgumentException(string.Format("Type {0} couldn't be found in the SystemRegistry. (This is likely a bug in an ILPostprocessor.)", TypeManager.GetSystemName(typeIndex)));
			}
			ushort num;
			ushort num2;
			SystemState* ptr = this._stateMemory.Alloc(out num, out num2, typeHash);
			Entity entity = this.CreateSystemEntity(self, typeIndex, ptr);
			SystemHandle systemHandle = new SystemHandle(entity, num, num2, (uint)this.SequenceNumber);
			systemPtr = Memory.Unmanaged.Allocate((long)structSize, 16, Allocator.Persistent);
			UnsafeUtility.MemClear(systemPtr, (long)structSize);
			ptr->InitUnmanaged(self, systemHandle, systemTypeMetaIndex, systemPtr);
			int num3 = this.Version + 1;
			this.Version = num3;
			this._unmanagedSlotByTypeHash.Add(typeHash, num);
			this.m_SystemStatePtrMap.Add(ptr->m_SystemID, (IntPtr)((void*)ptr));
			if (callOnCreate)
			{
				this.CallSystemOnCreateWithCleanup(ptr);
			}
			PerWorldSystemInfo perWorldSystemInfo = default(PerWorldSystemInfo);
			perWorldSystemInfo.handle = systemHandle;
			perWorldSystemInfo.systemTypeIndex = typeIndex;
			this.sysHandlesInCreationOrder.Add(in perWorldSystemInfo);
			return systemHandle;
		}

		// Token: 0x06001707 RID: 5895 RVA: 0x00057348 File Offset: 0x00055548
		private unsafe Entity CreateSystemEntity(World self, int typeIndex, SystemState* statePtr)
		{
			ComponentType componentType = ComponentType.ReadWrite<SystemInstance>();
			Entity entity = self.EntityManager.CreateEntity(self.EntityManager.CreateArchetype(&componentType, 1));
			FixedString64Bytes fixedString64Bytes = default(FixedString64Bytes);
			NativeText.ReadOnly systemName = TypeManager.GetSystemName(typeIndex);
			(ref fixedString64Bytes).CopyFromTruncated<FixedString64Bytes, NativeText.ReadOnly>(in systemName);
			self.EntityManager.SetName(entity, fixedString64Bytes);
			self.EntityManager.SetComponentData<SystemInstance>(entity, new SystemInstance
			{
				state = statePtr
			});
			return entity;
		}

		// Token: 0x06001708 RID: 5896 RVA: 0x000573D0 File Offset: 0x000555D0
		internal unsafe SystemState* AllocateSystemStateForManagedSystem(World self, ComponentSystemBase system)
		{
			Type type = system.GetType();
			long hashCode = Unity.Burst.BurstRuntime.GetHashCode64(type);
			ushort num;
			ushort num2;
			SystemState* ptr = this._stateMemory.Alloc(out num, out num2, hashCode);
			Entity entity = default(Entity);
			if (!typeof(ComponentSystemGroup).IsAssignableFrom(type))
			{
				entity = this.CreateSystemEntity(self, TypeManager.GetSystemTypeIndex(type), ptr);
			}
			SystemHandle systemHandle = new SystemHandle(entity, num, num2, (uint)this.SequenceNumber);
			ptr->InitManaged(self, systemHandle, type, system);
			this._unmanagedSlotByTypeHash.Add(hashCode, num);
			this.m_SystemStatePtrMap.Add(ptr->m_SystemID, (IntPtr)((void*)ptr));
			int num3 = this.Version + 1;
			this.Version = num3;
			return ptr;
		}

		// Token: 0x06001709 RID: 5897 RVA: 0x00057480 File Offset: 0x00055680
		[ExcludeFromBurstCompatTesting("Takes managed World")]
		internal unsafe SystemHandle CreateUnmanagedSystem<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(World self, bool callOnCreate) where T : struct, ValueType, ISystem
		{
			int num = TypeManager.GetSystemTypeIndex<T>();
			void* ptr;
			return this.CreateUnmanagedSystemInternal(self, UnsafeUtility.SizeOf<T>(), Unity.Burst.BurstRuntime.GetHashCode64<T>(), num, out ptr, callOnCreate);
		}

		// Token: 0x0600170A RID: 5898 RVA: 0x000574B0 File Offset: 0x000556B0
		[ExcludeFromBurstCompatTesting("Uses managed World")]
		private unsafe SystemHandle CreateUnmanagedSystem(SystemTypeIndex t, long typeHash, bool callOnCreate)
		{
			void* ptr;
			return this.CreateUnmanagedSystemInternal(this.m_EntityManager.World, TypeManager.GetSystemTypeSize(t), typeHash, t, out ptr, callOnCreate);
		}

		// Token: 0x0600170B RID: 5899 RVA: 0x000574E0 File Offset: 0x000556E0
		internal unsafe void CallSystemOnCreateWithCleanup(SystemState* statePtr)
		{
			WorldUnmanagedImpl.PreviousSystemGlobalState previousSystemGlobalState = new WorldUnmanagedImpl.PreviousSystemGlobalState(ref this, statePtr);
			try
			{
				statePtr->m_EntityComponentStore->IncrementGlobalSystemVersion(in statePtr->m_Handle);
				SystemBaseRegistry.CallOnCreateForCompiler(statePtr);
				SystemBaseRegistry.CallOnCreate(statePtr);
				previousSystemGlobalState.Restore(ref this, statePtr);
				ref EntityComponentStore entityComponentStore = ref *statePtr->m_EntityComponentStore;
				SystemHandle systemHandle = default(SystemHandle);
				entityComponentStore.IncrementGlobalSystemVersion(in systemHandle);
			}
			catch
			{
				previousSystemGlobalState.Restore(ref this, statePtr);
				ushort handle = statePtr->m_Handle.m_Handle;
				long systemTypeHash = TypeManager.GetSystemTypeHash(SystemBaseRegistry.GetStructType(statePtr->UnmanagedMetaIndex));
				this._unmanagedSlotByTypeHash.Remove<ushort>(systemTypeHash, handle);
				for (int i = 0; i < this.sysHandlesInCreationOrder.Length; i++)
				{
					if (this.sysHandlesInCreationOrder[i].handle == statePtr->m_Handle)
					{
						this.sysHandlesInCreationOrder.RemoveAt(i);
						break;
					}
				}
				this.FreeSlotWithoutOnDestroy(handle, statePtr);
				throw;
			}
		}

		// Token: 0x0600170C RID: 5900 RVA: 0x000575CC File Offset: 0x000557CC
		[ExcludeFromBurstCompatTesting("Uses managed World")]
		internal SystemHandle GetOrCreateUnmanagedSystem<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(bool callOnCreate = true) where T : struct, ValueType, ISystem
		{
			if (this._unmanagedSlotByTypeHash.ContainsKey(Unity.Burst.BurstRuntime.GetHashCode64<T>()))
			{
				return this.GetExistingUnmanagedSystem<T>();
			}
			return this.CreateUnmanagedSystem<T>(this.m_EntityManager.World, callOnCreate);
		}

		// Token: 0x0600170D RID: 5901 RVA: 0x000575FC File Offset: 0x000557FC
		[ExcludeFromBurstCompatTesting("Uses managed World")]
		internal SystemHandle GetOrCreateUnmanagedSystem(SystemTypeIndex t, bool callOnCreate = true)
		{
			long systemTypeHash = TypeManager.GetSystemTypeHash(t);
			if (this._unmanagedSlotByTypeHash.ContainsKey(systemTypeHash))
			{
				return this.GetExistingUnmanagedSystem(systemTypeHash);
			}
			return this.CreateUnmanagedSystem(t, systemTypeHash, callOnCreate);
		}

		// Token: 0x0600170E RID: 5902 RVA: 0x0005762F File Offset: 0x0005582F
		internal SystemHandle CreateUnmanagedSystem(Type t, bool callOnCreate)
		{
			return this.CreateUnmanagedSystem(TypeManager.GetSystemTypeIndex(t), callOnCreate);
		}

		// Token: 0x0600170F RID: 5903 RVA: 0x00057640 File Offset: 0x00055840
		internal SystemHandle CreateUnmanagedSystem(SystemTypeIndex t, bool callOnCreate)
		{
			long systemTypeHash = TypeManager.GetSystemTypeHash(t);
			return this.CreateUnmanagedSystem(t, systemTypeHash, callOnCreate);
		}

		// Token: 0x06001710 RID: 5904 RVA: 0x0005765D File Offset: 0x0005585D
		internal bool IsSystemValid(SystemHandle id)
		{
			return this.ResolveSystemState(id) != null;
		}

		// Token: 0x06001711 RID: 5905 RVA: 0x00057670 File Offset: 0x00055870
		internal unsafe SystemState* ResolveSystemState(SystemHandle id)
		{
			if (id.m_WorldSeqNo != (uint)this.SequenceNumber)
			{
				return null;
			}
			ushort handle = id.m_Handle;
			ushort version = id.m_Version;
			if (handle >= 4096)
			{
				return null;
			}
			return this._stateMemory.Resolve(handle, version);
		}

		// Token: 0x06001712 RID: 5906 RVA: 0x000576B8 File Offset: 0x000558B8
		internal unsafe SystemState* ResolveSystemStateChecked(SystemHandle id)
		{
			if (id.m_WorldSeqNo != (uint)this.SequenceNumber)
			{
				throw new InvalidOperationException("This system handle belongs to a different world.");
			}
			ushort handle = id.m_Handle;
			ushort version = id.m_Version;
			if (handle >= 4096)
			{
				throw new InvalidOperationException("System handle is invalid (ID out of bounds).");
			}
			return this._stateMemory.Resolve(handle, version);
		}

		// Token: 0x06001713 RID: 5907 RVA: 0x00057710 File Offset: 0x00055910
		internal unsafe NativeArray<SystemHandle> GetAllUnmanagedSystems(AllocatorManager.AllocatorHandle a)
		{
			int num = 0;
			for (int i = 0; i < 64; i++)
			{
				WorldUnmanagedImpl.StateAllocLevel1* ptr = this._stateMemory.m_Level1 + i;
				num += math.countbits(~ptr->FreeBits);
			}
			int num2 = 0;
			NativeArray<SystemHandle> nativeArray = CollectionHelper.CreateNativeArray<SystemHandle>(num, a, NativeArrayOptions.ClearMemory);
			for (int j = 0; j < 64; j++)
			{
				int num4;
				for (ulong num3 = ~this._stateMemory.m_Level1[j].FreeBits; num3 != 0UL; num3 &= ~(1UL << num4))
				{
					num4 = math.tzcnt(num3);
					SystemState* state = this._stateMemory.GetState((ushort)(j * 64 + num4));
					if (state->m_SystemPtr != null)
					{
						nativeArray[num2++] = state->m_Handle;
					}
				}
			}
			nativeArray.m_Length = num2;
			return nativeArray;
		}

		// Token: 0x06001714 RID: 5908 RVA: 0x000577E8 File Offset: 0x000559E8
		internal unsafe NativeArray<SystemHandle> GetAllSystems(AllocatorManager.AllocatorHandle a)
		{
			int num = 0;
			for (int i = 0; i < 64; i++)
			{
				WorldUnmanagedImpl.StateAllocLevel1* ptr = this._stateMemory.m_Level1 + i;
				num += math.countbits(~ptr->FreeBits);
			}
			int num2 = 0;
			NativeArray<SystemHandle> nativeArray = CollectionHelper.CreateNativeArray<SystemHandle>(num, a, NativeArrayOptions.ClearMemory);
			for (int j = 0; j < 64; j++)
			{
				int num4;
				for (ulong num3 = ~this._stateMemory.m_Level1[j].FreeBits; num3 != 0UL; num3 &= ~(1UL << num4))
				{
					num4 = math.tzcnt(num3);
					SystemState* state = this._stateMemory.GetState((ushort)(j * 64 + num4));
					nativeArray[num2++] = state->m_Handle;
				}
			}
			return nativeArray;
		}

		// Token: 0x06001715 RID: 5909 RVA: 0x000578AC File Offset: 0x00055AAC
		[BurstCompile]
		[MonoPInvokeCallback(typeof(WorldUnmanagedImpl.UnmanagedUpdateSignature))]
		internal unsafe static void UnmanagedUpdate(void* pSystemState)
		{
			WorldUnmanagedImpl.UnmanagedUpdate_00001411$BurstDirectCall.Invoke(pSystemState);
		}

		// Token: 0x06001716 RID: 5910 RVA: 0x000578B4 File Offset: 0x00055AB4
		internal unsafe void UpdateSystem(SystemHandle sh)
		{
			if (sh == SystemHandle.Null)
			{
				throw new NullReferenceException("The system couldn't be updated. The SystemHandle is default/null, so was never assigned.");
			}
			SystemState* ptr = this.ResolveSystemStateChecked(sh);
			if (ptr == null)
			{
				throw new NullReferenceException("The system couldn't be resolved. The System has been destroyed.");
			}
			if (ptr->m_ManagedSystem.IsAllocated)
			{
				ptr->ManagedSystem.Update();
				return;
			}
			WorldUnmanagedImpl.PreviousSystemGlobalState previousSystemGlobalState = new WorldUnmanagedImpl.PreviousSystemGlobalState(ref this, ptr);
			try
			{
				WorldUnmanagedImpl.UnmanagedUpdate((void*)ptr);
			}
			catch
			{
				ptr->AfterOnUpdate();
				previousSystemGlobalState.Restore(ref this, ptr);
				throw;
			}
			previousSystemGlobalState.Restore(ref this, ptr);
		}

		// Token: 0x06001717 RID: 5911 RVA: 0x00057948 File Offset: 0x00055B48
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void UnmanagedUpdate$BurstManaged(void* pSystemState)
		{
			if (((SystemState*)pSystemState)->Enabled && ((SystemState*)pSystemState)->ShouldRunSystem())
			{
				((SystemState*)pSystemState)->BeforeOnUpdate();
				if (!((SystemState*)pSystemState)->PreviouslyEnabled)
				{
					((SystemState*)pSystemState)->PreviouslyEnabled = true;
					SystemBaseRegistry.CallOnStartRunning((SystemState*)pSystemState);
				}
				SystemBaseRegistry.CallOnUpdate((SystemState*)pSystemState);
				((SystemState*)pSystemState)->AfterOnUpdate();
				return;
			}
			if (((SystemState*)pSystemState)->PreviouslyEnabled)
			{
				((SystemState*)pSystemState)->PreviouslyEnabled = false;
				((SystemState*)pSystemState)->BeforeOnUpdate();
				SystemBaseRegistry.CallOnStopRunning((SystemState*)pSystemState);
				((SystemState*)pSystemState)->AfterOnUpdate();
			}
		}

		// Token: 0x040008B0 RID: 2224
		internal AllocatorHelper<AutoFreeAllocator> m_WorldAllocatorHelper;

		// Token: 0x040008B1 RID: 2225
		internal NativeParallelHashMap<int, IntPtr> m_SystemStatePtrMap;

		// Token: 0x040008B2 RID: 2226
		private WorldUnmanagedImpl.StateAllocator _stateMemory;

		// Token: 0x040008B3 RID: 2227
		private UnsafeParallelMultiHashMap<long, ushort> _unmanagedSlotByTypeHash;

		// Token: 0x040008B4 RID: 2228
		internal UnsafeList<PerWorldSystemInfo> sysHandlesInCreationOrder;

		// Token: 0x040008B5 RID: 2229
		internal readonly ulong SequenceNumber;

		// Token: 0x040008B6 RID: 2230
		public WorldFlags Flags;

		// Token: 0x040008B7 RID: 2231
		public TimeData CurrentTime;

		// Token: 0x040008B8 RID: 2232
		public FixedString128Bytes Name;

		// Token: 0x040008B9 RID: 2233
		public SystemHandle ExecutingSystem;

		// Token: 0x040008BA RID: 2234
		public unsafe DoubleRewindableAllocators* DoubleUpdateAllocators;

		// Token: 0x040008BB RID: 2235
		internal unsafe DoubleRewindableAllocators* GroupUpdateAllocators;

		// Token: 0x040008BC RID: 2236
		internal EntityManager m_EntityManager;

		// Token: 0x040008BD RID: 2237
		public float MaximumDeltaTime;

		// Token: 0x040008BE RID: 2238
		private static WorldUnmanagedImpl.UnmanagedUpdateSignature s_UnmanagedUpdateFn;

		// Token: 0x040008BF RID: 2239
		private const int InitialUpdateAllocatorBlockSizeInBytes = 131072;

		// Token: 0x0200035C RID: 860
		// (Invoke) Token: 0x06001719 RID: 5913
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		internal unsafe delegate void UnmanagedUpdateSignature(void* pSystemState);

		// Token: 0x0200035D RID: 861
		internal struct PreviousSystemGlobalState
		{
			// Token: 0x0600171C RID: 5916 RVA: 0x000579B0 File Offset: 0x00055BB0
			public unsafe PreviousSystemGlobalState(ref WorldUnmanagedImpl world, SystemState* state)
			{
				this.OldExecutingSystem = world.ExecutingSystem;
				world.ExecutingSystem = state->m_Handle;
			}

			// Token: 0x0600171D RID: 5917 RVA: 0x000579CA File Offset: 0x00055BCA
			public unsafe void Restore(ref WorldUnmanagedImpl world, SystemState* state)
			{
				world.ExecutingSystem = this.OldExecutingSystem;
			}

			// Token: 0x040008C1 RID: 2241
			public SystemHandle OldExecutingSystem;
		}

		// Token: 0x0200035E RID: 862
		internal struct StateAllocator
		{
			// Token: 0x0600171E RID: 5918 RVA: 0x000579D8 File Offset: 0x00055BD8
			public unsafe void Init()
			{
				this.m_FreeBits = ulong.MaxValue;
				int num = sizeof(WorldUnmanagedImpl.StateAllocLevel1) * 64;
				WorldUnmanagedImpl.StateAllocLevel1* ptr = (this.m_Level1 = (WorldUnmanagedImpl.StateAllocLevel1*)Memory.Unmanaged.Allocate((long)num, 16, Allocator.Persistent));
				UnsafeUtility.MemClear((void*)ptr, (long)num);
				for (int i = 0; i < 64; i++)
				{
					ptr[i].FreeBits = ulong.MaxValue;
				}
			}

			// Token: 0x0600171F RID: 5919 RVA: 0x00057A38 File Offset: 0x00055C38
			public unsafe void Dispose()
			{
				WorldUnmanagedImpl.StateAllocLevel1* level = this.m_Level1;
				for (int i = 0; i < 64; i++)
				{
					if (level[i].States != null)
					{
						Memory.Unmanaged.Free((void*)level[i].States, Allocator.Persistent);
					}
				}
				Memory.Unmanaged.Free<WorldUnmanagedImpl.StateAllocLevel1>(level, Allocator.Persistent);
				this.m_Level1 = null;
				this = default(WorldUnmanagedImpl.StateAllocator);
			}

			// Token: 0x06001720 RID: 5920 RVA: 0x00057AA4 File Offset: 0x00055CA4
			public unsafe SystemState* Resolve(ushort handle, ushort version)
			{
				int num = handle >> 6;
				int num2 = (int)(handle & 63);
				ref WorldUnmanagedImpl.StateAllocLevel1 ptr = ref this.m_Level1[num];
				if (*((ref ptr.Version.FixedElementField) + (IntPtr)num2 * 2) != version)
				{
					return null;
				}
				return ptr.States + num2;
			}

			// Token: 0x06001721 RID: 5921 RVA: 0x00057AF4 File Offset: 0x00055CF4
			public unsafe SystemState* ResolveNoCheck(ushort handle)
			{
				int num = handle >> 6;
				int num2 = (int)(handle & 63);
				return this.m_Level1[num].States + num2;
			}

			// Token: 0x06001722 RID: 5922 RVA: 0x00057B2C File Offset: 0x00055D2C
			public unsafe long GetTypeHashNoCheck(ushort handle)
			{
				int num = handle >> 6;
				int num2 = (int)(handle & 63);
				return *((ref this.m_Level1[num].TypeHash.FixedElementField) + (IntPtr)num2 * 8);
			}

			// Token: 0x06001723 RID: 5923 RVA: 0x00057B64 File Offset: 0x00055D64
			public unsafe SystemState* Alloc(out ushort outHandle, out ushort outVersion, long typeHash)
			{
				int num = math.tzcnt(this.m_FreeBits);
				ref WorldUnmanagedImpl.StateAllocLevel1 ptr = ref this.m_Level1[num];
				int num2 = math.tzcnt(ptr.FreeBits);
				if (ptr.States == null)
				{
					ptr.States = (SystemState*)Memory.Unmanaged.Allocate((long)(64 * sizeof(SystemState)), 16, Allocator.Persistent);
				}
				ptr.FreeBits &= ~(1UL << num2);
				ulong num3 = ((ptr.FreeBits == 0UL) ? 1UL : 0UL);
				this.m_FreeBits &= ~(num3 << num);
				SystemState* ptr2 = ptr.States + num2;
				UnsafeUtility.MemClear((void*)ptr2, (long)sizeof(SystemState));
				outHandle = (ushort)((num << 6) + num2);
				WorldUnmanagedImpl.StateAllocator.IncVersion((ref ptr.Version.FixedElementField) + (IntPtr)num2 * 2);
				outVersion = *((ref ptr.Version.FixedElementField) + (IntPtr)num2 * 2);
				*((ref ptr.TypeHash.FixedElementField) + (IntPtr)num2 * 8) = typeHash;
				return ptr2;
			}

			// Token: 0x06001724 RID: 5924 RVA: 0x00002287 File Offset: 0x00000487
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			private void CheckFull()
			{
			}

			// Token: 0x06001725 RID: 5925 RVA: 0x00002287 File Offset: 0x00000487
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			private static void CheckSubIndex(int subIndex)
			{
			}

			// Token: 0x06001726 RID: 5926 RVA: 0x00057C58 File Offset: 0x00055E58
			public unsafe void Free(ushort handle)
			{
				int num = handle >> 6;
				int num2 = (int)(handle & 63);
				this.m_FreeBits |= 1UL << num;
				WorldUnmanagedImpl.StateAllocLevel1* ptr = this.m_Level1 + num;
				ptr->FreeBits |= 1UL << num2;
				WorldUnmanagedImpl.StateAllocator.IncVersion((ref ptr->Version.FixedElementField) + (IntPtr)num2 * 2);
				*((ref ptr->TypeHash.FixedElementField) + (IntPtr)num2 * 8) = 0L;
			}

			// Token: 0x06001727 RID: 5927 RVA: 0x00002287 File Offset: 0x00000487
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			private static void CheckIsAllocated(ref WorldUnmanagedImpl.StateAllocLevel1 leaf, int subIndex)
			{
			}

			// Token: 0x06001728 RID: 5928 RVA: 0x00002287 File Offset: 0x00000487
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			private static void CheckIndex(int index)
			{
			}

			// Token: 0x06001729 RID: 5929 RVA: 0x00057CCC File Offset: 0x00055ECC
			public static void IncVersion(ref ushort v)
			{
				uint num = (uint)v;
				num += 1U;
				num = (num >> 16) | num;
				v = (ushort)num;
			}

			// Token: 0x0600172A RID: 5930 RVA: 0x00057CEC File Offset: 0x00055EEC
			public unsafe SystemState* GetState(ushort handle)
			{
				ushort num;
				return this.GetBlock(handle, out num)->States + num;
			}

			// Token: 0x0600172B RID: 5931 RVA: 0x00057D14 File Offset: 0x00055F14
			public unsafe WorldUnmanagedImpl.StateAllocLevel1* GetBlock(ushort handle, out ushort subIndex)
			{
				ushort num = (ushort)(handle >> 6);
				subIndex = handle & 63;
				return this.m_Level1 + num;
			}

			// Token: 0x040008C2 RID: 2242
			internal ulong m_FreeBits;

			// Token: 0x040008C3 RID: 2243
			internal unsafe WorldUnmanagedImpl.StateAllocLevel1* m_Level1;
		}

		// Token: 0x0200035F RID: 863
		internal struct StateAllocLevel1
		{
			// Token: 0x040008C4 RID: 2244
			public ulong FreeBits;

			// Token: 0x040008C5 RID: 2245
			public unsafe SystemState* States;

			// Token: 0x040008C6 RID: 2246
			[FixedBuffer(typeof(ushort), 64)]
			public WorldUnmanagedImpl.StateAllocLevel1.<Version>e__FixedBuffer Version;

			// Token: 0x040008C7 RID: 2247
			[FixedBuffer(typeof(long), 64)]
			public WorldUnmanagedImpl.StateAllocLevel1.<TypeHash>e__FixedBuffer TypeHash;

			// Token: 0x02000360 RID: 864
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 512)]
			public struct <TypeHash>e__FixedBuffer
			{
				// Token: 0x040008C8 RID: 2248
				public long FixedElementField;
			}

			// Token: 0x02000361 RID: 865
			[CompilerGenerated]
			[UnsafeValueType]
			[StructLayout(LayoutKind.Sequential, Size = 128)]
			public struct <Version>e__FixedBuffer
			{
				// Token: 0x040008C9 RID: 2249
				public ushort FixedElementField;
			}
		}

		// Token: 0x02000362 RID: 866
		// (Invoke) Token: 0x0600172D RID: 5933
		internal unsafe delegate void UnmanagedUpdate_00001411$PostfixBurstDelegate(void* pSystemState);

		// Token: 0x02000363 RID: 867
		internal static class UnmanagedUpdate_00001411$BurstDirectCall
		{
			// Token: 0x06001730 RID: 5936 RVA: 0x00057D3D File Offset: 0x00055F3D
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (WorldUnmanagedImpl.UnmanagedUpdate_00001411$BurstDirectCall.Pointer == 0)
				{
					WorldUnmanagedImpl.UnmanagedUpdate_00001411$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(WorldUnmanagedImpl.UnmanagedUpdate_00001411$BurstDirectCall.DeferredCompilation, methodof(WorldUnmanagedImpl.UnmanagedUpdate$BurstManaged(void*)).MethodHandle, typeof(WorldUnmanagedImpl.UnmanagedUpdate_00001411$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = WorldUnmanagedImpl.UnmanagedUpdate_00001411$BurstDirectCall.Pointer;
			}

			// Token: 0x06001731 RID: 5937 RVA: 0x00057D6C File Offset: 0x00055F6C
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				WorldUnmanagedImpl.UnmanagedUpdate_00001411$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06001732 RID: 5938 RVA: 0x00057D84 File Offset: 0x00055F84
			public unsafe static void Constructor()
			{
				WorldUnmanagedImpl.UnmanagedUpdate_00001411$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(WorldUnmanagedImpl.UnmanagedUpdate(void*)).MethodHandle);
			}

			// Token: 0x06001733 RID: 5939 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x06001734 RID: 5940 RVA: 0x00057D95 File Offset: 0x00055F95
			// Note: this type is marked as 'beforefieldinit'.
			static UnmanagedUpdate_00001411$BurstDirectCall()
			{
				WorldUnmanagedImpl.UnmanagedUpdate_00001411$BurstDirectCall.Constructor();
			}

			// Token: 0x06001735 RID: 5941 RVA: 0x00057D9C File Offset: 0x00055F9C
			public unsafe static void Invoke(void* pSystemState)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = WorldUnmanagedImpl.UnmanagedUpdate_00001411$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(System.Void*), pSystemState, functionPointer);
						return;
					}
				}
				WorldUnmanagedImpl.UnmanagedUpdate$BurstManaged(pSystemState);
			}

			// Token: 0x040008CA RID: 2250
			private static IntPtr Pointer;

			// Token: 0x040008CB RID: 2251
			private static IntPtr DeferredCompilation;
		}
	}
}
