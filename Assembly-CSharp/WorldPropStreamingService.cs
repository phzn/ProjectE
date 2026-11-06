using System;
using System.Collections.Generic;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using Unity.Profiling;
using UnityEngine;

// Token: 0x020000AD RID: 173
public sealed class WorldPropStreamingService : MonoBehaviour
{
	// Token: 0x17000196 RID: 406
	// (get) Token: 0x060006DD RID: 1757 RVA: 0x0003126D File Offset: 0x0002F46D
	public int RegisteredSlots
	{
		get
		{
			return this.slots.Count;
		}
	}

	// Token: 0x17000197 RID: 407
	// (get) Token: 0x060006DE RID: 1758 RVA: 0x0003127A File Offset: 0x0002F47A
	public int PendingViews
	{
		get
		{
			return this.pending.Count;
		}
	}

	// Token: 0x17000198 RID: 408
	// (get) Token: 0x060006DF RID: 1759 RVA: 0x00031287 File Offset: 0x0002F487
	// (set) Token: 0x060006E0 RID: 1760 RVA: 0x0003128F File Offset: 0x0002F48F
	public int IdleViews { get; private set; }

	// Token: 0x17000199 RID: 409
	// (get) Token: 0x060006E1 RID: 1761 RVA: 0x00031298 File Offset: 0x0002F498
	// (set) Token: 0x060006E2 RID: 1762 RVA: 0x000312A0 File Offset: 0x0002F4A0
	public int ColdCreates { get; private set; }

	// Token: 0x1700019A RID: 410
	// (get) Token: 0x060006E3 RID: 1763 RVA: 0x000312A9 File Offset: 0x0002F4A9
	// (set) Token: 0x060006E4 RID: 1764 RVA: 0x000312B1 File Offset: 0x0002F4B1
	public int OperationsLastFrame { get; private set; }

	// Token: 0x1700019B RID: 411
	// (get) Token: 0x060006E5 RID: 1765 RVA: 0x000312BA File Offset: 0x0002F4BA
	// (set) Token: 0x060006E6 RID: 1766 RVA: 0x000312C2 File Offset: 0x0002F4C2
	public int PeakOperations { get; private set; }

	// Token: 0x1700019C RID: 412
	// (get) Token: 0x060006E7 RID: 1767 RVA: 0x000312CB File Offset: 0x0002F4CB
	// (set) Token: 0x060006E8 RID: 1768 RVA: 0x000312D3 File Offset: 0x0002F4D3
	public float MaximumTickMs { get; private set; }

	// Token: 0x1700019D RID: 413
	// (get) Token: 0x060006E9 RID: 1769 RVA: 0x000312DC File Offset: 0x0002F4DC
	public static WorldPropStreamingService Instance
	{
		get
		{
			return WorldPropStreamingService.instance;
		}
	}

	// Token: 0x060006EA RID: 1770 RVA: 0x000312E3 File Offset: 0x0002F4E3
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void ResetStatics()
	{
		WorldPropStreamingService.instance = null;
	}

	// Token: 0x060006EB RID: 1771 RVA: 0x000312EC File Offset: 0x0002F4EC
	internal static void Register(StreamedWorldPropSlot slot)
	{
		if (WorldPropStreamingService.instance == null)
		{
			WorldPropStreamingService.instance = new GameObject("WorldPropStreamingService").AddComponent<WorldPropStreamingService>();
		}
		if (slot.RegistryIndex >= 0)
		{
			return;
		}
		slot.RegistryIndex = WorldPropStreamingService.instance.slots.Count;
		WorldPropStreamingService.instance.slots.Add(slot);
	}

	// Token: 0x060006EC RID: 1772 RVA: 0x0003134C File Offset: 0x0002F54C
	internal static void Unregister(StreamedWorldPropSlot slot)
	{
		if (WorldPropStreamingService.instance == null || slot.RegistryIndex < 0)
		{
			slot.RegistryIndex = -1;
			return;
		}
		int registryIndex = slot.RegistryIndex;
		int num = WorldPropStreamingService.instance.slots.Count - 1;
		if (registryIndex <= num)
		{
			WorldPropStreamingService.instance.slots[registryIndex] = WorldPropStreamingService.instance.slots[num];
			WorldPropStreamingService.instance.slots[registryIndex].RegistryIndex = registryIndex;
			WorldPropStreamingService.instance.slots.RemoveAt(num);
		}
		slot.RegistryIndex = -1;
		StreamedWorldPropView view = slot.View;
		WorldPropStreamingService.instance.QueueRetirement(view);
		if (view != null)
		{
			view.ForgetOwner();
			view.gameObject.SetActive(false);
		}
	}

	// Token: 0x060006ED RID: 1773 RVA: 0x00031410 File Offset: 0x0002F610
	private void Awake()
	{
		if (WorldPropStreamingService.instance != null && WorldPropStreamingService.instance != this)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		WorldPropStreamingService.instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
		GameObject gameObject = new GameObject("Inactive world prop views");
		gameObject.transform.SetParent(base.transform, false);
		gameObject.SetActive(false);
		this.poolRoot = gameObject.transform;
	}

	// Token: 0x060006EE RID: 1774 RVA: 0x00031484 File Offset: 0x0002F684
	private void OnDestroy()
	{
		if (WorldPropStreamingService.instance != this)
		{
			return;
		}
		foreach (StreamedWorldPropSlot streamedWorldPropSlot in this.slots)
		{
			if (streamedWorldPropSlot != null)
			{
				streamedWorldPropSlot.RegistryIndex = -1;
			}
		}
		WorldPropStreamingService.instance = null;
	}

	// Token: 0x060006EF RID: 1775 RVA: 0x000314F4 File Offset: 0x0002F6F4
	private void QueueRetirement(StreamedWorldPropView view)
	{
		if (view == null || view.RetirementQueued)
		{
			return;
		}
		view.RetirementQueued = true;
		this.retiring.Enqueue(view);
	}

	// Token: 0x060006F0 RID: 1776 RVA: 0x0003151C File Offset: 0x0002F71C
	public static bool ShouldExist(float playerDistance, float cameraDistance, float forwardDot, bool resident, float near, float visible, float margin)
	{
		float num = (resident ? margin : 0f);
		return playerDistance <= near + num || (cameraDistance <= visible + num && forwardDot >= 0f);
	}

	// Token: 0x060006F1 RID: 1777 RVA: 0x00031554 File Offset: 0x0002F754
	private bool Relevant(StreamedWorldPropSlot slot, Vector3 player, Vector3 camera, Vector3 forward, bool resident)
	{
		Bounds worldBounds = slot.WorldBounds;
		Vector3 vector = worldBounds.center - camera;
		return WorldPropStreamingService.ShouldExist(Mathf.Sqrt(worldBounds.SqrDistance(player)), Mathf.Sqrt(worldBounds.SqrDistance(camera)), Vector3.Dot(vector, forward), resident, this.nearbyRadius, this.visibleRadius, this.releaseMargin);
	}

	// Token: 0x060006F2 RID: 1778 RVA: 0x000315B4 File Offset: 0x0002F7B4
	private void Update()
	{
		using (WorldPropStreamingService.TickMarker.Auto())
		{
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			this.OperationsLastFrame = 0;
			Transform player = PlayerRegistry.Player;
			if (this.viewCamera == null || !this.viewCamera.isActiveAndEnabled)
			{
				this.viewCamera = Camera.main;
			}
			bool flag = player != null && this.viewCamera != null;
			Vector3 vector = (flag ? player.position : Vector3.zero);
			Vector3 vector2 = (flag ? this.viewCamera.transform.position : Vector3.zero);
			Vector3 vector3 = (flag ? this.viewCamera.transform.forward : Vector3.forward);
			bool flag2 = PlayerActivityGate.Allows(PlayerActivity.Interact);
			if (flag)
			{
				int num = Mathf.Min(this.checksPerFrame, this.slots.Count);
				for (int i = 0; i < num; i++)
				{
					if (this.cursor >= this.slots.Count)
					{
						this.cursor = 0;
					}
					List<StreamedWorldPropSlot> list = this.slots;
					int num2 = this.cursor;
					this.cursor = num2 + 1;
					StreamedWorldPropSlot streamedWorldPropSlot = list[num2];
					if (!(streamedWorldPropSlot == null) && streamedWorldPropSlot.isActiveAndEnabled && !streamedWorldPropSlot.Consumed)
					{
						StreamedWorldPropView view = streamedWorldPropSlot.View;
						if (view != null && view.IsConsumed)
						{
							streamedWorldPropSlot.Consumed = true;
							this.QueueRetirement(view);
						}
						else if (this.Relevant(streamedWorldPropSlot, vector, vector2, vector3, view != null && view.gameObject.activeSelf))
						{
							streamedWorldPropSlot.LastRelevantTime = Time.unscaledTime;
							if ((view == null || !view.gameObject.activeSelf) && !streamedWorldPropSlot.Queued)
							{
								streamedWorldPropSlot.Queued = true;
								this.pending.Add(streamedWorldPropSlot);
							}
						}
						else if (view != null && view.gameObject.activeSelf && flag2 && !view.IsBusy && Time.unscaledTime - streamedWorldPropSlot.LastRelevantTime >= this.releaseDelay)
						{
							this.QueueRetirement(view);
						}
					}
				}
			}
			while (this.OperationsLastFrame < this.operationsPerFrame && (Time.realtimeSinceStartup - realtimeSinceStartup) * 1000f < this.frameBudgetMs)
			{
				if (this.retiring.Count > 0)
				{
					StreamedWorldPropView streamedWorldPropView = this.retiring.Dequeue();
					if (!(streamedWorldPropView == null))
					{
						streamedWorldPropView.RetirementQueued = false;
						StreamedWorldPropSlot owner = streamedWorldPropView.Owner;
						bool flag3 = owner != null && owner.isActiveAndEnabled && streamedWorldPropView.BelongsToCurrentPlacement;
						if (!flag3 || owner.Consumed || (flag2 && !streamedWorldPropView.IsBusy && (!flag || !this.Relevant(owner, vector, vector2, vector3, true))))
						{
							using (WorldPropStreamingService.SleepMarker.Auto())
							{
								this.Sleep(streamedWorldPropView, flag3 && !owner.Consumed);
							}
							int num2 = this.OperationsLastFrame;
							this.OperationsLastFrame = num2 + 1;
						}
					}
				}
				else
				{
					if (!flag || this.pending.Count == 0)
					{
						break;
					}
					for (int j = this.pending.Count - 1; j >= 0; j--)
					{
						StreamedWorldPropSlot streamedWorldPropSlot2 = this.pending[j];
						if (streamedWorldPropSlot2 == null || !streamedWorldPropSlot2.isActiveAndEnabled || streamedWorldPropSlot2.Consumed || !this.Relevant(streamedWorldPropSlot2, vector, vector2, vector3, false))
						{
							if (streamedWorldPropSlot2 != null)
							{
								streamedWorldPropSlot2.Queued = false;
							}
							this.pending.RemoveAt(j);
						}
					}
					int num3 = -1;
					float num4 = float.PositiveInfinity;
					for (int k = 0; k < this.pending.Count; k++)
					{
						float num5 = this.pending[k].WorldBounds.SqrDistance(vector);
						if (num5 < num4)
						{
							num4 = num5;
							num3 = k;
						}
					}
					if (num3 < 0)
					{
						break;
					}
					StreamedWorldPropSlot streamedWorldPropSlot3 = this.pending[num3];
					this.pending.RemoveAt(num3);
					streamedWorldPropSlot3.Queued = false;
					if (!(streamedWorldPropSlot3.View != null) || !streamedWorldPropSlot3.View.gameObject.activeSelf)
					{
						using (WorldPropStreamingService.WakeMarker.Auto())
						{
							this.Wake(streamedWorldPropSlot3);
						}
						int num2 = this.OperationsLastFrame;
						this.OperationsLastFrame = num2 + 1;
					}
				}
			}
			this.PeakOperations = Mathf.Max(this.PeakOperations, this.OperationsLastFrame);
			this.MaximumTickMs = Mathf.Max(this.MaximumTickMs, (Time.realtimeSinceStartup - realtimeSinceStartup) * 1000f);
		}
	}

	// Token: 0x060006F3 RID: 1779 RVA: 0x00031AF4 File Offset: 0x0002FCF4
	private void Wake(StreamedWorldPropSlot slot)
	{
		StreamedWorldPropView streamedWorldPropView = slot.View;
		if (streamedWorldPropView != null)
		{
			streamedWorldPropView.Wake(slot);
			return;
		}
		GameObject viewPrefab = slot.ViewPrefab;
		if (viewPrefab == null)
		{
			return;
		}
		HeavyZoneStreamingProxy componentInParent = slot.GetComponentInParent<HeavyZoneStreamingProxy>();
		bool flag = componentInParent != null && componentInParent.HasCanonicalGameplayReferences && slot.UsesCanonicalGameplayView;
		HeavyZoneAddressableResidency.Lease lease = ((componentInParent != null && !flag) ? componentInParent.RetainAddressablePayload() : null);
		GameObject gameObject = null;
		try
		{
			Stack<StreamedWorldPropView> stack;
			if (lease == null && this.pools.TryGetValue(viewPrefab, out stack))
			{
				while (stack.Count > 0 && streamedWorldPropView == null)
				{
					streamedWorldPropView = stack.Pop();
					int num = this.IdleViews;
					this.IdleViews = num - 1;
				}
			}
			if (streamedWorldPropView == null)
			{
				gameObject = Object.Instantiate<GameObject>(viewPrefab, this.poolRoot);
				gameObject.SetActive(false);
				streamedWorldPropView = gameObject.GetComponent<StreamedWorldPropView>();
				if (streamedWorldPropView == null)
				{
					streamedWorldPropView = gameObject.AddComponent<StreamedWorldPropView>();
				}
				streamedWorldPropView.BindAddressablePayload(lease);
				streamedWorldPropView.Template = viewPrefab;
				streamedWorldPropView.Initialize();
				int num = this.ColdCreates;
				this.ColdCreates = num + 1;
			}
			streamedWorldPropView.Wake(slot);
		}
		catch
		{
			if (lease != null)
			{
				if (gameObject == null)
				{
					lease.Dispose();
				}
				else
				{
					HeavyZoneAddressableResidency.ReleaseAfterDestroyed(lease, new Object[] { gameObject });
				}
			}
			if (gameObject != null)
			{
				WorldPropStreamingService.DestroyView(gameObject);
			}
			throw;
		}
	}

	// Token: 0x060006F4 RID: 1780 RVA: 0x00031C58 File Offset: 0x0002FE58
	private void Sleep(StreamedWorldPropView view, bool samePlacement)
	{
		view.gameObject.SetActive(false);
		if (samePlacement && view.HasPersistentState)
		{
			return;
		}
		bool flag = view.IsConsumed || view.HasMissingScopedPickups;
		view.ForgetOwner();
		view.transform.SetParent(this.poolRoot, false);
		if (view.HasAddressablePayloadLease || flag || this.IdleViews >= this.maximumIdleViews || view.Template == null)
		{
			WorldPropStreamingService.DestroyView(view.gameObject);
			return;
		}
		Stack<StreamedWorldPropView> stack;
		if (!this.pools.TryGetValue(view.Template, out stack))
		{
			stack = new Stack<StreamedWorldPropView>(this.maximumIdlePerPrefab);
			this.pools.Add(view.Template, stack);
		}
		if (stack.Count >= this.maximumIdlePerPrefab)
		{
			WorldPropStreamingService.DestroyView(view.gameObject);
			return;
		}
		stack.Push(view);
		int idleViews = this.IdleViews;
		this.IdleViews = idleViews + 1;
	}

	// Token: 0x060006F5 RID: 1781 RVA: 0x00031D40 File Offset: 0x0002FF40
	private static void DestroyView(GameObject view)
	{
		StreamedWorldPropView component = view.GetComponent<StreamedWorldPropView>();
		if (component != null)
		{
			component.ReleaseAddressablePayloadAfterDestruction();
		}
		Object.Destroy(view);
	}

	// Token: 0x040005AC RID: 1452
	[SerializeField]
	[Min(1f)]
	private float nearbyRadius = 35f;

	// Token: 0x040005AD RID: 1453
	[SerializeField]
	[Min(1f)]
	private float visibleRadius = 60f;

	// Token: 0x040005AE RID: 1454
	[SerializeField]
	[Min(0f)]
	private float releaseMargin = 10f;

	// Token: 0x040005AF RID: 1455
	[SerializeField]
	[Min(0f)]
	private float releaseDelay = 2f;

	// Token: 0x040005B0 RID: 1456
	[SerializeField]
	[Min(1f)]
	private int checksPerFrame = 128;

	// Token: 0x040005B1 RID: 1457
	[SerializeField]
	[Min(1f)]
	private int operationsPerFrame = 2;

	// Token: 0x040005B2 RID: 1458
	[SerializeField]
	[Min(0.1f)]
	private float frameBudgetMs = 1f;

	// Token: 0x040005B3 RID: 1459
	[SerializeField]
	[Min(0f)]
	private int maximumIdleViews = 128;

	// Token: 0x040005B4 RID: 1460
	[SerializeField]
	[Min(0f)]
	private int maximumIdlePerPrefab = 2;

	// Token: 0x040005B5 RID: 1461
	private static readonly ProfilerMarker TickMarker = new ProfilerMarker("WorldProps.Streaming");

	// Token: 0x040005B6 RID: 1462
	private static readonly ProfilerMarker WakeMarker = new ProfilerMarker("WorldProps.Wake");

	// Token: 0x040005B7 RID: 1463
	private static readonly ProfilerMarker SleepMarker = new ProfilerMarker("WorldProps.Sleep");

	// Token: 0x040005B8 RID: 1464
	private static WorldPropStreamingService instance;

	// Token: 0x040005B9 RID: 1465
	private readonly List<StreamedWorldPropSlot> slots = new List<StreamedWorldPropSlot>(4096);

	// Token: 0x040005BA RID: 1466
	private readonly List<StreamedWorldPropSlot> pending = new List<StreamedWorldPropSlot>(256);

	// Token: 0x040005BB RID: 1467
	private readonly Queue<StreamedWorldPropView> retiring = new Queue<StreamedWorldPropView>(256);

	// Token: 0x040005BC RID: 1468
	private readonly Dictionary<GameObject, Stack<StreamedWorldPropView>> pools = new Dictionary<GameObject, Stack<StreamedWorldPropView>>();

	// Token: 0x040005BD RID: 1469
	private Transform poolRoot;

	// Token: 0x040005BE RID: 1470
	private Camera viewCamera;

	// Token: 0x040005BF RID: 1471
	private int cursor;
}
