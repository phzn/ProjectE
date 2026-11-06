using System;
using UnityEngine;
using UnityEngine.VFX;

namespace Dinopunch.LivingMud
{
	// Token: 0x02000754 RID: 1876
	public class MakeMudBlobsWhileInMud : MonoBehaviour, ISetColorMap
	{
		// Token: 0x06003089 RID: 12425 RVA: 0x00112356 File Offset: 0x00110556
		public void Start()
		{
			base.TryGetComponent<ICheckInMud>(out this.mudChecker);
			if (this.MudBlobsVFX != null)
			{
				this.MudBlobsVFX.Stop();
			}
		}

		// Token: 0x0600308A RID: 12426 RVA: 0x00112380 File Offset: 0x00110580
		public void Update()
		{
			if (this.mudChecker == null)
			{
				return;
			}
			if (!this.makingMudBlobs && this.mudChecker.InMud)
			{
				this.StartMudBlobs();
			}
			if (this.makingMudBlobs && !this.mudChecker.InMud)
			{
				this.StopMudBlobs();
			}
		}

		// Token: 0x0600308B RID: 12427 RVA: 0x001123CC File Offset: 0x001105CC
		public void StartMudBlobs()
		{
			if (this.mudChecker == null || this.mudChecker.Surface == null || this.MudBlobsVFX == null)
			{
				return;
			}
			this.MudBlobsVFX.Play();
			this.makingMudBlobs = true;
			ICheckInMud checkInMud = this.mudChecker;
			this.SetColorMap((checkInMud != null) ? checkInMud.Surface.GetColorMap() : null);
			ICheckInMud checkInMud2 = this.mudChecker;
			this.SetColor(((checkInMud2 != null) ? new Color?(checkInMud2.Surface.GetColor()) : null).Value);
		}

		// Token: 0x0600308C RID: 12428 RVA: 0x00112464 File Offset: 0x00110664
		public void StopMudBlobs()
		{
			if (this.MudBlobsVFX != null)
			{
				this.MudBlobsVFX.Stop();
			}
			this.makingMudBlobs = false;
		}

		// Token: 0x0600308D RID: 12429 RVA: 0x00112486 File Offset: 0x00110686
		public void SetColorMap(Texture2D tex)
		{
			if (this.MudBlobsVFX == null || tex == null)
			{
				return;
			}
			this.MudBlobsVFX.SetTexture("Color Map", tex);
		}

		// Token: 0x0600308E RID: 12430 RVA: 0x001124B1 File Offset: 0x001106B1
		public void SetColor(Color color)
		{
			if (this.MudBlobsVFX == null)
			{
				return;
			}
			this.MudBlobsVFX.SetVector4("Tint", new Vector4(color.r, color.g, color.b, color.a));
		}

		// Token: 0x04003102 RID: 12546
		[SerializeField]
		private VisualEffect MudBlobsVFX;

		// Token: 0x04003103 RID: 12547
		private ICheckInMud mudChecker;

		// Token: 0x04003104 RID: 12548
		private bool makingMudBlobs;
	}
}
