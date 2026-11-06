using System;
using Pathfinding.Collections;
using Pathfinding.Pooling;
using Pathfinding.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pathfinding
{
	// Token: 0x02000129 RID: 297
	[HelpURL("https://arongranberg.com/astar/documentation/stable/navmeshadd.html")]
	public class NavmeshAdd : NavmeshClipper
	{
		// Token: 0x060009E7 RID: 2535 RVA: 0x00034164 File Offset: 0x00032364
		public override bool RequiresUpdate(GridLookup<NavmeshClipper>.Root previousState)
		{
			return (this.tr.position - previousState.previousPosition).sqrMagnitude > this.updateDistance * this.updateDistance || (this.useRotationAndScale && Quaternion.Angle(previousState.previousRotation, this.tr.rotation) > this.updateRotationDistance);
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x000341C8 File Offset: 0x000323C8
		public override void ForceUpdate()
		{
			if (AstarPath.active != null)
			{
				AstarPath.active.navmeshUpdates.ForceUpdateAround(this);
			}
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x000341E7 File Offset: 0x000323E7
		protected override void Awake()
		{
			base.Awake();
			this.tr = base.transform;
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x000341FB File Offset: 0x000323FB
		public override void NotifyUpdated(GridLookup<NavmeshClipper>.Root previousState)
		{
			previousState.previousPosition = this.tr.position;
			if (this.useRotationAndScale)
			{
				previousState.previousRotation = this.tr.rotation;
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x060009EB RID: 2539 RVA: 0x00034227 File Offset: 0x00032427
		public Vector3 Center
		{
			get
			{
				return this.tr.position + (this.useRotationAndScale ? this.tr.TransformPoint(this.center) : this.center);
			}
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x0003425C File Offset: 0x0003245C
		[ContextMenu("Rebuild Mesh")]
		public void RebuildMesh()
		{
			if (this.type != NavmeshAdd.MeshType.CustomMesh)
			{
				if (this.verts == null || this.verts.Length != 4 || this.tris == null || this.tris.Length != 6)
				{
					this.verts = new Vector3[4];
					this.tris = new int[6];
				}
				this.tris[0] = 0;
				this.tris[1] = 1;
				this.tris[2] = 2;
				this.tris[3] = 0;
				this.tris[4] = 2;
				this.tris[5] = 3;
				this.verts[0] = new Vector3(-this.rectangleSize.x * 0.5f, 0f, -this.rectangleSize.y * 0.5f);
				this.verts[1] = new Vector3(this.rectangleSize.x * 0.5f, 0f, -this.rectangleSize.y * 0.5f);
				this.verts[2] = new Vector3(this.rectangleSize.x * 0.5f, 0f, this.rectangleSize.y * 0.5f);
				this.verts[3] = new Vector3(-this.rectangleSize.x * 0.5f, 0f, this.rectangleSize.y * 0.5f);
				return;
			}
			if (this.mesh == null)
			{
				this.verts = null;
				this.tris = null;
				return;
			}
			this.verts = this.mesh.vertices;
			this.tris = this.mesh.triangles;
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x0003440C File Offset: 0x0003260C
		public override Bounds GetBounds(GraphTransform inverseTransform, float radiusMargin)
		{
			if (this.verts == null)
			{
				this.RebuildMesh();
			}
			Int3[] array = ArrayPool<Int3>.Claim((this.verts != null) ? this.verts.Length : 0);
			int[] array2;
			int num;
			this.GetMesh(ref array, out array2, out num, inverseTransform);
			if (num == 0)
			{
				ArrayPool<Int3>.Release(ref array, false);
				return default(Bounds);
			}
			float num2 = float.MaxValue;
			float num3 = float.MinValue;
			float num4 = float.MaxValue;
			float num5 = float.MinValue;
			float num6 = float.MaxValue;
			float num7 = float.MinValue;
			for (int i = 0; i < num; i++)
			{
				Vector3 vector = (Vector3)array[i];
				num3 = Math.Max(num3, vector.x);
				num2 = Math.Min(num2, vector.x);
				num5 = Math.Max(num5, vector.y);
				num4 = Math.Min(num4, vector.y);
				num7 = Math.Max(num7, vector.z);
				num6 = Math.Min(num6, vector.z);
			}
			ArrayPool<Int3>.Release(ref array, false);
			return new Bounds(new Vector3((num2 + num3) * 0.5f, (num4 + num5) * 0.5f, (num6 + num7) * 0.5f), new Vector3(num3 - num2, num5 - num4, num7 - num6));
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x00034550 File Offset: 0x00032750
		public void GetMesh(ref Int3[] vbuffer, out int[] tbuffer, out int vertexCount, GraphTransform inverseTransform = null)
		{
			if (this.verts == null)
			{
				this.RebuildMesh();
			}
			if (this.verts == null)
			{
				tbuffer = ArrayPool<int>.Claim(0);
				vertexCount = 0;
				return;
			}
			if (vbuffer == null || vbuffer.Length < this.verts.Length)
			{
				if (vbuffer != null)
				{
					ArrayPool<Int3>.Release(ref vbuffer, false);
				}
				vbuffer = ArrayPool<Int3>.Claim(this.verts.Length);
			}
			tbuffer = this.tris;
			vertexCount = this.verts.Length;
			if (this.useRotationAndScale)
			{
				Matrix4x4 matrix4x = Matrix4x4.TRS(this.tr.position + this.center, this.tr.rotation, this.tr.localScale * this.meshScale);
				for (int i = 0; i < this.verts.Length; i++)
				{
					Vector3 vector = matrix4x.MultiplyPoint3x4(this.verts[i]);
					if (inverseTransform != null)
					{
						vector = inverseTransform.InverseTransform(vector);
					}
					vbuffer[i] = (Int3)vector;
				}
				return;
			}
			Vector3 vector2 = this.tr.position + this.center;
			for (int j = 0; j < this.verts.Length; j++)
			{
				Vector3 vector3 = vector2 + this.verts[j] * this.meshScale;
				if (inverseTransform != null)
				{
					vector3 = inverseTransform.InverseTransform(vector3);
				}
				vbuffer[j] = (Int3)vector3;
			}
		}

		// Token: 0x0400066F RID: 1647
		public NavmeshAdd.MeshType type;

		// Token: 0x04000670 RID: 1648
		public Mesh mesh;

		// Token: 0x04000671 RID: 1649
		private Vector3[] verts;

		// Token: 0x04000672 RID: 1650
		private int[] tris;

		// Token: 0x04000673 RID: 1651
		public Vector2 rectangleSize = new Vector2(1f, 1f);

		// Token: 0x04000674 RID: 1652
		public float meshScale = 1f;

		// Token: 0x04000675 RID: 1653
		public Vector3 center;

		// Token: 0x04000676 RID: 1654
		[FormerlySerializedAs("useRotation")]
		public bool useRotationAndScale;

		// Token: 0x04000677 RID: 1655
		[Tooltip("Distance between positions to require an update of the navmesh\nA smaller distance gives better accuracy, but requires more updates when moving the object over time, so it is often slower.")]
		public float updateDistance = 0.4f;

		// Token: 0x04000678 RID: 1656
		[Tooltip("How many degrees rotation that is required for an update to the navmesh. Should be between 0 and 180.")]
		public float updateRotationDistance = 10f;

		// Token: 0x04000679 RID: 1657
		protected Transform tr;

		// Token: 0x0400067A RID: 1658
		public static readonly Color GizmoColor = new Color(0.6039216f, 0.13725491f, 0.9372549f);

		// Token: 0x0200012A RID: 298
		public enum MeshType
		{
			// Token: 0x0400067C RID: 1660
			Rectangle,
			// Token: 0x0400067D RID: 1661
			CustomMesh
		}
	}
}
