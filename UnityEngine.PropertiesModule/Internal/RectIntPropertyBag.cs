using System;
using UnityEngine;

namespace Unity.Properties.Internal
{
	// Token: 0x020000B7 RID: 183
	internal class RectIntPropertyBag : ContainerPropertyBag<RectInt>
	{
		// Token: 0x060003B1 RID: 945 RVA: 0x0000C0B1 File Offset: 0x0000A2B1
		public RectIntPropertyBag()
		{
			base.AddProperty<int>(new RectIntPropertyBag.XProperty());
			base.AddProperty<int>(new RectIntPropertyBag.YProperty());
			base.AddProperty<int>(new RectIntPropertyBag.WidthProperty());
			base.AddProperty<int>(new RectIntPropertyBag.HeightProperty());
		}

		// Token: 0x020000B8 RID: 184
		private class XProperty : Property<RectInt, int>
		{
			// Token: 0x17000093 RID: 147
			// (get) Token: 0x060003B2 RID: 946 RVA: 0x0000BE24 File Offset: 0x0000A024
			public override string Name
			{
				get
				{
					return "x";
				}
			}

			// Token: 0x17000094 RID: 148
			// (get) Token: 0x060003B3 RID: 947 RVA: 0x000057E1 File Offset: 0x000039E1
			public override bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060003B4 RID: 948 RVA: 0x0000C0EB File Offset: 0x0000A2EB
			public override int GetValue(ref RectInt container)
			{
				return container.x;
			}

			// Token: 0x060003B5 RID: 949 RVA: 0x0000C0F3 File Offset: 0x0000A2F3
			public override void SetValue(ref RectInt container, int value)
			{
				container.x = value;
			}
		}

		// Token: 0x020000B9 RID: 185
		private class YProperty : Property<RectInt, int>
		{
			// Token: 0x17000095 RID: 149
			// (get) Token: 0x060003B7 RID: 951 RVA: 0x0000BE45 File Offset: 0x0000A045
			public override string Name
			{
				get
				{
					return "y";
				}
			}

			// Token: 0x17000096 RID: 150
			// (get) Token: 0x060003B8 RID: 952 RVA: 0x000057E1 File Offset: 0x000039E1
			public override bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060003B9 RID: 953 RVA: 0x0000C106 File Offset: 0x0000A306
			public override int GetValue(ref RectInt container)
			{
				return container.y;
			}

			// Token: 0x060003BA RID: 954 RVA: 0x0000C10E File Offset: 0x0000A30E
			public override void SetValue(ref RectInt container, int value)
			{
				container.y = value;
			}
		}

		// Token: 0x020000BA RID: 186
		private class WidthProperty : Property<RectInt, int>
		{
			// Token: 0x17000097 RID: 151
			// (get) Token: 0x060003BC RID: 956 RVA: 0x0000C07F File Offset: 0x0000A27F
			public override string Name
			{
				get
				{
					return "width";
				}
			}

			// Token: 0x17000098 RID: 152
			// (get) Token: 0x060003BD RID: 957 RVA: 0x000057E1 File Offset: 0x000039E1
			public override bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060003BE RID: 958 RVA: 0x0000C118 File Offset: 0x0000A318
			public override int GetValue(ref RectInt container)
			{
				return container.width;
			}

			// Token: 0x060003BF RID: 959 RVA: 0x0000C120 File Offset: 0x0000A320
			public override void SetValue(ref RectInt container, int value)
			{
				container.width = value;
			}
		}

		// Token: 0x020000BB RID: 187
		private class HeightProperty : Property<RectInt, int>
		{
			// Token: 0x17000099 RID: 153
			// (get) Token: 0x060003C1 RID: 961 RVA: 0x0000C098 File Offset: 0x0000A298
			public override string Name
			{
				get
				{
					return "height";
				}
			}

			// Token: 0x1700009A RID: 154
			// (get) Token: 0x060003C2 RID: 962 RVA: 0x000057E1 File Offset: 0x000039E1
			public override bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060003C3 RID: 963 RVA: 0x0000C12A File Offset: 0x0000A32A
			public override int GetValue(ref RectInt container)
			{
				return container.height;
			}

			// Token: 0x060003C4 RID: 964 RVA: 0x0000C132 File Offset: 0x0000A332
			public override void SetValue(ref RectInt container, int value)
			{
				container.height = value;
			}
		}
	}
}
