using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

namespace Dinopunch.LivingMud
{
	// Token: 0x02000760 RID: 1888
	public class SplatterMudBlobs : MonoBehaviour
	{
		// Token: 0x060030D1 RID: 12497 RVA: 0x001132F0 File Offset: 0x001114F0
		private void Start()
		{
			if (!this.MudBlobsVFX || !this.blobSpawnTarget1 || !this.blobSpawnTarget2)
			{
				Debug.LogWarning("You must set references for mud blob vfx and spawn targets to splatter mud blobs");
				return;
			}
			this.blobSpawnTarget1.TryGetComponent<StayOnMudSurface>(out this.spawn1MudSurfaceSnap);
			this.blobSpawnTarget2.TryGetComponent<StayOnMudSurface>(out this.spawn2MudSurfaceSnap);
		}

		// Token: 0x060030D2 RID: 12498 RVA: 0x00113353 File Offset: 0x00111553
		public void Splatter(int amount = 0)
		{
			if (amount == 0)
			{
				amount = this.defaultAmount;
			}
			this.Splatter(amount, base.transform.position, this.defaultDistance);
		}

		// Token: 0x060030D3 RID: 12499 RVA: 0x00113378 File Offset: 0x00111578
		public void Splatter(int amount, Vector3 origin)
		{
			this.Splatter(amount, origin, this.defaultDistance);
		}

		// Token: 0x060030D4 RID: 12500 RVA: 0x00113388 File Offset: 0x00111588
		public void Splatter(int amount, float distance)
		{
			this.Splatter(amount, base.transform.position, distance);
		}

		// Token: 0x060030D5 RID: 12501 RVA: 0x001133A0 File Offset: 0x001115A0
		public void Splatter(int amount, Vector3 origin, float distance)
		{
			if (!this.MudBlobsVFX || !this.blobSpawnTarget1 || !this.blobSpawnTarget2)
			{
				Debug.LogWarning("You must set references for mud blob vfx and spawn targets to splatter mud blobs");
				return;
			}
			if (distance == 0f)
			{
				distance = this.defaultDistance;
			}
			this.MudBlobsVFX.SetBool("Spawn Over Distance", false);
			this.MudBlobsVFX.SetInt("Mud Blobs Amount", 2);
			base.StartCoroutine(this.PlaceMudBlobs(amount, origin, distance));
		}

		// Token: 0x060030D6 RID: 12502 RVA: 0x00113421 File Offset: 0x00111621
		public IEnumerator PlaceMudBlobs(int amount, Vector3 origin, float distance)
		{
			int total = 0;
			while (total < amount)
			{
				this.blobSpawnTarget1.position = Random.insideUnitSphere * distance + origin;
				if (this.spawn1MudSurfaceSnap)
				{
					this.spawn1MudSurfaceSnap.SnapToMudSurface();
				}
				this.blobSpawnTarget2.position = Random.insideUnitSphere * distance + origin;
				if (this.spawn2MudSurfaceSnap)
				{
					this.spawn2MudSurfaceSnap.SnapToMudSurface();
				}
				this.MudBlobsVFX.Play();
				total += 2;
				yield return null;
			}
			yield break;
		}

		// Token: 0x0400313C RID: 12604
		[SerializeField]
		private VisualEffect MudBlobsVFX;

		// Token: 0x0400313D RID: 12605
		[SerializeField]
		private int defaultAmount = 6;

		// Token: 0x0400313E RID: 12606
		[SerializeField]
		private float defaultDistance = 1f;

		// Token: 0x0400313F RID: 12607
		[SerializeField]
		private Transform blobSpawnTarget1;

		// Token: 0x04003140 RID: 12608
		[SerializeField]
		private Transform blobSpawnTarget2;

		// Token: 0x04003141 RID: 12609
		private StayOnMudSurface spawn1MudSurfaceSnap;

		// Token: 0x04003142 RID: 12610
		private StayOnMudSurface spawn2MudSurfaceSnap;
	}
}
