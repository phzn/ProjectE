using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Threading.Tasks;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;

namespace DA_Assets.FCU
{
	// Token: 0x02000351 RID: 849
	[Serializable]
	public class SpritePathSetter : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x06001670 RID: 5744 RVA: 0x00093470 File Offset: 0x00091670
		public async Task SetSpritePaths(List<FObject> fobjects)
		{
			await Task.Yield();
		}

		// Token: 0x06001671 RID: 5745 RVA: 0x000934AC File Offset: 0x000916AC
		private string GetSpritePath(FObject fobject)
		{
			string text;
			if (fobject.Data.IsMutual)
			{
				text = "Mutual";
			}
			else
			{
				text = fobject.Data.RootFrame.Names.FileName;
			}
			string text2 = Path.Combine(this.monoBeh.Settings.ImageSpritesSettings.SpritesPath.GetFullAssetPath(), text);
			string text3 = Path.Combine(this.monoBeh.Settings.ImageSpritesSettings.SpritesPath, text, fobject.Data.Names.FileName);
			text2.CreateFolderIfNotExists();
			return text3;
		}

		// Token: 0x06001672 RID: 5746 RVA: 0x0009353C File Offset: 0x0009173C
		private bool IsTargetExtension(FObject fobject, string spritePath)
		{
			string text = Path.GetExtension(spritePath);
			if (text.StartsWith(".") && text.Length > 1)
			{
				text = text.Remove(0, 1);
			}
			string text2 = null;
			if (this.monoBeh.UsingSvgImage() && fobject.CanUseUnityImage(this.monoBeh))
			{
				text2 = ImageFormat.PNG.ToLower();
			}
			if (text2 == null)
			{
				text2 = this.monoBeh.Settings.ImageSpritesSettings.ImageFormat.ToLower();
			}
			return text == text2;
		}

		// Token: 0x06001673 RID: 5747 RVA: 0x000935C4 File Offset: 0x000917C4
		public bool GetSpritePath(FObject fobject, string[] spritePathes, out string path)
		{
			foreach (string text in spritePathes)
			{
				float num;
				BigInteger bigInteger;
				bool flag = text.TryParseSpriteName(out num, out bigInteger);
				float num2;
				BigInteger bigInteger2;
				bool flag2 = fobject.Data.Names.FileName.TryParseSpriteName(out num2, out bigInteger2);
				bool flag3 = flag && flag2;
				bool flag4 = this.IsTargetExtension(fobject, text);
				bool flag5 = bigInteger == bigInteger2;
				if (flag3 && flag4 && flag5)
				{
					path = text;
					return true;
				}
			}
			path = null;
			return false;
		}

		// Token: 0x06001674 RID: 5748 RVA: 0x00093634 File Offset: 0x00091834
		private void SetNeedDownloadFileFlag(FObject fobject, bool imageFileExists)
		{
			if (!fobject.IsDownloadableType())
			{
				fobject.Data.NeedDownload = false;
				return;
			}
			if (this.monoBeh.Settings.ImageSpritesSettings.RedownloadSprites)
			{
				fobject.Data.NeedDownload = true;
				return;
			}
			if (imageFileExists)
			{
				fobject.Data.NeedDownload = false;
				return;
			}
			fobject.Data.NeedDownload = true;
		}

		// Token: 0x06001675 RID: 5749 RVA: 0x0009369C File Offset: 0x0009189C
		private void SetNeedGenerateFlag(FObject fobject, bool imageFileExists)
		{
			if (!fobject.IsGenerativeType())
			{
				fobject.Data.NeedGenerate = false;
				return;
			}
			if (this.monoBeh.Settings.ImageSpritesSettings.RedownloadSprites)
			{
				fobject.Data.NeedGenerate = true;
				return;
			}
			if (imageFileExists)
			{
				fobject.Data.NeedGenerate = false;
				return;
			}
			fobject.Data.NeedGenerate = true;
		}
	}
}
