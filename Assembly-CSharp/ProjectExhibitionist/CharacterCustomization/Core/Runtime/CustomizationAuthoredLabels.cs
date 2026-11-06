using System;
using System.Collections.Generic;
using Features.Localization;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectExhibitionist.CharacterCustomization.Core.Runtime
{
	// Token: 0x02000AE3 RID: 2787
	[DisallowMultipleComponent]
	public sealed class CustomizationAuthoredLabels : MonoBehaviour
	{
		// Token: 0x06004EE6 RID: 20198 RVA: 0x001E8D8C File Offset: 0x001E6F8C
		private static string CameraHint(string keyboardSource, bool richText)
		{
			if (!ControllerInput.IsActive)
			{
				return GameLocalization.GetSource("cielos_apps", keyboardSource);
			}
			Func<string, string> func;
			if (!richText)
			{
				func = (string value) => ControllerGlyphs.Plain(value, ControllerGlyphs.Family);
			}
			else
			{
				func = new Func<string, string>(ControllerGlyphs.Format);
			}
			Func<string, string> func2 = func;
			return string.Concat(new string[]
			{
				func2("LT + RS"),
				"  ",
				GameLocalization.GetSource("cielos_apps", "Zoom / rotate"),
				"\n",
				func2("LT + RT + RS"),
				"  ",
				GameLocalization.GetSource("cielos_apps", "Move up / down")
			});
		}

		// Token: 0x06004EE7 RID: 20199 RVA: 0x001E8E40 File Offset: 0x001E7040
		public static void BindRig(Transform presenter)
		{
			Transform transform = presenter;
			while (transform != null)
			{
				Transform transform2 = transform.Find("---UI");
				if (transform2 != null)
				{
					(transform2.GetComponent<CustomizationAuthoredLabels>() ?? transform2.gameObject.AddComponent<CustomizationAuthoredLabels>()).BindAuthoredTargets();
					return;
				}
				transform = transform.parent;
			}
		}

		// Token: 0x06004EE8 RID: 20200 RVA: 0x001E8E91 File Offset: 0x001E7091
		private void Start()
		{
			this.BindAuthoredTargets();
		}

		// Token: 0x06004EE9 RID: 20201 RVA: 0x001E8E9C File Offset: 0x001E709C
		private void BindAuthoredTargets()
		{
			TMP_InputField[] componentsInChildren = base.GetComponentsInChildren<TMP_InputField>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				LocalizedGameText.BindFontOnly(componentsInChildren[i].textComponent);
			}
			TMP_Text[] componentsInChildren2 = base.GetComponentsInChildren<TMP_Text>(true);
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				TMP_Text text2 = componentsInChildren2[i];
				if (!this._targets.Exists((CustomizationAuthoredLabels.Target target) => target.tmp == text2) && (!(text2.GetComponent<LocalizedGameText>() != null) || !(text2.name != "CameraGestureHint")))
				{
					CustomizationAuthoredLabels.Entry entry = this.FindEntry(text2.transform, text2.text);
					if (entry != null)
					{
						CustomizationAuthoredLabels.Target target3 = new CustomizationAuthoredLabels.Target
						{
							tmp = text2,
							entry = entry
						};
						this._targets.Add(target3);
						target3.Bind();
					}
				}
			}
			Text[] componentsInChildren3 = base.GetComponentsInChildren<Text>(true);
			for (int i = 0; i < componentsInChildren3.Length; i++)
			{
				Text text = componentsInChildren3[i];
				if (!this._targets.Exists((CustomizationAuthoredLabels.Target target) => target.legacy == text) && (!(text.GetComponent<LocalizedLegacyGameText>() != null) || !(text.name != "CameraGestureHint")))
				{
					CustomizationAuthoredLabels.Entry entry2 = this.FindEntry(text.transform, text.text);
					if (entry2 != null)
					{
						CustomizationAuthoredLabels.Target target2 = new CustomizationAuthoredLabels.Target
						{
							legacy = text,
							entry = entry2
						};
						this._targets.Add(target2);
						target2.Bind();
					}
				}
			}
		}

		// Token: 0x06004EEA RID: 20202 RVA: 0x001E905C File Offset: 0x001E725C
		private CustomizationAuthoredLabels.Entry FindEntry(Transform text, string english)
		{
			string text2 = text.name;
			Transform transform = text.parent;
			while (transform != null)
			{
				text2 = transform.name + "/" + text2;
				if (transform == base.transform)
				{
					break;
				}
				transform = transform.parent;
			}
			foreach (CustomizationAuthoredLabels.Entry entry in CustomizationAuthoredLabels.Entries)
			{
				if ((entry.english == english || entry.suffix.EndsWith("/CameraGestureHint", StringComparison.Ordinal)) && (text2 == entry.suffix || text2.EndsWith("/" + entry.suffix, StringComparison.Ordinal)))
				{
					return entry;
				}
			}
			return null;
		}

		// Token: 0x06004EEB RID: 20203 RVA: 0x001E9114 File Offset: 0x001E7314
		private void LateUpdate()
		{
			bool isActive = ControllerInput.IsActive;
			int version = ControllerGlyphs.Version;
			if (this._controllerHints != isActive || this._hintGlyphVersion != version)
			{
				this._controllerHints = isActive;
				this._hintGlyphVersion = version;
				foreach (CustomizationAuthoredLabels.Target target in this._targets)
				{
					if (target.IsCameraHint)
					{
						target.Bind();
					}
				}
			}
			if (GameLocalization.CurrentCode == "en")
			{
				return;
			}
			foreach (CustomizationAuthoredLabels.Target target2 in this._targets)
			{
				if (((target2.tmp != null) ? target2.tmp.text : ((target2.legacy != null) ? target2.legacy.text : null)) == target2.entry.english)
				{
					target2.Bind();
				}
			}
		}

		// Token: 0x04005020 RID: 20512
		private static readonly CustomizationAuthoredLabels.Entry[] Entries = new CustomizationAuthoredLabels.Entry[]
		{
			new CustomizationAuthoredLabels.Entry("Header/Container/Text (TMP)", "Who are you?"),
			new CustomizationAuthoredLabels.Entry("Options_Tabs/Window_Manager/Buttons/Style/Normal/Text", "STYLE"),
			new CustomizationAuthoredLabels.Entry("Options_Tabs/Window_Manager/Buttons/Expressions/Highlighted/Text", "EXPRESSIONS"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/NpcEditorRoot/Toolbar/Tools/Main_Categories/btn - Outfit/Selected/Title (1)", "Outfit"),
			new CustomizationAuthoredLabels.Entry("Sub_Category_Selector/Container/mingcute mouth-fill/Title", "Mouth"),
			new CustomizationAuthoredLabels.Entry("Options_Tabs/Window_Manager/Buttons/Shapes/Normal/Text", "SHAPES"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/NpcEditorRoot/Toolbar/Tools/Main_Panel/General_Category/Content/SCROLLVIEW/VIEWPORT/Info_Container/Surname/Label", "Surname"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/NpcEditorRoot/Toolbar/Tools/Main_Categories/btn - Colors/Selected/Title (1)", "Colors"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/CielEditorRoot/Toolbar/Tools/Main_Categories/btn - Colors/Selected/Title (1)", "Colors"),
			new CustomizationAuthoredLabels.Entry("---UI/SharedUI/ConfirmationModal/Content/Buttons/Confirm/Highlighted/Text", "Confirm"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/CielEditorRoot/Toolbar/Tools/Main_Categories/btn - Outfit/Normal/Title (1)", "Outfit"),
			new CustomizationAuthoredLabels.Entry("Options_Tabs/Window_Manager/Buttons/Expressions/Normal/Text", "EXPRESSIONS"),
			new CustomizationAuthoredLabels.Entry("Sub_Category_Selector/Container/mingcute ear-fill/Title", "Ears"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/CielEditorRoot/Toolbar/Tools/Main_Panel/General_Category/Content/SCROLLVIEW/VIEWPORT/Info_Container/Name/Label", "Ciel Preset Name"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/CielEditorRoot/Toolbar/Tools/Main_Categories/btn - General/Normal/Title (1)", "General"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/NpcEditorRoot/Toolbar/Tools/Main_Panel/General_Category/Content/SCROLLVIEW/VIEWPORT/Info_Container/Attitude/Label", "Attitude"),
			new CustomizationAuthoredLabels.Entry("Options_Tabs/Window_Manager/Buttons/Shapes/Pressed/Text", "SHAPES"),
			new CustomizationAuthoredLabels.Entry("Options_Tabs/Window_Manager/Buttons/Style/Highlighted/Text", "STYLE"),
			new CustomizationAuthoredLabels.Entry("Header/Container/Text (TMP)", "Color settings"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/NpcEditorRoot/Toolbar/Tools/Main_Categories/btn - Advanced/Normal/Title (1)", "Advanced"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/NpcEditorRoot/Toolbar/Tools/Main_Categories/btn - Advanced/Selected/Title (1)", "Advanced"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/CielEditorRoot/Camera_Controls/CameraGestureHint", "SCROLL  Zoom\nHOLD WHEEL + DRAG  Move up / down"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/CielEditorRoot/Toolbar/Widget_Area/PresetCustomization/Widget_Header/Widget_Name", "Make it custom"),
			new CustomizationAuthoredLabels.Entry("Options_Tabs/Window_Manager/Buttons/Style/Pressed/Text", "STYLE"),
			new CustomizationAuthoredLabels.Entry("Options_Tabs/Window_Manager/Buttons/Decals/Normal/Text", "DECALS"),
			new CustomizationAuthoredLabels.Entry("Sub_Category_Selector/Container/mingcute eye-2-fill (1)/Title", "Hair"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/NpcEditorRoot/Toolbar/Tools/Main_Categories/btn - General/Selected/Title (1)", "General"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationHub/FigmaCanvas/Hub/Toolbar/Tools/Main_Categories/btn - NPCs/Selected/Title (1)", "NPCs"),
			new CustomizationAuthoredLabels.Entry("Header/Container/AddPreset/Name", "Add"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/CielEditorRoot/Toolbar/Header/btn - Save/Title", "Save"),
			new CustomizationAuthoredLabels.Entry("Header/Container/Text (TMP)", "Ciel Data"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationHub/FigmaCanvas/Hub/Toolbar/Tools/Main_Categories/btn - NPCs/Normal/Title (1)", "NPCs"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationHub/FigmaCanvas/Hub/Toolbar/Tools/Main_Categories/btn - Main_Character/Normal/Title", "Main\nCharacter"),
			new CustomizationAuthoredLabels.Entry("---UI/SharedUI/ConfirmationModal/Content/Buttons/Cancel/Disabled/Text", "Cancel"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/CielEditorRoot/Toolbar/Tools/Main_Categories/btn - Outfit/Selected/Title (1)", "Outfit"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/CielEditorRoot/Toolbar/Widget_Area/PresetCustomization/Widget_Content/Colors/Text (TMP)", "COLORS\n"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/CielEditorRoot/Toolbar/Tools/Main_Categories/btn - Body/Normal/Title (1)", "Body"),
			new CustomizationAuthoredLabels.Entry("Header/Container/Text (TMP)", "More settings"),
			new CustomizationAuthoredLabels.Entry("Sub_Category_Selector/Container/mingcute eyebrow-fill/Title", "Eyebrows"),
			new CustomizationAuthoredLabels.Entry("Options_Tabs/Window_Manager/Buttons/Shapes/Highlighted/Text", "SHAPES"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/CielEditorRoot/Toolbar/Tools/Main_Categories/btn - Head/Normal/Title", "Head"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/CielEditorRoot/Toolbar/Tools/Main_Panel/General_Category/Content/SCROLLVIEW/VIEWPORT/Info_Container/Description/Label", "Biography"),
			new CustomizationAuthoredLabels.Entry("---UI/SharedUI/ConfirmationModal/Content/Buttons/Confirm/Normal/Text", "Confirm"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/NpcEditorRoot/Toolbar/Tools/Main_Panel/General_Category/Content/SCROLLVIEW/VIEWPORT/Info_Container/Descrizione/Input Field - Multi-Line/Placeholder", "Describe this character's personality, style, and role in the game..."),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/CielEditorRoot/Toolbar/Tools/Main_Panel/General_Category/Content/SCROLLVIEW/VIEWPORT/Info_Container/Description/Input Field - Multi-Line/Placeholder", "Describe this character's personality, style, and whatever you want to roleplay the game story..."),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/NpcEditorRoot/Toolbar/Tools/Main_Panel/General_Category/Content/SCROLLVIEW/VIEWPORT/Info_Container/Descrizione/Label", "Biography"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/NpcEditorRoot/Toolbar/Tools/Main_Categories/btn - Head/Selected/Title", "Head"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/NpcEditorRoot/Toolbar/Tools/Main_Categories/btn - Colors/Normal/Title (1)", "Colors"),
			new CustomizationAuthoredLabels.Entry("---UI/SharedUI/ConfirmationModal/Content/Buttons/Confirm/Disabled/Text", "Confirm"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationHub/FigmaCanvas/Hub/Toolbar/Tools/MainPanel/CielUsePreset/Container/PB - Standard/Label", "Keep pressed to use"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/NpcEditorRoot/Toolbar/Tools/Main_Panel/Advanced_Category/Content/SCROLLVIEW/VIEWPORT/AdvancedSettingsContainer/DialogueOverride/Title/Label", "NPC Dialogue Override"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/CielEditorRoot/Toolbar/Widget_Area/PresetCustomization/Widget_Content/Advanced/Text (TMP)", "MORE SETTINGS"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/NpcEditorRoot/Toolbar/Header/btn - Save/Title", "Save"),
			new CustomizationAuthoredLabels.Entry("Options_Tabs/Window_Manager/Buttons/Accessories/Highlighted/Text", "Accessories"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/NpcEditorRoot/Toolbar/Widget_Area/Color_Picker/Widget_Header/Widget_Name", "Color Picker"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/NpcEditorRoot/Toolbar/Tools/Main_Categories/btn - Body/Selected/Title (1)", "Body"),
			new CustomizationAuthoredLabels.Entry("Options_Tabs/Window_Manager/Buttons/Decals/Pressed/Text", "DECALS"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/NpcEditorRoot/Toolbar/Tools/Main_Panel/General_Category/Content/SCROLLVIEW/VIEWPORT/Info_Container/Gender Row/Label", "Gender"),
			new CustomizationAuthoredLabels.Entry("Options_Tabs/Window_Manager/Buttons/Expressions/Pressed/Text", "EXPRESSIONS"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/NpcEditorRoot/Camera_Controls/CameraGestureHint", "SCROLL  Zoom\nHOLD WHEEL + DRAG  Move up / down"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationHub/FigmaCanvas/Hub/Toolbar/Tools/MainPanel/CategoryPanel/Content/SCROLLVIEW/VIEWPORT/Empty/Placeholder/Label", "No NPCs found. \nCreate your first one!"),
			new CustomizationAuthoredLabels.Entry("---UI/SharedUI/ConfirmationModal/Content/Buttons/Cancel/Highlighted/Text", "Cancel"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/CielEditorRoot/Toolbar/Tools/Main_Categories/btn - Advanced/Normal/Title (1)", "Advanced"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/NpcEditorRoot/Toolbar/Tools/Main_Panel/General_Category/Content/SCROLLVIEW/VIEWPORT/Info_Container/Birthday/Date/Year/Placeholder", "Year"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/CielEditorRoot/Toolbar/Tools/Main_Categories/btn - Colors/Normal/Title (1)", "Colors"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationHub/FigmaCanvas/Hub/Toolbar/Header/Title/Default", "Characters List"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/CielEditorRoot/Toolbar/Tools/Main_Panel/General_Category/Content/SCROLLVIEW/VIEWPORT/Info_Container/PresetPic/PictureSection/TakePic/Label", "Click to take a Pic"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/NpcEditorRoot/Toolbar/Tools/Main_Panel/General_Category/Content/SCROLLVIEW/VIEWPORT/Info_Container/Birthday/Date/Day/Placeholder", "Day"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/CielEditorRoot/Toolbar/Tools/Main_Categories/btn - Head/Selected/Title", "Head"),
			new CustomizationAuthoredLabels.Entry("Sub_Category_Selector/Container/mingcute eye-2-fill/Title", "Eyes"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/NpcEditorRoot/Toolbar/Tools/Main_Panel/Advanced_Category/Content/SCROLLVIEW/VIEWPORT/AdvancedSettingsContainer/DialogueOverride/Title/Hint", "(if not selected, a dialogue based on character attitude will be used randomly)"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationHub/FigmaCanvas/Hub/Toolbar/Tools/Main_Categories/btn - Main_Character/Selected/Title", "Main\nCharacter"),
			new CustomizationAuthoredLabels.Entry("Header/Container/Text (TMP)", "Manage Your Characters"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/CielEditorRoot/Toolbar/Tools/Main_Categories/btn - Advanced/Selected/Title (1)", "Advanced"),
			new CustomizationAuthoredLabels.Entry("Options_Tabs/Window_Manager/Buttons/Accessories/Pressed/Text", "Accessories"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/NpcEditorRoot/Toolbar/Tools/Main_Panel/General_Category/Content/SCROLLVIEW/VIEWPORT/Info_Container/Birthday/Label", "Birthday"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/NpcEditorRoot/Toolbar/Tools/Main_Categories/btn - Head/Normal/Title", "Head"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationHub/FigmaCanvas/Hub/Toolbar/Tools/MainPanel/CategoryPanel/Content/SCROLLVIEW/VIEWPORT/Empty/Create_Character_Button/Name", "Create new"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/CielEditorRoot/Toolbar/Tools/Main_Panel/Advanced_Category/Content/SCROLLVIEW/VIEWPORT/WIP/Placeholder/Label", "Function under development!"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/NpcEditorRoot/Toolbar/Tools/Main_Categories/btn - Outfit/Normal/Title (1)", "Outfit"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationHub/FigmaCanvas/Hub/Camera_Controls/CameraGestureHint", "SCROLL  Zoom\nHOLD WHEEL + DRAG  Move up / down"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/CielEditorRoot/Toolbar/Tools/Main_Categories/btn - General/Selected/Title (1)", "General"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/NpcEditorRoot/Toolbar/Tools/Main_Categories/btn - General/Normal/Title (1)", "General"),
			new CustomizationAuthoredLabels.Entry("---UI/SharedUI/ConfirmationModal/Content/Buttons/Cancel/Normal/Text", "Cancel"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/NpcEditorRoot/Toolbar/Tools/Main_Categories/btn - Body/Normal/Title (1)", "Body"),
			new CustomizationAuthoredLabels.Entry("Options_Tabs/Window_Manager/Buttons/Accessories/Normal/Text", "Accessories"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/CielEditorRoot/Toolbar/Tools/Main_Categories/btn - Body/Selected/Title (1)", "Body"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/CielEditorRoot/Toolbar/Widget_Area/PresetCustomization/Widget_Content/Surface/Text (TMP)", "SURFACE"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/CielEditorRoot/Toolbar/Widget_Area/Color_Picker/Widget_Header/Widget_Name", "Color Picker"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/CielEditorRoot/Toolbar/Tools/Main_Panel/General_Category/Content/SCROLLVIEW/VIEWPORT/Info_Container/PresetPic/Label", "Ciel Picture"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/NpcEditorRoot/Toolbar/Tools/Main_Panel/General_Category/Content/SCROLLVIEW/VIEWPORT/Info_Container/Birthday/Date/Month/Placeholder", "Month"),
			new CustomizationAuthoredLabels.Entry("Options_Tabs/Window_Manager/Buttons/Decals/Highlighted/Text", "DECALS"),
			new CustomizationAuthoredLabels.Entry("---UI/CustomizationRoot/EditorCanvas/NpcEditorRoot/Toolbar/Tools/Main_Panel/General_Category/Content/SCROLLVIEW/VIEWPORT/Info_Container/Name/Label", "Name")
		};

		// Token: 0x04005021 RID: 20513
		private readonly List<CustomizationAuthoredLabels.Target> _targets = new List<CustomizationAuthoredLabels.Target>();

		// Token: 0x04005022 RID: 20514
		private bool _controllerHints;

		// Token: 0x04005023 RID: 20515
		private int _hintGlyphVersion = -1;

		// Token: 0x02000AE4 RID: 2788
		private sealed class Entry
		{
			// Token: 0x06004EEE RID: 20206 RVA: 0x001E9953 File Offset: 0x001E7B53
			public Entry(string suffix, string english)
			{
				this.suffix = suffix;
				this.english = english;
			}

			// Token: 0x04005024 RID: 20516
			public readonly string suffix;

			// Token: 0x04005025 RID: 20517
			public readonly string english;
		}

		// Token: 0x02000AE5 RID: 2789
		private sealed class Target
		{
			// Token: 0x17000D34 RID: 3380
			// (get) Token: 0x06004EEF RID: 20207 RVA: 0x001E9969 File Offset: 0x001E7B69
			public bool IsCameraHint
			{
				get
				{
					return this.entry.suffix.EndsWith("/CameraGestureHint", StringComparison.Ordinal);
				}
			}

			// Token: 0x06004EF0 RID: 20208 RVA: 0x001E9984 File Offset: 0x001E7B84
			public void Bind()
			{
				if (this.IsCameraHint)
				{
					if (!(this.tmp != null))
					{
						if (this.legacy != null)
						{
							LocalizedLegacyGameText localizedLegacyGameText = LocalizedLegacyGameText.BindComputed(this.legacy, () => CustomizationAuthoredLabels.CameraHint(this.entry.english, false));
							if (localizedLegacyGameText == null)
							{
								return;
							}
							localizedLegacyGameText.FitTranslationToRectangle();
						}
						return;
					}
					LocalizedGameText localizedGameText = LocalizedGameText.BindComputed(this.tmp, () => CustomizationAuthoredLabels.CameraHint(this.entry.english, true));
					if (localizedGameText == null)
					{
						return;
					}
					localizedGameText.FitTranslationToRectangle(false);
					return;
				}
				else
				{
					if (!(this.tmp != null))
					{
						if (this.legacy != null)
						{
							LocalizedLegacyGameText localizedLegacyGameText2 = LocalizedLegacyGameText.BindSource(this.legacy, "cielos_apps", this.entry.english);
							if (localizedLegacyGameText2 == null)
							{
								return;
							}
							localizedLegacyGameText2.FitTranslationToRectangle();
						}
						return;
					}
					LocalizedGameText localizedGameText2 = LocalizedGameText.BindSource(this.tmp, "cielos_apps", this.entry.english, null);
					if (localizedGameText2 == null)
					{
						return;
					}
					localizedGameText2.FitTranslationToRectangle(false);
					return;
				}
			}

			// Token: 0x04005026 RID: 20518
			public TMP_Text tmp;

			// Token: 0x04005027 RID: 20519
			public Text legacy;

			// Token: 0x04005028 RID: 20520
			public CustomizationAuthoredLabels.Entry entry;
		}
	}
}
