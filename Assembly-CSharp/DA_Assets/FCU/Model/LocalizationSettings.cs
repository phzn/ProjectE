using System;
using System.IO;
using DA_Assets.DAI;
using DA_Assets.Logging;
using DA_Assets.Tools;
using UnityEngine;

namespace DA_Assets.FCU.Model
{
	// Token: 0x02000380 RID: 896
	[Serializable]
	public class LocalizationSettings : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x0600175D RID: 5981 RVA: 0x0009705B File Offset: 0x0009525B
		// (set) Token: 0x0600175E RID: 5982 RVA: 0x00097064 File Offset: 0x00095264
		public LocalizationComponent LocalizationComponent
		{
			get
			{
				return this.locComponent;
			}
			set
			{
				if (value != LocalizationComponent.DALocalizator)
				{
					if (value == LocalizationComponent.I2Localization)
					{
						DALogger.LogError(FcuLocKey.log_asset_not_imported.Localize(new object[] { "I2Localization" }));
						value = LocalizationComponent.None;
					}
				}
				else
				{
					DALogger.LogError(FcuLocKey.log_asset_not_imported.Localize(new object[] { "DALocalizator" }));
					value = LocalizationComponent.None;
				}
				base.SetValue<LocalizationComponent>(ref this.locComponent, value);
			}
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x0600175F RID: 5983 RVA: 0x000970C4 File Offset: 0x000952C4
		// (set) Token: 0x06001760 RID: 5984 RVA: 0x000970CC File Offset: 0x000952CC
		[SerializeProperty("locKeyCaseType")]
		public LocalizationKeyCaseType LocKeyCaseType
		{
			get
			{
				return this.locKeyCaseType;
			}
			set
			{
				base.SetValue<LocalizationKeyCaseType>(ref this.locKeyCaseType, value);
			}
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06001761 RID: 5985 RVA: 0x000970DB File Offset: 0x000952DB
		// (set) Token: 0x06001762 RID: 5986 RVA: 0x000970E3 File Offset: 0x000952E3
		public string CurrentFigmaLayoutCulture
		{
			get
			{
				return this.currentFigmaLayoutCulture;
			}
			set
			{
				base.SetValue<string>(ref this.currentFigmaLayoutCulture, value);
			}
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06001763 RID: 5987 RVA: 0x000970F2 File Offset: 0x000952F2
		// (set) Token: 0x06001764 RID: 5988 RVA: 0x000970FA File Offset: 0x000952FA
		public int LocKeyMaxLenght
		{
			get
			{
				return this.locKeyMaxLenght;
			}
			set
			{
				base.SetValue<int>(ref this.locKeyMaxLenght, value);
			}
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06001765 RID: 5989 RVA: 0x00097109 File Offset: 0x00095309
		// (set) Token: 0x06001766 RID: 5990 RVA: 0x00097111 File Offset: 0x00095311
		public ScriptableObject Localizator
		{
			get
			{
				return this.localizator;
			}
			set
			{
				base.SetValue<ScriptableObject>(ref this.localizator, value);
			}
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06001767 RID: 5991 RVA: 0x00097120 File Offset: 0x00095320
		// (set) Token: 0x06001768 RID: 5992 RVA: 0x00097128 File Offset: 0x00095328
		public string LocFolderPath
		{
			get
			{
				return this.locFolderPath;
			}
			set
			{
				base.SetValue<string>(ref this.locFolderPath, value);
			}
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x06001769 RID: 5993 RVA: 0x00097137 File Offset: 0x00095337
		// (set) Token: 0x0600176A RID: 5994 RVA: 0x0009713F File Offset: 0x0009533F
		public string LocFileName
		{
			get
			{
				return this.locFileName;
			}
			set
			{
				base.SetValue<string>(ref this.locFileName, value);
			}
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x0600176B RID: 5995 RVA: 0x0009714E File Offset: 0x0009534E
		// (set) Token: 0x0600176C RID: 5996 RVA: 0x00097156 File Offset: 0x00095356
		public CsvSeparator CsvSeparator
		{
			get
			{
				return this.csvSeparator;
			}
			set
			{
				base.SetValue<CsvSeparator>(ref this.csvSeparator, value);
			}
		}

		// Token: 0x040018F4 RID: 6388
		[SerializeField]
		private LocalizationComponent locComponent;

		// Token: 0x040018F5 RID: 6389
		[SerializeField]
		private LocalizationKeyCaseType locKeyCaseType;

		// Token: 0x040018F6 RID: 6390
		[SerializeField]
		private string currentFigmaLayoutCulture = FcuConfig.DefaultLocalizationCulture;

		// Token: 0x040018F7 RID: 6391
		[SerializeField]
		private int locKeyMaxLenght = 24;

		// Token: 0x040018F8 RID: 6392
		[SerializeField]
		private ScriptableObject localizator;

		// Token: 0x040018F9 RID: 6393
		[SerializeField]
		private string locFolderPath = Path.Combine("Assets", "Resources", "Localizations");

		// Token: 0x040018FA RID: 6394
		[SerializeField]
		private string locFileName = "Localization.csv";

		// Token: 0x040018FB RID: 6395
		[SerializeField]
		private CsvSeparator csvSeparator = CsvSeparator.Semicolon;
	}
}
