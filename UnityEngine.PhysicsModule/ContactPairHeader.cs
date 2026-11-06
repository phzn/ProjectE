using System;

namespace UnityEngine
{
	// Token: 0x0200003B RID: 59
	public readonly struct ContactPairHeader
	{
		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060004A5 RID: 1189 RVA: 0x00006947 File Offset: 0x00004B47
		public int BodyInstanceID
		{
			get
			{
				return this.m_BodyID;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060004A6 RID: 1190 RVA: 0x0000694F File Offset: 0x00004B4F
		public int OtherBodyInstanceID
		{
			get
			{
				return this.m_OtherBodyID;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060004A7 RID: 1191 RVA: 0x00006957 File Offset: 0x00004B57
		public Component Body
		{
			get
			{
				return Physics.GetBodyByInstanceID(this.m_BodyID);
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060004A8 RID: 1192 RVA: 0x00006964 File Offset: 0x00004B64
		public Component OtherBody
		{
			get
			{
				return Physics.GetBodyByInstanceID(this.m_OtherBodyID);
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060004A9 RID: 1193 RVA: 0x00006971 File Offset: 0x00004B71
		public int PairCount
		{
			get
			{
				return (int)this.m_NbPairs;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060004AA RID: 1194 RVA: 0x00006979 File Offset: 0x00004B79
		internal bool HasRemovedBody
		{
			get
			{
				return (this.m_Flags & CollisionPairHeaderFlags.RemovedActor) != (CollisionPairHeaderFlags)0 || (this.m_Flags & CollisionPairHeaderFlags.RemovedOtherActor) > (CollisionPairHeaderFlags)0;
			}
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x00006994 File Offset: 0x00004B94
		public readonly ref ContactPair GetContactPair(int index)
		{
			return this.GetContactPair_Internal(index);
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x000069B0 File Offset: 0x00004BB0
		internal unsafe ContactPair* GetContactPair_Internal(int index)
		{
			bool flag = (long)index >= (long)((ulong)this.m_NbPairs);
			if (flag)
			{
				throw new IndexOutOfRangeException("Invalid ContactPair index. Index should be greater than 0 and less than ContactPairHeader.PairCount");
			}
			return this.m_StartPtr.ToInt64() / (long)sizeof(ContactPair) + index * sizeof(ContactPair);
		}

		// Token: 0x040000D1 RID: 209
		internal readonly int m_BodyID;

		// Token: 0x040000D2 RID: 210
		internal readonly int m_OtherBodyID;

		// Token: 0x040000D3 RID: 211
		internal readonly IntPtr m_StartPtr;

		// Token: 0x040000D4 RID: 212
		internal readonly uint m_NbPairs;

		// Token: 0x040000D5 RID: 213
		internal readonly CollisionPairHeaderFlags m_Flags;

		// Token: 0x040000D6 RID: 214
		internal readonly Vector3 m_RelativeVelocity;
	}
}
