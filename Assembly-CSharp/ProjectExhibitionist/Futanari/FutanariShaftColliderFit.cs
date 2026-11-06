using System;
using System.Collections.Generic;
using MagicaCloth2;
using UnityEngine;

namespace ProjectExhibitionist.Futanari
{
	// Token: 0x020009E1 RID: 2529
	public sealed class FutanariShaftColliderFit : IDisposable
	{
		// Token: 0x0600478C RID: 18316 RVA: 0x001B2148 File Offset: 0x001B0348
		private FutanariShaftColliderFit(MagicaCapsuleCollider capsule, SkinnedMeshRenderer renderer, int boneIndex)
		{
			this._capsule = capsule;
			this._renderer = renderer;
			this._originalRotation = capsule.transform.localRotation;
			this._originalCenter = capsule.center;
			this._originalSize = capsule.GetSize();
			this._originalDirection = capsule.direction;
			this._originalAlignedOnCenter = capsule.alignedOnCenter;
			this._originalReverseDirection = capsule.reverseDirection;
			this._originalRadiusSeparation = capsule.radiusSeparation;
			this._frame = ((renderer.bones[0].parent != null) ? renderer.bones[0].parent : capsule.transform.parent);
			this._source = renderer.sharedMesh;
			Vector3[] vertices = this._source.vertices;
			Matrix4x4 matrix4x = this._source.bindposes[boneIndex];
			float num = 0f;
			for (int i = 0; i < vertices.Length; i++)
			{
				num = Mathf.Max(num, matrix4x.MultiplyPoint3x4(vertices[i]).y);
			}
			for (int j = 0; j < vertices.Length; j++)
			{
				float y = matrix4x.MultiplyPoint3x4(vertices[j]).y;
				if (y >= 0f)
				{
					this._shaft.Add(j);
					if (y <= num * 0.15f)
					{
						this._basePatch.Add(j);
					}
					if (y >= num * 0.85f)
					{
						this._tipPatch.Add(j);
					}
				}
			}
			this._baked = new Mesh
			{
				name = "Futanari shaft collider snapshot"
			};
			this._baked.MarkDynamic();
			this._vertices.Capacity = vertices.Length;
		}

		// Token: 0x0600478D RID: 18317 RVA: 0x001B2318 File Offset: 0x001B0518
		public static FutanariShaftColliderFit TryCreate(MagicaCapsuleCollider capsule, SkinnedMeshRenderer renderer)
		{
			if (capsule == null || capsule.transform.parent == null || renderer == null || renderer.sharedMesh == null || !renderer.sharedMesh.isReadable)
			{
				return null;
			}
			int num = Array.IndexOf<Transform>(renderer.bones, capsule.transform.parent);
			if (num < 0 || num >= renderer.sharedMesh.bindposes.Length)
			{
				return null;
			}
			FutanariShaftColliderFit futanariShaftColliderFit = new FutanariShaftColliderFit(capsule, renderer, num);
			if (futanariShaftColliderFit._basePatch.Count > 0 && futanariShaftColliderFit._tipPatch.Count > 0)
			{
				return futanariShaftColliderFit;
			}
			futanariShaftColliderFit.Dispose();
			return null;
		}

		// Token: 0x0600478E RID: 18318 RVA: 0x001B23C4 File Offset: 0x001B05C4
		public void CapturePose()
		{
			if (this._disposed || this._capsule == null || this._renderer == null || this._frame == null || this._renderer.sharedMesh != this._source)
			{
				return;
			}
			this._renderer.BakeMesh(this._baked, true);
			this._baked.GetVertices(this._vertices);
			Matrix4x4 matrix4x = this._frame.worldToLocalMatrix * this._renderer.transform.localToWorldMatrix;
			Vector3 vector = this.Average(this._basePatch, matrix4x);
			Vector3 vector2 = this.Average(this._tipPatch, matrix4x) - vector;
			if (vector2.sqrMagnitude < 1E-10f)
			{
				return;
			}
			vector2.Normalize();
			float num = 0f;
			foreach (int num2 in this._shaft)
			{
				Vector3 vector3 = matrix4x.MultiplyPoint3x4(this._vertices[num2]) - vector;
				float num3 = Vector3.Dot(vector3, vector2);
				num = Mathf.Max(num, (vector3 - vector2 * num3).sqrMagnitude);
			}
			float num4 = Mathf.Sqrt(num);
			float num5 = float.PositiveInfinity;
			float num6 = float.NegativeInfinity;
			foreach (int num7 in this._shaft)
			{
				Vector3 vector4 = matrix4x.MultiplyPoint3x4(this._vertices[num7]) - vector;
				float num8 = Vector3.Dot(vector4, vector2);
				float sqrMagnitude = (vector4 - vector2 * num8).sqrMagnitude;
				float num9 = Mathf.Sqrt(Mathf.Max(0f, num - sqrMagnitude));
				num5 = Mathf.Min(num5, num8 + num9);
				num6 = Mathf.Max(num6, num8 - num9);
			}
			this._center = vector + vector2 * ((num5 + num6) * 0.5f);
			this._rotation = Quaternion.FromToRotation(Vector3.up, vector2);
			this._size = new Vector3(num4, num4, Mathf.Max(0f, num6 - num5) + num4 * 2f);
			this._hasPose = true;
			this.ApplyPose();
		}

		// Token: 0x0600478F RID: 18319 RVA: 0x001B2648 File Offset: 0x001B0848
		public void ApplyPose()
		{
			if (this._disposed || !this._hasPose || this._capsule == null || this._frame == null)
			{
				return;
			}
			this._capsule.transform.rotation = this._frame.rotation * this._rotation;
			Vector3 vector = this._capsule.transform.InverseTransformPoint(this._frame.TransformPoint(this._center));
			float num = Mathf.Abs(this._frame.lossyScale.x) / Mathf.Max(0.0001f, Mathf.Abs(this._capsule.GetScale()));
			Vector3 vector2 = this._size * num;
			bool flag = this._capsule.center != vector || this._capsule.GetSize() != vector2 || this._capsule.direction != MagicaCapsuleCollider.Direction.Y || !this._capsule.alignedOnCenter || this._capsule.reverseDirection;
			this._capsule.direction = MagicaCapsuleCollider.Direction.Y;
			this._capsule.alignedOnCenter = true;
			this._capsule.reverseDirection = false;
			this._capsule.center = vector;
			this._capsule.SetSize(vector2.x, vector2.y, vector2.z);
			if (flag)
			{
				this._capsule.UpdateParameters();
			}
		}

		// Token: 0x06004790 RID: 18320 RVA: 0x001B27B4 File Offset: 0x001B09B4
		private Vector3 Average(List<int> indices, Matrix4x4 toFrame)
		{
			Vector3 vector = Vector3.zero;
			foreach (int num in indices)
			{
				vector += this._vertices[num];
			}
			return toFrame.MultiplyPoint3x4(vector / (float)indices.Count);
		}

		// Token: 0x06004791 RID: 18321 RVA: 0x001B2828 File Offset: 0x001B0A28
		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			this._disposed = true;
			if (this._hasPose && this._capsule != null)
			{
				this._capsule.transform.localRotation = this._originalRotation;
				this._capsule.center = this._originalCenter;
				this._capsule.SetSize(this._originalSize);
				this._capsule.direction = this._originalDirection;
				this._capsule.alignedOnCenter = this._originalAlignedOnCenter;
				this._capsule.reverseDirection = this._originalReverseDirection;
				this._capsule.radiusSeparation = this._originalRadiusSeparation;
				if (MagicaManager.IsPlaying() && MagicaManager.Collider != null)
				{
					this._capsule.UpdateParameters();
				}
			}
			if (this._baked == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(this._baked);
				return;
			}
			Object.DestroyImmediate(this._baked);
		}

		// Token: 0x0400472C RID: 18220
		private readonly MagicaCapsuleCollider _capsule;

		// Token: 0x0400472D RID: 18221
		private readonly SkinnedMeshRenderer _renderer;

		// Token: 0x0400472E RID: 18222
		private readonly Transform _frame;

		// Token: 0x0400472F RID: 18223
		private readonly Mesh _source;

		// Token: 0x04004730 RID: 18224
		private readonly Mesh _baked;

		// Token: 0x04004731 RID: 18225
		private readonly Quaternion _originalRotation;

		// Token: 0x04004732 RID: 18226
		private readonly Vector3 _originalCenter;

		// Token: 0x04004733 RID: 18227
		private readonly Vector3 _originalSize;

		// Token: 0x04004734 RID: 18228
		private readonly MagicaCapsuleCollider.Direction _originalDirection;

		// Token: 0x04004735 RID: 18229
		private readonly bool _originalAlignedOnCenter;

		// Token: 0x04004736 RID: 18230
		private readonly bool _originalReverseDirection;

		// Token: 0x04004737 RID: 18231
		private readonly bool _originalRadiusSeparation;

		// Token: 0x04004738 RID: 18232
		private readonly List<Vector3> _vertices = new List<Vector3>();

		// Token: 0x04004739 RID: 18233
		private readonly List<int> _shaft = new List<int>();

		// Token: 0x0400473A RID: 18234
		private readonly List<int> _basePatch = new List<int>();

		// Token: 0x0400473B RID: 18235
		private readonly List<int> _tipPatch = new List<int>();

		// Token: 0x0400473C RID: 18236
		private Vector3 _center;

		// Token: 0x0400473D RID: 18237
		private Quaternion _rotation;

		// Token: 0x0400473E RID: 18238
		private Vector3 _size;

		// Token: 0x0400473F RID: 18239
		private bool _hasPose;

		// Token: 0x04004740 RID: 18240
		private bool _disposed;
	}
}
