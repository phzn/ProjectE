using System;
using DA_Assets.DAI;
using DA_Assets.FCU.Model;
using DA_Assets.Logging;
using UnityEngine;

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
	// Token: 0x020003BC RID: 956
	[Serializable]
	public class DALocalizatorDrawer : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x06001893 RID: 6291 RVA: 0x00004F31 File Offset: 0x00003131
		public void Init()
		{
		}

		// Token: 0x06001894 RID: 6292 RVA: 0x0009CBA0 File Offset: 0x0009ADA0
		public void ConnectTable(string filePath)
		{
			if (this.monoBeh.Settings.LocalizationSettings.Localizator == null)
			{
				DALogger.LogError("Localizator is null.");
				return;
			}
			if (!(Resources.Load<TextAsset>(filePath) != null))
			{
				Debug.LogError("Localization file '" + filePath + "' could not be loaded!");
			}
		}

		// Token: 0x06001895 RID: 6293 RVA: 0x00004F31 File Offset: 0x00003131
		public void Draw(string locKey, FObject fobject)
		{
		}
	}
}
