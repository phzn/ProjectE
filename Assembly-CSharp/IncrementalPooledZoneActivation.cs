using System;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

// Token: 0x02000077 RID: 119
[DisallowMultipleComponent]
public sealed class IncrementalPooledZoneActivation : MonoBehaviour
{
	// Token: 0x1400000F RID: 15
	// (add) Token: 0x060005B8 RID: 1464 RVA: 0x00029FE4 File Offset: 0x000281E4
	// (remove) Token: 0x060005B9 RID: 1465 RVA: 0x0002A01C File Offset: 0x0002821C
	public event Action<IncrementalPooledZoneActivation> OperationalReady;

	// Token: 0x17000157 RID: 343
	// (get) Token: 0x060005BA RID: 1466 RVA: 0x0002A051 File Offset: 0x00028251
	public bool HasSnapshot
	{
		get
		{
			return this._snapshotReady;
		}
	}

	// Token: 0x17000158 RID: 344
	// (get) Token: 0x060005BB RID: 1467 RVA: 0x0002A059 File Offset: 0x00028259
	public bool IsDormant
	{
		get
		{
			return this._dormant;
		}
	}

	// Token: 0x17000159 RID: 345
	// (get) Token: 0x060005BC RID: 1468 RVA: 0x0002A061 File Offset: 0x00028261
	// (set) Token: 0x060005BD RID: 1469 RVA: 0x0002A069 File Offset: 0x00028269
	public bool IsOperationalReady { get; private set; }

	// Token: 0x1700015A RID: 346
	// (get) Token: 0x060005BE RID: 1470 RVA: 0x0002A072 File Offset: 0x00028272
	public int StagedTransformCount
	{
		get
		{
			return this._stagedObjects.Count;
		}
	}

	// Token: 0x1700015B RID: 347
	// (get) Token: 0x060005BF RID: 1471 RVA: 0x0002A07F File Offset: 0x0002827F
	public int StagedWakeWorkRemaining
	{
		get
		{
			return Mathf.Max(0, this._stagedObjects.Count - this._wakeIndex);
		}
	}

	// Token: 0x1700015C RID: 348
	// (get) Token: 0x060005C0 RID: 1472 RVA: 0x0002A099 File Offset: 0x00028299
	public int StagedSleepWorkRemaining
	{
		get
		{
			return Mathf.Max(0, this._sleepIndex + 1);
		}
	}

	// Token: 0x060005C1 RID: 1473 RVA: 0x0002A0AC File Offset: 0x000282AC
	public void CaptureAuthoredHierarchy()
	{
		if (this._snapshotReady)
		{
			return;
		}
		this._stagedObjects.Clear();
		this._activeDefaults.Clear();
		Transform transform = base.transform;
		for (int i = 0; i < transform.childCount; i++)
		{
			this._stagedObjects.Add(transform.GetChild(i).gameObject);
		}
		for (int j = 0; j < this._stagedObjects.Count; j++)
		{
			GameObject gameObject = this._stagedObjects[j];
			if (gameObject == null)
			{
				this._activeDefaults.Add(false);
			}
			else
			{
				this._activeDefaults.Add(gameObject.activeSelf);
				Transform transform2 = gameObject.transform;
				for (int k = 0; k < transform2.childCount; k++)
				{
					this._stagedObjects.Add(transform2.GetChild(k).gameObject);
				}
			}
		}
		this._snapshotReady = true;
		this._dormant = false;
		this._wakeIndex = this._stagedObjects.Count;
		this.IsOperationalReady = true;
	}

	// Token: 0x060005C2 RID: 1474 RVA: 0x0002A1B0 File Offset: 0x000283B0
	public bool PrepareDormancyWhileRootInactive()
	{
		if (!this._snapshotReady || base.gameObject.activeInHierarchy)
		{
			return false;
		}
		for (int i = 0; i < this._stagedObjects.Count; i++)
		{
			GameObject gameObject = this._stagedObjects[i];
			if (!(gameObject == null))
			{
				this._activeDefaults[i] = gameObject.activeSelf;
				if (gameObject.activeSelf)
				{
					gameObject.SetActive(false);
				}
			}
		}
		this._wakeIndex = 0;
		this._sleepIndex = -1;
		this._dormant = true;
		this._wakeArmed = false;
		this.IsOperationalReady = false;
		return true;
	}

	// Token: 0x060005C3 RID: 1475 RVA: 0x0002A248 File Offset: 0x00028448
	public bool RestoreSnapshotWhileRootInactive()
	{
		if (!this._snapshotReady || base.gameObject.activeInHierarchy)
		{
			return false;
		}
		for (int i = 0; i < this._stagedObjects.Count; i++)
		{
			GameObject gameObject = this._stagedObjects[i];
			if (gameObject != null && gameObject.activeSelf != this._activeDefaults[i])
			{
				gameObject.SetActive(this._activeDefaults[i]);
			}
		}
		this._wakeIndex = this._stagedObjects.Count;
		this._sleepIndex = -1;
		this._dormant = false;
		this._wakeArmed = false;
		this.IsOperationalReady = true;
		return true;
	}

	// Token: 0x060005C4 RID: 1476 RVA: 0x0002A2EC File Offset: 0x000284EC
	public void ArmStagedWake()
	{
		if (!this._snapshotReady)
		{
			return;
		}
		this._wakeArmed = this._dormant;
		this.IsOperationalReady = !this._dormant;
	}

	// Token: 0x060005C5 RID: 1477 RVA: 0x0002A314 File Offset: 0x00028514
	public bool ProcessOneStagedWakeStep()
	{
		if (!this._snapshotReady || !this._dormant || !this._wakeArmed)
		{
			return false;
		}
		if (!base.gameObject.activeInHierarchy)
		{
			return true;
		}
		if (this._wakeIndex < this._stagedObjects.Count)
		{
			using (IncrementalPooledZoneActivation.StagedWakeMarker.Auto())
			{
				int wakeIndex = this._wakeIndex;
				this._wakeIndex = wakeIndex + 1;
				int num = wakeIndex;
				GameObject gameObject = this._stagedObjects[num];
				if (gameObject != null && gameObject.activeSelf != this._activeDefaults[num])
				{
					gameObject.SetActive(this._activeDefaults[num]);
				}
			}
		}
		if (this._wakeIndex < this._stagedObjects.Count)
		{
			return true;
		}
		this._wakeArmed = false;
		this._dormant = false;
		this._sleepIndex = -1;
		this.IsOperationalReady = true;
		Action<IncrementalPooledZoneActivation> operationalReady = this.OperationalReady;
		if (operationalReady != null)
		{
			operationalReady(this);
		}
		return false;
	}

	// Token: 0x060005C6 RID: 1478 RVA: 0x0002A420 File Offset: 0x00028620
	public void BeginStagedSleep()
	{
		if (!this._snapshotReady)
		{
			return;
		}
		this._wakeArmed = false;
		this.IsOperationalReady = false;
		this._sleepIndex = (this._dormant ? Mathf.Min(this._wakeIndex - 1, this._stagedObjects.Count - 1) : (this._stagedObjects.Count - 1));
	}

	// Token: 0x060005C7 RID: 1479 RVA: 0x0002A47C File Offset: 0x0002867C
	public bool ProcessOneStagedSleepStep()
	{
		if (this._sleepIndex < 0)
		{
			this.FinishSleep();
			return false;
		}
		using (IncrementalPooledZoneActivation.StagedSleepMarker.Auto())
		{
			int sleepIndex = this._sleepIndex;
			this._sleepIndex = sleepIndex - 1;
			int num = sleepIndex;
			GameObject gameObject = this._stagedObjects[num];
			if (gameObject != null)
			{
				this._activeDefaults[num] = gameObject.activeSelf;
				if (gameObject.activeSelf)
				{
					gameObject.SetActive(false);
				}
			}
		}
		if (this._sleepIndex < 0)
		{
			this.FinishSleep();
			return false;
		}
		return true;
	}

	// Token: 0x060005C8 RID: 1480 RVA: 0x0002A528 File Offset: 0x00028728
	public void CancelStagedWorkForTeardown()
	{
		this._wakeArmed = false;
		this.IsOperationalReady = false;
	}

	// Token: 0x060005C9 RID: 1481 RVA: 0x0002A538 File Offset: 0x00028738
	private void FinishSleep()
	{
		this._dormant = true;
		this._wakeIndex = 0;
		this._sleepIndex = -1;
		this.IsOperationalReady = false;
	}

	// Token: 0x04000451 RID: 1105
	private static readonly ProfilerMarker StagedWakeMarker = new ProfilerMarker("ChunkManager.ZoneStagedWake");

	// Token: 0x04000452 RID: 1106
	private static readonly ProfilerMarker StagedSleepMarker = new ProfilerMarker("ChunkManager.ZoneStagedSleep");

	// Token: 0x04000453 RID: 1107
	private readonly List<GameObject> _stagedObjects = new List<GameObject>(256);

	// Token: 0x04000454 RID: 1108
	private readonly List<bool> _activeDefaults = new List<bool>(256);

	// Token: 0x04000455 RID: 1109
	private int _wakeIndex;

	// Token: 0x04000456 RID: 1110
	private int _sleepIndex = -1;

	// Token: 0x04000457 RID: 1111
	private bool _snapshotReady;

	// Token: 0x04000458 RID: 1112
	private bool _dormant;

	// Token: 0x04000459 RID: 1113
	private bool _wakeArmed;
}
