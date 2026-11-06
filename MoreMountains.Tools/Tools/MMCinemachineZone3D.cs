using System;
using Cinemachine;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000023 RID: 35
	[RequireComponent(typeof(Collider))]
	public class MMCinemachineZone3D : MMCinemachineZone
	{
		// Token: 0x060000C7 RID: 199 RVA: 0x000045E5 File Offset: 0x000027E5
		protected override void InitializeCollider()
		{
			this._collider = base.GetComponent<Collider>();
			this._boxCollider = base.GetComponent<BoxCollider>();
			this._sphereCollider = base.GetComponent<SphereCollider>();
			this._collider.isTrigger = true;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00004618 File Offset: 0x00002818
		protected override void SetupConfiner()
		{
			this._confinerRigidbody = this._confinerGameObject.AddComponent<Rigidbody>();
			this._confinerRigidbody.useGravity = false;
			this._confinerRigidbody.gameObject.isStatic = true;
			this._confinerRigidbody.isKinematic = true;
			this.CopyCollider();
			this._confinerGameObject.transform.localPosition = Vector3.zero;
			this._cinemachineConfiner = this.VirtualCamera.gameObject.MMGetComponentAroundOrAdd<CinemachineConfiner>();
			this._cinemachineConfiner.m_ConfineMode = CinemachineConfiner.Mode.Confine3D;
			this._cinemachineConfiner.m_ConfineScreenEdges = true;
			if (this._boxCollider != null)
			{
				this._cinemachineConfiner.m_BoundingVolume = this._boxCollider;
			}
			if (this._sphereCollider != null)
			{
				this._cinemachineConfiner.m_BoundingVolume = this._sphereCollider;
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x000046E8 File Offset: 0x000028E8
		protected virtual void CopyCollider()
		{
			if (this._boxCollider != null)
			{
				BoxCollider boxCollider = this._confinerGameObject.AddComponent<BoxCollider>();
				boxCollider.size = this._boxCollider.size;
				boxCollider.center = this._boxCollider.center;
				boxCollider.isTrigger = true;
			}
			if (this._sphereCollider != null)
			{
				SphereCollider sphereCollider = this._confinerGameObject.AddComponent<SphereCollider>();
				sphereCollider.isTrigger = true;
				sphereCollider.center = this._sphereCollider.center;
				sphereCollider.radius = this._sphereCollider.radius;
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004777 File Offset: 0x00002977
		protected virtual void OnTriggerEnter(Collider collider)
		{
			if (!this.TestCollidingGameObject(collider.gameObject))
			{
				return;
			}
			if (this.TriggerMask.MMContains(collider.gameObject))
			{
				this.EnterZone();
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x000047A1 File Offset: 0x000029A1
		protected virtual void OnTriggerExit(Collider collider)
		{
			if (!this.TestCollidingGameObject(collider.gameObject))
			{
				return;
			}
			if (this.TriggerMask.MMContains(collider.gameObject))
			{
				this.ExitZone();
			}
		}

		// Token: 0x040000E3 RID: 227
		protected Collider _collider;

		// Token: 0x040000E4 RID: 228
		protected Collider _confinerCollider;

		// Token: 0x040000E5 RID: 229
		protected Rigidbody _confinerRigidbody;

		// Token: 0x040000E6 RID: 230
		protected BoxCollider _boxCollider;

		// Token: 0x040000E7 RID: 231
		protected SphereCollider _sphereCollider;

		// Token: 0x040000E8 RID: 232
		protected CinemachineConfiner _cinemachineConfiner;
	}
}
