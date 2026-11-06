using System;
using Features.NipplePiercings;
using UnityEngine;

namespace Features.NippleRotor
{
	// Token: 0x02000F51 RID: 3921
	public sealed class NippleRotorBreastSurface
	{
		// Token: 0x17001347 RID: 4935
		// (get) Token: 0x06007578 RID: 30072 RVA: 0x002F9349 File Offset: 0x002F7549
		public bool IsBound
		{
			get
			{
				return this._data != null && this._body != null && this._boundMesh != null;
			}
		}

		// Token: 0x17001348 RID: 4936
		// (get) Token: 0x06007579 RID: 30073 RVA: 0x002F9375 File Offset: 0x002F7575
		public bool IsValid
		{
			get
			{
				return this._valid;
			}
		}

		// Token: 0x17001349 RID: 4937
		// (get) Token: 0x0600757A RID: 30074 RVA: 0x002F937D File Offset: 0x002F757D
		public int Count
		{
			get
			{
				return this._count;
			}
		}

		// Token: 0x1700134A RID: 4938
		// (get) Token: 0x0600757B RID: 30075 RVA: 0x002F9385 File Offset: 0x002F7585
		public int LeftCount
		{
			get
			{
				return this._leftCount;
			}
		}

		// Token: 0x1700134B RID: 4939
		// (get) Token: 0x0600757C RID: 30076 RVA: 0x002F938D File Offset: 0x002F758D
		public SkinnedMeshRenderer Body
		{
			get
			{
				return this._body;
			}
		}

		// Token: 0x1700134C RID: 4940
		// (get) Token: 0x0600757D RID: 30077 RVA: 0x002F9395 File Offset: 0x002F7595
		public NipplePiercingSurfaceBinding Data
		{
			get
			{
				return this._data;
			}
		}

		// Token: 0x0600757E RID: 30078 RVA: 0x002F93A0 File Offset: 0x002F75A0
		public bool Bind(NipplePiercingSurfaceBinding data, SkinnedMeshRenderer body)
		{
			this.Unbind();
			if (data == null || data.vertices == null || data.vertices.Length == 0 || data.bindposes == null)
			{
				return false;
			}
			if (data.leftCount <= 0 || data.leftCount >= data.vertices.Length)
			{
				return false;
			}
			if (!this.ResolveBody(data, body))
			{
				return false;
			}
			this._data = data;
			this._count = data.vertices.Length;
			this._leftCount = data.leftCount;
			this._sampleFrame = new int[this._count];
			this._world = new Vector3[this._count];
			this._worldNormal = new Vector3[this._count];
			this._boneFrame = new int[this._bones.Length];
			this._skin = new Matrix4x4[this._bones.Length];
			int num = ((data.shapes != null) ? data.shapes.Length : 0);
			this._activeShape = new int[num];
			this._activeLower = new int[num];
			this._activeUpper = new int[num];
			this._activeT = new float[num];
			this.BuildNeighbours();
			for (int i = 0; i < 2; i++)
			{
				this.BuildBounds(i);
			}
			this._frameId = 0;
			return true;
		}

		// Token: 0x0600757F RID: 30079 RVA: 0x002F94E0 File Offset: 0x002F76E0
		public void Unbind()
		{
			this._data = null;
			this._body = null;
			this._boundMesh = null;
			this._bones = null;
			this._valid = false;
			this._count = (this._leftCount = 0);
		}

		// Token: 0x06007580 RID: 30080 RVA: 0x002F9520 File Offset: 0x002F7720
		private bool ResolveBody(NipplePiercingSurfaceBinding data, SkinnedMeshRenderer body)
		{
			if (body == null || body.sharedMesh == null || data.sourceMesh == null || body.sharedMesh.vertexCount != data.sourceMesh.vertexCount)
			{
				return false;
			}
			Transform[] bones = body.bones;
			if (bones.Length != data.bindposes.Length)
			{
				return false;
			}
			NipplePiercingSurfaceBinding.Vertex[] vertices = data.vertices;
			for (int i = 0; i < vertices.Length; i++)
			{
				BoneWeight weights = vertices[i].weights;
				if (!NippleRotorBreastSurface.ValidBone(bones, weights.boneIndex0, weights.weight0) || !NippleRotorBreastSurface.ValidBone(bones, weights.boneIndex1, weights.weight1) || !NippleRotorBreastSurface.ValidBone(bones, weights.boneIndex2, weights.weight2) || !NippleRotorBreastSurface.ValidBone(bones, weights.boneIndex3, weights.weight3))
				{
					return false;
				}
			}
			int num = ((data.shapes != null) ? data.shapes.Length : 0);
			int[] array = new int[num];
			for (int j = 0; j < num; j++)
			{
				array[j] = body.sharedMesh.GetBlendShapeIndex(data.shapes[j].name);
			}
			this._body = body;
			this._boundMesh = body.sharedMesh;
			this._bones = bones;
			this._shapeIndex = array;
			return true;
		}

		// Token: 0x06007581 RID: 30081 RVA: 0x002F966A File Offset: 0x002F786A
		private static bool ValidBone(Transform[] bones, int index, float weight)
		{
			return weight <= 0f || (index >= 0 && index < bones.Length && bones[index] != null);
		}

		// Token: 0x06007582 RID: 30082 RVA: 0x002F968C File Offset: 0x002F788C
		public bool BeginFrame()
		{
			this._valid = false;
			if (this._data == null || this._body == null)
			{
				return false;
			}
			if (this._body.sharedMesh != this._boundMesh && !this.ResolveBody(this._data, this._body))
			{
				return false;
			}
			this._frameId = ((this._frameId == int.MaxValue) ? 1 : (this._frameId + 1));
			this._scale = Mathf.Abs(this._body.transform.lossyScale.x);
			if (this._scale <= 1E-05f || float.IsInfinity(this._scale))
			{
				this._scale = 1f;
			}
			this._activeCount = 0;
			NipplePiercingSurfaceBinding.Shape[] shapes = this._data.shapes;
			int num = 0;
			while (shapes != null && num < shapes.Length)
			{
				int num2 = this._shapeIndex[num];
				if (num2 >= 0)
				{
					float blendShapeWeight = this._body.GetBlendShapeWeight(num2);
					if (Mathf.Abs(blendShapeWeight) >= 1E-05f)
					{
						NipplePiercingSurfaceBinding.Frame[] frames = shapes[num].frames;
						if (frames != null && frames.Length != 0)
						{
							int num3 = 0;
							while (num3 < frames.Length - 1 && blendShapeWeight > frames[num3].weight)
							{
								num3++;
							}
							float num4 = ((num3 > 0) ? frames[num3 - 1].weight : 0f);
							this._activeShape[this._activeCount] = num;
							this._activeUpper[this._activeCount] = num3;
							this._activeLower[this._activeCount] = num3 - 1;
							this._activeT[this._activeCount] = (blendShapeWeight - num4) / Mathf.Max(1E-05f, frames[num3].weight - num4);
							this._activeCount++;
						}
					}
				}
				num++;
			}
			this._valid = true;
			return true;
		}

		// Token: 0x06007583 RID: 30083 RVA: 0x002F9868 File Offset: 0x002F7A68
		public Vector3 GetPoint(int sample)
		{
			this.EnsureSample(sample);
			return this._world[sample];
		}

		// Token: 0x06007584 RID: 30084 RVA: 0x002F987D File Offset: 0x002F7A7D
		public Vector3 GetNormal(int sample)
		{
			this.EnsureSample(sample);
			return this._worldNormal[sample];
		}

		// Token: 0x06007585 RID: 30085 RVA: 0x002F9892 File Offset: 0x002F7A92
		public int SideOf(int sample)
		{
			if (sample >= this._leftCount)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06007586 RID: 30086 RVA: 0x002F98A0 File Offset: 0x002F7AA0
		public bool TryContact(int side, Vector3 point, float reach, ref int hint, out Vector3 surfacePoint, out Vector3 normal)
		{
			surfacePoint = (normal = Vector3.zero);
			if (!this._valid || side < 0 || side > 1 || !NippleRotorBreastSurface.IsFinite(point))
			{
				return false;
			}
			Vector3 point2 = this.GetPoint(this._centreSample[side]);
			float num = this._bindRadius[side] * this._scale * 1.6f + Mathf.Max(0f, reach);
			if ((point - point2).sqrMagnitude > num * num)
			{
				return false;
			}
			int num2 = this.FindNearest(side, point, hint);
			hint = num2;
			if (num2 < 0)
			{
				return false;
			}
			surfacePoint = this._world[num2];
			normal = this._worldNormal[num2];
			Vector3 vector = point - surfacePoint;
			return (vector - normal * Vector3.Dot(vector, normal)).sqrMagnitude <= reach * reach;
		}

		// Token: 0x06007587 RID: 30087 RVA: 0x002F99A4 File Offset: 0x002F7BA4
		public static float PushOut(ref Vector3 point, Vector3 surfacePoint, Vector3 normal, float clearance)
		{
			float num = clearance - Vector3.Dot(point - surfacePoint, normal);
			if (num <= 1E-06f)
			{
				return 0f;
			}
			point += normal * num;
			return num;
		}

		// Token: 0x06007588 RID: 30088 RVA: 0x002F99F0 File Offset: 0x002F7BF0
		private int FindNearest(int side, Vector3 point, int hint)
		{
			int num = ((side == 0) ? 0 : this._leftCount);
			int num2 = ((side == 0) ? this._leftCount : this._count);
			if (hint < num || hint >= num2)
			{
				return this.FullScan(num, num2, point);
			}
			int num3 = hint;
			float num4 = (this.GetPoint(num3) - point).sqrMagnitude;
			for (int i = 0; i < 64; i++)
			{
				int num5 = num3;
				int num6 = num3 * 8;
				for (int j = 0; j < 8; j++)
				{
					int num7 = this._neighbours[num6 + j];
					if (num7 < 0)
					{
						break;
					}
					float sqrMagnitude = (this.GetPoint(num7) - point).sqrMagnitude;
					if (sqrMagnitude < num4)
					{
						num4 = sqrMagnitude;
						num5 = num7;
					}
				}
				if (num5 == num3)
				{
					return num3;
				}
				num3 = num5;
			}
			return num3;
		}

		// Token: 0x06007589 RID: 30089 RVA: 0x002F9AB4 File Offset: 0x002F7CB4
		private int FullScan(int start, int end, Vector3 point)
		{
			int num = -1;
			float num2 = float.PositiveInfinity;
			for (int i = start; i < end; i++)
			{
				float sqrMagnitude = (this.GetPoint(i) - point).sqrMagnitude;
				if (sqrMagnitude < num2)
				{
					num2 = sqrMagnitude;
					num = i;
				}
			}
			return num;
		}

		// Token: 0x0600758A RID: 30090 RVA: 0x002F9AF8 File Offset: 0x002F7CF8
		private void EnsureSample(int sample)
		{
			if (this._sampleFrame[sample] == this._frameId)
			{
				return;
			}
			this._sampleFrame[sample] = this._frameId;
			NipplePiercingSurfaceBinding.Vertex vertex = this._data.vertices[sample];
			Vector3 vector = vertex.position;
			Vector3 vector2 = vertex.normal;
			for (int i = 0; i < this._activeCount; i++)
			{
				NipplePiercingSurfaceBinding.Frame[] frames = this._data.shapes[this._activeShape[i]].frames;
				NipplePiercingSurfaceBinding.Frame frame = frames[this._activeUpper[i]];
				int num = this._activeLower[i];
				float num2 = this._activeT[i];
				if (num < 0)
				{
					vector += frame.positions[sample] * num2;
					vector2 += frame.normals[sample] * num2;
				}
				else
				{
					NipplePiercingSurfaceBinding.Frame frame2 = frames[num];
					vector += Vector3.LerpUnclamped(frame2.positions[sample], frame.positions[sample], num2);
					vector2 += Vector3.LerpUnclamped(frame2.normals[sample], frame.normals[sample], num2);
				}
			}
			BoneWeight weights = vertex.weights;
			Vector3 zero = Vector3.zero;
			Vector3 zero2 = Vector3.zero;
			this.Skin(weights.boneIndex0, weights.weight0, vector, vector2, ref zero, ref zero2);
			this.Skin(weights.boneIndex1, weights.weight1, vector, vector2, ref zero, ref zero2);
			this.Skin(weights.boneIndex2, weights.weight2, vector, vector2, ref zero, ref zero2);
			this.Skin(weights.boneIndex3, weights.weight3, vector, vector2, ref zero, ref zero2);
			this._world[sample] = zero;
			float magnitude = zero2.magnitude;
			this._worldNormal[sample] = ((magnitude > 1E-08f) ? (zero2 / magnitude) : Vector3.up);
		}

		// Token: 0x0600758B RID: 30091 RVA: 0x002F9CE8 File Offset: 0x002F7EE8
		private void Skin(int bone, float weight, Vector3 position, Vector3 normal, ref Vector3 world, ref Vector3 worldNormal)
		{
			if (weight <= 0f)
			{
				return;
			}
			if (this._boneFrame[bone] != this._frameId)
			{
				this._boneFrame[bone] = this._frameId;
				Transform transform = this._bones[bone];
				this._skin[bone] = ((transform != null) ? (transform.localToWorldMatrix * this._data.bindposes[bone]) : Matrix4x4.identity);
			}
			Matrix4x4 matrix4x = this._skin[bone];
			world += matrix4x.MultiplyPoint3x4(position) * weight;
			worldNormal += matrix4x.MultiplyVector(normal) * weight;
		}

		// Token: 0x0600758C RID: 30092 RVA: 0x002F9DB0 File Offset: 0x002F7FB0
		private void BuildNeighbours()
		{
			this._neighbours = new int[this._count * 8];
			float[] array = new float[8];
			for (int i = 0; i < this._count; i++)
			{
				int num = ((i < this._leftCount) ? 0 : this._leftCount);
				int num2 = ((i < this._leftCount) ? this._leftCount : this._count);
				int num3 = i * 8;
				for (int j = 0; j < 8; j++)
				{
					this._neighbours[num3 + j] = -1;
					array[j] = float.PositiveInfinity;
				}
				Vector3 position = this._data.vertices[i].position;
				for (int k = num; k < num2; k++)
				{
					if (k != i)
					{
						float sqrMagnitude = (this._data.vertices[k].position - position).sqrMagnitude;
						if (sqrMagnitude < array[7])
						{
							int num4 = 7;
							while (num4 > 0 && sqrMagnitude < array[num4 - 1])
							{
								array[num4] = array[num4 - 1];
								this._neighbours[num3 + num4] = this._neighbours[num3 + num4 - 1];
								num4--;
							}
							array[num4] = sqrMagnitude;
							this._neighbours[num3 + num4] = k;
						}
					}
				}
			}
		}

		// Token: 0x0600758D RID: 30093 RVA: 0x002F9EF4 File Offset: 0x002F80F4
		private void BuildBounds(int side)
		{
			int num = ((side == 0) ? 0 : this._leftCount);
			int num2 = ((side == 0) ? this._leftCount : this._count);
			Vector3 vector = Vector3.zero;
			for (int i = num; i < num2; i++)
			{
				vector += this._data.vertices[i].position;
			}
			vector /= (float)Mathf.Max(1, num2 - num);
			int num3 = num;
			float num4 = float.PositiveInfinity;
			for (int j = num; j < num2; j++)
			{
				float sqrMagnitude = (this._data.vertices[j].position - vector).sqrMagnitude;
				if (sqrMagnitude < num4)
				{
					num4 = sqrMagnitude;
					num3 = j;
				}
			}
			float num5 = 0f;
			Vector3 position = this._data.vertices[num3].position;
			for (int k = num; k < num2; k++)
			{
				num5 = Mathf.Max(num5, (this._data.vertices[k].position - position).magnitude);
			}
			this._centreSample[side] = num3;
			this._bindRadius[side] = num5;
		}

		// Token: 0x0600758E RID: 30094 RVA: 0x002FA018 File Offset: 0x002F8218
		private static bool IsFinite(Vector3 value)
		{
			return !float.IsNaN(value.x) && !float.IsNaN(value.y) && !float.IsNaN(value.z) && !float.IsInfinity(value.x) && !float.IsInfinity(value.y) && !float.IsInfinity(value.z);
		}

		// Token: 0x0400784F RID: 30799
		public const int Neighbours = 8;

		// Token: 0x04007850 RID: 30800
		private const float BoundsGrowth = 1.6f;

		// Token: 0x04007851 RID: 30801
		private const int MaxWalkSteps = 64;

		// Token: 0x04007852 RID: 30802
		private const float MinPush = 1E-06f;

		// Token: 0x04007853 RID: 30803
		private NipplePiercingSurfaceBinding _data;

		// Token: 0x04007854 RID: 30804
		private SkinnedMeshRenderer _body;

		// Token: 0x04007855 RID: 30805
		private Mesh _boundMesh;

		// Token: 0x04007856 RID: 30806
		private Transform[] _bones;

		// Token: 0x04007857 RID: 30807
		private int _count;

		// Token: 0x04007858 RID: 30808
		private int _leftCount;

		// Token: 0x04007859 RID: 30809
		private int[] _neighbours;

		// Token: 0x0400785A RID: 30810
		private int[] _shapeIndex;

		// Token: 0x0400785B RID: 30811
		private int _frameId;

		// Token: 0x0400785C RID: 30812
		private int[] _sampleFrame;

		// Token: 0x0400785D RID: 30813
		private int[] _boneFrame;

		// Token: 0x0400785E RID: 30814
		private Vector3[] _world;

		// Token: 0x0400785F RID: 30815
		private Vector3[] _worldNormal;

		// Token: 0x04007860 RID: 30816
		private Matrix4x4[] _skin;

		// Token: 0x04007861 RID: 30817
		private int _activeCount;

		// Token: 0x04007862 RID: 30818
		private int[] _activeShape;

		// Token: 0x04007863 RID: 30819
		private int[] _activeLower;

		// Token: 0x04007864 RID: 30820
		private int[] _activeUpper;

		// Token: 0x04007865 RID: 30821
		private float[] _activeT;

		// Token: 0x04007866 RID: 30822
		private float _scale = 1f;

		// Token: 0x04007867 RID: 30823
		private bool _valid;

		// Token: 0x04007868 RID: 30824
		private readonly int[] _centreSample = new int[2];

		// Token: 0x04007869 RID: 30825
		private readonly float[] _bindRadius = new float[2];
	}
}
