using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectExhibitionist.Core
{
	// Token: 0x02000A61 RID: 2657
	public static class PlayerActivityGate
	{
		// Token: 0x1400007A RID: 122
		// (add) Token: 0x06004B98 RID: 19352 RVA: 0x001D5B64 File Offset: 0x001D3D64
		// (remove) Token: 0x06004B99 RID: 19353 RVA: 0x001D5B98 File Offset: 0x001D3D98
		public static event Action<PlayerActivity> Changed;

		// Token: 0x1400007B RID: 123
		// (add) Token: 0x06004B9A RID: 19354 RVA: 0x001D5BCC File Offset: 0x001D3DCC
		// (remove) Token: 0x06004B9B RID: 19355 RVA: 0x001D5C00 File Offset: 0x001D3E00
		public static event Action<PlayerContextResource> ResourcesChanged;

		// Token: 0x17000CC8 RID: 3272
		// (get) Token: 0x06004B9C RID: 19356 RVA: 0x001D5C33 File Offset: 0x001D3E33
		public static PlayerActivity BlockedMask
		{
			get
			{
				return PlayerActivityGate._blockedMask;
			}
		}

		// Token: 0x17000CC9 RID: 3273
		// (get) Token: 0x06004B9D RID: 19357 RVA: 0x001D5C3A File Offset: 0x001D3E3A
		public static PlayerContextResource ActiveResources
		{
			get
			{
				return PlayerActivityGate._activeResources;
			}
		}

		// Token: 0x17000CCA RID: 3274
		// (get) Token: 0x06004B9E RID: 19358 RVA: 0x001D5C41 File Offset: 0x001D3E41
		public static bool AnyActive
		{
			get
			{
				return PlayerActivityGate._locks.Count > 0 || PlayerActivityGate._leases.Count > 0;
			}
		}

		// Token: 0x06004B9F RID: 19359 RVA: 0x001D5C5F File Offset: 0x001D3E5F
		public static bool IsBlocked(PlayerActivity activity)
		{
			return activity != PlayerActivity.None && (GameplayInputGate.BlocksGameplayInput || (PlayerActivityGate._blockedMask & activity) > PlayerActivity.None);
		}

		// Token: 0x06004BA0 RID: 19360 RVA: 0x001D5C79 File Offset: 0x001D3E79
		public static bool Allows(PlayerActivity activity)
		{
			return !PlayerActivityGate.IsBlocked(activity);
		}

		// Token: 0x06004BA1 RID: 19361 RVA: 0x001D5C84 File Offset: 0x001D3E84
		public static bool AllowsIgnoringLegacyReason(PlayerActivity activity, string ignoredLegacyReason)
		{
			if (activity != PlayerActivity.None && GameplayInputGate.BlocksGameplayInput)
			{
				return false;
			}
			foreach (PlayerActivityGate.Lock @lock in PlayerActivityGate._locks)
			{
				if (@lock.reason != ignoredLegacyReason && (@lock.mask & activity) != PlayerActivity.None)
				{
					return false;
				}
			}
			using (List<PlayerActivityGate.LeaseRecord>.Enumerator enumerator2 = PlayerActivityGate._leases.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if ((enumerator2.Current.blockedActivities & activity) != PlayerActivity.None)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x17000CCB RID: 3275
		// (get) Token: 0x06004BA2 RID: 19362 RVA: 0x001D5D40 File Offset: 0x001D3F40
		public static bool CanStartWorldInteraction
		{
			get
			{
				return !TextInputFocus.BlocksGameplayInput && PlayerActivityGate.Allows(PlayerActivity.Interact) && !PlayerActivityGate.IsResourceOwned(PlayerContextResource.WorldInteraction);
			}
		}

		// Token: 0x17000CCC RID: 3276
		// (get) Token: 0x06004BA3 RID: 19363 RVA: 0x001D5D60 File Offset: 0x001D3F60
		public static bool CanStartMap
		{
			get
			{
				PlayerContextResource playerContextResource;
				return PlayerActivityGate.CanAcquireUtilityModal(PlayerActivity.AllPlayerActions, PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.ModalUI | PlayerContextResource.Rider | PlayerContextResource.HeldItem | PlayerContextResource.WorldSimulation | PlayerContextResource.WaterMovement, out playerContextResource, "Rank");
			}
		}

		// Token: 0x06004BA4 RID: 19364 RVA: 0x001D5D84 File Offset: 0x001D3F84
		public static bool CanAcquireUtilityModal(PlayerActivity requiredActivities, PlayerContextResource resources, out PlayerContextResource availableResources, string ignoredLegacyReason = null)
		{
			availableResources = resources;
			if (TextInputFocus.BlocksGameplayInput || GameplayInputGate.BlocksGameplayInput)
			{
				return false;
			}
			foreach (PlayerActivityGate.Lock @lock in PlayerActivityGate._locks)
			{
				if (@lock.reason != ignoredLegacyReason && (@lock.mask & requiredActivities) != PlayerActivity.None)
				{
					return false;
				}
			}
			foreach (PlayerActivityGate.LeaseRecord leaseRecord in PlayerActivityGate._leases)
			{
				if (leaseRecord.lease.AllowsUtilityModals && leaseRecord.lease.Owner != null)
				{
					availableResources &= ~leaseRecord.resources;
				}
				else if ((leaseRecord.blockedActivities & requiredActivities) != PlayerActivity.None || (leaseRecord.resources & resources) != PlayerContextResource.None)
				{
					return false;
				}
			}
			return (availableResources & PlayerContextResource.ModalUI) > PlayerContextResource.None;
		}

		// Token: 0x06004BA5 RID: 19365 RVA: 0x001D5E94 File Offset: 0x001D4094
		public static bool TryAcquireUtilityModal(string reason, Object owner, PlayerActivity requiredActivities, PlayerContextResource resources, PlayerActivity blockedActivities, out PlayerActivityLease lease, string ignoredLegacyReason = null)
		{
			lease = null;
			PlayerContextResource playerContextResource;
			return PlayerActivityGate.CanAcquireUtilityModal(requiredActivities, resources, out playerContextResource, ignoredLegacyReason) && PlayerActivityGate.TryAcquire(reason, owner, PlayerActivity.None, playerContextResource, blockedActivities, out lease, false, null);
		}

		// Token: 0x06004BA6 RID: 19366 RVA: 0x001D5EC3 File Offset: 0x001D40C3
		public static bool IsResourceOwned(PlayerContextResource resources)
		{
			return (PlayerActivityGate._activeResources & resources) > PlayerContextResource.None;
		}

		// Token: 0x06004BA7 RID: 19367 RVA: 0x001D5ED0 File Offset: 0x001D40D0
		public static T GetResourceOwner<T>(PlayerContextResource resources) where T : Object
		{
			foreach (PlayerActivityGate.LeaseRecord leaseRecord in PlayerActivityGate._leases)
			{
				if ((leaseRecord.resources & resources) != PlayerContextResource.None)
				{
					T t = leaseRecord.lease.Owner as T;
					if (t != null && t != null)
					{
						return t;
					}
				}
			}
			return default(T);
		}

		// Token: 0x06004BA8 RID: 19368 RVA: 0x001D5F64 File Offset: 0x001D4164
		public static bool CanAcquireAfterReleasing(PlayerActivity requiredActivities, PlayerContextResource resources, Predicate<Object> willReleaseOwner)
		{
			if (requiredActivities != PlayerActivity.None && GameplayInputGate.BlocksGameplayInput)
			{
				return false;
			}
			for (int i = 0; i < PlayerActivityGate._locks.Count; i++)
			{
				if ((PlayerActivityGate._locks[i].mask & requiredActivities) != PlayerActivity.None)
				{
					return false;
				}
			}
			for (int j = 0; j < PlayerActivityGate._leases.Count; j++)
			{
				PlayerActivityGate.LeaseRecord leaseRecord = PlayerActivityGate._leases[j];
				if ((!(leaseRecord.lease.Owner != null) || willReleaseOwner == null || !willReleaseOwner(leaseRecord.lease.Owner)) && ((leaseRecord.blockedActivities & requiredActivities) != PlayerActivity.None || (leaseRecord.resources & resources) != PlayerContextResource.None))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004BA9 RID: 19369 RVA: 0x001D600C File Offset: 0x001D420C
		public static bool CanAcquireAlongside(PlayerActivity requiredActivities, PlayerContextResource resources, Object companion, string companionLegacyReason)
		{
			if (companion == null || TextInputFocus.BlocksGameplayInput || (requiredActivities != PlayerActivity.None && GameplayInputGate.BlocksGameplayInput))
			{
				return false;
			}
			for (int i = 0; i < PlayerActivityGate._locks.Count; i++)
			{
				if (PlayerActivityGate._locks[i].reason != companionLegacyReason && (PlayerActivityGate._locks[i].mask & requiredActivities) != PlayerActivity.None)
				{
					return false;
				}
			}
			for (int j = 0; j < PlayerActivityGate._leases.Count; j++)
			{
				PlayerActivityGate.LeaseRecord leaseRecord = PlayerActivityGate._leases[j];
				if (leaseRecord.lease.Owner != companion && ((leaseRecord.blockedActivities & requiredActivities) != PlayerActivity.None || (leaseRecord.resources & resources) != PlayerContextResource.None))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004BAA RID: 19370 RVA: 0x001D60C0 File Offset: 0x001D42C0
		public static bool TryAcquire(string reason, Object owner, PlayerActivity requiredActivities, PlayerContextResource resources, PlayerActivity blockedActivities, out PlayerActivityLease lease, bool allowsUtilityModals = false, string ignoredLegacyReason = null)
		{
			lease = null;
			if (string.IsNullOrEmpty(reason) || resources == PlayerContextResource.None || (resources & ~(PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.ModalUI | PlayerContextResource.Rider | PlayerContextResource.HeldItem | PlayerContextResource.WorldSimulation | PlayerContextResource.BodyAttachment | PlayerContextResource.WaterMovement)) != PlayerContextResource.None)
			{
				return false;
			}
			if (!PlayerActivityGate.AllowsIgnoringLegacyReason(requiredActivities, ignoredLegacyReason))
			{
				if (PlayerActivityGate.DebugLogs)
				{
					Debug.Log(string.Format("[PlayerActivityGate] DENY '{0}' required={1} ", reason, requiredActivities) + string.Format("blocked={0}", PlayerActivityGate._blockedMask & requiredActivities));
				}
				return false;
			}
			PlayerContextResource playerContextResource = PlayerActivityGate._activeResources & resources;
			if (playerContextResource != PlayerContextResource.None)
			{
				if (PlayerActivityGate.DebugLogs)
				{
					Debug.Log(string.Format("[PlayerActivityGate] DENY '{0}' resources={1} ", reason, resources) + string.Format("conflicts={0}", playerContextResource));
				}
				return false;
			}
			lease = new PlayerActivityLease(reason, owner, requiredActivities, resources, blockedActivities, allowsUtilityModals);
			PlayerActivityGate._leases.Add(new PlayerActivityGate.LeaseRecord
			{
				lease = lease,
				resources = resources,
				blockedActivities = blockedActivities
			});
			PlayerActivityGate.Recalculate();
			if (!lease.IsActive)
			{
				lease = null;
				return false;
			}
			if (PlayerActivityGate.DebugLogs)
			{
				Debug.Log(string.Format("[PlayerActivityGate] ACQUIRE '{0}' resources={1} ", reason, resources) + string.Format("blocked={0}", blockedActivities));
			}
			return true;
		}

		// Token: 0x06004BAB RID: 19371 RVA: 0x001D61F1 File Offset: 0x001D43F1
		public static bool TryAcquire(string reason, PlayerActivity requiredActivities, PlayerContextResource resources, PlayerActivity blockedActivities, out PlayerActivityLease lease)
		{
			return PlayerActivityGate.TryAcquire(reason, null, requiredActivities, resources, blockedActivities, out lease, false, null);
		}

		// Token: 0x06004BAC RID: 19372 RVA: 0x001D6204 File Offset: 0x001D4404
		public static bool TryUpdate(PlayerActivityLease lease, PlayerActivity requiredActivities, PlayerContextResource resources, PlayerActivity blockedActivities)
		{
			if (requiredActivities != PlayerActivity.None && GameplayInputGate.BlocksGameplayInput)
			{
				return false;
			}
			if (lease == null || resources == PlayerContextResource.None || (resources & ~(PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.ModalUI | PlayerContextResource.Rider | PlayerContextResource.HeldItem | PlayerContextResource.WorldSimulation | PlayerContextResource.BodyAttachment | PlayerContextResource.WaterMovement)) != PlayerContextResource.None)
			{
				return false;
			}
			int num = PlayerActivityGate.FindLeaseIndex(lease);
			if (num < 0)
			{
				return false;
			}
			PlayerActivity playerActivity = PlayerActivity.None;
			for (int i = 0; i < PlayerActivityGate._locks.Count; i++)
			{
				playerActivity |= PlayerActivityGate._locks[i].mask;
			}
			PlayerContextResource playerContextResource = PlayerContextResource.None;
			for (int j = 0; j < PlayerActivityGate._leases.Count; j++)
			{
				if (j != num)
				{
					playerActivity |= PlayerActivityGate._leases[j].blockedActivities;
					playerContextResource |= PlayerActivityGate._leases[j].resources;
				}
			}
			PlayerActivity playerActivity2 = playerActivity & requiredActivities;
			if (playerActivity2 != PlayerActivity.None)
			{
				if (PlayerActivityGate.DebugLogs)
				{
					Debug.Log("[PlayerActivityGate] DENY UPDATE '" + lease.Reason + "' " + string.Format("required={0} blocked={1}", requiredActivities, playerActivity2));
				}
				return false;
			}
			PlayerContextResource playerContextResource2 = playerContextResource & resources;
			if (playerContextResource2 != PlayerContextResource.None)
			{
				if (PlayerActivityGate.DebugLogs)
				{
					Debug.Log("[PlayerActivityGate] DENY UPDATE '" + lease.Reason + "' " + string.Format("resources={0} conflicts={1}", resources, playerContextResource2));
				}
				return false;
			}
			PlayerActivityGate.LeaseRecord leaseRecord = PlayerActivityGate._leases[num];
			if (lease.RequiredActivities == requiredActivities && leaseRecord.resources == resources && leaseRecord.blockedActivities == blockedActivities)
			{
				return true;
			}
			leaseRecord.resources = resources;
			leaseRecord.blockedActivities = blockedActivities;
			PlayerActivityGate._leases[num] = leaseRecord;
			lease.ApplyProfile(requiredActivities, resources, blockedActivities);
			PlayerActivityGate.Recalculate();
			if (PlayerActivityGate.FindLeaseIndex(lease) < 0 || lease.RequiredActivities != requiredActivities || lease.Resources != resources || lease.BlockedActivities != blockedActivities)
			{
				return false;
			}
			if (PlayerActivityGate.DebugLogs)
			{
				Debug.Log("[PlayerActivityGate] UPDATE LEASE '" + lease.Reason + "' " + string.Format("resources={0} blocked={1}", resources, blockedActivities));
			}
			return true;
		}

		// Token: 0x06004BAD RID: 19373 RVA: 0x001D63EA File Offset: 0x001D45EA
		internal static bool IsLeaseActive(PlayerActivityLease lease)
		{
			return PlayerActivityGate.FindLeaseIndex(lease) >= 0;
		}

		// Token: 0x06004BAE RID: 19374 RVA: 0x001D63F8 File Offset: 0x001D45F8
		private static int FindLeaseIndex(PlayerActivityLease lease)
		{
			if (lease == null)
			{
				return -1;
			}
			for (int i = 0; i < PlayerActivityGate._leases.Count; i++)
			{
				if (PlayerActivityGate._leases[i].lease == lease)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06004BAF RID: 19375 RVA: 0x001D6438 File Offset: 0x001D4638
		internal static void Release(PlayerActivityLease lease)
		{
			if (lease == null)
			{
				return;
			}
			for (int i = 0; i < PlayerActivityGate._leases.Count; i++)
			{
				if (PlayerActivityGate._leases[i].lease == lease)
				{
					PlayerActivityGate._leases.RemoveAt(i);
					PlayerActivityGate.Recalculate();
					if (PlayerActivityGate.DebugLogs)
					{
						Debug.Log("[PlayerActivityGate] RELEASE '" + lease.Reason + "'");
					}
					return;
				}
			}
		}

		// Token: 0x06004BB0 RID: 19376 RVA: 0x001D64A4 File Offset: 0x001D46A4
		public static bool HasLock(string reason)
		{
			for (int i = 0; i < PlayerActivityGate._locks.Count; i++)
			{
				if (PlayerActivityGate._locks[i].reason == reason)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004BB1 RID: 19377 RVA: 0x001D64E4 File Offset: 0x001D46E4
		public static bool HasLease(string reason)
		{
			for (int i = 0; i < PlayerActivityGate._leases.Count; i++)
			{
				if (PlayerActivityGate._leases[i].lease.Reason == reason)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004BB2 RID: 19378 RVA: 0x001D6528 File Offset: 0x001D4728
		public static void Push(string reason, PlayerActivity mask = PlayerActivity.AllPlayerActions)
		{
			if (string.IsNullOrEmpty(reason))
			{
				return;
			}
			for (int i = 0; i < PlayerActivityGate._locks.Count; i++)
			{
				if (PlayerActivityGate._locks[i].reason == reason)
				{
					PlayerActivityGate._locks[i] = new PlayerActivityGate.Lock
					{
						reason = reason,
						mask = mask
					};
					PlayerActivityGate.Recalculate();
					if (PlayerActivityGate.DebugLogs)
					{
						Debug.Log(string.Format("[PlayerActivityGate] UPDATE '{0}' -> {1}  (blocked={2})", reason, mask, PlayerActivityGate._blockedMask));
					}
					return;
				}
			}
			PlayerActivityGate._locks.Add(new PlayerActivityGate.Lock
			{
				reason = reason,
				mask = mask
			});
			PlayerActivityGate.Recalculate();
			if (PlayerActivityGate.DebugLogs)
			{
				Debug.Log(string.Format("[PlayerActivityGate] PUSH '{0}' -> {1}  (blocked={2})", reason, mask, PlayerActivityGate._blockedMask));
			}
		}

		// Token: 0x06004BB3 RID: 19379 RVA: 0x001D660C File Offset: 0x001D480C
		public static void Pop(string reason)
		{
			if (string.IsNullOrEmpty(reason))
			{
				return;
			}
			for (int i = 0; i < PlayerActivityGate._locks.Count; i++)
			{
				if (PlayerActivityGate._locks[i].reason == reason)
				{
					PlayerActivityGate._locks.RemoveAt(i);
					PlayerActivityGate.Recalculate();
					if (PlayerActivityGate.DebugLogs)
					{
						Debug.Log(string.Format("[PlayerActivityGate] POP '{0}'  (blocked={1})", reason, PlayerActivityGate._blockedMask));
					}
					return;
				}
			}
		}

		// Token: 0x06004BB4 RID: 19380 RVA: 0x001D6684 File Offset: 0x001D4884
		public static void ForceClear()
		{
			if (PlayerActivityGate._locks.Count == 0 && PlayerActivityGate._leases.Count == 0)
			{
				return;
			}
			PlayerActivityGate._locks.Clear();
			PlayerActivityGate._leases.Clear();
			PlayerActivityGate.Recalculate();
			if (PlayerActivityGate.DebugLogs)
			{
				Debug.Log("[PlayerActivityGate] FORCE CLEAR");
			}
		}

		// Token: 0x06004BB5 RID: 19381 RVA: 0x001D66D4 File Offset: 0x001D48D4
		private static void ClearLegacyLocks()
		{
			if (PlayerActivityGate._locks.Count == 0)
			{
				return;
			}
			PlayerActivityGate._locks.Clear();
			PlayerActivityGate.Recalculate();
		}

		// Token: 0x06004BB6 RID: 19382 RVA: 0x001D66F4 File Offset: 0x001D48F4
		private static void Recalculate()
		{
			PlayerActivity playerActivity = PlayerActivity.None;
			for (int i = 0; i < PlayerActivityGate._locks.Count; i++)
			{
				playerActivity |= PlayerActivityGate._locks[i].mask;
			}
			PlayerContextResource playerContextResource = PlayerContextResource.None;
			for (int j = 0; j < PlayerActivityGate._leases.Count; j++)
			{
				playerActivity |= PlayerActivityGate._leases[j].blockedActivities;
				playerContextResource |= PlayerActivityGate._leases[j].resources;
			}
			bool flag = playerActivity != PlayerActivityGate._blockedMask;
			bool flag2 = playerContextResource != PlayerActivityGate._activeResources;
			if (!flag && !flag2)
			{
				return;
			}
			PlayerActivityGate._blockedMask = playerActivity;
			PlayerActivityGate._activeResources = playerContextResource;
			if (flag2)
			{
				Action<PlayerContextResource> resourcesChanged = PlayerActivityGate.ResourcesChanged;
				if (resourcesChanged != null)
				{
					resourcesChanged(PlayerActivityGate._activeResources);
				}
			}
			if (flag)
			{
				Action<PlayerActivity> changed = PlayerActivityGate.Changed;
				if (changed == null)
				{
					return;
				}
				changed(PlayerActivityGate._blockedMask);
			}
		}

		// Token: 0x06004BB7 RID: 19383 RVA: 0x001D67D0 File Offset: 0x001D49D0
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStaticState()
		{
			PlayerActivityGate._locks.Clear();
			PlayerActivityGate._leases.Clear();
			PlayerActivityGate._blockedMask = PlayerActivity.None;
			PlayerActivityGate._activeResources = PlayerContextResource.None;
			PlayerActivityGate.Changed = null;
			PlayerActivityGate.ResourcesChanged = null;
			PlayerActivityGate.DebugLogs = false;
			SceneManager.activeSceneChanged -= PlayerActivityGate.OnActiveSceneChanged;
		}

		// Token: 0x06004BB8 RID: 19384 RVA: 0x001D6820 File Offset: 0x001D4A20
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void InstallSessionResetHook()
		{
			SceneManager.activeSceneChanged -= PlayerActivityGate.OnActiveSceneChanged;
			SceneManager.activeSceneChanged += PlayerActivityGate.OnActiveSceneChanged;
		}

		// Token: 0x06004BB9 RID: 19385 RVA: 0x001D6844 File Offset: 0x001D4A44
		private static void OnActiveSceneChanged(Scene from, Scene to)
		{
			if (PlayerActivityGate.DebugLogs && PlayerActivityGate._locks.Count > 0)
			{
				Debug.Log("[PlayerActivityGate] active scene change -> clear " + string.Format("({0} lock legacy; {1} lease preservate)", PlayerActivityGate._locks.Count, PlayerActivityGate._leases.Count));
			}
			PlayerActivityGate.ClearLegacyLocks();
		}

		// Token: 0x04004D68 RID: 19816
		private static readonly List<PlayerActivityGate.Lock> _locks = new List<PlayerActivityGate.Lock>(4);

		// Token: 0x04004D69 RID: 19817
		private static readonly List<PlayerActivityGate.LeaseRecord> _leases = new List<PlayerActivityGate.LeaseRecord>(4);

		// Token: 0x04004D6A RID: 19818
		private static PlayerActivity _blockedMask = PlayerActivity.None;

		// Token: 0x04004D6B RID: 19819
		private static PlayerContextResource _activeResources = PlayerContextResource.None;

		// Token: 0x04004D6C RID: 19820
		public static bool DebugLogs = false;

		// Token: 0x02000A62 RID: 2658
		private struct Lock
		{
			// Token: 0x04004D6F RID: 19823
			public string reason;

			// Token: 0x04004D70 RID: 19824
			public PlayerActivity mask;
		}

		// Token: 0x02000A63 RID: 2659
		private struct LeaseRecord
		{
			// Token: 0x04004D71 RID: 19825
			public PlayerActivityLease lease;

			// Token: 0x04004D72 RID: 19826
			public PlayerContextResource resources;

			// Token: 0x04004D73 RID: 19827
			public PlayerActivity blockedActivities;
		}
	}
}
