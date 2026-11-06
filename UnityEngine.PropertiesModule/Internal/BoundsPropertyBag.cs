using System;
using UnityEngine;

namespace Unity.Properties.Internal
{
	// Token: 0x020000BC RID: 188
	internal class BoundsPropertyBag : ContainerPropertyBag<Bounds>
	{
		// Token: 0x060003C6 RID: 966 RVA: 0x0000C13C File Offset: 0x0000A33C
		public BoundsPropertyBag()
		{
			base.AddProperty<Vector3>(new BoundsPropertyBag.CenterProperty());
			base.AddProperty<Vector3>(new BoundsPropertyBag.ExtentsProperty());
		}

		// Token: 0x020000BD RID: 189
		private class CenterProperty : Property<Bounds, Vector3>
		{
			// Token: 0x1700009B RID: 155
			// (get) Token: 0x060003C7 RID: 967 RVA: 0x0000C15E File Offset: 0x0000A35E
			public override string Name
			{
				get
				{
					return "center";
				}
			}

			// Token: 0x1700009C RID: 156
			// (get) Token: 0x060003C8 RID: 968 RVA: 0x000057E1 File Offset: 0x000039E1
			public override bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060003C9 RID: 969 RVA: 0x0000C165 File Offset: 0x0000A365
			public override Vector3 GetValue(ref Bounds container)
			{
				return container.center;
			}

			// Token: 0x060003CA RID: 970 RVA: 0x0000C16D File Offset: 0x0000A36D
			public override void SetValue(ref Bounds container, Vector3 value)
			{
				container.center = value;
			}
		}

		// Token: 0x020000BE RID: 190
		private class ExtentsProperty : Property<Bounds, Vector3>
		{
			// Token: 0x1700009D RID: 157
			// (get) Token: 0x060003CC RID: 972 RVA: 0x0000C180 File Offset: 0x0000A380
			public override string Name
			{
				get
				{
					return "extents";
				}
			}

			// Token: 0x1700009E RID: 158
			// (get) Token: 0x060003CD RID: 973 RVA: 0x000057E1 File Offset: 0x000039E1
			public override bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x060003CE RID: 974 RVA: 0x0000C187 File Offset: 0x0000A387
			public override Vector3 GetValue(ref Bounds container)
			{
				return container.extents;
			}

			// Token: 0x060003CF RID: 975 RVA: 0x0000C18F File Offset: 0x0000A38F
			public override void SetValue(ref Bounds container, Vector3 value)
			{
				container.extents = value;
			}
		}
	}
}
