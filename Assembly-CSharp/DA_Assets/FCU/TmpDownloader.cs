using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DA_Assets.CssNet;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

namespace DA_Assets.FCU
{
	// Token: 0x0200030D RID: 781
	[Serializable]
	public class TmpDownloader : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x0600157C RID: 5500 RVA: 0x0008B3B4 File Offset: 0x000895B4
		public async Task CreateFonts(List<FontMetadata> figmaFonts)
		{
			this.unityTmpFonts = this.monoBeh.FontDownloader.FindUnityFonts<TMP_FontAsset>(figmaFonts, this.monoBeh.FontLoader.TmpFonts);
			if (this.unityTmpFonts.Missing.Count == 0)
			{
				await this.SetFallbackFontAssets(figmaFonts);
			}
			else
			{
				List<FontStruct> generated = new List<FontStruct>();
				List<FontError> notGenerated = new List<FontError>();
				using (List<FontStruct>.Enumerator enumerator = this.unityTmpFonts.Missing.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						FontStruct missingFont = enumerator.Current;
						this.GenerateFont(missingFont, delegate(DAResult<FontStruct> @return)
						{
							generated.Add(@return.Object);
							if (!@return.Success)
							{
								notGenerated.Add(new FontError
								{
									FontStruct = missingFont,
									Error = @return.Error
								});
							}
						});
					}
				}
				int tempCount = -1;
				while (FcuLogger.WriteLogBeforeEqual(generated, this.unityTmpFonts.Missing, FcuLocKey.log_generating_tmp_fonts, ref tempCount))
				{
					await Task.Delay(1000);
				}
				if (notGenerated.Count > 0)
				{
					this.monoBeh.FontDownloader.PrintFontNames(FcuLocKey.cant_generate_fonts, notGenerated);
				}
				await this.monoBeh.FontLoader.AddToTmpMeshFontsList();
				await this.SetFallbackFontAssets(figmaFonts);
			}
		}

		// Token: 0x0600157D RID: 5501 RVA: 0x0008B400 File Offset: 0x00089600
		private async Task SetFallbackFontAssets(List<FontMetadata> figmaFonts)
		{
			this.unityTmpFonts = this.monoBeh.FontDownloader.FindUnityFonts<TMP_FontAsset>(figmaFonts, this.monoBeh.FontLoader.TmpFonts);
			if (this.unityTmpFonts.Existing.Count != 0)
			{
				FontStruct fontStruct = this.unityTmpFonts.Existing.FirstOrDefault<FontStruct>((FontStruct x) => x.FontSubset == FontSubset.Latin);
				if (!fontStruct.IsDefault<FontStruct>())
				{
					TMP_FontAsset tmp_FontAsset = fontStruct.Font as TMP_FontAsset;
					if (!this.unityTmpFonts.Existing.IsEmpty<FontStruct>())
					{
						foreach (FontStruct fontStruct2 in this.unityTmpFonts.Existing)
						{
							if (fontStruct2.FontSubset != FontSubset.Latin)
							{
								TMP_FontAsset tmp_FontAsset2 = fontStruct2.Font as TMP_FontAsset;
								if (tmp_FontAsset.fallbackFontAssetTable.IsEmpty<TMP_FontAsset>())
								{
									tmp_FontAsset.fallbackFontAssetTable = new List<TMP_FontAsset>();
								}
								tmp_FontAsset.fallbackFontAssetTable.Add(tmp_FontAsset2);
							}
						}
						if (!tmp_FontAsset.fallbackFontAssetTable.IsEmpty<TMP_FontAsset>())
						{
							tmp_FontAsset.fallbackFontAssetTable = tmp_FontAsset.fallbackFontAssetTable.Where<TMP_FontAsset>((TMP_FontAsset x) => x != null).ToList<TMP_FontAsset>();
						}
						if (!tmp_FontAsset.fallbackFontAssetTable.IsEmpty<TMP_FontAsset>())
						{
							tmp_FontAsset.fallbackFontAssetTable = tmp_FontAsset.fallbackFontAssetTable.Distinct<TMP_FontAsset>().ToList<TMP_FontAsset>();
						}
						await Task.Yield();
					}
				}
			}
		}

		// Token: 0x0600157E RID: 5502 RVA: 0x0008B44C File Offset: 0x0008964C
		private async Task GenerateFont(FontStruct fs, Return<FontStruct> @return)
		{
			await Task.Yield();
		}

		// Token: 0x0600157F RID: 5503 RVA: 0x0008B488 File Offset: 0x00089688
		private async Task<DAResult<string>> GetUnicodeRange(string family, FontSubset fontSubset)
		{
			string text = "https://fonts.googleapis.com/css2?family=" + family;
			DARequest darequest = new DARequest
			{
				RequestType = RequestType.Get,
				Query = text,
				RequestHeader = new RequestHeader
				{
					Name = "User-Agent",
					Value = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36"
				}
			};
			string @object = (await this.monoBeh.RequestSender.SendRequest<string>(darequest)).Object;
			DAResult<string> daresult;
			try
			{
				CssStylesheet cssStylesheet = new CssStylesheet(@object);
				string text2 = null;
				foreach (CssSelector cssSelector in cssStylesheet.selectors)
				{
					if (cssSelector.Name.Replace("}", "").Replace("{", "").Replace(" ", "")
						.Replace("/*", "")
						.Replace("*/", "")
						.Replace("@font-face", "")
						.Replace("-", "")
						.ToLower()
						.Trim() == fontSubset.ToLower())
					{
						text2 = cssSelector.rules.First<CssRule>((CssRule x) => x.rule == "unicode-range").objectValue.ToString().Replace("U+", "").Replace(" ", "")
							.Trim();
						break;
					}
				}
				if (text2.IsEmpty())
				{
					throw new Exception("Unicode range is empty");
				}
				daresult = new DAResult<string>
				{
					Success = true,
					Object = text2
				};
			}
			catch (Exception ex)
			{
				daresult = new DAResult<string>
				{
					Success = false,
					Error = new WebError(0, ex.Message, null)
				};
			}
			return daresult;
		}

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06001580 RID: 5504 RVA: 0x0008B4DB File Offset: 0x000896DB
		// (set) Token: 0x06001581 RID: 5505 RVA: 0x0008B4E3 File Offset: 0x000896E3
		public int SamplingPointSize
		{
			get
			{
				return this.samplingPointSize;
			}
			set
			{
				base.SetValue<int>(ref this.samplingPointSize, value);
			}
		}

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x06001582 RID: 5506 RVA: 0x0008B4F2 File Offset: 0x000896F2
		// (set) Token: 0x06001583 RID: 5507 RVA: 0x0008B4FA File Offset: 0x000896FA
		public int AtlasPadding
		{
			get
			{
				return this.atlasPadding;
			}
			set
			{
				base.SetValue<int>(ref this.atlasPadding, value);
			}
		}

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06001584 RID: 5508 RVA: 0x0008B509 File Offset: 0x00089709
		// (set) Token: 0x06001585 RID: 5509 RVA: 0x0008B511 File Offset: 0x00089711
		public GlyphRenderMode RenderMode
		{
			get
			{
				return this.renderMode;
			}
			set
			{
				base.SetValue<GlyphRenderMode>(ref this.renderMode, value);
			}
		}

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06001586 RID: 5510 RVA: 0x0008B520 File Offset: 0x00089720
		// (set) Token: 0x06001587 RID: 5511 RVA: 0x0008B528 File Offset: 0x00089728
		public int AtlasWidth
		{
			get
			{
				return this.atlasWidth;
			}
			set
			{
				base.SetValue<int>(ref this.atlasWidth, value);
			}
		}

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06001588 RID: 5512 RVA: 0x0008B537 File Offset: 0x00089737
		// (set) Token: 0x06001589 RID: 5513 RVA: 0x0008B53F File Offset: 0x0008973F
		public int AtlasHeight
		{
			get
			{
				return this.atlasHeight;
			}
			set
			{
				base.SetValue<int>(ref this.atlasHeight, value);
			}
		}

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x0600158A RID: 5514 RVA: 0x0008B54E File Offset: 0x0008974E
		// (set) Token: 0x0600158B RID: 5515 RVA: 0x0008B556 File Offset: 0x00089756
		public AtlasPopulationMode AtlasPopulationMode
		{
			get
			{
				return this.atlasPopulationMode;
			}
			set
			{
				base.SetValue<AtlasPopulationMode>(ref this.atlasPopulationMode, value);
			}
		}

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x0600158C RID: 5516 RVA: 0x0008B565 File Offset: 0x00089765
		// (set) Token: 0x0600158D RID: 5517 RVA: 0x0008B56D File Offset: 0x0008976D
		public bool EnableMultiAtlasSupport
		{
			get
			{
				return this.enableMultiAtlasSupport;
			}
			set
			{
				base.SetValue<bool>(ref this.enableMultiAtlasSupport, value);
			}
		}

		// Token: 0x04001719 RID: 5913
		[SerializeField]
		private int samplingPointSize = 90;

		// Token: 0x0400171A RID: 5914
		[SerializeField]
		private int atlasPadding = 5;

		// Token: 0x0400171B RID: 5915
		[SerializeField]
		private GlyphRenderMode renderMode = GlyphRenderMode.SDFAA;

		// Token: 0x0400171C RID: 5916
		[SerializeField]
		private int atlasWidth = 512;

		// Token: 0x0400171D RID: 5917
		[SerializeField]
		private int atlasHeight = 512;

		// Token: 0x0400171E RID: 5918
		[SerializeField]
		private AtlasPopulationMode atlasPopulationMode = AtlasPopulationMode.Dynamic;

		// Token: 0x0400171F RID: 5919
		[SerializeField]
		private bool enableMultiAtlasSupport = true;

		// Token: 0x04001720 RID: 5920
		[Space]
		[SerializeField]
		private UnityFonts unityTmpFonts;
	}
}
