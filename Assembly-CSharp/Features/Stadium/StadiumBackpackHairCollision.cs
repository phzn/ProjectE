using System;
using System.Collections.Generic;
using MagicaCloth2;
using UnityEngine;

namespace Features.Stadium
{
	// Token: 0x02000C3B RID: 3131
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(20002)]
	public sealed class StadiumBackpackHairCollision : MonoBehaviour
	{
		// Token: 0x06005A4C RID: 23116 RVA: 0x002356F9 File Offset: 0x002338F9
		private void OnEnable()
		{
			this._startedAt = Time.time;
			this._nextRefresh = 0f;
			if (this._frame != null)
			{
				this._frame.gameObject.SetActive(true);
			}
		}

		// Token: 0x06005A4D RID: 23117 RVA: 0x00235730 File Offset: 0x00233930
		private bool Initialize()
		{
			if (this._frame != null)
			{
				return this._bone != null;
			}
			if (this.shell == null || this.shell.sharedMesh == null)
			{
				return false;
			}
			Animator componentInParent = base.GetComponentInParent<Animator>();
			if (componentInParent == null)
			{
				return false;
			}
			Mesh sharedMesh = this.shell.sharedMesh;
			int boneIndex = sharedMesh.boneWeights[0].boneIndex0;
			if (boneIndex >= this.shell.bones.Length || this.shell.bones[boneIndex] == null || !this.shell.bones[boneIndex].IsChildOf(componentInParent.transform))
			{
				return false;
			}
			this._owner = componentInParent.transform;
			this._bone = this.shell.bones[boneIndex];
			this._bindPose = sharedMesh.bindposes[boneIndex];
			this._bounds = sharedMesh.bounds;
			this._frame = new GameObject("Backpack hair clearance").transform;
			this._frame.SetParent(base.transform, false);
			this._morphMoves = new Vector3[sharedMesh.blendShapeCount];
			Vector3[] array = new Vector3[sharedMesh.vertexCount];
			for (int i = 0; i < this._morphMoves.Length; i++)
			{
				sharedMesh.GetBlendShapeFrameVertices(i, 0, array, null, null);
				this._morphMoves[i] = array[0];
			}
			for (int j = 0; j < 3; j++)
			{
				GameObject gameObject = new GameObject("Hair capsule " + j.ToString());
				gameObject.transform.SetParent(this._frame, false);
				MagicaCapsuleCollider magicaCapsuleCollider = gameObject.AddComponent<MagicaCapsuleCollider>();
				magicaCapsuleCollider.direction = MagicaCapsuleCollider.Direction.Z;
				magicaCapsuleCollider.alignedOnCenter = true;
				this._colliders.Add(magicaCapsuleCollider);
			}
			this._startedAt = Time.time;
			return true;
		}

		// Token: 0x06005A4E RID: 23118 RVA: 0x0023590C File Offset: 0x00233B0C
		private void LateUpdate()
		{
			if (!this.Initialize())
			{
				return;
			}
			Matrix4x4 matrix4x = base.transform.worldToLocalMatrix * this._bone.localToWorldMatrix * this._bindPose;
			this._frame.localPosition = matrix4x.GetColumn(3);
			this._frame.localRotation = matrix4x.rotation;
			this._frame.localScale = matrix4x.lossyScale;
			Vector3 vector = Vector3.zero;
			for (int i = 0; i < this._morphMoves.Length; i++)
			{
				vector += this._morphMoves[i] * (this.shell.GetBlendShapeWeight(i) / 100f);
			}
			float num = this.clearanceMeters / Mathf.Max(0.001f, this._frame.lossyScale.x);
			float num2 = this.torsoBiasMeters / Mathf.Max(0.001f, this._frame.lossyScale.x);
			float num3 = this._bounds.extents.y + num2 + num;
			float num4 = Mathf.Max(0f, this._bounds.extents.x + num - num3);
			float num5 = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((Time.time - this._startedAt) / this.settleDuration));
			for (int j = 0; j < this._colliders.Count; j++)
			{
				Vector3 vector2 = this._bounds.center + vector;
				vector2.x += (float)(j - 1) * num4;
				vector2.y = Mathf.Lerp(vector.y, vector2.y - num2, num5);
				Vector3 vector3 = new Vector3(num3, num3, this._bounds.size.z + num * 2f);
				bool flag = this._colliders[j].center != vector2;
				this._colliders[j].center = vector2;
				bool flag2 = this._colliders[j].GetSize() != vector3;
				if (flag2)
				{
					this._colliders[j].SetSize(vector3);
				}
				if (flag || flag2)
				{
					this._colliders[j].UpdateParameters();
				}
			}
			if (Time.unscaledTime >= this._nextRefresh)
			{
				this._nextRefresh = Time.unscaledTime + 0.5f;
				this.RefreshHair();
			}
		}

		// Token: 0x06005A4F RID: 23119 RVA: 0x00235B94 File Offset: 0x00233D94
		private void RefreshHair()
		{
			if (this._owner == null)
			{
				return;
			}
			this._hair.RemoveWhere((MagicaCloth c) => c == null);
			this._owner.GetComponentsInChildren<MagicaCloth>(true, this._hairCandidates);
			foreach (MagicaCloth magicaCloth in this._hairCandidates)
			{
				if (StadiumBackpackHairCollision.IsHair(magicaCloth))
				{
					List<ColliderComponent> colliderList = magicaCloth.SerializeData.colliderCollisionConstraint.colliderList;
					bool flag = false;
					foreach (MagicaCapsuleCollider magicaCapsuleCollider in this._colliders)
					{
						if (!colliderList.Contains(magicaCapsuleCollider))
						{
							colliderList.Add(magicaCapsuleCollider);
							flag = true;
						}
					}
					this._hair.Add(magicaCloth);
					if (flag)
					{
						magicaCloth.SetParameterChange();
					}
				}
			}
		}

		// Token: 0x06005A50 RID: 23120 RVA: 0x00235CB4 File Offset: 0x00233EB4
		private static bool IsHair(MagicaCloth cloth)
		{
			if (cloth.SerializeData.clothType != ClothProcess.ClothType.BoneCloth)
			{
				return false;
			}
			foreach (Transform transform in cloth.SerializeData.rootBones)
			{
				if (transform != null && transform.name.StartsWith("Hair", StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005A51 RID: 23121 RVA: 0x00235D38 File Offset: 0x00233F38
		private void OnDisable()
		{
			foreach (MagicaCloth magicaCloth in this._hair)
			{
				if (!(magicaCloth == null))
				{
					List<ColliderComponent> colliderList = magicaCloth.SerializeData.colliderCollisionConstraint.colliderList;
					foreach (MagicaCapsuleCollider magicaCapsuleCollider in this._colliders)
					{
						colliderList.Remove(magicaCapsuleCollider);
					}
					magicaCloth.SetParameterChange();
				}
			}
			this._hair.Clear();
			if (this._frame != null)
			{
				this._frame.gameObject.SetActive(false);
			}
		}

		// Token: 0x04005B3A RID: 23354
		public SkinnedMeshRenderer shell;

		// Token: 0x04005B3B RID: 23355
		[Min(0f)]
		public float clearanceMeters = 0.006f;

		// Token: 0x04005B3C RID: 23356
		[Tooltip("Bias the collision centre into the torso so hair moves behind the bag instead of being trapped between bag and back.")]
		[Min(0f)]
		public float torsoBiasMeters = 0.012f;

		// Token: 0x04005B3D RID: 23357
		[Min(0.05f)]
		public float settleDuration = 0.35f;

		// Token: 0x04005B3E RID: 23358
		private Transform _frame;

		// Token: 0x04005B3F RID: 23359
		private Transform _owner;

		// Token: 0x04005B40 RID: 23360
		private Transform _bone;

		// Token: 0x04005B41 RID: 23361
		private Matrix4x4 _bindPose;

		// Token: 0x04005B42 RID: 23362
		private Bounds _bounds;

		// Token: 0x04005B43 RID: 23363
		private readonly List<MagicaCapsuleCollider> _colliders = new List<MagicaCapsuleCollider>();

		// Token: 0x04005B44 RID: 23364
		private readonly HashSet<MagicaCloth> _hair = new HashSet<MagicaCloth>();

		// Token: 0x04005B45 RID: 23365
		private readonly List<MagicaCloth> _hairCandidates = new List<MagicaCloth>();

		// Token: 0x04005B46 RID: 23366
		private Vector3[] _morphMoves;

		// Token: 0x04005B47 RID: 23367
		private float _startedAt;

		// Token: 0x04005B48 RID: 23368
		private float _nextRefresh;
	}
}
