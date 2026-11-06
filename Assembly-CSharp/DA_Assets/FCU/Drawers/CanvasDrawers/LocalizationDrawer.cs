using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using UnityEngine;

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
	// Token: 0x020003BD RID: 957
	[Serializable]
	public class LocalizationDrawer : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06001897 RID: 6295 RVA: 0x0009CBF8 File Offset: 0x0009ADF8
		public Dictionary<string, string> LocalizationDictionary
		{
			get
			{
				return this._localizationDictionary;
			}
		}

		// Token: 0x06001898 RID: 6296 RVA: 0x0009CC00 File Offset: 0x0009AE00
		internal void Init()
		{
			this._localizationDictionary.Clear();
			LocalizationComponent localizationComponent = this.monoBeh.Settings.LocalizationSettings.LocalizationComponent;
			if (localizationComponent != LocalizationComponent.DALocalizator)
			{
				return;
			}
			this.DALocalizatorDrawer.Init();
		}

		// Token: 0x06001899 RID: 6297 RVA: 0x0009CC44 File Offset: 0x0009AE44
		public void Draw(FObject fobject)
		{
			string locKey = fobject.Data.Names.LocKey;
			if (locKey.IsEmpty())
			{
				return;
			}
			string text = fobject.GetText();
			if (text.IsEmpty())
			{
				return;
			}
			this._localizationDictionary.TryAddValue(locKey, text);
			LocalizationComponent localizationComponent = this.monoBeh.Settings.LocalizationSettings.LocalizationComponent;
			if (localizationComponent != LocalizationComponent.DALocalizator)
			{
				return;
			}
			this.DALocalizatorDrawer.Draw(locKey, fobject);
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x0009CCB8 File Offset: 0x0009AEB8
		public string SaveTable()
		{
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("Key", typeof(string));
			dataTable.Columns.Add(this.monoBeh.Settings.LocalizationSettings.CurrentFigmaLayoutCulture, typeof(string));
			foreach (KeyValuePair<string, string> keyValuePair in this._localizationDictionary)
			{
				dataTable.Rows.Add(new object[] { keyValuePair.Key, keyValuePair.Value });
			}
			string locFolderPath = this.monoBeh.Settings.LocalizationSettings.LocFolderPath;
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(this.monoBeh.Settings.LocalizationSettings.LocFileName);
			string extension = Path.GetExtension(this.monoBeh.Settings.LocalizationSettings.LocFileName);
			if (!Directory.Exists(locFolderPath))
			{
				Directory.CreateDirectory(locFolderPath);
			}
			int num = Directory.GetFiles(locFolderPath, fileNameWithoutExtension + "*" + extension).Length;
			string text = string.Format("{0}-{1}{2}", fileNameWithoutExtension, num + 1, extension);
			return LocalizationDrawer.RemoveAssetsAndResourcesFolders(Path.Combine(locFolderPath, text)).RemovePathExtension();
		}

		// Token: 0x0600189B RID: 6299 RVA: 0x0009CE14 File Offset: 0x0009B014
		private static string RemoveAssetsAndResourcesFolders(string path)
		{
			string text = path.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
			string text2 = string.Format("Assets{0}Resources{1}", Path.DirectorySeparatorChar, Path.DirectorySeparatorChar);
			string text3 = string.Format("Assets{0}", Path.DirectorySeparatorChar);
			if (text.StartsWith(text2))
			{
				return text.Substring(text2.Length);
			}
			if (text.StartsWith(text3))
			{
				return text.Substring(text3.Length);
			}
			return text;
		}

		// Token: 0x0600189C RID: 6300 RVA: 0x0009CE9C File Offset: 0x0009B09C
		public void SaveAndConnectTable()
		{
			string text = this.SaveTable();
			this.ConnectTable(text);
		}

		// Token: 0x0600189D RID: 6301 RVA: 0x0009CEB8 File Offset: 0x0009B0B8
		internal void ConnectTable(string filePath)
		{
			LocalizationComponent localizationComponent = this.monoBeh.Settings.LocalizationSettings.LocalizationComponent;
			if (localizationComponent != LocalizationComponent.DALocalizator)
			{
				return;
			}
			this.DALocalizatorDrawer.ConnectTable(filePath);
		}

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x0600189E RID: 6302 RVA: 0x0009CEF0 File Offset: 0x0009B0F0
		[SerializeProperty("dalDrawer")]
		public DALocalizatorDrawer DALocalizatorDrawer
		{
			get
			{
				return this.monoBeh.Link(ref this.dalDrawer);
			}
		}

		// Token: 0x040019BF RID: 6591
		private Dictionary<string, string> _localizationDictionary = new Dictionary<string, string>();

		// Token: 0x040019C0 RID: 6592
		[SerializeField]
		private DALocalizatorDrawer dalDrawer;
	}
}
