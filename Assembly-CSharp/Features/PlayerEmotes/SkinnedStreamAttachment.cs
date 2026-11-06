using System;
using System.Collections.Generic;
using MagicaCloth2;
using UnityEngine;

namespace Features.PlayerEmotes
{
	// Token: 0x02000E2C RID: 3628
	[DefaultExecutionOrder(1000)]
	public sealed class SkinnedStreamAttachment : MonoBehaviour
	{
		// Token: 0x06006C49 RID: 27721 RVA: 0x002B5004 File Offset: 0x002B3204
		public bool Bind(SkinnedMeshRenderer renderer, string endBoneName)
		{
			if (renderer == null || renderer.sharedMesh == null || !renderer.sharedMesh.isReadable)
			{
				return false;
			}
			int num = Array.FindIndex<Transform>(renderer.bones, (Transform bone) => bone != null && bone.name == endBoneName);
			Matrix4x4[] bindposes = renderer.sharedMesh.bindposes;
			if (num < 0 || num >= bindposes.Length)
			{
				return false;
			}
			Vector3[] vertices = renderer.sharedMesh.vertices;
			Matrix4x4 matrix4x = bindposes[num];
			float num2 = float.NegativeInfinity;
			foreach (Vector3 vector in vertices)
			{
				num2 = Mathf.Max(num2, matrix4x.MultiplyPoint3x4(vector).y);
			}
			this._endVertices.Clear();
			this._directionVertices.Clear();
			for (int j = 0; j < vertices.Length; j++)
			{
				float num3 = num2 - matrix4x.MultiplyPoint3x4(vertices[j]).y;
				if (num3 <= 0.0003f)
				{
					this._endVertices.Add(j);
				}
				else if (num3 >= 0.006f && num3 <= 0.008f)
				{
					this._directionVertices.Add(j);
				}
			}
			if (this._endVertices.Count == 0 || this._directionVertices.Count == 0)
			{
				return false;
			}
			ParticleSystem componentInChildren = base.GetComponentInChildren<ParticleSystem>(true);
			if (componentInChildren == null)
			{
				return false;
			}
			ParticleSystem.ShapeModule shape = componentInChildren.shape;
			if (shape.enabled)
			{
				shape.position = Vector3.zero;
			}
			this._emitterLocalPosition = base.transform.InverseTransformPoint(componentInChildren.transform.position);
			this._emitterLocalRotation = Quaternion.Inverse(base.transform.rotation) * componentInChildren.transform.rotation * (shape.enabled ? Quaternion.Euler(shape.rotation) : Quaternion.identity);
			this._renderer = renderer;
			this._sourceMesh = renderer.sharedMesh;
			if (this._bakedMesh == null)
			{
				this._bakedMesh = new Mesh
				{
					name = "Stream attachment snapshot"
				};
				this._bakedMesh.MarkDynamic();
			}
			this._vertices.Capacity = Mathf.Max(this._vertices.Capacity, vertices.Length);
			ParticleSystem[] componentsInChildren = base.GetComponentsInChildren<ParticleSystem>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].main.simulationSpace = ParticleSystemSimulationSpace.World;
			}
			this.UpdatePose();
			return true;
		}

		// Token: 0x06006C4A RID: 27722 RVA: 0x002B528B File Offset: 0x002B348B
		private void OnEnable()
		{
			MagicaManager.OnPostSimulation = (Action)Delegate.Combine(MagicaManager.OnPostSimulation, new Action(this.AfterSimulation));
			this.UpdatePose();
		}

		// Token: 0x06006C4B RID: 27723 RVA: 0x002B52B3 File Offset: 0x002B34B3
		private void OnDisable()
		{
			MagicaManager.OnPostSimulation = (Action)Delegate.Remove(MagicaManager.OnPostSimulation, new Action(this.AfterSimulation));
		}

		// Token: 0x06006C4C RID: 27724 RVA: 0x002B52D5 File Offset: 0x002B34D5
		private void AfterSimulation()
		{
			this.UpdatePose();
			this._postSimulationFrame = Time.frameCount;
		}

		// Token: 0x06006C4D RID: 27725 RVA: 0x002B52E8 File Offset: 0x002B34E8
		private void LateUpdate()
		{
			if (this._postSimulationFrame != Time.frameCount)
			{
				this.UpdatePose();
			}
		}

		// Token: 0x06006C4E RID: 27726 RVA: 0x002B5300 File Offset: 0x002B3500
		public void UpdatePose()
		{
			if (this._renderer == null || this._bakedMesh == null || this._renderer.sharedMesh != this._sourceMesh)
			{
				return;
			}
			this._renderer.BakeMesh(this._bakedMesh, true);
			this._bakedMesh.GetVertices(this._vertices);
			Vector3 vector = this.WorldAverage(this._endVertices);
			Vector3 vector2 = vector - this.WorldAverage(this._directionVertices);
			if (vector2.sqrMagnitude < 1E-08f)
			{
				return;
			}
			Vector3 vector3 = this._renderer.transform.up;
			if (Mathf.Abs(Vector3.Dot(vector2.normalized, vector3)) > 0.99f)
			{
				vector3 = this._renderer.transform.right;
			}
			base.transform.rotation = Quaternion.LookRotation(vector2, vector3) * Quaternion.Inverse(this._emitterLocalRotation);
			base.transform.position = vector - base.transform.TransformVector(this._emitterLocalPosition);
		}

		// Token: 0x06006C4F RID: 27727 RVA: 0x002B5414 File Offset: 0x002B3614
		private Vector3 WorldAverage(List<int> indices)
		{
			Vector3 vector = Vector3.zero;
			foreach (int num in indices)
			{
				vector += this._vertices[num];
			}
			return this._renderer.transform.TransformPoint(vector / (float)indices.Count);
		}

		// Token: 0x06006C50 RID: 27728 RVA: 0x002B5494 File Offset: 0x002B3694
		private void OnDestroy()
		{
			MagicaManager.OnPostSimulation = (Action)Delegate.Remove(MagicaManager.OnPostSimulation, new Action(this.AfterSimulation));
			if (this._bakedMesh == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(this._bakedMesh);
				return;
			}
			Object.DestroyImmediate(this._bakedMesh);
		}

		// Token: 0x04006EBE RID: 28350
		private SkinnedMeshRenderer _renderer;

		// Token: 0x04006EBF RID: 28351
		private Mesh _sourceMesh;

		// Token: 0x04006EC0 RID: 28352
		private Mesh _bakedMesh;

		// Token: 0x04006EC1 RID: 28353
		private readonly List<Vector3> _vertices = new List<Vector3>();

		// Token: 0x04006EC2 RID: 28354
		private readonly List<int> _endVertices = new List<int>();

		// Token: 0x04006EC3 RID: 28355
		private readonly List<int> _directionVertices = new List<int>();

		// Token: 0x04006EC4 RID: 28356
		private Vector3 _emitterLocalPosition;

		// Token: 0x04006EC5 RID: 28357
		private Quaternion _emitterLocalRotation;

		// Token: 0x04006EC6 RID: 28358
		private int _postSimulationFrame = -1;
	}
}
