using System;
using System.IO;
using DA_Assets.DAI;
using UnityEngine;

namespace DA_Assets.FCU.Model
{
	// Token: 0x02000383 RID: 899
	[Serializable]
	public class PrefabSettings : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x06001788 RID: 6024 RVA: 0x0009745B File Offset: 0x0009565B
		// (set) Token: 0x06001789 RID: 6025 RVA: 0x00097463 File Offset: 0x00095663
		public string PrefabsPath
		{
			get
			{
				return this.prefabsPath;
			}
			set
			{
				base.SetValue<string>(ref this.prefabsPath, value);
			}
		}

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x0600178A RID: 6026 RVA: 0x00097472 File Offset: 0x00095672
		// (set) Token: 0x0600178B RID: 6027 RVA: 0x0009747A File Offset: 0x0009567A
		public TextPrefabNameType TextPrefabNameType
		{
			get
			{
				return this.textPrefabNameType;
			}
			set
			{
				base.SetValue<TextPrefabNameType>(ref this.textPrefabNameType, value);
			}
		}

		// Token: 0x04001908 RID: 6408
		[SerializeField]
		private string prefabsPath = Path.Combine("Assets", "Prefabs");

		// Token: 0x04001909 RID: 6409
		[SerializeField]
		private TextPrefabNameType textPrefabNameType;
	}
}
