using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.Logging;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x02000366 RID: 870
	[Serializable]
	public class DaGoogleFontsApi : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x060016B9 RID: 5817 RVA: 0x00095141 File Offset: 0x00093341
		// (set) Token: 0x060016BA RID: 5818 RVA: 0x00095149 File Offset: 0x00093349
		[SerializeProperty("fontSubsets")]
		public FontSubset FontSubsets
		{
			get
			{
				return this.fontSubsets;
			}
			set
			{
				value |= FontSubset.Latin;
				base.SetValue<FontSubset>(ref this.fontSubsets, value);
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x060016BB RID: 5819 RVA: 0x0009515D File Offset: 0x0009335D
		public List<FontSubset> SelectedFontAssets
		{
			get
			{
				return (from FontSubset x in Enum.GetValues(this.fontSubsets.GetType())
					where this.fontSubsets.HasFlag(x)
					select x).ToList<FontSubset>();
			}
		}

		// Token: 0x060016BC RID: 5820 RVA: 0x00095190 File Offset: 0x00093390
		public async Task GetGoogleFontsBySubset(FontSubset fontSubset)
		{
			(from FontSubset x in Enum.GetValues(this.fontSubsets.GetType())
				where this.fontSubsets.HasFlag(x)
				select x).ToList<FontSubset>();
			List<FontSubset> list = new List<FontSubset>();
			foreach (object obj in Enum.GetValues(this.fontSubsets.GetType()))
			{
				FontSubset fontSubset2 = (FontSubset)obj;
				List<FontItem> list2;
				if (this.fontSubsets.HasFlag(fontSubset2) && !this.googleFontsBySubset.TryGetValue(fontSubset2, out list2))
				{
					list.Add(fontSubset2);
				}
			}
			if (list.Count != 0)
			{
				foreach (FontSubset missingSubset in list)
				{
					string text = missingSubset.ToLower();
					if (text == FontSubset.LatinExt.ToLower())
					{
						text = "latin-ext";
					}
					DALogger.Log(FcuLocKey.loading_google_fonts.Localize(new object[] { missingSubset.ToString() }));
					string text2 = string.Format("https://content-webfonts.googleapis.com/v1/webfonts?subset={0}&key={1}", text, FcuConfig.GoogleFontsApiKey);
					DARequest darequest = new DARequest
					{
						RequestType = RequestType.Get,
						Query = text2
					};
					DAResult<FontRoot> daresult = await this.monoBeh.RequestSender.SendRequest<FontRoot>(darequest);
					if (daresult.Success)
					{
						this.googleFontsBySubset.Add(missingSubset, daresult.Object.Items);
					}
					else
					{
						DALogger.LogError(string.Format("Can't get '{0}' fonts from Google repository.", missingSubset));
					}
				}
				List<FontSubset>.Enumerator enumerator2 = default(List<FontSubset>.Enumerator);
			}
		}

		// Token: 0x060016BD RID: 5821 RVA: 0x000951D4 File Offset: 0x000933D4
		public string GetUrlByWeight(FontItem fontItem, int weight, FontStyle fontStyle)
		{
			try
			{
				if (fontStyle == FontStyle.Normal)
				{
					if (weight <= 400)
					{
						if (weight <= 200)
						{
							if (weight == 100)
							{
								return fontItem.Files["100"];
							}
							if (weight == 200)
							{
								return fontItem.Files["200"];
							}
						}
						else
						{
							if (weight == 300)
							{
								return fontItem.Files["300"];
							}
							if (weight == 400)
							{
								return fontItem.Files["regular"];
							}
						}
					}
					else if (weight <= 600)
					{
						if (weight == 500)
						{
							return fontItem.Files["500"];
						}
						if (weight == 600)
						{
							return fontItem.Files["600"];
						}
					}
					else
					{
						if (weight == 700)
						{
							return fontItem.Files["700"];
						}
						if (weight == 800)
						{
							return fontItem.Files["800"];
						}
						if (weight == 900)
						{
							return fontItem.Files["900"];
						}
					}
				}
				else if (fontStyle == FontStyle.Italic)
				{
					if (weight <= 400)
					{
						if (weight <= 200)
						{
							if (weight == 100)
							{
								return fontItem.Files["100italic"];
							}
							if (weight == 200)
							{
								return fontItem.Files["200italic"];
							}
						}
						else
						{
							if (weight == 300)
							{
								return fontItem.Files["300italic"];
							}
							if (weight == 400)
							{
								return fontItem.Files["italic"];
							}
						}
					}
					else if (weight <= 600)
					{
						if (weight == 500)
						{
							return fontItem.Files["500italic"];
						}
						if (weight == 600)
						{
							return fontItem.Files["600italic"];
						}
					}
					else
					{
						if (weight == 700)
						{
							return fontItem.Files["700italic"];
						}
						if (weight == 800)
						{
							return fontItem.Files["800italic"];
						}
						if (weight == 900)
						{
							return fontItem.Files["900italic"];
						}
					}
				}
			}
			catch
			{
			}
			return null;
		}

		// Token: 0x060016BE RID: 5822 RVA: 0x000954B0 File Offset: 0x000936B0
		public FontItem GetFontItem(FontMetadata fontMetadata, FontSubset fontSubset)
		{
			try
			{
				List<FontItem> list;
				this.googleFontsBySubset.TryGetValue(fontSubset, out list);
				foreach (FontItem fontItem in list)
				{
					if (fontItem.Family.ToLower() == fontMetadata.Family.ToLower())
					{
						return fontItem;
					}
				}
			}
			catch
			{
			}
			return default(FontItem);
		}

		// Token: 0x0400187C RID: 6268
		[SerializeField]
		private FontSubset fontSubsets = FontSubset.Latin;

		// Token: 0x0400187D RID: 6269
		private Dictionary<FontSubset, List<FontItem>> googleFontsBySubset = new Dictionary<FontSubset, List<FontItem>>();
	}
}
