using System;
using MagicaCloth2;
using UnityEngine;

namespace Features.EggRotor
{
	// Token: 0x0200122A RID: 4650
	[DefaultExecutionOrder(22200)]
	[DisallowMultipleComponent]
	public sealed class EggRotorCumFollower : MonoBehaviour
	{
		// Token: 0x06009072 RID: 36978 RVA: 0x003B2408 File Offset: 0x003B0608
		public bool Bind(EggRotorController controller)
		{
			this._controller = controller;
			ParticleSystem componentInChildren = base.GetComponentInChildren<ParticleSystem>(true);
			if (controller == null || componentInChildren == null)
			{
				this._bound = false;
				return false;
			}
			ParticleSystem.ShapeModule shape = componentInChildren.shape;
			if (shape.enabled)
			{
				shape.position = Vector3.zero;
			}
			this._emitterLocalPosition = base.transform.InverseTransformPoint(componentInChildren.transform.position);
			this._emitterLocalRotation = Quaternion.Inverse(base.transform.rotation) * componentInChildren.transform.rotation * (shape.enabled ? Quaternion.Euler(shape.rotation) : Quaternion.identity);
			ParticleSystem[] componentsInChildren = base.GetComponentsInChildren<ParticleSystem>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].main.simulationSpace = ParticleSystemSimulationSpace.World;
			}
			this._bound = true;
			this.UpdatePose();
			return true;
		}

		// Token: 0x06009073 RID: 36979 RVA: 0x003B24F4 File Offset: 0x003B06F4
		private void OnEnable()
		{
			this.Hook(true);
		}

		// Token: 0x06009074 RID: 36980 RVA: 0x003B24FD File Offset: 0x003B06FD
		private void OnDisable()
		{
			this.Hook(false);
		}

		// Token: 0x06009075 RID: 36981 RVA: 0x003B24FD File Offset: 0x003B06FD
		private void OnDestroy()
		{
			this.Hook(false);
		}

		// Token: 0x06009076 RID: 36982 RVA: 0x003B2508 File Offset: 0x003B0708
		private void Hook(bool hook)
		{
			if (hook == this._hooked)
			{
				return;
			}
			if (this._postSimulation == null)
			{
				this._postSimulation = new Action(this.UpdatePose);
			}
			if (hook)
			{
				MagicaManager.OnPostSimulation = (Action)Delegate.Combine(MagicaManager.OnPostSimulation, this._postSimulation);
			}
			else
			{
				MagicaManager.OnPostSimulation = (Action)Delegate.Remove(MagicaManager.OnPostSimulation, this._postSimulation);
			}
			this._hooked = hook;
		}

		// Token: 0x06009077 RID: 36983 RVA: 0x003B2579 File Offset: 0x003B0779
		private void LateUpdate()
		{
			this.UpdatePose();
		}

		// Token: 0x06009078 RID: 36984 RVA: 0x003B2584 File Offset: 0x003B0784
		public void UpdatePose()
		{
			Vector3 vector;
			Vector3 vector2;
			if (!this._bound || this._controller == null || !this._controller.TryGetPenisTip(out vector, out vector2))
			{
				return;
			}
			SkinnedMeshRenderer penisRenderer = this._controller.PenisRenderer;
			Vector3 vector3 = ((penisRenderer != null) ? penisRenderer.transform.up : Vector3.up);
			if (Mathf.Abs(Vector3.Dot(vector2, vector3)) > 0.99f)
			{
				vector3 = ((penisRenderer != null) ? penisRenderer.transform.right : Vector3.right);
			}
			base.transform.rotation = Quaternion.LookRotation(vector2, vector3) * Quaternion.Inverse(this._emitterLocalRotation);
			base.transform.position = vector - base.transform.TransformVector(this._emitterLocalPosition);
		}

		// Token: 0x04009477 RID: 38007
		private EggRotorController _controller;

		// Token: 0x04009478 RID: 38008
		private Vector3 _emitterLocalPosition;

		// Token: 0x04009479 RID: 38009
		private Quaternion _emitterLocalRotation = Quaternion.identity;

		// Token: 0x0400947A RID: 38010
		private bool _bound;

		// Token: 0x0400947B RID: 38011
		private bool _hooked;

		// Token: 0x0400947C RID: 38012
		private Action _postSimulation;
	}
}
