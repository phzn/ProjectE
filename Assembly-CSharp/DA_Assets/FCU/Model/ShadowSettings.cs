using System;
using DA_Assets.DAI;
using DA_Assets.Logging;
using UnityEngine;

namespace DA_Assets.FCU.Model
{
	// Token: 0x02000387 RID: 903
	[Serializable]
	public class ShadowSettings : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x0600179C RID: 6044 RVA: 0x0009759E File Offset: 0x0009579E
		// (set) Token: 0x0600179D RID: 6045 RVA: 0x000975A6 File Offset: 0x000957A6
		public ShadowComponent ShadowComponent
		{
			get
			{
				return this.shadowComponent;
			}
			set
			{
				if (value == ShadowComponent.TrueShadow)
				{
					DALogger.LogError(FcuLocKey.log_asset_not_imported.Localize(new object[] { "TrueShadow" }));
					value = ShadowComponent.Figma;
				}
				base.SetValue<ShadowComponent>(ref this.shadowComponent, value);
			}
		}

		// Token: 0x04001919 RID: 6425
		[SerializeField]
		private ShadowComponent shadowComponent;
	}
}
