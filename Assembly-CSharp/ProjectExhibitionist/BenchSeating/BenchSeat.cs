using System;
using System.Collections.Generic;
using Features.Detection;
using Features.NPC;
using UnityEngine;

namespace ProjectExhibitionist.BenchSeating
{
	// Token: 0x02000B33 RID: 2867
	[DisallowMultipleComponent]
	public sealed class BenchSeat : MonoBehaviour
	{
		// Token: 0x17000D90 RID: 3472
		// (get) Token: 0x06005172 RID: 20850 RVA: 0x001F9BC0 File Offset: 0x001F7DC0
		public static IReadOnlyList<BenchSeat> ActiveSeats
		{
			get
			{
				return BenchSeat.activeSeats;
			}
		}

		// Token: 0x17000D91 RID: 3473
		// (get) Token: 0x06005173 RID: 20851 RVA: 0x001F9BC7 File Offset: 0x001F7DC7
		public BenchAnimationSet Animations
		{
			get
			{
				return this.animations;
			}
		}

		// Token: 0x17000D92 RID: 3474
		// (get) Token: 0x06005174 RID: 20852 RVA: 0x001F9BCF File Offset: 0x001F7DCF
		public Vector3 SeatPosition
		{
			get
			{
				if (!(this.seatAnchor != null))
				{
					return base.transform.position;
				}
				return this.seatAnchor.position;
			}
		}

		// Token: 0x17000D93 RID: 3475
		// (get) Token: 0x06005175 RID: 20853 RVA: 0x001F9BF8 File Offset: 0x001F7DF8
		public Quaternion SeatRotation
		{
			get
			{
				if (this.cushionPoint != null)
				{
					return this.cushionPoint.rotation;
				}
				if (!(this.seatAnchor != null))
				{
					return base.transform.rotation;
				}
				return this.seatAnchor.rotation;
			}
		}

		// Token: 0x17000D94 RID: 3476
		// (get) Token: 0x06005176 RID: 20854 RVA: 0x001F9C44 File Offset: 0x001F7E44
		public Vector3 CushionPosition
		{
			get
			{
				if (!(this.cushionPoint != null))
				{
					return this.SeatPosition - this.SeatRotation * Vector3.forward * 0.1f + Vector3.up * (0.44f + this.sittingHeightOffset);
				}
				return this.cushionPoint.position;
			}
		}

		// Token: 0x17000D95 RID: 3477
		// (get) Token: 0x06005177 RID: 20855 RVA: 0x001F9CAB File Offset: 0x001F7EAB
		public Quaternion CushionRotation
		{
			get
			{
				if (!(this.cushionPoint != null))
				{
					return this.SeatRotation;
				}
				return this.cushionPoint.rotation;
			}
		}

		// Token: 0x17000D96 RID: 3478
		// (get) Token: 0x06005178 RID: 20856 RVA: 0x001F9CD0 File Offset: 0x001F7ED0
		public Vector3 ApproachPosition
		{
			get
			{
				if (this.cushionPoint != null)
				{
					return new Vector3(this.cushionPoint.position.x, this.SeatPosition.y, this.cushionPoint.position.z) + this.CushionRotation * Vector3.forward * (this.frontEdgeDistance + 0.55f);
				}
				if (!(this.approachPoint != null))
				{
					return this.SeatPosition + this.SeatRotation * Vector3.forward * 0.8f;
				}
				return this.approachPoint.position;
			}
		}

		// Token: 0x17000D97 RID: 3479
		// (get) Token: 0x06005179 RID: 20857 RVA: 0x001F9D81 File Offset: 0x001F7F81
		public bool IsOccupied
		{
			get
			{
				return this.occupied && this.OwnerId != 0;
			}
		}

		// Token: 0x17000D98 RID: 3480
		// (get) Token: 0x0600517A RID: 20858 RVA: 0x001F9D96 File Offset: 0x001F7F96
		public int OwnerId
		{
			get
			{
				this.PruneOwner();
				return this.ownerId;
			}
		}

		// Token: 0x17000D99 RID: 3481
		// (get) Token: 0x0600517B RID: 20859 RVA: 0x001F9DA4 File Offset: 0x001F7FA4
		public bool IsAvailable
		{
			get
			{
				return base.isActiveAndEnabled && this.OwnerId == 0 && !this.HasStaticOccupant && !this.HasNearbyActor(0);
			}
		}

		// Token: 0x17000D9A RID: 3482
		// (get) Token: 0x0600517C RID: 20860 RVA: 0x001F9DCA File Offset: 0x001F7FCA
		public bool HasStaticOccupant
		{
			get
			{
				return this.blockedByStaticOccupant || (this.staticOccupant != null && this.staticOccupant.gameObject.activeInHierarchy);
			}
		}

		// Token: 0x0600517D RID: 20861 RVA: 0x001F9DF6 File Offset: 0x001F7FF6
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetRegistry()
		{
			BenchSeat.activeSeats.Clear();
		}

		// Token: 0x0600517E RID: 20862 RVA: 0x001F9E04 File Offset: 0x001F8004
		private void OnEnable()
		{
			if (this.activeIndex >= 0 && this.activeIndex < BenchSeat.activeSeats.Count && BenchSeat.activeSeats[this.activeIndex] == this)
			{
				return;
			}
			this.activeIndex = BenchSeat.activeSeats.Count;
			BenchSeat.activeSeats.Add(this);
		}

		// Token: 0x0600517F RID: 20863 RVA: 0x001F9E5B File Offset: 0x001F805B
		private void OnDisable()
		{
			this.RemoveActive();
			this.ClearOwner();
		}

		// Token: 0x06005180 RID: 20864 RVA: 0x001F9E6C File Offset: 0x001F806C
		private void RemoveActive()
		{
			int num = this.activeIndex;
			this.activeIndex = -1;
			if (num < 0 || num >= BenchSeat.activeSeats.Count || BenchSeat.activeSeats[num] != this)
			{
				num = BenchSeat.activeSeats.IndexOf(this);
				if (num < 0)
				{
					return;
				}
			}
			int num2 = BenchSeat.activeSeats.Count - 1;
			if (num != num2)
			{
				BenchSeat.activeSeats[num] = BenchSeat.activeSeats[num2];
				BenchSeat.activeSeats[num].activeIndex = num;
			}
			BenchSeat.activeSeats.RemoveAt(num2);
		}

		// Token: 0x06005181 RID: 20865 RVA: 0x001F9EFA File Offset: 0x001F80FA
		public bool TryReserve(Object actor)
		{
			if (actor == null || !this.TryReserve(actor.GetInstanceID()))
			{
				return false;
			}
			this.owner = actor;
			this.tracksOwner = true;
			return true;
		}

		// Token: 0x06005182 RID: 20866 RVA: 0x001F9F24 File Offset: 0x001F8124
		public bool TryReserve(int actorId)
		{
			if (actorId == 0 || !base.isActiveAndEnabled || this.HasStaticOccupant)
			{
				return false;
			}
			this.PruneOwner();
			if (this.ownerId == actorId)
			{
				return true;
			}
			if (this.ownerId != 0 || this.HasNearbyActor(actorId))
			{
				return false;
			}
			this.ownerId = actorId;
			return true;
		}

		// Token: 0x06005183 RID: 20867 RVA: 0x001F9F72 File Offset: 0x001F8172
		public bool IsReservedBy(Object actor)
		{
			return actor != null && this.IsReservedBy(actor.GetInstanceID());
		}

		// Token: 0x06005184 RID: 20868 RVA: 0x001F9F8B File Offset: 0x001F818B
		public bool IsReservedBy(int actorId)
		{
			return actorId != 0 && this.OwnerId == actorId;
		}

		// Token: 0x06005185 RID: 20869 RVA: 0x001F9F9B File Offset: 0x001F819B
		public bool Occupy(Object actor)
		{
			return actor != null && this.Occupy(actor.GetInstanceID());
		}

		// Token: 0x06005186 RID: 20870 RVA: 0x001F9FB4 File Offset: 0x001F81B4
		public bool Occupy(int actorId)
		{
			if (!this.IsReservedBy(actorId) || !base.isActiveAndEnabled || this.HasStaticOccupant || this.HasNearbyActor(actorId))
			{
				return false;
			}
			this.occupied = true;
			return true;
		}

		// Token: 0x06005187 RID: 20871 RVA: 0x001F9FE2 File Offset: 0x001F81E2
		public void Release(Object actor)
		{
			if (actor != null)
			{
				this.Release(actor.GetInstanceID());
			}
		}

		// Token: 0x06005188 RID: 20872 RVA: 0x001F9FF9 File Offset: 0x001F81F9
		public void Release(int actorId)
		{
			if (actorId != 0 && this.ownerId == actorId)
			{
				this.ClearOwner();
			}
		}

		// Token: 0x06005189 RID: 20873 RVA: 0x001FA010 File Offset: 0x001F8210
		private void PruneOwner()
		{
			if (!this.tracksOwner)
			{
				return;
			}
			if (!(this.owner == null))
			{
				GameObject gameObject = this.owner as GameObject;
				if (gameObject == null || gameObject.activeInHierarchy)
				{
					Component component = this.owner as Component;
					if (component == null || component.gameObject.activeInHierarchy)
					{
						return;
					}
				}
			}
			this.ClearOwner();
		}

		// Token: 0x0600518A RID: 20874 RVA: 0x001FA06D File Offset: 0x001F826D
		private void ClearOwner()
		{
			this.ownerId = 0;
			this.owner = null;
			this.tracksOwner = false;
			this.occupied = false;
		}

		// Token: 0x0600518B RID: 20875 RVA: 0x001FA08C File Offset: 0x001F828C
		private bool HasNearbyActor(int exceptId)
		{
			int num = Physics.OverlapSphereNonAlloc(this.SeatPosition + Vector3.up * 0.6f, this.occupantRadius + 0.35f, this.overlap, -1, QueryTriggerInteraction.Collide);
			if (num >= this.overlap.Length)
			{
				return true;
			}
			for (int i = 0; i < num; i++)
			{
				Collider collider = this.overlap[i];
				if (!(collider == null))
				{
					NPCSenses componentInParent = collider.GetComponentInParent<NPCSenses>();
					if (!(componentInParent == null) && componentInParent.gameObject.GetInstanceID() != exceptId)
					{
						Vector3 vector = componentInParent.transform.position - this.SeatPosition;
						if (Mathf.Abs(vector.y) < 0.85f && new Vector2(vector.x, vector.z).sqrMagnitude < this.occupantRadius * this.occupantRadius)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600518C RID: 20876 RVA: 0x001FA178 File Offset: 0x001F8378
		public bool TryGetExitPose(Transform actor, out Vector3 position, out Quaternion rotation, bool ignoreNpcs = false)
		{
			rotation = this.SeatRotation;
			position = this.ApproachPosition;
			Vector3 vector = rotation * Vector3.right;
			Vector3 vector2 = rotation * Vector3.forward;
			for (int i = 0; i < 5; i++)
			{
				Vector3 vector3 = this.ApproachPosition;
				if (i == 1)
				{
					vector3 += vector * 0.55f;
				}
				else if (i == 2)
				{
					vector3 -= vector * 0.55f;
				}
				else if (i == 3)
				{
					vector3 += vector2 * 0.45f;
				}
				else if (i == 4)
				{
					vector3 += vector2 * 0.85f;
				}
				if (this.HasStandingClearance(vector3, actor, ignoreNpcs))
				{
					Vector3 exitProbeOrigin = this.GetExitProbeOrigin(this.SeatPosition, 0.65f, 0f);
					Vector3 vector4 = vector3 + Vector3.up * 0.65f - exitProbeOrigin;
					int num = this.CastExitRay(exitProbeOrigin, vector4.normalized, vector4.magnitude);
					bool flag = false;
					for (int j = 0; j < num; j++)
					{
						if (!BenchSeat.ShouldIgnoreExitCollider(this.exitHits[j].collider, actor, ignoreNpcs))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						num = this.CastExitRay(vector3 + Vector3.up * 0.35f, Vector3.down, 0.75f);
						int num2 = -1;
						for (int k = 0; k < num; k++)
						{
							if (!BenchSeat.ShouldIgnoreExitCollider(this.exitHits[k].collider, actor, ignoreNpcs) && (num2 < 0 || this.exitHits[k].distance < this.exitHits[num2].distance))
							{
								num2 = k;
							}
						}
						if (num2 >= 0 && this.exitHits[num2].normal.y >= 0.65f)
						{
							vector3.y = this.exitHits[num2].point.y + 0.015f;
							position = vector3;
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600518D RID: 20877 RVA: 0x001FA3A8 File Offset: 0x001F85A8
		public bool HasStandingClearance(Vector3 position, Transform actor, bool ignoreNpcs = false)
		{
			int num;
			while ((num = Physics.OverlapCapsuleNonAlloc(position + Vector3.up * 0.31f, position + Vector3.up * 1.42f, 0.23f, this.overlap, this.clearanceLayers, QueryTriggerInteraction.Ignore)) == this.overlap.Length)
			{
				Array.Resize<Collider>(ref this.overlap, this.overlap.Length * 2);
			}
			for (int i = 0; i < num; i++)
			{
				if (!BenchSeat.ShouldIgnoreExitCollider(this.overlap[i], actor, ignoreNpcs))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600518E RID: 20878 RVA: 0x001FA440 File Offset: 0x001F8640
		private int CastExitRay(Vector3 origin, Vector3 direction, float distance)
		{
			int num;
			while ((num = Physics.RaycastNonAlloc(origin, direction, this.exitHits, distance, this.clearanceLayers, QueryTriggerInteraction.Ignore)) == this.exitHits.Length)
			{
				Array.Resize<RaycastHit>(ref this.exitHits, this.exitHits.Length * 2);
			}
			return num;
		}

		// Token: 0x0600518F RID: 20879 RVA: 0x001FA48B File Offset: 0x001F868B
		internal static bool ShouldIgnoreExitCollider(Collider hit, Transform actor, bool ignoreNpcs)
		{
			return hit == null || BenchSeat.IsActorCollider(hit, actor) || (ignoreNpcs && BenchSeat.IsNpcCollider(hit));
		}

		// Token: 0x06005190 RID: 20880 RVA: 0x001FA4AC File Offset: 0x001F86AC
		internal static bool IsNpcCollider(Collider hit)
		{
			return hit != null && (hit.GetComponentInParent<NPCSenses>() != null || hit.GetComponentInParent<AStarNPCController>() != null || hit.GetComponentInParent<NPCWanderController>() != null || hit.GetComponentInParent<NPCAnimationStance>() != null);
		}

		// Token: 0x06005191 RID: 20881 RVA: 0x001FA4FC File Offset: 0x001F86FC
		public Vector3 GetExitProbeOrigin(Vector3 rootPosition, float standingHeight, float radius)
		{
			rootPosition.y = Mathf.Max(rootPosition.y + standingHeight, this.CushionPosition.y + radius + 0.04f);
			return rootPosition;
		}

		// Token: 0x06005192 RID: 20882 RVA: 0x001FA526 File Offset: 0x001F8726
		private static bool IsActorCollider(Collider hit, Transform actor)
		{
			return actor != null && hit != null && (hit.transform == actor || hit.transform.IsChildOf(actor));
		}

		// Token: 0x06005193 RID: 20883 RVA: 0x001FA558 File Offset: 0x001F8758
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = (this.HasStaticOccupant ? Color.red : new Color(0.2f, 0.8f, 1f));
			Gizmos.DrawWireSphere(this.SeatPosition + Vector3.up * 0.45f, 0.25f);
			Gizmos.DrawLine(this.SeatPosition, this.ApproachPosition);
			Gizmos.DrawWireSphere(this.ApproachPosition, 0.23f);
			Gizmos.DrawRay(this.SeatPosition, this.SeatRotation * Vector3.forward * 0.5f);
		}

		// Token: 0x04005225 RID: 21029
		private static readonly List<BenchSeat> activeSeats = new List<BenchSeat>();

		// Token: 0x04005226 RID: 21030
		[Tooltip("Character root on the floor, facing away from the backrest; not the cushion.")]
		public Transform seatAnchor;

		// Token: 0x04005227 RID: 21031
		public Transform approachPoint;

		// Token: 0x04005228 RID: 21032
		[Tooltip("Measured cushion contact, facing out from the bench. Independent of character proportions.")]
		public Transform cushionPoint;

		// Token: 0x04005229 RID: 21033
		[Min(0f)]
		public float frontEdgeDistance = 0.24f;

		// Token: 0x0400522A RID: 21034
		public BenchAnimationSet animations;

		// Token: 0x0400522B RID: 21035
		[Tooltip("Cushion-height correction applied to the animated pelvis, blended out when standing. The character root stays on the floor.")]
		[Range(-0.12f, 0.3f)]
		public float sittingHeightOffset;

		// Token: 0x0400522C RID: 21036
		[Tooltip("Protect an authored static NPC's place, even before that NPC has spawned.")]
		public bool blockedByStaticOccupant;

		// Token: 0x0400522D RID: 21037
		public Transform staticOccupant;

		// Token: 0x0400522E RID: 21038
		[Min(0.1f)]
		public float occupantRadius = 0.55f;

		// Token: 0x0400522F RID: 21039
		public LayerMask clearanceLayers = -1;

		// Token: 0x04005230 RID: 21040
		private int activeIndex = -1;

		// Token: 0x04005231 RID: 21041
		private int ownerId;

		// Token: 0x04005232 RID: 21042
		private Object owner;

		// Token: 0x04005233 RID: 21043
		private bool tracksOwner;

		// Token: 0x04005234 RID: 21044
		private bool occupied;

		// Token: 0x04005235 RID: 21045
		private Collider[] overlap = new Collider[64];

		// Token: 0x04005236 RID: 21046
		private RaycastHit[] exitHits = new RaycastHit[24];
	}
}
