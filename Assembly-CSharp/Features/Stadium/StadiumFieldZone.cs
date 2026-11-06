using System;
using Invector.vCharacterController;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;

namespace Features.Stadium
{
	// Token: 0x02000C40 RID: 3136
	[RequireComponent(typeof(Collider))]
	public class StadiumFieldZone : MonoBehaviour
	{
		// Token: 0x14000096 RID: 150
		// (add) Token: 0x06005A6B RID: 23147 RVA: 0x00236730 File Offset: 0x00234930
		// (remove) Token: 0x06005A6C RID: 23148 RVA: 0x00236768 File Offset: 0x00234968
		public event Action<Transform> PlayerEntered;

		// Token: 0x14000097 RID: 151
		// (add) Token: 0x06005A6D RID: 23149 RVA: 0x002367A0 File Offset: 0x002349A0
		// (remove) Token: 0x06005A6E RID: 23150 RVA: 0x002367D8 File Offset: 0x002349D8
		public event Action<Transform> PlayerExited;

		// Token: 0x17000EDE RID: 3806
		// (get) Token: 0x06005A6F RID: 23151 RVA: 0x0023680D File Offset: 0x00234A0D
		// (set) Token: 0x06005A70 RID: 23152 RVA: 0x00236815 File Offset: 0x00234A15
		public bool IsPlayerInside { get; private set; }

		// Token: 0x17000EDF RID: 3807
		// (get) Token: 0x06005A71 RID: 23153 RVA: 0x0023681E File Offset: 0x00234A1E
		// (set) Token: 0x06005A72 RID: 23154 RVA: 0x00236826 File Offset: 0x00234A26
		public Transform CurrentPlayer { get; private set; }

		// Token: 0x06005A73 RID: 23155 RVA: 0x0023682F File Offset: 0x00234A2F
		private void Awake()
		{
			this._zoneCollider = base.GetComponent<Collider>();
			if (!this._zoneCollider.isTrigger)
			{
				this._zoneCollider.isTrigger = true;
			}
		}

		// Token: 0x06005A74 RID: 23156 RVA: 0x00236858 File Offset: 0x00234A58
		private void OnDisable()
		{
			this._hasPending = false;
			if (!this.IsPlayerInside)
			{
				return;
			}
			Transform currentPlayer = this.CurrentPlayer;
			this.IsPlayerInside = false;
			this.CurrentPlayer = null;
			Action<Transform> playerExited = this.PlayerExited;
			if (playerExited == null)
			{
				return;
			}
			playerExited(currentPlayer);
		}

		// Token: 0x06005A75 RID: 23157 RVA: 0x0023689C File Offset: 0x00234A9C
		private void Update()
		{
			if (this._trackedPlayer == null && PlayerRegistry.Player != null)
			{
				vThirdPersonController componentInChildren = PlayerRegistry.Player.GetComponentInChildren<vThirdPersonController>(true);
				this._trackedPlayer = ((componentInChildren != null) ? componentInChildren.transform : PlayerRegistry.Player);
			}
			this.RefreshPresence(Time.time);
		}

		// Token: 0x06005A76 RID: 23158 RVA: 0x002368F8 File Offset: 0x00234AF8
		private void OnTriggerEnter(Collider other)
		{
			vThirdPersonController componentInParent = other.GetComponentInParent<vThirdPersonController>(true);
			if (componentInParent == null)
			{
				return;
			}
			this.TrackPlayer(componentInParent.transform);
		}

		// Token: 0x06005A77 RID: 23159 RVA: 0x00236924 File Offset: 0x00234B24
		private void OnTriggerExit(Collider other)
		{
			vThirdPersonController componentInParent = other.GetComponentInParent<vThirdPersonController>(true);
			if (componentInParent == null)
			{
				return;
			}
			this.TrackPlayer(componentInParent.transform);
		}

		// Token: 0x06005A78 RID: 23160 RVA: 0x0023694F File Offset: 0x00234B4F
		private void TrackPlayer(Transform player)
		{
			if (this._trackedPlayer != null && player != this._trackedPlayer)
			{
				return;
			}
			this._trackedPlayer = player;
			this.RefreshPresence(Time.time);
		}

		// Token: 0x06005A79 RID: 23161 RVA: 0x00236980 File Offset: 0x00234B80
		private bool ContainsPlayerRoot()
		{
			if (this._zoneCollider == null)
			{
				this._zoneCollider = base.GetComponent<Collider>();
			}
			if (this._trackedPlayer == null || this._zoneCollider == null || !this._zoneCollider.enabled)
			{
				return false;
			}
			Vector3 position = this._trackedPlayer.position;
			BoxCollider boxCollider = this._zoneCollider as BoxCollider;
			if (boxCollider != null)
			{
				Vector3 vector = boxCollider.transform.InverseTransformPoint(position) - boxCollider.center;
				Vector3 vector2 = boxCollider.size * 0.5f;
				return Mathf.Abs(vector.x) <= vector2.x && Mathf.Abs(vector.y) <= vector2.y && Mathf.Abs(vector.z) <= vector2.z;
			}
			return (this._zoneCollider.ClosestPoint(position) - position).sqrMagnitude <= 1E-06f;
		}

		// Token: 0x06005A7A RID: 23162 RVA: 0x00236A7C File Offset: 0x00234C7C
		private void RefreshPresence(float now)
		{
			bool flag = this.ContainsPlayerRoot();
			if (flag == this.IsPlayerInside)
			{
				this._hasPending = false;
				return;
			}
			if (!this._hasPending || this._pendingTargetInside != flag)
			{
				this._hasPending = true;
				this._pendingTargetInside = flag;
				this._pendingSince = now;
			}
			float num = (flag ? this.enterDelay : this.exitDelay);
			if (now - this._pendingSince < num)
			{
				return;
			}
			this._hasPending = false;
			if (flag)
			{
				this.IsPlayerInside = true;
				this.CurrentPlayer = this._trackedPlayer;
				if (this.debugLogs)
				{
					Debug.Log("[StadiumFieldZone] PlayerEntered committed", this);
				}
				Action<Transform> playerEntered = this.PlayerEntered;
				if (playerEntered == null)
				{
					return;
				}
				playerEntered(this.CurrentPlayer);
				return;
			}
			else
			{
				Transform currentPlayer = this.CurrentPlayer;
				this.IsPlayerInside = false;
				this.CurrentPlayer = null;
				if (this.debugLogs)
				{
					Debug.Log("[StadiumFieldZone] PlayerExited committed", this);
				}
				Action<Transform> playerExited = this.PlayerExited;
				if (playerExited == null)
				{
					return;
				}
				playerExited(currentPlayer);
				return;
			}
		}

		// Token: 0x04005B62 RID: 23394
		[Header("Hysteresis (seconds)")]
		[Tooltip("Time the player must stay inside before PlayerEntered fires.")]
		[Min(0f)]
		[SerializeField]
		private float enterDelay = 0.35f;

		// Token: 0x04005B63 RID: 23395
		[Tooltip("Time the player must stay outside before PlayerExited fires.")]
		[Min(0f)]
		[SerializeField]
		private float exitDelay = 0.6f;

		// Token: 0x04005B64 RID: 23396
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04005B69 RID: 23401
		private Collider _zoneCollider;

		// Token: 0x04005B6A RID: 23402
		private Transform _trackedPlayer;

		// Token: 0x04005B6B RID: 23403
		private bool _hasPending;

		// Token: 0x04005B6C RID: 23404
		private bool _pendingTargetInside;

		// Token: 0x04005B6D RID: 23405
		private float _pendingSince;
	}
}
