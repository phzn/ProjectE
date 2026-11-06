using System;
using System.Collections;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Invector.vCharacterController
{
	// Token: 0x02000686 RID: 1670
	[SelectionBase]
	[RequireComponent(typeof(BoxCollider))]
	[vClassHeader("Cover Point", true, "icon_v2", false, "", openClose = false)]
	public class vCoverPoint : vMonoBehaviour
	{
		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x0600273D RID: 10045 RVA: 0x000DFEB2 File Offset: 0x000DE0B2
		private float space
		{
			get
			{
				return this.boxCollider.size.z;
			}
		}

		// Token: 0x0600273E RID: 10046 RVA: 0x000DFEC4 File Offset: 0x000DE0C4
		private void Awake()
		{
			if (this.boxCollider)
			{
				this.boxCollider.isTrigger = true;
			}
		}

		// Token: 0x0600273F RID: 10047 RVA: 0x000DFEDF File Offset: 0x000DE0DF
		private IEnumerator Start()
		{
			yield return new WaitForEndOfFrame();
			if (this.manualConnections)
			{
				this.isValid = true;
			}
			else
			{
				this.isValid = this.CheckPosePositionValid();
			}
			base.gameObject.SetActive(this.isValid);
			this.CheckConnections();
			yield break;
		}

		// Token: 0x06002740 RID: 10048 RVA: 0x000DFEEE File Offset: 0x000DE0EE
		public void ClearConnections()
		{
			this.left = null;
			this.right = null;
			this.leftCorner = null;
			this.rightCorner = null;
			this.corner = (vCoverPoint.Corner)0;
		}

		// Token: 0x06002741 RID: 10049 RVA: 0x000DFF14 File Offset: 0x000DE114
		public virtual bool CheckPosePositionValid()
		{
			if (!(AstarPath.active != null))
			{
				NavMeshHit navMeshHit;
				NavMesh.SamplePosition(this.posePosition, out navMeshHit, 0.1f, -1);
				return true;
			}
			if (!((AStarGraphManager.Instance != null) ? AStarGraphManager.Instance.IsReady : (AstarPath.active.data.graphs != null && AstarPath.active.data.graphs.Length != 0 && !AstarPath.active.isScanning)))
			{
				return true;
			}
			NNInfo nearest = AstarPath.active.GetNearest(this.posePosition, NearestNodeConstraint.Walkable);
			return nearest.node != null && Vector3.Distance(nearest.position, this.posePosition) < 1f;
		}

		// Token: 0x06002742 RID: 10050 RVA: 0x000DFFD0 File Offset: 0x000DE1D0
		public void CheckConnections()
		{
			if (this.manualConnections)
			{
				return;
			}
			if (!this.boxCollider.enabled)
			{
				return;
			}
			if (this.autoDetectCorner)
			{
				this.corner = (vCoverPoint.Corner)0;
			}
			this.mask = 1 | (1 << base.gameObject.layer);
			Scene scene = base.gameObject.scene;
			this.physics = base.gameObject.scene.GetPhysicsScene();
			this.CheckLeftConnection();
			this.CheckRightConnection();
		}

		// Token: 0x06002743 RID: 10051 RVA: 0x000E0050 File Offset: 0x000DE250
		private void CheckRightConnection()
		{
			vCoverPoint vCoverPoint = null;
			if (this.RayCastConnections(base.transform.position + base.transform.TransformDirection(this.boxCollider.center) + base.transform.right * this.boxCollider.size.x * this.rayCastNeighborOffsetX, -base.transform.right, out this.hit, this.boxCollider.size.x * (1.1f + this.rayCastNeighborOffsetX), this.mask))
			{
				vCoverPoint = this.hit.transform.gameObject.GetComponent<vCoverPoint>();
			}
			else
			{
				for (int i = 0; i < 6; i++)
				{
					Vector3 vector = Quaternion.AngleAxis((float)(i * 15), Vector3.up) * -base.transform.right;
					Vector3 vector2 = this.boxCollider.bounds.center - base.transform.right * this.boxCollider.size.x * 0.45f;
					if (this.RayCastConnections(vector2, vector, out this.hit, this.space, this.mask))
					{
						vCoverPoint = this.hit.transform.gameObject.GetComponent<vCoverPoint>();
						if (this.showRays)
						{
							Debug.DrawLine(vector2, this.hit.point, Color.red);
						}
						if (vCoverPoint)
						{
							break;
						}
					}
					else if (this.showRays)
					{
						Debug.DrawRay(vector2, vector * this.space, Color.green);
					}
				}
				if (!vCoverPoint)
				{
					for (int j = 0; j < 6; j++)
					{
						Vector3 vector3 = Quaternion.AngleAxis((float)(-(float)(j * 15)), Vector3.up) * -base.transform.right;
						Vector3 vector4 = this.boxCollider.bounds.center - base.transform.right * this.boxCollider.size.x * 0.45f;
						if (this.RayCastConnections(vector4, vector3, out this.hit, this.space, this.mask))
						{
							vCoverPoint = this.hit.transform.gameObject.GetComponent<vCoverPoint>();
							if (this.showRays)
							{
								Debug.DrawLine(vector4, this.hit.point, Color.red);
							}
							if (vCoverPoint)
							{
								break;
							}
						}
						else if (this.showRays)
						{
							Debug.DrawRay(vector4, vector3 * this.space, Color.yellow);
						}
					}
				}
			}
			bool flag;
			if (vCoverPoint)
			{
				Vector3 vector5 = (vCoverPoint.transform.eulerAngles - base.transform.eulerAngles).NormalizeAngle();
				this.right = ((vector5.y > -75f) ? vCoverPoint : null);
				flag = this.right == null;
				this.rightCorner = ((this.right == null) ? vCoverPoint : null);
			}
			else
			{
				this.right = null;
				this.rightCorner = null;
				flag = true;
			}
			if (flag && this.autoDetectCorner)
			{
				this.corner |= vCoverPoint.Corner.Right;
			}
		}

		// Token: 0x06002744 RID: 10052 RVA: 0x000E03AC File Offset: 0x000DE5AC
		private void CheckLeftConnection()
		{
			vCoverPoint vCoverPoint = null;
			if (this.RayCastConnections(base.transform.position + base.transform.TransformDirection(this.boxCollider.center) - base.transform.right * this.boxCollider.size.x * this.rayCastNeighborOffsetX, base.transform.right, out this.hit, this.boxCollider.size.x * (1.1f + this.rayCastNeighborOffsetX), this.mask))
			{
				vCoverPoint = this.hit.transform.gameObject.GetComponent<vCoverPoint>();
			}
			else
			{
				for (int i = 0; i < 6; i++)
				{
					Vector3 vector = Quaternion.AngleAxis((float)(-(float)(i * 15)), Vector3.up) * base.transform.right;
					Vector3 vector2 = this.boxCollider.bounds.center + base.transform.right * this.boxCollider.size.x * 0.45f;
					if (this.RayCastConnections(vector2, vector, out this.hit, this.space, this.mask))
					{
						vCoverPoint = this.hit.transform.gameObject.GetComponent<vCoverPoint>();
						if (this.showRays)
						{
							Debug.DrawLine(vector2, this.hit.point, Color.red);
						}
						if (vCoverPoint)
						{
							break;
						}
					}
					else if (this.showRays)
					{
						Debug.DrawRay(vector2, vector * this.space, Color.green);
					}
				}
				if (!vCoverPoint)
				{
					for (int j = 0; j < 6; j++)
					{
						Vector3 vector3 = Quaternion.AngleAxis((float)(j * 15), Vector3.up) * base.transform.right;
						Vector3 vector4 = this.boxCollider.bounds.center + base.transform.right * this.boxCollider.size.x * 0.45f;
						if (this.RayCastConnections(vector4, vector3, out this.hit, this.space, this.mask))
						{
							vCoverPoint = this.hit.transform.gameObject.GetComponent<vCoverPoint>();
							if (this.showRays)
							{
								Debug.DrawLine(vector4, this.hit.point, Color.red);
							}
							if (vCoverPoint)
							{
								break;
							}
						}
						else if (this.showRays)
						{
							Debug.DrawRay(vector4, vector3 * this.space, Color.yellow);
						}
					}
				}
			}
			bool flag;
			if (vCoverPoint)
			{
				Vector3 vector5 = (vCoverPoint.transform.eulerAngles - base.transform.eulerAngles).NormalizeAngle();
				this.left = ((vector5.y < 75f) ? vCoverPoint : null);
				flag = this.left == null;
				this.leftCorner = ((this.left == null) ? vCoverPoint : null);
			}
			else
			{
				this.left = null;
				this.leftCorner = null;
				flag = true;
			}
			if (flag && this.autoDetectCorner)
			{
				this.corner |= vCoverPoint.Corner.Left;
			}
		}

		// Token: 0x06002745 RID: 10053 RVA: 0x000E06F8 File Offset: 0x000DE8F8
		private void OnDrawGizmos()
		{
			if (!Application.isPlaying)
			{
				this.isValid = true;
			}
			this.CheckConnections();
			Matrix4x4 matrix = Gizmos.matrix;
			Gizmos.color = Color.white * 0.1f;
			Gizmos.DrawSphere(this.posePosition, 0.1f);
			Gizmos.matrix = Matrix4x4.TRS(base.transform.position + base.transform.TransformDirection(this.boxCollider.center) + base.transform.forward * this.posePositionZ * 0.45f, base.transform.rotation, new Vector3(this.boxCollider.BoxSize().x, this.boxCollider.BoxSize().y, 0.01f));
			Gizmos.color = ((this.isValid && this.boxCollider.enabled) ? (Color.white * 0.8f) : (Color.red * 0.8f));
			Gizmos.DrawWireCube(Vector3.zero, new Vector3(1f, 1f, 0.2f));
			Gizmos.color = (this.isValid ? (Color.white * 0.25f) : (Color.red * 0.25f));
			Gizmos.DrawCube(Vector3.zero, Vector3.one);
			Gizmos.matrix = matrix;
		}

		// Token: 0x06002746 RID: 10054 RVA: 0x000E0868 File Offset: 0x000DEA68
		private void OnDrawGizmosSelected()
		{
			float num = 1.5f;
			float posePositionZ = this.posePositionZ;
			if (this.peekLeftRangeStart != null && this.peekLeftRangeEnd != null)
			{
				Gizmos.color = new Color(0f, 1f, 1f, 0.3f);
				Vector3 vector = this.peekLeftRangeStart.position + Vector3.up * num * 0.5f;
				Vector3 vector2 = this.peekLeftRangeEnd.position + Vector3.up * num * 0.5f;
				Vector3 vector3 = (vector + vector2) * 0.5f;
				float num2 = Vector3.Distance(vector, vector2);
				Matrix4x4 matrix = Gizmos.matrix;
				Gizmos.matrix = Matrix4x4.TRS(vector3, base.transform.rotation, Vector3.one);
				Gizmos.DrawCube(Vector3.zero, new Vector3(num2, num, posePositionZ * 0.5f));
				Gizmos.color = Color.cyan;
				Gizmos.DrawWireCube(Vector3.zero, new Vector3(num2, num, posePositionZ * 0.5f));
				Gizmos.matrix = matrix;
				Gizmos.color = Color.cyan;
				Gizmos.DrawSphere(this.peekLeftRangeStart.position, 0.1f);
				Gizmos.DrawSphere(this.peekLeftRangeEnd.position, 0.1f);
			}
			if (this.peekRightRangeStart != null && this.peekRightRangeEnd != null)
			{
				Gizmos.color = new Color(1f, 0f, 1f, 0.3f);
				Vector3 vector4 = this.peekRightRangeStart.position + Vector3.up * num * 0.5f;
				Vector3 vector5 = this.peekRightRangeEnd.position + Vector3.up * num * 0.5f;
				Vector3 vector6 = (vector4 + vector5) * 0.5f;
				float num3 = Vector3.Distance(vector4, vector5);
				Matrix4x4 matrix2 = Gizmos.matrix;
				Gizmos.matrix = Matrix4x4.TRS(vector6, base.transform.rotation, Vector3.one);
				Gizmos.DrawCube(Vector3.zero, new Vector3(num3, num, posePositionZ * 0.5f));
				Gizmos.color = Color.magenta;
				Gizmos.DrawWireCube(Vector3.zero, new Vector3(num3, num, posePositionZ * 0.5f));
				Gizmos.matrix = matrix2;
				Gizmos.color = Color.magenta;
				Gizmos.DrawSphere(this.peekRightRangeStart.position, 0.1f);
				Gizmos.DrawSphere(this.peekRightRangeEnd.position, 0.1f);
			}
		}

		// Token: 0x06002747 RID: 10055 RVA: 0x000E0AFB File Offset: 0x000DECFB
		protected virtual bool RayCastConnections(Vector3 rayOrigin, Vector3 dir, out RaycastHit hit, float distance, LayerMask mask)
		{
			if (Application.isPlaying)
			{
				return Physics.Raycast(rayOrigin, dir, out hit, distance, mask, QueryTriggerInteraction.Collide);
			}
			return this.physics.Raycast(rayOrigin, dir, out hit, distance, mask, QueryTriggerInteraction.Collide);
		}

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x06002748 RID: 10056 RVA: 0x000E0B2F File Offset: 0x000DED2F
		public float posePositionZ
		{
			get
			{
				if (!this.boxCollider)
				{
					return 0f;
				}
				return this.boxCollider.size.z;
			}
		}

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x06002749 RID: 10057 RVA: 0x000E0B54 File Offset: 0x000DED54
		// (set) Token: 0x0600274A RID: 10058 RVA: 0x000E0BA0 File Offset: 0x000DEDA0
		public BoxCollider boxCollider
		{
			get
			{
				if (this._boxCollider == null)
				{
					this._boxCollider = base.GetComponent<BoxCollider>();
				}
				if (this._boxCollider == null)
				{
					this._boxCollider = base.gameObject.AddComponent<BoxCollider>();
				}
				return this._boxCollider;
			}
			set
			{
				this._boxCollider = value;
			}
		}

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x0600274B RID: 10059 RVA: 0x000E0BAC File Offset: 0x000DEDAC
		public Vector3 posePosition
		{
			get
			{
				return base.transform.position + base.transform.forward * this.posePositionZ + base.transform.right * this.offsetPosePositionX;
			}
		}

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x0600274C RID: 10060 RVA: 0x000E0BFC File Offset: 0x000DEDFC
		public Vector3 rightCornerP
		{
			get
			{
				Vector3[] array = new Vector3[2];
				Vector3 posePosition = this.posePosition;
				array[0] = posePosition - base.transform.right * (this.boxCollider.size.z * 1.5f);
				array[1] = (this.rightCorner ? (this.rightCorner.posePosition + this.rightCorner.transform.right * (this.rightCorner.boxCollider.size.z * 1.5f)) : array[0]);
				return (array[0] + array[1]) / 2f;
			}
		}

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x0600274D RID: 10061 RVA: 0x000E0CC4 File Offset: 0x000DEEC4
		public Vector3 leftCornerP
		{
			get
			{
				Vector3[] array = new Vector3[2];
				Vector3 posePosition = this.posePosition;
				array[0] = posePosition + base.transform.right * (this.boxCollider.size.z * 1.5f);
				array[1] = (this.leftCorner ? (this.leftCorner.posePosition - this.leftCorner.transform.right * (this.leftCorner.boxCollider.size.z * 1.5f)) : array[0]);
				return (array[0] + array[1]) / 2f;
			}
		}

		// Token: 0x0600274E RID: 10062 RVA: 0x000E0D8C File Offset: 0x000DEF8C
		public void EnterCover(GameObject visitor)
		{
			this.onEnterCover.Invoke(this, visitor);
		}

		// Token: 0x0600274F RID: 10063 RVA: 0x000E0D9C File Offset: 0x000DEF9C
		public bool IsInLeftPeekRange(Vector3 worldPosition)
		{
			if (this.peekLeftRangeStart == null || this.peekLeftRangeEnd == null)
			{
				return false;
			}
			float x = base.transform.InverseTransformPoint(worldPosition).x;
			float x2 = base.transform.InverseTransformPoint(this.peekLeftRangeStart.position).x;
			float x3 = base.transform.InverseTransformPoint(this.peekLeftRangeEnd.position).x;
			float num = Mathf.Min(x2, x3);
			float num2 = Mathf.Max(x2, x3);
			return x >= num && x <= num2;
		}

		// Token: 0x06002750 RID: 10064 RVA: 0x000E0E2C File Offset: 0x000DF02C
		public bool IsInRightPeekRange(Vector3 worldPosition)
		{
			if (this.peekRightRangeStart == null || this.peekRightRangeEnd == null)
			{
				return false;
			}
			float x = base.transform.InverseTransformPoint(worldPosition).x;
			float x2 = base.transform.InverseTransformPoint(this.peekRightRangeStart.position).x;
			float x3 = base.transform.InverseTransformPoint(this.peekRightRangeEnd.position).x;
			float num = Mathf.Min(x2, x3);
			float num2 = Mathf.Max(x2, x3);
			return x >= num && x <= num2;
		}

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x06002751 RID: 10065 RVA: 0x000E0EBC File Offset: 0x000DF0BC
		public bool HasLeftPeekRange
		{
			get
			{
				return this.peekLeftRangeStart != null && this.peekLeftRangeEnd != null;
			}
		}

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x06002752 RID: 10066 RVA: 0x000E0EDA File Offset: 0x000DF0DA
		public bool HasRightPeekRange
		{
			get
			{
				return this.peekRightRangeStart != null && this.peekRightRangeEnd != null;
			}
		}

		// Token: 0x040027D0 RID: 10192
		[Tooltip("Auto assign the properties : Corner enum, LeftCorner and RightCorner ")]
		public bool autoDetectCorner = true;

		// Token: 0x040027D1 RID: 10193
		[Tooltip("PROJECT EXHIBITIONIST: left/right/corner are authored by CoverWallSetup. Skip raycast re-detection (Start + gizmos), which can link to other covers or flag fake corners.")]
		public bool manualConnections;

		// Token: 0x040027D2 RID: 10194
		public LayerMask mask = 1;

		// Token: 0x040027D3 RID: 10195
		[vEnumFlag]
		public vCoverPoint.Corner corner;

		// Token: 0x040027D4 RID: 10196
		public float offsetPosePositionX;

		// Token: 0x040027D5 RID: 10197
		public float rayCastNeighborOffsetX = 0.15f;

		// Token: 0x040027D6 RID: 10198
		public vCoverPoint leftCorner;

		// Token: 0x040027D7 RID: 10199
		public vCoverPoint rightCorner;

		// Token: 0x040027D8 RID: 10200
		public vCoverPoint left;

		// Token: 0x040027D9 RID: 10201
		public vCoverPoint right;

		// Token: 0x040027DA RID: 10202
		public vCoverPoint.CoverEvent onEnterCover;

		// Token: 0x040027DB RID: 10203
		[vSeparator("Peek Range Settings", "")]
		[Tooltip("Transform marking the START of the left peek range (child of this CoverPoint). Character can peek left when between peekLeftRangeStart and peekLeftRangeEnd.")]
		public Transform peekLeftRangeStart;

		// Token: 0x040027DC RID: 10204
		[Tooltip("Transform marking the END of the left peek range (child of this CoverPoint). This should be closer to the corner.")]
		public Transform peekLeftRangeEnd;

		// Token: 0x040027DD RID: 10205
		[Tooltip("Transform marking the START of the right peek range (child of this CoverPoint). Character can peek right when between peekRightRangeStart and peekRightRangeEnd.")]
		public Transform peekRightRangeStart;

		// Token: 0x040027DE RID: 10206
		[Tooltip("Transform marking the END of the right peek range (child of this CoverPoint). This should be closer to the corner.")]
		public Transform peekRightRangeEnd;

		// Token: 0x040027DF RID: 10207
		public bool showRays;

		// Token: 0x040027E0 RID: 10208
		private RaycastHit hit;

		// Token: 0x040027E1 RID: 10209
		private PhysicsScene physics;

		// Token: 0x040027E2 RID: 10210
		public bool isValid;

		// Token: 0x040027E3 RID: 10211
		[SerializeField]
		[HideInInspector]
		protected Vector3 lastPosition;

		// Token: 0x040027E4 RID: 10212
		[SerializeField]
		[HideInInspector]
		private float lastOffsetPosePositionX;

		// Token: 0x040027E5 RID: 10213
		[SerializeField]
		[HideInInspector]
		private float lastRayCastNeighborOffsetX;

		// Token: 0x040027E6 RID: 10214
		[SerializeField]
		[HideInInspector]
		private Vector3 lastColliderCenter;

		// Token: 0x040027E7 RID: 10215
		[SerializeField]
		[HideInInspector]
		private Vector3 lastColliderSize;

		// Token: 0x040027E8 RID: 10216
		protected BoxCollider _boxCollider;

		// Token: 0x040027E9 RID: 10217
		public bool isOccuped;

		// Token: 0x02000687 RID: 1671
		[Serializable]
		public class CoverEvent : UnityEvent<vCoverPoint, GameObject>
		{
		}

		// Token: 0x02000688 RID: 1672
		[Flags]
		public enum Corner
		{
			// Token: 0x040027EB RID: 10219
			Left = 1,
			// Token: 0x040027EC RID: 10220
			Right = 2
		}
	}
}
