using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000028 RID: 40
	public static class BackgroundPropertyHelper
	{
		// Token: 0x06000193 RID: 403 RVA: 0x000047F8 File Offset: 0x000029F8
		public static BackgroundPosition ConvertScaleModeToBackgroundPosition(ScaleMode scaleMode = ScaleMode.StretchToFill)
		{
			return new BackgroundPosition(BackgroundPositionKeyword.Center);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00004810 File Offset: 0x00002A10
		public static BackgroundRepeat ConvertScaleModeToBackgroundRepeat(ScaleMode scaleMode = ScaleMode.StretchToFill)
		{
			return new BackgroundRepeat(Repeat.NoRepeat, Repeat.NoRepeat);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000482C File Offset: 0x00002A2C
		public static BackgroundSize ConvertScaleModeToBackgroundSize(ScaleMode scaleMode = ScaleMode.StretchToFill)
		{
			bool flag = scaleMode == ScaleMode.ScaleAndCrop;
			BackgroundSize backgroundSize;
			if (flag)
			{
				backgroundSize = new BackgroundSize(BackgroundSizeType.Cover);
			}
			else
			{
				bool flag2 = scaleMode == ScaleMode.ScaleToFit;
				if (flag2)
				{
					backgroundSize = new BackgroundSize(BackgroundSizeType.Contain);
				}
				else
				{
					backgroundSize = new BackgroundSize(Length.Percent(100f), Length.Percent(100f));
				}
			}
			return backgroundSize;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000487C File Offset: 0x00002A7C
		public static ScaleMode ResolveUnityBackgroundScaleMode(BackgroundPosition backgroundPositionX, BackgroundPosition backgroundPositionY, BackgroundRepeat backgroundRepeat, BackgroundSize backgroundSize, out bool valid)
		{
			bool flag = backgroundPositionX == BackgroundPropertyHelper.ConvertScaleModeToBackgroundPosition(ScaleMode.ScaleAndCrop) && backgroundPositionY == BackgroundPropertyHelper.ConvertScaleModeToBackgroundPosition(ScaleMode.ScaleAndCrop) && backgroundRepeat == BackgroundPropertyHelper.ConvertScaleModeToBackgroundRepeat(ScaleMode.ScaleAndCrop) && backgroundSize == BackgroundPropertyHelper.ConvertScaleModeToBackgroundSize(ScaleMode.ScaleAndCrop);
			ScaleMode scaleMode;
			if (flag)
			{
				valid = true;
				scaleMode = ScaleMode.ScaleAndCrop;
			}
			else
			{
				bool flag2 = backgroundPositionX == BackgroundPropertyHelper.ConvertScaleModeToBackgroundPosition(ScaleMode.ScaleToFit) && backgroundPositionY == BackgroundPropertyHelper.ConvertScaleModeToBackgroundPosition(ScaleMode.ScaleToFit) && backgroundRepeat == BackgroundPropertyHelper.ConvertScaleModeToBackgroundRepeat(ScaleMode.ScaleToFit) && backgroundSize == BackgroundPropertyHelper.ConvertScaleModeToBackgroundSize(ScaleMode.ScaleToFit);
				if (flag2)
				{
					valid = true;
					scaleMode = ScaleMode.ScaleToFit;
				}
				else
				{
					bool flag3 = backgroundPositionX == BackgroundPropertyHelper.ConvertScaleModeToBackgroundPosition(ScaleMode.StretchToFill) && backgroundPositionY == BackgroundPropertyHelper.ConvertScaleModeToBackgroundPosition(ScaleMode.StretchToFill) && backgroundRepeat == BackgroundPropertyHelper.ConvertScaleModeToBackgroundRepeat(ScaleMode.StretchToFill) && backgroundSize == BackgroundPropertyHelper.ConvertScaleModeToBackgroundSize(ScaleMode.StretchToFill);
					if (flag3)
					{
						valid = true;
						scaleMode = ScaleMode.StretchToFill;
					}
					else
					{
						valid = false;
						scaleMode = ScaleMode.StretchToFill;
					}
				}
			}
			return scaleMode;
		}
	}
}
