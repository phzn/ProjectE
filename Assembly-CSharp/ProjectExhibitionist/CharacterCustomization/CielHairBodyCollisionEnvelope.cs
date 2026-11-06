using System;
using System.Collections.Generic;
using MagicaCloth2;
using RayznGames.DressRigger;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectExhibitionist.CharacterCustomization
{
	// Token: 0x02000AC5 RID: 2757
	[DisallowMultipleComponent]
	public sealed class CielHairBodyCollisionEnvelope : MonoBehaviour
	{
		// Token: 0x17000D1F RID: 3359
		// (get) Token: 0x06004E45 RID: 20037 RVA: 0x001E5884 File Offset: 0x001E3A84
		public CielHairBodySurfaceBinding Binding
		{
			get
			{
				return this.binding;
			}
		}

		// Token: 0x17000D20 RID: 3360
		// (get) Token: 0x06004E46 RID: 20038 RVA: 0x001E588C File Offset: 0x001E3A8C
		public SkinnedMeshRenderer Body
		{
			get
			{
				return this._body;
			}
		}

		// Token: 0x17000D21 RID: 3361
		// (get) Token: 0x06004E47 RID: 20039 RVA: 0x001E5894 File Offset: 0x001E3A94
		public IReadOnlyList<MagicaCapsuleCollider> OwnedColliders
		{
			get
			{
				return this._colliders;
			}
		}

		// Token: 0x17000D22 RID: 3362
		// (get) Token: 0x06004E48 RID: 20040 RVA: 0x001E589C File Offset: 0x001E3A9C
		public int SampleCount
		{
			get
			{
				if (this._positions == null)
				{
					return 0;
				}
				return this._positions.Length;
			}
		}

		// Token: 0x17000D23 RID: 3363
		// (get) Token: 0x06004E49 RID: 20041 RVA: 0x001E58B0 File Offset: 0x001E3AB0
		public bool Ready
		{
			get
			{
				return this._body != null && this._volumes != null && this._hair != null;
			}
		}

		// Token: 0x17000D24 RID: 3364
		// (get) Token: 0x06004E4A RID: 20042 RVA: 0x001E58DC File Offset: 0x001E3ADC
		// (set) Token: 0x06004E4B RID: 20043 RVA: 0x001E58E4 File Offset: 0x001E3AE4
		public int SurfaceCaptureCount { get; private set; }

		// Token: 0x17000D25 RID: 3365
		// (get) Token: 0x06004E4C RID: 20044 RVA: 0x001E58ED File Offset: 0x001E3AED
		public float ClearanceMeters
		{
			get
			{
				return this.clearanceMeters;
			}
		}

		// Token: 0x06004E4D RID: 20045 RVA: 0x001E58F5 File Offset: 0x001E3AF5
		public void Configure(CielHairBodySurfaceBinding value)
		{
			this.Release();
			this.binding = value;
			this._nextResolve = 0f;
		}

		// Token: 0x06004E4E RID: 20046 RVA: 0x001E5910 File Offset: 0x001E3B10
		private void OnEnable()
		{
			this._nextResolve = 0f;
			if (!Application.isPlaying)
			{
				return;
			}
			MagicaManager.OnPreSimulation = (Action)Delegate.Combine(MagicaManager.OnPreSimulation, new Action(this.PrepareSimulation));
			MagicaManager.OnPostSimulation = (Action)Delegate.Combine(MagicaManager.OnPostSimulation, new Action(this.CaptureAfterSimulation));
		}

		// Token: 0x06004E4F RID: 20047 RVA: 0x001E5970 File Offset: 0x001E3B70
		private void OnDisable()
		{
			MagicaManager.OnPreSimulation = (Action)Delegate.Remove(MagicaManager.OnPreSimulation, new Action(this.PrepareSimulation));
			MagicaManager.OnPostSimulation = (Action)Delegate.Remove(MagicaManager.OnPostSimulation, new Action(this.CaptureAfterSimulation));
			this.Release();
		}

		// Token: 0x06004E50 RID: 20048 RVA: 0x001E59C3 File Offset: 0x001E3BC3
		private void OnDestroy()
		{
			this.OnDisable();
		}

		// Token: 0x06004E51 RID: 20049 RVA: 0x001E59CB File Offset: 0x001E3BCB
		private void Start()
		{
			this.Resolve();
		}

		// Token: 0x06004E52 RID: 20050 RVA: 0x001E59D4 File Offset: 0x001E3BD4
		private void LateUpdate()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (!this.Ready || this._body.sharedMesh != this._mesh || base.GetComponent<MagicaCloth>() != this._hair)
			{
				this.Resolve();
			}
			if (this.Ready && !this._hair.isActiveAndEnabled && this._captureFrame != Time.frameCount)
			{
				this.CaptureSurface();
				this.ApplyCachedPose();
			}
		}

		// Token: 0x06004E53 RID: 20051 RVA: 0x001E5A50 File Offset: 0x001E3C50
		private void Resolve()
		{
			if (!Application.isPlaying || Time.unscaledTime < this._nextResolve)
			{
				return;
			}
			this._nextResolve = Time.unscaledTime + 0.25f;
			PlayerDresser componentInParent = base.GetComponentInParent<PlayerDresser>(true);
			SkinnedMeshRenderer skinnedMeshRenderer = ((componentInParent != null) ? componentInParent.CharacterBodyRenderer : null);
			MagicaCloth component = base.GetComponent<MagicaCloth>();
			if (skinnedMeshRenderer == null || component == null || component.SerializeData.rootBones.Count == 0)
			{
				return;
			}
			Transform transform = Array.Find<Transform>(skinnedMeshRenderer.bones, (Transform bone) => bone != null && bone.name == "head.x");
			if (transform == null)
			{
				return;
			}
			foreach (Transform transform2 in component.SerializeData.rootBones)
			{
				if (transform2 == null || !transform2.IsChildOf(transform))
				{
					return;
				}
			}
			this.BindBody(skinnedMeshRenderer, component);
		}

		// Token: 0x06004E54 RID: 20052 RVA: 0x001E5B64 File Offset: 0x001E3D64
		public bool BindBody(SkinnedMeshRenderer body, MagicaCloth hair)
		{
			if (this.binding == null || body == null || body.sharedMesh == null || hair == null || hair.SerializeData.clothType != ClothProcess.ClothType.BoneCloth || this.binding.vertices == null || this.binding.vertices.Length == 0 || this.binding.patches == null || this.binding.patches.Length == 0 || this.binding.patches.Length > 5 || this.binding.boneNames == null || this.binding.bindposes == null || this.binding.boneNames.Length != this.binding.bindposes.Length || body.sharedMesh.vertexCount != this.binding.sourceVertexCount)
			{
				return false;
			}
			if (this.Ready && body == this._body && body.sharedMesh == this._mesh && hair == this._hair)
			{
				return true;
			}
			Transform[] bones = body.bones;
			Transform[] array = new Transform[this.binding.boneNames.Length];
			bool[] array2 = new bool[array.Length];
			CielHairBodySurfaceBinding.Vertex[] vertices = this.binding.vertices;
			for (int i = 0; i < vertices.Length; i++)
			{
				BoneWeight weights = vertices[i].weights;
				if (!CielHairBodyCollisionEnvelope.MarkBone(array2, weights.boneIndex0, weights.weight0) || !CielHairBodyCollisionEnvelope.MarkBone(array2, weights.boneIndex1, weights.weight1) || !CielHairBodyCollisionEnvelope.MarkBone(array2, weights.boneIndex2, weights.weight2) || !CielHairBodyCollisionEnvelope.MarkBone(array2, weights.boneIndex3, weights.weight3))
				{
					return false;
				}
			}
			foreach (CielHairBodySurfaceBinding.Patch patch in this.binding.patches)
			{
				if (patch == null || patch.vertices == null || patch.vertices.Length < 3 || patch.frameBone < 0 || patch.frameBone >= array2.Length)
				{
					return false;
				}
				array2[patch.frameBone] = true;
				foreach (int num in patch.vertices)
				{
					if (num < 0 || num >= this.binding.vertices.Length)
					{
						return false;
					}
				}
			}
			for (int k = 0; k < array.Length; k++)
			{
				if (array2[k])
				{
					string name = this.binding.boneNames[k];
					array[k] = Array.Find<Transform>(bones, (Transform bone) => bone != null && bone.name == name);
					if (array[k] == null)
					{
						return false;
					}
				}
			}
			this.Release();
			this._hair = hair;
			this._body = body;
			this._mesh = body.sharedMesh;
			this._bones = array;
			this._usedBones = array2;
			this._skin = new Matrix4x4[array.Length];
			this._positions = new Vector3[this.binding.vertices.Length];
			this._world = new Vector3[this._positions.Length];
			this._shapeIndices = new int[(this.binding.shapes != null) ? this.binding.shapes.Length : 0];
			for (int l = 0; l < this._shapeIndices.Length; l++)
			{
				this._shapeIndices[l] = this._mesh.GetBlendShapeIndex(this.binding.shapes[l].name);
			}
			this._poses = new CielHairBodyCollisionEnvelope.Pose[this.binding.patches.Length];
			GameObject gameObject = new GameObject("Hair-only body collision [" + base.GetInstanceID().ToString() + "]");
			gameObject.hideFlags = HideFlags.DontSave;
			if (base.gameObject.scene.IsValid() && gameObject.scene != base.gameObject.scene)
			{
				SceneManager.MoveGameObjectToScene(gameObject, base.gameObject.scene);
			}
			this._volumes = gameObject.transform;
			CielHairBodySurfaceBinding.Patch[] array3 = this.binding.patches;
			for (int i = 0; i < array3.Length; i++)
			{
				GameObject gameObject2 = new GameObject(array3[i].name);
				gameObject2.hideFlags = HideFlags.DontSave;
				gameObject2.transform.SetParent(this._volumes, false);
				MagicaCapsuleCollider magicaCapsuleCollider = gameObject2.AddComponent<MagicaCapsuleCollider>();
				magicaCapsuleCollider.direction = MagicaCapsuleCollider.Direction.Y;
				magicaCapsuleCollider.alignedOnCenter = true;
				this._colliders.Add(magicaCapsuleCollider);
			}
			this.CaptureSurface();
			this.ApplyCachedPose();
			List<ColliderComponent> colliderList = this._hair.SerializeData.colliderCollisionConstraint.colliderList;
			foreach (MagicaCapsuleCollider magicaCapsuleCollider2 in this._colliders)
			{
				colliderList.Add(magicaCapsuleCollider2);
			}
			if (this._hair.IsValid())
			{
				this._hair.SetParameterChange();
			}
			return true;
		}

		// Token: 0x06004E55 RID: 20053 RVA: 0x001E6088 File Offset: 0x001E4288
		private static bool MarkBone(bool[] used, int bone, float weight)
		{
			if (weight <= 0f)
			{
				return true;
			}
			if (bone < 0 || bone >= used.Length)
			{
				return false;
			}
			used[bone] = true;
			return true;
		}

		// Token: 0x06004E56 RID: 20054 RVA: 0x001E60A5 File Offset: 0x001E42A5
		private void PrepareSimulation()
		{
			if (!this.Ready)
			{
				this.Resolve();
			}
			if (this.Ready)
			{
				this.ApplyCachedPose();
			}
		}

		// Token: 0x06004E57 RID: 20055 RVA: 0x001E60C3 File Offset: 0x001E42C3
		private void CaptureAfterSimulation()
		{
			if (!this.Ready)
			{
				return;
			}
			this.CaptureSurface();
			this.ApplyCachedPose();
		}

		// Token: 0x06004E58 RID: 20056 RVA: 0x001E60DC File Offset: 0x001E42DC
		public void CaptureSurface()
		{
			if (!this.Ready || this._body.sharedMesh != this._mesh)
			{
				return;
			}
			for (int i = 0; i < this._positions.Length; i++)
			{
				this._positions[i] = this.binding.vertices[i].position;
			}
			for (int j = 0; j < this._shapeIndices.Length; j++)
			{
				if (this._shapeIndices[j] >= 0)
				{
					float blendShapeWeight = this._body.GetBlendShapeWeight(this._shapeIndices[j]);
					if (Mathf.Abs(blendShapeWeight) >= 1E-05f && CielHairBodyCollisionEnvelope.Finite(blendShapeWeight))
					{
						CielHairBodySurfaceBinding.Frame[] frames = this.binding.shapes[j].frames;
						if (frames != null && frames.Length != 0)
						{
							int num = 0;
							while (num < frames.Length - 1 && blendShapeWeight > frames[num].weight)
							{
								num++;
							}
							CielHairBodySurfaceBinding.Frame frame = frames[num];
							CielHairBodySurfaceBinding.Frame frame2 = ((num > 0) ? frames[num - 1] : null);
							float num2 = ((frame2 != null) ? frame2.weight : 0f);
							float num3 = (blendShapeWeight - num2) / Mathf.Max(1E-05f, frame.weight - num2);
							for (int k = 0; k < this._positions.Length; k++)
							{
								this._positions[k] += Vector3.LerpUnclamped((frame2 != null) ? frame2.deltas[k] : Vector3.zero, frame.deltas[k], num3);
							}
						}
					}
				}
			}
			for (int l = 0; l < this._skin.Length; l++)
			{
				if (this._usedBones[l] && this._bones[l] != null)
				{
					this._skin[l] = this._bones[l].localToWorldMatrix * this.binding.bindposes[l];
				}
			}
			for (int m = 0; m < this._world.Length; m++)
			{
				BoneWeight weights = this.binding.vertices[m].weights;
				Vector3 vector = this._positions[m];
				this._world[m] = this.Skin(vector, weights.boneIndex0, weights.weight0) + this.Skin(vector, weights.boneIndex1, weights.weight1) + this.Skin(vector, weights.boneIndex2, weights.weight2) + this.Skin(vector, weights.boneIndex3, weights.weight3);
			}
			float num4 = ((this.shrinkSharpness <= 0f) ? 1f : (1f - Mathf.Exp(-this.shrinkSharpness * Mathf.Min(Time.deltaTime, 0.1f))));
			for (int n = 0; n < this._poses.Length; n++)
			{
				CielHairBodySurfaceBinding.Patch patch = this.binding.patches[n];
				Transform transform = this._bones[patch.frameBone];
				Matrix4x4 matrix4x = Matrix4x4.Rotate(Quaternion.Inverse(patch.patchToFrame)) * transform.worldToLocalMatrix;
				float num5 = this.clearanceMeters / Mathf.Max(0.0001f, Mathf.Abs(transform.lossyScale.x));
				CielHairBodyCollisionEnvelope.Pose pose = this.Fit(patch, matrix4x, num5);
				if (pose.valid)
				{
					if (this._poses[n].valid)
					{
						float num6 = Mathf.Max(0f, pose.length - pose.radius * 2f);
						float num7 = Mathf.Max(0f, this._poses[n].length - this._poses[n].radius * 2f);
						num6 = Mathf.Max(num6, Mathf.Lerp(num7, num6, num4));
						pose.radius = Mathf.Max(pose.radius, Mathf.Lerp(this._poses[n].radius, pose.radius, num4));
						pose.length = num6 + pose.radius * 2f;
					}
					this._poses[n] = pose;
				}
			}
			this._captureFrame = Time.frameCount;
			int surfaceCaptureCount = this.SurfaceCaptureCount;
			this.SurfaceCaptureCount = surfaceCaptureCount + 1;
		}

		// Token: 0x06004E59 RID: 20057 RVA: 0x001E6569 File Offset: 0x001E4769
		private Vector3 Skin(Vector3 point, int bone, float weight)
		{
			if (weight <= 0f)
			{
				return Vector3.zero;
			}
			return this._skin[bone].MultiplyPoint3x4(point) * weight;
		}

		// Token: 0x06004E5A RID: 20058 RVA: 0x001E6594 File Offset: 0x001E4794
		private CielHairBodyCollisionEnvelope.Pose Fit(CielHairBodySurfaceBinding.Patch patch, Matrix4x4 toPatch, float padding)
		{
			float num = float.PositiveInfinity;
			float num2 = float.NegativeInfinity;
			float num3 = float.PositiveInfinity;
			float num4 = float.NegativeInfinity;
			foreach (int num5 in patch.vertices)
			{
				Vector3 vector = toPatch.MultiplyPoint3x4(this._world[num5]);
				num = Mathf.Min(num, vector.x);
				num2 = Mathf.Max(num2, vector.x);
				num3 = Mathf.Min(num3, vector.z);
				num4 = Mathf.Max(num4, vector.z);
			}
			float num6 = (num + num2) * 0.5f;
			float num7 = Mathf.Max(0.001f, Mathf.Max((num2 - num) * 0.56f, (num4 - num3) * 0.56f));
			float num8 = 0f;
			float num9 = 0f;
			for (int j = 0; j < 20; j++)
			{
				num8 = float.NegativeInfinity;
				num9 = float.PositiveInfinity;
				foreach (int num10 in patch.vertices)
				{
					Vector3 vector2 = toPatch.MultiplyPoint3x4(this._world[num10]);
					float num11 = Mathf.Sqrt(Mathf.Max(0f, num7 * num7 - (vector2.x - num6) * (vector2.x - num6)));
					num8 = Mathf.Max(num8, vector2.z - num11);
					num9 = Mathf.Min(num9, vector2.z + num11);
				}
				if (num8 <= num9)
				{
					break;
				}
				num7 *= 1.2f;
			}
			float num12 = (patch.front ? num8 : num9);
			num7 += padding;
			float num13 = float.PositiveInfinity;
			float num14 = float.NegativeInfinity;
			foreach (int num15 in patch.vertices)
			{
				Vector3 vector3 = toPatch.MultiplyPoint3x4(this._world[num15]);
				float num16 = vector3.x - num6;
				float num17 = vector3.z - num12;
				float num18 = Mathf.Sqrt(Mathf.Max(0f, num7 * num7 - num16 * num16 - num17 * num17));
				num13 = Mathf.Min(num13, vector3.y + num18);
				num14 = Mathf.Max(num14, vector3.y - num18);
			}
			float num19 = Mathf.Max(0f, num14 - num13) + num7 * 2f;
			return new CielHairBodyCollisionEnvelope.Pose
			{
				center = new Vector3(num6, (num13 + num14) * 0.5f, num12),
				radius = num7,
				length = num19,
				valid = (CielHairBodyCollisionEnvelope.Finite(num7) && CielHairBodyCollisionEnvelope.Finite(num19) && CielHairBodyCollisionEnvelope.Finite(num12))
			};
		}

		// Token: 0x06004E5B RID: 20059 RVA: 0x001E685C File Offset: 0x001E4A5C
		public void ApplyCachedPose()
		{
			if (!this.Ready)
			{
				return;
			}
			for (int i = 0; i < this._poses.Length; i++)
			{
				CielHairBodyCollisionEnvelope.Pose pose = this._poses[i];
				if (pose.valid)
				{
					CielHairBodySurfaceBinding.Patch patch = this.binding.patches[i];
					Transform transform = this._bones[patch.frameBone];
					if (!(transform == null))
					{
						MagicaCapsuleCollider magicaCapsuleCollider = this._colliders[i];
						magicaCapsuleCollider.transform.SetPositionAndRotation(transform.TransformPoint(patch.patchToFrame * pose.center), transform.rotation * patch.patchToFrame);
						float num = Mathf.Abs(transform.lossyScale.x);
						Vector3 vector = new Vector3(pose.radius, pose.radius, pose.length) * num;
						if ((magicaCapsuleCollider.GetSize() - vector).sqrMagnitude > 1E-12f)
						{
							magicaCapsuleCollider.SetSize(vector);
							magicaCapsuleCollider.UpdateParameters();
						}
					}
				}
			}
		}

		// Token: 0x06004E5C RID: 20060 RVA: 0x001E6970 File Offset: 0x001E4B70
		public float MaximumSampleOutside()
		{
			if (!this.Ready)
			{
				return float.PositiveInfinity;
			}
			float num = float.NegativeInfinity;
			for (int i = 0; i < this.binding.patches.Length; i++)
			{
				MagicaCapsuleCollider magicaCapsuleCollider = this._colliders[i];
				Vector3 size = magicaCapsuleCollider.GetSize();
				float num2 = Mathf.Max(0f, size.z * 0.5f - size.x);
				foreach (int num3 in this.binding.patches[i].vertices)
				{
					Vector3 vector = magicaCapsuleCollider.transform.InverseTransformPoint(this._world[num3]);
					vector.y -= Mathf.Clamp(vector.y, -num2, num2);
					num = Mathf.Max(num, vector.magnitude - size.x);
				}
			}
			return num;
		}

		// Token: 0x06004E5D RID: 20061 RVA: 0x0002920B File Offset: 0x0002740B
		private static bool Finite(float value)
		{
			return !float.IsNaN(value) && !float.IsInfinity(value);
		}

		// Token: 0x06004E5E RID: 20062 RVA: 0x001E6A5C File Offset: 0x001E4C5C
		public void Release()
		{
			this.RemoveOwnedColliders(this._hair);
			foreach (MagicaCloth magicaCloth in base.GetComponents<MagicaCloth>())
			{
				if (magicaCloth != this._hair)
				{
					this.RemoveOwnedColliders(magicaCloth);
				}
			}
			if (this._volumes != null)
			{
				this._volumes.gameObject.SetActive(false);
				if (Application.isPlaying)
				{
					Object.Destroy(this._volumes.gameObject);
				}
				else
				{
					Object.DestroyImmediate(this._volumes.gameObject);
				}
			}
			this._colliders.Clear();
			this._volumes = null;
			this._body = null;
			this._mesh = null;
			this._hair = null;
			this._positions = null;
			this._world = null;
			this._bones = null;
			this._skin = null;
			this._poses = null;
			this._usedBones = null;
			this._shapeIndices = null;
			this._captureFrame = -1;
		}

		// Token: 0x06004E5F RID: 20063 RVA: 0x001E6B4C File Offset: 0x001E4D4C
		private void RemoveOwnedColliders(MagicaCloth cloth)
		{
			if (cloth == null || this._colliders.Count == 0)
			{
				return;
			}
			List<ColliderComponent> colliderList = cloth.SerializeData.colliderCollisionConstraint.colliderList;
			bool flag = false;
			foreach (MagicaCapsuleCollider magicaCapsuleCollider in this._colliders)
			{
				while (colliderList.Remove(magicaCapsuleCollider))
				{
					flag = true;
				}
			}
			if (flag && cloth.IsValid())
			{
				cloth.SetParameterChange();
			}
		}

		// Token: 0x04004F95 RID: 20373
		[SerializeField]
		private CielHairBodySurfaceBinding binding;

		// Token: 0x04004F96 RID: 20374
		[SerializeField]
		[Min(0f)]
		private float clearanceMeters = 0.003f;

		// Token: 0x04004F97 RID: 20375
		[SerializeField]
		[Min(0f)]
		private float shrinkSharpness = 20f;

		// Token: 0x04004F98 RID: 20376
		private MagicaCloth _hair;

		// Token: 0x04004F99 RID: 20377
		private SkinnedMeshRenderer _body;

		// Token: 0x04004F9A RID: 20378
		private Mesh _mesh;

		// Token: 0x04004F9B RID: 20379
		private Transform[] _bones;

		// Token: 0x04004F9C RID: 20380
		private Matrix4x4[] _skin;

		// Token: 0x04004F9D RID: 20381
		private bool[] _usedBones;

		// Token: 0x04004F9E RID: 20382
		private int[] _shapeIndices;

		// Token: 0x04004F9F RID: 20383
		private Vector3[] _positions;

		// Token: 0x04004FA0 RID: 20384
		private Vector3[] _world;

		// Token: 0x04004FA1 RID: 20385
		private Transform _volumes;

		// Token: 0x04004FA2 RID: 20386
		private readonly List<MagicaCapsuleCollider> _colliders = new List<MagicaCapsuleCollider>(5);

		// Token: 0x04004FA3 RID: 20387
		private CielHairBodyCollisionEnvelope.Pose[] _poses;

		// Token: 0x04004FA4 RID: 20388
		private int _captureFrame = -1;

		// Token: 0x04004FA5 RID: 20389
		private float _nextResolve;

		// Token: 0x02000AC6 RID: 2758
		private struct Pose
		{
			// Token: 0x04004FA7 RID: 20391
			public Vector3 center;

			// Token: 0x04004FA8 RID: 20392
			public float radius;

			// Token: 0x04004FA9 RID: 20393
			public float length;

			// Token: 0x04004FAA RID: 20394
			public bool valid;
		}
	}
}
