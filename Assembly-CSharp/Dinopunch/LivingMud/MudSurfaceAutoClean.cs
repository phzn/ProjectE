using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dinopunch.LivingMud
{
	// Token: 0x02000748 RID: 1864
	[RequireComponent(typeof(DynamicMudSurface))]
	public class MudSurfaceAutoClean : MonoBehaviour
	{
		// Token: 0x06003047 RID: 12359 RVA: 0x00111917 File Offset: 0x0010FB17
		private void Awake()
		{
			this.surface = base.GetComponent<DynamicMudSurface>();
		}

		// Token: 0x06003048 RID: 12360 RVA: 0x00111925 File Offset: 0x0010FB25
		private void Start()
		{
			this.lastCleanTime = Time.time;
		}

		// Token: 0x06003049 RID: 12361 RVA: 0x00111934 File Offset: 0x0010FB34
		private void Update()
		{
			if (this.cleanInterval <= 0f)
			{
				return;
			}
			if (Time.time - this.lastCleanTime < this.cleanInterval)
			{
				return;
			}
			if (this.onlyClearWhenEmpty && this.activeOccupants.Count > 0)
			{
				return;
			}
			this.lastCleanTime = Time.time;
			this.surface.ClearStampTexture();
		}

		// Token: 0x0600304A RID: 12362 RVA: 0x00111991 File Offset: 0x0010FB91
		private void OnTriggerEnter(Collider other)
		{
			if (other.GetComponentInParent<MudFootTrigger>() != null)
			{
				this.activeOccupants.Add(other);
			}
		}

		// Token: 0x0600304B RID: 12363 RVA: 0x001119AE File Offset: 0x0010FBAE
		private void OnTriggerExit(Collider other)
		{
			this.activeOccupants.Remove(other);
		}

		// Token: 0x040030DC RID: 12508
		[Tooltip("Seconds between automatic stamp clears. 0 = disabled.")]
		[SerializeField]
		private float cleanInterval = 30f;

		// Token: 0x040030DD RID: 12509
		[Tooltip("If true, only clears when no foot triggers are currently in the mud")]
		[SerializeField]
		private bool onlyClearWhenEmpty = true;

		// Token: 0x040030DE RID: 12510
		private DynamicMudSurface surface;

		// Token: 0x040030DF RID: 12511
		private float lastCleanTime;

		// Token: 0x040030E0 RID: 12512
		private readonly HashSet<Collider> activeOccupants = new HashSet<Collider>();
	}
}
