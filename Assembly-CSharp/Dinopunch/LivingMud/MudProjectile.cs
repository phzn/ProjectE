using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dinopunch.LivingMud
{
	// Token: 0x02000757 RID: 1879
	[SelectionBase]
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(Collider))]
	public class MudProjectile : MonoBehaviour, ICheckInMud
	{
		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x06003095 RID: 12437 RVA: 0x00112668 File Offset: 0x00110868
		public bool InMud
		{
			get
			{
				return this.inMud;
			}
		}

		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x06003096 RID: 12438 RVA: 0x00112670 File Offset: 0x00110870
		// (set) Token: 0x06003097 RID: 12439 RVA: 0x00112678 File Offset: 0x00110878
		public DynamicMudSurface Surface { get; private set; }

		// Token: 0x06003098 RID: 12440 RVA: 0x00112681 File Offset: 0x00110881
		public void Start()
		{
			this.myRigidbody = base.GetComponent<Rigidbody>();
			this.mudEffects = base.GetComponents<LivingMudEffectsPlayer>().ToList<LivingMudEffectsPlayer>();
		}

		// Token: 0x06003099 RID: 12441 RVA: 0x001126A0 File Offset: 0x001108A0
		public void OnCollisionEnter(Collision collision)
		{
			DynamicMudSurface dynamicMudSurface;
			if (!collision.gameObject.TryGetComponent<DynamicMudSurface>(out dynamicMudSurface))
			{
				return;
			}
			this.Surface = dynamicMudSurface;
			this.impactPoint = collision.contacts[0].point;
			this.EnteredMud();
		}

		// Token: 0x0600309A RID: 12442 RVA: 0x001126E4 File Offset: 0x001108E4
		public void OnCollisionExit(Collision collision)
		{
			DynamicMudSurface dynamicMudSurface;
			if (!collision.gameObject.TryGetComponent<DynamicMudSurface>(out dynamicMudSurface))
			{
				return;
			}
			this.ExitMud();
		}

		// Token: 0x0600309B RID: 12443 RVA: 0x00112708 File Offset: 0x00110908
		private void EnteredMud()
		{
			this.inMud = true;
			float magnitude = this.myRigidbody.velocity.magnitude;
			Vector3 normalized = this.myRigidbody.velocity.normalized;
			this.MakeMaterialMuddy();
			foreach (LivingMudEffectsPlayer livingMudEffectsPlayer in this.mudEffects)
			{
				if (livingMudEffectsPlayer is IUseImpactData)
				{
					(livingMudEffectsPlayer as IUseImpactData).SetImpactData(this.mass, magnitude, normalized, this.impactPoint);
				}
				livingMudEffectsPlayer.StartEffects(this.Surface, this.Surface.Thickness, false);
			}
		}

		// Token: 0x0600309C RID: 12444 RVA: 0x001127C8 File Offset: 0x001109C8
		private void ExitMud()
		{
			this.inMud = false;
		}

		// Token: 0x0600309D RID: 12445 RVA: 0x001127D4 File Offset: 0x001109D4
		public void MakeMaterialMuddy()
		{
			MuddyMaterial[] array = this.muddyMaterialComponents;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].MakeMuddy();
			}
		}

		// Token: 0x0400310D RID: 12557
		[SerializeField]
		private float mass;

		// Token: 0x0400310E RID: 12558
		[SerializeField]
		private MuddyMaterial[] muddyMaterialComponents;

		// Token: 0x0400310F RID: 12559
		private bool inMud;

		// Token: 0x04003111 RID: 12561
		private Rigidbody myRigidbody;

		// Token: 0x04003112 RID: 12562
		private Vector3 impactPoint;

		// Token: 0x04003113 RID: 12563
		private List<LivingMudEffectsPlayer> mudEffects;

		// Token: 0x04003114 RID: 12564
		private float mudAmount;
	}
}
