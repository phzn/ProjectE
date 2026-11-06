using System;
using System.Collections.Generic;
using Features.NPC;
using Pathfinding;
using UnityEngine;

namespace ProjectExhibitionist.Map
{
	// Token: 0x020009C4 RID: 2500
	[DisallowMultipleComponent]
	[RequireComponent(typeof(MallEscalatorConveyor))]
	public sealed class MallEscalatorNpcRecruiter : MonoBehaviour
	{
		// Token: 0x17000BF4 RID: 3060
		// (get) Token: 0x0600465D RID: 18013 RVA: 0x001AA0FE File Offset: 0x001A82FE
		public bool DebugLogs
		{
			get
			{
				return this.debugLogs;
			}
		}

		// Token: 0x0600465E RID: 18014 RVA: 0x001AA106 File Offset: 0x001A8306
		private void Awake()
		{
			this._conveyor = base.GetComponent<MallEscalatorConveyor>();
			this._nextAttemptTime = Time.time + Random.Range(this.recruitInterval, this.recruitInterval + 6f);
		}

		// Token: 0x0600465F RID: 18015 RVA: 0x001AA137 File Offset: 0x001A8337
		private void Start()
		{
			this.InstallNavmeshBlockers();
		}

		// Token: 0x06004660 RID: 18016 RVA: 0x001AA140 File Offset: 0x001A8340
		private void InstallNavmeshBlockers()
		{
			Collider walkSurface = this._conveyor.WalkSurface;
			if (walkSurface != null && walkSurface.GetComponent<RecastNavmeshModifier>() == null)
			{
				RecastNavmeshModifier recastNavmeshModifier = walkSurface.gameObject.AddComponent<RecastNavmeshModifier>();
				recastNavmeshModifier.mode = RecastNavmeshModifier.Mode.UnwalkableSurface;
				recastNavmeshModifier.includeInScan = RecastNavmeshModifier.ScanInclusion.AlwaysInclude;
				recastNavmeshModifier.geometrySource = RecastNavmeshModifier.GeometrySource.Collider;
				recastNavmeshModifier.dynamic = true;
			}
			Transform visualStepsRoot = this._conveyor.VisualStepsRoot;
			if (visualStepsRoot == null)
			{
				return;
			}
			for (int i = 0; i < visualStepsRoot.childCount; i++)
			{
				GameObject gameObject = visualStepsRoot.GetChild(i).gameObject;
				if (!(gameObject.GetComponent<RecastNavmeshModifier>() != null))
				{
					RecastNavmeshModifier recastNavmeshModifier2 = gameObject.AddComponent<RecastNavmeshModifier>();
					recastNavmeshModifier2.includeInScan = RecastNavmeshModifier.ScanInclusion.AlwaysExclude;
					recastNavmeshModifier2.dynamic = true;
				}
			}
		}

		// Token: 0x06004661 RID: 18017 RVA: 0x001AA1EC File Offset: 0x001A83EC
		private void Update()
		{
			if (Time.time < this._nextAttemptTime)
			{
				return;
			}
			this._nextAttemptTime = Time.time + this.recruitInterval;
			if (this._activeRide != null && this._activeRide.IsBusy)
			{
				return;
			}
			if (AstarPath.active == null || this._conveyor == null || this._conveyor.WalkSurface == null)
			{
				return;
			}
			this.TryRecruit();
		}

		// Token: 0x06004662 RID: 18018 RVA: 0x001AA26C File Offset: 0x001A846C
		private void TryRecruit()
		{
			Vector3 vector = (this._conveyor.GoesDown ? this._conveyor.UpperWorldPoint : this._conveyor.LowerWorldPoint);
			NPCWanderController npcwanderController = null;
			float num = float.PositiveInfinity;
			IReadOnlyList<NPCWanderController> activeInstances = NPCWanderController.ActiveInstances;
			for (int i = 0; i < activeInstances.Count; i++)
			{
				NPCWanderController npcwanderController2 = activeInstances[i];
				if (!(npcwanderController2 == null) && npcwanderController2.isActiveAndEnabled && !npcwanderController2.ExternalControl && (npcwanderController2.CurrentState == NPCWanderController.WanderState.Idle || npcwanderController2.CurrentState == NPCWanderController.WanderState.Navigating))
				{
					Vector3 position = npcwanderController2.transform.position;
					if (Mathf.Abs(position.y - vector.y) <= this.verticalRecruitWindow)
					{
						Vector3 vector2 = position - vector;
						vector2.y = 0f;
						float sqrMagnitude = vector2.sqrMagnitude;
						if (sqrMagnitude <= this.recruitRadius * this.recruitRadius && sqrMagnitude < num)
						{
							num = sqrMagnitude;
							npcwanderController = npcwanderController2;
						}
					}
				}
			}
			if (npcwanderController == null)
			{
				return;
			}
			MallEscalatorNpcRide mallEscalatorNpcRide = npcwanderController.GetComponent<MallEscalatorNpcRide>();
			if (mallEscalatorNpcRide == null)
			{
				mallEscalatorNpcRide = npcwanderController.gameObject.AddComponent<MallEscalatorNpcRide>();
			}
			if (mallEscalatorNpcRide.Begin(this, this._conveyor))
			{
				this._activeRide = mallEscalatorNpcRide;
				this.Log(string.Concat(new string[]
				{
					"Recruited '",
					npcwanderController.name,
					"' (",
					this._conveyor.GoesDown ? "down" : "up",
					")"
				}));
			}
		}

		// Token: 0x06004663 RID: 18019 RVA: 0x001AA3FB File Offset: 0x001A85FB
		public void NotifyRideFinished(MallEscalatorNpcRide ride)
		{
			if (this._activeRide == ride)
			{
				this._activeRide = null;
			}
			this._nextAttemptTime = Time.time + Random.Range(this.rideCooldown.x, this.rideCooldown.y);
		}

		// Token: 0x06004664 RID: 18020 RVA: 0x001AA439 File Offset: 0x001A8639
		private void Log(string message)
		{
			if (this.debugLogs)
			{
				Debug.Log("[EscalatorNPC] " + base.name + ": " + message);
			}
		}

		// Token: 0x040045F3 RID: 17907
		[SerializeField]
		[Min(1f)]
		private float recruitRadius = 9f;

		// Token: 0x040045F4 RID: 17908
		[SerializeField]
		[Min(0.5f)]
		private float recruitInterval = 3f;

		// Token: 0x040045F5 RID: 17909
		[SerializeField]
		private Vector2 rideCooldown = new Vector2(12f, 24f);

		// Token: 0x040045F6 RID: 17910
		[SerializeField]
		[Min(0.5f)]
		private float verticalRecruitWindow = 3f;

		// Token: 0x040045F7 RID: 17911
		[SerializeField]
		private bool debugLogs;

		// Token: 0x040045F8 RID: 17912
		private MallEscalatorConveyor _conveyor;

		// Token: 0x040045F9 RID: 17913
		private MallEscalatorNpcRide _activeRide;

		// Token: 0x040045FA RID: 17914
		private float _nextAttemptTime;
	}
}
