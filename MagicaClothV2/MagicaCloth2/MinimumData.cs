using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MagicaCloth2
{
	// Token: 0x02000106 RID: 262
	public class MinimumData<[IsUnmanaged] T1, [IsUnmanaged] T2> where T1 : struct, ValueType where T2 : struct, ValueType
	{
		// Token: 0x060004E8 RID: 1256 RVA: 0x0002D580 File Offset: 0x0002B780
		public void Add(T1 distance, T2 data)
		{
			if (this.isValid)
			{
				if (Comparer<T1>.Default.Compare(distance, this.minDist) < 0)
				{
					this.minDist = distance;
					this.minData = data;
					return;
				}
			}
			else
			{
				this.minDist = distance;
				this.minData = data;
				this.isValid = true;
			}
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0002D5CD File Offset: 0x0002B7CD
		public void Clear()
		{
			this.isValid = false;
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060004EA RID: 1258 RVA: 0x0002D5D6 File Offset: 0x0002B7D6
		public bool IsValid
		{
			get
			{
				return this.isValid;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x0002D5DE File Offset: 0x0002B7DE
		public T1 MinDistance
		{
			get
			{
				return this.minDist;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060004EC RID: 1260 RVA: 0x0002D5E6 File Offset: 0x0002B7E6
		public T2 MinData
		{
			get
			{
				return this.minData;
			}
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0002D5EE File Offset: 0x0002B7EE
		public override string ToString()
		{
			return string.Format("MinimumData. IsValid:{0}, minDist:{1}, minData:{2}", this.isValid, this.minDist, this.minData);
		}

		// Token: 0x04000897 RID: 2199
		private T1 minDist;

		// Token: 0x04000898 RID: 2200
		private T2 minData;

		// Token: 0x04000899 RID: 2201
		private bool isValid;
	}
}
