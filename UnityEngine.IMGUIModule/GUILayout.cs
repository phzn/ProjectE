using System;

namespace UnityEngine
{
	// Token: 0x0200001A RID: 26
	public class GUILayout
	{
		// Token: 0x060001B9 RID: 441 RVA: 0x000081D6 File Offset: 0x000063D6
		public static void Label(Texture image, params GUILayoutOption[] options)
		{
			GUILayout.DoLabel(GUIContent.Temp(image), GUI.skin.label, options);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x000081F0 File Offset: 0x000063F0
		public static void Label(string text, params GUILayoutOption[] options)
		{
			GUILayout.DoLabel(GUIContent.Temp(text), GUI.skin.label, options);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000820A File Offset: 0x0000640A
		public static void Label(GUIContent content, params GUILayoutOption[] options)
		{
			GUILayout.DoLabel(content, GUI.skin.label, options);
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000821F File Offset: 0x0000641F
		public static void Label(Texture image, GUIStyle style, params GUILayoutOption[] options)
		{
			GUILayout.DoLabel(GUIContent.Temp(image), style, options);
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00008230 File Offset: 0x00006430
		public static void Label(string text, GUIStyle style, params GUILayoutOption[] options)
		{
			GUILayout.DoLabel(GUIContent.Temp(text), style, options);
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00008241 File Offset: 0x00006441
		public static void Label(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
		{
			GUILayout.DoLabel(content, style, options);
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000824D File Offset: 0x0000644D
		private static void DoLabel(GUIContent content, GUIStyle style, GUILayoutOption[] options)
		{
			GUI.Label(GUILayoutUtility.GetRect(content, style, options), content, style);
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00008260 File Offset: 0x00006460
		public static void Box(Texture image, params GUILayoutOption[] options)
		{
			GUILayout.DoBox(GUIContent.Temp(image), GUI.skin.box, options);
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000827A File Offset: 0x0000647A
		public static void Box(string text, params GUILayoutOption[] options)
		{
			GUILayout.DoBox(GUIContent.Temp(text), GUI.skin.box, options);
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00008294 File Offset: 0x00006494
		public static void Box(GUIContent content, params GUILayoutOption[] options)
		{
			GUILayout.DoBox(content, GUI.skin.box, options);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x000082A9 File Offset: 0x000064A9
		public static void Box(Texture image, GUIStyle style, params GUILayoutOption[] options)
		{
			GUILayout.DoBox(GUIContent.Temp(image), style, options);
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x000082BA File Offset: 0x000064BA
		public static void Box(string text, GUIStyle style, params GUILayoutOption[] options)
		{
			GUILayout.DoBox(GUIContent.Temp(text), style, options);
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x000082CB File Offset: 0x000064CB
		public static void Box(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
		{
			GUILayout.DoBox(content, style, options);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x000082D7 File Offset: 0x000064D7
		private static void DoBox(GUIContent content, GUIStyle style, GUILayoutOption[] options)
		{
			GUI.Box(GUILayoutUtility.GetRect(content, style, options), content, style);
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x000082EC File Offset: 0x000064EC
		public static bool Button(Texture image, params GUILayoutOption[] options)
		{
			return GUILayout.DoButton(GUIContent.Temp(image), GUI.skin.button, options);
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00008314 File Offset: 0x00006514
		public static bool Button(string text, params GUILayoutOption[] options)
		{
			return GUILayout.DoButton(GUIContent.Temp(text), GUI.skin.button, options);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000833C File Offset: 0x0000653C
		public static bool Button(GUIContent content, params GUILayoutOption[] options)
		{
			return GUILayout.DoButton(content, GUI.skin.button, options);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00008360 File Offset: 0x00006560
		public static bool Button(Texture image, GUIStyle style, params GUILayoutOption[] options)
		{
			return GUILayout.DoButton(GUIContent.Temp(image), style, options);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00008380 File Offset: 0x00006580
		public static bool Button(string text, GUIStyle style, params GUILayoutOption[] options)
		{
			return GUILayout.DoButton(GUIContent.Temp(text), style, options);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x000083A0 File Offset: 0x000065A0
		public static bool Button(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
		{
			return GUILayout.DoButton(content, style, options);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x000083BC File Offset: 0x000065BC
		private static bool DoButton(GUIContent content, GUIStyle style, GUILayoutOption[] options)
		{
			return GUI.Button(GUILayoutUtility.GetRect(content, style, options), content, style);
		}

		// Token: 0x060001CE RID: 462 RVA: 0x000083E0 File Offset: 0x000065E0
		public static bool RepeatButton(Texture image, params GUILayoutOption[] options)
		{
			return GUILayout.DoRepeatButton(GUIContent.Temp(image), GUI.skin.button, options);
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00008408 File Offset: 0x00006608
		public static bool RepeatButton(string text, params GUILayoutOption[] options)
		{
			return GUILayout.DoRepeatButton(GUIContent.Temp(text), GUI.skin.button, options);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00008430 File Offset: 0x00006630
		public static bool RepeatButton(GUIContent content, params GUILayoutOption[] options)
		{
			return GUILayout.DoRepeatButton(content, GUI.skin.button, options);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00008454 File Offset: 0x00006654
		public static bool RepeatButton(Texture image, GUIStyle style, params GUILayoutOption[] options)
		{
			return GUILayout.DoRepeatButton(GUIContent.Temp(image), style, options);
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00008474 File Offset: 0x00006674
		public static bool RepeatButton(string text, GUIStyle style, params GUILayoutOption[] options)
		{
			return GUILayout.DoRepeatButton(GUIContent.Temp(text), style, options);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00008494 File Offset: 0x00006694
		public static bool RepeatButton(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
		{
			return GUILayout.DoRepeatButton(content, style, options);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x000084B0 File Offset: 0x000066B0
		private static bool DoRepeatButton(GUIContent content, GUIStyle style, GUILayoutOption[] options)
		{
			return GUI.RepeatButton(GUILayoutUtility.GetRect(content, style, options), content, style);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x000084D4 File Offset: 0x000066D4
		public static string TextField(string text, params GUILayoutOption[] options)
		{
			return GUILayout.DoTextField(text, -1, false, GUI.skin.textField, options);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x000084FC File Offset: 0x000066FC
		public static string TextField(string text, int maxLength, params GUILayoutOption[] options)
		{
			return GUILayout.DoTextField(text, maxLength, false, GUI.skin.textField, options);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00008524 File Offset: 0x00006724
		public static string TextField(string text, GUIStyle style, params GUILayoutOption[] options)
		{
			return GUILayout.DoTextField(text, -1, false, style, options);
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00008540 File Offset: 0x00006740
		public static string TextField(string text, int maxLength, GUIStyle style, params GUILayoutOption[] options)
		{
			return GUILayout.DoTextField(text, maxLength, false, style, options);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000855C File Offset: 0x0000675C
		public static string PasswordField(string password, char maskChar, params GUILayoutOption[] options)
		{
			return GUILayout.PasswordField(password, maskChar, -1, GUI.skin.textField, options);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00008584 File Offset: 0x00006784
		public static string PasswordField(string password, char maskChar, int maxLength, params GUILayoutOption[] options)
		{
			return GUILayout.PasswordField(password, maskChar, maxLength, GUI.skin.textField, options);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x000085AC File Offset: 0x000067AC
		public static string PasswordField(string password, char maskChar, GUIStyle style, params GUILayoutOption[] options)
		{
			return GUILayout.PasswordField(password, maskChar, -1, style, options);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x000085C8 File Offset: 0x000067C8
		public static string PasswordField(string password, char maskChar, int maxLength, GUIStyle style, params GUILayoutOption[] options)
		{
			GUIContent guicontent = GUIContent.Temp(GUI.PasswordFieldGetStrToShow(password, maskChar));
			return GUI.PasswordField(GUILayoutUtility.GetRect(guicontent, GUI.skin.textField, options), password, maskChar, maxLength, style);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00008604 File Offset: 0x00006804
		public static string TextArea(string text, params GUILayoutOption[] options)
		{
			return GUILayout.DoTextField(text, -1, true, GUI.skin.textArea, options);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000862C File Offset: 0x0000682C
		public static string TextArea(string text, int maxLength, params GUILayoutOption[] options)
		{
			return GUILayout.DoTextField(text, maxLength, true, GUI.skin.textArea, options);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00008654 File Offset: 0x00006854
		public static string TextArea(string text, GUIStyle style, params GUILayoutOption[] options)
		{
			return GUILayout.DoTextField(text, -1, true, style, options);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00008670 File Offset: 0x00006870
		public static string TextArea(string text, int maxLength, GUIStyle style, params GUILayoutOption[] options)
		{
			return GUILayout.DoTextField(text, maxLength, true, style, options);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000868C File Offset: 0x0000688C
		private static string DoTextField(string text, int maxLength, bool multiline, GUIStyle style, GUILayoutOption[] options)
		{
			int controlID = GUIUtility.GetControlID(FocusType.Keyboard);
			GUIContent guicontent = GUIContent.Temp(text);
			bool flag = GUIUtility.keyboardControl != controlID;
			if (flag)
			{
				guicontent = GUIContent.Temp(text);
			}
			else
			{
				guicontent = GUIContent.Temp(text + GUIUtility.compositionString);
			}
			Rect rect = GUILayoutUtility.GetRect(guicontent, style, options);
			bool flag2 = GUIUtility.keyboardControl == controlID;
			if (flag2)
			{
				guicontent = GUIContent.Temp(text);
			}
			GUI.DoTextField(rect, controlID, guicontent, multiline, maxLength, style);
			return guicontent.text;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00008708 File Offset: 0x00006908
		public static bool Toggle(bool value, Texture image, params GUILayoutOption[] options)
		{
			return GUILayout.DoToggle(value, GUIContent.Temp(image), GUI.skin.toggle, options);
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00008734 File Offset: 0x00006934
		public static bool Toggle(bool value, string text, params GUILayoutOption[] options)
		{
			return GUILayout.DoToggle(value, GUIContent.Temp(text), GUI.skin.toggle, options);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00008760 File Offset: 0x00006960
		public static bool Toggle(bool value, GUIContent content, params GUILayoutOption[] options)
		{
			return GUILayout.DoToggle(value, content, GUI.skin.toggle, options);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00008784 File Offset: 0x00006984
		public static bool Toggle(bool value, Texture image, GUIStyle style, params GUILayoutOption[] options)
		{
			return GUILayout.DoToggle(value, GUIContent.Temp(image), style, options);
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x000087A4 File Offset: 0x000069A4
		public static bool Toggle(bool value, string text, GUIStyle style, params GUILayoutOption[] options)
		{
			return GUILayout.DoToggle(value, GUIContent.Temp(text), style, options);
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x000087C4 File Offset: 0x000069C4
		public static bool Toggle(bool value, GUIContent content, GUIStyle style, params GUILayoutOption[] options)
		{
			return GUILayout.DoToggle(value, content, style, options);
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x000087E0 File Offset: 0x000069E0
		private static bool DoToggle(bool value, GUIContent content, GUIStyle style, GUILayoutOption[] options)
		{
			return GUI.Toggle(GUILayoutUtility.GetRect(content, style, options), value, content, style);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00008804 File Offset: 0x00006A04
		public static int Toolbar(int selected, string[] texts, params GUILayoutOption[] options)
		{
			return GUILayout.Toolbar(selected, GUIContent.Temp(texts), GUI.skin.button, options);
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00008830 File Offset: 0x00006A30
		public static int Toolbar(int selected, Texture[] images, params GUILayoutOption[] options)
		{
			return GUILayout.Toolbar(selected, GUIContent.Temp(images), GUI.skin.button, options);
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000885C File Offset: 0x00006A5C
		public static int Toolbar(int selected, GUIContent[] contents, params GUILayoutOption[] options)
		{
			return GUILayout.Toolbar(selected, contents, GUI.skin.button, options);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00008880 File Offset: 0x00006A80
		public static int Toolbar(int selected, string[] texts, GUIStyle style, params GUILayoutOption[] options)
		{
			return GUILayout.Toolbar(selected, GUIContent.Temp(texts), style, options);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x000088A0 File Offset: 0x00006AA0
		public static int Toolbar(int selected, Texture[] images, GUIStyle style, params GUILayoutOption[] options)
		{
			return GUILayout.Toolbar(selected, GUIContent.Temp(images), style, options);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x000088C0 File Offset: 0x00006AC0
		public static int Toolbar(int selected, string[] texts, GUIStyle style, GUI.ToolbarButtonSize buttonSize, params GUILayoutOption[] options)
		{
			return GUILayout.Toolbar(selected, GUIContent.Temp(texts), style, buttonSize, options);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x000088E4 File Offset: 0x00006AE4
		public static int Toolbar(int selected, Texture[] images, GUIStyle style, GUI.ToolbarButtonSize buttonSize, params GUILayoutOption[] options)
		{
			return GUILayout.Toolbar(selected, GUIContent.Temp(images), style, buttonSize, options);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00008908 File Offset: 0x00006B08
		public static int Toolbar(int selected, GUIContent[] contents, GUIStyle style, params GUILayoutOption[] options)
		{
			return GUILayout.Toolbar(selected, contents, style, GUI.ToolbarButtonSize.Fixed, options);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00008924 File Offset: 0x00006B24
		public static int Toolbar(int selected, GUIContent[] contents, GUIStyle style, GUI.ToolbarButtonSize buttonSize, params GUILayoutOption[] options)
		{
			return GUILayout.Toolbar(selected, contents, null, style, buttonSize, options);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00008944 File Offset: 0x00006B44
		public static int Toolbar(int selected, GUIContent[] contents, bool[] enabled, GUIStyle style, params GUILayoutOption[] options)
		{
			return GUILayout.Toolbar(selected, contents, enabled, style, GUI.ToolbarButtonSize.Fixed, options);
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00008964 File Offset: 0x00006B64
		public static int Toolbar(int selected, GUIContent[] contents, bool[] enabled, GUIStyle style, GUI.ToolbarButtonSize buttonSize, params GUILayoutOption[] options)
		{
			GUIStyle guistyle;
			GUIStyle guistyle2;
			GUIStyle guistyle3;
			GUI.FindStyles(ref style, out guistyle, out guistyle2, out guistyle3, "left", "mid", "right");
			Vector2 vector = default(Vector2);
			int num = contents.Length;
			GUIStyle guistyle4 = ((num > 1) ? guistyle : style);
			GUIStyle guistyle5 = ((num > 1) ? guistyle2 : style);
			GUIStyle guistyle6 = ((num > 1) ? guistyle3 : style);
			float num2 = 0f;
			for (int i = 0; i < contents.Length; i++)
			{
				bool flag = i == num - 2;
				if (flag)
				{
					guistyle5 = guistyle6;
				}
				Vector2 vector2 = guistyle4.CalcSize(contents[i]);
				if (buttonSize != GUI.ToolbarButtonSize.Fixed)
				{
					if (buttonSize == GUI.ToolbarButtonSize.FitToContents)
					{
						vector.x += vector2.x;
					}
				}
				else
				{
					bool flag2 = vector2.x > vector.x;
					if (flag2)
					{
						vector.x = vector2.x;
					}
				}
				bool flag3 = vector2.y > vector.y;
				if (flag3)
				{
					vector.y = vector2.y;
				}
				bool flag4 = i == num - 1;
				if (flag4)
				{
					num2 += (float)guistyle4.margin.right;
				}
				else
				{
					num2 += (float)Mathf.Max(guistyle4.margin.right, guistyle5.margin.left);
				}
				guistyle4 = guistyle5;
			}
			if (buttonSize != GUI.ToolbarButtonSize.Fixed)
			{
				if (buttonSize == GUI.ToolbarButtonSize.FitToContents)
				{
					vector.x += num2;
				}
			}
			else
			{
				vector.x = vector.x * (float)contents.Length + num2;
			}
			return GUI.Toolbar(GUILayoutUtility.GetRect(vector.x, vector.y, style, options), selected, contents, null, style, buttonSize, enabled);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00008B20 File Offset: 0x00006D20
		public static int SelectionGrid(int selected, string[] texts, int xCount, params GUILayoutOption[] options)
		{
			return GUILayout.SelectionGrid(selected, GUIContent.Temp(texts), xCount, GUI.skin.button, options);
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00008B4C File Offset: 0x00006D4C
		public static int SelectionGrid(int selected, Texture[] images, int xCount, params GUILayoutOption[] options)
		{
			return GUILayout.SelectionGrid(selected, GUIContent.Temp(images), xCount, GUI.skin.button, options);
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00008B78 File Offset: 0x00006D78
		public static int SelectionGrid(int selected, GUIContent[] content, int xCount, params GUILayoutOption[] options)
		{
			return GUILayout.SelectionGrid(selected, content, xCount, GUI.skin.button, options);
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00008BA0 File Offset: 0x00006DA0
		public static int SelectionGrid(int selected, string[] texts, int xCount, GUIStyle style, params GUILayoutOption[] options)
		{
			return GUILayout.SelectionGrid(selected, GUIContent.Temp(texts), xCount, style, options);
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00008BC4 File Offset: 0x00006DC4
		public static int SelectionGrid(int selected, Texture[] images, int xCount, GUIStyle style, params GUILayoutOption[] options)
		{
			return GUILayout.SelectionGrid(selected, GUIContent.Temp(images), xCount, style, options);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00008BE8 File Offset: 0x00006DE8
		public static int SelectionGrid(int selected, GUIContent[] contents, int xCount, GUIStyle style, params GUILayoutOption[] options)
		{
			return GUI.SelectionGrid(GUIGridSizer.GetRect(contents, xCount, style, options), selected, contents, xCount, style);
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00008C10 File Offset: 0x00006E10
		public static float HorizontalSlider(float value, float leftValue, float rightValue, params GUILayoutOption[] options)
		{
			return GUILayout.DoHorizontalSlider(value, leftValue, rightValue, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb, options);
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00008C40 File Offset: 0x00006E40
		public static float HorizontalSlider(float value, float leftValue, float rightValue, GUIStyle slider, GUIStyle thumb, params GUILayoutOption[] options)
		{
			return GUILayout.DoHorizontalSlider(value, leftValue, rightValue, slider, thumb, options);
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00008C60 File Offset: 0x00006E60
		private static float DoHorizontalSlider(float value, float leftValue, float rightValue, GUIStyle slider, GUIStyle thumb, GUILayoutOption[] options)
		{
			return GUI.HorizontalSlider(GUILayoutUtility.GetRect(GUIContent.Temp("mmmm"), slider, options), value, leftValue, rightValue, slider, thumb);
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00008C90 File Offset: 0x00006E90
		public static float VerticalSlider(float value, float leftValue, float rightValue, params GUILayoutOption[] options)
		{
			return GUILayout.DoVerticalSlider(value, leftValue, rightValue, GUI.skin.verticalSlider, GUI.skin.verticalSliderThumb, options);
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00008CC0 File Offset: 0x00006EC0
		public static float VerticalSlider(float value, float leftValue, float rightValue, GUIStyle slider, GUIStyle thumb, params GUILayoutOption[] options)
		{
			return GUILayout.DoVerticalSlider(value, leftValue, rightValue, slider, thumb, options);
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00008CE0 File Offset: 0x00006EE0
		private static float DoVerticalSlider(float value, float leftValue, float rightValue, GUIStyle slider, GUIStyle thumb, params GUILayoutOption[] options)
		{
			return GUI.VerticalSlider(GUILayoutUtility.GetRect(GUIContent.Temp("\n\n\n\n\n"), slider, options), value, leftValue, rightValue, slider, thumb);
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00008D10 File Offset: 0x00006F10
		public static float HorizontalScrollbar(float value, float size, float leftValue, float rightValue, params GUILayoutOption[] options)
		{
			return GUILayout.HorizontalScrollbar(value, size, leftValue, rightValue, GUI.skin.horizontalScrollbar, options);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00008D38 File Offset: 0x00006F38
		public static float HorizontalScrollbar(float value, float size, float leftValue, float rightValue, GUIStyle style, params GUILayoutOption[] options)
		{
			return GUI.HorizontalScrollbar(GUILayoutUtility.GetRect(GUIContent.Temp("mmmm"), style, options), value, size, leftValue, rightValue, style);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00008D68 File Offset: 0x00006F68
		public static float VerticalScrollbar(float value, float size, float topValue, float bottomValue, params GUILayoutOption[] options)
		{
			return GUILayout.VerticalScrollbar(value, size, topValue, bottomValue, GUI.skin.verticalScrollbar, options);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x00008D90 File Offset: 0x00006F90
		public static float VerticalScrollbar(float value, float size, float topValue, float bottomValue, GUIStyle style, params GUILayoutOption[] options)
		{
			return GUI.VerticalScrollbar(GUILayoutUtility.GetRect(GUIContent.Temp("\n\n\n\n"), style, options), value, size, topValue, bottomValue, style);
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00008DC0 File Offset: 0x00006FC0
		public static void Space(float pixels)
		{
			GUIUtility.CheckOnGUI();
			bool isVertical = GUILayoutUtility.current.topLevel.isVertical;
			if (isVertical)
			{
				GUILayoutUtility.GetRect(0f, pixels, GUILayoutUtility.spaceStyle, new GUILayoutOption[] { GUILayout.Height(pixels) });
			}
			else
			{
				GUILayoutUtility.GetRect(pixels, 0f, GUILayoutUtility.spaceStyle, new GUILayoutOption[] { GUILayout.Width(pixels) });
			}
			bool flag = Event.current.type == EventType.Layout;
			if (flag)
			{
				GUILayoutUtility.current.topLevel.entries[GUILayoutUtility.current.topLevel.entries.Count - 1].consideredForMargin = false;
			}
		}

		// Token: 0x06000205 RID: 517 RVA: 0x00008E6C File Offset: 0x0000706C
		public static void FlexibleSpace()
		{
			GUIUtility.CheckOnGUI();
			bool isVertical = GUILayoutUtility.current.topLevel.isVertical;
			GUILayoutOption guilayoutOption;
			if (isVertical)
			{
				guilayoutOption = GUILayout.ExpandHeight(true);
			}
			else
			{
				guilayoutOption = GUILayout.ExpandWidth(true);
			}
			guilayoutOption = new GUILayoutOption(guilayoutOption.type, 10000);
			GUILayoutUtility.GetRect(0f, 0f, GUILayoutUtility.spaceStyle, new GUILayoutOption[] { guilayoutOption });
			bool flag = Event.current.type == EventType.Layout;
			if (flag)
			{
				GUILayoutUtility.current.topLevel.entries[GUILayoutUtility.current.topLevel.entries.Count - 1].consideredForMargin = false;
			}
		}

		// Token: 0x06000206 RID: 518 RVA: 0x00008F1B File Offset: 0x0000711B
		public static void BeginHorizontal(params GUILayoutOption[] options)
		{
			GUILayout.BeginHorizontal(GUIContent.none, GUIStyle.none, options);
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00008F2F File Offset: 0x0000712F
		public static void BeginHorizontal(GUIStyle style, params GUILayoutOption[] options)
		{
			GUILayout.BeginHorizontal(GUIContent.none, style, options);
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00008F3F File Offset: 0x0000713F
		public static void BeginHorizontal(string text, GUIStyle style, params GUILayoutOption[] options)
		{
			GUILayout.BeginHorizontal(GUIContent.Temp(text), style, options);
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00008F50 File Offset: 0x00007150
		public static void BeginHorizontal(Texture image, GUIStyle style, params GUILayoutOption[] options)
		{
			GUILayout.BeginHorizontal(GUIContent.Temp(image), style, options);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00008F64 File Offset: 0x00007164
		public static void BeginHorizontal(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
		{
			GUILayoutGroup guilayoutGroup = GUILayoutUtility.BeginLayoutGroup(style, options, typeof(GUILayoutGroup));
			guilayoutGroup.isVertical = false;
			bool flag = style != GUIStyle.none || content != GUIContent.none;
			if (flag)
			{
				GUI.Box(guilayoutGroup.rect, content, style);
			}
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00008FB3 File Offset: 0x000071B3
		public static void EndHorizontal()
		{
			GUILayoutUtility.EndLayoutGroup();
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00008FBC File Offset: 0x000071BC
		public static void BeginVertical(params GUILayoutOption[] options)
		{
			GUILayout.BeginVertical(GUIContent.none, GUIStyle.none, options);
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00008FD0 File Offset: 0x000071D0
		public static void BeginVertical(GUIStyle style, params GUILayoutOption[] options)
		{
			GUILayout.BeginVertical(GUIContent.none, style, options);
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00008FE0 File Offset: 0x000071E0
		public static void BeginVertical(string text, GUIStyle style, params GUILayoutOption[] options)
		{
			GUILayout.BeginVertical(GUIContent.Temp(text), style, options);
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00008FF1 File Offset: 0x000071F1
		public static void BeginVertical(Texture image, GUIStyle style, params GUILayoutOption[] options)
		{
			GUILayout.BeginVertical(GUIContent.Temp(image), style, options);
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00009004 File Offset: 0x00007204
		public static void BeginVertical(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
		{
			GUILayoutGroup guilayoutGroup = GUILayoutUtility.BeginLayoutGroup(style, options, typeof(GUILayoutGroup));
			guilayoutGroup.isVertical = true;
			bool flag = style != GUIStyle.none || content != GUIContent.none;
			if (flag)
			{
				GUI.Box(guilayoutGroup.rect, content, style);
			}
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00008FB3 File Offset: 0x000071B3
		public static void EndVertical()
		{
			GUILayoutUtility.EndLayoutGroup();
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00009053 File Offset: 0x00007253
		public static void BeginArea(Rect screenRect)
		{
			GUILayout.BeginArea(screenRect, GUIContent.none, GUIStyle.none);
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00009067 File Offset: 0x00007267
		public static void BeginArea(Rect screenRect, string text)
		{
			GUILayout.BeginArea(screenRect, GUIContent.Temp(text), GUIStyle.none);
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000907C File Offset: 0x0000727C
		public static void BeginArea(Rect screenRect, Texture image)
		{
			GUILayout.BeginArea(screenRect, GUIContent.Temp(image), GUIStyle.none);
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00009091 File Offset: 0x00007291
		public static void BeginArea(Rect screenRect, GUIContent content)
		{
			GUILayout.BeginArea(screenRect, content, GUIStyle.none);
		}

		// Token: 0x06000216 RID: 534 RVA: 0x000090A1 File Offset: 0x000072A1
		public static void BeginArea(Rect screenRect, GUIStyle style)
		{
			GUILayout.BeginArea(screenRect, GUIContent.none, style);
		}

		// Token: 0x06000217 RID: 535 RVA: 0x000090B1 File Offset: 0x000072B1
		public static void BeginArea(Rect screenRect, string text, GUIStyle style)
		{
			GUILayout.BeginArea(screenRect, GUIContent.Temp(text), style);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x000090C2 File Offset: 0x000072C2
		public static void BeginArea(Rect screenRect, Texture image, GUIStyle style)
		{
			GUILayout.BeginArea(screenRect, GUIContent.Temp(image), style);
		}

		// Token: 0x06000219 RID: 537 RVA: 0x000090D4 File Offset: 0x000072D4
		public static void BeginArea(Rect screenRect, GUIContent content, GUIStyle style)
		{
			GUIUtility.CheckOnGUI();
			GUILayoutGroup guilayoutGroup = GUILayoutUtility.BeginLayoutArea(style, typeof(GUILayoutGroup));
			bool flag = Event.current.type == EventType.Layout;
			if (flag)
			{
				guilayoutGroup.resetCoords = true;
				guilayoutGroup.minWidth = (guilayoutGroup.maxWidth = screenRect.width);
				guilayoutGroup.minHeight = (guilayoutGroup.maxHeight = screenRect.height);
				guilayoutGroup.rect = Rect.MinMaxRect(screenRect.xMin, screenRect.yMin, guilayoutGroup.rect.xMax, guilayoutGroup.rect.yMax);
			}
			GUI.BeginGroup(guilayoutGroup.rect, content, style);
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000917C File Offset: 0x0000737C
		public static void EndArea()
		{
			GUIUtility.CheckOnGUI();
			GUILayoutUtility.EndLayoutArea();
			bool flag = Event.current.type == EventType.Used;
			if (!flag)
			{
				GUI.EndGroup();
			}
		}

		// Token: 0x0600021B RID: 539 RVA: 0x000091B0 File Offset: 0x000073B0
		public static Vector2 BeginScrollView(Vector2 scrollPosition, params GUILayoutOption[] options)
		{
			return GUILayout.BeginScrollView(scrollPosition, false, false, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar, GUI.skin.scrollView, options);
		}

		// Token: 0x0600021C RID: 540 RVA: 0x000091EC File Offset: 0x000073EC
		public static Vector2 BeginScrollView(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, params GUILayoutOption[] options)
		{
			return GUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar, GUI.skin.scrollView, options);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00009228 File Offset: 0x00007428
		public static Vector2 BeginScrollView(Vector2 scrollPosition, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, params GUILayoutOption[] options)
		{
			return GUILayout.BeginScrollView(scrollPosition, false, false, horizontalScrollbar, verticalScrollbar, GUI.skin.scrollView, options);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00009250 File Offset: 0x00007450
		public static Vector2 BeginScrollView(Vector2 scrollPosition, GUIStyle style)
		{
			GUILayoutOption[] array = null;
			return GUILayout.BeginScrollView(scrollPosition, style, array);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000926C File Offset: 0x0000746C
		public static Vector2 BeginScrollView(Vector2 scrollPosition, GUIStyle style, params GUILayoutOption[] options)
		{
			string name = style.name;
			GUIStyle guistyle = GUI.skin.FindStyle(name + "VerticalScrollbar");
			bool flag = guistyle == null;
			if (flag)
			{
				guistyle = GUI.skin.verticalScrollbar;
			}
			GUIStyle guistyle2 = GUI.skin.FindStyle(name + "HorizontalScrollbar");
			bool flag2 = guistyle2 == null;
			if (flag2)
			{
				guistyle2 = GUI.skin.horizontalScrollbar;
			}
			return GUILayout.BeginScrollView(scrollPosition, false, false, guistyle2, guistyle, style, options);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x000092E8 File Offset: 0x000074E8
		public static Vector2 BeginScrollView(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, params GUILayoutOption[] options)
		{
			return GUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar, GUI.skin.scrollView, options);
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00009314 File Offset: 0x00007514
		public static Vector2 BeginScrollView(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, GUIStyle background, params GUILayoutOption[] options)
		{
			GUIUtility.CheckOnGUI();
			GUIScrollGroup guiscrollGroup = (GUIScrollGroup)GUILayoutUtility.BeginLayoutGroup(background, null, typeof(GUIScrollGroup));
			EventType type = Event.current.type;
			EventType eventType = type;
			if (eventType == EventType.Layout)
			{
				guiscrollGroup.resetCoords = true;
				guiscrollGroup.isVertical = true;
				guiscrollGroup.stretchWidth = 1;
				guiscrollGroup.stretchHeight = 1;
				guiscrollGroup.verticalScrollbar = verticalScrollbar;
				guiscrollGroup.horizontalScrollbar = horizontalScrollbar;
				guiscrollGroup.needsVerticalScrollbar = alwaysShowVertical;
				guiscrollGroup.needsHorizontalScrollbar = alwaysShowHorizontal;
				guiscrollGroup.ApplyOptions(options);
			}
			return GUI.BeginScrollView(guiscrollGroup.rect, scrollPosition, new Rect(0f, 0f, guiscrollGroup.clientWidth, guiscrollGroup.clientHeight), alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar, background);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x000093CB File Offset: 0x000075CB
		public static void EndScrollView()
		{
			GUILayout.EndScrollView(true);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x000093D5 File Offset: 0x000075D5
		internal static void EndScrollView(bool handleScrollWheel)
		{
			GUILayoutUtility.EndLayoutGroup();
			GUI.EndScrollView(handleScrollWheel);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x000093E8 File Offset: 0x000075E8
		public static Rect Window(int id, Rect screenRect, GUI.WindowFunction func, string text, params GUILayoutOption[] options)
		{
			return GUILayout.DoWindow(id, screenRect, func, GUIContent.Temp(text), GUI.skin.window, options);
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00009414 File Offset: 0x00007614
		public static Rect Window(int id, Rect screenRect, GUI.WindowFunction func, Texture image, params GUILayoutOption[] options)
		{
			return GUILayout.DoWindow(id, screenRect, func, GUIContent.Temp(image), GUI.skin.window, options);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00009440 File Offset: 0x00007640
		public static Rect Window(int id, Rect screenRect, GUI.WindowFunction func, GUIContent content, params GUILayoutOption[] options)
		{
			return GUILayout.DoWindow(id, screenRect, func, content, GUI.skin.window, options);
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00009468 File Offset: 0x00007668
		public static Rect Window(int id, Rect screenRect, GUI.WindowFunction func, string text, GUIStyle style, params GUILayoutOption[] options)
		{
			return GUILayout.DoWindow(id, screenRect, func, GUIContent.Temp(text), style, options);
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000948C File Offset: 0x0000768C
		public static Rect Window(int id, Rect screenRect, GUI.WindowFunction func, Texture image, GUIStyle style, params GUILayoutOption[] options)
		{
			return GUILayout.DoWindow(id, screenRect, func, GUIContent.Temp(image), style, options);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x000094B0 File Offset: 0x000076B0
		public static Rect Window(int id, Rect screenRect, GUI.WindowFunction func, GUIContent content, GUIStyle style, params GUILayoutOption[] options)
		{
			return GUILayout.DoWindow(id, screenRect, func, content, style, options);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x000094D0 File Offset: 0x000076D0
		private static Rect DoWindow(int id, Rect screenRect, GUI.WindowFunction func, GUIContent content, GUIStyle style, GUILayoutOption[] options)
		{
			GUIUtility.CheckOnGUI();
			GUILayout.LayoutedWindow layoutedWindow = new GUILayout.LayoutedWindow(func, screenRect, content, options, style);
			return GUI.Window(id, screenRect, new GUI.WindowFunction(layoutedWindow.DoWindow), content, style);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000950C File Offset: 0x0000770C
		public static GUILayoutOption Width(float width)
		{
			return new GUILayoutOption(GUILayoutOption.Type.fixedWidth, width);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000952C File Offset: 0x0000772C
		public static GUILayoutOption MinWidth(float minWidth)
		{
			return new GUILayoutOption(GUILayoutOption.Type.minWidth, minWidth);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000954C File Offset: 0x0000774C
		public static GUILayoutOption MaxWidth(float maxWidth)
		{
			return new GUILayoutOption(GUILayoutOption.Type.maxWidth, maxWidth);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000956C File Offset: 0x0000776C
		public static GUILayoutOption Height(float height)
		{
			return new GUILayoutOption(GUILayoutOption.Type.fixedHeight, height);
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000958C File Offset: 0x0000778C
		public static GUILayoutOption MinHeight(float minHeight)
		{
			return new GUILayoutOption(GUILayoutOption.Type.minHeight, minHeight);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x000095AC File Offset: 0x000077AC
		public static GUILayoutOption MaxHeight(float maxHeight)
		{
			return new GUILayoutOption(GUILayoutOption.Type.maxHeight, maxHeight);
		}

		// Token: 0x06000231 RID: 561 RVA: 0x000095CC File Offset: 0x000077CC
		public static GUILayoutOption ExpandWidth(bool expand)
		{
			return new GUILayoutOption(GUILayoutOption.Type.stretchWidth, expand ? 1 : 0);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x000095F0 File Offset: 0x000077F0
		public static GUILayoutOption ExpandHeight(bool expand)
		{
			return new GUILayoutOption(GUILayoutOption.Type.stretchHeight, expand ? 1 : 0);
		}

		// Token: 0x0200001B RID: 27
		private sealed class LayoutedWindow
		{
			// Token: 0x06000234 RID: 564 RVA: 0x00009614 File Offset: 0x00007814
			internal LayoutedWindow(GUI.WindowFunction f, Rect screenRect, GUIContent content, GUILayoutOption[] options, GUIStyle style)
			{
				this.m_Func = f;
				this.m_ScreenRect = screenRect;
				this.m_Options = options;
				this.m_Style = style;
			}

			// Token: 0x06000235 RID: 565 RVA: 0x0000963C File Offset: 0x0000783C
			public void DoWindow(int windowID)
			{
				GUILayoutGroup topLevel = GUILayoutUtility.current.topLevel;
				EventType type = Event.current.type;
				EventType eventType = type;
				if (eventType != EventType.Layout)
				{
					topLevel.ResetCursor();
				}
				else
				{
					topLevel.resetCoords = true;
					topLevel.rect = this.m_ScreenRect;
					bool flag = this.m_Options != null;
					if (flag)
					{
						topLevel.ApplyOptions(this.m_Options);
					}
					topLevel.isWindow = true;
					topLevel.windowID = windowID;
					topLevel.style = this.m_Style;
				}
				this.m_Func(windowID);
			}

			// Token: 0x0400007D RID: 125
			private readonly GUI.WindowFunction m_Func;

			// Token: 0x0400007E RID: 126
			private readonly Rect m_ScreenRect;

			// Token: 0x0400007F RID: 127
			private readonly GUILayoutOption[] m_Options;

			// Token: 0x04000080 RID: 128
			private readonly GUIStyle m_Style;
		}

		// Token: 0x0200001C RID: 28
		public class HorizontalScope : GUI.Scope
		{
			// Token: 0x06000236 RID: 566 RVA: 0x000096C8 File Offset: 0x000078C8
			public HorizontalScope(params GUILayoutOption[] options)
			{
				GUILayout.BeginHorizontal(options);
			}

			// Token: 0x06000237 RID: 567 RVA: 0x000096D9 File Offset: 0x000078D9
			public HorizontalScope(GUIStyle style, params GUILayoutOption[] options)
			{
				GUILayout.BeginHorizontal(style, options);
			}

			// Token: 0x06000238 RID: 568 RVA: 0x000096EB File Offset: 0x000078EB
			public HorizontalScope(string text, GUIStyle style, params GUILayoutOption[] options)
			{
				GUILayout.BeginHorizontal(text, style, options);
			}

			// Token: 0x06000239 RID: 569 RVA: 0x000096FE File Offset: 0x000078FE
			public HorizontalScope(Texture image, GUIStyle style, params GUILayoutOption[] options)
			{
				GUILayout.BeginHorizontal(image, style, options);
			}

			// Token: 0x0600023A RID: 570 RVA: 0x00009711 File Offset: 0x00007911
			public HorizontalScope(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
			{
				GUILayout.BeginHorizontal(content, style, options);
			}

			// Token: 0x0600023B RID: 571 RVA: 0x00009724 File Offset: 0x00007924
			protected override void CloseScope()
			{
				GUILayout.EndHorizontal();
			}
		}

		// Token: 0x0200001D RID: 29
		public class VerticalScope : GUI.Scope
		{
			// Token: 0x0600023C RID: 572 RVA: 0x0000972D File Offset: 0x0000792D
			public VerticalScope(params GUILayoutOption[] options)
			{
				GUILayout.BeginVertical(options);
			}

			// Token: 0x0600023D RID: 573 RVA: 0x0000973E File Offset: 0x0000793E
			public VerticalScope(GUIStyle style, params GUILayoutOption[] options)
			{
				GUILayout.BeginVertical(style, options);
			}

			// Token: 0x0600023E RID: 574 RVA: 0x00009750 File Offset: 0x00007950
			public VerticalScope(string text, GUIStyle style, params GUILayoutOption[] options)
			{
				GUILayout.BeginVertical(text, style, options);
			}

			// Token: 0x0600023F RID: 575 RVA: 0x00009763 File Offset: 0x00007963
			public VerticalScope(Texture image, GUIStyle style, params GUILayoutOption[] options)
			{
				GUILayout.BeginVertical(image, style, options);
			}

			// Token: 0x06000240 RID: 576 RVA: 0x00009776 File Offset: 0x00007976
			public VerticalScope(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
			{
				GUILayout.BeginVertical(content, style, options);
			}

			// Token: 0x06000241 RID: 577 RVA: 0x00009789 File Offset: 0x00007989
			protected override void CloseScope()
			{
				GUILayout.EndVertical();
			}
		}

		// Token: 0x0200001E RID: 30
		public class AreaScope : GUI.Scope
		{
			// Token: 0x06000242 RID: 578 RVA: 0x00009792 File Offset: 0x00007992
			public AreaScope(Rect screenRect)
			{
				GUILayout.BeginArea(screenRect);
			}

			// Token: 0x06000243 RID: 579 RVA: 0x000097A3 File Offset: 0x000079A3
			public AreaScope(Rect screenRect, string text)
			{
				GUILayout.BeginArea(screenRect, text);
			}

			// Token: 0x06000244 RID: 580 RVA: 0x000097B5 File Offset: 0x000079B5
			public AreaScope(Rect screenRect, Texture image)
			{
				GUILayout.BeginArea(screenRect, image);
			}

			// Token: 0x06000245 RID: 581 RVA: 0x000097C7 File Offset: 0x000079C7
			public AreaScope(Rect screenRect, GUIContent content)
			{
				GUILayout.BeginArea(screenRect, content);
			}

			// Token: 0x06000246 RID: 582 RVA: 0x000097D9 File Offset: 0x000079D9
			public AreaScope(Rect screenRect, string text, GUIStyle style)
			{
				GUILayout.BeginArea(screenRect, text, style);
			}

			// Token: 0x06000247 RID: 583 RVA: 0x000097EC File Offset: 0x000079EC
			public AreaScope(Rect screenRect, Texture image, GUIStyle style)
			{
				GUILayout.BeginArea(screenRect, image, style);
			}

			// Token: 0x06000248 RID: 584 RVA: 0x000097FF File Offset: 0x000079FF
			public AreaScope(Rect screenRect, GUIContent content, GUIStyle style)
			{
				GUILayout.BeginArea(screenRect, content, style);
			}

			// Token: 0x06000249 RID: 585 RVA: 0x00009812 File Offset: 0x00007A12
			protected override void CloseScope()
			{
				GUILayout.EndArea();
			}
		}

		// Token: 0x0200001F RID: 31
		public class ScrollViewScope : GUI.Scope
		{
			// Token: 0x1700003F RID: 63
			// (get) Token: 0x0600024A RID: 586 RVA: 0x0000981B File Offset: 0x00007A1B
			// (set) Token: 0x0600024B RID: 587 RVA: 0x00009823 File Offset: 0x00007A23
			public Vector2 scrollPosition { get; private set; }

			// Token: 0x17000040 RID: 64
			// (get) Token: 0x0600024C RID: 588 RVA: 0x0000982C File Offset: 0x00007A2C
			// (set) Token: 0x0600024D RID: 589 RVA: 0x00009834 File Offset: 0x00007A34
			public bool handleScrollWheel { get; set; }

			// Token: 0x0600024E RID: 590 RVA: 0x0000983D File Offset: 0x00007A3D
			public ScrollViewScope(Vector2 scrollPosition, params GUILayoutOption[] options)
			{
				this.handleScrollWheel = true;
				this.scrollPosition = GUILayout.BeginScrollView(scrollPosition, options);
			}

			// Token: 0x0600024F RID: 591 RVA: 0x0000985D File Offset: 0x00007A5D
			public ScrollViewScope(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, params GUILayoutOption[] options)
			{
				this.handleScrollWheel = true;
				this.scrollPosition = GUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, options);
			}

			// Token: 0x06000250 RID: 592 RVA: 0x00009880 File Offset: 0x00007A80
			public ScrollViewScope(Vector2 scrollPosition, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, params GUILayoutOption[] options)
			{
				this.handleScrollWheel = true;
				this.scrollPosition = GUILayout.BeginScrollView(scrollPosition, horizontalScrollbar, verticalScrollbar, options);
			}

			// Token: 0x06000251 RID: 593 RVA: 0x000098A3 File Offset: 0x00007AA3
			public ScrollViewScope(Vector2 scrollPosition, GUIStyle style, params GUILayoutOption[] options)
			{
				this.handleScrollWheel = true;
				this.scrollPosition = GUILayout.BeginScrollView(scrollPosition, style, options);
			}

			// Token: 0x06000252 RID: 594 RVA: 0x000098C4 File Offset: 0x00007AC4
			public ScrollViewScope(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, params GUILayoutOption[] options)
			{
				this.handleScrollWheel = true;
				this.scrollPosition = GUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar, options);
			}

			// Token: 0x06000253 RID: 595 RVA: 0x000098EB File Offset: 0x00007AEB
			public ScrollViewScope(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, GUIStyle background, params GUILayoutOption[] options)
			{
				this.handleScrollWheel = true;
				this.scrollPosition = GUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar, background, options);
			}

			// Token: 0x06000254 RID: 596 RVA: 0x00009914 File Offset: 0x00007B14
			protected override void CloseScope()
			{
				GUILayout.EndScrollView(this.handleScrollWheel);
			}
		}
	}
}
