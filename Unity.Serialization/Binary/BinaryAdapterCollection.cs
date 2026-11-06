using System;
using System.Collections.Generic;

namespace Unity.Serialization.Binary
{
	// Token: 0x0200009F RID: 159
	internal struct BinaryAdapterCollection
	{
		// Token: 0x060003FF RID: 1023 RVA: 0x0000FFE7 File Offset: 0x0000E1E7
		public BinaryAdapterCollection.Enumerator GetEnumerator()
		{
			return new BinaryAdapterCollection.Enumerator(this.UserDefined, this.Global, this.Internal);
		}

		// Token: 0x040001E1 RID: 481
		public List<IBinaryAdapter> UserDefined;

		// Token: 0x040001E2 RID: 482
		public List<IBinaryAdapter> Global;

		// Token: 0x040001E3 RID: 483
		public BinaryAdapter Internal;

		// Token: 0x020000A0 RID: 160
		public struct Enumerator
		{
			// Token: 0x06000400 RID: 1024 RVA: 0x00010000 File Offset: 0x0000E200
			public Enumerator(List<IBinaryAdapter> user, List<IBinaryAdapter> global, IBinaryAdapter @internal)
			{
				this.m_User = user;
				this.m_Global = global;
				this.m_Internal = @internal;
				this.m_Current = null;
				this.m_State = ((user != null) ? BinaryAdapterCollection.Enumerator.State.User : ((global != null) ? BinaryAdapterCollection.Enumerator.State.Global : BinaryAdapterCollection.Enumerator.State.Internal));
				this.m_Index = -1;
			}

			// Token: 0x17000074 RID: 116
			// (get) Token: 0x06000401 RID: 1025 RVA: 0x00010038 File Offset: 0x0000E238
			public IBinaryAdapter Current
			{
				get
				{
					return this.m_Current;
				}
			}

			// Token: 0x06000402 RID: 1026 RVA: 0x00010040 File Offset: 0x0000E240
			public bool MoveNext()
			{
				for (;;)
				{
					this.m_Index++;
					switch (this.m_State)
					{
					case BinaryAdapterCollection.Enumerator.State.User:
						if (this.m_Index < this.m_User.Count)
						{
							goto Block_1;
						}
						this.m_State = BinaryAdapterCollection.Enumerator.State.Global;
						this.m_Index = -1;
						continue;
					case BinaryAdapterCollection.Enumerator.State.Global:
						if (this.m_Index < this.m_Global.Count)
						{
							goto Block_2;
						}
						this.m_State = BinaryAdapterCollection.Enumerator.State.Internal;
						this.m_Index = -1;
						continue;
					case BinaryAdapterCollection.Enumerator.State.Internal:
						goto IL_00AB;
					case BinaryAdapterCollection.Enumerator.State.End:
						goto IL_00C7;
					}
					break;
				}
				throw new ArgumentOutOfRangeException();
				Block_1:
				this.m_Current = this.m_User[this.m_Index];
				return true;
				Block_2:
				this.m_Current = this.m_Global[this.m_Index];
				return true;
				IL_00AB:
				this.m_Current = this.m_Internal;
				this.m_State = BinaryAdapterCollection.Enumerator.State.End;
				this.m_Index = -1;
				return true;
				IL_00C7:
				this.m_Index = -1;
				return false;
			}

			// Token: 0x040001E4 RID: 484
			private readonly List<IBinaryAdapter> m_User;

			// Token: 0x040001E5 RID: 485
			private readonly List<IBinaryAdapter> m_Global;

			// Token: 0x040001E6 RID: 486
			private readonly IBinaryAdapter m_Internal;

			// Token: 0x040001E7 RID: 487
			private IBinaryAdapter m_Current;

			// Token: 0x040001E8 RID: 488
			private BinaryAdapterCollection.Enumerator.State m_State;

			// Token: 0x040001E9 RID: 489
			private int m_Index;

			// Token: 0x020000A1 RID: 161
			private enum State
			{
				// Token: 0x040001EB RID: 491
				User,
				// Token: 0x040001EC RID: 492
				Global,
				// Token: 0x040001ED RID: 493
				Internal,
				// Token: 0x040001EE RID: 494
				End
			}
		}
	}
}
