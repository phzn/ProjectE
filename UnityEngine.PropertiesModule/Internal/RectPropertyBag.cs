using System;
using UnityEngine;

namespace Unity.Properties.Internal
{
	// Token: 0x020000B2 RID: 178
	internal class RectPropertyBag : ContainerPropertyBag<Rect>
	{
		// Token: 0x0600039C RID: 924 RVA: 0x0000C018 File Offset: 0x0000A218
		public RectPropertyBag()
		{
			base.AddProperty<float>(new RectPropertyBag.XProperty());
			base.AddProperty<float>(new RectPropertyBag.YProperty());
			base.AddProperty<float>(new RectPropertyBag.WidthProperty());
			base.AddProperty<float>(new RectPropertyBag.HeightProperty());
		}

		// Token: 0x020000B3 RID: 179
		private class XProperty : Property<Rect, float>
		{
			// Token: 0x1700008B RID: 139
			// (get) Token: 0x0600039D RID: 925 RVA: 0x0000BE24 File Offset: 0x0000A024
			public override string Name
			{
				get
				{
					return "x";
				}
			}

			// Token: 0x1700008C RID: 140
			// (get) Token: 0x0600039E RID: 926 RVA: 0x000057E1 File Offset: 0x000039E1
			public override bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x0600039F RID: 927 RVA: 0x0000C052 File Offset: 0x0000A252
			public override float GetValue(ref Rect container)
			{
				return container.x;
			}

			// Token: 0x060003A0 RID: 928 RVA: 0x0000C05A File Offset: 0x0000A25A
			public override void SetValue(ref Rect container, float value)
			{
				container.x = value;
			}
		}

		// Token: 0x020000B4 RID: 180
		private class YProperty : Property<Rect, float>
		{
			// Token: 0x1700008D RID: 141
			// (get) Token: 0x060003A2 RID: 930 RVA: 0x0000BE45 File Offset: 0x0000A045
			public override string Name
			{
				get
				{
					return "y";
				}
			}

			// Token: 0x1700008E RID: 142
			// (get) Token: 0x060003A3 RID: 931 RVA: 0x000057E1 File Offset: 0x000039E1
			public override bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060003A4 RID: 932 RVA: 0x0000C06D File Offset: 0x0000A26D
			public override float GetValue(ref Rect container)
			{
				return container.y;
			}

			// Token: 0x060003A5 RID: 933 RVA: 0x0000C075 File Offset: 0x0000A275
			public override void SetValue(ref Rect container, float value)
			{
				container.y = value;
			}
		}

		// Token: 0x020000B5 RID: 181
		private class WidthProperty : Property<Rect, float>
		{
			// Token: 0x1700008F RID: 143
			// (get) Token: 0x060003A7 RID: 935 RVA: 0x0000C07F File Offset: 0x0000A27F
			public override string Name
			{
				get
				{
					return "width";
				}
			}

			// Token: 0x17000090 RID: 144
			// (get) Token: 0x060003A8 RID: 936 RVA: 0x000057E1 File Offset: 0x000039E1
			public override bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060003A9 RID: 937 RVA: 0x0000C086 File Offset: 0x0000A286
			public override float GetValue(ref Rect container)
			{
				return container.width;
			}

			// Token: 0x060003AA RID: 938 RVA: 0x0000C08E File Offset: 0x0000A28E
			public override void SetValue(ref Rect container, float value)
			{
				container.width = value;
			}
		}

		// Token: 0x020000B6 RID: 182
		private class HeightProperty : Property<Rect, float>
		{
			// Token: 0x17000091 RID: 145
			// (get) Token: 0x060003AC RID: 940 RVA: 0x0000C098 File Offset: 0x0000A298
			public override string Name
			{
				get
				{
					return "height";
				}
			}

			// Token: 0x17000092 RID: 146
			// (get) Token: 0x060003AD RID: 941 RVA: 0x000057E1 File Offset: 0x000039E1
			public override bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060003AE RID: 942 RVA: 0x0000C09F File Offset: 0x0000A29F
			public override float GetValue(ref Rect container)
			{
				return container.height;
			}

			// Token: 0x060003AF RID: 943 RVA: 0x0000C0A7 File Offset: 0x0000A2A7
			public override void SetValue(ref Rect container, float value)
			{
				container.height = value;
			}
		}
	}
}
