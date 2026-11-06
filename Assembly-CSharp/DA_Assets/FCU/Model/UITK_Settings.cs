using System;
using System.IO;
using DA_Assets.DAI;
using UnityEngine;

namespace DA_Assets.FCU.Model
{
	// Token: 0x0200038B RID: 907
	[Serializable]
	public class UITK_Settings : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x060017CA RID: 6090 RVA: 0x00097800 File Offset: 0x00095A00
		// (set) Token: 0x060017CB RID: 6091 RVA: 0x00097808 File Offset: 0x00095A08
		public string UitkOutputPath
		{
			get
			{
				return this.uitkOutputPath;
			}
			set
			{
				base.SetValue<string>(ref this.uitkOutputPath, value);
			}
		}

		// Token: 0x0400192E RID: 6446
		[SerializeField]
		private string uitkOutputPath = Path.Combine("Assets", "UITK Output");
	}
}
