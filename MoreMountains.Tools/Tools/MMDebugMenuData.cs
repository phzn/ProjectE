using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000039 RID: 57
	[CreateAssetMenu(fileName = "MMDebugMenuData", menuName = "MoreMountains/MMDebugMenu/MMDebugMenuData")]
	public class MMDebugMenuData : ScriptableObject
	{
		// Token: 0x04000177 RID: 375
		[Header("Prefabs")]
		public MMDebugMenuItemTitle TitlePrefab;

		// Token: 0x04000178 RID: 376
		public MMDebugMenuItemButton ButtonPrefab;

		// Token: 0x04000179 RID: 377
		public MMDebugMenuItemButton ButtonBorderPrefab;

		// Token: 0x0400017A RID: 378
		public MMDebugMenuItemCheckbox CheckboxPrefab;

		// Token: 0x0400017B RID: 379
		public MMDebugMenuItemSlider SliderPrefab;

		// Token: 0x0400017C RID: 380
		public GameObject SpacerSmallPrefab;

		// Token: 0x0400017D RID: 381
		public GameObject SpacerBigPrefab;

		// Token: 0x0400017E RID: 382
		public MMDebugMenuItemText TextTinyPrefab;

		// Token: 0x0400017F RID: 383
		public MMDebugMenuItemText TextSmallPrefab;

		// Token: 0x04000180 RID: 384
		public MMDebugMenuItemText TextLongPrefab;

		// Token: 0x04000181 RID: 385
		public MMDebugMenuItemValue ValuePrefab;

		// Token: 0x04000182 RID: 386
		public MMDebugMenuItemChoices TwoChoicesPrefab;

		// Token: 0x04000183 RID: 387
		public MMDebugMenuItemChoices ThreeChoicesPrefab;

		// Token: 0x04000184 RID: 388
		public MMDebugMenuTab TabPrefab;

		// Token: 0x04000185 RID: 389
		public MMDebugMenuTabContents TabContentsPrefab;

		// Token: 0x04000186 RID: 390
		public RectTransform TabSpacerPrefab;

		// Token: 0x04000187 RID: 391
		public MMDebugMenuDebugTab DebugTabPrefab;

		// Token: 0x04000188 RID: 392
		public string DebugTabName = "Logs";

		// Token: 0x04000189 RID: 393
		[Header("Tabs")]
		public List<MMDebugMenuTabData> Tabs;

		// Token: 0x0400018A RID: 394
		public bool DisplayDebugTab = true;

		// Token: 0x0400018B RID: 395
		public int MaxTabs = 5;

		// Token: 0x0400018C RID: 396
		public int InitialActiveTabIndex;

		// Token: 0x0400018D RID: 397
		[Header("Toggle")]
		public MMDebugMenu.ToggleDirections ToggleDirection = MMDebugMenu.ToggleDirections.RightToLeft;

		// Token: 0x0400018E RID: 398
		public float ToggleDuration = 0.2f;

		// Token: 0x0400018F RID: 399
		public MMTween.MMTweenCurve ToggleCurve = MMTween.MMTweenCurve.EaseInCubic;

		// Token: 0x04000190 RID: 400
		public KeyCode ToggleShortcut = KeyCode.Quote;

		// Token: 0x04000191 RID: 401
		[Header("Style")]
		public Font RegularFont;

		// Token: 0x04000192 RID: 402
		public Font BoldFont;

		// Token: 0x04000193 RID: 403
		public Color BackgroundColor = Color.black;

		// Token: 0x04000194 RID: 404
		public Color AccentColor = MMColors.ReunoYellow;

		// Token: 0x04000195 RID: 405
		public Color TextColor = Color.white;
	}
}
