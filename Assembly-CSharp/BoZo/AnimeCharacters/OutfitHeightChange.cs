using System;
using UnityEngine;

namespace Bozo.AnimeCharacters
{
	// Token: 0x020008BF RID: 2239
	public class OutfitHeightChange : MonoBehaviour
	{
		// Token: 0x06003C28 RID: 15400 RVA: 0x00165E00 File Offset: 0x00164000
		private void OnEnable()
		{
			OutfitSystem componentInParent = base.GetComponentInParent<OutfitSystem>();
			if (componentInParent == null)
			{
				return;
			}
			componentInParent.transform.localPosition = new Vector3(componentInParent.transform.localPosition.x, componentInParent.transform.localPosition.y + this.HeightOffset, componentInParent.transform.localPosition.z);
		}

		// Token: 0x06003C29 RID: 15401 RVA: 0x00165E68 File Offset: 0x00164068
		private void OnDisable()
		{
			OutfitSystem componentInParent = base.GetComponentInParent<OutfitSystem>();
			if (componentInParent == null)
			{
				return;
			}
			componentInParent.transform.localPosition = new Vector3(componentInParent.transform.localPosition.x, componentInParent.transform.localPosition.y - this.HeightOffset, componentInParent.transform.localPosition.z);
		}

		// Token: 0x04003C3D RID: 15421
		[SerializeField]
		private float HeightOffset;
	}
}
