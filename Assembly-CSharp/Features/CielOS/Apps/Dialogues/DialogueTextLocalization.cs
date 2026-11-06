using System;
using Features.Detection.Customization;
using Features.Localization;
using ProjectExhibitionist.UI;
using TMPro;
using UnityEngine;

namespace Features.CielOS.Apps.Dialogues
{
	// Token: 0x020014AC RID: 5292
	internal static class DialogueTextLocalization
	{
		// Token: 0x0600A2EF RID: 41711 RVA: 0x0043A4C6 File Offset: 0x004386C6
		internal static string Get(string english)
		{
			return GameLocalization.GetSource("dialogues_ui", english);
		}

		// Token: 0x0600A2F0 RID: 41712 RVA: 0x0043A4D3 File Offset: 0x004386D3
		internal static string Format(string english, params object[] arguments)
		{
			return GameLocalization.FormatSource("dialogues_ui", english, arguments);
		}

		// Token: 0x0600A2F1 RID: 41713 RVA: 0x0043A4E4 File Offset: 0x004386E4
		internal static string SystemContent(string english)
		{
			string text;
			if (!GameLocalization.TryGetSourceKey("dialogue", english, out text))
			{
				return DialogueTextLocalization.Get(english);
			}
			return GameLocalization.GetSource("dialogue", english);
		}

		// Token: 0x0600A2F2 RID: 41714 RVA: 0x0043A512 File Offset: 0x00438712
		internal static void Bind(TMP_Text target, string english)
		{
			LocalizedGameText localizedGameText = LocalizedGameText.BindSource(target, "dialogues_ui", english, null);
			if (localizedGameText == null)
			{
				return;
			}
			localizedGameText.FitTranslationToRectangle(false);
		}

		// Token: 0x0600A2F3 RID: 41715 RVA: 0x0043A52C File Offset: 0x0043872C
		internal static void Computed(TMP_Text target, Func<string> resolver)
		{
			LocalizedGameText localizedGameText = LocalizedGameText.BindComputed(target, resolver);
			if (localizedGameText == null)
			{
				return;
			}
			localizedGameText.FitTranslationToRectangle(false);
		}

		// Token: 0x0600A2F4 RID: 41716 RVA: 0x0043A540 File Offset: 0x00438740
		internal static void BindFormat(TMP_Text target, string english, params object[] arguments)
		{
			LocalizedGameText localizedGameText = LocalizedGameText.BindFormat(target, "dialogues_ui", english, arguments);
			if (localizedGameText == null)
			{
				return;
			}
			localizedGameText.FitTranslationToRectangle(false);
		}

		// Token: 0x0600A2F5 RID: 41717 RVA: 0x00345A4E File Offset: 0x00343C4E
		internal static void UserText(TMP_Text target, string value)
		{
			if (target == null)
			{
				return;
			}
			target.text = value;
			LocalizedGameText.BindFontOnly(target);
		}

		// Token: 0x0600A2F6 RID: 41718 RVA: 0x0043A55A File Offset: 0x0043875A
		internal static string DocumentTitle(DialogueDocView doc)
		{
			if (string.IsNullOrWhiteSpace(doc.dialogueName))
			{
				return DialogueTextLocalization.Get("(unnamed)");
			}
			if (!doc.isSystem)
			{
				return doc.DisplayLabel;
			}
			return DialogueTextLocalization.SystemContent(doc.DisplayLabel);
		}

		// Token: 0x0600A2F7 RID: 41719 RVA: 0x0043A590 File Offset: 0x00438790
		internal static void BindDocumentTitle(TMP_Text target, DialogueDocView doc)
		{
			if (doc.isSystem || string.IsNullOrWhiteSpace(doc.dialogueName))
			{
				DialogueTextLocalization.Computed(target, () => DialogueTextLocalization.DocumentTitle(doc));
				return;
			}
			DialogueTextLocalization.UserText(target, doc.DisplayLabel);
		}

		// Token: 0x0600A2F8 RID: 41720 RVA: 0x0043A5F0 File Offset: 0x004387F0
		internal static void BindLine(TMP_Text target, string source, bool isSystem)
		{
			if (string.IsNullOrEmpty(source))
			{
				DialogueTextLocalization.Bind(target, "(empty)");
				return;
			}
			if (isSystem)
			{
				DialogueTextLocalization.Computed(target, () => DialogueTextLocalization.SystemContent(source));
				return;
			}
			DialogueTextLocalization.UserText(target, source);
		}

		// Token: 0x0600A2F9 RID: 41721 RVA: 0x0043A645 File Offset: 0x00438845
		internal static void Toast(string message, string title)
		{
			ToastService.Show(message, title, null, -1f, "dialogues_ui");
		}

		// Token: 0x0600A2FA RID: 41722 RVA: 0x0043A65C File Offset: 0x0043885C
		internal static void BindPrefab(Transform root)
		{
			Transform transform = root.Find("MainView/Main/MainContent/Toolbar/Search/InputHost/TextArea/Placeholder");
			DialogueTextLocalization.Bind((transform != null) ? transform.GetComponent<TMP_Text>() : null, "Search lines...");
			Transform transform2 = root.Find("MainView/Main/MainContent/ColumnHeader/WEIGHT");
			DialogueTextLocalization.Bind((transform2 != null) ? transform2.GetComponent<TMP_Text>() : null, "WEIGHT");
			Transform transform3 = root.Find("WizardView/Step_Actor/Pills/Pill_Police/Label");
			DialogueTextLocalization.Bind((transform3 != null) ? transform3.GetComponent<TMP_Text>() : null, "Police");
			Transform transform4 = root.Find("DetailView/Container/SetsScroll/Viewport/SetsContent/SetTemplate/Lines/AddLine/Label");
			DialogueTextLocalization.Bind((transform4 != null) ? transform4.GetComponent<TMP_Text>() : null, "Add Line");
			Transform transform5 = root.Find("MainView/Sidebar/FilterHead/Title");
			DialogueTextLocalization.Bind((transform5 != null) ? transform5.GetComponent<TMP_Text>() : null, "Filters");
			Transform transform6 = root.Find("WizardView/Step_Personality/Pills/Pill_Shy/Label");
			DialogueTextLocalization.Bind((transform6 != null) ? transform6.GetComponent<TMP_Text>() : null, "Shy");
			Transform transform7 = root.Find("MainView/Sidebar/Section_GENDER/Pills/Pill_All/Label");
			DialogueTextLocalization.Bind((transform7 != null) ? transform7.GetComponent<TMP_Text>() : null, "All");
			Transform transform8 = root.Find("MainView/Main/MainContent/ListArea/Scroll/Viewport/Content/DialogueDocRow/Badges/Badge_CUSTOM/Label");
			DialogueTextLocalization.Bind((transform8 != null) ? transform8.GetComponent<TMP_Text>() : null, "CUSTOM");
			Transform transform9 = root.Find("WizardView/Subtitle");
			DialogueTextLocalization.Bind((transform9 != null) ? transform9.GetComponent<TMP_Text>() : null, "Pick who speaks this dialogue. Actor, personality and gender are the dialogue's identity — they can't be changed after creation.");
			Transform transform10 = root.Find("MainView/Sidebar/Section_PERSONALITY/Pills/Pill_Neutral/Label");
			DialogueTextLocalization.Bind((transform10 != null) ? transform10.GetComponent<TMP_Text>() : null, "Neutral");
			Transform transform11 = root.Find("MainView/Sidebar/Section_NPCGroups/GroupButtons/Group_Civilians/Label");
			DialogueTextLocalization.Bind((transform11 != null) ? transform11.GetComponent<TMP_Text>() : null, "Civilians");
			Transform transform12 = root.Find("DetailView/Container/SetsScroll/Viewport/SetsContent/SetTemplate/Lines/LineHeader/Text");
			DialogueTextLocalization.Bind((transform12 != null) ? transform12.GetComponent<TMP_Text>() : null, "Lines Preview");
			Transform transform13 = root.Find("MainView/Main/MainContent/ListArea/Scroll/Viewport/Content/DialogueDocRow/Badges/Badge_SYSTEM/Label");
			DialogueTextLocalization.Bind((transform13 != null) ? transform13.GetComponent<TMP_Text>() : null, "SYSTEM");
			Transform transform14 = root.Find("MainView/Sidebar/Section_PERSONALITY/Caption");
			DialogueTextLocalization.Bind((transform14 != null) ? transform14.GetComponent<TMP_Text>() : null, "Personality");
			Transform transform15 = root.Find("MainView/Sidebar/Section_TYPE/Pills/Pill_Custom/Label");
			DialogueTextLocalization.Bind((transform15 != null) ? transform15.GetComponent<TMP_Text>() : null, "Custom");
			Transform transform16 = root.Find("MainView/Sidebar/Section_PERSONALITY/Pills/Pill_Friendly/Label");
			DialogueTextLocalization.Bind((transform16 != null) ? transform16.GetComponent<TMP_Text>() : null, "Friendly");
			Transform transform17 = root.Find("MainView/Sidebar/Section_PERSONALITY/Pills/Pill_Bold/Label");
			DialogueTextLocalization.Bind((transform17 != null) ? transform17.GetComponent<TMP_Text>() : null, "Bold");
			Transform transform18 = root.Find("MainView/Sidebar/Section_GENDER/Caption");
			DialogueTextLocalization.Bind((transform18 != null) ? transform18.GetComponent<TMP_Text>() : null, "Gender");
			Transform transform19 = root.Find("MainView/Main/MainContent/ListArea/Scroll/Viewport/Content/DialogueDocRow/Info/DraftRow/Badge_DRAFT/Tip/Label");
			DialogueTextLocalization.Bind((transform19 != null) ? transform19.GetComponent<TMP_Text>() : null, "Draft: this dialogue won't be used in game until every phase has a set with at least one line.");
			Transform transform20 = root.Find("WizardView/Step_Name/Caption");
			DialogueTextLocalization.Bind((transform20 != null) ? transform20.GetComponent<TMP_Text>() : null, "4  ·  NAME");
			Transform transform21 = root.Find("MainView/Sidebar/Section_PERSONALITY/Pills/Pill_Shy/Label");
			DialogueTextLocalization.Bind((transform21 != null) ? transform21.GetComponent<TMP_Text>() : null, "Shy");
			Transform transform22 = root.Find("MainView/Main/MainContent/ListArea/Scroll/Viewport/Content/EmptyState");
			DialogueTextLocalization.Bind((transform22 != null) ? transform22.GetComponent<TMP_Text>() : null, "No lines match these filters.");
			Transform transform23 = root.Find("WizardView/Step_Personality/Pills/Pill_Friendly/Label");
			DialogueTextLocalization.Bind((transform23 != null) ? transform23.GetComponent<TMP_Text>() : null, "Friendly");
			Transform transform24 = root.Find("MainView/Main/MainContent/ListArea/Scroll/Viewport/Content/DialogueDocRow/Actions/Open/Label");
			DialogueTextLocalization.Bind((transform24 != null) ? transform24.GetComponent<TMP_Text>() : null, "View");
			Transform transform25 = root.Find("WizardView/Step_Personality/Pills/Pill_Neutral/Label");
			DialogueTextLocalization.Bind((transform25 != null) ? transform25.GetComponent<TMP_Text>() : null, "Neutral");
			Transform transform26 = root.Find("LineDialog/Card/Input/TextArea/Placeholder");
			DialogueTextLocalization.Bind((transform26 != null) ? transform26.GetComponent<TMP_Text>() : null, "Type the line…");
			Transform transform27 = root.Find("LineDialog/Card/Buttons/Cancel/Label");
			DialogueTextLocalization.Bind((transform27 != null) ? transform27.GetComponent<TMP_Text>() : null, "Cancel");
			Transform transform28 = root.Find("DetailView/Container/SetsScroll/Viewport/SetsContent/SetTemplate/Lines/LinesEmpty");
			DialogueTextLocalization.Bind((transform28 != null) ? transform28.GetComponent<TMP_Text>() : null, "Start adding a line");
			Transform transform29 = root.Find("MainView/Sidebar/Section_NPCGroups/Caption");
			DialogueTextLocalization.Bind((transform29 != null) ? transform29.GetComponent<TMP_Text>() : null, "NPC GROUPS");
			Transform transform30 = root.Find("MainView/Sidebar/Section_NPCGroups/GroupButtons/Group_Journalists/Label");
			DialogueTextLocalization.Bind((transform30 != null) ? transform30.GetComponent<TMP_Text>() : null, "Journalists");
			Transform transform31 = root.Find("WizardView/Step_Gender/Caption");
			DialogueTextLocalization.Bind((transform31 != null) ? transform31.GetComponent<TMP_Text>() : null, "3  ·  GENDER");
			Transform transform32 = root.Find("WizardView/Step_Name/NameInput/TextArea/Placeholder");
			DialogueTextLocalization.Bind((transform32 != null) ? transform32.GetComponent<TMP_Text>() : null, "Dialogue name...");
			Transform transform33 = root.Find("WizardView/Step_Personality/Pills/Pill_Bold/Label");
			DialogueTextLocalization.Bind((transform33 != null) ? transform33.GetComponent<TMP_Text>() : null, "Bold");
			Transform transform34 = root.Find("WizardView/Step_Personality/Caption");
			DialogueTextLocalization.Bind((transform34 != null) ? transform34.GetComponent<TMP_Text>() : null, "2  ·  PERSONALITY");
			Transform transform35 = root.Find("WizardView/Header/Title");
			DialogueTextLocalization.Bind((transform35 != null) ? transform35.GetComponent<TMP_Text>() : null, "New Dialogue");
			Transform transform36 = root.Find("WizardView/Step_Actor/Pills/Pill_Journalist/Label");
			DialogueTextLocalization.Bind((transform36 != null) ? transform36.GetComponent<TMP_Text>() : null, "Journalist");
			Transform transform37 = root.Find("WizardView/Step_Personality/Pills/Pill_Hostile/Label");
			DialogueTextLocalization.Bind((transform37 != null) ? transform37.GetComponent<TMP_Text>() : null, "Hostile");
			Transform transform38 = root.Find("MainView/Sidebar/FilterHead/Reset/Label");
			DialogueTextLocalization.Bind((transform38 != null) ? transform38.GetComponent<TMP_Text>() : null, "Reset");
			Transform transform39 = root.Find("MainView/Sidebar/Section_NPCGroups/GroupButtons/Group_Police/Label");
			DialogueTextLocalization.Bind((transform39 != null) ? transform39.GetComponent<TMP_Text>() : null, "Police");
			Transform transform40 = root.Find("MainView/Sidebar/Section_NPCGroups/GroupButtons/Group_Referee/Label");
			DialogueTextLocalization.Bind((transform40 != null) ? transform40.GetComponent<TMP_Text>() : null, "Referee");
			Transform transform41 = root.Find("WizardView/Step_Gender/Pills/Pill_Female/Label");
			DialogueTextLocalization.Bind((transform41 != null) ? transform41.GetComponent<TMP_Text>() : null, "Female");
			Transform transform42 = root.Find("DetailView/Container/SetsScroll/Viewport/SetsContent/SetCreate/Title");
			DialogueTextLocalization.Bind((transform42 != null) ? transform42.GetComponent<TMP_Text>() : null, "Add New Set");
			Transform transform43 = root.Find("WizardView/Step_Actor/Pills/Pill_Civilian/Label");
			DialogueTextLocalization.Bind((transform43 != null) ? transform43.GetComponent<TMP_Text>() : null, "Civilian");
			Transform transform44 = root.Find("MainView/Sidebar/Section_PERSONALITY/Pills/Pill_All/Label");
			DialogueTextLocalization.Bind((transform44 != null) ? transform44.GetComponent<TMP_Text>() : null, "All");
			Transform transform45 = root.Find("ConfirmDialog/Card/Buttons/Cancel/Label");
			DialogueTextLocalization.Bind((transform45 != null) ? transform45.GetComponent<TMP_Text>() : null, "Cancel");
			Transform transform46 = root.Find("DetailView/Container/SetsScroll/Viewport/SetsContent/SetTemplate/TopRow/SetActions/ResetWeight/Label");
			DialogueTextLocalization.Bind((transform46 != null) ? transform46.GetComponent<TMP_Text>() : null, "Reset Weight");
			Transform transform47 = root.Find("WizardView/Step_Personality/Pills/Pill_Curious/Label");
			DialogueTextLocalization.Bind((transform47 != null) ? transform47.GetComponent<TMP_Text>() : null, "Curious");
			Transform transform48 = root.Find("LineDialog/Card/Buttons/Save/Label");
			DialogueTextLocalization.Bind((transform48 != null) ? transform48.GetComponent<TMP_Text>() : null, "Save");
			Transform transform49 = root.Find("WizardView/Step_Gender/Pills/Pill_Any/Label");
			DialogueTextLocalization.Bind((transform49 != null) ? transform49.GetComponent<TMP_Text>() : null, "Any");
			Transform transform50 = root.Find("MainView/Sidebar/Section_PERSONALITY/Pills/Pill_Hostile/Label");
			DialogueTextLocalization.Bind((transform50 != null) ? transform50.GetComponent<TMP_Text>() : null, "Hostile");
			Transform transform51 = root.Find("MainView/Main/MainContent/ColumnHeader/LINE");
			DialogueTextLocalization.Bind((transform51 != null) ? transform51.GetComponent<TMP_Text>() : null, "LINE");
			Transform transform52 = root.Find("WizardView/Step_Gender/Pills/Pill_Male/Label");
			DialogueTextLocalization.Bind((transform52 != null) ? transform52.GetComponent<TMP_Text>() : null, "Male");
			Transform transform53 = root.Find("DetailView/Container/SetsScroll/Viewport/SetsContent/EmptyState");
			DialogueTextLocalization.Bind((transform53 != null) ? transform53.GetComponent<TMP_Text>() : null, "No alternatives in this phase.");
			Transform transform54 = root.Find("MainView/Main/Header/Save/Label");
			DialogueTextLocalization.Bind((transform54 != null) ? transform54.GetComponent<TMP_Text>() : null, "SAVE");
			Transform transform55 = root.Find("MainView/Main/MainContent/Toolbar/NewLine/Label");
			DialogueTextLocalization.Bind((transform55 != null) ? transform55.GetComponent<TMP_Text>() : null, "+ Create");
			Transform transform56 = root.Find("MainView/Sidebar/Section_GENDER/Pills/Pill_Female/Label");
			DialogueTextLocalization.Bind((transform56 != null) ? transform56.GetComponent<TMP_Text>() : null, "Female");
			Transform transform57 = root.Find("MainView/Sidebar/Section_TYPE/Caption");
			DialogueTextLocalization.Bind((transform57 != null) ? transform57.GetComponent<TMP_Text>() : null, "TYPE");
			Transform transform58 = root.Find("WizardView/Step_Actor/Caption");
			DialogueTextLocalization.Bind((transform58 != null) ? transform58.GetComponent<TMP_Text>() : null, "1  ·  ACTOR");
			Transform transform59 = root.Find("MainView/Sidebar/Section_TYPE/Pills/Pill_System/Label");
			DialogueTextLocalization.Bind((transform59 != null) ? transform59.GetComponent<TMP_Text>() : null, "System");
			Transform transform60 = root.Find("MainView/Sidebar/Section_PERSONALITY/Pills/Pill_Curious/Label");
			DialogueTextLocalization.Bind((transform60 != null) ? transform60.GetComponent<TMP_Text>() : null, "Curious");
			Transform transform61 = root.Find("ConfirmDialog/Card/Buttons/Delete/Label");
			DialogueTextLocalization.Bind((transform61 != null) ? transform61.GetComponent<TMP_Text>() : null, "Delete");
			Transform transform62 = root.Find("DetailView/Container/Header/TitleCol/TitleInput/TextArea/Placeholder");
			DialogueTextLocalization.Bind((transform62 != null) ? transform62.GetComponent<TMP_Text>() : null, "Dialogue name…");
			Transform transform63 = root.Find("MainView/Main/MainContent/ColumnHeader/ACTIONS");
			DialogueTextLocalization.Bind((transform63 != null) ? transform63.GetComponent<TMP_Text>() : null, "ACTIONS");
			Transform transform64 = root.Find("MainView/Sidebar/Section_GENDER/Pills/Pill_Male/Label");
			DialogueTextLocalization.Bind((transform64 != null) ? transform64.GetComponent<TMP_Text>() : null, "Male");
			Transform transform65 = root.Find("MainView/Main/Header/HeaderTitle/Title");
			DialogueTextLocalization.Bind((transform65 != null) ? transform65.GetComponent<TMP_Text>() : null, "Dialogues");
			Transform transform66 = root.Find("MainView/Main/MainContent/ListArea/Scroll/Viewport/Content/DialogueDocRow/Info/DraftRow/Badge_DRAFT/Label");
			DialogueTextLocalization.Bind((transform66 != null) ? transform66.GetComponent<TMP_Text>() : null, "DRAFT");
			Transform transform67 = root.Find("WizardView/Footer/Create/Label");
			DialogueTextLocalization.Bind((transform67 != null) ? transform67.GetComponent<TMP_Text>() : null, "Create Dialogue");
			Transform transform68 = root.Find("MainView/Sidebar/Section_TYPE/Pills/Pill_All/Label");
			DialogueTextLocalization.Bind((transform68 != null) ? transform68.GetComponent<TMP_Text>() : null, "All");
			Transform transform69 = root.Find("WizardView/Step_Actor/Pills/Pill_Referee/Label");
			DialogueTextLocalization.Bind((transform69 != null) ? transform69.GetComponent<TMP_Text>() : null, "Referee");
			Transform transform70 = root.Find("DetailView/Container/Header/Duplicate/Label");
			DialogueTextLocalization.Bind((transform70 != null) ? transform70.GetComponent<TMP_Text>() : null, "Duplicate");
			Transform transform71 = root.Find("DetailView/Container/Header/Save/Label");
			DialogueTextLocalization.Bind((transform71 != null) ? transform71.GetComponent<TMP_Text>() : null, "Done");
			Transform transform72 = root.Find("MainView/Sidebar/Note/Note2/HintControls/HintCounter");
			LocalizedGameText.BindFontOnly((transform72 != null) ? transform72.GetComponent<TMP_Text>() : null);
			Transform transform73 = root.Find("DetailView/Container/Header/TitleCol/BindingChips/Chip_Personality/Label");
			LocalizedGameText.BindFontOnly((transform73 != null) ? transform73.GetComponent<TMP_Text>() : null);
			Transform transform74 = root.Find("MainView/Main/MainContent/Toolbar/Sort/Value");
			LocalizedGameText.BindFontOnly((transform74 != null) ? transform74.GetComponent<TMP_Text>() : null);
			Transform transform75 = root.Find("MainView/Main/MainContent/CategoryTabs/Tab_Other/Label");
			LocalizedGameText.BindFontOnly((transform75 != null) ? transform75.GetComponent<TMP_Text>() : null);
			Transform transform76 = root.Find("MainView/Sidebar/Section_NPCGroups/GroupButtons/Group_Police/Count");
			LocalizedGameText.BindFontOnly((transform76 != null) ? transform76.GetComponent<TMP_Text>() : null);
			Transform transform77 = root.Find("WizardView/Step_Name/NameInput/TextArea/Text");
			LocalizedGameText.BindFontOnly((transform77 != null) ? transform77.GetComponent<TMP_Text>() : null);
			Transform transform78 = root.Find("DetailView/Container/Header/TitleCol/BindingChips/Chip_Actor/Label");
			LocalizedGameText.BindFontOnly((transform78 != null) ? transform78.GetComponent<TMP_Text>() : null);
			Transform transform79 = root.Find("DetailView/Container/PhaseCaption/Subtitle");
			LocalizedGameText.BindFontOnly((transform79 != null) ? transform79.GetComponent<TMP_Text>() : null);
			Transform transform80 = root.Find("MainView/Main/MainContent/Footer/RangeWrap/Range");
			LocalizedGameText.BindFontOnly((transform80 != null) ? transform80.GetComponent<TMP_Text>() : null);
			Transform transform81 = root.Find("WizardView/Step_Name/Counter");
			LocalizedGameText.BindFontOnly((transform81 != null) ? transform81.GetComponent<TMP_Text>() : null);
			Transform transform82 = root.Find("DetailView/Container/Steps/OptionalChaseStep/ChaseStep/Label");
			LocalizedGameText.BindFontOnly((transform82 != null) ? transform82.GetComponent<TMP_Text>() : null);
			Transform transform83 = root.Find("DetailView/Container/Steps/ReactionStep/Label");
			LocalizedGameText.BindFontOnly((transform83 != null) ? transform83.GetComponent<TMP_Text>() : null);
			Transform transform84 = root.Find("ConfirmDialog/Card/Title");
			LocalizedGameText.BindFontOnly((transform84 != null) ? transform84.GetComponent<TMP_Text>() : null);
			Transform transform85 = root.Find("MainView/Main/MainContent/ListArea/Scroll/Viewport/Content/DialogueDocRow/Lines");
			LocalizedGameText.BindFontOnly((transform85 != null) ? transform85.GetComponent<TMP_Text>() : null);
			Transform transform86 = root.Find("DetailView/Container/SetsScroll/Viewport/SetsContent/SetTemplate/TopRow/SetTitle/TypeBadge/Label");
			LocalizedGameText.BindFontOnly((transform86 != null) ? transform86.GetComponent<TMP_Text>() : null);
			Transform transform87 = root.Find("MainView/Main/MainContent/CategoryTabs/Tab_Exit/Label");
			LocalizedGameText.BindFontOnly((transform87 != null) ? transform87.GetComponent<TMP_Text>() : null);
			Transform transform88 = root.Find("DetailView/Container/Steps/ActionsSteps/ExposeStep (1)/Count");
			LocalizedGameText.BindFontOnly((transform88 != null) ? transform88.GetComponent<TMP_Text>() : null);
			Transform transform89 = root.Find("DetailView/Container/Steps/ActionsSteps/ExposeStep/Label");
			LocalizedGameText.BindFontOnly((transform89 != null) ? transform89.GetComponent<TMP_Text>() : null);
			Transform transform90 = root.Find("DetailView/Container/SetsScroll/Viewport/SetsContent/SetTemplate/Lines/LinesContainer/LineRow/Number");
			LocalizedGameText.BindFontOnly((transform90 != null) ? transform90.GetComponent<TMP_Text>() : null);
			Transform transform91 = root.Find("DetailView/Container/Header/TitleCol/TitleInput/TextArea/Text");
			LocalizedGameText.BindFontOnly((transform91 != null) ? transform91.GetComponent<TMP_Text>() : null);
			Transform transform92 = root.Find("MainView/Main/MainContent/Toolbar/Search/InputHost/TextArea/Text");
			LocalizedGameText.BindFontOnly((transform92 != null) ? transform92.GetComponent<TMP_Text>() : null);
			Transform transform93 = root.Find("MainView/Main/MainContent/CategoryTabs/Tab_Exhibition/Label");
			LocalizedGameText.BindFontOnly((transform93 != null) ? transform93.GetComponent<TMP_Text>() : null);
			Transform transform94 = root.Find("LineDialog/Card/Body");
			LocalizedGameText.BindFontOnly((transform94 != null) ? transform94.GetComponent<TMP_Text>() : null);
			Transform transform95 = root.Find("MainView/Sidebar/Section_NPCGroups/GroupButtons/Group_Civilians/Count");
			LocalizedGameText.BindFontOnly((transform95 != null) ? transform95.GetComponent<TMP_Text>() : null);
			Transform transform96 = root.Find("MainView/Main/MainContent/Footer/Pages/Next/Label");
			LocalizedGameText.BindFontOnly((transform96 != null) ? transform96.GetComponent<TMP_Text>() : null);
			Transform transform97 = root.Find("DetailView/Container/Header/Back/Label");
			LocalizedGameText.BindFontOnly((transform97 != null) ? transform97.GetComponent<TMP_Text>() : null);
			Transform transform98 = root.Find("ConfirmDialog/Card/Foot");
			LocalizedGameText.BindFontOnly((transform98 != null) ? transform98.GetComponent<TMP_Text>() : null);
			Transform transform99 = root.Find("MainView/Main/MainContent/Footer/Pages/Last/Label");
			LocalizedGameText.BindFontOnly((transform99 != null) ? transform99.GetComponent<TMP_Text>() : null);
			Transform transform100 = root.Find("LineDialog/Card/Title");
			LocalizedGameText.BindFontOnly((transform100 != null) ? transform100.GetComponent<TMP_Text>() : null);
			Transform transform101 = root.Find("DetailView/Container/Header/TitleCol/Binding");
			LocalizedGameText.BindFontOnly((transform101 != null) ? transform101.GetComponent<TMP_Text>() : null);
			Transform transform102 = root.Find("DetailView/Container/SetsScroll/Viewport/SetsContent/SetTemplate/TopRow/SetActions/Percent");
			LocalizedGameText.BindFontOnly((transform102 != null) ? transform102.GetComponent<TMP_Text>() : null);
			Transform transform103 = root.Find("DetailView/Container/SetsScroll/Viewport/SetsContent/SetTemplate/TopRow/SetTitle/Title");
			LocalizedGameText.BindFontOnly((transform103 != null) ? transform103.GetComponent<TMP_Text>() : null);
			Transform transform104 = root.Find("DetailView/Container/Steps/OptionalChaseStep/ChaseStep/Count");
			LocalizedGameText.BindFontOnly((transform104 != null) ? transform104.GetComponent<TMP_Text>() : null);
			Transform transform105 = root.Find("MainView/Sidebar/Section_NPCGroups/GroupButtons/Group_Referee/Count");
			LocalizedGameText.BindFontOnly((transform105 != null) ? transform105.GetComponent<TMP_Text>() : null);
			Transform transform106 = root.Find("MainView/Main/MainContent/Footer/Pages/Prev/Label");
			LocalizedGameText.BindFontOnly((transform106 != null) ? transform106.GetComponent<TMP_Text>() : null);
			Transform transform107 = root.Find("MainView/Main/MainContent/CategoryTabs/Tab_Shame/Label");
			LocalizedGameText.BindFontOnly((transform107 != null) ? transform107.GetComponent<TMP_Text>() : null);
			Transform transform108 = root.Find("MainView/Main/MainContent/Footer/Pages/Numbers/PageTemplate/Label");
			LocalizedGameText.BindFontOnly((transform108 != null) ? transform108.GetComponent<TMP_Text>() : null);
			Transform transform109 = root.Find("DetailView/Container/Steps/ExitStep/Count");
			LocalizedGameText.BindFontOnly((transform109 != null) ? transform109.GetComponent<TMP_Text>() : null);
			Transform transform110 = root.Find("DetailView/Container/SetsScroll/Viewport/SetsContent/SetTemplate/Lines/LinesContainer/LineRow/Text");
			LocalizedGameText.BindFontOnly((transform110 != null) ? transform110.GetComponent<TMP_Text>() : null);
			Transform transform111 = root.Find("MainView/Main/MainContent/ListArea/Scroll/Viewport/Content/DialogueDocRow/Macro/Percent");
			LocalizedGameText.BindFontOnly((transform111 != null) ? transform111.GetComponent<TMP_Text>() : null);
			Transform transform112 = root.Find("MainView/Main/Header/HeaderTitle/EditingLabel");
			LocalizedGameText.BindFontOnly((transform112 != null) ? transform112.GetComponent<TMP_Text>() : null);
			Transform transform113 = root.Find("DetailView/Container/Steps/ExitStep/Label");
			LocalizedGameText.BindFontOnly((transform113 != null) ? transform113.GetComponent<TMP_Text>() : null);
			Transform transform114 = root.Find("MainView/Main/MainContent/ListArea/Scroll/Viewport/Content/DialogueDocRow/Info/Name");
			LocalizedGameText.BindFontOnly((transform114 != null) ? transform114.GetComponent<TMP_Text>() : null);
			Transform transform115 = root.Find("MainView/Main/MainContent/ListArea/Scroll/Viewport/Content/DialogueDocRow/Info/Binding");
			LocalizedGameText.BindFontOnly((transform115 != null) ? transform115.GetComponent<TMP_Text>() : null);
			Transform transform116 = root.Find("DetailView/Container/Steps/ReactionStep/Count");
			LocalizedGameText.BindFontOnly((transform116 != null) ? transform116.GetComponent<TMP_Text>() : null);
			Transform transform117 = root.Find("DetailView/Container/Header/TitleCol/Title");
			LocalizedGameText.BindFontOnly((transform117 != null) ? transform117.GetComponent<TMP_Text>() : null);
			Transform transform118 = root.Find("LineDialog/Card/Foot");
			LocalizedGameText.BindFontOnly((transform118 != null) ? transform118.GetComponent<TMP_Text>() : null);
			Transform transform119 = root.Find("MainView/Sidebar/Note/Note2/HintControls/HintPrev/Label");
			LocalizedGameText.BindFontOnly((transform119 != null) ? transform119.GetComponent<TMP_Text>() : null);
			Transform transform120 = root.Find("MainView/Sidebar/Note/Note2/HintControls/HintNext/Label");
			LocalizedGameText.BindFontOnly((transform120 != null) ? transform120.GetComponent<TMP_Text>() : null);
			Transform transform121 = root.Find("MainView/Main/MainContent/Footer/Pages/First/Label");
			LocalizedGameText.BindFontOnly((transform121 != null) ? transform121.GetComponent<TMP_Text>() : null);
			Transform transform122 = root.Find("ConfirmDialog/Card/Body");
			LocalizedGameText.BindFontOnly((transform122 != null) ? transform122.GetComponent<TMP_Text>() : null);
			Transform transform123 = root.Find("DetailView/Container/PhaseCaption/Title");
			LocalizedGameText.BindFontOnly((transform123 != null) ? transform123.GetComponent<TMP_Text>() : null);
			Transform transform124 = root.Find("MainView/Sidebar/Note/Note2/L1 (1)");
			LocalizedGameText.BindFontOnly((transform124 != null) ? transform124.GetComponent<TMP_Text>() : null);
			Transform transform125 = root.Find("DetailView/Container/Header/TitleCol/BindingChips/Chip_Gender/Label");
			LocalizedGameText.BindFontOnly((transform125 != null) ? transform125.GetComponent<TMP_Text>() : null);
			Transform transform126 = root.Find("LineDialog/Card/Input/TextArea/Text");
			LocalizedGameText.BindFontOnly((transform126 != null) ? transform126.GetComponent<TMP_Text>() : null);
			Transform transform127 = root.Find("MainView/Sidebar/Section_NPCGroups/GroupButtons/Group_Journalists/Count");
			LocalizedGameText.BindFontOnly((transform127 != null) ? transform127.GetComponent<TMP_Text>() : null);
			Transform transform128 = root.Find("DetailView/Container/Steps/ActionsSteps/ExposeStep (1)/Label");
			LocalizedGameText.BindFontOnly((transform128 != null) ? transform128.GetComponent<TMP_Text>() : null);
			Transform transform129 = root.Find("WizardView/Header/Back/Label");
			LocalizedGameText.BindFontOnly((transform129 != null) ? transform129.GetComponent<TMP_Text>() : null);
			Transform transform130 = root.Find("MainView/Main/MainContent/CategoryTabs/Tab_Reaction/Label");
			LocalizedGameText.BindFontOnly((transform130 != null) ? transform130.GetComponent<TMP_Text>() : null);
			Transform transform131 = root.Find("DetailView/Container/Steps/ActionsSteps/ExposeStep/Count");
			LocalizedGameText.BindFontOnly((transform131 != null) ? transform131.GetComponent<TMP_Text>() : null);
			Transform transform132 = root.Find("DetailView/Container/PhaseCaption/ValidationHint");
			LocalizedGameText.BindFontOnly((transform132 != null) ? transform132.GetComponent<TMP_Text>() : null);
			Transform transform133 = root.Find("WizardView/Footer/Summary");
			LocalizedGameText.BindFontOnly((transform133 != null) ? transform133.GetComponent<TMP_Text>() : null);
			Transform transform134 = root.Find("LineDialog/Card/Counter");
			LocalizedGameText.BindFontOnly((transform134 != null) ? transform134.GetComponent<TMP_Text>() : null);
		}

		// Token: 0x0400A6A2 RID: 42658
		internal const string Scope = "dialogues_ui";

		// Token: 0x020014AD RID: 5293
		internal sealed class Argument
		{
			// Token: 0x0600A2FB RID: 41723 RVA: 0x0043B6B3 File Offset: 0x004398B3
			internal Argument(Func<string> value)
			{
				this._value = value;
			}

			// Token: 0x0600A2FC RID: 41724 RVA: 0x0043B6C2 File Offset: 0x004398C2
			public override string ToString()
			{
				return this._value();
			}

			// Token: 0x0400A6A3 RID: 42659
			private readonly Func<string> _value;
		}
	}
}
