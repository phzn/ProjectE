using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.Loading
{
	// Token: 0x020010E0 RID: 4320
	public static class LoadingStallMonitor
	{
		// Token: 0x170014C5 RID: 5317
		// (get) Token: 0x06008305 RID: 33541 RVA: 0x003578B3 File Offset: 0x00355AB3
		// (set) Token: 0x06008306 RID: 33542 RVA: 0x003578BA File Offset: 0x00355ABA
		public static string LastDiagnostic { get; private set; }

		// Token: 0x170014C6 RID: 5318
		// (get) Token: 0x06008307 RID: 33543 RVA: 0x003578C2 File Offset: 0x00355AC2
		public static bool IsStalled
		{
			get
			{
				return LoadingStallMonitor.IsStalledAt(Time.frameCount);
			}
		}

		// Token: 0x06008308 RID: 33544 RVA: 0x003578CE File Offset: 0x00355ACE
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			LoadingStallMonitor.Clear();
		}

		// Token: 0x06008309 RID: 33545 RVA: 0x003578D5 File Offset: 0x00355AD5
		public static void Report(object owner, bool stalled, Func<string> diagnostic = null)
		{
			LoadingStallMonitor.Report(owner, stalled, diagnostic, Time.frameCount, Time.realtimeSinceStartupAsDouble);
		}

		// Token: 0x0600830A RID: 33546 RVA: 0x003578EC File Offset: 0x00355AEC
		public static void Report(object owner, bool stalled, Func<string> diagnostic, int frame, double now)
		{
			if (owner == null)
			{
				return;
			}
			LoadingStallMonitor.Entry entry;
			if (!LoadingStallMonitor.Reports.TryGetValue(owner, out entry))
			{
				if (!stalled)
				{
					return;
				}
				entry = new LoadingStallMonitor.Entry();
				LoadingStallMonitor.Reports.Add(owner, entry);
			}
			bool flag = entry.stalled && LoadingStallMonitor.IsLive(entry, frame);
			entry.frame = frame;
			if (stalled && !flag)
			{
				entry.stalled = true;
				entry.stalledSince = now;
				LoadingStallMonitor.LastDiagnostic = ((diagnostic != null) ? diagnostic() : null);
				Debug.LogWarning("[Loading] Preparation is taking longer than usual; still waiting (" + LoadingStallMonitor.Describe(owner) + "). " + LoadingStallMonitor.LastDiagnostic);
				return;
			}
			if (!stalled)
			{
				if (flag)
				{
					Debug.Log(string.Format("[Loading] Preparation resumed after {0:0.#}s without progress ", now - entry.stalledSince) + "(" + LoadingStallMonitor.Describe(owner) + ").");
				}
				LoadingStallMonitor.Reports.Remove(owner);
			}
		}

		// Token: 0x0600830B RID: 33547 RVA: 0x003579C6 File Offset: 0x00355BC6
		public static void Remove(object owner)
		{
			if (owner != null)
			{
				LoadingStallMonitor.Reports.Remove(owner);
			}
		}

		// Token: 0x0600830C RID: 33548 RVA: 0x003579D8 File Offset: 0x00355BD8
		public static bool IsStalledAt(int frame)
		{
			foreach (LoadingStallMonitor.Entry entry in LoadingStallMonitor.Reports.Values)
			{
				if (entry.stalled && LoadingStallMonitor.IsLive(entry, frame))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600830D RID: 33549 RVA: 0x00357A40 File Offset: 0x00355C40
		public static void Clear()
		{
			LoadingStallMonitor.Reports.Clear();
			LoadingStallMonitor.LastDiagnostic = null;
		}

		// Token: 0x0600830E RID: 33550 RVA: 0x00357A52 File Offset: 0x00355C52
		private static bool IsLive(LoadingStallMonitor.Entry report, int frame)
		{
			return frame - report.frame <= 2;
		}

		// Token: 0x0600830F RID: 33551 RVA: 0x00357A64 File Offset: 0x00355C64
		private static string Describe(object owner)
		{
			Object @object = owner as Object;
			if (@object == null || !(@object != null))
			{
				return owner.GetType().Name;
			}
			return owner.GetType().Name + " '" + @object.name + "'";
		}

		// Token: 0x0400862B RID: 34347
		private const int ReportLifetimeFrames = 2;

		// Token: 0x0400862C RID: 34348
		private static readonly Dictionary<object, LoadingStallMonitor.Entry> Reports = new Dictionary<object, LoadingStallMonitor.Entry>();

		// Token: 0x020010E1 RID: 4321
		private sealed class Entry
		{
			// Token: 0x0400862E RID: 34350
			public int frame;

			// Token: 0x0400862F RID: 34351
			public bool stalled;

			// Token: 0x04008630 RID: 34352
			public double stalledSince;
		}
	}
}
