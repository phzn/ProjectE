using System;

namespace Invector.vEventSystems
{
	// Token: 0x0200061B RID: 1563
	public static class vIAnimatorStateInfoHelper
	{
		// Token: 0x060023F3 RID: 9203 RVA: 0x000CAC6C File Offset: 0x000C8E6C
		public static void Register(this vIAnimatorStateInfoController animatorStateInfos)
		{
			if (animatorStateInfos.isValid())
			{
				animatorStateInfos.animatorStateInfos.RegisterListener();
			}
		}

		// Token: 0x060023F4 RID: 9204 RVA: 0x000CAC81 File Offset: 0x000C8E81
		public static void UnRegister(this vIAnimatorStateInfoController animatorStateInfos)
		{
			if (animatorStateInfos.isValid())
			{
				animatorStateInfos.animatorStateInfos.RemoveListener();
			}
		}

		// Token: 0x060023F5 RID: 9205 RVA: 0x000CAC96 File Offset: 0x000C8E96
		public static bool isValid(this vIAnimatorStateInfoController animatorStateInfos)
		{
			return animatorStateInfos != null && animatorStateInfos.animatorStateInfos != null && animatorStateInfos.animatorStateInfos.animator != null;
		}
	}
}
