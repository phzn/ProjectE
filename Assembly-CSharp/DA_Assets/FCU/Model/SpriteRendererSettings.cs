using System;
using DA_Assets.DAI;
using UnityEngine;

namespace DA_Assets.FCU.Model
{
	// Token: 0x0200037C RID: 892
	[FcuPropertyHeader(FcuLocKey.label_sr_settings, FcuLocKey.tooltip_sr_settings)]
	[Serializable]
	public class SpriteRendererSettings : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x06001733 RID: 5939 RVA: 0x00096DF2 File Offset: 0x00094FF2
		// (set) Token: 0x06001734 RID: 5940 RVA: 0x00096DFA File Offset: 0x00094FFA
		[FcuInspectorProperty(ComponentType.Toggle, FcuLocKey.label_flip_x, FcuLocKey.tooltip_flip_x, 0f, 1f)]
		public bool FlipX
		{
			get
			{
				return this.flipX;
			}
			set
			{
				base.SetValue<bool>(ref this.flipX, value);
			}
		}

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06001735 RID: 5941 RVA: 0x00096E09 File Offset: 0x00095009
		// (set) Token: 0x06001736 RID: 5942 RVA: 0x00096E11 File Offset: 0x00095011
		[FcuInspectorProperty(ComponentType.Toggle, FcuLocKey.label_flip_y, FcuLocKey.tooltip_flip_y, 0f, 1f)]
		public bool FlipY
		{
			get
			{
				return this.flipY;
			}
			set
			{
				base.SetValue<bool>(ref this.flipY, value);
			}
		}

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x06001737 RID: 5943 RVA: 0x00096E20 File Offset: 0x00095020
		// (set) Token: 0x06001738 RID: 5944 RVA: 0x00096E28 File Offset: 0x00095028
		[FcuInspectorProperty(ComponentType.EnumField, FcuLocKey.label_mask_interaction, FcuLocKey.tooltip_mask_interaction, 0f, 1f)]
		public SpriteMaskInteraction MaskInteraction
		{
			get
			{
				return this.maskInteraction;
			}
			set
			{
				base.SetValue<SpriteMaskInteraction>(ref this.maskInteraction, value);
			}
		}

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x06001739 RID: 5945 RVA: 0x00096E37 File Offset: 0x00095037
		// (set) Token: 0x0600173A RID: 5946 RVA: 0x00096E3F File Offset: 0x0009503F
		[FcuInspectorProperty(ComponentType.EnumField, FcuLocKey.label_sort_point, FcuLocKey.tooltip_sort_point, 0f, 1f)]
		public SpriteSortPoint SortPoint
		{
			get
			{
				return this.sortPoint;
			}
			set
			{
				base.SetValue<SpriteSortPoint>(ref this.sortPoint, value);
			}
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x0600173B RID: 5947 RVA: 0x00096E4E File Offset: 0x0009504E
		// (set) Token: 0x0600173C RID: 5948 RVA: 0x00096E56 File Offset: 0x00095056
		[FcuInspectorProperty(ComponentType.TextField, FcuLocKey.label_sorting_layer, FcuLocKey.tooltip_sorting_layer, 0f, 1f)]
		public string SortingLayer
		{
			get
			{
				return this.sortingLayer;
			}
			set
			{
				base.SetValue<string>(ref this.sortingLayer, value);
			}
		}

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x0600173D RID: 5949 RVA: 0x00096E65 File Offset: 0x00095065
		// (set) Token: 0x0600173E RID: 5950 RVA: 0x00096E6D File Offset: 0x0009506D
		[FcuInspectorProperty(ComponentType.IntField, FcuLocKey.label_next_order_step, FcuLocKey.tooltip_next_order_step, 0f, 1f)]
		public int NextOrderStep
		{
			get
			{
				return this.nextOrderStep;
			}
			set
			{
				base.SetValue<int>(ref this.nextOrderStep, value);
			}
		}

		// Token: 0x040018E1 RID: 6369
		[SerializeField]
		private bool flipX;

		// Token: 0x040018E2 RID: 6370
		[SerializeField]
		private bool flipY;

		// Token: 0x040018E3 RID: 6371
		[SerializeField]
		private SpriteMaskInteraction maskInteraction;

		// Token: 0x040018E4 RID: 6372
		[SerializeField]
		private SpriteSortPoint sortPoint;

		// Token: 0x040018E5 RID: 6373
		[SerializeField]
		private string sortingLayer = "Default";

		// Token: 0x040018E6 RID: 6374
		[SerializeField]
		private int nextOrderStep = 10;
	}
}
