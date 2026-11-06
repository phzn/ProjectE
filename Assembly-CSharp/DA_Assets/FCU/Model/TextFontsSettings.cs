using System;
using DA_Assets.DAI;
using UnityEngine;

namespace DA_Assets.FCU.Model
{
	// Token: 0x02000388 RID: 904
	[Serializable]
	public class TextFontsSettings : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x0600179F RID: 6047 RVA: 0x000975D6 File Offset: 0x000957D6
		// (set) Token: 0x060017A0 RID: 6048 RVA: 0x000975DE File Offset: 0x000957DE
		public TextComponent TextComponent
		{
			get
			{
				return this.textComponent;
			}
			set
			{
				base.SetValue<TextComponent>(ref this.textComponent, value);
			}
		}

		// Token: 0x0400191A RID: 6426
		[SerializeField]
		private TextComponent textComponent;
	}
}
