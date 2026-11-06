using System;
using Cinemachine;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000022 RID: 34
	[RequireComponent(typeof(Collider2D))]
	public class MMCinemachineZone2D : MMCinemachineZone
	{
		// Token: 0x060000C1 RID: 193 RVA: 0x00004383 File Offset: 0x00002583
		protected override void InitializeCollider()
		{
			this._collider2D = base.GetComponent<Collider2D>();
			this._boxCollider2D = base.GetComponent<BoxCollider2D>();
			this._circleCollider2D = base.GetComponent<CircleCollider2D>();
			this._polygonCollider2D = base.GetComponent<PolygonCollider2D>();
			this._collider2D.isTrigger = true;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x000043C4 File Offset: 0x000025C4
		protected override void SetupConfiner()
		{
			this._confinerRigidbody2D = this._confinerGameObject.AddComponent<Rigidbody2D>();
			this._confinerRigidbody2D.bodyType = RigidbodyType2D.Static;
			this._confinerRigidbody2D.simulated = false;
			this._confinerRigidbody2D.useAutoMass = true;
			this._confinerRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
			this.CopyCollider();
			this._confinerGameObject.transform.localPosition = Vector3.zero;
			this._confinerRigidbody2D.bodyType = RigidbodyType2D.Static;
			this._confinerRigidbody2D.useAutoMass = false;
			this._confinerCompositeCollider2D = this._confinerGameObject.AddComponent<CompositeCollider2D>();
			this._confinerCompositeCollider2D.geometryType = CompositeCollider2D.GeometryType.Polygons;
			this._cinemachineConfiner = this.VirtualCamera.gameObject.MMGetComponentAroundOrAdd<CinemachineConfiner>();
			this._cinemachineConfiner.m_ConfineMode = CinemachineConfiner.Mode.Confine2D;
			this._cinemachineConfiner.m_ConfineScreenEdges = true;
			this._cinemachineConfiner.m_BoundingShape2D = this._confinerCompositeCollider2D;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000044A4 File Offset: 0x000026A4
		protected virtual void CopyCollider()
		{
			if (this._boxCollider2D != null)
			{
				BoxCollider2D boxCollider2D = this._confinerGameObject.AddComponent<BoxCollider2D>();
				boxCollider2D.size = this._boxCollider2D.size;
				boxCollider2D.offset = this._boxCollider2D.offset;
				boxCollider2D.usedByComposite = true;
				boxCollider2D.isTrigger = true;
			}
			if (this._circleCollider2D != null)
			{
				CircleCollider2D circleCollider2D = this._confinerGameObject.AddComponent<CircleCollider2D>();
				circleCollider2D.isTrigger = true;
				circleCollider2D.usedByComposite = true;
				circleCollider2D.offset = this._circleCollider2D.offset;
				circleCollider2D.radius = this._circleCollider2D.radius;
			}
			if (this._polygonCollider2D != null)
			{
				PolygonCollider2D polygonCollider2D = this._confinerGameObject.AddComponent<PolygonCollider2D>();
				polygonCollider2D.isTrigger = true;
				polygonCollider2D.usedByComposite = true;
				polygonCollider2D.offset = this._polygonCollider2D.offset;
				polygonCollider2D.points = this._polygonCollider2D.points;
			}
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00004589 File Offset: 0x00002789
		protected virtual void OnTriggerEnter2D(Collider2D collider)
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

		// Token: 0x060000C5 RID: 197 RVA: 0x000045B3 File Offset: 0x000027B3
		protected virtual void OnTriggerExit2D(Collider2D collider)
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

		// Token: 0x040000DB RID: 219
		protected Collider2D _collider2D;

		// Token: 0x040000DC RID: 220
		protected Collider2D _confinerCollider2D;

		// Token: 0x040000DD RID: 221
		protected Rigidbody2D _confinerRigidbody2D;

		// Token: 0x040000DE RID: 222
		protected CompositeCollider2D _confinerCompositeCollider2D;

		// Token: 0x040000DF RID: 223
		protected BoxCollider2D _boxCollider2D;

		// Token: 0x040000E0 RID: 224
		protected CircleCollider2D _circleCollider2D;

		// Token: 0x040000E1 RID: 225
		protected PolygonCollider2D _polygonCollider2D;

		// Token: 0x040000E2 RID: 226
		protected CinemachineConfiner _cinemachineConfiner;
	}
}
