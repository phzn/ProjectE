using System;
using System.Collections.Generic;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;

namespace ProjectExhibitionist.BenchSeating
{
	// Token: 0x02000B2F RID: 2863
	public sealed class BenchInteractionScheduler : MonoBehaviour
	{
		// Token: 0x17000D8A RID: 3466
		// (get) Token: 0x0600513A RID: 20794 RVA: 0x001F78F4 File Offset: 0x001F5AF4
		public static int ScheduledCount
		{
			get
			{
				return BenchInteractionScheduler._all.Count;
			}
		}

		// Token: 0x17000D8B RID: 3467
		// (get) Token: 0x0600513B RID: 20795 RVA: 0x001F7900 File Offset: 0x001F5B00
		public static int NearbyCount
		{
			get
			{
				return BenchInteractionScheduler._nearby.Count;
			}
		}

		// Token: 0x0600513C RID: 20796 RVA: 0x001F790C File Offset: 0x001F5B0C
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			BenchInteractionScheduler._all.Clear();
			BenchInteractionScheduler._nearby.Clear();
			BenchInteractionScheduler._instance = null;
			BenchInteractionScheduler._cursor = 0;
		}

		// Token: 0x0600513D RID: 20797 RVA: 0x001F7930 File Offset: 0x001F5B30
		internal static void Add(BenchInteractable bench)
		{
			if (bench == null || bench.ScheduledIndex >= 0)
			{
				return;
			}
			bench.ScheduledIndex = BenchInteractionScheduler._all.Count;
			BenchInteractionScheduler._all.Add(bench);
			if (BenchInteractionScheduler._instance == null && Application.isPlaying)
			{
				BenchInteractionScheduler.Install();
			}
		}

		// Token: 0x0600513E RID: 20798 RVA: 0x001F7984 File Offset: 0x001F5B84
		internal static void Remove(BenchInteractable bench)
		{
			if (bench == null)
			{
				return;
			}
			if (bench.NearbyIndex >= 0)
			{
				BenchInteractionScheduler.SetNearby(bench, false);
			}
			BenchInteractionScheduler.RemoveAt(BenchInteractionScheduler._all, bench, bench.ScheduledIndex, true);
			bench.ScheduledIndex = -1;
		}

		// Token: 0x0600513F RID: 20799 RVA: 0x001F79B3 File Offset: 0x001F5BB3
		private static void Install()
		{
			GameObject gameObject = new GameObject("[BenchInteractionScheduler]");
			Object.DontDestroyOnLoad(gameObject);
			BenchInteractionScheduler._instance = gameObject.AddComponent<BenchInteractionScheduler>();
		}

		// Token: 0x06005140 RID: 20800 RVA: 0x001F79CF File Offset: 0x001F5BCF
		private void OnDestroy()
		{
			if (BenchInteractionScheduler._instance == this)
			{
				BenchInteractionScheduler._instance = null;
			}
		}

		// Token: 0x06005141 RID: 20801 RVA: 0x001F79E4 File Offset: 0x001F5BE4
		private void Update()
		{
			BenchInteractionScheduler.Step(PlayerRegistry.Player);
		}

		// Token: 0x06005142 RID: 20802 RVA: 0x001F79F0 File Offset: 0x001F5BF0
		internal static void Step(Transform player)
		{
			if (player == null)
			{
				for (int i = BenchInteractionScheduler._nearby.Count - 1; i >= 0; i--)
				{
					BenchInteractionScheduler.SetNearby(BenchInteractionScheduler._nearby[i], false);
				}
				return;
			}
			Vector3 position = player.position;
			int num = Mathf.Min(BenchInteractionScheduler._all.Count, Mathf.Max(16, (BenchInteractionScheduler._all.Count + 10 - 1) / 10));
			for (int j = 0; j < num; j++)
			{
				if (BenchInteractionScheduler._cursor >= BenchInteractionScheduler._all.Count)
				{
					BenchInteractionScheduler._cursor = 0;
				}
				BenchInteractable benchInteractable = BenchInteractionScheduler._all[BenchInteractionScheduler._cursor++];
				float sqrMagnitude = (benchInteractable.ProximityPosition - position).sqrMagnitude;
				if (benchInteractable.NearbyIndex < 0)
				{
					if (sqrMagnitude < 36f)
					{
						BenchInteractionScheduler.SetNearby(benchInteractable, true);
					}
				}
				else if (sqrMagnitude > 56.25f && !benchInteractable.IsBeingUsed)
				{
					BenchInteractionScheduler.SetNearby(benchInteractable, false);
				}
			}
			for (int k = BenchInteractionScheduler._nearby.Count - 1; k >= 0; k--)
			{
				if (k < BenchInteractionScheduler._nearby.Count)
				{
					BenchInteractionScheduler._nearby[k].TickNearby();
				}
			}
		}

		// Token: 0x06005143 RID: 20803 RVA: 0x001F7B2C File Offset: 0x001F5D2C
		private static void SetNearby(BenchInteractable bench, bool nearby)
		{
			if (nearby)
			{
				if (bench.NearbyIndex >= 0)
				{
					return;
				}
				bench.NearbyIndex = BenchInteractionScheduler._nearby.Count;
				BenchInteractionScheduler._nearby.Add(bench);
				bench.Wake();
				return;
			}
			else
			{
				if (bench.NearbyIndex < 0)
				{
					return;
				}
				BenchInteractionScheduler.RemoveAt(BenchInteractionScheduler._nearby, bench, bench.NearbyIndex, false);
				bench.NearbyIndex = -1;
				bench.Sleep();
				return;
			}
		}

		// Token: 0x06005144 RID: 20804 RVA: 0x001F7B94 File Offset: 0x001F5D94
		private static void RemoveAt(List<BenchInteractable> list, BenchInteractable bench, int index, bool scheduled)
		{
			if (index < 0 || index >= list.Count || list[index] != bench)
			{
				index = list.IndexOf(bench);
				if (index < 0)
				{
					return;
				}
			}
			int num = list.Count - 1;
			if (index != num)
			{
				BenchInteractable benchInteractable = list[num];
				list[index] = benchInteractable;
				if (scheduled)
				{
					benchInteractable.ScheduledIndex = index;
				}
				else
				{
					benchInteractable.NearbyIndex = index;
				}
			}
			list.RemoveAt(num);
		}

		// Token: 0x040051E8 RID: 20968
		public const float WakeDistance = 6f;

		// Token: 0x040051E9 RID: 20969
		public const float SleepDistance = 7.5f;

		// Token: 0x040051EA RID: 20970
		private const int SweepFrames = 10;

		// Token: 0x040051EB RID: 20971
		private const int MinimumSweep = 16;

		// Token: 0x040051EC RID: 20972
		private static readonly List<BenchInteractable> _all = new List<BenchInteractable>(256);

		// Token: 0x040051ED RID: 20973
		private static readonly List<BenchInteractable> _nearby = new List<BenchInteractable>(16);

		// Token: 0x040051EE RID: 20974
		private static BenchInteractionScheduler _instance;

		// Token: 0x040051EF RID: 20975
		private static int _cursor;
	}
}
