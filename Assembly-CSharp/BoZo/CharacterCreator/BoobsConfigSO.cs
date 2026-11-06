using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoZo.CharacterCreator
{
	// Token: 0x020007AF RID: 1967
	[CreateAssetMenu(fileName = "NewBoobsConfig", menuName = "Character Creator/Boobs Config")]
	public class BoobsConfigSO : ScriptableObject
	{
		// Token: 0x170009AB RID: 2475
		// (get) Token: 0x060033EF RID: 13295 RVA: 0x001276DB File Offset: 0x001258DB
		public bool IsDefault
		{
			get
			{
				return this.activeBlendShapeNames == null || this.activeBlendShapeNames.Count == 0;
			}
		}

		// Token: 0x04003479 RID: 13433
		[Header("Configuration Info")]
		[Tooltip("Display name for this configuration")]
		public string configName = "Default";

		// Token: 0x0400347A RID: 13434
		[Tooltip("Icon for UI selection")]
		public Sprite icon;

		// Token: 0x0400347B RID: 13435
		[TextArea(2, 3)]
		public string description;

		// Token: 0x0400347C RID: 13436
		[Header("Animator Settings")]
		[Tooltip("Value to set for the BoobsType animator parameter (0-5)")]
		[Range(0f, 5f)]
		public int animatorBoobsTypeValue;

		// Token: 0x0400347D RID: 13437
		[Header("Blend Shapes")]
		[Tooltip("List of blend shape names to activate (set to 100) for this configuration. Empty list = Default (all blend shapes at 0)")]
		public List<string> activeBlendShapeNames = new List<string>();
	}
}
