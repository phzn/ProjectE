using System;
using UnityEngine.Serialization;

namespace UnityEngine.Splines
{
	// Token: 0x0200000A RID: 10
	[Serializable]
	public struct DataPoint<TDataType> : IComparable<DataPoint<TDataType>>, IComparable<float>, IDataPoint
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00003333 File Offset: 0x00001533
		// (set) Token: 0x0600003B RID: 59 RVA: 0x0000333B File Offset: 0x0000153B
		public float Index
		{
			get
			{
				return this.m_Index;
			}
			set
			{
				this.m_Index = value;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00003344 File Offset: 0x00001544
		// (set) Token: 0x0600003D RID: 61 RVA: 0x0000334C File Offset: 0x0000154C
		public TDataType Value
		{
			get
			{
				return this.m_Value;
			}
			set
			{
				this.m_Value = value;
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003355 File Offset: 0x00001555
		public DataPoint(float index, TDataType value)
		{
			this.m_Index = index;
			this.m_Value = value;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003368 File Offset: 0x00001568
		public int CompareTo(DataPoint<TDataType> other)
		{
			return this.Index.CompareTo(other.Index);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x0000338C File Offset: 0x0000158C
		public int CompareTo(float other)
		{
			return this.Index.CompareTo(other);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000033A8 File Offset: 0x000015A8
		public override string ToString()
		{
			return string.Format("{0} {1}", this.Index, this.Value);
		}

		// Token: 0x04000015 RID: 21
		[FormerlySerializedAs("m_Time")]
		[SerializeField]
		private float m_Index;

		// Token: 0x04000016 RID: 22
		[SerializeField]
		private TDataType m_Value;
	}
}
