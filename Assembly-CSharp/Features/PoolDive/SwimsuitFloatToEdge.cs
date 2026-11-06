using System;
using System.Collections.Generic;
using Features.Slip;
using UnityEngine;

namespace Features.PoolDive
{
	// Token: 0x02000DE0 RID: 3552
	public class SwimsuitFloatToEdge : MonoBehaviour
	{
		// Token: 0x17001150 RID: 4432
		// (get) Token: 0x0600693B RID: 26939 RVA: 0x002A1F8C File Offset: 0x002A018C
		// (set) Token: 0x0600693C RID: 26940 RVA: 0x002A1F94 File Offset: 0x002A0194
		public bool IsAnchored { get; private set; }

		// Token: 0x0600693D RID: 26941 RVA: 0x002A1F9D File Offset: 0x002A019D
		private void Awake()
		{
			if (this.slipController == null)
			{
				this.slipController = base.GetComponent<ClothSlipController>();
			}
			if (this.slipController == null)
			{
				this.slipController = base.GetComponentInParent<ClothSlipController>();
			}
		}

		// Token: 0x0600693E RID: 26942 RVA: 0x002A1FD4 File Offset: 0x002A01D4
		private void Update()
		{
			if (this.slipController == null)
			{
				return;
			}
			GameObject gameObject = this.slipController.EnvInstanceFor("School_Swimsuit");
			if (gameObject == null || !this.slipController.IsFloatingPiece("School_Swimsuit"))
			{
				if (this._tracked != null)
				{
					this.ResetTracking();
				}
				return;
			}
			if (gameObject != this._tracked)
			{
				this.BeginTracking(gameObject);
			}
			if (this.IsAnchored || !this._driving)
			{
				return;
			}
			if (Time.time < this._driftStartTime)
			{
				return;
			}
			this.DriftTowardEdge(gameObject);
		}

		// Token: 0x0600693F RID: 26943 RVA: 0x002A206C File Offset: 0x002A026C
		private void BeginTracking(GameObject env)
		{
			this._tracked = env;
			this.IsAnchored = false;
			this._driving = true;
			this._driftStartTime = Time.time + this.driftStartDelay;
			this._targetEdge = this.FindNearestEdge(env.transform.position);
			this.slipController.SuspendFloatBob();
			if (this.debugLogs)
			{
				Debug.Log("[SwimsuitFloatToEdge] Tracking '" + env.name + "'. Nearest edge: " + ((this._targetEdge != null) ? this._targetEdge.name : "NONE"), this);
			}
		}

		// Token: 0x06006940 RID: 26944 RVA: 0x002A2105 File Offset: 0x002A0305
		private void ResetTracking()
		{
			this._tracked = null;
			this._driving = false;
			this.IsAnchored = false;
			this._targetEdge = null;
		}

		// Token: 0x06006941 RID: 26945 RVA: 0x002A2124 File Offset: 0x002A0324
		private void DriftTowardEdge(GameObject env)
		{
			Vector3 position = env.transform.position;
			if (this._targetEdge == null)
			{
				this._targetEdge = this.FindNearestEdge(position);
				if (this._targetEdge == null)
				{
					this._driving = false;
					return;
				}
			}
			Vector3 position2 = this._targetEdge.position;
			Vector2 vector = new Vector2(position.x, position.z);
			Vector2 vector2 = new Vector2(position2.x, position2.z);
			if (Vector2.Distance(vector, vector2) <= this.anchorDistance)
			{
				position.y = this.waterlineY;
				env.transform.position = position;
				this.IsAnchored = true;
				this._driving = false;
				if (this.debugLogs)
				{
					Debug.Log("[SwimsuitFloatToEdge] Anchored at edge '" + this._targetEdge.name + "'. NPCs may now pick it up.", this);
				}
				return;
			}
			Vector2 vector3 = Vector2.MoveTowards(vector, vector2, this.driftSpeed * Time.deltaTime);
			position.x = vector3.x;
			position.z = vector3.y;
			position.y = this.waterlineY + Mathf.Sin(Time.time * this.bobSpeed) * this.bobAmplitude;
			env.transform.position = position;
			Vector3 vector4 = new Vector3(vector2.x - vector.x, 0f, vector2.y - vector.y);
			if (vector4.sqrMagnitude > 0.0001f)
			{
				env.transform.rotation = Quaternion.Slerp(env.transform.rotation, Quaternion.LookRotation(vector4, Vector3.up), 2f * Time.deltaTime);
			}
		}

		// Token: 0x06006942 RID: 26946 RVA: 0x002A22C8 File Offset: 0x002A04C8
		private Transform FindNearestEdge(Vector3 from)
		{
			Transform transform = null;
			float num = float.MaxValue;
			if (this.edgePoints != null && this.edgePoints.Count > 0)
			{
				foreach (Transform transform2 in this.edgePoints)
				{
					if (!(transform2 == null))
					{
						float num2 = SwimsuitFloatToEdge.SqrXZ(from, transform2.position);
						if (num2 < num)
						{
							num = num2;
							transform = transform2;
						}
					}
				}
				if (transform != null)
				{
					return transform;
				}
			}
			foreach (MonoBehaviour monoBehaviour in Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
			{
				if (!(monoBehaviour == null) && !(monoBehaviour.GetType().Name != this.edgeComponentName))
				{
					float num3 = SwimsuitFloatToEdge.SqrXZ(from, monoBehaviour.transform.position);
					if (num3 < num)
					{
						num = num3;
						transform = monoBehaviour.transform;
					}
				}
			}
			return transform;
		}

		// Token: 0x06006943 RID: 26947 RVA: 0x002A23C8 File Offset: 0x002A05C8
		private static float SqrXZ(Vector3 a, Vector3 b)
		{
			float num = a.x - b.x;
			float num2 = a.z - b.z;
			return num * num + num2 * num2;
		}

		// Token: 0x04006BE7 RID: 27623
		[Header("Refs")]
		[SerializeField]
		private ClothSlipController slipController;

		// Token: 0x04006BE8 RID: 27624
		[Header("Edge targets")]
		[Tooltip("Explicit edge points the suit can drift to. If empty, every ClimbUpInteractable in the scene is used (the pool's PoolClimbPoints/ClimbUp* nodes).")]
		[SerializeField]
		private List<Transform> edgePoints = new List<Transform>();

		// Token: 0x04006BE9 RID: 27625
		[Tooltip("Component name auto-discovered as an edge when no explicit edgePoints are set.")]
		[SerializeField]
		private string edgeComponentName = "ClimbUpInteractable";

		// Token: 0x04006BEA RID: 27626
		[Header("Drift")]
		[Tooltip("Horizontal drift speed toward the edge (m/s). Kept slow so the suit visibly floats in.")]
		[SerializeField]
		private float driftSpeed = 0.35f;

		// Token: 0x04006BEB RID: 27627
		[Tooltip("Delay after the suit surfaces before it starts drifting (it lingers mid-pool first).")]
		[SerializeField]
		private float driftStartDelay = 1.5f;

		// Token: 0x04006BEC RID: 27628
		[Tooltip("XZ distance from the edge point at which the suit anchors and stops. Larger = it settles further from the rim (stays in the water just short of the wall instead of drifting into it).")]
		[SerializeField]
		private float anchorDistance = 1.4f;

		// Token: 0x04006BED RID: 27629
		[Tooltip("World Y the floating suit rides at (pool waterline). Matches SwimsuitSlipController.waterlineY.")]
		[SerializeField]
		private float waterlineY = -5.66f;

		// Token: 0x04006BEE RID: 27630
		[Header("Bob")]
		[SerializeField]
		private float bobAmplitude = 0.06f;

		// Token: 0x04006BEF RID: 27631
		[SerializeField]
		private float bobSpeed = 1.2f;

		// Token: 0x04006BF0 RID: 27632
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04006BF2 RID: 27634
		private GameObject _tracked;

		// Token: 0x04006BF3 RID: 27635
		private bool _driving;

		// Token: 0x04006BF4 RID: 27636
		private float _driftStartTime;

		// Token: 0x04006BF5 RID: 27637
		private Transform _targetEdge;
	}
}
