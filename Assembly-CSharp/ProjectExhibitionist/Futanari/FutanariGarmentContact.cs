using System;
using System.Collections.Generic;
using MagicaCloth2;
using RayznGames.DressRigger;
using Unity.Mathematics;
using UnityEngine;

namespace ProjectExhibitionist.Futanari
{
	// Token: 0x020009DC RID: 2524
	internal sealed class FutanariGarmentContact : IDisposable
	{
		// Token: 0x0600474A RID: 18250 RVA: 0x001AF198 File Offset: 0x001AD398
		public FutanariGarmentContact(Transform root, SkinnedMeshRenderer anatomy)
		{
			this.root = root;
			this.anatomy = anatomy;
			this.bind = anatomy.sharedMesh.bindposes[Array.IndexOf<Transform>(anatomy.bones, root)];
			this.original = anatomy.sharedMesh.vertices;
			this.deformed = new Vector3[this.original.Length];
			this.delta = new Vector3[this.original.Length];
			this.baseRotation = root.localRotation;
			this.basePosition = root.localPosition;
			int num = Array.IndexOf<Transform>(anatomy.bones, root.parent);
			if (num >= 0)
			{
				Matrix4x4 matrix4x = anatomy.sharedMesh.bindposes[num] * this.bind.inverse;
				this.simulationRestRotation = matrix4x.rotation;
				this.simulationRestPosition = matrix4x.GetColumn(3);
			}
			else
			{
				this.simulationRestRotation = this.baseRotation;
				this.simulationRestPosition = this.basePosition;
			}
			for (int i = 0; i < this.contactCells.Length; i++)
			{
				this.contactCells[i] = new List<int>();
			}
			this.baked.MarkDynamic();
		}

		// Token: 0x0600474B RID: 18251 RVA: 0x001AF341 File Offset: 0x001AD541
		public void BeginWardrobe()
		{
			this.visibleGarments.Clear();
		}

		// Token: 0x17000C1D RID: 3101
		// (get) Token: 0x0600474C RID: 18252 RVA: 0x001AF350 File Offset: 0x001AD550
		public bool HasStableSupport
		{
			get
			{
				foreach (GameObject gameObject in this.visibleGarments)
				{
					if (!(gameObject == null) && gameObject.activeInHierarchy)
					{
						foreach (FutanariGarmentContact.Surface surface in this.garments[gameObject])
						{
							if (surface.renderer != null && surface.renderer.enabled && surface.renderer.gameObject.activeInHierarchy && surface.support != null && surface.support.SupportsAnatomy)
							{
								return true;
							}
						}
					}
				}
				return false;
			}
		}

		// Token: 0x0600474D RID: 18253 RVA: 0x001AF42C File Offset: 0x001AD62C
		public void Prepare()
		{
			if (this.root == null)
			{
				return;
			}
			if (this.ownsRotation && Quaternion.Angle(this.root.localRotation, this.appliedRotation) < 0.01f)
			{
				this.root.localRotation = this.baseRotation;
			}
			else
			{
				this.baseRotation = this.root.localRotation;
			}
			if (this.ownsRotation && Vector3.Distance(this.root.localPosition, this.appliedPosition) < 0.0001f)
			{
				this.root.localPosition = this.basePosition;
			}
			else
			{
				this.basePosition = this.root.localPosition;
			}
			this.ownsRotation = false;
			this.ownsSimulationPose = false;
		}

		// Token: 0x0600474E RID: 18254 RVA: 0x001AF4E8 File Offset: 0x001AD6E8
		public void PrepareSimulation()
		{
			if (this.root == null)
			{
				return;
			}
			this.baseRotation = this.simulationRestRotation;
			this.basePosition = this.simulationRestPosition;
			this.root.localRotation = this.baseRotation;
			this.root.localPosition = this.basePosition;
			this.ownsRotation = false;
			this.ownsSimulationPose = true;
		}

		// Token: 0x0600474F RID: 18255 RVA: 0x001AF54C File Offset: 0x001AD74C
		public void AddGarment(GameObject garment)
		{
			if (garment == null)
			{
				return;
			}
			this.visibleGarments.Add(garment);
			if (this.garments.ContainsKey(garment))
			{
				return;
			}
			SkinnedMeshRenderer[] componentsInChildren = garment.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			List<FutanariGarmentContact.Surface> list = new List<FutanariGarmentContact.Surface>();
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in componentsInChildren)
			{
				Mesh mesh = skinnedMeshRenderer.sharedMesh;
				float num = 0f;
				AnimatedGarmentCloth component = skinnedMeshRenderer.GetComponent<AnimatedGarmentCloth>();
				if (component != null)
				{
					if (component.ReferenceMesh != null)
					{
						mesh = component.ReferenceMesh;
					}
					num = component.ContactAllowance;
				}
				if (mesh != null && mesh.isReadable)
				{
					int[] triangles = mesh.triangles;
					bool[] array2 = null;
					if (component != null)
					{
						Vector3[] vertices = mesh.vertices;
						array2 = new bool[triangles.Length / 3];
						for (int j = 0; j < triangles.Length; j += 3)
						{
							array2[j / 3] = vertices[triangles[j]].z + vertices[triangles[j + 1]].z + vertices[triangles[j + 2]].z < 0f;
						}
					}
					list.Add(new FutanariGarmentContact.Surface
					{
						renderer = skinnedMeshRenderer,
						source = mesh,
						rearTriangles = array2,
						support = component,
						allowance = num,
						triangles = triangles
					});
				}
			}
			this.garments.Add(garment, list.ToArray());
		}

		// Token: 0x06004750 RID: 18256 RVA: 0x001AF6D2 File Offset: 0x001AD8D2
		public void Apply(bool active)
		{
			this.Solve(active, 0f, false);
		}

		// Token: 0x06004751 RID: 18257 RVA: 0x001AF6E1 File Offset: 0x001AD8E1
		public void ApplySupported(float progress)
		{
			this.Solve(true, Mathf.Lerp(55f, 0f, Mathf.SmoothStep(0f, 1f, progress)), true);
		}

		// Token: 0x06004752 RID: 18258 RVA: 0x001AF70C File Offset: 0x001AD90C
		private void Solve(bool active, float relaxedBend, bool supported)
		{
			if (this.root == null)
			{
				return;
			}
			if (!this.ownsRotation || Quaternion.Angle(this.root.localRotation, this.appliedRotation) > 0.01f)
			{
				this.baseRotation = this.root.localRotation;
			}
			if (!this.ownsRotation || Vector3.Distance(this.root.localPosition, this.appliedPosition) > 0.0001f)
			{
				this.basePosition = this.root.localPosition;
			}
			if (!active || this.visibleGarments.Count == 0)
			{
				this.Release();
				return;
			}
			if (this.lastFrame == Time.frameCount)
			{
				return;
			}
			this.lastFrame = Time.frameCount;
			float num;
			float num2;
			this.MeasureAnatomy(out num, out num2);
			Matrix4x4 matrix4x = Matrix4x4.TRS(this.root.position, (this.root.parent != null) ? (this.root.parent.rotation * this.baseRotation) : this.baseRotation, this.root.lossyScale);
			Matrix4x4 inverse = matrix4x.inverse;
			this.anatomy.BakeMesh(this.baked, true);
			this.baked.GetVertices(this.posedAnatomy);
			Matrix4x4 matrix4x2 = inverse * this.anatomy.transform.localToWorldMatrix;
			Vector3 vector = Vector3.zero;
			int num3 = 0;
			for (int i = 0; i < this.posedAnatomy.Count; i++)
			{
				if (this.bind.MultiplyPoint3x4(this.deformed[i]).y >= num * 0.85f)
				{
					vector += matrix4x2.MultiplyPoint3x4(this.posedAnatomy[i]);
					num3++;
				}
			}
			this.shaftDirection = ((num3 > 0 && vector.sqrMagnitude > 1E-08f) ? vector.normalized : Vector3.up);
			this.contactTriangles.Clear();
			this.rearTriangles.Clear();
			List<int>[] array = this.contactCells;
			for (int j = 0; j < array.Length; j++)
			{
				array[j].Clear();
			}
			this.gridOrigin = new Vector2(-num2, -num - num2);
			this.gridScale = new Vector2(16f / (2f * num2), 16f / (2f * (num + num2)));
			foreach (GameObject gameObject in this.visibleGarments)
			{
				if (!(gameObject == null) && gameObject.activeInHierarchy)
				{
					foreach (FutanariGarmentContact.Surface surface in this.garments[gameObject])
					{
						SkinnedMeshRenderer renderer = surface.renderer;
						if (!(renderer == null) && renderer.enabled && renderer.gameObject.activeInHierarchy && !(renderer.sharedMesh == null) && renderer.sharedMesh.vertexCount == surface.source.vertexCount)
						{
							Mesh sharedMesh = renderer.sharedMesh;
							try
							{
								if (sharedMesh != surface.source)
								{
									renderer.sharedMesh = surface.source;
								}
								renderer.BakeMesh(this.baked, true);
							}
							finally
							{
								if (renderer.sharedMesh != sharedMesh)
								{
									renderer.sharedMesh = sharedMesh;
								}
							}
							this.baked.GetVertices(surface.vertices);
							Matrix4x4 matrix4x3 = inverse * renderer.transform.localToWorldMatrix;
							for (int k = 0; k < surface.vertices.Count; k++)
							{
								surface.vertices[k] = matrix4x3.MultiplyPoint3x4(surface.vertices[k]);
							}
							for (int l = 0; l < surface.triangles.Length; l += 3)
							{
								Vector3 vector2 = surface.vertices[surface.triangles[l]];
								Vector3 vector3 = surface.vertices[surface.triangles[l + 1]];
								Vector3 vector4 = surface.vertices[surface.triangles[l + 2]];
								Vector3 vector5 = Vector3.Min(vector2, Vector3.Min(vector3, vector4));
								Vector3 vector6 = Vector3.Max(vector2, Vector3.Max(vector3, vector4));
								if (vector5.x <= num2 && vector6.x >= -num2 && vector6.y >= -num - num2 && vector5.y <= num + num2 && vector6.z >= -num - num2 && vector5.z <= num + num2)
								{
									Vector3 vector7 = vector3 - vector2;
									Vector3 vector8 = vector4 - vector2;
									if (supported && surface.rearTriangles != null && surface.rearTriangles[l / 3])
									{
										this.rearTriangles.Add(new FutanariGarmentContact.Triangle
										{
											a = vector2,
											edge1 = vector7,
											edge2 = vector8
										});
									}
									else
									{
										float num4 = vector7.x * vector8.z - vector7.z * vector8.x;
										if (num4 < -1E-10f)
										{
											this.contactTriangles.Add(new FutanariGarmentContact.Triangle
											{
												a = vector2,
												edge1 = vector7,
												edge2 = vector8,
												min = new Vector2(vector5.x, vector5.z),
												max = new Vector2(vector6.x, vector6.z),
												inverseDeterminant = 1f / num4,
												allowance = surface.allowance
											});
											int num5 = this.GridX(vector5.x);
											int num6 = this.GridX(vector6.x);
											int num7 = this.GridZ(vector5.z);
											int num8 = this.GridZ(vector6.z);
											for (int m = num7; m <= num8; m++)
											{
												for (int n = num5; n <= num6; n++)
												{
													this.contactCells[m * 16 + n].Add(this.contactTriangles.Count - 1);
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			float num9 = 0f;
			float num10 = this.SurfaceDepth(0f, 0f, -num2 * 2f, num);
			if (!float.IsPositiveInfinity(num10))
			{
				num9 = Mathf.Min(0f, num10 - num2);
			}
			float num11 = 0f;
			if (this.RequiredRetraction(0f, num, num2) < num9 - 0.003f)
			{
				num11 = 85f;
				for (float num12 = 1f; num12 <= 85f; num12 += 1f)
				{
					if (this.RequiredRetraction(num12, num, num2) >= num9 - 0.003f)
					{
						float num13 = num12 - 1f;
						float num14 = num12;
						for (int num15 = 0; num15 < 6; num15++)
						{
							float num16 = (num13 + num14) * 0.5f;
							if (this.RequiredRetraction(num16, num, num2) >= num9 - 0.003f)
							{
								num14 = num16;
							}
							else
							{
								num13 = num16;
							}
						}
						num11 = num14;
						break;
					}
				}
			}
			float num17 = Mathf.Max(num11, relaxedBend);
			float num18 = (supported ? this.LimitRearBend(Mathf.Max(this.bend, num17), num, num2) : 85f);
			if (supported)
			{
				num11 = Mathf.Min(num11, num18);
				num17 = Mathf.Min(num17, num18);
			}
			this.bend = Mathf.Max(num11, Mathf.SmoothDamp(this.bend, num17, ref this.bendVelocity, 0.16f, float.PositiveInfinity, Time.deltaTime));
			if (supported && this.bend > num18)
			{
				this.bend = num18;
				this.bendVelocity = 0f;
			}
			if (this.bend <= num11 + 0.001f && this.bendVelocity < 0f)
			{
				this.bendVelocity = 0f;
			}
			float num19 = this.RequiredRetraction(this.bend, num, num2);
			if (num19 < num9 - 0.003f)
			{
				num9 = num19;
			}
			if (supported)
			{
				num9 = 0f;
			}
			this.retraction = ((num9 < this.retraction) ? num9 : Mathf.MoveTowards(this.retraction, num9, Time.deltaTime * 0.5f));
			this.appliedRotation = this.baseRotation * Quaternion.Euler(this.bend, 0f, 0f);
			Vector3 vector9 = matrix4x.MultiplyVector(Vector3.up * this.retraction);
			this.appliedPosition = this.basePosition + ((this.root.parent != null) ? this.root.parent.InverseTransformVector(vector9) : vector9);
			this.root.localPosition = this.appliedPosition;
			this.root.localRotation = this.appliedRotation;
			this.ownsRotation = true;
			this.expired.Clear();
			foreach (KeyValuePair<GameObject, FutanariGarmentContact.Surface[]> keyValuePair in this.garments)
			{
				if (keyValuePair.Key == null)
				{
					this.expired.Add(keyValuePair.Key);
				}
			}
			foreach (GameObject gameObject2 in this.expired)
			{
				this.garments.Remove(gameObject2);
			}
		}

		// Token: 0x06004753 RID: 18259 RVA: 0x001B0110 File Offset: 0x001AE310
		private void MeasureAnatomy(out float length, out float radius)
		{
			Array.Copy(this.original, this.deformed, this.original.Length);
			Mesh sharedMesh = this.anatomy.sharedMesh;
			for (int i = 0; i < sharedMesh.blendShapeCount; i++)
			{
				float blendShapeWeight = this.anatomy.GetBlendShapeWeight(i);
				if (Mathf.Abs(blendShapeWeight) >= 0.001f)
				{
					float blendShapeFrameWeight = sharedMesh.GetBlendShapeFrameWeight(i, 0);
					if (Mathf.Abs(blendShapeFrameWeight) >= 0.001f)
					{
						sharedMesh.GetBlendShapeFrameVertices(i, 0, this.delta, null, null);
						for (int j = 0; j < this.deformed.Length; j++)
						{
							this.deformed[j] += this.delta[j] * (blendShapeWeight / blendShapeFrameWeight);
						}
					}
				}
			}
			length = 0f;
			float num = 0f;
			foreach (Vector3 vector in this.deformed)
			{
				Vector3 vector2 = this.bind.MultiplyPoint3x4(vector);
				if (vector2.y >= 0f)
				{
					length = Mathf.Max(length, vector2.y);
					num = Mathf.Max(num, vector2.x * vector2.x + vector2.z * vector2.z);
				}
			}
			radius = Mathf.Sqrt(num) + 0.002f;
		}

		// Token: 0x06004754 RID: 18260 RVA: 0x001B0280 File Offset: 0x001AE480
		private float LimitRearBend(float desired, float length, float radius)
		{
			if (!this.TouchesRear(desired, length, radius))
			{
				return desired;
			}
			if (this.TouchesRear(0f, length, radius))
			{
				return 0f;
			}
			float num = 0f;
			float num2 = desired;
			for (float num3 = 5f; num3 < desired; num3 += 5f)
			{
				if (this.TouchesRear(num3, length, radius))
				{
					num2 = num3;
					break;
				}
				num = num3;
			}
			for (int i = 0; i < 7; i++)
			{
				float num4 = (num + num2) * 0.5f;
				if (this.TouchesRear(num4, length, radius))
				{
					num2 = num4;
				}
				else
				{
					num = num4;
				}
			}
			return num;
		}

		// Token: 0x06004755 RID: 18261 RVA: 0x001B0308 File Offset: 0x001AE508
		private bool TouchesRear(float angle, float length, float radius)
		{
			Vector3 vector = Quaternion.Euler(angle, 0f, 0f) * this.shaftDirection;
			float num = radius * radius;
			foreach (FutanariGarmentContact.Triangle triangle in this.rearTriangles)
			{
				for (int i = 1; i <= 8; i++)
				{
					Vector3 vector2 = vector * (length * (float)i / 8f);
					float3 @float = vector2;
					float3 float2 = triangle.a;
					float3 float3 = triangle.a + triangle.edge1;
					float3 float4 = triangle.a + triangle.edge2;
					float3 float6;
					float3 float5 = MathUtility.ClosestPtPointTriangle(in @float, in float2, in float3, in float4, out float6);
					if ((vector2 - float5).sqrMagnitude < num)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004756 RID: 18262 RVA: 0x001B0418 File Offset: 0x001AE618
		private float RequiredRetraction(float angle, float length, float radius)
		{
			Vector3 vector = Quaternion.Euler(angle, 0f, 0f) * this.shaftDirection;
			Vector3 normalized = Vector3.Cross(Vector3.right, vector).normalized;
			float num = 0f;
			float num2 = -Mathf.Max(0.1f, radius * 3f);
			for (int i = 0; i <= 6; i++)
			{
				Vector3 vector2 = vector * (length * (float)i / 6f);
				for (int j = 0; j < 5; j++)
				{
					Vector3 vector3 = vector2 + ((j == 0) ? Vector3.zero : ((j == 1) ? (Vector3.right * radius) : ((j == 2) ? (Vector3.left * radius) : ((j == 3) ? (normalized * radius) : (-normalized * radius)))));
					float num3 = this.SurfaceDepth(vector3.x, vector3.z, num2, length);
					if (!float.IsPositiveInfinity(num3))
					{
						num = Mathf.Min(num, num3 - vector3.y - ((j == 0) ? radius : 0.002f));
					}
				}
			}
			return num;
		}

		// Token: 0x06004757 RID: 18263 RVA: 0x001B0540 File Offset: 0x001AE740
		private float SurfaceDepth(float x, float z, float start, float end)
		{
			float num = float.PositiveInfinity;
			foreach (int num2 in this.contactCells[this.GridZ(z) * 16 + this.GridX(x)])
			{
				FutanariGarmentContact.Triangle triangle = this.contactTriangles[num2];
				if (x >= triangle.min.x && x <= triangle.max.x && z >= triangle.min.y && z <= triangle.max.y)
				{
					float num3 = x - triangle.a.x;
					float num4 = z - triangle.a.z;
					float num5 = (num3 * triangle.edge2.z - num4 * triangle.edge2.x) * triangle.inverseDeterminant;
					if (num5 >= 0f && num5 <= 1f)
					{
						float num6 = (triangle.edge1.x * num4 - triangle.edge1.z * num3) * triangle.inverseDeterminant;
						if (num6 >= 0f && num5 + num6 <= 1f)
						{
							float num7 = triangle.a.y + triangle.edge1.y * num5 + triangle.edge2.y * num6;
							if (num7 >= start && num7 <= end)
							{
								num = Mathf.Min(num, num7 + triangle.allowance);
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06004758 RID: 18264 RVA: 0x001B06E8 File Offset: 0x001AE8E8
		private int GridX(float x)
		{
			return Mathf.Clamp(Mathf.FloorToInt((x - this.gridOrigin.x) * this.gridScale.x), 0, 15);
		}

		// Token: 0x06004759 RID: 18265 RVA: 0x001B0710 File Offset: 0x001AE910
		private int GridZ(float z)
		{
			return Mathf.Clamp(Mathf.FloorToInt((z - this.gridOrigin.y) * this.gridScale.y), 0, 15);
		}

		// Token: 0x0600475A RID: 18266 RVA: 0x001B0738 File Offset: 0x001AE938
		public void Release()
		{
			if (this.root != null && this.ownsSimulationPose)
			{
				this.root.localRotation = this.simulationRestRotation;
				this.root.localPosition = this.simulationRestPosition;
			}
			else
			{
				if (this.ownsRotation && this.root != null && Quaternion.Angle(this.root.localRotation, this.appliedRotation) < 0.01f)
				{
					this.root.localRotation = this.baseRotation;
				}
				if (this.ownsRotation && this.root != null && Vector3.Distance(this.root.localPosition, this.appliedPosition) < 0.0001f)
				{
					this.root.localPosition = this.basePosition;
				}
			}
			this.ownsRotation = false;
			this.ownsSimulationPose = false;
			this.bend = 0f;
			this.bendVelocity = 0f;
			this.retraction = 0f;
			this.lastFrame = -1;
		}

		// Token: 0x0600475B RID: 18267 RVA: 0x001B0840 File Offset: 0x001AEA40
		public void Dispose()
		{
			this.Release();
			if (Application.isPlaying)
			{
				Object.Destroy(this.baked);
			}
			else
			{
				Object.DestroyImmediate(this.baked);
			}
			this.garments.Clear();
			this.visibleGarments.Clear();
		}

		// Token: 0x040046DA RID: 18138
		private readonly Transform root;

		// Token: 0x040046DB RID: 18139
		private readonly SkinnedMeshRenderer anatomy;

		// Token: 0x040046DC RID: 18140
		private readonly Matrix4x4 bind;

		// Token: 0x040046DD RID: 18141
		private readonly Vector3[] original;

		// Token: 0x040046DE RID: 18142
		private readonly Vector3[] deformed;

		// Token: 0x040046DF RID: 18143
		private readonly Vector3[] delta;

		// Token: 0x040046E0 RID: 18144
		private readonly Dictionary<GameObject, FutanariGarmentContact.Surface[]> garments = new Dictionary<GameObject, FutanariGarmentContact.Surface[]>();

		// Token: 0x040046E1 RID: 18145
		private readonly HashSet<GameObject> visibleGarments = new HashSet<GameObject>();

		// Token: 0x040046E2 RID: 18146
		private readonly List<GameObject> expired = new List<GameObject>();

		// Token: 0x040046E3 RID: 18147
		private readonly List<FutanariGarmentContact.Triangle> contactTriangles = new List<FutanariGarmentContact.Triangle>();

		// Token: 0x040046E4 RID: 18148
		private readonly List<FutanariGarmentContact.Triangle> rearTriangles = new List<FutanariGarmentContact.Triangle>();

		// Token: 0x040046E5 RID: 18149
		private const int GridSize = 16;

		// Token: 0x040046E6 RID: 18150
		private readonly List<int>[] contactCells = new List<int>[256];

		// Token: 0x040046E7 RID: 18151
		private Vector2 gridOrigin;

		// Token: 0x040046E8 RID: 18152
		private Vector2 gridScale;

		// Token: 0x040046E9 RID: 18153
		private readonly Mesh baked = new Mesh
		{
			name = "Futanari garment contact snapshot"
		};

		// Token: 0x040046EA RID: 18154
		private readonly List<Vector3> posedAnatomy = new List<Vector3>();

		// Token: 0x040046EB RID: 18155
		private Vector3 shaftDirection = Vector3.up;

		// Token: 0x040046EC RID: 18156
		private Quaternion baseRotation;

		// Token: 0x040046ED RID: 18157
		private Quaternion appliedRotation;

		// Token: 0x040046EE RID: 18158
		private readonly Quaternion simulationRestRotation;

		// Token: 0x040046EF RID: 18159
		private readonly Vector3 simulationRestPosition;

		// Token: 0x040046F0 RID: 18160
		private Vector3 basePosition;

		// Token: 0x040046F1 RID: 18161
		private Vector3 appliedPosition;

		// Token: 0x040046F2 RID: 18162
		private bool ownsRotation;

		// Token: 0x040046F3 RID: 18163
		private bool ownsSimulationPose;

		// Token: 0x040046F4 RID: 18164
		private float bend;

		// Token: 0x040046F5 RID: 18165
		private float bendVelocity;

		// Token: 0x040046F6 RID: 18166
		private float retraction;

		// Token: 0x040046F7 RID: 18167
		private int lastFrame = -1;

		// Token: 0x020009DD RID: 2525
		private sealed class Surface
		{
			// Token: 0x040046F8 RID: 18168
			public SkinnedMeshRenderer renderer;

			// Token: 0x040046F9 RID: 18169
			public Mesh source;

			// Token: 0x040046FA RID: 18170
			public int[] triangles;

			// Token: 0x040046FB RID: 18171
			public bool[] rearTriangles;

			// Token: 0x040046FC RID: 18172
			public float allowance;

			// Token: 0x040046FD RID: 18173
			public AnimatedGarmentCloth support;

			// Token: 0x040046FE RID: 18174
			public readonly List<Vector3> vertices = new List<Vector3>();
		}

		// Token: 0x020009DE RID: 2526
		private struct Triangle
		{
			// Token: 0x040046FF RID: 18175
			public Vector3 a;

			// Token: 0x04004700 RID: 18176
			public Vector3 edge1;

			// Token: 0x04004701 RID: 18177
			public Vector3 edge2;

			// Token: 0x04004702 RID: 18178
			public Vector2 min;

			// Token: 0x04004703 RID: 18179
			public Vector2 max;

			// Token: 0x04004704 RID: 18180
			public float inverseDeterminant;

			// Token: 0x04004705 RID: 18181
			public float allowance;
		}
	}
}
