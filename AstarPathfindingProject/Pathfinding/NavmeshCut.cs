using System;
using System.Collections.Generic;
using Pathfinding.Collections;
using Pathfinding.Drawing;
using Pathfinding.Pooling;
using Pathfinding.Serialization;
using Pathfinding.Util;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pathfinding
{
	// Token: 0x0200012C RID: 300
	[AddComponentMenu("Pathfinding/Navmesh/Navmesh Cut")]
	[ExecuteAlways]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/navmeshcut.html")]
	public class NavmeshCut : NavmeshClipper
	{
		// Token: 0x060009FD RID: 2557 RVA: 0x0003489D File Offset: 0x00032A9D
		protected override void Awake()
		{
			base.Awake();
			this.tr = base.transform;
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x000348B1 File Offset: 0x00032AB1
		protected override void OnDisable()
		{
			if (this.meshContourVertices.IsCreated)
			{
				this.meshContourVertices.Dispose();
			}
			if (this.meshContours.IsCreated)
			{
				this.meshContours.Dispose();
			}
			this.lastMesh = null;
			base.OnDisable();
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x000341C8 File Offset: 0x000323C8
		public override void ForceUpdate()
		{
			if (AstarPath.active != null)
			{
				AstarPath.active.navmeshUpdates.ForceUpdateAround(this);
			}
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x000348F0 File Offset: 0x00032AF0
		public override bool RequiresUpdate(GridLookup<NavmeshClipper>.Root previousState)
		{
			if (!this.useRotationAndScale)
			{
				return (this.tr.position - previousState.previousPosition).sqrMagnitude > this.updateDistance * this.updateDistance;
			}
			Vector3 vector;
			Quaternion quaternion;
			this.tr.GetPositionAndRotation(out vector, out quaternion);
			return (vector - previousState.previousPosition).sqrMagnitude > this.updateDistance * this.updateDistance || Quaternion.Angle(previousState.previousRotation, quaternion) > this.updateRotationDistance;
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x00003788 File Offset: 0x00001988
		public virtual void UsedForCut()
		{
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x0003497B File Offset: 0x00032B7B
		public override void NotifyUpdated(GridLookup<NavmeshClipper>.Root previousState)
		{
			previousState.previousPosition = this.tr.position;
			if (this.useRotationAndScale)
			{
				previousState.previousRotation = this.tr.rotation;
			}
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x000349A8 File Offset: 0x00032BA8
		private void CalculateMeshContour()
		{
			if (this.mesh == null)
			{
				return;
			}
			NavmeshCut.edges.Clear();
			NavmeshCut.pointers.Clear();
			Vector3[] vertices = this.mesh.vertices;
			int[] triangles = this.mesh.triangles;
			for (int i = 0; i < triangles.Length; i += 3)
			{
				if (VectorMath.IsClockwiseXZ(vertices[triangles[i]], vertices[triangles[i + 1]], vertices[triangles[i + 2]]))
				{
					int num = triangles[i];
					triangles[i] = triangles[i + 2];
					triangles[i + 2] = num;
				}
				NavmeshCut.edges[new Vector2Int(triangles[i], triangles[i + 1])] = i;
				NavmeshCut.edges[new Vector2Int(triangles[i + 1], triangles[i + 2])] = i;
				NavmeshCut.edges[new Vector2Int(triangles[i + 2], triangles[i])] = i;
			}
			for (int j = 0; j < triangles.Length; j += 3)
			{
				for (int k = 0; k < 3; k++)
				{
					if (!NavmeshCut.edges.ContainsKey(new Vector2Int(triangles[j + (k + 1) % 3], triangles[j + k % 3])))
					{
						NavmeshCut.pointers[triangles[j + k % 3]] = triangles[j + (k + 1) % 3];
					}
				}
			}
			NativeList<float3> nativeList = new NativeList<float3>(Allocator.Persistent);
			NativeList<NavmeshCut.ContourBurst> nativeList2 = new NativeList<NavmeshCut.ContourBurst>(Allocator.Persistent);
			for (int l = 0; l < vertices.Length; l++)
			{
				if (NavmeshCut.pointers.ContainsKey(l))
				{
					int length = nativeList.Length;
					int num2 = l;
					do
					{
						int num3 = NavmeshCut.pointers[num2];
						if (num3 == -1)
						{
							break;
						}
						NavmeshCut.pointers[num2] = -1;
						float3 @float = vertices[num2];
						nativeList.Add(in @float);
						num2 = num3;
					}
					while (num2 != l);
					if (nativeList.Length != length)
					{
						NavmeshCut.ContourBurst contourBurst = default(NavmeshCut.ContourBurst);
						contourBurst.startIndex = length;
						contourBurst.endIndex = nativeList.Length;
						contourBurst.ymin = 0f;
						contourBurst.ymax = 0f;
						nativeList2.Add(in contourBurst);
					}
				}
			}
			if (this.meshContourVertices.IsCreated)
			{
				this.meshContourVertices.Dispose();
			}
			if (this.meshContours.IsCreated)
			{
				this.meshContours.Dispose();
			}
			this.meshContourVertices = nativeList;
			this.meshContours = nativeList2;
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x00034C1C File Offset: 0x00032E1C
		public unsafe override Bounds GetBounds(GraphTransform inverseTransform, float radiusMargin)
		{
			UnsafeList<float2> unsafeList = new UnsafeList<float2>(0, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			UnsafeList<NavmeshCut.ContourBurst> unsafeList2 = new UnsafeList<NavmeshCut.ContourBurst>(1, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			this.GetContourBurst(&unsafeList, &unsafeList2, inverseTransform.inverseMatrix, radiusMargin);
			float num = float.MaxValue;
			float num2 = float.MinValue;
			float num3 = float.MaxValue;
			float num4 = float.MinValue;
			float num5 = float.MaxValue;
			float num6 = float.MinValue;
			for (int i = 0; i < unsafeList.Length; i++)
			{
				float2 @float = unsafeList[i];
				num = Math.Min(num, @float.x);
				num2 = Math.Max(num2, @float.x);
				num5 = Math.Min(num5, @float.y);
				num6 = Math.Max(num6, @float.y);
			}
			for (int j = 0; j < unsafeList2.Length; j++)
			{
				NavmeshCut.ContourBurst contourBurst = unsafeList2[j];
				num3 = Math.Min(num3, contourBurst.ymin);
				num4 = Math.Max(num4, contourBurst.ymax);
			}
			unsafeList.Dispose();
			unsafeList2.Dispose();
			return new Bounds(new Vector3((num + num2) * 0.5f, (num3 + num4) * 0.5f, (num5 + num6) * 0.5f), new Vector3(num2 - num, num4 - num3, num6 - num5));
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000A05 RID: 2565 RVA: 0x00034D6C File Offset: 0x00032F6C
		private Matrix4x4 contourTransformationMatrix
		{
			get
			{
				if (this.useRotationAndScale)
				{
					return this.tr.localToWorldMatrix * Matrix4x4.Translate(this.center);
				}
				return Matrix4x4.Translate(this.tr.position + this.center);
			}
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x00034DB8 File Offset: 0x00032FB8
		public unsafe void GetContour(List<NavmeshCut.Contour> buffer, Matrix4x4 matrix, float radiusMargin)
		{
			UnsafeList<float2> unsafeList = new UnsafeList<float2>(0, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			UnsafeList<NavmeshCut.ContourBurst> unsafeList2 = new UnsafeList<NavmeshCut.ContourBurst>(1, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			this.GetContourBurst(&unsafeList, &unsafeList2, matrix, radiusMargin);
			for (int i = 0; i < unsafeList2.Length; i++)
			{
				List<Vector2> list = ListPool<Vector2>.Claim();
				NavmeshCut.ContourBurst contourBurst = unsafeList2[i];
				for (int j = contourBurst.startIndex; j < contourBurst.endIndex; j++)
				{
					list.Add(unsafeList[j]);
				}
				buffer.Add(new NavmeshCut.Contour
				{
					ymin = contourBurst.ymin,
					ymax = contourBurst.ymax,
					contour = list
				});
			}
			unsafeList.Dispose();
			unsafeList2.Dispose();
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x00034E88 File Offset: 0x00033088
		public unsafe void GetContourBurst(UnsafeList<float2>* outputVertices, UnsafeList<NavmeshCut.ContourBurst>* outputContours, Matrix4x4 matrix, float radiusMargin)
		{
			if (this.radiusExpansionMode == NavmeshCut.RadiusExpansionMode.DontExpand)
			{
				radiusMargin = 0f;
			}
			if (this.type == NavmeshCut.MeshType.CustomMesh && (this.mesh != this.lastMesh || !this.meshContours.IsCreated || !this.meshContourVertices.IsCreated))
			{
				this.CalculateMeshContour();
				this.lastMesh = this.mesh;
			}
			NavmeshCutJobs.JobCalculateContour jobCalculateContour = new NavmeshCutJobs.JobCalculateContour
			{
				outputVertices = outputVertices,
				outputContours = outputContours,
				matrix = matrix,
				localToWorldMatrix = this.contourTransformationMatrix,
				radiusMargin = radiusMargin,
				circleResolution = this.circleResolution,
				circleRadius = this.circleRadius,
				rectangleSize = this.rectangleSize,
				height = this.height,
				meshType = this.type,
				meshContours = this.meshContours.GetUnsafeList(),
				meshContourVertices = this.meshContourVertices.GetUnsafeList(),
				meshScale = this.meshScale
			};
			NavmeshCutJobs.CalculateContour(ref jobCalculateContour);
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x00034FAC File Offset: 0x000331AC
		private static NavmeshBase ClosestGraph(Vector3 position)
		{
			AstarPath active = AstarPath.active;
			NavGraph[] array = ((active != null) ? active.data.graphs : null);
			if (array == null)
			{
				return null;
			}
			NavmeshBase navmeshBase = null;
			float num = float.PositiveInfinity;
			for (int i = 0; i < array.Length; i++)
			{
				NavmeshBase navmeshBase2 = array[i] as NavmeshBase;
				if (navmeshBase2 != null)
				{
					float num2 = (navmeshBase2.IsInsideBounds(position) ? (-1f) : navmeshBase2.bounds.SqrDistance(position));
					if (num2 < num)
					{
						num = num2;
						navmeshBase = navmeshBase2;
					}
				}
			}
			return navmeshBase;
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x0003502C File Offset: 0x0003322C
		public unsafe override void DrawGizmos()
		{
			if (this.tr == null)
			{
				this.tr = base.transform;
			}
			bool flag = GizmoContext.InActiveSelection(this.tr);
			NavmeshBase navmeshBase = NavmeshCut.ClosestGraph(this.tr.position);
			GraphTransform graphTransform = ((navmeshBase != null) ? navmeshBase.transform : GraphTransform.identityTransform);
			float num = ((navmeshBase != null) ? navmeshBase.NavmeshCuttingCharacterRadius : 0f);
			UnsafeList<float2> unsafeList = new UnsafeList<float2>(0, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			UnsafeList<NavmeshCut.ContourBurst> unsafeList2 = new UnsafeList<NavmeshCut.ContourBurst>(0, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			this.GetContourBurst(&unsafeList, &unsafeList2, graphTransform.inverseMatrix, num);
			Color color = Color.Lerp(NavmeshCut.GizmoColor, Color.white, 0.5f);
			color.a *= 0.5f;
			using (Draw.WithColor(color))
			{
				for (int i = 0; i < unsafeList2.Length; i++)
				{
					NavmeshCut.ContourBurst contourBurst = unsafeList2[i];
					float num2 = (contourBurst.ymin + contourBurst.ymax) * 0.5f;
					int num3 = contourBurst.endIndex - contourBurst.startIndex;
					for (int j = 0; j < num3; j++)
					{
						float2 @float = unsafeList[contourBurst.startIndex + j];
						float2 float2 = unsafeList[contourBurst.startIndex + (j + 1) % num3];
						Vector3 vector = new Vector3(@float.x, num2, @float.y);
						Vector3 vector2 = new Vector3(float2.x, num2, float2.y);
						Draw.Line(graphTransform.Transform(vector), graphTransform.Transform(vector2), NavmeshCut.GizmoColor);
						if (flag)
						{
							Vector3 vector3 = vector;
							Vector3 vector4 = vector2;
							Vector3 vector5 = vector;
							Vector3 vector6 = vector2;
							vector3.y = (vector4.y = contourBurst.ymin);
							vector5.y = (vector6.y = contourBurst.ymax);
							Draw.Line(graphTransform.Transform(vector3), graphTransform.Transform(vector4));
							Draw.Line(graphTransform.Transform(vector5), graphTransform.Transform(vector6));
							Draw.Line(graphTransform.Transform(vector3), graphTransform.Transform(vector5));
						}
					}
				}
			}
			if (flag)
			{
				switch (this.type)
				{
				case NavmeshCut.MeshType.CustomMesh:
					goto IL_045C;
				case NavmeshCut.MeshType.Box:
					using (Draw.WithMatrix(this.contourTransformationMatrix * Matrix4x4.Scale(new Vector3(this.rectangleSize.x, this.height, this.rectangleSize.y))))
					{
						Draw.WireBox(Vector3.zero, Vector3.one, NavmeshCut.GizmoColor2);
						goto IL_04B1;
					}
					break;
				case NavmeshCut.MeshType.Sphere:
				{
					float num4 = (this.useRotationAndScale ? math.cmax(this.tr.lossyScale) : 1f);
					using (Draw.WithMatrix(Matrix4x4.TRS(this.tr.position, this.useRotationAndScale ? this.tr.rotation : Quaternion.identity, Vector3.one * num4) * Matrix4x4.Translate(this.center)))
					{
						Draw.WireSphere(Vector3.zero, this.circleRadius, NavmeshCut.GizmoColor2);
						goto IL_04B1;
					}
					goto IL_045C;
				}
				case NavmeshCut.MeshType.Capsule:
					break;
				default:
					goto IL_04B1;
				}
				Matrix4x4 contourTransformationMatrix = this.contourTransformationMatrix;
				float num5 = Mathf.Max(this.height, this.circleRadius * 2f);
				float num6 = math.length(contourTransformationMatrix.GetColumn(0));
				float num7 = math.length(contourTransformationMatrix.GetColumn(2));
				float num8 = this.circleRadius * math.max(num6, num7);
				Vector3 normalized = contourTransformationMatrix.GetColumn(1).normalized;
				Vector3 vector7 = this.contourTransformationMatrix.MultiplyPoint3x4(new Vector3(0f, num5 * 0.5f, 0f)) - normalized * num8;
				Vector3 vector8 = this.contourTransformationMatrix.MultiplyPoint3x4(-new Vector3(0f, num5 * 0.5f, 0f)) + normalized * num8;
				Draw.WireCapsule(vector7, vector8, num8, NavmeshCut.GizmoColor2);
				goto IL_04B1;
				IL_045C:
				if (this.mesh != null)
				{
					using (Draw.WithMatrix(this.contourTransformationMatrix * Matrix4x4.Scale(Vector3.one * this.meshScale)))
					{
						Draw.WireMesh(this.mesh, NavmeshCut.GizmoColor2);
					}
				}
			}
			IL_04B1:
			unsafeList.Dispose();
			unsafeList2.Dispose();
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x0003555C File Offset: 0x0003375C
		protected override void OnUpgradeSerializedData(ref Migrations migrations, bool unityThread)
		{
			int num;
			if (migrations.TryMigrateFromLegacyFormat(out num) && num < 2)
			{
				this.radiusExpansionMode = NavmeshCut.RadiusExpansionMode.DontExpand;
			}
		}

		// Token: 0x04000683 RID: 1667
		[Tooltip("Shape of the cut")]
		public NavmeshCut.MeshType type = NavmeshCut.MeshType.Box;

		// Token: 0x04000684 RID: 1668
		[Tooltip("The contour(s) of the mesh will be extracted. This mesh should only be a 2D surface, not a volume (see documentation).")]
		public Mesh mesh;

		// Token: 0x04000685 RID: 1669
		public Vector2 rectangleSize = new Vector2(1f, 1f);

		// Token: 0x04000686 RID: 1670
		public float circleRadius = 1f;

		// Token: 0x04000687 RID: 1671
		public int circleResolution = 6;

		// Token: 0x04000688 RID: 1672
		public float height = 1f;

		// Token: 0x04000689 RID: 1673
		[Tooltip("Scale of the custom mesh")]
		public float meshScale = 1f;

		// Token: 0x0400068A RID: 1674
		public Vector3 center;

		// Token: 0x0400068B RID: 1675
		[Tooltip("Distance between positions to require an update of the navmesh\nA smaller distance gives better accuracy, but requires more updates when moving the object over time, so it is often slower.")]
		public float updateDistance = 0.4f;

		// Token: 0x0400068C RID: 1676
		[Tooltip("Only makes a split in the navmesh, but does not remove the geometry to make a hole")]
		public bool isDual;

		// Token: 0x0400068D RID: 1677
		public NavmeshCut.RadiusExpansionMode radiusExpansionMode = NavmeshCut.RadiusExpansionMode.ExpandByAgentRadius;

		// Token: 0x0400068E RID: 1678
		public bool cutsAddedGeom = true;

		// Token: 0x0400068F RID: 1679
		[Tooltip("How many degrees rotation that is required for an update to the navmesh. Should be between 0 and 180.")]
		public float updateRotationDistance = 10f;

		// Token: 0x04000690 RID: 1680
		[Tooltip("Includes rotation in calculations. This is slower since a lot more matrix multiplications are needed but gives more flexibility.")]
		[FormerlySerializedAs("useRotation")]
		public bool useRotationAndScale;

		// Token: 0x04000691 RID: 1681
		private NativeList<float3> meshContourVertices;

		// Token: 0x04000692 RID: 1682
		private NativeList<NavmeshCut.ContourBurst> meshContours;

		// Token: 0x04000693 RID: 1683
		protected Transform tr;

		// Token: 0x04000694 RID: 1684
		private Mesh lastMesh;

		// Token: 0x04000695 RID: 1685
		private static readonly Dictionary<Vector2Int, int> edges = new Dictionary<Vector2Int, int>();

		// Token: 0x04000696 RID: 1686
		private static readonly Dictionary<int, int> pointers = new Dictionary<int, int>();

		// Token: 0x04000697 RID: 1687
		public static readonly Color GizmoColor = new Color(0.14509805f, 0.72156864f, 0.9372549f);

		// Token: 0x04000698 RID: 1688
		public static readonly Color GizmoColor2 = new Color(0.6627451f, 0.36078432f, 0.9490196f);

		// Token: 0x0200012D RID: 301
		public enum MeshType
		{
			// Token: 0x0400069A RID: 1690
			Rectangle,
			// Token: 0x0400069B RID: 1691
			Circle,
			// Token: 0x0400069C RID: 1692
			CustomMesh,
			// Token: 0x0400069D RID: 1693
			Box,
			// Token: 0x0400069E RID: 1694
			Sphere,
			// Token: 0x0400069F RID: 1695
			Capsule
		}

		// Token: 0x0200012E RID: 302
		public enum RadiusExpansionMode
		{
			// Token: 0x040006A1 RID: 1697
			DontExpand,
			// Token: 0x040006A2 RID: 1698
			ExpandByAgentRadius
		}

		// Token: 0x0200012F RID: 303
		public struct Contour
		{
			// Token: 0x040006A3 RID: 1699
			public float ymin;

			// Token: 0x040006A4 RID: 1700
			public float ymax;

			// Token: 0x040006A5 RID: 1701
			public List<Vector2> contour;
		}

		// Token: 0x02000130 RID: 304
		public struct ContourBurst
		{
			// Token: 0x040006A6 RID: 1702
			public int startIndex;

			// Token: 0x040006A7 RID: 1703
			public int endIndex;

			// Token: 0x040006A8 RID: 1704
			public float ymin;

			// Token: 0x040006A9 RID: 1705
			public float ymax;
		}
	}
}
