using System;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;

namespace DA_Assets.FCU
{
	// Token: 0x020002C6 RID: 710
	[Serializable]
	public class FolderCreator : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x060013EF RID: 5103 RVA: 0x0007FA68 File Offset: 0x0007DC68
		public void CreateAll()
		{
			this.monoBeh.FontLoader.TtfFontsPath.CreateFolderIfNotExists();
			this.monoBeh.FontLoader.TmpFontsPath.CreateFolderIfNotExists();
			if (this.monoBeh.IsUITK())
			{
				this.monoBeh.Settings.UITK_Settings.UitkOutputPath.CreateFolderIfNotExists();
			}
			this.monoBeh.Settings.ImageSpritesSettings.SpritesPath.CreateFolderIfNotExists();
			this.monoBeh.Settings.ScriptGeneratorSettings.OutputPath.CreateFolderIfNotExists();
			this.monoBeh.Settings.PrefabSettings.PrefabsPath.CreateFolderIfNotExists();
		}
	}
}
