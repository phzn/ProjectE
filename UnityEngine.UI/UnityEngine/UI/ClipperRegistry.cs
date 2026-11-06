using System;
using UnityEngine.UI.Collections;

namespace UnityEngine.UI
{
	// Token: 0x02000009 RID: 9
	public class ClipperRegistry
	{
		// Token: 0x06000044 RID: 68 RVA: 0x0000292A File Offset: 0x00000B2A
		protected ClipperRegistry()
		{
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000045 RID: 69 RVA: 0x0000293D File Offset: 0x00000B3D
		public static ClipperRegistry instance
		{
			get
			{
				if (ClipperRegistry.s_Instance == null)
				{
					ClipperRegistry.s_Instance = new ClipperRegistry();
				}
				return ClipperRegistry.s_Instance;
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002958 File Offset: 0x00000B58
		public void Cull()
		{
			int count = this.m_Clippers.Count;
			for (int i = 0; i < count; i++)
			{
				this.m_Clippers[i].PerformClipping();
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x0000298E File Offset: 0x00000B8E
		public static void Register(IClipper c)
		{
			if (c == null)
			{
				return;
			}
			ClipperRegistry.instance.m_Clippers.AddUnique(c, true);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000029A6 File Offset: 0x00000BA6
		public static void Unregister(IClipper c)
		{
			ClipperRegistry.instance.m_Clippers.Remove(c);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000029B9 File Offset: 0x00000BB9
		public static void Disable(IClipper c)
		{
			ClipperRegistry.instance.m_Clippers.DisableItem(c);
		}

		// Token: 0x04000023 RID: 35
		private static ClipperRegistry s_Instance;

		// Token: 0x04000024 RID: 36
		private readonly IndexedSet<IClipper> m_Clippers = new IndexedSet<IClipper>();
	}
}
