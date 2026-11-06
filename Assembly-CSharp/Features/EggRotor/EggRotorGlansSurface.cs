using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.EggRotor
{
	// Token: 0x0200122B RID: 4651
	public sealed class EggRotorGlansSurface
	{
		// Token: 0x17001720 RID: 5920
		// (get) Token: 0x0600907A RID: 36986 RVA: 0x003B2667 File Offset: 0x003B0867
		public bool IsBound
		{
			get
			{
				return this._renderer != null && this._mesh != null && this._renderer.sharedMesh == this._mesh;
			}
		}

		// Token: 0x17001721 RID: 5921
		// (get) Token: 0x0600907B RID: 36987 RVA: 0x003B269D File Offset: 0x003B089D
		public SkinnedMeshRenderer Renderer
		{
			get
			{
				return this._renderer;
			}
		}

		// Token: 0x17001722 RID: 5922
		// (get) Token: 0x0600907C RID: 36988 RVA: 0x003B26A5 File Offset: 0x003B08A5
		// (set) Token: 0x0600907D RID: 36989 RVA: 0x003B26AD File Offset: 0x003B08AD
		public float WorldRadius { get; private set; }

		// Token: 0x17001723 RID: 5923
		// (get) Token: 0x0600907E RID: 36990 RVA: 0x003B26B6 File Offset: 0x003B08B6
		// (set) Token: 0x0600907F RID: 36991 RVA: 0x003B26BE File Offset: 0x003B08BE
		public Vector3 TipPoint { get; private set; }

		// Token: 0x17001724 RID: 5924
		// (get) Token: 0x06009080 RID: 36992 RVA: 0x003B26C7 File Offset: 0x003B08C7
		// (set) Token: 0x06009081 RID: 36993 RVA: 0x003B26CF File Offset: 0x003B08CF
		public Vector3 TipAxis { get; private set; }

		// Token: 0x17001725 RID: 5925
		// (get) Token: 0x06009082 RID: 36994 RVA: 0x003B26D8 File Offset: 0x003B08D8
		// (set) Token: 0x06009083 RID: 36995 RVA: 0x003B26E0 File Offset: 0x003B08E0
		public bool HasTip { get; private set; }

		// Token: 0x06009084 RID: 36996 RVA: 0x003B26EC File Offset: 0x003B08EC
		public bool Bind(SkinnedMeshRenderer renderer, string tipBoneName, float middle, float span)
		{
			this.Unbind();
			if (renderer == null || renderer.sharedMesh == null || !renderer.sharedMesh.isReadable)
			{
				return false;
			}
			Mesh sharedMesh = renderer.sharedMesh;
			Transform[] bones = renderer.bones;
			Matrix4x4[] bindposes = sharedMesh.bindposes;
			int num = Array.FindIndex<Transform>(bones, (Transform bone) => bone != null && bone.name == tipBoneName);
			if (num < 0 || num >= bindposes.Length)
			{
				return false;
			}
			if (middle <= 0f || span <= 0f || float.IsInfinity(middle) || float.IsInfinity(span))
			{
				return false;
			}
			Vector3[] vertices = sharedMesh.vertices;
			BoneWeight[] boneWeights = sharedMesh.boneWeights;
			if (boneWeights.Length != vertices.Length)
			{
				return false;
			}
			Matrix4x4 matrix4x = bindposes[num];
			Vector3[] array = new Vector3[vertices.Length];
			float num2 = float.NegativeInfinity;
			for (int i = 0; i < vertices.Length; i++)
			{
				array[i] = matrix4x.MultiplyPoint3x4(vertices[i]);
				num2 = Mathf.Max(num2, array[i].y);
			}
			float[] array2 = new float[]
			{
				middle - span,
				middle,
				middle + span
			};
			List<int>[] array3 = new List<int>[8];
			for (int j = 0; j < array2.Length; j++)
			{
				List<int> list;
				List<int> list2;
				if (!EggRotorGlansSurface.SelectPatches(array, num2, array2[j], out list, out list2))
				{
					return false;
				}
				array3[j * 2] = list;
				array3[j * 2 + 1] = list2;
			}
			array3[6] = new List<int>();
			array3[7] = new List<int>();
			for (int k = 0; k < array.Length; k++)
			{
				float num3 = num2 - array[k].y;
				if (num3 <= 0.0003f)
				{
					array3[6].Add(k);
				}
				else if (num3 >= 0.006f && num3 <= 0.008f)
				{
					array3[7].Add(k);
				}
			}
			if (array3[6].Count == 0 || array3[7].Count == 0)
			{
				return false;
			}
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			List<int> list3 = new List<int>();
			this._patchStart = new int[8];
			this._patchCount = new int[8];
			List<int> list4 = new List<int>();
			for (int l = 0; l < 8; l++)
			{
				this._patchStart[l] = list4.Count;
				this._patchCount[l] = array3[l].Count;
				foreach (int num4 in array3[l])
				{
					int count;
					if (!dictionary.TryGetValue(num4, out count))
					{
						count = list3.Count;
						dictionary[num4] = count;
						list3.Add(num4);
					}
					list4.Add(count);
				}
			}
			this._patchVertex = list4.ToArray();
			this._base = new Vector3[list3.Count];
			this._weights = new BoneWeight[list3.Count];
			HashSet<int> hashSet = new HashSet<int>();
			for (int m = 0; m < list3.Count; m++)
			{
				this._base[m] = vertices[list3[m]];
				BoneWeight boneWeight = boneWeights[list3[m]];
				this._weights[m] = boneWeight;
				if (boneWeight.weight0 > 0f)
				{
					hashSet.Add(boneWeight.boneIndex0);
				}
				if (boneWeight.weight1 > 0f)
				{
					hashSet.Add(boneWeight.boneIndex1);
				}
				if (boneWeight.weight2 > 0f)
				{
					hashSet.Add(boneWeight.boneIndex2);
				}
				if (boneWeight.weight3 > 0f)
				{
					hashSet.Add(boneWeight.boneIndex3);
				}
			}
			foreach (int num5 in hashSet)
			{
				if (num5 < 0 || num5 >= bones.Length || num5 >= bindposes.Length || bones[num5] == null)
				{
					return false;
				}
			}
			this._usedBones = new int[hashSet.Count];
			hashSet.CopyTo(this._usedBones);
			List<int> list5 = new List<int>();
			List<float> list6 = new List<float>();
			List<Vector3[]> list7 = new List<Vector3[]>();
			Vector3[] array4 = new Vector3[vertices.Length];
			for (int n = 0; n < sharedMesh.blendShapeCount; n++)
			{
				int blendShapeFrameCount = sharedMesh.GetBlendShapeFrameCount(n);
				if (blendShapeFrameCount > 0)
				{
					sharedMesh.GetBlendShapeFrameVertices(n, blendShapeFrameCount - 1, array4, null, null);
					Vector3[] array5 = null;
					for (int num6 = 0; num6 < list3.Count; num6++)
					{
						Vector3 vector = array4[list3[num6]];
						if (vector.sqrMagnitude >= 1E-14f)
						{
							if (array5 == null)
							{
								array5 = new Vector3[list3.Count];
							}
							array5[num6] = vector;
						}
					}
					if (array5 != null)
					{
						list5.Add(n);
						float blendShapeFrameWeight = sharedMesh.GetBlendShapeFrameWeight(n, blendShapeFrameCount - 1);
						list6.Add((Mathf.Abs(blendShapeFrameWeight) > 0.0001f) ? blendShapeFrameWeight : 100f);
						list7.Add(array5);
					}
				}
			}
			this._shapeIndex = list5.ToArray();
			this._shapeFrameWeight = list6.ToArray();
			this._shapeFactor = new float[this._shapeIndex.Length];
			this._shapeDelta = list7.ToArray();
			this._world = new Vector3[list3.Count];
			this._skin = new Matrix4x4[bones.Length];
			this._renderer = renderer;
			this._mesh = sharedMesh;
			this._bones = bones;
			this._bindposes = bindposes;
			this._middle = middle;
			this._span = span;
			return true;
		}

		// Token: 0x06009085 RID: 36997 RVA: 0x003B2CC8 File Offset: 0x003B0EC8
		public bool IsBoundTo(SkinnedMeshRenderer renderer, float middle, float span)
		{
			return this.IsBound && renderer == this._renderer && Mathf.Approximately(middle, this._middle) && Mathf.Approximately(span, this._span);
		}

		// Token: 0x06009086 RID: 36998 RVA: 0x003B2CFC File Offset: 0x003B0EFC
		public void Unbind()
		{
			this._renderer = null;
			this._mesh = null;
			this._bones = null;
			this._bindposes = null;
			this._base = null;
			this._world = null;
			this._skin = null;
			this.WorldRadius = 0f;
		}

		// Token: 0x06009087 RID: 36999 RVA: 0x003B2D3C File Offset: 0x003B0F3C
		public bool TryEvaluate(out Pose frame)
		{
			frame = Pose.identity;
			this.HasTip = false;
			if (!this.IsBound)
			{
				return false;
			}
			for (int i = 0; i < this._usedBones.Length; i++)
			{
				int num = this._usedBones[i];
				Transform transform = this._bones[num];
				if (transform == null)
				{
					return false;
				}
				this._skin[num] = transform.localToWorldMatrix * this._bindposes[num];
			}
			for (int j = 0; j < this._shapeIndex.Length; j++)
			{
				this._shapeFactor[j] = this._renderer.GetBlendShapeWeight(this._shapeIndex[j]) / this._shapeFrameWeight[j];
			}
			for (int k = 0; k < this._base.Length; k++)
			{
				Vector3 vector = this._base[k];
				for (int l = 0; l < this._shapeIndex.Length; l++)
				{
					float num2 = this._shapeFactor[l];
					if (num2 != 0f)
					{
						Vector3 vector2 = this._shapeDelta[l][k];
						if (vector2.x != 0f || vector2.y != 0f || vector2.z != 0f)
						{
							vector += vector2 * num2;
						}
					}
				}
				BoneWeight boneWeight = this._weights[k];
				Vector3 vector3 = this.Skin(boneWeight.boneIndex0, vector) * boneWeight.weight0;
				if (boneWeight.weight1 > 0f)
				{
					vector3 += this.Skin(boneWeight.boneIndex1, vector) * boneWeight.weight1;
				}
				if (boneWeight.weight2 > 0f)
				{
					vector3 += this.Skin(boneWeight.boneIndex2, vector) * boneWeight.weight2;
				}
				if (boneWeight.weight3 > 0f)
				{
					vector3 += this.Skin(boneWeight.boneIndex3, vector) * boneWeight.weight3;
				}
				this._world[k] = vector3;
			}
			Vector3 vector4 = this.Average(0);
			Vector3 vector5 = this.Average(1);
			Vector3 vector6 = this.Average(2);
			Vector3 vector7 = this.Average(3);
			Vector3 vector8 = this.Average(4);
			Vector3 vector9 = this.Average(5);
			Vector3 vector10 = this.Average(6);
			Vector3 vector11 = vector10 - this.Average(7);
			if (EggRotorPlacementMath.IsFinite(vector10) && EggRotorPlacementMath.IsFinite(vector11) && vector11.sqrMagnitude > 1E-12f)
			{
				this.TipPoint = vector10;
				this.TipAxis = vector11.normalized;
				this.HasTip = true;
			}
			float num3;
			if (!EggRotorGlansSurface.TryFrame(vector4, vector5, vector6, vector7, vector8, vector9, out frame, out num3))
			{
				return false;
			}
			this.WorldRadius = num3;
			return true;
		}

		// Token: 0x06009088 RID: 37000 RVA: 0x003B3018 File Offset: 0x003B1218
		public static bool TryFrame(Vector3 frontVentral, Vector3 frontDorsal, Vector3 middleVentral, Vector3 middleDorsal, Vector3 backVentral, Vector3 backDorsal, out Pose frame, out float radius)
		{
			frame = Pose.identity;
			radius = 0f;
			Vector3 vector = (frontVentral + frontDorsal) * 0.5f - (backVentral + backDorsal) * 0.5f;
			if (!EggRotorPlacementMath.IsFinite(vector) || vector.sqrMagnitude < 1E-12f)
			{
				return false;
			}
			vector.Normalize();
			Vector3 vector2 = (middleVentral + middleDorsal) * 0.5f;
			Vector3 vector3 = middleVentral - vector2;
			vector3 -= vector * Vector3.Dot(vector3, vector);
			if (!EggRotorPlacementMath.IsFinite(vector3) || vector3.sqrMagnitude < 1E-12f)
			{
				return false;
			}
			radius = Vector3.Distance(middleVentral, middleDorsal) * 0.5f;
			frame = new Pose(middleVentral, Quaternion.LookRotation(vector3.normalized, vector));
			return EggRotorPlacementMath.IsFinite(frame.position) && EggRotorPlacementMath.IsFinite(frame.rotation);
		}

		// Token: 0x06009089 RID: 37001 RVA: 0x003B310E File Offset: 0x003B130E
		private Vector3 Skin(int bone, Vector3 v)
		{
			return this._skin[bone].MultiplyPoint3x4(v);
		}

		// Token: 0x0600908A RID: 37002 RVA: 0x003B3124 File Offset: 0x003B1324
		private Vector3 Average(int patch)
		{
			Vector3 vector = Vector3.zero;
			int num = this._patchStart[patch];
			int num2 = this._patchCount[patch];
			for (int i = 0; i < num2; i++)
			{
				vector += this._world[this._patchVertex[num + i]];
			}
			return vector / (float)Mathf.Max(1, num2);
		}

		// Token: 0x0600908B RID: 37003 RVA: 0x003B3180 File Offset: 0x003B1380
		private static bool SelectPatches(Vector3[] local, float end, float station, out List<int> ventral, out List<int> dorsal)
		{
			ventral = new List<int>();
			dorsal = new List<int>();
			float num = float.NegativeInfinity;
			float num2 = float.PositiveInfinity;
			for (int i = 0; i < local.Length; i++)
			{
				if (EggRotorGlansSurface.InSlab(local[i], end, station))
				{
					num = Mathf.Max(num, local[i].z);
					num2 = Mathf.Min(num2, local[i].z);
				}
			}
			if (float.IsInfinity(num) || num - num2 < 0.002f)
			{
				return false;
			}
			for (int j = 0; j < local.Length; j++)
			{
				if (EggRotorGlansSurface.InSlab(local[j], end, station))
				{
					if (local[j].z >= num - 0.0015f)
					{
						ventral.Add(j);
					}
					else if (local[j].z <= num2 + 0.0015f)
					{
						dorsal.Add(j);
					}
				}
			}
			return ventral.Count > 0 && dorsal.Count > 0;
		}

		// Token: 0x0600908C RID: 37004 RVA: 0x003B3272 File Offset: 0x003B1472
		private static bool InSlab(Vector3 p, float end, float station)
		{
			return Mathf.Abs(end - p.y - station) <= 0.0025f && Mathf.Abs(p.x) <= 0.0035f;
		}

		// Token: 0x0400947D RID: 38013
		private const float SlabHalfDepth = 0.0025f;

		// Token: 0x0400947E RID: 38014
		private const float PatchHalfWidth = 0.0035f;

		// Token: 0x0400947F RID: 38015
		private const float PatchDepth = 0.0015f;

		// Token: 0x04009480 RID: 38016
		private const int StationPatches = 6;

		// Token: 0x04009481 RID: 38017
		private const int TipEndPatch = 6;

		// Token: 0x04009482 RID: 38018
		private const int TipRingPatch = 7;

		// Token: 0x04009483 RID: 38019
		private const int Patches = 8;

		// Token: 0x04009484 RID: 38020
		private const float TipEndDepth = 0.0003f;

		// Token: 0x04009485 RID: 38021
		private const float TipRingNear = 0.006f;

		// Token: 0x04009486 RID: 38022
		private const float TipRingFar = 0.008f;

		// Token: 0x04009487 RID: 38023
		private SkinnedMeshRenderer _renderer;

		// Token: 0x04009488 RID: 38024
		private Mesh _mesh;

		// Token: 0x04009489 RID: 38025
		private Transform[] _bones;

		// Token: 0x0400948A RID: 38026
		private Matrix4x4[] _bindposes;

		// Token: 0x0400948B RID: 38027
		private Vector3[] _base;

		// Token: 0x0400948C RID: 38028
		private BoneWeight[] _weights;

		// Token: 0x0400948D RID: 38029
		private int[] _patchStart;

		// Token: 0x0400948E RID: 38030
		private int[] _patchCount;

		// Token: 0x0400948F RID: 38031
		private int[] _patchVertex;

		// Token: 0x04009490 RID: 38032
		private int[] _shapeIndex;

		// Token: 0x04009491 RID: 38033
		private float[] _shapeFrameWeight;

		// Token: 0x04009492 RID: 38034
		private float[] _shapeFactor;

		// Token: 0x04009493 RID: 38035
		private Vector3[][] _shapeDelta;

		// Token: 0x04009494 RID: 38036
		private int[] _usedBones;

		// Token: 0x04009495 RID: 38037
		private Matrix4x4[] _skin;

		// Token: 0x04009496 RID: 38038
		private Vector3[] _world;

		// Token: 0x04009497 RID: 38039
		private float _middle;

		// Token: 0x04009498 RID: 38040
		private float _span;
	}
}
