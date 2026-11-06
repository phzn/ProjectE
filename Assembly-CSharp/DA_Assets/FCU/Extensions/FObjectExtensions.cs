using System;
using System.Linq;
using DA_Assets.Extensions;
using DA_Assets.FCU.Model;

namespace DA_Assets.FCU.Extensions
{
	// Token: 0x0200038F RID: 911
	public static class FObjectExtensions
	{
		// Token: 0x060017EB RID: 6123 RVA: 0x00097C12 File Offset: 0x00095E12
		public static void SetData(this FObject fobject, SyncHelper syncHelper, FigmaConverterUnity fcu)
		{
			fobject.Data.FigmaConverterUnity = fcu;
			fobject.Data.GameObject = syncHelper.gameObject;
			syncHelper.Data = fobject.Data;
		}

		// Token: 0x060017EC RID: 6124 RVA: 0x00097C40 File Offset: 0x00095E40
		public static bool IsInsideDownloadable(this FObject fobject, FigmaConverterUnity fcu, out FObject downloadableFObject)
		{
			downloadableFObject = default(FObject);
			FObject fobject2;
			if (!fcu.CurrentProject.TryGetByIndex(fobject.Data.ParentIndex, out fobject2))
			{
				return false;
			}
			if (fcu.ImageTypeSetter.DownloadableIds.Contains(fobject2.Id))
			{
				downloadableFObject = fobject;
				return true;
			}
			return fobject2.IsInsideDownloadable(fcu, out downloadableFObject);
		}

		// Token: 0x060017ED RID: 6125 RVA: 0x00097C9C File Offset: 0x00095E9C
		public static bool CanUseUnityImage(this FObject fobject, FigmaConverterUnity fcu)
		{
			bool? flag = null;
			int num = 0;
			if (fcu.IsUGUI())
			{
				if (fcu.UsingAnyProceduralImage())
				{
					if (flag == null && fcu.Settings.ImageSpritesSettings.ProceduralCondition.HasFlag(ProceduralCondition.Sprite) && fobject.IsDownloadableType())
					{
						num = 1;
						flag = new bool?(true);
					}
					if (flag == null && fcu.Settings.ImageSpritesSettings.ProceduralCondition.HasFlag(ProceduralCondition.RectangleNoRoundedCorners))
					{
						bool flag2 = fobject.Type == NodeType.RECTANGLE || fobject.Type == NodeType.FRAME;
						bool flag3 = !fobject.ContainsRoundedCorners();
						string text;
						bool flag4 = !fcu.GraphicHelpers.IsDownloadableByFills(fobject, out text).ToBoolNullFalse();
						bool flag5 = !fobject.Data.Graphic.HasStroke;
						FcuLogger.Debug(string.Format("{0} | {1} | {2} | {3} | {4} | {5}", new object[]
						{
							"CanUseUnityImage",
							fobject.Data.NameHierarchy,
							flag2,
							flag3,
							flag4,
							flag5
						}), FcuDebugSettingsFlags.LogDefault);
						num = 2;
						flag = new bool?(flag2 && flag3 && flag4 && flag5);
					}
				}
				else if (fobject.IsSvgExtension())
				{
					num = 3;
					flag = new bool?(false);
				}
				else if (fcu.UsingSvgImage())
				{
					if (flag == null && fcu.Settings.ImageSpritesSettings.SvgCondition.HasFlag(SvgCondition.ImageOrVideo) && fobject.IsAnyImageOrVideoOrEmojiTypeInChildren())
					{
						num = 4;
						flag = new bool?(true);
					}
					if (flag == null && fcu.Settings.ImageSpritesSettings.SvgCondition.HasFlag(SvgCondition.AnyEffect) && fobject.IsAnyEffectInChildren())
					{
						num = 5;
						flag = new bool?(true);
					}
				}
			}
			FcuLogger.Debug(string.Format("{0} | {1} | {2} | {3}", new object[]
			{
				"CanUseUnityImage",
				fobject.Data.NameHierarchy,
				flag,
				num
			}), FcuDebugSettingsFlags.LogDefault);
			return flag.ToBoolNullFalse();
		}
	}
}
