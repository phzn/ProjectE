using System;
using UnityEngine;

namespace Unity.Properties.Internal
{
	// Token: 0x020000BF RID: 191
	internal class BoundsIntPropertyBag : ContainerPropertyBag<BoundsInt>
	{
		// Token: 0x060003D1 RID: 977 RVA: 0x0000C199 File Offset: 0x0000A399
		public BoundsIntPropertyBag()
		{
			base.AddProperty<Vector3Int>(new BoundsIntPropertyBag.PositionProperty());
			base.AddProperty<Vector3Int>(new BoundsIntPropertyBag.SizeProperty());
		}

		// Token: 0x020000C0 RID: 192
		private class PositionProperty : Property<BoundsInt, Vector3Int>
		{
			// Token: 0x1700009F RID: 159
			// (get) Token: 0x060003D2 RID: 978 RVA: 0x0000C1BB File Offset: 0x0000A3BB
			public override string Name
			{
				get
				{
					return "position";
				}
			}

			// Token: 0x170000A0 RID: 160
			// (get) Token: 0x060003D3 RID: 979 RVA: 0x000057E1 File Offset: 0x000039E1
			public override bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060003D4 RID: 980 RVA: 0x0000C1C2 File Offset: 0x0000A3C2
			public override Vector3Int GetValue(ref BoundsInt container)
			{
				return container.position;
			}

			// Token: 0x060003D5 RID: 981 RVA: 0x0000C1CA File Offset: 0x0000A3CA
			public override void SetValue(ref BoundsInt container, Vector3Int value)
			{
				container.position = value;
			}
		}

		// Token: 0x020000C1 RID: 193
		private class SizeProperty : Property<BoundsInt, Vector3Int>
		{
			// Token: 0x170000A1 RID: 161
			// (get) Token: 0x060003D7 RID: 983 RVA: 0x0000C1DD File Offset: 0x0000A3DD
			public override string Name
			{
				get
				{
					return "size";
				}
			}

			// Token: 0x170000A2 RID: 162
			// (get) Token: 0x060003D8 RID: 984 RVA: 0x000057E1 File Offset: 0x000039E1
			public override bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060003D9 RID: 985 RVA: 0x0000C1E4 File Offset: 0x0000A3E4
			public override Vector3Int GetValue(ref BoundsInt container)
			{
				return container.size;
			}

			// Token: 0x060003DA RID: 986 RVA: 0x0000C1EC File Offset: 0x0000A3EC
			public override void SetValue(ref BoundsInt container, Vector3Int value)
			{
				container.size = value;
			}
		}
	}
}
