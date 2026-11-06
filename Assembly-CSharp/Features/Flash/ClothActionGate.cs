using System;
using Features.HideFlash;
using ProjectExhibitionist.CoverWall;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.Flash
{
	// Token: 0x02001206 RID: 4614
	public static class ClothActionGate
	{
		// Token: 0x140000F9 RID: 249
		// (add) Token: 0x06008E6C RID: 36460 RVA: 0x003A3D04 File Offset: 0x003A1F04
		// (remove) Token: 0x06008E6D RID: 36461 RVA: 0x003A3D38 File Offset: 0x003A1F38
		public static event Action<object> Started;

		// Token: 0x06008E6E RID: 36462 RVA: 0x003A3D6C File Offset: 0x003A1F6C
		private static string Name(object o)
		{
			Object @object = o as Object;
			if (@object != null && @object != null)
			{
				return @object.name;
			}
			if (o == null)
			{
				return "null";
			}
			return o.ToString();
		}

		// Token: 0x170016C5 RID: 5829
		// (get) Token: 0x06008E6F RID: 36463 RVA: 0x003A3DA2 File Offset: 0x003A1FA2
		public static bool IsBusy
		{
			get
			{
				return ClothActionGate._owner != null;
			}
		}

		// Token: 0x170016C6 RID: 5830
		// (get) Token: 0x06008E70 RID: 36464 RVA: 0x003A3DAC File Offset: 0x003A1FAC
		public static object CurrentOwner
		{
			get
			{
				return ClothActionGate._owner;
			}
		}

		// Token: 0x06008E71 RID: 36465 RVA: 0x003A3DB4 File Offset: 0x003A1FB4
		public static bool TryBegin(object owner)
		{
			if (owner == null)
			{
				return false;
			}
			if (HideFlashController.HasHeldCoverItem)
			{
				return false;
			}
			if (WallCoverActivityLock.ActiveController != null)
			{
				return false;
			}
			if (ClothActionGate._owner != null)
			{
				if (ClothActionGate.DebugLogs)
				{
					Debug.Log(string.Concat(new string[]
					{
						"[ClothActionGate] DENY begin by '",
						ClothActionGate.Name(owner),
						"' — held by '",
						ClothActionGate.Name(ClothActionGate._owner),
						"'"
					}));
				}
				return false;
			}
			ClothActionGate._owner = owner;
			Action<object> started = ClothActionGate.Started;
			if (started != null)
			{
				started(owner);
			}
			if (ClothActionGate.DebugLogs)
			{
				Debug.Log("[ClothActionGate] BEGIN by '" + ClothActionGate.Name(owner) + "'");
			}
			return true;
		}

		// Token: 0x06008E72 RID: 36466 RVA: 0x003A3E68 File Offset: 0x003A2068
		public static bool TryAcquire(object owner)
		{
			if (owner == null)
			{
				return false;
			}
			if (ClothActionGate._owner == null && HideFlashController.HasHeldCoverItem)
			{
				return false;
			}
			if (ClothActionGate._owner == null && WallCoverActivityLock.ActiveController != null)
			{
				return false;
			}
			if (ClothActionGate._owner != null && ClothActionGate._owner != owner)
			{
				if (ClothActionGate.DebugLogs)
				{
					Debug.Log(string.Concat(new string[]
					{
						"[ClothActionGate] DENY acquire by '",
						ClothActionGate.Name(owner),
						"' — held by '",
						ClothActionGate.Name(ClothActionGate._owner),
						"'"
					}));
				}
				return false;
			}
			bool flag = ClothActionGate._owner == null;
			ClothActionGate._owner = owner;
			if (flag)
			{
				Action<object> started = ClothActionGate.Started;
				if (started != null)
				{
					started(owner);
				}
			}
			if (ClothActionGate.DebugLogs && flag)
			{
				Debug.Log("[ClothActionGate] ACQUIRE by '" + ClothActionGate.Name(owner) + "'");
			}
			return true;
		}

		// Token: 0x06008E73 RID: 36467 RVA: 0x003A3F3D File Offset: 0x003A213D
		public static void Release(object owner)
		{
			if (ClothActionGate._owner == owner)
			{
				if (ClothActionGate.DebugLogs)
				{
					Debug.Log("[ClothActionGate] RELEASE by '" + ClothActionGate.Name(owner) + "'");
				}
				ClothActionGate._owner = null;
			}
		}

		// Token: 0x06008E74 RID: 36468 RVA: 0x003A3F6E File Offset: 0x003A216E
		public static bool Holds(object owner)
		{
			return ClothActionGate._owner == owner;
		}

		// Token: 0x06008E75 RID: 36469 RVA: 0x003A3F78 File Offset: 0x003A2178
		public static void ForceClear()
		{
			ClothActionGate._owner = null;
		}

		// Token: 0x06008E76 RID: 36470 RVA: 0x003A3F80 File Offset: 0x003A2180
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void InstallSessionResetHook()
		{
			SceneManager.activeSceneChanged -= ClothActionGate.OnActiveSceneChanged;
			SceneManager.activeSceneChanged += ClothActionGate.OnActiveSceneChanged;
		}

		// Token: 0x06008E77 RID: 36471 RVA: 0x003A3FA4 File Offset: 0x003A21A4
		private static void OnActiveSceneChanged(Scene from, Scene to)
		{
			if (ClothActionGate.DebugLogs && ClothActionGate._owner != null)
			{
				Debug.Log("[ClothActionGate] scene change → clear (was held by '" + ClothActionGate.Name(ClothActionGate._owner) + "')");
			}
			ClothActionGate._owner = null;
		}

		// Token: 0x040091EE RID: 37358
		private static object _owner;

		// Token: 0x040091F0 RID: 37360
		public static bool DebugLogs;
	}
}
