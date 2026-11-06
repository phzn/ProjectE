using System;
using UnityEngine;

namespace Dinopunch.LivingMud
{
	// Token: 0x0200075E RID: 1886
	[RequireComponent(typeof(Collider))]
	public class SixSidedStamp : LivingMudEffectsPlayer, IPlaceStamp
	{
		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x060030B9 RID: 12473 RVA: 0x00112EEB File Offset: 0x001110EB
		public override MudType RequiredThickness { get; }

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x060030BA RID: 12474 RVA: 0x00112EF3 File Offset: 0x001110F3
		public DynamicMudSurface Surface
		{
			get
			{
				return this.surface;
			}
		}

		// Token: 0x060030BB RID: 12475 RVA: 0x00112EFC File Offset: 0x001110FC
		public void Start()
		{
			this.stamps = new OrientedStamp[6];
			this.stamps[0] = this.TopStamp;
			this.stamps[1] = this.BottomStamp;
			this.stamps[2] = this.LeftStamp;
			this.stamps[3] = this.RightStamp;
			this.stamps[4] = this.FrontStamp;
			this.stamps[5] = this.BackStamp;
		}

		// Token: 0x060030BC RID: 12476 RVA: 0x00112F6C File Offset: 0x0011116C
		public override void StartEffects(DynamicMudSurface surface, MudType thickness, bool overrideThickness = false)
		{
			OrientedStamp orientedStamp = null;
			this.surface = surface;
			Vector3 vector = -Vector3.up;
			Vector3[] array = new Vector3[]
			{
				base.transform.up,
				-base.transform.up,
				base.transform.right,
				-base.transform.right,
				base.transform.forward,
				-base.transform.forward
			};
			for (int i = 0; i < 6; i++)
			{
				if (Vector3.Dot(array[i], vector) > 0.5f)
				{
					orientedStamp = this.stamps[i];
					break;
				}
			}
			if (orientedStamp.Stamp)
			{
				this.PlaceStampWithOrientation(orientedStamp);
			}
		}

		// Token: 0x060030BD RID: 12477 RVA: 0x0011304E File Offset: 0x0011124E
		public void PlaceStampWithOrientation(OrientedStamp orientedStamp)
		{
			orientedStamp.Stamp.SetFlipX(orientedStamp.FlipX);
			orientedStamp.Stamp.SetFlipY(orientedStamp.FlipY);
			this.PlaceStamp(orientedStamp.Stamp);
		}

		// Token: 0x060030BE RID: 12478 RVA: 0x00113080 File Offset: 0x00111280
		public void PlaceStamp(MudStamp stamp = null)
		{
			if (stamp == null || this.surface == null)
			{
				Debug.LogWarning("Stamp " + base.gameObject.name + " doesn't have a reference to a stamp or surface");
				return;
			}
			stamp.SetSurface(this.surface);
			stamp.StampSurface();
		}

		// Token: 0x060030BF RID: 12479 RVA: 0x00004F31 File Offset: 0x00003131
		public override void StopEffects()
		{
		}

		// Token: 0x0400312A RID: 12586
		[SerializeField]
		private OrientedStamp TopStamp;

		// Token: 0x0400312B RID: 12587
		[SerializeField]
		private OrientedStamp BottomStamp;

		// Token: 0x0400312C RID: 12588
		[SerializeField]
		private OrientedStamp LeftStamp;

		// Token: 0x0400312D RID: 12589
		[SerializeField]
		private OrientedStamp RightStamp;

		// Token: 0x0400312E RID: 12590
		[SerializeField]
		private OrientedStamp FrontStamp;

		// Token: 0x0400312F RID: 12591
		[SerializeField]
		private OrientedStamp BackStamp;

		// Token: 0x04003131 RID: 12593
		private DynamicMudSurface surface;

		// Token: 0x04003132 RID: 12594
		private OrientedStamp[] stamps;
	}
}
