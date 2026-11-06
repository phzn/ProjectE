using System;
using System.Collections.Generic;
using ProjectExhibitionist.Core.Rendering;
using UnityEngine;

namespace Features.MainMenu
{
	// Token: 0x0200103C RID: 4156
	public static class GameSettingsUiSchema
	{
		// Token: 0x06007E28 RID: 32296 RVA: 0x00339634 File Offset: 0x00337834
		public static void BuildResolutionOptions(List<Vector2Int> resolutions, List<string> labels = null)
		{
			if (resolutions == null)
			{
				return;
			}
			resolutions.Clear();
			if (labels != null)
			{
				labels.Clear();
			}
			foreach (Resolution resolution in Screen.resolutions)
			{
				Vector2Int vector2Int = new Vector2Int(resolution.width, resolution.height);
				if (!resolutions.Contains(vector2Int))
				{
					resolutions.Add(vector2Int);
					if (labels != null)
					{
						labels.Add(string.Format("{0} x {1}", vector2Int.x, vector2Int.y));
					}
				}
			}
			if (resolutions.Count > 0)
			{
				return;
			}
			Vector2Int vector2Int2 = new Vector2Int(Screen.width, Screen.height);
			resolutions.Add(vector2Int2);
			if (labels != null)
			{
				labels.Add(string.Format("{0} x {1}", vector2Int2.x, vector2Int2.y));
			}
		}

		// Token: 0x06007E29 RID: 32297 RVA: 0x00339710 File Offset: 0x00337910
		public static int FindResolutionIndex(IReadOnlyList<Vector2Int> resolutions, int width, int height)
		{
			if (resolutions == null || resolutions.Count == 0)
			{
				return 0;
			}
			for (int i = 0; i < resolutions.Count; i++)
			{
				if (resolutions[i].x == width && resolutions[i].y == height)
				{
					return i;
				}
			}
			return resolutions.Count - 1;
		}

		// Token: 0x06007E2A RID: 32298 RVA: 0x0033976C File Offset: 0x0033796C
		public static int FindFpsIndex(int fps)
		{
			if (fps <= 0)
			{
				return 0;
			}
			int num = 1;
			int num2 = int.MaxValue;
			for (int i = 1; i < GameSettingsUiSchema.FpsValues.Length; i++)
			{
				int num3 = Mathf.Abs(GameSettingsUiSchema.FpsValues[i] - fps);
				if (num3 < num2)
				{
					num = i;
					num2 = num3;
				}
			}
			return num;
		}

		// Token: 0x06007E2B RID: 32299 RVA: 0x003397B4 File Offset: 0x003379B4
		public static int FindClosest(IReadOnlyList<int> values, int target)
		{
			if (values == null || values.Count == 0)
			{
				return 0;
			}
			int num = 0;
			int num2 = int.MaxValue;
			for (int i = 0; i < values.Count; i++)
			{
				int num3 = Mathf.Abs(values[i] - target);
				if (num3 < num2)
				{
					num = i;
					num2 = num3;
				}
			}
			return num;
		}

		// Token: 0x06007E2C RID: 32300 RVA: 0x00339800 File Offset: 0x00337A00
		public static int FindClosest(IReadOnlyList<float> values, float target)
		{
			if (values == null || values.Count == 0)
			{
				return 0;
			}
			int num = 0;
			float num2 = float.MaxValue;
			for (int i = 0; i < values.Count; i++)
			{
				float num3 = Mathf.Abs(values[i] - target);
				if (num3 < num2)
				{
					num = i;
					num2 = num3;
				}
			}
			return num;
		}

		// Token: 0x06007E2D RID: 32301 RVA: 0x0033984B File Offset: 0x00337A4B
		public static int ShadowQualityIndex(GraphicsSettingsModel model)
		{
			if (model == null || !model.shadowsEnabled)
			{
				return 0;
			}
			if (model.softShadows || model.mainShadowResolution >= 4096)
			{
				return 3;
			}
			if (model.shadowDistance > 40f)
			{
				return 2;
			}
			return 1;
		}

		// Token: 0x06007E2E RID: 32302 RVA: 0x00339884 File Offset: 0x00337A84
		public static void ApplyShadowQuality(GraphicsSettingsModel model, int index)
		{
			if (model == null)
			{
				return;
			}
			int num = Mathf.Clamp(index, 0, GameSettingsUiSchema.ShadowQualityLabels.Length - 1);
			model.shadowsEnabled = num > 0;
			model.softShadows = num >= 3;
			model.shadowDistance = GameSettingsUiSchema.ShadowDistances[num];
			model.shadowCascades = GameSettingsUiSchema.ShadowCascades[num];
			model.mainShadowResolution = GameSettingsUiSchema.ShadowResolutions[num];
		}

		// Token: 0x06007E2F RID: 32303 RVA: 0x003398E4 File Offset: 0x00337AE4
		public static int ModeToIndex(FullScreenMode mode)
		{
			if (mode == FullScreenMode.ExclusiveFullScreen)
			{
				return 0;
			}
			if (mode == FullScreenMode.Windowed)
			{
				return 2;
			}
			return 1;
		}

		// Token: 0x06007E30 RID: 32304 RVA: 0x003398F2 File Offset: 0x00337AF2
		public static FullScreenMode IndexToMode(int index)
		{
			if (index == 0)
			{
				return FullScreenMode.ExclusiveFullScreen;
			}
			if (index == 2)
			{
				return FullScreenMode.Windowed;
			}
			return FullScreenMode.FullScreenWindow;
		}

		// Token: 0x040081BD RID: 33213
		public static readonly string[] QualityLabels = new string[] { "LOW", "MEDIUM", "HIGH", "ULTRA", "CUSTOM" };

		// Token: 0x040081BE RID: 33214
		public static readonly string[] MapLightingLabels = new string[] { "PERFORMANCE", "BALANCED", "ORIGINAL" };

		// Token: 0x040081BF RID: 33215
		public static readonly string[] WindowModeLabels = new string[] { "FULLSCREEN", "BORDERLESS", "WINDOWED" };

		// Token: 0x040081C0 RID: 33216
		public static readonly int[] FpsValues = new int[] { 0, 30, 60, 90, 120, 144 };

		// Token: 0x040081C1 RID: 33217
		public static readonly string[] FpsLabels = new string[] { "UNLIMITED", "30", "60", "90", "120", "144" };

		// Token: 0x040081C2 RID: 33218
		public static readonly float[] RenderScales = new float[] { 0.5f, 0.67f, 0.75f, 0.85f, 1f, 1.25f, 1.5f };

		// Token: 0x040081C3 RID: 33219
		public static readonly string[] RenderScaleLabels = new string[] { "50%", "67%", "75%", "85%", "100%", "125%", "150%" };

		// Token: 0x040081C4 RID: 33220
		public static readonly string[] ShadowQualityLabels = new string[] { "OFF", "LOW", "MEDIUM", "HIGH" };

		// Token: 0x040081C5 RID: 33221
		public static readonly float[] ViewDistanceScales = new float[] { 0.45f, 0.7f, 1f, 1.35f, 1.8f };

		// Token: 0x040081C6 RID: 33222
		public static readonly string[] ViewDistanceLabels = new string[] { "VERY LOW", "LOW", "MEDIUM", "HIGH", "EPIC" };

		// Token: 0x040081C7 RID: 33223
		public static readonly string[] TextureQualityLabels = new string[] { "LOW", "MEDIUM", "HIGH", "ULTRA" };

		// Token: 0x040081C8 RID: 33224
		public static readonly string[] ColorVisionLabels = new string[] { "NORMAL", "PROTANOPIA", "DEUTERANOPIA", "TRITANOPIA" };

		// Token: 0x040081C9 RID: 33225
		public static readonly int[] PhoneVideoHeights = new int[] { 720, 1080, 1440 };

		// Token: 0x040081CA RID: 33226
		public static readonly string[] PhoneVideoResolutionLabels = new string[] { "1280 x 720", "1920 x 1080", "2560 x 1440" };

		// Token: 0x040081CB RID: 33227
		public static readonly int[] PhoneVideoFpsValues = new int[] { 24, 30, 60 };

		// Token: 0x040081CC RID: 33228
		public static readonly string[] PhoneVideoFpsLabels = new string[] { "24", "30", "60" };

		// Token: 0x040081CD RID: 33229
		public static readonly string[] BodyPhysicsLabels = new string[] { "ANIME", "CLASSIC" };

		// Token: 0x040081CE RID: 33230
		private static readonly float[] ShadowDistances = new float[] { 20f, 30f, 65f, 120f };

		// Token: 0x040081CF RID: 33231
		private static readonly int[] ShadowCascades = new int[] { 1, 1, 2, 3 };

		// Token: 0x040081D0 RID: 33232
		private static readonly int[] ShadowResolutions = new int[] { 512, 1024, 2048, 4096 };
	}
}
