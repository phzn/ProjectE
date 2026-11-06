using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using DA_Assets.Logging;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x020002CB RID: 715
	[Serializable]
	public class ImageTypeSetter : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06001405 RID: 5125 RVA: 0x00080E0A File Offset: 0x0007F00A
		public ConcurrentBag<string> DownloadableIds
		{
			get
			{
				return this.downloadableIds;
			}
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x06001406 RID: 5126 RVA: 0x00080E12 File Offset: 0x0007F012
		public ConcurrentBag<string> GenerativeIds
		{
			get
			{
				return this.generativeIds;
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06001407 RID: 5127 RVA: 0x00080E1A File Offset: 0x0007F01A
		public ConcurrentBag<string> DrawableIds
		{
			get
			{
				return this.drawableIds;
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06001408 RID: 5128 RVA: 0x00080E22 File Offset: 0x0007F022
		public ConcurrentBag<string> NoneIds
		{
			get
			{
				return this.noneIds;
			}
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x00080E2A File Offset: 0x0007F02A
		public void ClearAllIds()
		{
			this.downloadableIds = new ConcurrentBag<string>();
			this.generativeIds = new ConcurrentBag<string>();
			this.drawableIds = new ConcurrentBag<string>();
			this.noneIds = new ConcurrentBag<string>();
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x00080E58 File Offset: 0x0007F058
		private void SetImageFormat(FObject fobject)
		{
			ImageFormat imageFormat;
			if (this.monoBeh.UsingSvgImage())
			{
				if (fobject.CanUseUnityImage(this.monoBeh))
				{
					imageFormat = ImageFormat.PNG;
				}
				else
				{
					imageFormat = this.monoBeh.Settings.ImageSpritesSettings.ImageFormat;
				}
			}
			else
			{
				imageFormat = this.monoBeh.Settings.ImageSpritesSettings.ImageFormat;
			}
			fobject.Data.ImageFormat = imageFormat;
		}

		// Token: 0x0600140B RID: 5131 RVA: 0x00080EC0 File Offset: 0x0007F0C0
		public async Task SetInsideDownloadableFlag(FObject fobject)
		{
			await Task.Run(delegate
			{
				this.SetInsideDownloadableFlagRecursive(fobject);
			}, this.monoBeh.GetToken(TokenType.Import));
		}

		// Token: 0x0600140C RID: 5132 RVA: 0x00080F0C File Offset: 0x0007F10C
		private void SetInsideDownloadableFlagRecursive(FObject fobject)
		{
			FObject fobject2;
			fobject.Data.InsideDownloadable = fobject.IsInsideDownloadable(this.monoBeh, out fobject2);
			FcuLogger.Debug(string.Format("IsInsideDownloadable | {0} | {1} | {2}", fobject.Data.InsideDownloadable, fobject.Data.NameHierarchy, fobject2.Name), FcuDebugSettingsFlags.LogHashGenerator);
			if (fobject.Children != null)
			{
				foreach (FObject fobject3 in fobject.Children)
				{
					this.SetInsideDownloadableFlagRecursive(fobject3);
				}
			}
		}

		// Token: 0x0600140D RID: 5133 RVA: 0x00080FB8 File Offset: 0x0007F1B8
		public async Task SetImageTypes(List<FObject> fobjects)
		{
			DALogger.Log("SetImageTypes");
			this.downloadableIds = new ConcurrentBag<string>();
			this.generativeIds = new ConcurrentBag<string>();
			this.drawableIds = new ConcurrentBag<string>();
			this.noneIds = new ConcurrentBag<string>();
			Action<FObject> <>9__1;
			await Task.Run(delegate
			{
				IEnumerable<FObject> fobjects2 = fobjects;
				Action<FObject> action;
				if ((action = <>9__1) == null)
				{
					action = (<>9__1 = delegate(FObject fobject)
					{
						if (!fobject.ContainsTag(FcuTag.Image))
						{
							return;
						}
						this.SetImageFormat(fobject);
						bool flag = this.IsDownloadable(fobject);
						bool flag2 = this.IsGenerative(fobject, flag);
						bool flag3 = this.IsDrawable(fobject);
						if (fobject.Data.ForceImage)
						{
							fobject.Data.FcuImageType = FcuImageType.Downloadable;
							this.downloadableIds.Add(fobject.Id);
						}
						else if (flag2)
						{
							fobject.Data.FcuImageType = FcuImageType.Generative;
							this.generativeIds.Add(fobject.Id);
						}
						else if (flag)
						{
							fobject.Data.FcuImageType = FcuImageType.Downloadable;
							this.downloadableIds.Add(fobject.Id);
						}
						else if (flag3)
						{
							fobject.Data.FcuImageType = FcuImageType.Drawable;
							this.drawableIds.Add(fobject.Id);
						}
						else
						{
							fobject.Data.FcuImageType = FcuImageType.None;
							this.noneIds.Add(fobject.Id);
						}
						FcuLogger.Debug(string.Format("SetImageType | {0} | {1}", fobject.Data.NameHierarchy, fobject.Data.FcuImageType), FcuDebugSettingsFlags.LogIsDownloadable);
					});
				}
				Parallel.ForEach<FObject>(fobjects2, action);
				FcuLogger.Debug(string.Format("SetImageType | {0} | {1} | {2} | {3}", new object[]
				{
					this.downloadableIds.Count,
					this.generativeIds.Count,
					this.drawableIds.Count,
					this.noneIds.Count
				}), FcuDebugSettingsFlags.LogIsDownloadable);
			}, this.monoBeh.GetToken(TokenType.Import));
		}

		// Token: 0x0600140E RID: 5134 RVA: 0x00081004 File Offset: 0x0007F204
		private bool IsDownloadable(FObject fobject)
		{
			bool? flag = null;
			string text = "unknown";
			string text2;
			if (fobject.Data.IsEmpty)
			{
				text = "IsEmpty";
				flag = new bool?(false);
			}
			else if (fobject.Data.ForceImage)
			{
				text = "ForceImage";
				flag = new bool?(true);
			}
			else if (fobject.Type == NodeType.VECTOR)
			{
				text = "VECTOR";
				flag = new bool?(true);
			}
			else if (fobject.IsMask.ToBoolNullFalse())
			{
				text = "IsMask";
				flag = new bool?(true);
			}
			else if (fobject.HaveUndownloadableTags(out text2))
			{
				text = text2;
				flag = new bool?(false);
			}
			else if (fobject.IsArcDataFilled())
			{
				text = "IsArcDataFilled";
				flag = new bool?(true);
			}
			if (flag == null)
			{
				string text3;
				bool? flag2 = this.monoBeh.GraphicHelpers.IsDownloadableByFills(fobject, out text3);
				if (flag2 != null)
				{
					text = text3;
					flag = flag2;
				}
			}
			if (flag == null && !fobject.ContainsTag(FcuTag.Shadow) && !fobject.Effects.IsEmpty<Effect>())
			{
				if (fobject.Effects.Count<Effect>((Effect x) => x.IsVisible() && x.IsShadowType()) > 0)
				{
					text = "contains shadows";
					flag = new bool?(true);
				}
			}
			if (flag == null && !fobject.ContainsTag(FcuTag.Blur) && !fobject.Effects.IsEmpty<Effect>())
			{
				if (fobject.Effects.Count<Effect>((Effect x) => x.IsVisible() && x.IsBlurType()) > 0)
				{
					text = "contains blur";
					flag = new bool?(true);
				}
			}
			fobject.Data.DownloadableReason = text;
			FcuLogger.Debug(string.Format("{0} | {1} | {2} | {3}", new object[]
			{
				"IsDownloadable",
				flag,
				fobject.Data.NameHierarchy,
				text
			}), FcuDebugSettingsFlags.LogIsDownloadable);
			return flag.ToBoolNullFalse();
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x000811FC File Offset: 0x0007F3FC
		private bool IsGenerative(FObject fobject, bool isDownloadable)
		{
			bool? flag = null;
			string text = "not generative";
			FGraphic graphic = fobject.Data.Graphic;
			Vector2Int vector2Int;
			Vector2Int vector2Int2;
			if (this.monoBeh.UsingSVG())
			{
				text = "UsingSVG";
				flag = new bool?(false);
			}
			else if (this.monoBeh.UsingAnyProceduralImage())
			{
				text = "UsingAnyProceduralImage";
				flag = new bool?(false);
			}
			else if (this.monoBeh.IsUITK())
			{
				text = "IsUITK";
				flag = new bool?(false);
			}
			else if (this.monoBeh.IsNova())
			{
				text = "IsNova";
				flag = new bool?(false);
			}
			else if (isDownloadable)
			{
				text = "isDownloadable";
				flag = new bool?(false);
			}
			else if (fobject.Data.IsOverlappedByStroke)
			{
				text = "IsOverlappedByStroke";
				flag = new bool?(false);
			}
			else if (!fobject.Size.IsSupportedRenderSize(this.monoBeh.Settings.ImageSpritesSettings.ImageScale, out vector2Int, out vector2Int2))
			{
				text = string.Format("render size is big: {0} x {1}", vector2Int, vector2Int2);
				flag = new bool?(false);
			}
			else if (!fobject.IsRectangle())
			{
				text = "not rectangle";
				flag = new bool?(false);
			}
			else if (graphic.HasFill && graphic.HasStroke)
			{
				text = "hasFill && hasStroke";
				flag = new bool?(false);
			}
			else if (graphic.HasStroke)
			{
				text = "can generate stroke only";
				flag = new bool?(true);
			}
			else if (fobject.ContainsRoundedCorners())
			{
				text = "ContainsRoundedCorners";
				flag = new bool?(true);
			}
			fobject.Data.GenerativeReason = text;
			FcuLogger.Debug(string.Format("{0} | {1} | {2} | {3}", new object[]
			{
				"IsGenerative",
				flag,
				fobject.Data.NameHierarchy,
				text
			}), FcuDebugSettingsFlags.LogIsDownloadable);
			return flag.ToBoolNullFalse();
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x000813D8 File Offset: 0x0007F5D8
		private bool IsDrawable(FObject fobject)
		{
			bool flag = true;
			string text = "drawable";
			FcuLogger.Debug(string.Format("{0} | {1} | {2} | {3}", new object[]
			{
				"IsDrawable",
				flag,
				fobject.Data.NameHierarchy,
				text
			}), FcuDebugSettingsFlags.LogIsDownloadable);
			return flag;
		}

		// Token: 0x040015A8 RID: 5544
		[SerializeField]
		private ConcurrentBag<string> downloadableIds = new ConcurrentBag<string>();

		// Token: 0x040015A9 RID: 5545
		[SerializeField]
		private ConcurrentBag<string> generativeIds = new ConcurrentBag<string>();

		// Token: 0x040015AA RID: 5546
		[SerializeField]
		private ConcurrentBag<string> drawableIds = new ConcurrentBag<string>();

		// Token: 0x040015AB RID: 5547
		[SerializeField]
		private ConcurrentBag<string> noneIds = new ConcurrentBag<string>();
	}
}
