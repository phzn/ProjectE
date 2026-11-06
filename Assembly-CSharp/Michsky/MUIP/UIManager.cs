using System;
using TMPro;
using UnityEngine;

namespace Michsky.MUIP
{
	// Token: 0x0200024D RID: 589
	[CreateAssetMenu(fileName = "New UI Manager", menuName = "Modern UI Pack/New UI Manager")]
	public class UIManager : ScriptableObject
	{
		// Token: 0x040010F2 RID: 4338
		[HideInInspector]
		public bool enableDynamicUpdate = true;

		// Token: 0x040010F3 RID: 4339
		[HideInInspector]
		public bool enableExtendedColorPicker = true;

		// Token: 0x040010F4 RID: 4340
		[HideInInspector]
		public bool editorHints = true;

		// Token: 0x040010F5 RID: 4341
		public Color animatedIconColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x040010F6 RID: 4342
		public Color contextBackgroundColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x040010F7 RID: 4343
		public TMP_FontAsset buttonFont;

		// Token: 0x040010F8 RID: 4344
		public Color buttonNormalColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x040010F9 RID: 4345
		public Color buttonAccentColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x040010FA RID: 4346
		[Range(0f, 1f)]
		public float buttonDisabledAlpha = 0.4f;

		// Token: 0x040010FB RID: 4347
		public TMP_FontAsset dropdownFont;

		// Token: 0x040010FC RID: 4348
		public TMP_FontAsset dropdownItemFont;

		// Token: 0x040010FD RID: 4349
		public Color dropdownBackgroundColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x040010FE RID: 4350
		public Color dropdownContentBackgroundColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x040010FF RID: 4351
		public Color dropdownPrimaryColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x04001100 RID: 4352
		public Color dropdownItemBackgroundColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x04001101 RID: 4353
		public Color dropdownItemPrimaryColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x04001102 RID: 4354
		public TMP_FontAsset selectorFont;

		// Token: 0x04001103 RID: 4355
		public Color selectorColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x04001104 RID: 4356
		public Color selectorHighlightedColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x04001105 RID: 4357
		public TMP_FontAsset inputFieldFont;

		// Token: 0x04001106 RID: 4358
		public Color inputFieldColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x04001107 RID: 4359
		public TMP_FontAsset modalWindowTitleFont;

		// Token: 0x04001108 RID: 4360
		public TMP_FontAsset modalWindowContentFont;

		// Token: 0x04001109 RID: 4361
		public Color modalWindowTitleColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x0400110A RID: 4362
		public Color modalWindowDescriptionColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x0400110B RID: 4363
		public Color modalWindowIconColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x0400110C RID: 4364
		public Color modalWindowBackgroundColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x0400110D RID: 4365
		public Color modalWindowContentPanelColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x0400110E RID: 4366
		public TMP_FontAsset notificationTitleFont;

		// Token: 0x0400110F RID: 4367
		public float notificationTitleFontSize = 22.5f;

		// Token: 0x04001110 RID: 4368
		public TMP_FontAsset notificationDescriptionFont;

		// Token: 0x04001111 RID: 4369
		public float notificationDescriptionFontSize = 18f;

		// Token: 0x04001112 RID: 4370
		public UIManager.NotificationThemeType notificationThemeType;

		// Token: 0x04001113 RID: 4371
		public Color notificationBackgroundColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x04001114 RID: 4372
		public Color notificationTitleColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x04001115 RID: 4373
		public Color notificationDescriptionColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x04001116 RID: 4374
		public Color notificationIconColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x04001117 RID: 4375
		public TMP_FontAsset progressBarLabelFont;

		// Token: 0x04001118 RID: 4376
		public float progressBarLabelFontSize = 25f;

		// Token: 0x04001119 RID: 4377
		public Color progressBarColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x0400111A RID: 4378
		public Color progressBarBackgroundColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x0400111B RID: 4379
		public Color progressBarLoopBackgroundColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x0400111C RID: 4380
		public Color progressBarLabelColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x0400111D RID: 4381
		public Color scrollbarColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x0400111E RID: 4382
		public Color scrollbarBackgroundColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x0400111F RID: 4383
		public TMP_FontAsset sliderLabelFont;

		// Token: 0x04001120 RID: 4384
		public UIManager.SliderThemeType sliderThemeType;

		// Token: 0x04001121 RID: 4385
		public Color sliderColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x04001122 RID: 4386
		public Color sliderBackgroundColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x04001123 RID: 4387
		public Color sliderLabelColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x04001124 RID: 4388
		public Color sliderPopupLabelColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x04001125 RID: 4389
		public Color sliderHandleColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x04001126 RID: 4390
		public Color switchBorderColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x04001127 RID: 4391
		public Color switchBackgroundColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x04001128 RID: 4392
		public Color switchHandleOnColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x04001129 RID: 4393
		public Color switchHandleOffColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x0400112A RID: 4394
		public TMP_FontAsset toggleFont;

		// Token: 0x0400112B RID: 4395
		public UIManager.ToggleThemeType toggleThemeType;

		// Token: 0x0400112C RID: 4396
		public Color toggleTextColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x0400112D RID: 4397
		public Color toggleBorderColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x0400112E RID: 4398
		public Color toggleBackgroundColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x0400112F RID: 4399
		public Color toggleCheckColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x04001130 RID: 4400
		public TMP_FontAsset tooltipFont;

		// Token: 0x04001131 RID: 4401
		public float tooltipFontSize = 22f;

		// Token: 0x04001132 RID: 4402
		public Color tooltipTextColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x04001133 RID: 4403
		public Color tooltipBackgroundColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x04001134 RID: 4404
		public TMP_FontAsset customObjPrimaryFont;

		// Token: 0x04001135 RID: 4405
		public TMP_FontAsset customObjSecondaryFont;

		// Token: 0x04001136 RID: 4406
		public Color customObjPrimaryColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x04001137 RID: 4407
		public Color customObjSecondaryColor = new Color(255f, 255f, 255f, 255f);

		// Token: 0x0200024E RID: 590
		public enum ModalWindowThemeType
		{
			// Token: 0x04001139 RID: 4409
			Basic,
			// Token: 0x0400113A RID: 4410
			Custom
		}

		// Token: 0x0200024F RID: 591
		public enum NotificationThemeType
		{
			// Token: 0x0400113C RID: 4412
			Basic,
			// Token: 0x0400113D RID: 4413
			Custom
		}

		// Token: 0x02000250 RID: 592
		public enum SliderThemeType
		{
			// Token: 0x0400113F RID: 4415
			Basic,
			// Token: 0x04001140 RID: 4416
			Custom
		}

		// Token: 0x02000251 RID: 593
		public enum ToggleThemeType
		{
			// Token: 0x04001142 RID: 4418
			Basic,
			// Token: 0x04001143 RID: 4419
			Custom
		}
	}
}
