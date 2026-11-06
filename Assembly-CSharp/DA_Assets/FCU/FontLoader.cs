using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using DA_Assets.Logging;
using TMPro;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x02000306 RID: 774
	[Serializable]
	public class FontLoader : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x0600155C RID: 5468 RVA: 0x0008A908 File Offset: 0x00088B08
		// (set) Token: 0x0600155D RID: 5469 RVA: 0x0008A910 File Offset: 0x00088B10
		public string TtfFontsPath
		{
			get
			{
				return this.ttfFontsPath;
			}
			set
			{
				base.SetValue<string>(ref this.ttfFontsPath, value);
			}
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x0600155E RID: 5470 RVA: 0x0008A91F File Offset: 0x00088B1F
		// (set) Token: 0x0600155F RID: 5471 RVA: 0x0008A927 File Offset: 0x00088B27
		public string TmpFontsPath
		{
			get
			{
				return this.tmpFontsPath;
			}
			set
			{
				base.SetValue<string>(ref this.tmpFontsPath, value);
			}
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06001560 RID: 5472 RVA: 0x0008A936 File Offset: 0x00088B36
		// (set) Token: 0x06001561 RID: 5473 RVA: 0x0008A93E File Offset: 0x00088B3E
		[SerializeProperty("ttfFonts")]
		public List<Font> TtfFonts
		{
			get
			{
				return this.ttfFonts;
			}
			set
			{
				this.ttfFonts = value;
			}
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06001562 RID: 5474 RVA: 0x0008A947 File Offset: 0x00088B47
		// (set) Token: 0x06001563 RID: 5475 RVA: 0x0008A94F File Offset: 0x00088B4F
		[SerializeProperty("tmpFonts")]
		public List<TMP_FontAsset> TmpFonts
		{
			get
			{
				return this.tmpFonts;
			}
			set
			{
				this.tmpFonts = value;
			}
		}

		// Token: 0x06001564 RID: 5476 RVA: 0x0008A958 File Offset: 0x00088B58
		public void RemoveDubAndMissingFonts()
		{
			this.monoBeh.FontLoader.TtfFonts = this.monoBeh.FontLoader.TtfFonts.Where<Font>((Font x) => x != null).ToList<Font>();
			this.monoBeh.FontLoader.TtfFonts = this.monoBeh.FontLoader.TtfFonts.Distinct<Font>().ToList<Font>();
			this.monoBeh.FontLoader.TmpFonts = this.monoBeh.FontLoader.TmpFonts.Where<TMP_FontAsset>((TMP_FontAsset x) => x != null).ToList<TMP_FontAsset>();
			this.monoBeh.FontLoader.TmpFonts = this.monoBeh.FontLoader.TmpFonts.Distinct<TMP_FontAsset>().ToList<TMP_FontAsset>();
		}

		// Token: 0x06001565 RID: 5477 RVA: 0x0008AA4C File Offset: 0x00088C4C
		public T GetFontFromArray<T>(FObject fobject, List<T> fontArray) where T : Object
		{
			fobject.Data.HasFontAsset = true;
			T t = default(T);
			int num = 0;
			foreach (T t2 in fontArray)
			{
				if (!t2.IsDefault<T>() && fobject.FontNameToString(true, true, null, true) == t2.name.FormatFontName())
				{
					num = 1;
					t = t2;
					break;
				}
			}
			if (t == null)
			{
				foreach (T t3 in fontArray)
				{
					if (!t3.IsDefault<T>() && fobject.FontNameToString(true, false, null, true) == t3.name.FormatFontName())
					{
						num = 2;
						t = t3;
						break;
					}
				}
			}
			if (t == null)
			{
				fobject.Data.HasFontAsset = false;
				foreach (T t4 in fontArray)
				{
					if (!t4.IsDefault<T>())
					{
						string text = fobject.FontNameToString(false, false, null, true);
						if (t4.name.FormatFontName().Contains(text))
						{
							num = 3;
							t = t4;
							break;
						}
					}
				}
			}
			if (t == null)
			{
				fobject.Data.HasFontAsset = false;
				if (typeof(T) == typeof(Font))
				{
					num = 4;
					t = Resources.GetBuiltinResource<T>("LegacyRuntime.ttf");
				}
				else
				{
					num = 5;
					t = Resources.Load<T>("Fonts & Materials/LiberationSans SDF");
				}
			}
			FcuLogger.Debug(string.Format("FontLoader | {0} | {1} | reason: {2}", fobject.Data.NameHierarchy, t, num), FcuDebugSettingsFlags.LogDefault);
			return t;
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x0008AC70 File Offset: 0x00088E70
		public async Task AddToTtfFontsList()
		{
			DALogger.Log(FcuLocKey.log_start_adding_to_fonts_list.Localize(Array.Empty<object>()));
			await this.AddToList<Font>(this.monoBeh.FontLoader.TtfFontsPath, this.monoBeh.FontLoader.TtfFonts, delegate(int addedCount)
			{
				DALogger.Log(FcuLocKey.log_added_total.Localize(new object[]
				{
					addedCount,
					this.monoBeh.FontLoader.TtfFonts.Count<Font>()
				}));
			});
			this.RemoveDubAndMissingFonts();
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x0008ACB4 File Offset: 0x00088EB4
		public async Task AddToTmpMeshFontsList()
		{
			DALogger.Log(FcuLocKey.log_start_adding_to_fonts_list.Localize(Array.Empty<object>()));
			await this.AddToList<TMP_FontAsset>(this.monoBeh.FontLoader.TmpFontsPath, this.monoBeh.FontLoader.TmpFonts, delegate(int addedCount)
			{
				DALogger.Log(FcuLocKey.log_added_total.Localize(new object[]
				{
					addedCount,
					this.monoBeh.FontLoader.TmpFonts.Count<TMP_FontAsset>()
				}));
			});
			this.RemoveDubAndMissingFonts();
			await Task.Yield();
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x0008ACF8 File Offset: 0x00088EF8
		public async Task AddToList<T>(string fontsPath, List<T> list, Action<int> addedCount) where T : Object
		{
			List<T> loadedAssets = new List<T>();
			await this.LoadAssetFromFolder<T>(fontsPath, delegate(List<T> x)
			{
				loadedAssets = x;
			});
			int count = 0;
			await DACycles.ForEach<T>(loadedAssets, delegate(T asset)
			{
				if (!list.Contains(asset))
				{
					int count2 = count;
					count = count2 + 1;
					list.Add(asset);
				}
			}, 0.01f, 25);
			addedCount(count);
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x0008AD54 File Offset: 0x00088F54
		public async Task LoadAssetFromFolder<T>(string fontsPath, Action<List<T>> assets) where T : Object
		{
			new List<string>();
			List<T> list = new List<T>();
			assets(list);
			await Task.Yield();
		}

		// Token: 0x040016FA RID: 5882
		[SerializeField]
		private string ttfFontsPath = "Assets/Fonts/Ttf";

		// Token: 0x040016FB RID: 5883
		[SerializeField]
		private string tmpFontsPath = "Assets/Fonts/Sdf";

		// Token: 0x040016FC RID: 5884
		[SerializeField]
		private List<Font> ttfFonts = new List<Font>();

		// Token: 0x040016FD RID: 5885
		[SerializeField]
		private List<TMP_FontAsset> tmpFonts = new List<TMP_FontAsset>();
	}
}
