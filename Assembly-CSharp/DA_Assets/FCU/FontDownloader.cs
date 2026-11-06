using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using DA_Assets.Logging;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x02000302 RID: 770
	[Serializable]
	public class FontDownloader : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x0600154A RID: 5450 RVA: 0x00089ED4 File Offset: 0x000880D4
		public async Task DownloadFonts(List<FObject> fobjects)
		{
			if (FcuConfig.GoogleFontsApiKey.IsEmpty())
			{
				DALogger.Log(FcuLocKey.log_no_google_fonts_api_key.Localize(Array.Empty<object>()));
			}
			else
			{
				List<FontMetadata> figmaFonts = this.GetFigmaProjectFonts(fobjects);
				this.monoBeh.FontLoader.RemoveDubAndMissingFonts();
				await this.monoBeh.FontLoader.AddToTmpMeshFontsList();
				await this.monoBeh.FontLoader.AddToTtfFontsList();
				await this.TtfDownloader.Download(figmaFonts);
				if (this.monoBeh.UsingTextMesh())
				{
					await this.TmpDownloader.CreateFonts(figmaFonts);
				}
				this.monoBeh.FontLoader.RemoveDubAndMissingFonts();
			}
		}

		// Token: 0x0600154B RID: 5451 RVA: 0x00089F20 File Offset: 0x00088120
		public UnityFonts FindUnityFonts<T>(List<FontMetadata> figmaFonts, List<T> fontArray) where T : Object
		{
			UnityFonts unityFonts = new UnityFonts
			{
				Existing = new List<FontStruct>(),
				Missing = new List<FontStruct>()
			};
			foreach (FontMetadata fontMetadata in figmaFonts)
			{
				foreach (FontSubset fontSubset in this.monoBeh.FontDownloader.GFontsApi.SelectedFontAssets)
				{
					T t = default(T);
					foreach (T t2 in fontArray)
					{
						if (fontSubset == FontSubset.Latin)
						{
							if (fontMetadata.FontNameToString(true, true, null, true) == t2.name.FormatFontName())
							{
								t = t2;
								break;
							}
						}
						else if (fontMetadata.FontNameToString(true, true, new FontSubset?(fontSubset), true) == t2.name.FormatFontName())
						{
							t = t2;
							break;
						}
					}
					if (t != null)
					{
						unityFonts.Existing.Add(new FontStruct
						{
							FontMetadata = fontMetadata,
							FontSubset = fontSubset,
							Font = t
						});
					}
					else
					{
						unityFonts.Missing.Add(new FontStruct
						{
							FontMetadata = fontMetadata,
							FontSubset = fontSubset,
							Font = t
						});
					}
				}
			}
			return unityFonts;
		}

		// Token: 0x0600154C RID: 5452 RVA: 0x0008A128 File Offset: 0x00088328
		private List<FontMetadata> GetFigmaProjectFonts(List<FObject> fobjects)
		{
			HashSet<FontMetadata> hashSet = new HashSet<FontMetadata>();
			foreach (FObject fobject in fobjects)
			{
				if (fobject.ContainsTag(FcuTag.Text))
				{
					FontMetadata fontMetadata = fobject.GetFontMetadata();
					hashSet.Add(fontMetadata);
				}
			}
			return hashSet.ToList<FontMetadata>();
		}

		// Token: 0x0600154D RID: 5453 RVA: 0x0008A198 File Offset: 0x00088398
		public Task DownloadAllProjectFonts()
		{
			FontDownloader.<DownloadAllProjectFonts>d__3 <DownloadAllProjectFonts>d__;
			<DownloadAllProjectFonts>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<DownloadAllProjectFonts>d__.<>4__this = this;
			<DownloadAllProjectFonts>d__.<>1__state = -1;
			<DownloadAllProjectFonts>d__.<>t__builder.Start<FontDownloader.<DownloadAllProjectFonts>d__3>(ref <DownloadAllProjectFonts>d__);
			return <DownloadAllProjectFonts>d__.<>t__builder.Task;
		}

		// Token: 0x0600154E RID: 5454 RVA: 0x0008A1DC File Offset: 0x000883DC
		public string GetBaseFileName(FontStruct fs)
		{
			string text;
			if (fs.FontSubset == FontSubset.Latin)
			{
				text = fs.FontMetadata.FontNameToString(true, true, null, false);
			}
			else
			{
				text = fs.FontMetadata.FontNameToString(true, true, new FontSubset?(fs.FontSubset), false);
			}
			return text;
		}

		// Token: 0x0600154F RID: 5455 RVA: 0x0008A22C File Offset: 0x0008842C
		internal void PrintFontNames(FcuLocKey locKey, List<FontError> fonts)
		{
			List<string> list = new List<string>();
			foreach (FontError fontError in fonts)
			{
				string text;
				if (fontError.FontStruct.FontSubset == FontSubset.Latin)
				{
					text = fontError.FontStruct.FontMetadata.FontNameToString(true, true, null, false);
				}
				else
				{
					text = fontError.FontStruct.FontMetadata.FontNameToString(true, true, new FontSubset?(fontError.FontStruct.FontSubset), false);
				}
				string text2;
				if (!fontError.Error.Message.IsEmpty())
				{
					text2 = text + " - " + fontError.Error.Message;
				}
				else if (!fontError.Error.Exception.IsDefault<Exception>())
				{
					text2 = string.Format("{0} - {1}", text, fontError.Error.Exception);
				}
				else
				{
					text2 = text;
				}
				list.Add(text2);
			}
			string text3 = "\n" + string.Join("\n", list);
			DALogger.Log(locKey.Localize(new object[] { list.Count, text3 }));
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06001550 RID: 5456 RVA: 0x0008A39C File Offset: 0x0008859C
		[SerializeProperty("tmpDownloader")]
		public TmpDownloader TmpDownloader
		{
			get
			{
				return this.monoBeh.Link(ref this.tmpDownloader);
			}
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06001551 RID: 5457 RVA: 0x0008A3AF File Offset: 0x000885AF
		[SerializeProperty("ttfDownloader")]
		public TtfDownloader TtfDownloader
		{
			get
			{
				return this.monoBeh.Link(ref this.ttfDownloader);
			}
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06001552 RID: 5458 RVA: 0x0008A3C2 File Offset: 0x000885C2
		[SerializeProperty("gFontsApi")]
		public DaGoogleFontsApi GFontsApi
		{
			get
			{
				return this.monoBeh.Link(ref this.gFontsApi);
			}
		}

		// Token: 0x040016EA RID: 5866
		[SerializeField]
		private TmpDownloader tmpDownloader;

		// Token: 0x040016EB RID: 5867
		[SerializeField]
		private TtfDownloader ttfDownloader;

		// Token: 0x040016EC RID: 5868
		[SerializeField]
		private DaGoogleFontsApi gFontsApi;
	}
}
