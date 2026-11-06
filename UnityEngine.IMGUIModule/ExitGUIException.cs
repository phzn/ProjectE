using System;

namespace UnityEngine
{
	// Token: 0x02000031 RID: 49
	public sealed class ExitGUIException : Exception
	{
		// Token: 0x060003E5 RID: 997 RVA: 0x0000CE36 File Offset: 0x0000B036
		public ExitGUIException()
		{
			GUIUtility.guiIsExiting = true;
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0000CE47 File Offset: 0x0000B047
		internal ExitGUIException(string message)
			: base(message)
		{
			GUIUtility.guiIsExiting = true;
		}
	}
}
