using System;

namespace Pathfinding.Serialization
{
	// Token: 0x0200024C RID: 588
	public class SerializeSettings
	{
		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000EBC RID: 3772 RVA: 0x0005AA6F File Offset: 0x00058C6F
		public static SerializeSettings Settings
		{
			get
			{
				return new SerializeSettings
				{
					nodes = false
				};
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000EBD RID: 3773 RVA: 0x0005AA7D File Offset: 0x00058C7D
		public static SerializeSettings NodesAndSettings
		{
			get
			{
				return new SerializeSettings();
			}
		}

		// Token: 0x04000B14 RID: 2836
		public bool nodes = true;
	}
}
