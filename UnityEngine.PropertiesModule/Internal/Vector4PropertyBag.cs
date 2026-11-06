using System;
using UnityEngine;

namespace Unity.Properties.Internal
{
	// Token: 0x020000A6 RID: 166
	internal class Vector4PropertyBag : ContainerPropertyBag<Vector4>
	{
		// Token: 0x0600036C RID: 876 RVA: 0x0000BECE File Offset: 0x0000A0CE
		public Vector4PropertyBag()
		{
			base.AddProperty<float>(new Vector4PropertyBag.XProperty());
			base.AddProperty<float>(new Vector4PropertyBag.YProperty());
			base.AddProperty<float>(new Vector4PropertyBag.ZProperty());
			base.AddProperty<float>(new Vector4PropertyBag.WProperty());
		}

		// Token: 0x020000A7 RID: 167
		private class XProperty : Property<Vector4, float>
		{
			// Token: 0x17000079 RID: 121
			// (get) Token: 0x0600036D RID: 877 RVA: 0x0000BE24 File Offset: 0x0000A024
			public override string Name
			{
				get
				{
					return "x";
				}
			}

			// Token: 0x1700007A RID: 122
			// (get) Token: 0x0600036E RID: 878 RVA: 0x000057E1 File Offset: 0x000039E1
			public override bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x0600036F RID: 879 RVA: 0x0000BF08 File Offset: 0x0000A108
			public override float GetValue(ref Vector4 container)
			{
				return container.x;
			}

			// Token: 0x06000370 RID: 880 RVA: 0x0000BF10 File Offset: 0x0000A110
			public override void SetValue(ref Vector4 container, float value)
			{
				container.x = value;
			}
		}

		// Token: 0x020000A8 RID: 168
		private class YProperty : Property<Vector4, float>
		{
			// Token: 0x1700007B RID: 123
			// (get) Token: 0x06000372 RID: 882 RVA: 0x0000BE45 File Offset: 0x0000A045
			public override string Name
			{
				get
				{
					return "y";
				}
			}

			// Token: 0x1700007C RID: 124
			// (get) Token: 0x06000373 RID: 883 RVA: 0x000057E1 File Offset: 0x000039E1
			public override bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06000374 RID: 884 RVA: 0x0000BF22 File Offset: 0x0000A122
			public override float GetValue(ref Vector4 container)
			{
				return container.y;
			}

			// Token: 0x06000375 RID: 885 RVA: 0x0000BF2A File Offset: 0x0000A12A
			public override void SetValue(ref Vector4 container, float value)
			{
				container.y = value;
			}
		}

		// Token: 0x020000A9 RID: 169
		private class ZProperty : Property<Vector4, float>
		{
			// Token: 0x1700007D RID: 125
			// (get) Token: 0x06000377 RID: 887 RVA: 0x0000BEB6 File Offset: 0x0000A0B6
			public override string Name
			{
				get
				{
					return "z";
				}
			}

			// Token: 0x1700007E RID: 126
			// (get) Token: 0x06000378 RID: 888 RVA: 0x000057E1 File Offset: 0x000039E1
			public override bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06000379 RID: 889 RVA: 0x0000BF33 File Offset: 0x0000A133
			public override float GetValue(ref Vector4 container)
			{
				return container.z;
			}

			// Token: 0x0600037A RID: 890 RVA: 0x0000BF3B File Offset: 0x0000A13B
			public override void SetValue(ref Vector4 container, float value)
			{
				container.z = value;
			}
		}

		// Token: 0x020000AA RID: 170
		private class WProperty : Property<Vector4, float>
		{
			// Token: 0x1700007F RID: 127
			// (get) Token: 0x0600037C RID: 892 RVA: 0x0000BF44 File Offset: 0x0000A144
			public override string Name
			{
				get
				{
					return "w";
				}
			}

			// Token: 0x17000080 RID: 128
			// (get) Token: 0x0600037D RID: 893 RVA: 0x000057E1 File Offset: 0x000039E1
			public override bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x0600037E RID: 894 RVA: 0x0000BF4B File Offset: 0x0000A14B
			public override float GetValue(ref Vector4 container)
			{
				return container.w;
			}

			// Token: 0x0600037F RID: 895 RVA: 0x0000BF53 File Offset: 0x0000A153
			public override void SetValue(ref Vector4 container, float value)
			{
				container.w = value;
			}
		}
	}
}
