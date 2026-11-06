using System;
using UnityEngine;

namespace CompassNavigatorPro
{
	// Token: 0x020002AD RID: 685
	internal static class ShaderParams
	{
		// Token: 0x040014FB RID: 5371
		public static int MainTex = Shader.PropertyToID("_MainTex");

		// Token: 0x040014FC RID: 5372
		public static int MiniMapTex = Shader.PropertyToID("_MiniMapTex");

		// Token: 0x040014FD RID: 5373
		public static int MaskTex = Shader.PropertyToID("_MaskTex");

		// Token: 0x040014FE RID: 5374
		public static int BorderTex = Shader.PropertyToID("_BorderTex");

		// Token: 0x040014FF RID: 5375
		public static int FoWTexture = Shader.PropertyToID("_FogOfWarTex");

		// Token: 0x04001500 RID: 5376
		public static int FollowPos = Shader.PropertyToID("_FollowPos");

		// Token: 0x04001501 RID: 5377
		public static int Rotation = Shader.PropertyToID("_Rotation");

		// Token: 0x04001502 RID: 5378
		public static int ConeRotation = Shader.PropertyToID("_ConeRotation");

		// Token: 0x04001503 RID: 5379
		public static int UVOffset = Shader.PropertyToID("_UVOffset");

		// Token: 0x04001504 RID: 5380
		public static int UVFogOffset = Shader.PropertyToID("_UVFogOffset");

		// Token: 0x04001505 RID: 5381
		public static int FoWTintColor = Shader.PropertyToID("_FogOfWarTintColor");

		// Token: 0x04001506 RID: 5382
		public static int Effects = Shader.PropertyToID("_Effects");

		// Token: 0x04001507 RID: 5383
		public static int LUTTexture = Shader.PropertyToID("_LUTTex");

		// Token: 0x04001508 RID: 5384
		public static int VignetteColor = Shader.PropertyToID("_VignetteColor");

		// Token: 0x04001509 RID: 5385
		public static int RingsData = Shader.PropertyToID("_RingsData");

		// Token: 0x0400150A RID: 5386
		public static int RingsColor = Shader.PropertyToID("_RingsColor");

		// Token: 0x0400150B RID: 5387
		public static int RingsPulseData = Shader.PropertyToID("_RingsPulse");

		// Token: 0x0400150C RID: 5388
		public static int ViewConeColor = Shader.PropertyToID("_ViewConeColor");

		// Token: 0x0400150D RID: 5389
		public static int ViewConeData = Shader.PropertyToID("_ViewConeData");

		// Token: 0x0400150E RID: 5390
		public static int ViewConeOutlineColor = Shader.PropertyToID("_ViewConeOutlineColor");

		// Token: 0x0400150F RID: 5391
		public static int Color = Shader.PropertyToID("_Color");

		// Token: 0x04001510 RID: 5392
		public static int ColorBlend = Shader.PropertyToID("_ColorBlend");

		// Token: 0x04001511 RID: 5393
		public static int Angle = Shader.PropertyToID("_Angle");

		// Token: 0x04001512 RID: 5394
		public static int TintColor = Shader.PropertyToID("_TintColor");

		// Token: 0x04001513 RID: 5395
		public static int BackgroundColor = Shader.PropertyToID("_BackgroundColor");

		// Token: 0x04001514 RID: 5396
		public static int BackgroundOpaque = Shader.PropertyToID("_BackgroundOpaque");

		// Token: 0x04001515 RID: 5397
		public static int OffscreenIndicatorIconTexture = Shader.PropertyToID("_IconTex");

		// Token: 0x04001516 RID: 5398
		public static int CompassData = Shader.PropertyToID("_CompassData");

		// Token: 0x04001517 RID: 5399
		public static int CompassIP = Shader.PropertyToID("_CompassIP");

		// Token: 0x04001518 RID: 5400
		public static int CompassAngle = Shader.PropertyToID("_CompassAngle");

		// Token: 0x04001519 RID: 5401
		public static int TicksSize = Shader.PropertyToID("_TicksSize");

		// Token: 0x0400151A RID: 5402
		public static int TicksColor = Shader.PropertyToID("_TicksColor");

		// Token: 0x0400151B RID: 5403
		public static int FXData = Shader.PropertyToID("_FXData");

		// Token: 0x0400151C RID: 5404
		public static int CircleStartRadius = Shader.PropertyToID("_StartRadius");

		// Token: 0x0400151D RID: 5405
		public static int CircleInnerColor = Shader.PropertyToID("_InnerColor");

		// Token: 0x0400151E RID: 5406
		public static int ScrollOffset = Shader.PropertyToID("_ScrollOffset");

		// Token: 0x0400151F RID: 5407
		public const string SKW_COMPASS_LUT = "COMPASS_LUT";

		// Token: 0x04001520 RID: 5408
		public const string SKW_COMPASS_FOG_OF_WAR = "COMPASS_FOG_OF_WAR";

		// Token: 0x04001521 RID: 5409
		public const string SKW_COMPASS_ROTATED = "COMPASS_ROTATED";

		// Token: 0x04001522 RID: 5410
		public const string SKW_COMPASS_INDICATOR_KEEPSIZE = "COMPASS_INDICATOR_KEEPSIZE";

		// Token: 0x04001523 RID: 5411
		public const string SKW_COMPASS_RADAR = "COMPASS_RADAR";

		// Token: 0x04001524 RID: 5412
		public const string SKW_COMPASS_VIEW_CONE = "COMPASS_VIEW_CONE";

		// Token: 0x04001525 RID: 5413
		public const string SKW_COMPASS_VIEW_CONE_OUTLINE = "COMPASS_VIEW_CONE_OUTLINE";

		// Token: 0x04001526 RID: 5414
		public const string SKW_TICKS = "TICKS";

		// Token: 0x04001527 RID: 5415
		public const string SKW_TICKS_180 = "TICKS_180";

		// Token: 0x04001528 RID: 5416
		public const string SKW_TICKS_360 = "TICKS_360";

		// Token: 0x04001529 RID: 5417
		public const string SKW_SCROLLABLE = "SCROLLABLE";

		// Token: 0x0400152A RID: 5418
		public const string SKW_SCROLLABLE_180 = "SCROLLABLE_180";

		// Token: 0x0400152B RID: 5419
		public const string SKW_SCROLLABLE_360 = "SCROLLABLE_360";

		// Token: 0x0400152C RID: 5420
		public const string SKW_COMPASS_ROTATE_BORDER = "COMPASS_ROTATE_BORDER";

		// Token: 0x0400152D RID: 5421
		public static int Center = Shader.PropertyToID("_Center");

		// Token: 0x0400152E RID: 5422
		public static int Width = Shader.PropertyToID("_Width");

		// Token: 0x0400152F RID: 5423
		public static int MaxRange = Shader.PropertyToID("_MaxRange");

		// Token: 0x04001530 RID: 5424
		public static int Alpha = Shader.PropertyToID("_Alpha");

		// Token: 0x04001531 RID: 5425
		public static int GridColor = Shader.PropertyToID("_GridColor");

		// Token: 0x04001532 RID: 5426
		public static int GridData = Shader.PropertyToID("_GridData");

		// Token: 0x04001533 RID: 5427
		public static int Filled = Shader.PropertyToID("_Filled");

		// Token: 0x04001534 RID: 5428
		public static int ScanLines = Shader.PropertyToID("_ScanLines");

		// Token: 0x04001535 RID: 5429
		public static int Noise = Shader.PropertyToID("_Noise");

		// Token: 0x04001536 RID: 5430
		public static int EdgeColor = Shader.PropertyToID("_EdgeColor");

		// Token: 0x04001537 RID: 5431
		public static int EdgeData = Shader.PropertyToID("_EdgeData");

		// Token: 0x04001538 RID: 5432
		public const string SKW_SCAN_OUTLINE = "SCAN_OUTLINE";

		// Token: 0x04001539 RID: 5433
		public const string SKW_SCAN_GRID = "SCAN_GRID";
	}
}
