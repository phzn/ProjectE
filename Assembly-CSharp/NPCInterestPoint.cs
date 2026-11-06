using System;
using System.Collections.Generic;
using System.Text;
using Features.NPC;
using ProjectExhibitionist.BenchSeating;
using UnityEngine;

// Token: 0x020000BD RID: 189
public class NPCInterestPoint : MonoBehaviour
{
	// Token: 0x170001AA RID: 426
	// (get) Token: 0x0600078F RID: 1935 RVA: 0x000358FE File Offset: 0x00033AFE
	public BenchSeat BenchSeat
	{
		get
		{
			if (this._benchSeat == null)
			{
				base.TryGetComponent<BenchSeat>(out this._benchSeat);
			}
			return this._benchSeat;
		}
	}

	// Token: 0x170001AB RID: 427
	// (get) Token: 0x06000790 RID: 1936 RVA: 0x00035924 File Offset: 0x00033B24
	public bool AllowsAmbientLife
	{
		get
		{
			if (this.ambientActivity == NPCInterestPoint.AmbientActivity.Disabled || this.posture != NPCInterestPoint.PostureType.Standing || this.approachPoint != null)
			{
				return false;
			}
			if (this.allowedActions != null && this.allowedActions.Count > 0 && !this.allowedActions.Contains(NPCInterestPoint.ActionType.None))
			{
				return false;
			}
			if (this.propsToEnable != null)
			{
				using (List<string>.Enumerator enumerator = this.propsToEnable.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!string.Equals(enumerator.Current, "Smartphone", StringComparison.Ordinal))
						{
							return false;
						}
					}
				}
				return true;
			}
			return true;
		}
	}

	// Token: 0x170001AC RID: 428
	// (get) Token: 0x06000791 RID: 1937 RVA: 0x000359D4 File Offset: 0x00033BD4
	public bool SupportsAmbientConversation
	{
		get
		{
			return this.AllowsAmbientLife && this.IsGroupPoint && this.maxCapacity >= 2 && (this.ambientActivity == NPCInterestPoint.AmbientActivity.Automatic || this.ambientActivity == NPCInterestPoint.AmbientActivity.Social);
		}
	}

	// Token: 0x06000792 RID: 1938 RVA: 0x00035A04 File Offset: 0x00033C04
	public NPCAmbientConversation GetAmbientConversation()
	{
		if (this._ambientConversation == null)
		{
			this._ambientConversation = new NPCAmbientConversation(base.GetInstanceID());
		}
		return this._ambientConversation;
	}

	// Token: 0x06000793 RID: 1939 RVA: 0x00035A28 File Offset: 0x00033C28
	public Vector3 GetAmbientNavigationPosition(int npcInstanceId)
	{
		if (!this.SupportsAmbientConversation || (!this._reservedBy.Contains(npcInstanceId) && !this._occupiedBy.Contains(npcInstanceId)))
		{
			return this.NavigationPosition;
		}
		int num;
		if (!this._ambientSlots.TryGetValue(npcInstanceId, out num))
		{
			num = 0;
			while (this._ambientSlots.ContainsValue(num) && num < this.maxCapacity)
			{
				num++;
			}
			this._ambientSlots[npcInstanceId] = num;
		}
		float num2 = (float)num * 3.1415927f * 2f / (float)Mathf.Max(2, this.maxCapacity);
		float num3 = Mathf.Clamp(this.groupRadius * 0.55f, 0.65f, 1.25f);
		return base.transform.position + base.transform.rotation * new Vector3(Mathf.Sin(num2) * num3, 0f, Mathf.Cos(num2) * num3);
	}

	// Token: 0x170001AD RID: 429
	// (get) Token: 0x06000794 RID: 1940 RVA: 0x00035B0F File Offset: 0x00033D0F
	public Vector3 NavigationPosition
	{
		get
		{
			if (this.BenchSeat != null)
			{
				return this.BenchSeat.ApproachPosition;
			}
			if (!(this.approachPoint != null))
			{
				return this.TargetPosition;
			}
			return this.approachPoint.position;
		}
	}

	// Token: 0x170001AE RID: 430
	// (get) Token: 0x06000795 RID: 1941 RVA: 0x00035B4C File Offset: 0x00033D4C
	public Vector3 TargetPosition
	{
		get
		{
			if (this.BenchSeat != null)
			{
				return this.BenchSeat.SeatPosition;
			}
			if (this.mode != NPCInterestPoint.PointMode.Group)
			{
				return base.transform.position + base.transform.TransformDirection(this.positionOffset);
			}
			return this.GetRandomPositionInGroup();
		}
	}

	// Token: 0x170001AF RID: 431
	// (get) Token: 0x06000796 RID: 1942 RVA: 0x00035BA4 File Offset: 0x00033DA4
	public bool HasSeparateApproachPoint
	{
		get
		{
			return this.BenchSeat != null || this.approachPoint != null;
		}
	}

	// Token: 0x170001B0 RID: 432
	// (get) Token: 0x06000797 RID: 1943 RVA: 0x00035BC2 File Offset: 0x00033DC2
	public Quaternion TargetRotation
	{
		get
		{
			if (!(this.BenchSeat != null))
			{
				return base.transform.rotation;
			}
			return this.BenchSeat.SeatRotation;
		}
	}

	// Token: 0x170001B1 RID: 433
	// (get) Token: 0x06000798 RID: 1944 RVA: 0x00035BE9 File Offset: 0x00033DE9
	public float EffectiveArrivalRadius
	{
		get
		{
			if (this.mode != NPCInterestPoint.PointMode.Group)
			{
				return this.arrivalRadius;
			}
			return this.groupRadius;
		}
	}

	// Token: 0x170001B2 RID: 434
	// (get) Token: 0x06000799 RID: 1945 RVA: 0x00035C01 File Offset: 0x00033E01
	public bool RequiresAlignment
	{
		get
		{
			return this.mode == NPCInterestPoint.PointMode.Single && this.alignToRotation;
		}
	}

	// Token: 0x170001B3 RID: 435
	// (get) Token: 0x0600079A RID: 1946 RVA: 0x00035C13 File Offset: 0x00033E13
	public bool IsGroupPoint
	{
		get
		{
			return this.mode == NPCInterestPoint.PointMode.Group;
		}
	}

	// Token: 0x170001B4 RID: 436
	// (get) Token: 0x0600079B RID: 1947 RVA: 0x00035C1E File Offset: 0x00033E1E
	public int ReservationCount
	{
		get
		{
			return this._reservedBy.Count;
		}
	}

	// Token: 0x170001B5 RID: 437
	// (get) Token: 0x0600079C RID: 1948 RVA: 0x00035C2B File Offset: 0x00033E2B
	public int OccupantCount
	{
		get
		{
			return this._occupiedBy.Count;
		}
	}

	// Token: 0x170001B6 RID: 438
	// (get) Token: 0x0600079D RID: 1949 RVA: 0x00035C38 File Offset: 0x00033E38
	public bool CanReserve
	{
		get
		{
			return this._reservedBy.Count + this._occupiedBy.Count < this.maxCapacity && (this.BenchSeat == null || this.BenchSeat.IsAvailable);
		}
	}

	// Token: 0x170001B7 RID: 439
	// (get) Token: 0x0600079E RID: 1950 RVA: 0x00035C76 File Offset: 0x00033E76
	public bool IsAvailable
	{
		get
		{
			return this.CanReserve;
		}
	}

	// Token: 0x170001B8 RID: 440
	// (get) Token: 0x0600079F RID: 1951 RVA: 0x00035C80 File Offset: 0x00033E80
	public string OwnerChunkLabel
	{
		get
		{
			if (this._ownerChunkLabel != null)
			{
				return this._ownerChunkLabel;
			}
			Transform transform = base.transform.parent;
			Transform transform2 = null;
			while (transform != null)
			{
				string text = transform.name.ToLowerInvariant();
				if (text.Contains("chunk") || text.Contains("1x1") || text.Contains("1x2") || text.Contains("2x1") || text.Contains("2x2") || text.Contains("3x3"))
				{
					transform2 = transform;
				}
				transform = transform.parent;
			}
			if (transform2 != null)
			{
				this._ownerChunkLabel = transform2.name;
				return this._ownerChunkLabel;
			}
			this._ownerChunkLabel = ((base.transform.parent != null) ? base.transform.parent.name : "(orphan)");
			return this._ownerChunkLabel;
		}
	}

	// Token: 0x060007A0 RID: 1952 RVA: 0x00035D6C File Offset: 0x00033F6C
	public bool TryReserve(int npcInstanceId)
	{
		if (this.BenchSeat != null && !this.BenchSeat.TryReserve(npcInstanceId))
		{
			return false;
		}
		if (this._reservedBy.Contains(npcInstanceId))
		{
			this.VerboseLog(string.Format("TryReserve({0}): Already reserved by this NPC", npcInstanceId));
			return true;
		}
		if (this._occupiedBy.Contains(npcInstanceId))
		{
			this.VerboseLog(string.Format("TryReserve({0}): Already occupied by this NPC", npcInstanceId));
			return true;
		}
		if (this._reservedBy.Count + this._occupiedBy.Count >= this.maxCapacity)
		{
			if (this.BenchSeat != null)
			{
				this.BenchSeat.Release(npcInstanceId);
			}
			this.VerboseLog(string.Format("TryReserve({0}): FAILED - Capacity full (Reserved={1}, Occupied={2}, Max={3})", new object[] { npcInstanceId, this.ReservationCount, this.OccupantCount, this.maxCapacity }));
			return false;
		}
		this._reservedBy.Add(npcInstanceId);
		this.VerboseLog(string.Format("TryReserve({0}): SUCCESS (Reserved={1}, Occupied={2})", npcInstanceId, this.ReservationCount, this.OccupantCount));
		return true;
	}

	// Token: 0x060007A1 RID: 1953 RVA: 0x00035EA4 File Offset: 0x000340A4
	public void Occupy(int npcInstanceId)
	{
		if (this.BenchSeat != null && !this.BenchSeat.Occupy(npcInstanceId))
		{
			return;
		}
		this._reservedBy.Remove(npcInstanceId);
		this._occupiedBy.Add(npcInstanceId);
		this.VerboseLog(string.Format("Occupy({0}): NPC arrived (Reserved={1}, Occupied={2})", npcInstanceId, this.ReservationCount, this.OccupantCount));
	}

	// Token: 0x060007A2 RID: 1954 RVA: 0x00035F14 File Offset: 0x00034114
	public void Release(int npcInstanceId)
	{
		if (this.BenchSeat != null)
		{
			this.BenchSeat.Release(npcInstanceId);
		}
		this._ambientSlots.Remove(npcInstanceId);
		bool flag = this._reservedBy.Remove(npcInstanceId);
		bool flag2 = this._occupiedBy.Remove(npcInstanceId);
		this.VerboseLog(string.Format("Release({0}): wasReserved={1}, wasOccupied={2} (Now: Reserved={3}, Occupied={4})", new object[] { npcInstanceId, flag, flag2, this.ReservationCount, this.OccupantCount }));
	}

	// Token: 0x060007A3 RID: 1955 RVA: 0x00035FB0 File Offset: 0x000341B0
	public string GetDiagnosticSnapshot()
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (int num in this._reservedBy)
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append(',');
			}
			stringBuilder.Append(num);
		}
		StringBuilder stringBuilder2 = new StringBuilder();
		foreach (int num2 in this._occupiedBy)
		{
			if (stringBuilder2.Length > 0)
			{
				stringBuilder2.Append(',');
			}
			stringBuilder2.Append(num2);
		}
		return string.Format("[POI '{0}'@chunk='{1}'] active={2} maxCap={3} reserved=[{4}] occupied=[{5}]", new object[]
		{
			base.name,
			this.OwnerChunkLabel,
			base.gameObject.activeInHierarchy,
			this.maxCapacity,
			stringBuilder,
			stringBuilder2
		});
	}

	// Token: 0x060007A4 RID: 1956 RVA: 0x000360C4 File Offset: 0x000342C4
	private void VerboseLog(string msg)
	{
		if (this.enableDebugLogs || NPCInterestPoint.GlobalVerboseReservationLogs)
		{
			Debug.Log(string.Concat(new string[] { "[NPCInterestPoint '", base.name, "'@'", this.OwnerChunkLabel, "'] ", msg }));
		}
	}

	// Token: 0x060007A5 RID: 1957 RVA: 0x0003611E File Offset: 0x0003431E
	public float GetRandomWaitTime()
	{
		return Random.Range(this.waitTimeRange.x, this.waitTimeRange.y);
	}

	// Token: 0x060007A6 RID: 1958 RVA: 0x0003613B File Offset: 0x0003433B
	public NPCInterestPoint.ActionType GetRandomAction()
	{
		if (this.allowedActions == null || this.allowedActions.Count == 0)
		{
			return NPCInterestPoint.ActionType.None;
		}
		return this.allowedActions[Random.Range(0, this.allowedActions.Count)];
	}

	// Token: 0x060007A7 RID: 1959 RVA: 0x00036170 File Offset: 0x00034370
	public Vector3 GetRandomPositionInGroup()
	{
		Vector2 vector = Random.insideUnitCircle * this.groupRadius;
		return base.transform.position + new Vector3(vector.x, 0f, vector.y);
	}

	// Token: 0x060007A8 RID: 1960 RVA: 0x000361B4 File Offset: 0x000343B4
	public HashSet<int> GetCurrentOccupants()
	{
		return new HashSet<int>(this._occupiedBy);
	}

	// Token: 0x060007A9 RID: 1961 RVA: 0x000361C4 File Offset: 0x000343C4
	private void OnDisable()
	{
		if (this.BenchSeat != null)
		{
			foreach (int num in this._reservedBy)
			{
				this.BenchSeat.Release(num);
			}
			foreach (int num2 in this._occupiedBy)
			{
				this.BenchSeat.Release(num2);
			}
		}
		NPCAmbientConversation ambientConversation = this._ambientConversation;
		if (ambientConversation != null)
		{
			ambientConversation.Clear();
		}
		this._ambientSlots.Clear();
		this._reservedBy.Clear();
		this._occupiedBy.Clear();
	}

	// Token: 0x060007AA RID: 1962 RVA: 0x000362A4 File Offset: 0x000344A4
	private void Log(string message)
	{
		if (this.enableDebugLogs)
		{
			Debug.Log("[NPCInterestPoint] " + base.gameObject.name + ": " + message);
		}
	}

	// Token: 0x0400066F RID: 1647
	private BenchSeat _benchSeat;

	// Token: 0x04000670 RID: 1648
	[Header("Point Mode")]
	[Tooltip("Modalità del punto: Single (1 NPC) o Group (più NPC in un'area)")]
	public NPCInterestPoint.PointMode mode;

	// Token: 0x04000671 RID: 1649
	[Tooltip("Raggio dell'area di gruppo (usato solo se mode = Group)")]
	[Range(0.5f, 10f)]
	public float groupRadius = 2f;

	// Token: 0x04000672 RID: 1650
	[Header("Posture & Action")]
	[Tooltip("Postura richiesta per questo punto")]
	public NPCInterestPoint.PostureType posture;

	// Token: 0x04000673 RID: 1651
	[Tooltip("Azioni STATICHE permesse in questo punto. Se vuoto, solo idle.")]
	public List<NPCInterestPoint.ActionType> allowedActions = new List<NPCInterestPoint.ActionType> { NPCInterestPoint.ActionType.None };

	// Token: 0x04000674 RID: 1652
	[Tooltip("Peso per la selezione random di questo punto (più alto = più probabile)")]
	[Range(0.1f, 10f)]
	public float selectionWeight = 1f;

	// Token: 0x04000675 RID: 1653
	[Header("Arrival")]
	[Tooltip("Distanza per considerare l'NPC arrivato (ignorato per Group mode, usa groupRadius)")]
	public float arrivalRadius = 0.5f;

	// Token: 0x04000676 RID: 1654
	[Tooltip("Se true, l'NPC si allinea alla rotazione del punto (solo per Single mode)")]
	public bool alignToRotation = true;

	// Token: 0x04000677 RID: 1655
	[Header("Timing")]
	[Tooltip("Tempo di permanenza (Min/Max secondi)")]
	public Vector2 waitTimeRange = new Vector2(3f, 8f);

	// Token: 0x04000678 RID: 1656
	[Header("Capacity & Reservation")]
	[Tooltip("Numero massimo di NPC che possono usare questo punto")]
	public int maxCapacity = 1;

	// Token: 0x04000679 RID: 1657
	[Header("Interaction (Optional)")]
	[Tooltip("Oggetto nella scena con cui l'NPC interagisce (es. panchina, sedia)")]
	public Transform interactionTarget;

	// Token: 0x0400067A RID: 1658
	[Tooltip("Offset dalla posizione del punto per il posizionamento finale (solo Single mode)")]
	public Vector3 positionOffset;

	// Token: 0x0400067B RID: 1659
	[Tooltip("Punto di approccio opzionale. Se assegnato, l'NPC naviga qui prima di spostarsi alla posizione finale. Utile quando il POI è su un'area non navigabile (es. panchina).")]
	public Transform approachPoint;

	// Token: 0x0400067C RID: 1660
	[Header("Props")]
	[Tooltip("Nomi degli oggetti figli dell'NPC da attivare durante l'azione")]
	public List<string> propsToEnable = new List<string>();

	// Token: 0x0400067D RID: 1661
	[Header("Civilian Ambient Life")]
	[Tooltip("Automatic adds quiet ambient life to ordinary standing points and conversation to group points. Disabled preserves the authored behavior exactly.")]
	public NPCInterestPoint.AmbientActivity ambientActivity;

	// Token: 0x0400067E RID: 1662
	private readonly Dictionary<int, int> _ambientSlots = new Dictionary<int, int>();

	// Token: 0x0400067F RID: 1663
	private NPCAmbientConversation _ambientConversation;

	// Token: 0x04000680 RID: 1664
	private HashSet<int> _reservedBy = new HashSet<int>();

	// Token: 0x04000681 RID: 1665
	private HashSet<int> _occupiedBy = new HashSet<int>();

	// Token: 0x04000682 RID: 1666
	[Header("Debug")]
	public bool enableDebugLogs;

	// Token: 0x04000683 RID: 1667
	public static bool GlobalVerboseReservationLogs;

	// Token: 0x04000684 RID: 1668
	private string _ownerChunkLabel;

	// Token: 0x020000BE RID: 190
	public enum PostureType
	{
		// Token: 0x04000686 RID: 1670
		Standing,
		// Token: 0x04000687 RID: 1671
		Sitting
	}

	// Token: 0x020000BF RID: 191
	public enum ActionType
	{
		// Token: 0x04000689 RID: 1673
		None,
		// Token: 0x0400068A RID: 1674
		WritingPhone,
		// Token: 0x0400068B RID: 1675
		TalkingPhone
	}

	// Token: 0x020000C0 RID: 192
	public enum PointMode
	{
		// Token: 0x0400068D RID: 1677
		Single,
		// Token: 0x0400068E RID: 1678
		Group
	}

	// Token: 0x020000C1 RID: 193
	public enum AmbientActivity
	{
		// Token: 0x04000690 RID: 1680
		Automatic,
		// Token: 0x04000691 RID: 1681
		Disabled,
		// Token: 0x04000692 RID: 1682
		Social,
		// Token: 0x04000693 RID: 1683
		Observe,
		// Token: 0x04000694 RID: 1684
		Refreshment
	}
}
