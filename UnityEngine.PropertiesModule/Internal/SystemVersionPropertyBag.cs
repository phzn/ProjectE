using System;
using UnityEngine;

namespace Unity.Properties.Internal
{
	// Token: 0x020000C2 RID: 194
	internal class SystemVersionPropertyBag : ContainerPropertyBag<Version>
	{
		// Token: 0x060003DC RID: 988 RVA: 0x0000C1F6 File Offset: 0x0000A3F6
		public SystemVersionPropertyBag()
		{
			base.AddProperty<int>(new SystemVersionPropertyBag.MajorProperty());
			base.AddProperty<int>(new SystemVersionPropertyBag.MinorProperty());
			base.AddProperty<int>(new SystemVersionPropertyBag.BuildProperty());
			base.AddProperty<int>(new SystemVersionPropertyBag.RevisionProperty());
		}

		// Token: 0x020000C3 RID: 195
		private class MajorProperty : Property<Version, int>
		{
			// Token: 0x060003DD RID: 989 RVA: 0x0000C230 File Offset: 0x0000A430
			public MajorProperty()
			{
				base.AddAttribute(new MinAttribute(0f));
			}

			// Token: 0x170000A3 RID: 163
			// (get) Token: 0x060003DE RID: 990 RVA: 0x0000C24B File Offset: 0x0000A44B
			public override string Name
			{
				get
				{
					return "Major";
				}
			}

			// Token: 0x170000A4 RID: 164
			// (get) Token: 0x060003DF RID: 991 RVA: 0x000052B1 File Offset: 0x000034B1
			public override bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x060003E0 RID: 992 RVA: 0x0000C252 File Offset: 0x0000A452
			public override int GetValue(ref Version container)
			{
				return container.Major;
			}

			// Token: 0x060003E1 RID: 993 RVA: 0x00005483 File Offset: 0x00003683
			public override void SetValue(ref Version container, int value)
			{
			}
		}

		// Token: 0x020000C4 RID: 196
		private class MinorProperty : Property<Version, int>
		{
			// Token: 0x060003E2 RID: 994 RVA: 0x0000C230 File Offset: 0x0000A430
			public MinorProperty()
			{
				base.AddAttribute(new MinAttribute(0f));
			}

			// Token: 0x170000A5 RID: 165
			// (get) Token: 0x060003E3 RID: 995 RVA: 0x0000C25B File Offset: 0x0000A45B
			public override string Name
			{
				get
				{
					return "Minor";
				}
			}

			// Token: 0x170000A6 RID: 166
			// (get) Token: 0x060003E4 RID: 996 RVA: 0x000052B1 File Offset: 0x000034B1
			public override bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x060003E5 RID: 997 RVA: 0x0000C262 File Offset: 0x0000A462
			public override int GetValue(ref Version container)
			{
				return container.Minor;
			}

			// Token: 0x060003E6 RID: 998 RVA: 0x00005483 File Offset: 0x00003683
			public override void SetValue(ref Version container, int value)
			{
			}
		}

		// Token: 0x020000C5 RID: 197
		private class BuildProperty : Property<Version, int>
		{
			// Token: 0x060003E7 RID: 999 RVA: 0x0000C230 File Offset: 0x0000A430
			public BuildProperty()
			{
				base.AddAttribute(new MinAttribute(0f));
			}

			// Token: 0x170000A7 RID: 167
			// (get) Token: 0x060003E8 RID: 1000 RVA: 0x0000C26B File Offset: 0x0000A46B
			public override string Name
			{
				get
				{
					return "Build";
				}
			}

			// Token: 0x170000A8 RID: 168
			// (get) Token: 0x060003E9 RID: 1001 RVA: 0x000052B1 File Offset: 0x000034B1
			public override bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x060003EA RID: 1002 RVA: 0x0000C272 File Offset: 0x0000A472
			public override int GetValue(ref Version container)
			{
				return container.Build;
			}

			// Token: 0x060003EB RID: 1003 RVA: 0x00005483 File Offset: 0x00003683
			public override void SetValue(ref Version container, int value)
			{
			}
		}

		// Token: 0x020000C6 RID: 198
		private class RevisionProperty : Property<Version, int>
		{
			// Token: 0x060003EC RID: 1004 RVA: 0x0000C230 File Offset: 0x0000A430
			public RevisionProperty()
			{
				base.AddAttribute(new MinAttribute(0f));
			}

			// Token: 0x170000A9 RID: 169
			// (get) Token: 0x060003ED RID: 1005 RVA: 0x0000C27B File Offset: 0x0000A47B
			public override string Name
			{
				get
				{
					return "Revision";
				}
			}

			// Token: 0x170000AA RID: 170
			// (get) Token: 0x060003EE RID: 1006 RVA: 0x000052B1 File Offset: 0x000034B1
			public override bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x060003EF RID: 1007 RVA: 0x0000C282 File Offset: 0x0000A482
			public override int GetValue(ref Version container)
			{
				return container.Revision;
			}

			// Token: 0x060003F0 RID: 1008 RVA: 0x00005483 File Offset: 0x00003683
			public override void SetValue(ref Version container, int value)
			{
			}
		}
	}
}
