using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000149 RID: 329
	[AddComponentMenu("Pathfinding/Dynamic Obstacle")]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/dynamicobstacle.html")]
	public class DynamicObstacle : GraphModifier, DynamicGridObstacle
	{
		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000A9B RID: 2715 RVA: 0x000389FC File Offset: 0x00036BFC
		private Bounds bounds
		{
			get
			{
				if (this.coll != null)
				{
					return this.coll.bounds;
				}
				if (this.coll2D != null)
				{
					Bounds bounds = this.coll2D.bounds;
					bounds.extents += new Vector3(0f, 0f, 10000f);
					return bounds;
				}
				return default(Bounds);
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000A9C RID: 2716 RVA: 0x00038A6E File Offset: 0x00036C6E
		private bool colliderEnabled
		{
			get
			{
				if (!(this.coll != null))
				{
					return this.coll2D.enabled;
				}
				return this.coll.enabled;
			}
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x00038A98 File Offset: 0x00036C98
		protected override void Awake()
		{
			base.Awake();
			this.coll = base.GetComponent<Collider>();
			this.coll2D = base.GetComponent<Collider2D>();
			this.tr = base.transform;
			if (this.coll == null && this.coll2D == null && Application.isPlaying)
			{
				throw new Exception("A collider or 2D collider must be attached to the GameObject(" + base.gameObject.name + ") for the DynamicObstacle to work");
			}
			this.prevBounds = this.bounds;
			this.prevRotation = this.tr.rotation;
			this.prevEnabled = false;
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x00038B36 File Offset: 0x00036D36
		public override void OnPostScan()
		{
			if (this.coll == null)
			{
				this.Awake();
			}
			if (this.coll != null)
			{
				this.prevEnabled = this.colliderEnabled;
			}
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x00038B68 File Offset: 0x00036D68
		private void Update()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (this.coll == null && this.coll2D == null)
			{
				Debug.LogError("No collider attached to this GameObject. The DynamicObstacle component requires a collider.", this);
				base.enabled = false;
				return;
			}
			while (this.pendingGraphUpdates.Count > 0 && this.pendingGraphUpdates.Peek().stage != GraphUpdateStage.Pending)
			{
				this.pendingGraphUpdates.Dequeue();
			}
			if (AstarPath.active == null || AstarPath.active.isScanning || Time.realtimeSinceStartup - this.lastCheckTime < this.checkTime || !Application.isPlaying || this.pendingGraphUpdates.Count > 0)
			{
				return;
			}
			this.lastCheckTime = Time.realtimeSinceStartup;
			if (this.colliderEnabled)
			{
				Bounds bounds = this.bounds;
				Quaternion rotation = this.tr.rotation;
				Vector3 vector = this.prevBounds.min - bounds.min;
				Vector3 vector2 = this.prevBounds.max - bounds.max;
				float num = bounds.extents.magnitude * Quaternion.Angle(this.prevRotation, rotation) * 0.017453292f;
				if (vector.sqrMagnitude > this.updateError * this.updateError || vector2.sqrMagnitude > this.updateError * this.updateError || num > this.updateError || !this.prevEnabled)
				{
					this.DoUpdateGraphs();
					return;
				}
			}
			else if (this.prevEnabled)
			{
				this.DoUpdateGraphs();
			}
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x00038CF0 File Offset: 0x00036EF0
		protected override void OnDisable()
		{
			base.OnDisable();
			if (AstarPath.active != null && Application.isPlaying)
			{
				GraphUpdateObject graphUpdateObject = new GraphUpdateObject(this.prevBounds);
				this.pendingGraphUpdates.Enqueue(graphUpdateObject);
				AstarPath.active.UpdateGraphs(graphUpdateObject);
				this.prevEnabled = false;
			}
			this.pendingGraphUpdates.Clear();
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x00038D4C File Offset: 0x00036F4C
		public void DoUpdateGraphs()
		{
			if (this.coll == null && this.coll2D == null)
			{
				return;
			}
			Physics.SyncTransforms();
			Physics2D.SyncTransforms();
			if (!this.colliderEnabled)
			{
				GraphUpdateObject graphUpdateObject = new GraphUpdateObject(this.prevBounds);
				this.pendingGraphUpdates.Enqueue(graphUpdateObject);
				AstarPath.active.UpdateGraphs(graphUpdateObject);
			}
			else
			{
				Bounds bounds = this.bounds;
				Bounds bounds2 = bounds;
				bounds2.Encapsulate(this.prevBounds);
				if (DynamicObstacle.BoundsVolume(bounds2) < DynamicObstacle.BoundsVolume(bounds) + DynamicObstacle.BoundsVolume(this.prevBounds))
				{
					GraphUpdateObject graphUpdateObject2 = new GraphUpdateObject(bounds2);
					this.pendingGraphUpdates.Enqueue(graphUpdateObject2);
					AstarPath.active.UpdateGraphs(graphUpdateObject2);
				}
				else
				{
					GraphUpdateObject graphUpdateObject3 = new GraphUpdateObject(this.prevBounds);
					GraphUpdateObject graphUpdateObject4 = new GraphUpdateObject(bounds);
					this.pendingGraphUpdates.Enqueue(graphUpdateObject3);
					this.pendingGraphUpdates.Enqueue(graphUpdateObject4);
					AstarPath.active.UpdateGraphs(graphUpdateObject3);
					AstarPath.active.UpdateGraphs(graphUpdateObject4);
				}
				this.prevBounds = bounds;
			}
			this.prevEnabled = this.colliderEnabled;
			this.prevRotation = this.tr.rotation;
			this.lastCheckTime = Time.realtimeSinceStartup;
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x00038E76 File Offset: 0x00037076
		private static float BoundsVolume(Bounds b)
		{
			return Math.Abs(b.size.x * b.size.y * b.size.z);
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000AA3 RID: 2723 RVA: 0x00038EA3 File Offset: 0x000370A3
		// (set) Token: 0x06000AA4 RID: 2724 RVA: 0x00038EAB File Offset: 0x000370AB
		float DynamicGridObstacle.updateError
		{
			get
			{
				return this.updateError;
			}
			set
			{
				this.updateError = value;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000AA5 RID: 2725 RVA: 0x00038EB4 File Offset: 0x000370B4
		// (set) Token: 0x06000AA6 RID: 2726 RVA: 0x00038EBC File Offset: 0x000370BC
		float DynamicGridObstacle.checkTime
		{
			get
			{
				return this.checkTime;
			}
			set
			{
				this.checkTime = value;
			}
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x00030EF4 File Offset: 0x0002F0F4
		bool DynamicGridObstacle.get_enabled()
		{
			return base.enabled;
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x00030EFC File Offset: 0x0002F0FC
		void DynamicGridObstacle.set_enabled(bool value)
		{
			base.enabled = value;
		}

		// Token: 0x040006FD RID: 1789
		private Collider coll;

		// Token: 0x040006FE RID: 1790
		private Collider2D coll2D;

		// Token: 0x040006FF RID: 1791
		private Transform tr;

		// Token: 0x04000700 RID: 1792
		public float updateError = 1f;

		// Token: 0x04000701 RID: 1793
		public float checkTime = 0.2f;

		// Token: 0x04000702 RID: 1794
		private Bounds prevBounds;

		// Token: 0x04000703 RID: 1795
		private Quaternion prevRotation;

		// Token: 0x04000704 RID: 1796
		private bool prevEnabled;

		// Token: 0x04000705 RID: 1797
		private float lastCheckTime = -9999f;

		// Token: 0x04000706 RID: 1798
		private Queue<GraphUpdateObject> pendingGraphUpdates = new Queue<GraphUpdateObject>();
	}
}
