using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// Token: 0x0200005E RID: 94
public static class HeavyZoneAddressableResidency
{
	// Token: 0x17000111 RID: 273
	// (get) Token: 0x060004C5 RID: 1221 RVA: 0x00025B8F File Offset: 0x00023D8F
	public static int EntryCount
	{
		get
		{
			HeavyZoneAddressableResidency.Store store = HeavyZoneAddressableResidency._store;
			if (store == null)
			{
				return 0;
			}
			return store.EntryCount;
		}
	}

	// Token: 0x17000112 RID: 274
	// (get) Token: 0x060004C6 RID: 1222 RVA: 0x00025BA1 File Offset: 0x00023DA1
	public static int ActiveLoadCount
	{
		get
		{
			HeavyZoneAddressableResidency.Store store = HeavyZoneAddressableResidency._store;
			if (store == null)
			{
				return 0;
			}
			return store.ActiveLoadCount;
		}
	}

	// Token: 0x17000113 RID: 275
	// (get) Token: 0x060004C7 RID: 1223 RVA: 0x00025BB3 File Offset: 0x00023DB3
	public static int ActiveLeaseCount
	{
		get
		{
			HeavyZoneAddressableResidency.Store store = HeavyZoneAddressableResidency._store;
			if (store == null)
			{
				return 0;
			}
			return store.ActiveLeaseCount;
		}
	}

	// Token: 0x17000114 RID: 276
	// (get) Token: 0x060004C8 RID: 1224 RVA: 0x00025BC5 File Offset: 0x00023DC5
	public static int DeferredReleaseCount
	{
		get
		{
			HeavyZoneAddressableResidency.Store store = HeavyZoneAddressableResidency._store;
			if (store == null)
			{
				return 0;
			}
			return store.DeferredReleaseCount;
		}
	}

	// Token: 0x17000115 RID: 277
	// (get) Token: 0x060004C9 RID: 1225 RVA: 0x00025BD7 File Offset: 0x00023DD7
	public static int CompletedUnusedEntryCount
	{
		get
		{
			HeavyZoneAddressableResidency.Store store = HeavyZoneAddressableResidency._store;
			if (store == null)
			{
				return 0;
			}
			return store.CompletedUnusedEntryCount;
		}
	}

	// Token: 0x17000116 RID: 278
	// (get) Token: 0x060004CA RID: 1226 RVA: 0x00025BE9 File Offset: 0x00023DE9
	public static int LoadStartCount
	{
		get
		{
			HeavyZoneAddressableResidency.Store store = HeavyZoneAddressableResidency._store;
			if (store == null)
			{
				return 0;
			}
			return store.LoadStartCount;
		}
	}

	// Token: 0x17000117 RID: 279
	// (get) Token: 0x060004CB RID: 1227 RVA: 0x00025BFB File Offset: 0x00023DFB
	public static int OperationReleaseCount
	{
		get
		{
			HeavyZoneAddressableResidency.Store store = HeavyZoneAddressableResidency._store;
			if (store == null)
			{
				return 0;
			}
			return store.OperationReleaseCount;
		}
	}

	// Token: 0x17000118 RID: 280
	// (get) Token: 0x060004CC RID: 1228 RVA: 0x00025C0D File Offset: 0x00023E0D
	public static int FailedLoadCount
	{
		get
		{
			HeavyZoneAddressableResidency.Store store = HeavyZoneAddressableResidency._store;
			if (store == null)
			{
				return 0;
			}
			return store.FailedLoadCount;
		}
	}

	// Token: 0x17000119 RID: 281
	// (get) Token: 0x060004CD RID: 1229 RVA: 0x00025C1F File Offset: 0x00023E1F
	public static int ReleaseErrorCount
	{
		get
		{
			HeavyZoneAddressableResidency.Store store = HeavyZoneAddressableResidency._store;
			if (store == null)
			{
				return 0;
			}
			return store.ReleaseErrorCount;
		}
	}

	// Token: 0x1700011A RID: 282
	// (get) Token: 0x060004CE RID: 1230 RVA: 0x00025C31 File Offset: 0x00023E31
	public static string LastReleaseError
	{
		get
		{
			HeavyZoneAddressableResidency.Store store = HeavyZoneAddressableResidency._store;
			if (store == null)
			{
				return null;
			}
			return store.LastReleaseError;
		}
	}

	// Token: 0x1700011B RID: 283
	// (get) Token: 0x060004CF RID: 1231 RVA: 0x00025C43 File Offset: 0x00023E43
	public static double LastLoadSeconds
	{
		get
		{
			HeavyZoneAddressableResidency.Store store = HeavyZoneAddressableResidency._store;
			if (store == null)
			{
				return 0.0;
			}
			return store.LastLoadSeconds;
		}
	}

	// Token: 0x060004D0 RID: 1232 RVA: 0x00025C60 File Offset: 0x00023E60
	public static HeavyZoneAddressableResidency.AddressSnapshot ObserveAddress(string address)
	{
		if (string.IsNullOrWhiteSpace(address))
		{
			throw new ArgumentException("A payload address is required.", "address");
		}
		if (HeavyZoneAddressableResidency._store == null)
		{
			return default(HeavyZoneAddressableResidency.AddressSnapshot);
		}
		return HeavyZoneAddressableResidency._store.ObserveAddress(address);
	}

	// Token: 0x060004D1 RID: 1233 RVA: 0x00025CA1 File Offset: 0x00023EA1
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void BindRuntimeThread()
	{
		HeavyZoneAddressableResidency._runtimeThreadId = Thread.CurrentThread.ManagedThreadId;
		HeavyZoneAddressableResidency._quitting = false;
		HeavyZoneAddressableResidency._lastTickFrame = -1;
		HeavyZoneAddressableResidency.Store store = HeavyZoneAddressableResidency._store;
		if (store == null)
		{
			return;
		}
		store.ResumeRequests();
	}

	// Token: 0x060004D2 RID: 1234 RVA: 0x00025CCD File Offset: 0x00023ECD
	public static HeavyZoneAddressableResidency.Lease Acquire(string address)
	{
		return HeavyZoneAddressableResidency.EnsureRuntimeStore().Acquire(address);
	}

	// Token: 0x060004D3 RID: 1235 RVA: 0x00025CDA File Offset: 0x00023EDA
	public static bool Prefetch(string address)
	{
		return !string.IsNullOrWhiteSpace(address) && HeavyZoneAddressableResidency.EnsureRuntimeStore().Prefetch(address);
	}

	// Token: 0x060004D4 RID: 1236 RVA: 0x00025CF1 File Offset: 0x00023EF1
	public static void TrimUnused(bool force)
	{
		HeavyZoneAddressableResidency.Store store = HeavyZoneAddressableResidency._store;
		if (store == null)
		{
			return;
		}
		store.TrimUnused(force);
	}

	// Token: 0x060004D5 RID: 1237 RVA: 0x00025D03 File Offset: 0x00023F03
	public static void BeginShutdown()
	{
		HeavyZoneAddressableResidency._quitting = true;
		HeavyZoneAddressableResidency.Store store = HeavyZoneAddressableResidency._store;
		if (store == null)
		{
			return;
		}
		store.BeginShutdown();
	}

	// Token: 0x060004D6 RID: 1238 RVA: 0x00025D1A File Offset: 0x00023F1A
	public static void ReleaseAfterDestroyed(HeavyZoneAddressableResidency.Lease lease, IReadOnlyList<Object> guards)
	{
		if (lease == null || lease.IsDisposed)
		{
			return;
		}
		HeavyZoneAddressableResidency.Store owner = lease._owner;
		owner.ReleaseAfterDestroyed(lease, guards);
		if (owner == HeavyZoneAddressableResidency._store && Application.isPlaying && !HeavyZoneAddressableResidency._quitting)
		{
			HeavyZoneAddressableResidency.EnsureRunner();
		}
	}

	// Token: 0x060004D7 RID: 1239 RVA: 0x00025D50 File Offset: 0x00023F50
	public static void Tick()
	{
		if (HeavyZoneAddressableResidency._store == null || HeavyZoneAddressableResidency._lastTickFrame == Time.frameCount)
		{
			return;
		}
		HeavyZoneAddressableResidency._lastTickFrame = Time.frameCount;
		HeavyZoneAddressableResidency._store.Tick();
	}

	// Token: 0x060004D8 RID: 1240 RVA: 0x00025D7C File Offset: 0x00023F7C
	private static HeavyZoneAddressableResidency.Store EnsureRuntimeStore()
	{
		if (HeavyZoneAddressableResidency._quitting)
		{
			throw new InvalidOperationException("Cannot load payloads during application shutdown.");
		}
		if (!Application.isPlaying || Thread.CurrentThread.ManagedThreadId != HeavyZoneAddressableResidency._runtimeThreadId)
		{
			throw new InvalidOperationException("Runtime payload acquisition requires Unity's play-mode main thread.");
		}
		if (HeavyZoneAddressableResidency._store == null)
		{
			HeavyZoneAddressableResidency._store = new HeavyZoneAddressableResidency.Store(new HeavyZoneAddressableResidency.AddressablesBackend(), () => Time.realtimeSinceStartupAsDouble, 30.0);
		}
		HeavyZoneAddressableResidency.EnsureRunner();
		return HeavyZoneAddressableResidency._store;
	}

	// Token: 0x060004D9 RID: 1241 RVA: 0x00025E06 File Offset: 0x00024006
	private static void EnsureRunner()
	{
		if (HeavyZoneAddressableResidency._runner != null)
		{
			return;
		}
		GameObject gameObject = new GameObject("Heavy zone asset residency");
		Object.DontDestroyOnLoad(gameObject);
		HeavyZoneAddressableResidency._runner = gameObject.AddComponent<HeavyZoneAddressableResidencyRunner>();
	}

	// Token: 0x060004DA RID: 1242 RVA: 0x00025E30 File Offset: 0x00024030
	internal static void RunnerDestroyed(HeavyZoneAddressableResidencyRunner runner)
	{
		if (HeavyZoneAddressableResidency._runner == runner)
		{
			HeavyZoneAddressableResidency._runner = null;
		}
	}

	// Token: 0x0400039D RID: 925
	public const double DefaultCooldownSeconds = 30.0;

	// Token: 0x0400039E RID: 926
	private static HeavyZoneAddressableResidency.Store _store;

	// Token: 0x0400039F RID: 927
	private static HeavyZoneAddressableResidencyRunner _runner;

	// Token: 0x040003A0 RID: 928
	private static int _runtimeThreadId;

	// Token: 0x040003A1 RID: 929
	private static int _lastTickFrame = -1;

	// Token: 0x040003A2 RID: 930
	private static bool _quitting;

	// Token: 0x0200005F RID: 95
	public struct AddressSnapshot
	{
		// Token: 0x040003A3 RID: 931
		public int entries;

		// Token: 0x040003A4 RID: 932
		public int owners;

		// Token: 0x040003A5 RID: 933
		public int deferred;

		// Token: 0x040003A6 RID: 934
		public int releaseCount;

		// Token: 0x040003A7 RID: 935
		public int releaseErrors;
	}

	// Token: 0x02000060 RID: 96
	public interface ILoadOperation
	{
		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060004DC RID: 1244
		bool IsDone { get; }

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060004DD RID: 1245
		GameObject Asset { get; }

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060004DE RID: 1246
		string Error { get; }

		// Token: 0x060004DF RID: 1247
		void Release();
	}

	// Token: 0x02000061 RID: 97
	public interface ILoadBackend
	{
		// Token: 0x060004E0 RID: 1248
		HeavyZoneAddressableResidency.ILoadOperation BeginLoad(string address);
	}

	// Token: 0x02000062 RID: 98
	internal sealed class Entry
	{
		// Token: 0x040003A8 RID: 936
		public string address;

		// Token: 0x040003A9 RID: 937
		public HeavyZoneAddressableResidency.ILoadOperation operation;

		// Token: 0x040003AA RID: 938
		public GameObject asset;

		// Token: 0x040003AB RID: 939
		public string error;

		// Token: 0x040003AC RID: 940
		public int owners;

		// Token: 0x040003AD RID: 941
		public bool completed;

		// Token: 0x040003AE RID: 942
		public bool released;

		// Token: 0x040003AF RID: 943
		public bool releaseWhenCompleted;

		// Token: 0x040003B0 RID: 944
		public bool speculative;

		// Token: 0x040003B1 RID: 945
		public double startedAt;

		// Token: 0x040003B2 RID: 946
		public double unusedUntil = double.PositiveInfinity;

		// Token: 0x040003B3 RID: 947
		public double speculativeUntil;
	}

	// Token: 0x02000063 RID: 99
	public sealed class Lease : IDisposable
	{
		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060004E2 RID: 1250 RVA: 0x00025E5F File Offset: 0x0002405F
		public string Address { get; }

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060004E3 RID: 1251 RVA: 0x00025E67 File Offset: 0x00024067
		public bool IsDisposed
		{
			get
			{
				return this._owner == null;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060004E4 RID: 1252 RVA: 0x00025E72 File Offset: 0x00024072
		public bool IsDone
		{
			get
			{
				return this.IsDisposed || this._entry.completed;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060004E5 RID: 1253 RVA: 0x00025E89 File Offset: 0x00024089
		public GameObject Asset
		{
			get
			{
				if (!this.IsDisposed)
				{
					return this._entry.asset;
				}
				return null;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060004E6 RID: 1254 RVA: 0x00025EA0 File Offset: 0x000240A0
		public string Error
		{
			get
			{
				if (!this.IsDisposed)
				{
					return this._entry.error;
				}
				return "The payload lease has been disposed.";
			}
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00025EBB File Offset: 0x000240BB
		internal Lease(HeavyZoneAddressableResidency.Store owner, HeavyZoneAddressableResidency.Entry entry)
		{
			this._owner = owner;
			this._entry = entry;
			this.Address = entry.address;
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x00025EDD File Offset: 0x000240DD
		public HeavyZoneAddressableResidency.Lease Retain()
		{
			if (this.IsDisposed)
			{
				throw new ObjectDisposedException("Lease");
			}
			return this._owner.Retain(this);
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00025F00 File Offset: 0x00024100
		public void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}
			HeavyZoneAddressableResidency.Store owner = this._owner;
			HeavyZoneAddressableResidency.Entry entry = this._entry;
			owner.AssertThread();
			this.Invalidate();
			owner.ReleaseOwner(entry);
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00025F35 File Offset: 0x00024135
		internal void Invalidate()
		{
			this._owner = null;
			this._entry = null;
		}

		// Token: 0x040003B4 RID: 948
		internal HeavyZoneAddressableResidency.Store _owner;

		// Token: 0x040003B5 RID: 949
		internal HeavyZoneAddressableResidency.Entry _entry;
	}

	// Token: 0x02000064 RID: 100
	public sealed class Store
	{
		// Token: 0x060004EB RID: 1259 RVA: 0x00025F48 File Offset: 0x00024148
		public Store(HeavyZoneAddressableResidency.ILoadBackend backend, Func<double> clock, double cooldownSeconds = 30.0)
		{
			if (backend == null)
			{
				throw new ArgumentNullException("backend");
			}
			this._backend = backend;
			if (clock == null)
			{
				throw new ArgumentNullException("clock");
			}
			this._clock = clock;
			if (cooldownSeconds < 0.0 || double.IsNaN(cooldownSeconds) || double.IsInfinity(cooldownSeconds))
			{
				throw new ArgumentOutOfRangeException("cooldownSeconds");
			}
			this._cooldown = cooldownSeconds;
			this._threadId = Thread.CurrentThread.ManagedThreadId;
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060004EC RID: 1260 RVA: 0x00025FFB File Offset: 0x000241FB
		public int EntryCount
		{
			get
			{
				return this._entries.Count;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x00026008 File Offset: 0x00024208
		// (set) Token: 0x060004EE RID: 1262 RVA: 0x00026010 File Offset: 0x00024210
		public int ActiveLeaseCount { get; private set; }

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x00026019 File Offset: 0x00024219
		public int DeferredReleaseCount
		{
			get
			{
				return this._deferred.Count;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060004F0 RID: 1264 RVA: 0x00026026 File Offset: 0x00024226
		// (set) Token: 0x060004F1 RID: 1265 RVA: 0x0002602E File Offset: 0x0002422E
		public int LoadStartCount { get; private set; }

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060004F2 RID: 1266 RVA: 0x00026037 File Offset: 0x00024237
		// (set) Token: 0x060004F3 RID: 1267 RVA: 0x0002603F File Offset: 0x0002423F
		public int OperationReleaseCount { get; private set; }

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060004F4 RID: 1268 RVA: 0x00026048 File Offset: 0x00024248
		// (set) Token: 0x060004F5 RID: 1269 RVA: 0x00026050 File Offset: 0x00024250
		public int FailedLoadCount { get; private set; }

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060004F6 RID: 1270 RVA: 0x00026059 File Offset: 0x00024259
		// (set) Token: 0x060004F7 RID: 1271 RVA: 0x00026061 File Offset: 0x00024261
		public int ReleaseErrorCount { get; private set; }

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060004F8 RID: 1272 RVA: 0x0002606A File Offset: 0x0002426A
		// (set) Token: 0x060004F9 RID: 1273 RVA: 0x00026072 File Offset: 0x00024272
		public string LastReleaseError { get; private set; }

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060004FA RID: 1274 RVA: 0x0002607B File Offset: 0x0002427B
		// (set) Token: 0x060004FB RID: 1275 RVA: 0x00026083 File Offset: 0x00024283
		public double LastLoadSeconds { get; private set; }

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060004FC RID: 1276 RVA: 0x0002608C File Offset: 0x0002428C
		// (set) Token: 0x060004FD RID: 1277 RVA: 0x00026094 File Offset: 0x00024294
		public double TotalLoadSeconds { get; private set; }

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060004FE RID: 1278 RVA: 0x0002609D File Offset: 0x0002429D
		// (set) Token: 0x060004FF RID: 1279 RVA: 0x000260A5 File Offset: 0x000242A5
		public bool IsShuttingDown { get; private set; }

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000500 RID: 1280 RVA: 0x000260AE File Offset: 0x000242AE
		public int ActiveLoadCount
		{
			get
			{
				return this.CountEntries(false, false);
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x000260B8 File Offset: 0x000242B8
		public int CompletedUnusedEntryCount
		{
			get
			{
				return this.CountEntries(true, true);
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000502 RID: 1282 RVA: 0x000260C2 File Offset: 0x000242C2
		public int SpeculativeHoldCount
		{
			get
			{
				if (this._prefetch == null)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x000260D0 File Offset: 0x000242D0
		public HeavyZoneAddressableResidency.AddressSnapshot ObserveAddress(string address)
		{
			this.AssertThread();
			if (string.IsNullOrWhiteSpace(address))
			{
				throw new ArgumentException("A payload address is required.", "address");
			}
			HeavyZoneAddressableResidency.AddressSnapshot addressSnapshot;
			this._releaseHistory.TryGetValue(address, out addressSnapshot);
			HeavyZoneAddressableResidency.Entry entry;
			if (this._entries.TryGetValue(address, out entry))
			{
				addressSnapshot.entries = 1;
				addressSnapshot.owners = entry.owners;
			}
			using (List<HeavyZoneAddressableResidency.Store.DeferredRelease>.Enumerator enumerator = this._deferred.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (string.Equals(enumerator.Current.entry.address, address, StringComparison.Ordinal))
					{
						addressSnapshot.deferred++;
					}
				}
			}
			return addressSnapshot;
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00026190 File Offset: 0x00024390
		public HeavyZoneAddressableResidency.Lease Acquire(string address)
		{
			this.AssertThread();
			if (this.IsShuttingDown)
			{
				throw new InvalidOperationException("Payload residency is shutting down.");
			}
			if (string.IsNullOrWhiteSpace(address))
			{
				throw new ArgumentException("A payload address is required.", "address");
			}
			double num = this._clock();
			HeavyZoneAddressableResidency.Entry entry;
			if (!this._entries.TryGetValue(address, out entry))
			{
				entry = this.Start(address, num);
			}
			this.ClearSpeculation(entry);
			entry.releaseWhenCompleted = false;
			entry.unusedUntil = double.PositiveInfinity;
			entry.owners++;
			int activeLeaseCount = this.ActiveLeaseCount;
			this.ActiveLeaseCount = activeLeaseCount + 1;
			this.CompleteIfReady(entry, num);
			return new HeavyZoneAddressableResidency.Lease(this, entry);
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x00026240 File Offset: 0x00024440
		internal HeavyZoneAddressableResidency.Lease Retain(HeavyZoneAddressableResidency.Lease source)
		{
			this.AssertThread();
			if (source._owner != this || source._entry == null || source._entry.released)
			{
				throw new ObjectDisposedException("Lease");
			}
			if (!string.IsNullOrEmpty(source._entry.error))
			{
				throw new InvalidOperationException("A failed payload lease cannot be retained.");
			}
			HeavyZoneAddressableResidency.Entry entry = source._entry;
			entry.owners++;
			int activeLeaseCount = this.ActiveLeaseCount;
			this.ActiveLeaseCount = activeLeaseCount + 1;
			return new HeavyZoneAddressableResidency.Lease(this, entry);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x000262C8 File Offset: 0x000244C8
		public bool Prefetch(string address)
		{
			this.AssertThread();
			if (this.IsShuttingDown || string.IsNullOrWhiteSpace(address))
			{
				return false;
			}
			double num = this._clock();
			this.Sweep(num, false);
			HeavyZoneAddressableResidency.Entry entry;
			if (this._entries.TryGetValue(address, out entry) && entry.owners > 0)
			{
				return false;
			}
			if (this._prefetch != null && this._prefetch.address != address)
			{
				if (!this._prefetch.completed)
				{
					return false;
				}
				HeavyZoneAddressableResidency.Entry prefetch = this._prefetch;
				this.ClearSpeculation(prefetch);
				prefetch.releaseWhenCompleted = true;
				this.EvictIfUnused(prefetch, num);
			}
			foreach (HeavyZoneAddressableResidency.Entry entry2 in this._entries.Values)
			{
				if (!entry2.completed && entry2.owners == 0 && entry2.address != address)
				{
					return false;
				}
			}
			HeavyZoneAddressableResidency.Entry entry3;
			bool flag = !this._entries.TryGetValue(address, out entry3);
			if (flag)
			{
				entry3 = this.Start(address, num);
			}
			entry3.speculative = true;
			entry3.speculativeUntil = num + this._cooldown;
			entry3.releaseWhenCompleted = false;
			this._prefetch = entry3;
			this.CompleteIfReady(entry3, num);
			this.EvictIfUnused(entry3, num);
			return flag;
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x00026424 File Offset: 0x00024624
		public void ReleaseAfterDestroyed(HeavyZoneAddressableResidency.Lease lease, IReadOnlyList<Object> guards)
		{
			this.AssertThread();
			if (lease == null || lease.IsDisposed)
			{
				return;
			}
			if (lease._owner != this)
			{
				throw new ArgumentException("The lease belongs to a different store.", "lease");
			}
			Object[] array = new Object[(guards != null) ? guards.Count : 0];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = guards[i];
			}
			this._deferred.Add(new HeavyZoneAddressableResidency.Store.DeferredRelease
			{
				entry = lease._entry,
				guards = array,
				fence = this._tick + 1L
			});
			lease.Invalidate();
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x000264C0 File Offset: 0x000246C0
		public void Tick()
		{
			this.AssertThread();
			this._tick += 1L;
			for (int i = this._deferred.Count - 1; i >= 0; i--)
			{
				HeavyZoneAddressableResidency.Store.DeferredRelease deferredRelease = this._deferred[i];
				if (this._tick > deferredRelease.fence)
				{
					bool flag = false;
					for (int j = 0; j < deferredRelease.guards.Length; j++)
					{
						if (deferredRelease.guards[j] != null)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this._deferred.RemoveAt(i);
						this.ReleaseOwner(deferredRelease.entry);
					}
				}
			}
			this.Sweep(this._clock(), this.IsShuttingDown);
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x00026570 File Offset: 0x00024770
		public void TrimUnused(bool force)
		{
			this.AssertThread();
			this.Sweep(this._clock(), force);
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x0002658A File Offset: 0x0002478A
		public void BeginShutdown()
		{
			this.AssertThread();
			this.IsShuttingDown = true;
			this.TrimUnused(true);
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x000265A0 File Offset: 0x000247A0
		internal void ResumeRequests()
		{
			this.AssertThread();
			this.IsShuttingDown = false;
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x000265B0 File Offset: 0x000247B0
		private HeavyZoneAddressableResidency.Entry Start(string address, double now)
		{
			HeavyZoneAddressableResidency.Entry entry = new HeavyZoneAddressableResidency.Entry
			{
				address = address,
				startedAt = now
			};
			this._entries.Add(address, entry);
			int loadStartCount = this.LoadStartCount;
			this.LoadStartCount = loadStartCount + 1;
			try
			{
				entry.operation = this._backend.BeginLoad(address);
				if (entry.operation == null)
				{
					throw new InvalidOperationException("The payload backend returned no operation.");
				}
			}
			catch (Exception ex)
			{
				this.Complete(entry, null, ex.Message, now);
			}
			return entry;
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x00026638 File Offset: 0x00024838
		private void CompleteIfReady(HeavyZoneAddressableResidency.Entry entry, double now)
		{
			if (entry.completed || entry.released)
			{
				return;
			}
			try
			{
				if (entry.operation.IsDone)
				{
					GameObject asset = entry.operation.Asset;
					string text = entry.operation.Error;
					if (asset == null && string.IsNullOrEmpty(text))
					{
						text = "The payload operation completed without an asset.";
					}
					this.Complete(entry, asset, text, now);
				}
			}
			catch (Exception ex)
			{
				this.Complete(entry, null, ex.Message, now);
			}
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x000266C4 File Offset: 0x000248C4
		private void Complete(HeavyZoneAddressableResidency.Entry entry, GameObject asset, string error, double now)
		{
			if (entry.completed)
			{
				return;
			}
			entry.completed = true;
			entry.asset = asset;
			entry.error = error;
			this.LastLoadSeconds = Math.Max(0.0, now - entry.startedAt);
			this.TotalLoadSeconds += this.LastLoadSeconds;
			if (!string.IsNullOrEmpty(error))
			{
				int failedLoadCount = this.FailedLoadCount;
				this.FailedLoadCount = failedLoadCount + 1;
			}
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x00026738 File Offset: 0x00024938
		internal void ReleaseOwner(HeavyZoneAddressableResidency.Entry entry)
		{
			if (entry.released || entry.owners <= 0)
			{
				return;
			}
			entry.owners--;
			int activeLeaseCount = this.ActiveLeaseCount;
			this.ActiveLeaseCount = activeLeaseCount - 1;
			if (entry.owners != 0)
			{
				return;
			}
			double num = this._clock();
			entry.unusedUntil = num + this._cooldown;
			if (!entry.completed || this.IsShuttingDown)
			{
				entry.releaseWhenCompleted = true;
			}
			this.CompleteIfReady(entry, num);
			this.EvictIfUnused(entry, num);
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x000267C0 File Offset: 0x000249C0
		private void Sweep(double now, bool force)
		{
			this._sweep.Clear();
			this._sweep.AddRange(this._entries.Values);
			for (int i = 0; i < this._sweep.Count; i++)
			{
				HeavyZoneAddressableResidency.Entry entry = this._sweep[i];
				if (!entry.released)
				{
					if (entry.owners == 0 && (force || (entry.speculative && now >= entry.speculativeUntil)))
					{
						this.ClearSpeculation(entry);
						entry.releaseWhenCompleted = true;
					}
					this.CompleteIfReady(entry, now);
					this.EvictIfUnused(entry, now);
				}
			}
			this._sweep.Clear();
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x00026860 File Offset: 0x00024A60
		private void EvictIfUnused(HeavyZoneAddressableResidency.Entry entry, double now)
		{
			if (entry.released || !entry.completed || entry.owners != 0)
			{
				return;
			}
			if (string.IsNullOrEmpty(entry.error) && !entry.releaseWhenCompleted && (entry.speculative || now < entry.unusedUntil))
			{
				return;
			}
			this.ClearSpeculation(entry);
			entry.released = true;
			this._entries.Remove(entry.address);
			HeavyZoneAddressableResidency.ILoadOperation operation = entry.operation;
			entry.operation = null;
			entry.asset = null;
			if (operation == null)
			{
				return;
			}
			int num = this.OperationReleaseCount;
			this.OperationReleaseCount = num + 1;
			HeavyZoneAddressableResidency.AddressSnapshot addressSnapshot;
			this._releaseHistory.TryGetValue(entry.address, out addressSnapshot);
			addressSnapshot.releaseCount++;
			try
			{
				operation.Release();
			}
			catch (Exception ex)
			{
				num = this.ReleaseErrorCount;
				this.ReleaseErrorCount = num + 1;
				addressSnapshot.releaseErrors++;
				this.LastReleaseError = ex.ToString();
			}
			this._releaseHistory[entry.address] = addressSnapshot;
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00026970 File Offset: 0x00024B70
		private void ClearSpeculation(HeavyZoneAddressableResidency.Entry entry)
		{
			entry.speculative = false;
			if (this._prefetch == entry)
			{
				this._prefetch = null;
			}
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x0002698C File Offset: 0x00024B8C
		private int CountEntries(bool completed, bool unused)
		{
			int num = 0;
			foreach (HeavyZoneAddressableResidency.Entry entry in this._entries.Values)
			{
				if (entry.completed == completed && (!unused || entry.owners == 0))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x000269F8 File Offset: 0x00024BF8
		internal void AssertThread()
		{
			if (Thread.CurrentThread.ManagedThreadId != this._threadId)
			{
				throw new InvalidOperationException("Payload ownership must be accessed on its creating Unity thread.");
			}
		}

		// Token: 0x040003B7 RID: 951
		private readonly HeavyZoneAddressableResidency.ILoadBackend _backend;

		// Token: 0x040003B8 RID: 952
		private readonly Func<double> _clock;

		// Token: 0x040003B9 RID: 953
		private readonly double _cooldown;

		// Token: 0x040003BA RID: 954
		private readonly int _threadId;

		// Token: 0x040003BB RID: 955
		private readonly Dictionary<string, HeavyZoneAddressableResidency.Entry> _entries = new Dictionary<string, HeavyZoneAddressableResidency.Entry>(StringComparer.Ordinal);

		// Token: 0x040003BC RID: 956
		private readonly Dictionary<string, HeavyZoneAddressableResidency.AddressSnapshot> _releaseHistory = new Dictionary<string, HeavyZoneAddressableResidency.AddressSnapshot>(StringComparer.Ordinal);

		// Token: 0x040003BD RID: 957
		private readonly List<HeavyZoneAddressableResidency.Entry> _sweep = new List<HeavyZoneAddressableResidency.Entry>();

		// Token: 0x040003BE RID: 958
		private readonly List<HeavyZoneAddressableResidency.Store.DeferredRelease> _deferred = new List<HeavyZoneAddressableResidency.Store.DeferredRelease>();

		// Token: 0x040003BF RID: 959
		private HeavyZoneAddressableResidency.Entry _prefetch;

		// Token: 0x040003C0 RID: 960
		private long _tick;

		// Token: 0x02000065 RID: 101
		private sealed class DeferredRelease
		{
			// Token: 0x040003CA RID: 970
			public HeavyZoneAddressableResidency.Entry entry;

			// Token: 0x040003CB RID: 971
			public Object[] guards;

			// Token: 0x040003CC RID: 972
			public long fence;
		}
	}

	// Token: 0x02000066 RID: 102
	private sealed class AddressablesBackend : HeavyZoneAddressableResidency.ILoadBackend
	{
		// Token: 0x06000516 RID: 1302 RVA: 0x00026A17 File Offset: 0x00024C17
		public HeavyZoneAddressableResidency.ILoadOperation BeginLoad(string address)
		{
			return new HeavyZoneAddressableResidency.AddressablesOperation(address);
		}
	}

	// Token: 0x02000067 RID: 103
	private sealed class AddressablesOperation : HeavyZoneAddressableResidency.ILoadOperation
	{
		// Token: 0x06000518 RID: 1304 RVA: 0x00026A20 File Offset: 0x00024C20
		public AddressablesOperation(string address)
		{
			HeavyZoneStreamingProxy.BeginSharedPayloadLoad();
			this._priorityHeld = true;
			try
			{
				this._handle = Addressables.LoadAssetAsync<GameObject>(address);
			}
			catch
			{
				this.EndPriorityHold();
				throw;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000519 RID: 1305 RVA: 0x00026A68 File Offset: 0x00024C68
		public bool IsDone
		{
			get
			{
				bool flag = !this._handle.IsValid() || this._handle.IsDone;
				if (flag)
				{
					this.EndPriorityHold();
				}
				return flag;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x00026A8E File Offset: 0x00024C8E
		public GameObject Asset
		{
			get
			{
				if (!this._handle.IsValid() || this._handle.Status != AsyncOperationStatus.Succeeded)
				{
					return null;
				}
				return this._handle.Result;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x00026AB8 File Offset: 0x00024CB8
		public string Error
		{
			get
			{
				string text;
				if (this._handle.IsValid())
				{
					if (this._handle.Status != AsyncOperationStatus.Failed)
					{
						return null;
					}
					Exception operationException = this._handle.OperationException;
					if ((text = ((operationException != null) ? operationException.Message : null)) == null)
					{
						return "Addressables payload load failed.";
					}
				}
				else
				{
					text = "The Addressables payload handle is invalid.";
				}
				return text;
			}
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00026B08 File Offset: 0x00024D08
		public void Release()
		{
			if (this._released)
			{
				return;
			}
			this._released = true;
			this.EndPriorityHold();
			if (this._handle.IsValid())
			{
				Addressables.Release<GameObject>(this._handle);
			}
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x00026B38 File Offset: 0x00024D38
		private void EndPriorityHold()
		{
			if (!this._priorityHeld)
			{
				return;
			}
			this._priorityHeld = false;
			HeavyZoneStreamingProxy.EndSharedPayloadLoad();
		}

		// Token: 0x040003CD RID: 973
		private AsyncOperationHandle<GameObject> _handle;

		// Token: 0x040003CE RID: 974
		private bool _priorityHeld;

		// Token: 0x040003CF RID: 975
		private bool _released;
	}
}
