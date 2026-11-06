using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000364 RID: 868
	[HelpURL("UIE-tss")]
	[Serializable]
	public class ThemeStyleSheet : StyleSheet
	{
		// Token: 0x06001CE1 RID: 7393 RVA: 0x0007039B File Offset: 0x0006E59B
		internal override void OnEnable()
		{
			base.isDefaultStyleSheet = true;
			base.OnEnable();
		}
	}
}
