using System;
using UnityEngine;

namespace Unity.Properties.Internal
{
	// Token: 0x020000AE RID: 174
	internal class Vector3IntPropertyBag : ContainerPropertyBag<Vector3Int>
	{
		// Token: 0x0600038C RID: 908 RVA: 0x0000BFAB File Offset: 0x0000A1AB
		public Vector3IntPropertyBag()
		{
			base.AddProperty<int>(new Vector3IntPropertyBag.XProperty());
			base.AddProperty<int>(new Vector3IntPropertyBag.YProperty());
			base.AddProperty<int>(new Vector3IntPropertyBag.ZProperty());
		}

		// Token: 0x020000AF RID: 175
		private class XProperty : Property<Vector3Int, int>
		{
			// Token: 0x17000085 RID: 133
			// (get) Token: 0x0600038D RID: 909 RVA: 0x0000BE24 File Offset: 0x0000A024
			public override string Name
			{
				get
				{
					return "x";
				}
			}

			// Token: 0x17000086 RID: 134
			// (get) Token: 0x0600038E RID: 910 RVA: 0x000057E1 File Offset: 0x000039E1
			public override bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x0600038F RID: 911 RVA: 0x0000BFD9 File Offset: 0x0000A1D9
			public override int GetValue(ref Vector3Int container)
			{
				return container.x;
			}

			// Token: 0x06000390 RID: 912 RVA: 0x0000BFE1 File Offset: 0x0000A1E1
			public override void SetValue(ref Vector3Int container, int value)
			{
				container.x = value;
			}
		}

		// Token: 0x020000B0 RID: 176
		private class YProperty : Property<Vector3Int, int>
		{
			// Token: 0x17000087 RID: 135
			// (get) Token: 0x06000392 RID: 914 RVA: 0x0000BE45 File Offset: 0x0000A045
			public override string Name
			{
				get
				{
					return "y";
				}
			}

			// Token: 0x17000088 RID: 136
			// (get) Token: 0x06000393 RID: 915 RVA: 0x000057E1 File Offset: 0x000039E1
			public override bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06000394 RID: 916 RVA: 0x0000BFF4 File Offset: 0x0000A1F4
			public override int GetValue(ref Vector3Int container)
			{
				return container.y;
			}

			// Token: 0x06000395 RID: 917 RVA: 0x0000BFFC File Offset: 0x0000A1FC
			public override void SetValue(ref Vector3Int container, int value)
			{
				container.y = value;
			}
		}

		// Token: 0x020000B1 RID: 177
		private class ZProperty : Property<Vector3Int, int>
		{
			// Token: 0x17000089 RID: 137
			// (get) Token: 0x06000397 RID: 919 RVA: 0x0000BEB6 File Offset: 0x0000A0B6
			public override string Name
			{
				get
				{
					return "z";
				}
			}

			// Token: 0x1700008A RID: 138
			// (get) Token: 0x06000398 RID: 920 RVA: 0x000057E1 File Offset: 0x000039E1
			public override bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06000399 RID: 921 RVA: 0x0000C006 File Offset: 0x0000A206
			public override int GetValue(ref Vector3Int container)
			{
				return container.z;
			}

			// Token: 0x0600039A RID: 922 RVA: 0x0000C00E File Offset: 0x0000A20E
			public override void SetValue(ref Vector3Int container, int value)
			{
				container.z = value;
			}
		}
	}
}
