using System;
using System.Collections.Generic;
using ProjectExhibitionist.Minimap;
using UnityEngine;

namespace Features.CielOS.Apps.Maps
{
	// Token: 0x02001463 RID: 5219
	public static class CielOSCityMapLayout
	{
		// Token: 0x0600A112 RID: 41234 RVA: 0x0042B5DF File Offset: 0x004297DF
		public static IReadOnlyList<CielOSCityMapLayout.Landmark> GetLandmarks(int variant)
		{
			return CielOSCityMapLayout.Variants[Mathf.Clamp(variant, 0, 4)];
		}

		// Token: 0x0600A113 RID: 41235 RVA: 0x0042B5F0 File Offset: 0x004297F0
		public static bool TryGetLandmark(int variant, MiniMapPoiKind kind, out CielOSCityMapLayout.Landmark landmark)
		{
			IReadOnlyList<CielOSCityMapLayout.Landmark> landmarks = CielOSCityMapLayout.GetLandmarks(variant);
			for (int i = 0; i < landmarks.Count; i++)
			{
				if (landmarks[i].Kind == kind)
				{
					landmark = landmarks[i];
					return true;
				}
			}
			landmark = default(CielOSCityMapLayout.Landmark);
			return false;
		}

		// Token: 0x0600A114 RID: 41236 RVA: 0x0042B63C File Offset: 0x0042983C
		public static bool IsLandmarkCell(int variant, int x, int z)
		{
			IReadOnlyList<CielOSCityMapLayout.Landmark> landmarks = CielOSCityMapLayout.GetLandmarks(variant);
			for (int i = 0; i < landmarks.Count; i++)
			{
				if (landmarks[i].ContainsCell(x, z))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600A115 RID: 41237 RVA: 0x0042B678 File Offset: 0x00429878
		public static string ResourcePath(int variant)
		{
			int num = Mathf.Clamp(variant, 0, 4) + 1;
			return string.Format("CielOSMaps/CityMap_{0:00}", num);
		}

		// Token: 0x0600A116 RID: 41238 RVA: 0x0042B6A0 File Offset: 0x004298A0
		public static Vector3 CellCenter(int x, int z)
		{
			return new Vector3((float)x * 60f - 330f, 0f, (float)z * 60f - 210f);
		}

		// Token: 0x0600A117 RID: 41239 RVA: 0x0042B6C8 File Offset: 0x004298C8
		private static Vector3 CellRegionCenter(Vector2Int origin, Vector2Int size)
		{
			return new Vector3(((float)origin.x + (float)(size.x - 1) * 0.5f) * 60f - 330f, 0f, ((float)origin.y + (float)(size.y - 1) * 0.5f) * 60f - 210f);
		}

		// Token: 0x0400A4D4 RID: 42196
		public const int Columns = 12;

		// Token: 0x0400A4D5 RID: 42197
		public const int Rows = 8;

		// Token: 0x0400A4D6 RID: 42198
		public const float CellSize = 60f;

		// Token: 0x0400A4D7 RID: 42199
		public const int VariantCount = 5;

		// Token: 0x0400A4D8 RID: 42200
		public static readonly Vector2 WorldSize = new Vector2(720f, 480f);

		// Token: 0x0400A4D9 RID: 42201
		private static readonly CielOSCityMapLayout.Landmark[][] Variants = new CielOSCityMapLayout.Landmark[][]
		{
			new CielOSCityMapLayout.Landmark[]
			{
				new CielOSCityMapLayout.Landmark(MiniMapPoiKind.University, 0, 5, 3, 3),
				new CielOSCityMapLayout.Landmark(MiniMapPoiKind.Stadium, 8, 4, 4, 4),
				new CielOSCityMapLayout.Landmark(MiniMapPoiKind.Mall, 0, 0, 3, 3),
				new CielOSCityMapLayout.Landmark(MiniMapPoiKind.Gallery, 6, 3, 1, 1),
				new CielOSCityMapLayout.Landmark(MiniMapPoiKind.Pool, 9, 0, 1, 1)
			},
			new CielOSCityMapLayout.Landmark[]
			{
				new CielOSCityMapLayout.Landmark(MiniMapPoiKind.University, 0, 0, 3, 3),
				new CielOSCityMapLayout.Landmark(MiniMapPoiKind.Stadium, 8, 0, 4, 4),
				new CielOSCityMapLayout.Landmark(MiniMapPoiKind.Mall, 0, 5, 3, 3),
				new CielOSCityMapLayout.Landmark(MiniMapPoiKind.Gallery, 5, 5, 1, 1),
				new CielOSCityMapLayout.Landmark(MiniMapPoiKind.Pool, 10, 6, 1, 1)
			},
			new CielOSCityMapLayout.Landmark[]
			{
				new CielOSCityMapLayout.Landmark(MiniMapPoiKind.University, 4, 5, 3, 3),
				new CielOSCityMapLayout.Landmark(MiniMapPoiKind.Stadium, 8, 0, 4, 4),
				new CielOSCityMapLayout.Landmark(MiniMapPoiKind.Mall, 0, 0, 3, 3),
				new CielOSCityMapLayout.Landmark(MiniMapPoiKind.Gallery, 1, 5, 1, 1),
				new CielOSCityMapLayout.Landmark(MiniMapPoiKind.Pool, 5, 2, 1, 1)
			},
			new CielOSCityMapLayout.Landmark[]
			{
				new CielOSCityMapLayout.Landmark(MiniMapPoiKind.University, 0, 5, 3, 3),
				new CielOSCityMapLayout.Landmark(MiniMapPoiKind.Stadium, 0, 0, 4, 4),
				new CielOSCityMapLayout.Landmark(MiniMapPoiKind.Mall, 9, 5, 3, 3),
				new CielOSCityMapLayout.Landmark(MiniMapPoiKind.Gallery, 6, 3, 1, 1),
				new CielOSCityMapLayout.Landmark(MiniMapPoiKind.Pool, 9, 1, 1, 1)
			},
			new CielOSCityMapLayout.Landmark[]
			{
				new CielOSCityMapLayout.Landmark(MiniMapPoiKind.University, 9, 0, 3, 3),
				new CielOSCityMapLayout.Landmark(MiniMapPoiKind.Stadium, 0, 4, 4, 4),
				new CielOSCityMapLayout.Landmark(MiniMapPoiKind.Mall, 9, 5, 3, 3),
				new CielOSCityMapLayout.Landmark(MiniMapPoiKind.Gallery, 5, 3, 1, 1),
				new CielOSCityMapLayout.Landmark(MiniMapPoiKind.Pool, 6, 0, 1, 1)
			}
		};

		// Token: 0x02001464 RID: 5220
		public readonly struct Landmark
		{
			// Token: 0x0600A119 RID: 41241 RVA: 0x0042B934 File Offset: 0x00429B34
			public Landmark(MiniMapPoiKind kind, int x, int z, int width, int height)
			{
				this.Kind = kind;
				this.OriginCell = new Vector2Int(x, z);
				this.GridSize = new Vector2Int(width, height);
			}

			// Token: 0x1700190A RID: 6410
			// (get) Token: 0x0600A11A RID: 41242 RVA: 0x0042B959 File Offset: 0x00429B59
			public Vector3 WorldCenter
			{
				get
				{
					return CielOSCityMapLayout.CellRegionCenter(this.OriginCell, this.GridSize);
				}
			}

			// Token: 0x1700190B RID: 6411
			// (get) Token: 0x0600A11B RID: 41243 RVA: 0x0042B96C File Offset: 0x00429B6C
			public Vector2 NormalizedCenter
			{
				get
				{
					return new Vector2(((float)this.OriginCell.x + (float)this.GridSize.x * 0.5f) / 12f, ((float)this.OriginCell.y + (float)this.GridSize.y * 0.5f) / 8f);
				}
			}

			// Token: 0x0600A11C RID: 41244 RVA: 0x0042B9D4 File Offset: 0x00429BD4
			public bool ContainsCell(int x, int z)
			{
				return x >= this.OriginCell.x && x < this.OriginCell.x + this.GridSize.x && z >= this.OriginCell.y && z < this.OriginCell.y + this.GridSize.y;
			}

			// Token: 0x0400A4DA RID: 42202
			public readonly MiniMapPoiKind Kind;

			// Token: 0x0400A4DB RID: 42203
			public readonly Vector2Int OriginCell;

			// Token: 0x0400A4DC RID: 42204
			public readonly Vector2Int GridSize;
		}
	}
}
