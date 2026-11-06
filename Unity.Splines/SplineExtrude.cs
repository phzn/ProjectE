using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Splines.ExtrusionShapes;

namespace UnityEngine.Splines
{
	// Token: 0x02000027 RID: 39
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	[AddComponentMenu("Splines/Spline Extrude")]
	[ExecuteAlways]
	public class SplineExtrude : MonoBehaviour
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001BB RID: 443 RVA: 0x000081D0 File Offset: 0x000063D0
		internal bool CanCapEnds
		{
			get
			{
				return this.m_CanCapEnds;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001BC RID: 444 RVA: 0x000081D8 File Offset: 0x000063D8
		// (set) Token: 0x060001BD RID: 445 RVA: 0x000081E0 File Offset: 0x000063E0
		internal IExtrudeShape Shape
		{
			get
			{
				return this.m_Shape;
			}
			set
			{
				this.m_Shape = value;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001BE RID: 446 RVA: 0x000081E9 File Offset: 0x000063E9
		[Obsolete("Use Container instead.", false)]
		public SplineContainer container
		{
			get
			{
				return this.Container;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001BF RID: 447 RVA: 0x000081F1 File Offset: 0x000063F1
		// (set) Token: 0x060001C0 RID: 448 RVA: 0x000081F9 File Offset: 0x000063F9
		public SplineContainer Container
		{
			get
			{
				return this.m_Container;
			}
			set
			{
				this.m_Container = value;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x00008202 File Offset: 0x00006402
		[Obsolete("Use RebuildOnSplineChange instead.", false)]
		public bool rebuildOnSplineChange
		{
			get
			{
				return this.RebuildOnSplineChange;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x0000820A File Offset: 0x0000640A
		// (set) Token: 0x060001C3 RID: 451 RVA: 0x00008212 File Offset: 0x00006412
		public bool RebuildOnSplineChange
		{
			get
			{
				return this.m_RebuildOnSplineChange;
			}
			set
			{
				this.m_RebuildOnSplineChange = value;
				if (!value)
				{
					this.m_RebuildRequested = value;
				}
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x00008225 File Offset: 0x00006425
		[Obsolete("Use RebuildFrequency instead.", false)]
		public int rebuildFrequency
		{
			get
			{
				return this.RebuildFrequency;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x0000822D File Offset: 0x0000642D
		// (set) Token: 0x060001C6 RID: 454 RVA: 0x00008235 File Offset: 0x00006435
		public int RebuildFrequency
		{
			get
			{
				return this.m_RebuildFrequency;
			}
			set
			{
				this.m_RebuildFrequency = Mathf.Max(value, 1);
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x00008244 File Offset: 0x00006444
		[Obsolete("Use Sides instead.", false)]
		public int sides
		{
			get
			{
				return this.Sides;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x0000824C File Offset: 0x0000644C
		// (set) Token: 0x060001C9 RID: 457 RVA: 0x00008254 File Offset: 0x00006454
		public int Sides
		{
			get
			{
				return this.m_Sides;
			}
			set
			{
				this.m_Sides = Mathf.Max(value, 3);
				if (this.m_Shape == null)
				{
					this.m_Shape = new Circle
					{
						SideCount = this.m_Sides
					};
				}
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001CA RID: 458 RVA: 0x0000828F File Offset: 0x0000648F
		[Obsolete("Use SegmentsPerUnit instead.", false)]
		public float segmentsPerUnit
		{
			get
			{
				return this.SegmentsPerUnit;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001CB RID: 459 RVA: 0x00008297 File Offset: 0x00006497
		// (set) Token: 0x060001CC RID: 460 RVA: 0x0000829F File Offset: 0x0000649F
		public float SegmentsPerUnit
		{
			get
			{
				return this.m_SegmentsPerUnit;
			}
			set
			{
				this.m_SegmentsPerUnit = Mathf.Max(value, 0.0001f);
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001CD RID: 461 RVA: 0x000082B2 File Offset: 0x000064B2
		[Obsolete("Use Capped instead.", false)]
		public bool capped
		{
			get
			{
				return this.Capped;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060001CE RID: 462 RVA: 0x000082BA File Offset: 0x000064BA
		// (set) Token: 0x060001CF RID: 463 RVA: 0x000082C2 File Offset: 0x000064C2
		public bool Capped
		{
			get
			{
				return this.m_Capped;
			}
			set
			{
				this.m_Capped = value;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x000082CB File Offset: 0x000064CB
		[Obsolete("Use Radius instead.", false)]
		public float radius
		{
			get
			{
				return this.Radius;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x000082D3 File Offset: 0x000064D3
		// (set) Token: 0x060001D2 RID: 466 RVA: 0x000082DB File Offset: 0x000064DB
		public float Radius
		{
			get
			{
				return this.m_Radius;
			}
			set
			{
				this.m_Radius = Mathf.Max(value, 1E-05f);
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x000082EE File Offset: 0x000064EE
		[Obsolete("Use Range instead.", false)]
		public Vector2 range
		{
			get
			{
				return this.Range;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x000082F6 File Offset: 0x000064F6
		// (set) Token: 0x060001D5 RID: 469 RVA: 0x000082FE File Offset: 0x000064FE
		public Vector2 Range
		{
			get
			{
				return this.m_Range;
			}
			set
			{
				this.m_Range = new Vector2(Mathf.Min(value.x, value.y), Mathf.Max(value.x, value.y));
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001D6 RID: 470 RVA: 0x0000832D File Offset: 0x0000652D
		// (set) Token: 0x060001D7 RID: 471 RVA: 0x00008335 File Offset: 0x00006535
		public bool FlipNormals
		{
			get
			{
				return this.m_FlipNormals;
			}
			set
			{
				this.m_FlipNormals = value;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x0000833E File Offset: 0x0000653E
		// (set) Token: 0x060001D9 RID: 473 RVA: 0x00008346 File Offset: 0x00006546
		public Mesh targetMesh
		{
			get
			{
				return this.m_TargetMesh;
			}
			set
			{
				if (this.m_TargetMesh == value)
				{
					return;
				}
				this.CleanupMesh();
				this.m_TargetMesh = value;
				this.EnsureMeshExists();
				this.Rebuild();
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060001DA RID: 474 RVA: 0x00008370 File Offset: 0x00006570
		[Obsolete("Use Spline instead.", false)]
		public Spline spline
		{
			get
			{
				return this.Spline;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060001DB RID: 475 RVA: 0x00008378 File Offset: 0x00006578
		public Spline Spline
		{
			get
			{
				SplineContainer container = this.m_Container;
				if (container == null)
				{
					return null;
				}
				return container.Spline;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060001DC RID: 476 RVA: 0x0000838B File Offset: 0x0000658B
		public IReadOnlyList<Spline> Splines
		{
			get
			{
				SplineContainer container = this.m_Container;
				if (container == null)
				{
					return null;
				}
				return container.Splines;
			}
		}

		// Token: 0x060001DD RID: 477 RVA: 0x000083A0 File Offset: 0x000065A0
		internal void Reset()
		{
			base.TryGetComponent<SplineContainer>(out this.m_Container);
			MeshRenderer meshRenderer;
			if (base.TryGetComponent<MeshRenderer>(out meshRenderer) && meshRenderer.sharedMaterial == null)
			{
				GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
				Material sharedMaterial = gameObject.GetComponent<MeshRenderer>().sharedMaterial;
				Object.DestroyImmediate(gameObject);
				meshRenderer.sharedMaterial = sharedMaterial;
			}
			this.Rebuild();
		}

		// Token: 0x060001DE RID: 478 RVA: 0x000083F6 File Offset: 0x000065F6
		private bool IsNullOrEmptyContainer()
		{
			return this.m_Container == null || this.m_Container.Spline == null || this.m_Container.Splines.Count == 0;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00008428 File Offset: 0x00006628
		internal void SetSplineContainerOnGO()
		{
			SplineContainer component = base.gameObject.GetComponent<SplineContainer>();
			if (component != null && component != this.m_Container)
			{
				this.m_Container = component;
			}
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000845F File Offset: 0x0000665F
		private void OnEnable()
		{
			this.EnsureMeshExists();
			Spline.Changed += this.OnSplineChanged;
			if (!this.IsNullOrEmptyContainer())
			{
				this.Rebuild();
			}
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00008486 File Offset: 0x00006686
		private void OnDisable()
		{
			Spline.Changed -= this.OnSplineChanged;
			this.CleanupMesh();
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x000084A0 File Offset: 0x000066A0
		private void OnSplineChanged(Spline spline, int knotIndex, SplineModification modificationType)
		{
			if (!this.m_RebuildOnSplineChange)
			{
				return;
			}
			bool flag = this.m_Container != null && this.Splines.Contains(spline);
			SplineShape splineShape = this.m_Shape as SplineShape;
			bool flag2 = splineShape != null && splineShape.Spline != null && splineShape.Spline.Equals(spline);
			this.m_RebuildRequested |= flag || flag2;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000850B File Offset: 0x0000670B
		private void Update()
		{
			if (this.m_RebuildRequested && Time.time >= this.m_NextScheduledRebuild)
			{
				this.Rebuild();
			}
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00008528 File Offset: 0x00006728
		private void EnsureMeshExists()
		{
			if (this.m_Mesh == null)
			{
				if (this.targetMesh != null)
				{
					this.m_Mesh = this.targetMesh;
				}
				else
				{
					this.m_Mesh = new Mesh
					{
						name = "<Spline Extruded Mesh>"
					};
					this.m_Mesh.hideFlags = HideFlags.HideAndDontSave;
				}
			}
			MeshFilter meshFilter;
			if (base.TryGetComponent<MeshFilter>(out meshFilter))
			{
				meshFilter.hideFlags = HideFlags.NotEditable;
				meshFilter.sharedMesh = this.m_Mesh;
			}
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x000085A0 File Offset: 0x000067A0
		private void CleanupMesh()
		{
			MeshFilter meshFilter;
			if (base.TryGetComponent<MeshFilter>(out meshFilter))
			{
				meshFilter.hideFlags = HideFlags.None;
				meshFilter.sharedMesh = null;
			}
			if (this.m_Mesh != this.m_TargetMesh)
			{
				Object.DestroyImmediate(this.m_Mesh);
			}
			this.m_Mesh = null;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x000085EC File Offset: 0x000067EC
		public void Rebuild()
		{
			if (this.m_Shape == null)
			{
				this.m_Shape = new Circle
				{
					SideCount = this.m_Sides
				};
			}
			if (this.m_Mesh == null)
			{
				return;
			}
			if (this.IsNullOrEmptyContainer())
			{
				if (Application.isPlaying)
				{
					Debug.LogError(SplineExtrude.k_EmptyContainerError, this);
				}
				return;
			}
			this.m_Mesh.Clear();
			if (this.m_Range.x == this.m_Range.y)
			{
				return;
			}
			ExtrudeSettings<IExtrudeShape> extrudeSettings = new ExtrudeSettings<IExtrudeShape>(this.m_Shape)
			{
				Radius = this.m_Radius,
				CapEnds = this.m_Capped,
				Range = this.m_Range,
				FlipNormals = this.m_FlipNormals
			};
			SplineMesh.Extrude<Spline, IExtrudeShape>(this.m_Container.Splines, this.m_Mesh, extrudeSettings, this.SegmentsPerUnit);
			this.m_CanCapEnds = SplineMesh.s_IsConvex && !this.Spline.Closed;
			this.AutosmoothNormals();
			this.m_NextScheduledRebuild = Time.time + 1f / (float)this.m_RebuildFrequency;
			if (this.m_UpdateColliders)
			{
				MeshCollider meshCollider;
				if (base.TryGetComponent<MeshCollider>(out meshCollider))
				{
					meshCollider.sharedMesh = this.m_Mesh;
				}
				BoxCollider boxCollider;
				if (base.TryGetComponent<BoxCollider>(out boxCollider))
				{
					boxCollider.center = this.m_Mesh.bounds.center;
					boxCollider.size = this.m_Mesh.bounds.size;
				}
				SphereCollider sphereCollider;
				if (base.TryGetComponent<SphereCollider>(out sphereCollider))
				{
					sphereCollider.center = this.m_Mesh.bounds.center;
					Vector3 extents = this.m_Mesh.bounds.extents;
					sphereCollider.radius = Mathf.Max(new float[] { extents.x, extents.y, extents.z });
				}
			}
			this.m_RebuildRequested = false;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x000087E0 File Offset: 0x000069E0
		private void AutosmoothNormals()
		{
			Vector3[] vertices = this.m_Mesh.vertices;
			int[] triangles = this.m_Mesh.triangles;
			Vector3[] array = new Vector3[vertices.Length];
			Dictionary<int, Vector3> dictionary = new Dictionary<int, Vector3>();
			Dictionary<int, List<int>> dictionary2 = new Dictionary<int, List<int>>();
			for (int i = 0; i < triangles.Length; i += 3)
			{
				Vector3 vector = vertices[triangles[i]];
				Vector3 vector2 = vertices[triangles[i + 1]];
				Vector3 vector3 = vertices[triangles[i + 2]];
				Vector3 normalized = Vector3.Cross(vector2 - vector, vector3 - vector).normalized;
				int num = i / 3;
				dictionary[num] = normalized;
				for (int j = 0; j < 3; j++)
				{
					int num2 = triangles[i + j];
					if (!dictionary2.ContainsKey(num2))
					{
						dictionary2[num2] = new List<int>();
					}
					dictionary2[num2].Add(num);
				}
			}
			foreach (KeyValuePair<int, List<int>> keyValuePair in dictionary2)
			{
				int key = keyValuePair.Key;
				List<int> value = keyValuePair.Value;
				Vector3 vector4 = Vector3.zero;
				foreach (int num3 in value)
				{
					Vector3 vector5 = dictionary[num3];
					bool flag = true;
					foreach (int num4 in value)
					{
						if (num3 != num4)
						{
							Vector3 vector6 = dictionary[num4];
							if (Vector3.Angle(vector5, vector6) > this.m_AutosmoothAngle)
							{
								flag = false;
								break;
							}
						}
					}
					if (!flag)
					{
						array[key] = vector5;
						break;
					}
					vector4 += vector5;
				}
				if (array[key] == Vector3.zero)
				{
					array[key] = vector4.normalized;
				}
			}
			this.m_Mesh.normals = array;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00008A48 File Offset: 0x00006C48
		internal Mesh CreateMeshAsset()
		{
			return new Mesh
			{
				name = base.name
			};
		}

		// Token: 0x04000087 RID: 135
		[SerializeField]
		[Tooltip("The Spline to extrude.")]
		private SplineContainer m_Container;

		// Token: 0x04000088 RID: 136
		[SerializeField]
		[Tooltip("The mesh that should be extruded. If none, a temporary mesh will be created.")]
		private Mesh m_TargetMesh;

		// Token: 0x04000089 RID: 137
		[SerializeField]
		[Tooltip("Enable to regenerate the extruded mesh when the target Spline is modified. Disable this option if the Spline will not be modified at runtime.")]
		private bool m_RebuildOnSplineChange = true;

		// Token: 0x0400008A RID: 138
		[SerializeField]
		[Tooltip("The maximum number of times per-second that the mesh will be rebuilt.")]
		private int m_RebuildFrequency = 30;

		// Token: 0x0400008B RID: 139
		[SerializeField]
		[Tooltip("Automatically update any Mesh, Box, or Sphere collider components when the mesh is extruded.")]
		private bool m_UpdateColliders = true;

		// Token: 0x0400008C RID: 140
		[SerializeField]
		[Tooltip("The number of sides that comprise the radius of the mesh.")]
		private int m_Sides = 8;

		// Token: 0x0400008D RID: 141
		[SerializeField]
		[Tooltip("The number of edge loops that comprise the length of one unit of the mesh. The total number of sections is equal to \"Spline.GetLength() * segmentsPerUnit\".")]
		private float m_SegmentsPerUnit = 4f;

		// Token: 0x0400008E RID: 142
		[SerializeField]
		[Tooltip("Indicates if the start and end of the mesh are filled. When the target Spline is closed or when the profile of the shape to extrude is concave, this setting is ignored.")]
		private bool m_Capped = true;

		// Token: 0x0400008F RID: 143
		[SerializeField]
		[Tooltip("The radius of the extruded mesh.")]
		private float m_Radius = 0.25f;

		// Token: 0x04000090 RID: 144
		[SerializeField]
		[Tooltip("The section of the Spline to extrude.")]
		private Vector2 m_Range = new Vector2(0f, 1f);

		// Token: 0x04000091 RID: 145
		[SerializeField]
		[Tooltip("Set true to reverse the winding order of vertices so that the face normals are inverted.")]
		private bool m_FlipNormals;

		// Token: 0x04000092 RID: 146
		private Mesh m_Mesh;

		// Token: 0x04000093 RID: 147
		private float m_NextScheduledRebuild;

		// Token: 0x04000094 RID: 148
		private float m_AutosmoothAngle = 180f;

		// Token: 0x04000095 RID: 149
		private bool m_RebuildRequested;

		// Token: 0x04000096 RID: 150
		private bool m_CanCapEnds;

		// Token: 0x04000097 RID: 151
		[SerializeReference]
		private IExtrudeShape m_Shape;

		// Token: 0x04000098 RID: 152
		internal static readonly string k_EmptyContainerError = "Spline Extrude does not have a valid SplineContainer set.";
	}
}
