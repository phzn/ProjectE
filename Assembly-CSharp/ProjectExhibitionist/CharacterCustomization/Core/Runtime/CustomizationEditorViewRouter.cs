using System;
using UnityEngine;

namespace ProjectExhibitionist.CharacterCustomization.Core.Runtime
{
	// Token: 0x02000AED RID: 2797
	public class CustomizationEditorViewRouter : MonoBehaviour
	{
		// Token: 0x17000D35 RID: 3381
		// (get) Token: 0x06004F13 RID: 20243 RVA: 0x001EA66E File Offset: 0x001E886E
		public bool HasCommonEditorRoot
		{
			get
			{
				return this.commonEditorRoot != null;
			}
		}

		// Token: 0x17000D36 RID: 3382
		// (get) Token: 0x06004F14 RID: 20244 RVA: 0x001EA67C File Offset: 0x001E887C
		public bool HasMainCharacterEditorRoot
		{
			get
			{
				return this.mainCharacterEditorRoot != null;
			}
		}

		// Token: 0x17000D37 RID: 3383
		// (get) Token: 0x06004F15 RID: 20245 RVA: 0x001EA68A File Offset: 0x001E888A
		public bool HasNpcEditorRoot
		{
			get
			{
				return this.npcEditorRoot != null;
			}
		}

		// Token: 0x17000D38 RID: 3384
		// (get) Token: 0x06004F16 RID: 20246 RVA: 0x001EA698 File Offset: 0x001E8898
		public bool HasSharedCanvasRoot
		{
			get
			{
				return this.sharedCanvasRoot != null;
			}
		}

		// Token: 0x17000D39 RID: 3385
		// (get) Token: 0x06004F17 RID: 20247 RVA: 0x001EA6A6 File Offset: 0x001E88A6
		public bool UsesCommonEditorRoot
		{
			get
			{
				return this.shellMode == CustomizationEditorViewRouter.EditorShellMode.SharedCommonRoot;
			}
		}

		// Token: 0x06004F18 RID: 20248 RVA: 0x001EA6B1 File Offset: 0x001E88B1
		private void Awake()
		{
			this.HideEditors();
		}

		// Token: 0x06004F19 RID: 20249 RVA: 0x001EA6BC File Offset: 0x001E88BC
		public void ShowForTarget(CustomizationTargetType targetType)
		{
			Debug.Log(string.Concat(new string[]
			{
				string.Format("[EditorViewRouter] ShowForTarget({0}) — ", targetType),
				"sharedCanvas=",
				(this.sharedCanvasRoot != null) ? this.sharedCanvasRoot.name : "NULL",
				", commonRoot=",
				(this.commonEditorRoot != null) ? this.commonEditorRoot.name : "NULL",
				", mcRoot=",
				(this.mainCharacterEditorRoot != null) ? this.mainCharacterEditorRoot.name : "NULL",
				", npcRoot=",
				(this.npcEditorRoot != null) ? this.npcEditorRoot.name : "NULL",
				", ",
				string.Format("shellMode={0}", this.shellMode)
			}));
			this.SetSharedCanvasVisible(true);
			if (this.commonEditorRoot != null)
			{
				this.commonEditorRoot.SetActive(this.shellMode == CustomizationEditorViewRouter.EditorShellMode.SharedCommonRoot);
			}
			if (this.mainCharacterEditorRoot != null)
			{
				bool flag = targetType == CustomizationTargetType.MainCharacter;
				Debug.Log(string.Format("[EditorViewRouter] Setting mainCharacterEditorRoot '{0}' active={1}", this.mainCharacterEditorRoot.name, flag));
				this.mainCharacterEditorRoot.SetActive(flag);
			}
			else
			{
				Debug.LogWarning("[EditorViewRouter] mainCharacterEditorRoot is NULL — MC editor cannot be shown!");
			}
			if (this.npcEditorRoot != null)
			{
				bool flag2 = targetType == CustomizationTargetType.Npc;
				this.npcEditorRoot.SetActive(flag2);
				if (flag2)
				{
					CharacterCreatorManager componentInChildren = this.npcEditorRoot.GetComponentInChildren<CharacterCreatorManager>(true);
					if (componentInChildren != null)
					{
						componentInChildren.ResetNavigationForNewSession();
					}
				}
			}
		}

		// Token: 0x06004F1A RID: 20250 RVA: 0x001EA870 File Offset: 0x001E8A70
		public void HideEditors()
		{
			Debug.Log("[EditorViewRouter] HideEditors called");
			if (this.commonEditorRoot != null)
			{
				this.commonEditorRoot.SetActive(false);
			}
			if (this.mainCharacterEditorRoot != null)
			{
				this.mainCharacterEditorRoot.SetActive(false);
			}
			if (this.npcEditorRoot != null)
			{
				this.npcEditorRoot.SetActive(false);
			}
			if (!this.keepCanvasEnabledWhenHidden)
			{
				this.SetSharedCanvasVisible(false);
				return;
			}
			this.SetSharedCanvasVisible(true);
		}

		// Token: 0x06004F1B RID: 20251 RVA: 0x001EA8EC File Offset: 0x001E8AEC
		private void SetSharedCanvasVisible(bool visible)
		{
			if (this.sharedCanvasRoot != null)
			{
				this.sharedCanvasRoot.SetActive(visible);
			}
		}

		// Token: 0x0400503D RID: 20541
		[Header("Roots")]
		[SerializeField]
		private GameObject sharedCanvasRoot;

		// Token: 0x0400503E RID: 20542
		[SerializeField]
		private GameObject commonEditorRoot;

		// Token: 0x0400503F RID: 20543
		[SerializeField]
		private GameObject mainCharacterEditorRoot;

		// Token: 0x04005040 RID: 20544
		[SerializeField]
		private GameObject npcEditorRoot;

		// Token: 0x04005041 RID: 20545
		[Header("Behavior")]
		[SerializeField]
		private CustomizationEditorViewRouter.EditorShellMode shellMode = CustomizationEditorViewRouter.EditorShellMode.DuplicatedPerEditor;

		// Token: 0x04005042 RID: 20546
		[SerializeField]
		private bool keepCanvasEnabledWhenHidden = true;

		// Token: 0x02000AEE RID: 2798
		public enum EditorShellMode
		{
			// Token: 0x04005044 RID: 20548
			SharedCommonRoot,
			// Token: 0x04005045 RID: 20549
			DuplicatedPerEditor
		}
	}
}
