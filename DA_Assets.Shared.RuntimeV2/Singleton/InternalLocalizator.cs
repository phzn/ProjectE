using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace DA_Assets.Singleton
{
	// Token: 0x02000004 RID: 4
	[Serializable]
	public class InternalLocalizator
	{
		// Token: 0x06000006 RID: 6 RVA: 0x000020DC File Offset: 0x000002DC
		public InternalLocalizator Init()
		{
			return this;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000020E0 File Offset: 0x000002E0
		private List<LocItem> ConvertFileToLocItems(string csvText)
		{
			List<LocItem> list = new List<LocItem>();
			using (StringReader stringReader = new StringReader(csvText))
			{
				LocItem locItem = default(LocItem);
				StringBuilder stringBuilder = null;
				string text;
				while ((text = stringReader.ReadLine()) != null)
				{
					if (locItem.Equals(default(LocItem)))
					{
						string[] array = text.Split(';', StringSplitOptions.None);
						if (array.Length >= 2)
						{
							locItem = new LocItem
							{
								key = array[0],
								en = array[1]
							};
							if (locItem.en.StartsWith("\"") && !locItem.en.EndsWith("\""))
							{
								stringBuilder = new StringBuilder();
								stringBuilder.AppendLine(locItem.en.TrimStart('"'));
							}
							else
							{
								list.Add(locItem);
								locItem = default(LocItem);
							}
						}
					}
					else if (stringBuilder != null)
					{
						if (text.EndsWith("\"") && !text.EndsWith(";\""))
						{
							stringBuilder.AppendLine(text.TrimEnd('"'));
							locItem.en = stringBuilder.ToString();
							list.Add(locItem);
							locItem = default(LocItem);
							stringBuilder = null;
						}
						else
						{
							stringBuilder.AppendLine(text);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002248 File Offset: 0x00000448
		public string GetLocalizedText(Enum key, params object[] args)
		{
			return this.GetLocalizedText(key.ToString(), args);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002258 File Offset: 0x00000458
		private string GetLocalizedText(string key, params object[] args)
		{
			foreach (LocItem locItem in this.localizations)
			{
				if (locItem.key == key)
				{
					string en = locItem.en;
					if (en == "")
					{
						return key;
					}
					try
					{
						return string.Format(en, args);
					}
					catch
					{
						return en;
					}
				}
			}
			return key;
		}

		// Token: 0x04000003 RID: 3
		[SerializeField]
		private TextAsset locFile;

		// Token: 0x04000004 RID: 4
		private List<LocItem> localizations = new List<LocItem>();
	}
}
