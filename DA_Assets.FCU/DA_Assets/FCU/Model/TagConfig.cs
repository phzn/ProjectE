using System;
using UnityEngine;

namespace DA_Assets.FCU.Model
{
	// Token: 0x0200007D RID: 125
	[Serializable]
	public struct TagConfig
	{
		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000331 RID: 817 RVA: 0x0000798A File Offset: 0x00005B8A
		public FcuTag FcuTag
		{
			get
			{
				return this.fcuTag;
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000332 RID: 818 RVA: 0x00007992 File Offset: 0x00005B92
		public string FigmaTag
		{
			get
			{
				return this.figmaTag;
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000333 RID: 819 RVA: 0x0000799A File Offset: 0x00005B9A
		public bool CustomButtonTag
		{
			get
			{
				return this.customButtonTag;
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000334 RID: 820 RVA: 0x000079A2 File Offset: 0x00005BA2
		public bool CanBeDownloaded
		{
			get
			{
				return this.canBeDownloaded;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000335 RID: 821 RVA: 0x000079AA File Offset: 0x00005BAA
		public bool CanBeInsideSingleImage
		{
			get
			{
				return this.canBeInsideSingleImage;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000336 RID: 822 RVA: 0x000079B2 File Offset: 0x00005BB2
		public bool HasComponent
		{
			get
			{
				return this.hasComponent;
			}
		}

		// Token: 0x06000337 RID: 823 RVA: 0x000079BA File Offset: 0x00005BBA
		public void SetDefaultData(FcuTag fcuTag)
		{
			this.label = fcuTag.ToString();
			this.fcuTag = fcuTag;
		}

		// Token: 0x0400047C RID: 1148
		[SerializeField]
		private string label;

		// Token: 0x0400047D RID: 1149
		[SerializeField]
		private FcuTag fcuTag;

		// Token: 0x0400047E RID: 1150
		[SerializeField]
		private string figmaTag;

		// Token: 0x0400047F RID: 1151
		[SerializeField]
		private bool customButtonTag;

		// Token: 0x04000480 RID: 1152
		[SerializeField]
		private bool canBeDownloaded;

		// Token: 0x04000481 RID: 1153
		[SerializeField]
		private bool canBeInsideSingleImage;

		// Token: 0x04000482 RID: 1154
		[SerializeField]
		private bool hasComponent;
	}
}
