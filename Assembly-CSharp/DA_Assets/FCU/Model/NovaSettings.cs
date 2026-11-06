using System;
using DA_Assets.DAI;
using UnityEngine;

namespace DA_Assets.FCU.Model
{
	// Token: 0x02000382 RID: 898
	[Serializable]
	public class NovaSettings : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x06001785 RID: 6021 RVA: 0x00097444 File Offset: 0x00095644
		// (set) Token: 0x06001786 RID: 6022 RVA: 0x0009744C File Offset: 0x0009564C
		[SerializeProperty("inputTexture")]
		public Texture InputTexture
		{
			get
			{
				return this.inputTexture;
			}
			set
			{
				base.SetValue<Texture>(ref this.inputTexture, value);
			}
		}

		// Token: 0x04001907 RID: 6407
		[SerializeField]
		private Texture inputTexture;
	}
}
