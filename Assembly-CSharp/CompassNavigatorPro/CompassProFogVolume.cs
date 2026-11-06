using System;
using UnityEngine;

namespace CompassNavigatorPro
{
	// Token: 0x0200028C RID: 652
	[ExecuteInEditMode]
	public class CompassProFogVolume : MonoBehaviour
	{
		// Token: 0x0600130B RID: 4875 RVA: 0x0007BF6F File Offset: 0x0007A16F
		private void OnEnable()
		{
			if (!Application.isPlaying)
			{
				this.ShowFogArea(true);
			}
		}

		// Token: 0x0600130C RID: 4876 RVA: 0x0007BF7F File Offset: 0x0007A17F
		private void Start()
		{
			if (Application.isPlaying)
			{
				this.ShowFogArea(true);
			}
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x0007BF8F File Offset: 0x0007A18F
		private void OnDisable()
		{
			this.ShowFogArea(false);
		}

		// Token: 0x0600130E RID: 4878 RVA: 0x0007BF98 File Offset: 0x0007A198
		private void Update()
		{
			if (this.order != this.oldOrder || this.alpha != this.oldAlpha || this.border != this.oldBorder || base.transform.position != this.oldPos || base.transform.localScale != this.oldScale)
			{
				this.NotifyChanges();
			}
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x0007C008 File Offset: 0x0007A208
		private void NotifyChanges()
		{
			this.oldPos = base.transform.position;
			this.oldScale = base.transform.localScale;
			this.oldAlpha = this.alpha;
			this.oldBorder = this.border;
			this.oldOrder = this.order;
			CompassPro instance = CompassPro.instance;
			if (instance != null)
			{
				instance.UpdateFogOfWar();
			}
		}

		// Token: 0x06001310 RID: 4880 RVA: 0x0007C070 File Offset: 0x0007A270
		private void ShowFogArea(bool state)
		{
			CompassPro instance = CompassPro.instance;
			if (instance != null)
			{
				Bounds bounds = base.GetComponent<BoxCollider>().bounds;
				float num = (state ? this.alpha : 0f);
				instance.SetFogOfWarAlpha(bounds, num, this.border);
			}
		}

		// Token: 0x040013D8 RID: 5080
		[Range(0f, 1f)]
		[Tooltip("Transparency of the fog of war. A value of 1 means fully opaque fog.")]
		public float alpha;

		// Token: 0x040013D9 RID: 5081
		[Range(0f, 1f)]
		[Tooltip("Controls the hardness of the border.")]
		public float border;

		// Token: 0x040013DA RID: 5082
		[Tooltip("Fog volumes are rendered in ascending order.")]
		public int order;

		// Token: 0x040013DB RID: 5083
		private Vector3 oldPos;

		// Token: 0x040013DC RID: 5084
		private Vector3 oldScale;

		// Token: 0x040013DD RID: 5085
		private float oldAlpha;

		// Token: 0x040013DE RID: 5086
		private float oldBorder;

		// Token: 0x040013DF RID: 5087
		private int oldOrder;
	}
}
