using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Steamworks
{
	// Token: 0x02000172 RID: 370
	public static class CallbackDispatcher
	{
		// Token: 0x06000893 RID: 2195 RVA: 0x0000C55C File Offset: 0x0000A75C
		private static void DefaultExceptionHandler(Exception e)
		{
			Debug.LogException(e);
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x0000C564 File Offset: 0x0000A764
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void InitOnPlayMode()
		{
			CallbackDispatcher.m_initCount = 0;
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000895 RID: 2197 RVA: 0x0000C56C File Offset: 0x0000A76C
		public static bool IsInitialized
		{
			get
			{
				return CallbackDispatcher.m_initCount > 0;
			}
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x0000C578 File Offset: 0x0000A778
		internal static void Initialize()
		{
			object sync = CallbackDispatcher.m_sync;
			lock (sync)
			{
				if (CallbackDispatcher.m_initCount == 0)
				{
					NativeMethods.SteamAPI_ManualDispatch_Init();
					CallbackDispatcher.m_pCallbackMsg = Marshal.AllocHGlobal(Marshal.SizeOf<CallbackMsg_t>());
				}
				CallbackDispatcher.m_initCount++;
			}
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x0000C5D8 File Offset: 0x0000A7D8
		internal static void Shutdown()
		{
			object sync = CallbackDispatcher.m_sync;
			lock (sync)
			{
				CallbackDispatcher.m_initCount--;
				if (CallbackDispatcher.m_initCount == 0)
				{
					CallbackDispatcher.UnregisterAll();
					Marshal.FreeHGlobal(CallbackDispatcher.m_pCallbackMsg);
					CallbackDispatcher.m_pCallbackMsg = IntPtr.Zero;
				}
			}
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x0000C640 File Offset: 0x0000A840
		internal static void Register(Callback cb)
		{
			int callbackIdentity = CallbackIdentities.GetCallbackIdentity(cb.GetCallbackType());
			Dictionary<int, List<Callback>> dictionary = (cb.IsGameServer ? CallbackDispatcher.m_registeredGameServerCallbacks : CallbackDispatcher.m_registeredCallbacks);
			object sync = CallbackDispatcher.m_sync;
			lock (sync)
			{
				List<Callback> list;
				if (!dictionary.TryGetValue(callbackIdentity, out list))
				{
					list = new List<Callback>();
					dictionary.Add(callbackIdentity, list);
				}
				list.Add(cb);
			}
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x0000C6C0 File Offset: 0x0000A8C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void Register(SteamAPICall_t asyncCall, CallResult cr)
		{
			object sync = CallbackDispatcher.m_sync;
			lock (sync)
			{
				List<CallResult> list;
				if (!CallbackDispatcher.m_registeredCallResults.TryGetValue((ulong)asyncCall, out list))
				{
					list = new List<CallResult>();
					CallbackDispatcher.m_registeredCallResults.Add((ulong)asyncCall, list);
				}
				list.Add(cr);
			}
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x0000C72C File Offset: 0x0000A92C
		internal static void Unregister(Callback cb)
		{
			int callbackIdentity = CallbackIdentities.GetCallbackIdentity(cb.GetCallbackType());
			Dictionary<int, List<Callback>> dictionary = (cb.IsGameServer ? CallbackDispatcher.m_registeredGameServerCallbacks : CallbackDispatcher.m_registeredCallbacks);
			object sync = CallbackDispatcher.m_sync;
			lock (sync)
			{
				List<Callback> list;
				if (dictionary.TryGetValue(callbackIdentity, out list))
				{
					list.Remove(cb);
					if (list.Count == 0)
					{
						dictionary.Remove(callbackIdentity);
					}
				}
			}
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x0000C7AC File Offset: 0x0000A9AC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void Unregister(SteamAPICall_t asyncCall, CallResult cr)
		{
			object sync = CallbackDispatcher.m_sync;
			lock (sync)
			{
				List<CallResult> list;
				if (CallbackDispatcher.m_registeredCallResults.TryGetValue((ulong)asyncCall, out list))
				{
					list.Remove(cr);
					if (list.Count == 0)
					{
						CallbackDispatcher.m_registeredCallResults.Remove((ulong)asyncCall);
					}
				}
			}
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x0000C81C File Offset: 0x0000AA1C
		private static void UnregisterAll()
		{
			List<Callback> list = new List<Callback>();
			List<CallResult> list2 = new List<CallResult>();
			object sync = CallbackDispatcher.m_sync;
			lock (sync)
			{
				foreach (KeyValuePair<int, List<Callback>> keyValuePair in CallbackDispatcher.m_registeredCallbacks)
				{
					list.AddRange(keyValuePair.Value);
				}
				CallbackDispatcher.m_registeredCallbacks.Clear();
				foreach (KeyValuePair<int, List<Callback>> keyValuePair2 in CallbackDispatcher.m_registeredGameServerCallbacks)
				{
					list.AddRange(keyValuePair2.Value);
				}
				CallbackDispatcher.m_registeredGameServerCallbacks.Clear();
				foreach (KeyValuePair<ulong, List<CallResult>> keyValuePair3 in CallbackDispatcher.m_registeredCallResults)
				{
					list2.AddRange(keyValuePair3.Value);
				}
				CallbackDispatcher.m_registeredCallResults.Clear();
				foreach (Callback callback in list)
				{
					callback.SetUnregistered();
				}
				foreach (CallResult callResult in list2)
				{
					callResult.SetUnregistered();
				}
			}
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x0000CA18 File Offset: 0x0000AC18
		internal static void RunFrame(bool isGameServer)
		{
			if (!CallbackDispatcher.IsInitialized)
			{
				throw new InvalidOperationException("Callback dispatcher is not initialized.");
			}
			HSteamPipe hsteamPipe = (HSteamPipe)(isGameServer ? NativeMethods.SteamGameServer_GetHSteamPipe() : NativeMethods.SteamAPI_GetHSteamPipe());
			NativeMethods.SteamAPI_ManualDispatch_RunFrame(hsteamPipe);
			Dictionary<int, List<Callback>> dictionary = (isGameServer ? CallbackDispatcher.m_registeredGameServerCallbacks : CallbackDispatcher.m_registeredCallbacks);
			while (NativeMethods.SteamAPI_ManualDispatch_GetNextCallback(hsteamPipe, CallbackDispatcher.m_pCallbackMsg))
			{
				CallbackMsg_t callbackMsg_t = Marshal.PtrToStructure<CallbackMsg_t>(CallbackDispatcher.m_pCallbackMsg);
				try
				{
					List<Callback> list2;
					if (callbackMsg_t.m_iCallback == 703)
					{
						SteamAPICallCompleted_t steamAPICallCompleted_t = Marshal.PtrToStructure<SteamAPICallCompleted_t>(callbackMsg_t.m_pubParam);
						IntPtr intPtr = Marshal.AllocHGlobal((int)steamAPICallCompleted_t.m_cubParam);
						bool flag;
						if (NativeMethods.SteamAPI_ManualDispatch_GetAPICallResult(hsteamPipe, steamAPICallCompleted_t.m_hAsyncCall, intPtr, (int)steamAPICallCompleted_t.m_cubParam, steamAPICallCompleted_t.m_iCallback, out flag))
						{
							object obj = CallbackDispatcher.m_sync;
							lock (obj)
							{
								List<CallResult> list;
								if (CallbackDispatcher.m_registeredCallResults.TryGetValue((ulong)steamAPICallCompleted_t.m_hAsyncCall, out list))
								{
									CallbackDispatcher.m_registeredCallResults.Remove((ulong)steamAPICallCompleted_t.m_hAsyncCall);
									foreach (CallResult callResult in list)
									{
										callResult.OnRunCallResult(intPtr, flag, (ulong)steamAPICallCompleted_t.m_hAsyncCall);
										callResult.SetUnregistered();
									}
								}
							}
						}
						Marshal.FreeHGlobal(intPtr);
					}
					else if (dictionary.TryGetValue(callbackMsg_t.m_iCallback, out list2))
					{
						object obj = CallbackDispatcher.m_sync;
						List<Callback> list3;
						lock (obj)
						{
							list3 = new List<Callback>(list2);
						}
						foreach (Callback callback in list3)
						{
							callback.OnRunCallback(callbackMsg_t.m_pubParam);
						}
					}
				}
				catch (Exception ex)
				{
					CallbackDispatcher.ExceptionHandler(ex);
				}
				finally
				{
					NativeMethods.SteamAPI_ManualDispatch_FreeLastCallback(hsteamPipe);
				}
			}
		}

		// Token: 0x04000B01 RID: 2817
		public static CallbackDispatcher.SteamworksExceptionHandler ExceptionHandler = new CallbackDispatcher.SteamworksExceptionHandler(CallbackDispatcher.DefaultExceptionHandler);

		// Token: 0x04000B02 RID: 2818
		private static Dictionary<int, List<Callback>> m_registeredCallbacks = new Dictionary<int, List<Callback>>();

		// Token: 0x04000B03 RID: 2819
		private static Dictionary<int, List<Callback>> m_registeredGameServerCallbacks = new Dictionary<int, List<Callback>>();

		// Token: 0x04000B04 RID: 2820
		private static Dictionary<ulong, List<CallResult>> m_registeredCallResults = new Dictionary<ulong, List<CallResult>>();

		// Token: 0x04000B05 RID: 2821
		private static object m_sync = new object();

		// Token: 0x04000B06 RID: 2822
		private static IntPtr m_pCallbackMsg;

		// Token: 0x04000B07 RID: 2823
		private static int m_initCount;

		// Token: 0x020001C8 RID: 456
		// (Invoke) Token: 0x06000B92 RID: 2962
		public delegate void SteamworksExceptionHandler(Exception e);
	}
}
