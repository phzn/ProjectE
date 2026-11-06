using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectExhibitionist.BenchSeating
{
	// Token: 0x02000B30 RID: 2864
	[DisallowMultipleComponent]
	public sealed class BenchNpcCollisionRelease : MonoBehaviour
	{
		// Token: 0x06005147 RID: 20807 RVA: 0x001F7C1C File Offset: 0x001F5E1C
		public static void ProtectStandingCapsule(CapsuleCollider capsule)
		{
			if (capsule == null || !capsule.enabled || !capsule.gameObject.activeInHierarchy)
			{
				return;
			}
			BenchNpcCollisionRelease benchNpcCollisionRelease = capsule.GetComponent<BenchNpcCollisionRelease>();
			if (benchNpcCollisionRelease == null)
			{
				benchNpcCollisionRelease = capsule.gameObject.AddComponent<BenchNpcCollisionRelease>();
			}
			if (benchNpcCollisionRelease.player != capsule)
			{
				benchNpcCollisionRelease.RestoreOwnedPairs();
				benchNpcCollisionRelease.player = capsule;
			}
			benchNpcCollisionRelease.ProtectOverlappingNpcs();
		}

		// Token: 0x06005148 RID: 20808 RVA: 0x001F7C88 File Offset: 0x001F5E88
		private void ProtectOverlappingNpcs()
		{
			Physics.SyncTransforms();
			Vector3 vector;
			Vector3 vector2;
			float num;
			BenchNpcCollisionRelease.GetWorldCapsule(this.player, out vector, out vector2, out num);
			PhysicsScene physicsScene = this.player.gameObject.scene.GetPhysicsScene();
			int num2;
			for (;;)
			{
				num2 = physicsScene.OverlapCapsule(vector, vector2, num, this.overlaps, -1, QueryTriggerInteraction.Ignore);
				if (num2 < this.overlaps.Length)
				{
					break;
				}
				Array.Resize<Collider>(ref this.overlaps, this.overlaps.Length * 2);
			}
			for (int i = 0; i < num2; i++)
			{
				Collider collider = this.overlaps[i];
				this.overlaps[i] = null;
				if (!(collider == null) && !(collider == this.player) && !collider.transform.IsChildOf(this.player.transform) && collider.enabled && collider.gameObject.activeInHierarchy && !collider.isTrigger && BenchSeat.IsNpcCollider(collider) && this.OverlapsPlayer(collider))
				{
					if (this.ownedPairs.Contains(collider))
					{
						if (!Physics.GetIgnoreCollision(this.player, collider))
						{
							Physics.IgnoreCollision(this.player, collider, true);
						}
					}
					else if (!Physics.GetIgnoreCollision(this.player, collider))
					{
						Physics.IgnoreCollision(this.player, collider, true);
						this.ownedPairs.Add(collider);
					}
				}
			}
			base.enabled = this.ownedPairs.Count > 0;
		}

		// Token: 0x06005149 RID: 20809 RVA: 0x001F7E04 File Offset: 0x001F6004
		private void FixedUpdate()
		{
			if (this.player == null || !this.player.enabled || !this.player.gameObject.activeInHierarchy)
			{
				this.RestoreOwnedPairs();
				base.enabled = false;
				return;
			}
			for (int i = this.ownedPairs.Count - 1; i >= 0; i--)
			{
				Collider collider = this.ownedPairs[i];
				if (!(collider != null) || !collider.enabled || !collider.gameObject.activeInHierarchy || !this.OverlapsPlayer(collider))
				{
					this.RestorePair(collider);
					this.ownedPairs.RemoveAt(i);
				}
			}
			if (this.ownedPairs.Count == 0)
			{
				base.enabled = false;
			}
		}

		// Token: 0x0600514A RID: 20810 RVA: 0x001F7EC0 File Offset: 0x001F60C0
		private bool OverlapsPlayer(Collider other)
		{
			Vector3 vector;
			float num;
			return Physics.ComputePenetration(this.player, this.player.transform.position, this.player.transform.rotation, other, other.transform.position, other.transform.rotation, out vector, out num);
		}

		// Token: 0x0600514B RID: 20811 RVA: 0x001F7F14 File Offset: 0x001F6114
		private static void GetWorldCapsule(CapsuleCollider capsule, out Vector3 start, out Vector3 end, out float radius)
		{
			Vector3 lossyScale = capsule.transform.lossyScale;
			lossyScale = new Vector3(Mathf.Abs(lossyScale.x), Mathf.Abs(lossyScale.y), Mathf.Abs(lossyScale.z));
			int direction = capsule.direction;
			Vector3 vector;
			float num;
			float num2;
			if (direction != 0)
			{
				if (direction != 2)
				{
					vector = Vector3.up;
					num = lossyScale.y;
					num2 = Mathf.Max(lossyScale.x, lossyScale.z);
				}
				else
				{
					vector = Vector3.forward;
					num = lossyScale.z;
					num2 = Mathf.Max(lossyScale.x, lossyScale.y);
				}
			}
			else
			{
				vector = Vector3.right;
				num = lossyScale.x;
				num2 = Mathf.Max(lossyScale.y, lossyScale.z);
			}
			radius = capsule.radius * num2;
			float num3 = Mathf.Max(0f, capsule.height * num * 0.5f - radius);
			Vector3 vector2 = capsule.transform.TransformPoint(capsule.center);
			Vector3 vector3 = capsule.transform.TransformDirection(vector).normalized * num3;
			start = vector2 + vector3;
			end = vector2 - vector3;
		}

		// Token: 0x0600514C RID: 20812 RVA: 0x001F803F File Offset: 0x001F623F
		private void RestorePair(Collider other)
		{
			if (this.player != null && other != null)
			{
				Physics.IgnoreCollision(this.player, other, false);
			}
		}

		// Token: 0x0600514D RID: 20813 RVA: 0x001F8068 File Offset: 0x001F6268
		private void RestoreOwnedPairs()
		{
			for (int i = this.ownedPairs.Count - 1; i >= 0; i--)
			{
				this.RestorePair(this.ownedPairs[i]);
			}
			this.ownedPairs.Clear();
		}

		// Token: 0x0600514E RID: 20814 RVA: 0x001F80AA File Offset: 0x001F62AA
		private void OnDisable()
		{
			this.RestoreOwnedPairs();
		}

		// Token: 0x0600514F RID: 20815 RVA: 0x001F80AA File Offset: 0x001F62AA
		private void OnDestroy()
		{
			this.RestoreOwnedPairs();
		}

		// Token: 0x040051F0 RID: 20976
		private CapsuleCollider player;

		// Token: 0x040051F1 RID: 20977
		private Collider[] overlaps = new Collider[32];

		// Token: 0x040051F2 RID: 20978
		private readonly List<Collider> ownedPairs = new List<Collider>();
	}
}
