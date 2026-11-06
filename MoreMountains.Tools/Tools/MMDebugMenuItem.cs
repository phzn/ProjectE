using System;

namespace MoreMountains.Tools
{
	// Token: 0x02000038 RID: 56
	[Serializable]
	public class MMDebugMenuItem
	{
		// Token: 0x04000156 RID: 342
		public string Name;

		// Token: 0x04000157 RID: 343
		public bool Active = true;

		// Token: 0x04000158 RID: 344
		public MMDebugMenuItem.MMDebugMenuItemTypes Type;

		// Token: 0x04000159 RID: 345
		[MMEnumCondition("Type", new int[] { 0 })]
		public string TitleText = "Title text";

		// Token: 0x0400015A RID: 346
		[MMEnumCondition("Type", new int[] { 5 })]
		public MMDebugMenuItem.MMDebugMenuItemTextTypes TextType;

		// Token: 0x0400015B RID: 347
		[MMEnumCondition("Type", new int[] { 5 })]
		public string TextContents = "Lorem ipsum dolor sit amet";

		// Token: 0x0400015C RID: 348
		[MMEnumCondition("Type", new int[] { 7 })]
		public MMDebugMenuItem.MMDebugMenuItemChoicesTypes ChoicesType;

		// Token: 0x0400015D RID: 349
		[MMEnumCondition("Type", new int[] { 7 })]
		public string ChoiceOneText;

		// Token: 0x0400015E RID: 350
		[MMEnumCondition("Type", new int[] { 7 })]
		public string ChoiceOneEventName = "ChoiceOneEvent";

		// Token: 0x0400015F RID: 351
		[MMEnumCondition("Type", new int[] { 7 })]
		public string ChoiceTwoText;

		// Token: 0x04000160 RID: 352
		[MMEnumCondition("Type", new int[] { 7 })]
		public string ChoiceTwoEventName = "ChoiceTwoEvent";

		// Token: 0x04000161 RID: 353
		[MMEnumCondition("Type", new int[] { 7 })]
		public string ChoiceThreeText;

		// Token: 0x04000162 RID: 354
		[MMEnumCondition("Type", new int[] { 7 })]
		public string ChoiceThreeEventName = "ChoiceThreeEvent";

		// Token: 0x04000163 RID: 355
		[MMEnumCondition("Type", new int[] { 7 })]
		public int SelectedChoice;

		// Token: 0x04000164 RID: 356
		[MMEnumCondition("Type", new int[] { 6 })]
		public string ValueLabel = "Value Label";

		// Token: 0x04000165 RID: 357
		[MMEnumCondition("Type", new int[] { 6 })]
		public string ValueInitialValue = "255";

		// Token: 0x04000166 RID: 358
		[MMEnumCondition("Type", new int[] { 6 })]
		public int ValueMMRadioReceiverChannel;

		// Token: 0x04000167 RID: 359
		[MMEnumCondition("Type", new int[] { 2 })]
		public string ButtonText = "Button text";

		// Token: 0x04000168 RID: 360
		[MMEnumCondition("Type", new int[] { 2 })]
		public MMDebugMenuItem.MMDebugMenuItemButtonTypes ButtonType;

		// Token: 0x04000169 RID: 361
		[MMEnumCondition("Type", new int[] { 2 })]
		public string ButtonEventName = "Button";

		// Token: 0x0400016A RID: 362
		[MMEnumCondition("Type", new int[] { 1 })]
		public MMDebugMenuItem.MMDebugMenuItemSpacerTypes SpacerType;

		// Token: 0x0400016B RID: 363
		[MMEnumCondition("Type", new int[] { 3 })]
		public string CheckboxText;

		// Token: 0x0400016C RID: 364
		[MMEnumCondition("Type", new int[] { 3 })]
		public bool CheckboxInitialState;

		// Token: 0x0400016D RID: 365
		[MMEnumCondition("Type", new int[] { 3 })]
		public string CheckboxEventName = "CheckboxEventName";

		// Token: 0x0400016E RID: 366
		[MMEnumCondition("Type", new int[] { 4 })]
		public MMDebugMenuItemSlider.Modes SliderMode;

		// Token: 0x0400016F RID: 367
		[MMEnumCondition("Type", new int[] { 4 })]
		public string SliderText;

		// Token: 0x04000170 RID: 368
		[MMEnumCondition("Type", new int[] { 4 })]
		public float SliderRemapZero;

		// Token: 0x04000171 RID: 369
		[MMEnumCondition("Type", new int[] { 4 })]
		public float SliderRemapOne = 1f;

		// Token: 0x04000172 RID: 370
		[MMEnumCondition("Type", new int[] { 4 })]
		public float SliderInitialValue;

		// Token: 0x04000173 RID: 371
		[MMEnumCondition("Type", new int[] { 4 })]
		public string SliderEventName = "Slider";

		// Token: 0x04000174 RID: 372
		[MMHidden]
		public MMDebugMenuItemSlider TargetSlider;

		// Token: 0x04000175 RID: 373
		[MMHidden]
		public MMDebugMenuItemButton TargetButton;

		// Token: 0x04000176 RID: 374
		[MMHidden]
		public MMDebugMenuItemCheckbox TargetCheckbox;

		// Token: 0x020003B2 RID: 946
		public enum MMDebugMenuItemTypes
		{
			// Token: 0x04001D88 RID: 7560
			Title,
			// Token: 0x04001D89 RID: 7561
			Spacer,
			// Token: 0x04001D8A RID: 7562
			Button,
			// Token: 0x04001D8B RID: 7563
			Checkbox,
			// Token: 0x04001D8C RID: 7564
			Slider,
			// Token: 0x04001D8D RID: 7565
			Text,
			// Token: 0x04001D8E RID: 7566
			Value,
			// Token: 0x04001D8F RID: 7567
			Choices
		}

		// Token: 0x020003B3 RID: 947
		public enum MMDebugMenuItemTextTypes
		{
			// Token: 0x04001D91 RID: 7569
			Tiny,
			// Token: 0x04001D92 RID: 7570
			Small,
			// Token: 0x04001D93 RID: 7571
			Long
		}

		// Token: 0x020003B4 RID: 948
		public enum MMDebugMenuItemChoicesTypes
		{
			// Token: 0x04001D95 RID: 7573
			TwoChoices,
			// Token: 0x04001D96 RID: 7574
			ThreeChoices
		}

		// Token: 0x020003B5 RID: 949
		public enum MMDebugMenuItemButtonTypes
		{
			// Token: 0x04001D98 RID: 7576
			Border,
			// Token: 0x04001D99 RID: 7577
			Full
		}

		// Token: 0x020003B6 RID: 950
		public enum MMDebugMenuItemSpacerTypes
		{
			// Token: 0x04001D9B RID: 7579
			Small,
			// Token: 0x04001D9C RID: 7580
			Big
		}
	}
}
