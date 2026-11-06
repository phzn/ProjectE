using System;
using ProjectExhibitionist.Core.Rendering;

// Token: 0x02000082 RID: 130
public static class MapGraphicsSelection
{
	// Token: 0x060005D4 RID: 1492 RVA: 0x0002AAE7 File Offset: 0x00028CE7
	public static string Catalog(GraphicsSettingsModel.MapLightingQuality lighting)
	{
		return MapGraphicsSelection.CatalogForAuthoring(true, lighting);
	}

	// Token: 0x060005D5 RID: 1493 RVA: 0x0002AAF0 File Offset: 0x00028CF0
	public static string CatalogForAuthoring(bool performance, GraphicsSettingsModel.MapLightingQuality lighting)
	{
		if (performance)
		{
			if (lighting == GraphicsSettingsModel.MapLightingQuality.Balanced)
			{
				return "MapCatalog_PerformanceBalanced";
			}
			if (lighting != GraphicsSettingsModel.MapLightingQuality.Original)
			{
				return "MapCatalog_Performance";
			}
			return "MapCatalog_PerformanceOriginal";
		}
		else
		{
			if (lighting == GraphicsSettingsModel.MapLightingQuality.Performance)
			{
				return "MapCatalog_StandardPerformance";
			}
			if (lighting != GraphicsSettingsModel.MapLightingQuality.Balanced)
			{
				return "MapCatalog";
			}
			return "MapCatalog_StandardBalanced";
		}
	}

	// Token: 0x060005D6 RID: 1494 RVA: 0x0002AB2C File Offset: 0x00028D2C
	public static bool TryDecode(string resource, out bool performance, out GraphicsSettingsModel.MapLightingQuality lighting)
	{
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				if (MapGraphicsSelection.CatalogForAuthoring(i != 0, (GraphicsSettingsModel.MapLightingQuality)j) == resource)
				{
					performance = i != 0;
					lighting = (GraphicsSettingsModel.MapLightingQuality)j;
					return true;
				}
			}
		}
		performance = false;
		lighting = GraphicsSettingsModel.MapLightingQuality.Original;
		return false;
	}

	// Token: 0x060005D7 RID: 1495 RVA: 0x0002AB78 File Offset: 0x00028D78
	public static string PendingChanges(GraphicsSettingsModel selected, string activeCatalog)
	{
		bool flag;
		GraphicsSettingsModel.MapLightingQuality mapLightingQuality;
		if (selected == null || !MapGraphicsSelection.TryDecode(activeCatalog, out flag, out mapLightingQuality))
		{
			return "";
		}
		bool flag2 = !flag;
		bool flag3 = selected.mapLighting != mapLightingQuality;
		if (flag2 && flag3)
		{
			return "Map optimization and map lighting";
		}
		if (flag2)
		{
			return "Map optimization";
		}
		if (!flag3)
		{
			return "";
		}
		return "Map lighting";
	}

	// Token: 0x0400047E RID: 1150
	public const string RestartNotice = "Map lighting applies to a new run. Return to the main menu and start a new run after changing it. Quality presets can also change this setting.";
}
