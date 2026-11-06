using System;
using UnityEngine;

namespace Unity.Properties.Internal
{
	// Token: 0x020000AB RID: 171
	internal class Vector2IntPropertyBag : ContainerPropertyBag<Vector2Int>
	{
		// Token: 0x06000381 RID: 897 RVA: 0x0000BF5C File Offset: 0x0000A15C
		public Vector2IntPropertyBag()
		{
			base.AddProperty<int>(new Vector2IntPropertyBag.XProperty());
			base.AddProperty<int>(new Vector2IntPropertyBag.YProperty());
		}

		// Token: 0x020000AC RID: 172
		private class XProperty : Property<Vector2Int, int>
		{
			// Token: 0x17000081 RID: 129
			// (get) Token: 0x06000382 RID: 898 RVA: 0x0000BE24 File Offset: 0x0000A024
			public override string Name
			{
				get
				{
					return "x";
				}
			}

			// Token: 0x17000082 RID: 130
			// (get) Token: 0x06000383 RID: 899 RVA: 0x000057E1 File Offset: 0x000039E1
			public override bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06000384 RID: 900 RVA: 0x0000BF7E File Offset: 0x0000A17E
			public override int GetValue(ref Vector2Int container)
			{
				return container.x;
			}

			// Token: 0x06000385 RID: 901 RVA: 0x0000BF86 File Offset: 0x0000A186
			public override void SetValue(ref Vector2Int container, int value)
			{
				container.x = value;
			}
		}

		// Token: 0x020000AD RID: 173
		private class YProperty : Property<Vector2Int, int>
		{
			// Token: 0x17000083 RID: 131
			// (get) Token: 0x06000387 RID: 903 RVA: 0x0000BE45 File Offset: 0x0000A045
			public override string Name
			{
				get
				{
					return "y";
				}
			}

			// Token: 0x17000084 RID: 132
			// (get) Token: 0x06000388 RID: 904 RVA: 0x000057E1 File Offset: 0x000039E1
			public override bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06000389 RID: 905 RVA: 0x0000BF99 File Offset: 0x0000A199
			public override int GetValue(ref Vector2Int container)
			{
				return container.y;
			}

			// Token: 0x0600038A RID: 906 RVA: 0x0000BFA1 File Offset: 0x0000A1A1
			public override void SetValue(ref Vector2Int container, int value)
			{
				container.y = value;
			}
		}
	}
}
