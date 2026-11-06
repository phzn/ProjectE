using System;
using Features.Detection;
using Features.NPC.StateMachine;
using Invector.vCharacterController;
using UnityEngine;

namespace Features.Inventory.Placement
{
	// Token: 0x0200114C RID: 4428
	public class PlacementSurfaceDetector : MonoBehaviour
	{
		// Token: 0x17001555 RID: 5461
		// (get) Token: 0x0600868B RID: 34443 RVA: 0x0036F322 File Offset: 0x0036D522
		public float WallPlacementHeightFromFloor
		{
			get
			{
				return this.wallPlacementHeightFromFloor;
			}
		}

		// Token: 0x0600868C RID: 34444 RVA: 0x0036F32C File Offset: 0x0036D52C
		public PlacementResult DetectSurface(Camera cam, LayerMask mask, float maxDistance, Transform ignoreRoot = null, Vector3? originOverride = null, Vector3? directionOverride = null)
		{
			if (cam == null)
			{
				return PlacementResult.Invalid;
			}
			Vector3 vector = ((directionOverride != null) ? directionOverride.Value.normalized : cam.transform.forward);
			if (vector.sqrMagnitude < 0.001f)
			{
				return PlacementResult.Invalid;
			}
			Ray ray;
			if (originOverride != null)
			{
				ray = new Ray(originOverride.Value, vector);
			}
			else if (this.useEyeOrigin || (directionOverride != null && ignoreRoot != null))
			{
				Vector3 vector2 = ((ignoreRoot != null) ? ignoreRoot.position : base.transform.position) + Vector3.up * this.eyeHeight;
				vector2 += vector * this.eyeForwardOffset;
				ray = new Ray(vector2, vector);
			}
			else
			{
				ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
			}
			RaycastHit raycastHit;
			if (!this.TryGetFirstValidHit(ray, mask, maxDistance, ignoreRoot, out raycastHit))
			{
				if (this.debugLogs)
				{
					Debug.Log("[PlacementSurfaceDetector] No hit.");
				}
				return PlacementResult.Invalid;
			}
			this._lastHitPosition = raycastHit.point;
			if (ignoreRoot != null && (raycastHit.point - ignoreRoot.position).sqrMagnitude < this.minPlacementDistance * this.minPlacementDistance)
			{
				if (this.debugLogs)
				{
					Debug.Log("[PlacementSurfaceDetector] Hit too close to player, ignoring.");
				}
				return PlacementResult.Invalid;
			}
			if (this.useConfirmationRay && !this.ValidateSurfaceConsistency(raycastHit, mask, ignoreRoot))
			{
				if (this.debugLogs)
				{
					Debug.Log("[PlacementSurfaceDetector] Surface failed consistency check.");
				}
				return PlacementResult.Invalid;
			}
			PlacementResult placementResult;
			if (raycastHit.normal.y < this.wallNormalThreshold && raycastHit.normal.y > -this.wallNormalThreshold)
			{
				placementResult = this.BuildWallResult(raycastHit, ignoreRoot);
			}
			else
			{
				placementResult = this.BuildFloorResult(raycastHit);
			}
			if (this.useGridSnap && placementResult.IsValid && this.gridSize > 0.01f)
			{
				placementResult.Position = new Vector3(Mathf.Round(placementResult.Position.x / this.gridSize) * this.gridSize, placementResult.Position.y, Mathf.Round(placementResult.Position.z / this.gridSize) * this.gridSize);
			}
			return placementResult;
		}

		// Token: 0x0600868D RID: 34445 RVA: 0x0036F58C File Offset: 0x0036D78C
		private bool TryGetFirstValidHit(Ray ray, LayerMask mask, float maxDistance, Transform ignoreRoot, out RaycastHit bestHit)
		{
			bestHit = default(RaycastHit);
			int num = Physics.RaycastNonAlloc(ray, this._surfaceHitsBuffer, maxDistance, mask, QueryTriggerInteraction.Ignore);
			if (num <= 0)
			{
				return false;
			}
			bool flag = false;
			float num2 = float.MaxValue;
			for (int i = 0; i < num; i++)
			{
				RaycastHit raycastHit = this._surfaceHitsBuffer[i];
				if (!(raycastHit.collider == null) && (!(ignoreRoot != null) || !raycastHit.collider.transform.IsChildOf(ignoreRoot)) && raycastHit.distance < num2)
				{
					num2 = raycastHit.distance;
					bestHit = raycastHit;
					flag = true;
				}
			}
			return flag && !PlacementSurfaceDetector.IsCharacterSurface(bestHit.collider);
		}

		// Token: 0x0600868E RID: 34446 RVA: 0x0036F640 File Offset: 0x0036D840
		private static bool IsCharacterSurface(Collider collider)
		{
			return !(collider == null) && (collider.GetComponentInParent<NPCSenses>(true) != null || collider.GetComponentInParent<NPCEquipmentManager>(true) != null || collider.GetComponentInParent<NPCStateMachine>(true) != null || collider.GetComponentInParent<vThirdPersonController>(true) != null);
		}

		// Token: 0x0600868F RID: 34447 RVA: 0x0036F694 File Offset: 0x0036D894
		private PlacementResult BuildFloorResult(RaycastHit hit)
		{
			Vector3 vector = hit.point + Vector3.up * this.floorPlacementYOffset;
			Vector3 vector2 = Vector3.ProjectOnPlane((Camera.main != null) ? Camera.main.transform.forward : Vector3.forward, Vector3.up).normalized;
			if (vector2 == Vector3.zero)
			{
				vector2 = Vector3.forward;
			}
			Quaternion quaternion = Quaternion.LookRotation(vector2, Vector3.up) * Quaternion.Euler(this.floorRotationOffset);
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[PlacementSurfaceDetector] Floor hit at {0}", vector));
			}
			return new PlacementResult
			{
				IsValid = true,
				SurfaceType = PlacementSurfaceType.Floor,
				Position = vector,
				Rotation = quaternion,
				HitNormal = hit.normal,
				HitCollider = hit.collider,
				SurfaceTransform = MovingPlacementSupport.Resolve(hit.collider),
				FloorPosition = hit.point,
				HitPosition = hit.point
			};
		}

		// Token: 0x06008690 RID: 34448 RVA: 0x0036F7B8 File Offset: 0x0036D9B8
		private PlacementResult BuildWallResult(RaycastHit wallHit, Transform ignoreRoot)
		{
			Quaternion quaternion = Quaternion.LookRotation(wallHit.normal, Vector3.up) * Quaternion.Euler(this.wallRotationOffset);
			Vector3 vector;
			if (!this.TryFindFloorBelow(wallHit.point, wallHit.normal, out vector, ignoreRoot))
			{
				if (this.debugLogs)
				{
					Debug.Log("[PlacementSurfaceDetector] Wall hit but no floor detected below — invalid.");
				}
				return PlacementResult.Invalid;
			}
			this._lastFloorPosition = vector;
			Transform transform = MovingPlacementSupport.Resolve(wallHit.collider);
			float num = ((transform != null) ? wallHit.point.y : (vector.y + this.wallPlacementHeightFromFloor));
			Vector3 vector2 = new Vector3(wallHit.point.x, num, wallHit.point.z) + wallHit.normal * this.wallPlacementForwardOffset;
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[PlacementSurfaceDetector] Wall hit. Floor at y={0:F2}, placed at y={1:F2}", vector.y, num));
			}
			return new PlacementResult
			{
				IsValid = true,
				SurfaceType = PlacementSurfaceType.Wall,
				Position = vector2,
				Rotation = quaternion,
				HitNormal = wallHit.normal,
				HitCollider = wallHit.collider,
				SurfaceTransform = transform,
				FloorPosition = vector,
				HitPosition = wallHit.point
			};
		}

		// Token: 0x06008691 RID: 34449 RVA: 0x0036F915 File Offset: 0x0036DB15
		private bool TryFindFloorBelow(Vector3 wallHitPoint, out Vector3 floorPosition)
		{
			return this.TryFindFloorBelow(wallHitPoint, Vector3.zero, out floorPosition, null);
		}

		// Token: 0x06008692 RID: 34450 RVA: 0x0036F928 File Offset: 0x0036DB28
		private bool TryFindFloorBelow(Vector3 wallHitPoint, Vector3 wallNormal, out Vector3 floorPosition, Transform ignoreRoot = null)
		{
			floorPosition = Vector3.zero;
			Vector3 vector = wallHitPoint + Vector3.up * 0.05f;
			if (wallNormal != Vector3.zero && this.wallFloorCheckDepthOffset > 0f)
			{
				vector += wallNormal * this.wallFloorCheckDepthOffset;
			}
			Ray ray = new Ray(vector, Vector3.down);
			RaycastHit raycastHit;
			if (this.TryGetFirstValidHit(ray, -1, this.maxFloorDetectionDistance + 0.05f, ignoreRoot, out raycastHit) && raycastHit.normal.y >= this.wallNormalThreshold)
			{
				floorPosition = raycastHit.point;
				return true;
			}
			return false;
		}

		// Token: 0x06008693 RID: 34451 RVA: 0x0036F9D4 File Offset: 0x0036DBD4
		private bool ValidateSurfaceConsistency(RaycastHit primaryHit, LayerMask mask, Transform ignoreRoot)
		{
			Vector3 normal = primaryHit.normal;
			Vector3 vector = Vector3.Cross(normal, Vector3.forward);
			if (vector.sqrMagnitude < 0.001f)
			{
				vector = Vector3.Cross(normal, Vector3.right);
			}
			vector.Normalize();
			float num = Mathf.Cos(this.maxNormalDeviation * 0.017453292f);
			Vector3 normalized = Vector3.Cross(vector, normal).normalized;
			foreach (Vector3 vector2 in new Vector3[]
			{
				vector,
				-vector,
				normalized,
				-normalized
			})
			{
				Vector3 vector3 = primaryHit.point + normal * 0.1f + vector2 * this.confirmationRadius;
				RaycastHit raycastHit;
				if (this.TryGetFirstValidHit(new Ray(vector3, -normal), mask, 0.3f, ignoreRoot, out raycastHit))
				{
					if (Vector3.Dot(raycastHit.normal, primaryHit.normal) < num)
					{
						return false;
					}
				}
				else if (PlacementSurfaceDetector.IsCharacterSurface(raycastHit.collider))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06008694 RID: 34452 RVA: 0x0036FB04 File Offset: 0x0036DD04
		private void OnValidate()
		{
			this.wallNormalThreshold = Mathf.Clamp(this.wallNormalThreshold, 0f, 1f);
			this.wallPlacementHeightFromFloor = Mathf.Max(0f, this.wallPlacementHeightFromFloor);
			this.maxFloorDetectionDistance = Mathf.Max(0.1f, this.maxFloorDetectionDistance);
			this.wallPlacementForwardOffset = Mathf.Max(0f, this.wallPlacementForwardOffset);
			this.minPlacementDistance = Mathf.Max(0f, this.minPlacementDistance);
			this.gridSize = Mathf.Max(0.01f, this.gridSize);
			this.confirmationRadius = Mathf.Max(0.01f, this.confirmationRadius);
			this.maxNormalDeviation = Mathf.Clamp(this.maxNormalDeviation, 1f, 90f);
		}

		// Token: 0x06008695 RID: 34453 RVA: 0x0036FBCB File Offset: 0x0036DDCB
		private void OnDrawGizmos()
		{
			if (!this.drawGizmos)
			{
				return;
			}
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(this._lastHitPosition, 0.05f);
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(this._lastFloorPosition, 0.08f);
		}

		// Token: 0x04008932 RID: 35122
		private readonly RaycastHit[] _surfaceHitsBuffer = new RaycastHit[32];

		// Token: 0x04008933 RID: 35123
		[Header("Surface Classification")]
		[Tooltip("If hit normal Y is below this value, surface is classified as Wall (0.3 = ~72 deg from horizontal)")]
		[SerializeField]
		[Range(0f, 1f)]
		private float wallNormalThreshold = 0.3f;

		// Token: 0x04008934 RID: 35124
		[Header("Wall Height Constraint")]
		[Tooltip("Fixed height above the floor where wall-placed objects are anchored")]
		[SerializeField]
		private float wallPlacementHeightFromFloor = 0.85f;

		// Token: 0x04008935 RID: 35125
		[Tooltip("Max distance of the downward floor-detection raycast from the wall hit point")]
		[SerializeField]
		private float maxFloorDetectionDistance = 5f;

		// Token: 0x04008936 RID: 35126
		[Tooltip("Small forward offset to prevent z-fighting with wall surface")]
		[SerializeField]
		private float wallPlacementForwardOffset = 0.02f;

		// Token: 0x04008937 RID: 35127
		[Tooltip("Depth offset inward from wall surface for floor detection ray (avoids edges at wall base)")]
		[SerializeField]
		private float wallFloorCheckDepthOffset = 0.4f;

		// Token: 0x04008938 RID: 35128
		[Header("Floor Placement")]
		[Tooltip("Y offset applied on top of the floor hit point")]
		[SerializeField]
		private float floorPlacementYOffset;

		// Token: 0x04008939 RID: 35129
		[Tooltip("Additional Euler rotation applied to floor-placed objects. Tune if mesh axis differs from convention.")]
		[SerializeField]
		private Vector3 floorRotationOffset = Vector3.zero;

		// Token: 0x0400893A RID: 35130
		[Header("Wall Placement")]
		[Tooltip("Additional Euler rotation applied to wall-placed objects. Tune if mesh axis differs from convention.")]
		[SerializeField]
		private Vector3 wallRotationOffset = Vector3.zero;

		// Token: 0x0400893B RID: 35131
		[Header("Third Person Adjustments")]
		[Tooltip("If true, raycast starts from character eye origin but follows camera direction to avoid camera occlusion.")]
		[SerializeField]
		private bool useEyeOrigin = true;

		// Token: 0x0400893C RID: 35132
		[Tooltip("Height of character eyes relative to character root")]
		[SerializeField]
		private float eyeHeight = 1.6f;

		// Token: 0x0400893D RID: 35133
		[Tooltip("Small forward offset from eye origin to avoid self-collision with character")]
		[SerializeField]
		private float eyeForwardOffset = 0.5f;

		// Token: 0x0400893E RID: 35134
		[Header("Quality")]
		[Tooltip("Minimum distance from player root to accept a hit (prevents placing at feet)")]
		[SerializeField]
		private float minPlacementDistance = 0.6f;

		// Token: 0x0400893F RID: 35135
		[Tooltip("If true, snaps placement position to a grid for cleaner results")]
		[SerializeField]
		private bool useGridSnap;

		// Token: 0x04008940 RID: 35136
		[Tooltip("Grid cell size in world units (only if useGridSnap is true)")]
		[SerializeField]
		private float gridSize = 0.25f;

		// Token: 0x04008941 RID: 35137
		[Tooltip("Run a secondary confirmation raycast to validate surface flatness")]
		[SerializeField]
		private bool useConfirmationRay = true;

		// Token: 0x04008942 RID: 35138
		[Tooltip("Offset radius for confirmation rays around the hit point")]
		[SerializeField]
		private float confirmationRadius = 0.15f;

		// Token: 0x04008943 RID: 35139
		[Tooltip("Max normal deviation (degrees) between primary and confirmation hits for surface to be valid")]
		[SerializeField]
		private float maxNormalDeviation = 25f;

		// Token: 0x04008944 RID: 35140
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04008945 RID: 35141
		[SerializeField]
		private bool drawGizmos;

		// Token: 0x04008946 RID: 35142
		private Vector3 _lastFloorPosition;

		// Token: 0x04008947 RID: 35143
		private Vector3 _lastHitPosition;
	}
}
