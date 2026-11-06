using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;

namespace Unity.Entities
{
	// Token: 0x020002D3 RID: 723
	[GenerateTestsForBurstCompatibility]
	public static class SystemBaseRegistry
	{
		// Token: 0x06001342 RID: 4930 RVA: 0x0004B71C File Offset: 0x0004991C
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void AssertTypeRegistryInitialized(in UnmanagedSystemTypeRegistryData data)
		{
			UnmanagedSystemTypeRegistryData unmanagedSystemTypeRegistryData = data;
			if (!unmanagedSystemTypeRegistryData.Constructed)
			{
				throw new InvalidOperationException("type registry is not initialized");
			}
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x0004B744 File Offset: 0x00049944
		[GenerateTestsForBurstCompatibility]
		internal static int GetSystemTypeMetaIndex(long typeHash)
		{
			int num;
			if (!SystemBaseRegistry.s_Data.Data.FindSystemMetaIndex(typeHash, out num))
			{
				num = -1;
			}
			return num;
		}

		// Token: 0x06001344 RID: 4932 RVA: 0x0004B768 File Offset: 0x00049968
		[ExcludeFromBurstCompatTesting("Takes managed Type")]
		public static void AddUnmanagedSystemType(Type type, long typeHash, SystemBaseRegistry.ForwardingFunc onCreate, SystemBaseRegistry.ForwardingFunc onUpdate, SystemBaseRegistry.ForwardingFunc onDestroy, SystemBaseRegistry.ForwardingFunc onStartRunning, SystemBaseRegistry.ForwardingFunc onStopRunning, SystemBaseRegistry.ForwardingFunc onCreateForCompiler, string debugName, int burstCompileBits)
		{
			if (SystemBaseRegistry.Managed.s_PendingRegistrations == null)
			{
				SystemBaseRegistry.Managed.s_PendingRegistrations = new List<SystemBaseRegistry.RegistrationEntry>();
				SystemBaseRegistry.Managed.s_StructTypes = new List<Type>();
				SystemBaseRegistry.s_Data.Data.Construct();
				if (!SystemBaseRegistry.Managed.s_DisposeRegistered)
				{
					SystemBaseRegistry.Managed.s_DisposeRegistered = true;
					AppDomain.CurrentDomain.ProcessExit += delegate(object _, EventArgs __)
					{
						SystemBaseRegistry.s_Data.Data.Dispose();
						SystemBaseRegistry.Managed.s_PendingRegistrations = null;
					};
				}
			}
			SystemBaseRegistry.ForwardingFunc[] array = new SystemBaseRegistry.ForwardingFunc[] { onCreate, onUpdate, onDestroy, onStartRunning, onStopRunning, onCreateForCompiler };
			SystemBaseRegistry.Managed.s_PendingRegistrations.Add(new SystemBaseRegistry.RegistrationEntry
			{
				m_Type = type,
				m_TypeHash = typeHash,
				m_Functions = array,
				m_DebugName = debugName,
				m_BurstCompileBits = burstCompileBits
			});
		}

		// Token: 0x06001345 RID: 4933 RVA: 0x0004B838 File Offset: 0x00049A38
		[ExcludeFromBurstCompatTesting("Uses managed delegate")]
		private static void SelectManagedFn(out ulong result, ref ulong burstFn, SystemBaseRegistry.ForwardingFunc managedFn)
		{
			if (burstFn != 0UL)
			{
				FunctionPointer<SystemBaseRegistry.ForwardingFunc> functionPointer = new FunctionPointer<SystemBaseRegistry.ForwardingFunc>((IntPtr)((long)burstFn));
				try
				{
					result = (ulong)(long)GCHandle.ToIntPtr(GCHandle.Alloc(functionPointer.Invoke));
					return;
				}
				catch (InvalidCastException)
				{
					Debug.LogWarning(string.Format("SelectManagedFn failed to cast {0}. BurstFn: {1}.Forwarding Func: {2}. Falling back to managed delegate. Please report this as a bug using Help > Report a Bug...", functionPointer.GetType(), burstFn, managedFn.GetType()));
					burstFn = 0UL;
				}
			}
			result = (ulong)(long)GCHandle.ToIntPtr(GCHandle.Alloc(managedFn));
		}

		// Token: 0x06001346 RID: 4934 RVA: 0x0004B8C4 File Offset: 0x00049AC4
		[ExcludeFromBurstCompatTesting("Uses managed delegate")]
		private static void SelectBurstFn(out ulong result, out ulong defeatGc, ulong burstFn, SystemBaseRegistry.ForwardingFunc managedFn)
		{
			if (burstFn != 0UL)
			{
				result = burstFn;
				defeatGc = 0UL;
				return;
			}
			SystemBaseRegistry.ForwardingFunc forwardingFunc = delegate(IntPtr system, IntPtr state)
			{
				try
				{
					managedFn(system, state);
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
				}
			};
			defeatGc = (ulong)(long)GCHandle.ToIntPtr(GCHandle.Alloc(forwardingFunc));
			result = (ulong)(long)Marshal.GetFunctionPointerForDelegate<SystemBaseRegistry.ForwardingFunc>(forwardingFunc);
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x0004B918 File Offset: 0x00049B18
		[ExcludeFromBurstCompatTesting("Uses managed delegates")]
		public static void InitializePendingTypes()
		{
			if (SystemBaseRegistry.Managed.s_PendingRegistrations == null)
			{
				return;
			}
			foreach (SystemBaseRegistry.RegistrationEntry registrationEntry in SystemBaseRegistry.Managed.s_PendingRegistrations)
			{
				int burstCompileBits = registrationEntry.m_BurstCompileBits;
				UnmanagedComponentSystemDelegates unmanagedComponentSystemDelegates = default(UnmanagedComponentSystemDelegates);
				ushort num = 1;
				unmanagedComponentSystemDelegates.BurstFunctionBits = 0;
				int i = 0;
				while (i < registrationEntry.m_Functions.Length)
				{
					SystemBaseRegistry.ForwardingFunc forwardingFunc = registrationEntry.m_Functions[i];
					if (forwardingFunc != null)
					{
						bool flag = (burstCompileBits & (int)num) != 0;
						ulong num2 = (ulong)(flag ? ((long)BurstCompiler.CompileFunctionPointer<SystemBaseRegistry.ForwardingFunc>(forwardingFunc).Value) : 0L);
						SystemBaseRegistry.SelectManagedFn((ref unmanagedComponentSystemDelegates.ManagedFunctions.FixedElementField) + (IntPtr)i * 8, ref num2, forwardingFunc);
						SystemBaseRegistry.SelectBurstFn((ref unmanagedComponentSystemDelegates.BurstFunctions.FixedElementField) + (IntPtr)i * 8, (ref unmanagedComponentSystemDelegates.GCDefeat1.FixedElementField) + (IntPtr)i * 8, num2, forwardingFunc);
						unmanagedComponentSystemDelegates.PresentFunctionBits |= num;
						if (flag)
						{
							unmanagedComponentSystemDelegates.BurstFunctionBits |= num;
						}
					}
					i++;
					num = (ushort)(num << 1);
				}
				SystemBaseRegistry.Managed.s_StructTypes.Add(registrationEntry.m_Type);
				SystemBaseRegistry.s_Data.Data.AddSystemType(registrationEntry.m_TypeHash, unmanagedComponentSystemDelegates);
			}
			SystemBaseRegistry.Managed.s_PendingRegistrations = null;
		}

		// Token: 0x06001348 RID: 4936 RVA: 0x0004BA88 File Offset: 0x00049C88
		[BurstDiscard]
		internal static void CheckBurst(ref bool status)
		{
			status = false;
		}

		// Token: 0x06001349 RID: 4937 RVA: 0x0004BA90 File Offset: 0x00049C90
		[IgnoreWarning(1371)]
		private unsafe static void CallForwardingFunction(SystemState* systemState, UnmanagedSystemFunctionType functionType)
		{
			int unmanagedMetaIndex = systemState->UnmanagedMetaIndex;
			void* systemPtr = systemState->m_SystemPtr;
			UnmanagedComponentSystemDelegates unmanagedComponentSystemDelegates = *SystemBaseRegistry.s_Data.Data.GetSystemDelegates(unmanagedMetaIndex);
			bool flag = true;
			SystemBaseRegistry.CheckBurst(ref flag);
			if (((int)unmanagedComponentSystemDelegates.PresentFunctionBits & (1 << (int)functionType)) != 0)
			{
				if (flag)
				{
					new FunctionPointer<SystemBaseRegistry.ForwardingFunc>((IntPtr)((long)(*((ref unmanagedComponentSystemDelegates.BurstFunctions.FixedElementField) + (IntPtr)functionType * 8)))).Invoke((IntPtr)systemPtr, (IntPtr)((void*)systemState));
					return;
				}
				SystemBaseRegistry.ForwardToManaged((IntPtr)((long)(*((ref unmanagedComponentSystemDelegates.ManagedFunctions.FixedElementField) + (IntPtr)functionType * 8))), systemState, systemPtr);
			}
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x0004BB38 File Offset: 0x00049D38
		internal unsafe static readonly ref UnmanagedComponentSystemDelegates GetDelegates(SystemState* systemState)
		{
			int unmanagedMetaIndex = systemState->UnmanagedMetaIndex;
			return SystemBaseRegistry.s_Data.Data.GetSystemDelegates(unmanagedMetaIndex);
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x0004BB5C File Offset: 0x00049D5C
		internal unsafe static bool IsOnUpdateUsingBurst(in SystemState* state)
		{
			bool flag = true;
			SystemBaseRegistry.CheckBurst(ref flag);
			readonly ref UnmanagedComponentSystemDelegates delegates = ref SystemBaseRegistry.GetDelegates(state);
			return flag & ((delegates.BurstFunctionBits & 2) > 0);
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x0004BB88 File Offset: 0x00049D88
		[BurstDiscard]
		private unsafe static void ForwardToManaged(IntPtr delegateIntPtr, SystemState* systemState, void* systemPointer)
		{
			((SystemBaseRegistry.ForwardingFunc)GCHandle.FromIntPtr(delegateIntPtr).Target)((IntPtr)systemPointer, (IntPtr)((void*)systemState));
		}

		// Token: 0x0600134D RID: 4941 RVA: 0x0004BBB9 File Offset: 0x00049DB9
		[GenerateTestsForBurstCompatibility]
		internal unsafe static void CallOnCreate(SystemState* systemState)
		{
			SystemBaseRegistry.CallForwardingFunction(systemState, UnmanagedSystemFunctionType.OnCreate);
		}

		// Token: 0x0600134E RID: 4942 RVA: 0x0004BBC2 File Offset: 0x00049DC2
		[GenerateTestsForBurstCompatibility]
		internal unsafe static void CallOnUpdate(SystemState* systemState)
		{
			SystemBaseRegistry.CallForwardingFunction(systemState, UnmanagedSystemFunctionType.OnUpdate);
		}

		// Token: 0x0600134F RID: 4943 RVA: 0x0004BBCB File Offset: 0x00049DCB
		[GenerateTestsForBurstCompatibility]
		internal unsafe static void CallOnDestroy(SystemState* systemState)
		{
			SystemBaseRegistry.CallForwardingFunction(systemState, UnmanagedSystemFunctionType.OnDestroy);
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x0004BBD4 File Offset: 0x00049DD4
		[GenerateTestsForBurstCompatibility]
		internal unsafe static void CallOnStartRunning(SystemState* systemState)
		{
			SystemBaseRegistry.CallForwardingFunction(systemState, UnmanagedSystemFunctionType.OnStartRunning);
		}

		// Token: 0x06001351 RID: 4945 RVA: 0x0004BBDD File Offset: 0x00049DDD
		[GenerateTestsForBurstCompatibility]
		internal unsafe static void CallOnStopRunning(SystemState* systemState)
		{
			SystemBaseRegistry.CallForwardingFunction(systemState, UnmanagedSystemFunctionType.OnStopRunning);
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x0004BBE6 File Offset: 0x00049DE6
		[GenerateTestsForBurstCompatibility]
		internal unsafe static void CallOnCreateForCompiler(SystemState* systemState)
		{
			SystemBaseRegistry.CallForwardingFunction(systemState, UnmanagedSystemFunctionType.OnCreateForCompiler);
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x0004BBEF File Offset: 0x00049DEF
		[ExcludeFromBurstCompatTesting("returns managed Type")]
		internal static Type GetStructType(int metaIndex)
		{
			return SystemBaseRegistry.Managed.s_StructTypes[metaIndex];
		}

		// Token: 0x040006F9 RID: 1785
		internal static readonly SharedStatic<UnmanagedSystemTypeRegistryData> s_Data = SharedStatic<UnmanagedSystemTypeRegistryData>.GetOrCreateUnsafe(0U, 5421239482035215235L, 0L);

		// Token: 0x020002D4 RID: 724
		private class Managed
		{
			// Token: 0x040006FA RID: 1786
			public static List<Type> s_StructTypes;

			// Token: 0x040006FB RID: 1787
			public static List<SystemBaseRegistry.RegistrationEntry> s_PendingRegistrations;

			// Token: 0x040006FC RID: 1788
			public static bool s_DisposeRegistered;
		}

		// Token: 0x020002D5 RID: 725
		private struct Dummy
		{
		}

		// Token: 0x020002D6 RID: 726
		// (Invoke) Token: 0x06001357 RID: 4951
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void ForwardingFunc(IntPtr systemPtr, IntPtr state);

		// Token: 0x020002D7 RID: 727
		private struct RegistrationEntry
		{
			// Token: 0x040006FD RID: 1789
			public Type m_Type;

			// Token: 0x040006FE RID: 1790
			public long m_TypeHash;

			// Token: 0x040006FF RID: 1791
			public SystemBaseRegistry.ForwardingFunc[] m_Functions;

			// Token: 0x04000700 RID: 1792
			public string m_DebugName;

			// Token: 0x04000701 RID: 1793
			public int m_BurstCompileBits;
		}
	}
}
