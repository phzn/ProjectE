using System;
using System.Collections.Generic;
using ProjectExhibitionist.Minimap;

namespace Features.Achievements
{
	// Token: 0x02001533 RID: 5427
	public static class AchievementExplorationTargets
	{
		// Token: 0x0600A810 RID: 43024 RVA: 0x00462E04 File Offset: 0x00461004
		public static string GetPoiId(MiniMapPoiKind kind)
		{
			switch (kind)
			{
			case MiniMapPoiKind.Park:
				return "Park";
			case MiniMapPoiKind.Metro:
				return "Subway";
			case MiniMapPoiKind.University:
				return "University";
			case MiniMapPoiKind.Stadium:
				return "Stadium";
			case MiniMapPoiKind.Mall:
				return "Mall";
			case MiniMapPoiKind.Gallery:
				return "ArtGallery";
			case MiniMapPoiKind.Pool:
				return "Pool";
			case MiniMapPoiKind.Kombini:
				return "Kombini";
			default:
				return null;
			}
		}

		// Token: 0x0400ACC6 RID: 44230
		public static readonly IReadOnlyList<string> PoiIds = Array.AsReadOnly<string>(new string[] { "ArtGallery", "Park", "Kombini", "Pool", "Stadium", "University", "Mall", "Subway" });
	}
}
