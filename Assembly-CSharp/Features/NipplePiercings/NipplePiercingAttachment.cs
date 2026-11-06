using System;
using BoZo.CharacterCreator.MC.StyleSystem;
using MagicaCloth2;
using RayznGames.DressRigger;
using UnityEngine;

namespace Features.NipplePiercings
{
	// Token: 0x02000F63 RID: 3939
	[DefaultExecutionOrder(22000)]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(NipplePiercingErection))]
	public sealed class NipplePiercingAttachment : MonoBehaviour
	{
		// Token: 0x17001375 RID: 4981
		// (get) Token: 0x060076A4 RID: 30372 RVA: 0x00302E82 File Offset: 0x00301082
		public Transform LeftAnchor
		{
			get
			{
				return this.leftAnchor;
			}
		}

		// Token: 0x17001376 RID: 4982
		// (get) Token: 0x060076A5 RID: 30373 RVA: 0x00302E8A File Offset: 0x0030108A
		public Transform RightAnchor
		{
			get
			{
				return this.rightAnchor;
			}
		}

		// Token: 0x17001377 RID: 4983
		// (get) Token: 0x060076A6 RID: 30374 RVA: 0x00302E92 File Offset: 0x00301092
		public SkinnedMeshRenderer Body
		{
			get
			{
				return this._body;
			}
		}

		// Token: 0x17001378 RID: 4984
		// (get) Token: 0x060076A7 RID: 30375 RVA: 0x00302E9A File Offset: 0x0030109A
		public NipplePiercingSurfaceBinding Binding
		{
			get
			{
				return this.binding;
			}
		}

		// Token: 0x17001379 RID: 4985
		// (get) Token: 0x060076A8 RID: 30376 RVA: 0x00302EA2 File Offset: 0x003010A2
		public float InsertionDepth
		{
			get
			{
				return this.insertionDepth;
			}
		}

		// Token: 0x060076A9 RID: 30377 RVA: 0x00302EAA File Offset: 0x003010AA
		public void Configure(NipplePiercingSurfaceBinding data, Transform left, Transform right, float depth = 0.0012f)
		{
			this.binding = data;
			this.leftAnchor = left;
			this.rightAnchor = right;
			this.insertionDepth = depth;
			this._resolvedMesh = null;
		}

		// Token: 0x060076AA RID: 30378 RVA: 0x00302ED0 File Offset: 0x003010D0
		private void OnEnable()
		{
			MagicaManager.OnPostSimulation = (Action)Delegate.Combine(MagicaManager.OnPostSimulation, new Action(this.AfterSimulation));
			NipplePiercingUpdateLoop.Add(this);
			this._simulationFrame = -1;
			this._chains = base.GetComponentsInChildren<NipplePiercingChainPhysics>(true);
			this._visibility = base.GetComponent<NipplePiercingVisibility>();
			this._erection = base.GetComponent<NipplePiercingErection>();
		}

		// Token: 0x060076AB RID: 30379 RVA: 0x00302F30 File Offset: 0x00301130
		private void OnDisable()
		{
			MagicaManager.OnPostSimulation = (Action)Delegate.Remove(MagicaManager.OnPostSimulation, new Action(this.AfterSimulation));
			NipplePiercingUpdateLoop.Remove(this);
			if (this._erection != null)
			{
				this._erection.Release();
			}
		}

		// Token: 0x060076AC RID: 30380 RVA: 0x00302F7C File Offset: 0x0030117C
		private void OnDestroy()
		{
			MagicaManager.OnPostSimulation = (Action)Delegate.Remove(MagicaManager.OnPostSimulation, new Action(this.AfterSimulation));
			NipplePiercingUpdateLoop.Remove(this);
		}

		// Token: 0x060076AD RID: 30381 RVA: 0x00302FA4 File Offset: 0x003011A4
		private void OnTransformParentChanged()
		{
			if (this._erection != null)
			{
				this._erection.Release();
			}
			this._body = null;
			this._resolvedMesh = null;
			this._simulationFrame = -1;
		}

		// Token: 0x060076AE RID: 30382 RVA: 0x00302FD4 File Offset: 0x003011D4
		private void Start()
		{
			this.UpdatePose();
			this.ResetPhysics();
		}

		// Token: 0x060076AF RID: 30383 RVA: 0x00302FE2 File Offset: 0x003011E2
		private void AfterSimulation()
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			this.UpdatePose();
		}

		// Token: 0x060076B0 RID: 30384 RVA: 0x00302FF3 File Offset: 0x003011F3
		private void LateUpdate()
		{
			this.UpdatePose();
		}

		// Token: 0x060076B1 RID: 30385 RVA: 0x00302FFB File Offset: 0x003011FB
		internal void RefreshAfterDeformation()
		{
			this.UpdatePose();
			this.StepPhysics();
		}

		// Token: 0x060076B2 RID: 30386 RVA: 0x0030300C File Offset: 0x0030120C
		private void StepPhysics()
		{
			if (this._simulationFrame == Time.frameCount)
			{
				return;
			}
			this._simulationFrame = Time.frameCount;
			float deltaTime = Time.deltaTime;
			if (this._visibility != null && this._visibility.IsCovered)
			{
				this.ResetPhysics();
				return;
			}
			if (this._chains != null)
			{
				NipplePiercingChainPhysics[] chains = this._chains;
				for (int i = 0; i < chains.Length; i++)
				{
					chains[i].StepSimulation(deltaTime);
				}
			}
		}

		// Token: 0x060076B3 RID: 30387 RVA: 0x00303080 File Offset: 0x00301280
		public void ResetPhysics()
		{
			if (this._chains == null)
			{
				this._chains = base.GetComponentsInChildren<NipplePiercingChainPhysics>(true);
			}
			NipplePiercingChainPhysics[] chains = this._chains;
			for (int i = 0; i < chains.Length; i++)
			{
				chains[i].ResetSimulation();
			}
		}

		// Token: 0x060076B4 RID: 30388 RVA: 0x003030C0 File Offset: 0x003012C0
		public bool BindBody(SkinnedMeshRenderer body)
		{
			if (this.binding == null || this.binding.sourceMesh == null || body == null || body.sharedMesh == null || body.sharedMesh.vertexCount != this.binding.sourceMesh.vertexCount)
			{
				return false;
			}
			Transform[] bones = body.bones;
			if (this.binding.bindposes == null || bones.Length != this.binding.bindposes.Length)
			{
				return false;
			}
			NipplePiercingSurfaceBinding.Vertex[] vertices = this.binding.vertices;
			for (int i = 0; i < vertices.Length; i++)
			{
				BoneWeight weights = vertices[i].weights;
				if (!NipplePiercingAttachment.ValidBone(bones, weights.boneIndex0, weights.weight0) || !NipplePiercingAttachment.ValidBone(bones, weights.boneIndex1, weights.weight1) || !NipplePiercingAttachment.ValidBone(bones, weights.boneIndex2, weights.weight2) || !NipplePiercingAttachment.ValidBone(bones, weights.boneIndex3, weights.weight3))
				{
					return false;
				}
			}
			this._body = body;
			this._resolvedMesh = body.sharedMesh;
			this._bones = bones;
			this._shapeIndices = new int[this.binding.shapes.Length];
			for (int j = 0; j < this._shapeIndices.Length; j++)
			{
				this._shapeIndices[j] = body.sharedMesh.GetBlendShapeIndex(this.binding.shapes[j].name);
			}
			this._positions = new Vector3[this.binding.vertices.Length];
			this._normals = new Vector3[this.binding.vertices.Length];
			this._worldPositions = new Vector3[this.binding.vertices.Length];
			this._leftFrame = this.BuildPatchFrame(0, this.binding.leftCount);
			this._rightFrame = this.BuildPatchFrame(this.binding.leftCount, this.binding.vertices.Length);
			return true;
		}

		// Token: 0x060076B5 RID: 30389 RVA: 0x002F966A File Offset: 0x002F786A
		private static bool ValidBone(Transform[] bones, int index, float weight)
		{
			return weight <= 0f || (index >= 0 && index < bones.Length && bones[index] != null);
		}

		// Token: 0x060076B6 RID: 30390 RVA: 0x003032C0 File Offset: 0x003014C0
		private bool ResolveBody()
		{
			if (this._body != null && this._body.sharedMesh == this._resolvedMesh)
			{
				return true;
			}
			if (this._body != null && this.BindBody(this._body))
			{
				return true;
			}
			MCStyleCustomizationService componentInParent = base.GetComponentInParent<MCStyleCustomizationService>();
			Transform transform = ((componentInParent != null) ? componentInParent.transform : base.transform.root);
			PlayerDresser playerDresser = base.GetComponentInParent<PlayerDresser>(true);
			if (playerDresser == null)
			{
				playerDresser = transform.GetComponentInChildren<PlayerDresser>(true);
			}
			if (playerDresser != null && this.BindBody(playerDresser.GetCharacterBody()))
			{
				return true;
			}
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in transform.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				if (skinnedMeshRenderer.name == "Body_sotai_NSFW" && skinnedMeshRenderer.gameObject.activeSelf && this.BindBody(skinnedMeshRenderer))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060076B7 RID: 30391 RVA: 0x003033B8 File Offset: 0x003015B8
		public void UpdatePose()
		{
			if (this.leftAnchor == null || this.rightAnchor == null || !this.ResolveBody())
			{
				return;
			}
			if (this._erection == null)
			{
				this._erection = base.GetComponent<NipplePiercingErection>();
			}
			if (this._erection != null)
			{
				this._erection.ApplyToBody(this._body);
			}
			float num = Mathf.Abs(this._body.transform.lossyScale.x);
			base.transform.SetPositionAndRotation(this._body.transform.position, this._body.transform.rotation * Quaternion.Euler(90f, 0f, 0f));
			Vector3 vector = ((base.transform.parent != null) ? base.transform.parent.lossyScale : Vector3.one);
			base.transform.localScale = new Vector3(num / Mathf.Max(1E-05f, Mathf.Abs(vector.x)), num / Mathf.Max(1E-05f, Mathf.Abs(vector.y)), num / Mathf.Max(1E-05f, Mathf.Abs(vector.z)));
			for (int i = 0; i < this._positions.Length; i++)
			{
				this._positions[i] = this.binding.vertices[i].position;
				this._normals[i] = this.binding.vertices[i].normal;
			}
			for (int j = 0; j < this.binding.shapes.Length; j++)
			{
				int num2 = this._shapeIndices[j];
				if (num2 >= 0)
				{
					float blendShapeWeight = this._body.GetBlendShapeWeight(num2);
					if (Mathf.Abs(blendShapeWeight) >= 1E-05f)
					{
						NipplePiercingSurfaceBinding.Frame[] frames = this.binding.shapes[j].frames;
						if (frames.Length != 0)
						{
							int num3 = 0;
							while (num3 < frames.Length - 1 && blendShapeWeight > frames[num3].weight)
							{
								num3++;
							}
							NipplePiercingSurfaceBinding.Frame frame = frames[num3];
							NipplePiercingSurfaceBinding.Frame frame2 = ((num3 > 0) ? frames[num3 - 1] : null);
							float num4 = ((frame2 != null) ? frame2.weight : 0f);
							float num5 = (blendShapeWeight - num4) / Mathf.Max(1E-05f, frame.weight - num4);
							for (int k = 0; k < this._positions.Length; k++)
							{
								this._positions[k] += Vector3.LerpUnclamped((frame2 != null) ? frame2.positions[k] : Vector3.zero, frame.positions[k], num5);
								this._normals[k] += Vector3.LerpUnclamped((frame2 != null) ? frame2.normals[k] : Vector3.zero, frame.normals[k], num5);
							}
						}
					}
				}
			}
			this.PlaceAnchor(this.leftAnchor, 0, this.binding.leftCount, this._leftFrame);
			this.PlaceAnchor(this.rightAnchor, this.binding.leftCount, this.binding.vertices.Length, this._rightFrame);
		}

		// Token: 0x060076B8 RID: 30392 RVA: 0x00303724 File Offset: 0x00301924
		private NipplePiercingAttachment.PatchFrame BuildPatchFrame(int start, int end)
		{
			Vector3 vector = Vector3.zero;
			for (int i = start; i < end; i++)
			{
				vector += this.binding.vertices[i].normal;
			}
			vector.Normalize();
			Vector3 vector2 = Vector3.Cross(Vector3.forward, vector).normalized;
			if (vector2.sqrMagnitude < 1E-10f)
			{
				vector2 = Vector3.ProjectOnPlane(Vector3.right, vector).normalized;
			}
			NipplePiercingAttachment.PatchFrame patchFrame = new NipplePiercingAttachment.PatchFrame
			{
				minimum = start,
				maximum = start,
				referenceRight = vector2
			};
			float num = float.PositiveInfinity;
			float num2 = float.NegativeInfinity;
			for (int j = start; j < end; j++)
			{
				float num3 = Vector3.Dot(this.binding.vertices[j].position, vector2);
				if (num3 < num)
				{
					num = num3;
					patchFrame.minimum = j;
				}
				if (num3 > num2)
				{
					num2 = num3;
					patchFrame.maximum = j;
				}
			}
			Vector3 vector3 = Vector3.ProjectOnPlane(this.binding.vertices[patchFrame.maximum].position - this.binding.vertices[patchFrame.minimum].position, vector);
			patchFrame.tangentCorrection = ((vector3.sqrMagnitude > 1E-14f) ? Vector3.SignedAngle(vector3, vector2, vector) : 0f);
			return patchFrame;
		}

		// Token: 0x060076B9 RID: 30393 RVA: 0x00303880 File Offset: 0x00301A80
		private void PlaceAnchor(Transform anchor, int start, int end, NipplePiercingAttachment.PatchFrame frame)
		{
			Vector3 vector = Vector3.zero;
			Vector3 zero = Vector3.zero;
			for (int i = start; i < end; i++)
			{
				BoneWeight weights = this.binding.vertices[i].weights;
				Vector3 zero2 = Vector3.zero;
				this.Skin(i, weights.boneIndex0, weights.weight0, ref zero2, ref zero);
				this.Skin(i, weights.boneIndex1, weights.weight1, ref zero2, ref zero);
				this.Skin(i, weights.boneIndex2, weights.weight2, ref zero2, ref zero);
				this.Skin(i, weights.boneIndex3, weights.weight3, ref zero2, ref zero);
				this._worldPositions[i] = zero2;
				vector += zero2;
			}
			zero.Normalize();
			Vector3 vector2 = Vector3.ProjectOnPlane(this._worldPositions[frame.maximum] - this._worldPositions[frame.minimum], zero);
			if (vector2.sqrMagnitude > 1E-14f)
			{
				vector2 = Quaternion.AngleAxis(frame.tangentCorrection, zero) * vector2.normalized;
			}
			else
			{
				for (int j = start; j < end; j++)
				{
					BoneWeight weights2 = this.binding.vertices[j].weights;
					vector2 += this.SkinDirection(frame.referenceRight, weights2.boneIndex0, weights2.weight0);
					vector2 += this.SkinDirection(frame.referenceRight, weights2.boneIndex1, weights2.weight1);
					vector2 += this.SkinDirection(frame.referenceRight, weights2.boneIndex2, weights2.weight2);
					vector2 += this.SkinDirection(frame.referenceRight, weights2.boneIndex3, weights2.weight3);
				}
				vector2 = Vector3.ProjectOnPlane(vector2, zero).normalized;
			}
			Vector3 normalized = Vector3.Cross(zero, vector2).normalized;
			float num = Mathf.Abs(this._body.transform.lossyScale.x);
			anchor.SetPositionAndRotation(vector / (float)(end - start) - zero * (this.insertionDepth * num), Quaternion.LookRotation(zero, normalized));
			Vector3 lossyScale = anchor.parent.lossyScale;
			anchor.localScale = new Vector3(num / Mathf.Max(1E-05f, Mathf.Abs(lossyScale.x)), num / Mathf.Max(1E-05f, Mathf.Abs(lossyScale.y)), num / Mathf.Max(1E-05f, Mathf.Abs(lossyScale.z)));
		}

		// Token: 0x060076BA RID: 30394 RVA: 0x00303B30 File Offset: 0x00301D30
		private void Skin(int vertex, int bone, float weight, ref Vector3 position, ref Vector3 normal)
		{
			if (weight <= 0f || bone >= this._bones.Length || this._bones[bone] == null)
			{
				return;
			}
			Matrix4x4 matrix4x = this._bones[bone].localToWorldMatrix * this.binding.bindposes[bone];
			position += matrix4x.MultiplyPoint3x4(this._positions[vertex]) * weight;
			normal += matrix4x.MultiplyVector(this._normals[vertex]) * weight;
		}

		// Token: 0x060076BB RID: 30395 RVA: 0x00303BE0 File Offset: 0x00301DE0
		private Vector3 SkinDirection(Vector3 direction, int bone, float weight)
		{
			if (weight > 0f && bone < this._bones.Length && !(this._bones[bone] == null))
			{
				return (this._bones[bone].localToWorldMatrix * this.binding.bindposes[bone]).MultiplyVector(direction) * weight;
			}
			return Vector3.zero;
		}

		// Token: 0x040079E0 RID: 31200
		[SerializeField]
		private NipplePiercingSurfaceBinding binding;

		// Token: 0x040079E1 RID: 31201
		[SerializeField]
		private Transform leftAnchor;

		// Token: 0x040079E2 RID: 31202
		[SerializeField]
		private Transform rightAnchor;

		// Token: 0x040079E3 RID: 31203
		[SerializeField]
		private float insertionDepth = 0.0012f;

		// Token: 0x040079E4 RID: 31204
		private SkinnedMeshRenderer _body;

		// Token: 0x040079E5 RID: 31205
		private Mesh _resolvedMesh;

		// Token: 0x040079E6 RID: 31206
		private Transform[] _bones;

		// Token: 0x040079E7 RID: 31207
		private int[] _shapeIndices;

		// Token: 0x040079E8 RID: 31208
		private Vector3[] _positions;

		// Token: 0x040079E9 RID: 31209
		private Vector3[] _normals;

		// Token: 0x040079EA RID: 31210
		private Vector3[] _worldPositions;

		// Token: 0x040079EB RID: 31211
		private NipplePiercingAttachment.PatchFrame _leftFrame;

		// Token: 0x040079EC RID: 31212
		private NipplePiercingAttachment.PatchFrame _rightFrame;

		// Token: 0x040079ED RID: 31213
		private NipplePiercingChainPhysics[] _chains;

		// Token: 0x040079EE RID: 31214
		private NipplePiercingVisibility _visibility;

		// Token: 0x040079EF RID: 31215
		private NipplePiercingErection _erection;

		// Token: 0x040079F0 RID: 31216
		private int _simulationFrame = -1;

		// Token: 0x02000F64 RID: 3940
		private struct PatchFrame
		{
			// Token: 0x040079F1 RID: 31217
			public int minimum;

			// Token: 0x040079F2 RID: 31218
			public int maximum;

			// Token: 0x040079F3 RID: 31219
			public float tangentCorrection;

			// Token: 0x040079F4 RID: 31220
			public Vector3 referenceRight;
		}
	}
}
