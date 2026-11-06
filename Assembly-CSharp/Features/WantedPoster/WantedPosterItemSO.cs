using System;
using Features.Inventory;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.WantedPoster
{
	// Token: 0x02000B70 RID: 2928
	[CreateAssetMenu(fileName = "Item_WantedPoster", menuName = "Features/Wanted Poster/Item Definition")]
	public sealed class WantedPosterItemSO : InventoryItemSO, IInventoryInstanceVisualProvider, IInventoryPlacementPoseProvider, IInventoryPlacementPlayerVisibilityProvider
	{
		// Token: 0x17000DE5 RID: 3557
		// (get) Token: 0x06005339 RID: 21305 RVA: 0x0002AE8C File Offset: 0x0002908C
		public bool HidePlayerVisualsDuringPlacement
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600533A RID: 21306 RVA: 0x00204BF5 File Offset: 0x00202DF5
		public void ConfigureHandInstance(GameObject instance)
		{
			WantedPosterPhotoSystem instance2 = WantedPosterPhotoSystem.Instance;
			if (instance2 == null)
			{
				return;
			}
			instance2.ApplyCurrentMaterialSnapshotTo(instance);
		}

		// Token: 0x0600533B RID: 21307 RVA: 0x00204C08 File Offset: 0x00202E08
		public void ConfigureWorldInstance(GameObject instance)
		{
			if (instance == null)
			{
				return;
			}
			foreach (WantedPoster wantedPoster in instance.GetComponentsInChildren<WantedPoster>(true))
			{
				if (wantedPoster != null)
				{
					wantedPoster.SetPlayerPlaced();
				}
			}
			WantedPosterPickup[] componentsInChildren2 = instance.GetComponentsInChildren<WantedPosterPickup>(true);
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				if (componentsInChildren2[j] != null)
				{
					componentsInChildren2[j].enabled = false;
				}
			}
			if (Vector3.Dot(instance.transform.forward, Vector3.up) > 0.9f)
			{
				InteractablePromptUI componentInChildren = instance.GetComponentInChildren<InteractablePromptUI>(true);
				Renderer componentInChildren2 = instance.GetComponentInChildren<Renderer>(true);
				if (componentInChildren != null && componentInChildren2 != null)
				{
					componentInChildren.transform.position = componentInChildren2.bounds.center + Vector3.up * 0.06f;
					componentInChildren.transform.rotation = Quaternion.identity;
				}
			}
		}

		// Token: 0x0600533C RID: 21308 RVA: 0x00204CF2 File Offset: 0x00202EF2
		public Vector3 AdjustPlacementPosition(Vector3 surfacePosition, Vector3 surfaceNormal, bool wall)
		{
			if (!wall)
			{
				return surfacePosition + surfaceNormal.normalized * 0.003f;
			}
			return surfacePosition;
		}

		// Token: 0x0600533D RID: 21309 RVA: 0x00204D10 File Offset: 0x00202F10
		public Quaternion AdjustPlacementRotation(Quaternion surfaceRotation, bool wall)
		{
			if (!wall)
			{
				return surfaceRotation * Quaternion.AngleAxis(-90f, Vector3.right) * Quaternion.AngleAxis(180f, Vector3.forward);
			}
			return surfaceRotation;
		}

		// Token: 0x040053B8 RID: 21432
		private const float SurfaceClearance = 0.003f;
	}
}
