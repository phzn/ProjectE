using System;
using System.Collections.Generic;
using NGS.AdvancedCullingSystem.Dynamic;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Utils
{
	// Token: 0x0200000B RID: 11
	public class DC_ActivateNearObjects : MonoBehaviour
	{
		// Token: 0x06000031 RID: 49 RVA: 0x00002C72 File Offset: 0x00000E72
		private void Start()
		{
			this._hitablesDic = DC_Controller.GetHitables();
			this._layer = LayerMask.GetMask(new string[] { DC_Controller.GetCullingLayerName() });
			this._hits = new Collider[this._maxObjectsCount];
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002CAC File Offset: 0x00000EAC
		private void LateUpdate()
		{
			int num = Physics.OverlapSphereNonAlloc(base.transform.position, this._radius, this._hits, this._layer);
			for (int i = 0; i < num; i++)
			{
				Collider collider = this._hits[i];
				IHitable hitable;
				if (this._hitablesDic.TryGetValue(collider, out hitable))
				{
					hitable.OnHit();
				}
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002D07 File Offset: 0x00000F07
		private void OnDrawGizmos()
		{
			if (!this._drawGizmos)
			{
				return;
			}
			Gizmos.color = Color.white;
			Gizmos.DrawWireSphere(base.transform.position, this._radius);
		}

		// Token: 0x0400000E RID: 14
		[SerializeField]
		private bool _drawGizmos;

		// Token: 0x0400000F RID: 15
		[Space]
		[Min(0.1f)]
		[SerializeField]
		private float _radius = 20f;

		// Token: 0x04000010 RID: 16
		[Min(1f)]
		[SerializeField]
		private int _maxObjectsCount = 100;

		// Token: 0x04000011 RID: 17
		private IReadOnlyDictionary<Collider, IHitable> _hitablesDic;

		// Token: 0x04000012 RID: 18
		private int _layer;

		// Token: 0x04000013 RID: 19
		private Collider[] _hits;
	}
}
