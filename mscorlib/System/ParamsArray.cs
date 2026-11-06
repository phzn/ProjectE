using System;

namespace System
{
	// Token: 0x0200016D RID: 365
	internal readonly struct ParamsArray
	{
		// Token: 0x06000E73 RID: 3699 RVA: 0x0003AE52 File Offset: 0x00039052
		public ParamsArray(object arg0)
		{
			this._arg0 = arg0;
			this._arg1 = null;
			this._arg2 = null;
			this._args = ParamsArray.s_oneArgArray;
		}

		// Token: 0x06000E74 RID: 3700 RVA: 0x0003AE74 File Offset: 0x00039074
		public ParamsArray(object arg0, object arg1)
		{
			this._arg0 = arg0;
			this._arg1 = arg1;
			this._arg2 = null;
			this._args = ParamsArray.s_twoArgArray;
		}

		// Token: 0x06000E75 RID: 3701 RVA: 0x0003AE96 File Offset: 0x00039096
		public ParamsArray(object arg0, object arg1, object arg2)
		{
			this._arg0 = arg0;
			this._arg1 = arg1;
			this._arg2 = arg2;
			this._args = ParamsArray.s_threeArgArray;
		}

		// Token: 0x06000E76 RID: 3702 RVA: 0x0003AEB8 File Offset: 0x000390B8
		public ParamsArray(object[] args)
		{
			int num = args.Length;
			this._arg0 = ((num > 0) ? args[0] : null);
			this._arg1 = ((num > 1) ? args[1] : null);
			this._arg2 = ((num > 2) ? args[2] : null);
			this._args = args;
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000E77 RID: 3703 RVA: 0x0003AF00 File Offset: 0x00039100
		public int Length
		{
			get
			{
				return this._args.Length;
			}
		}

		// Token: 0x17000105 RID: 261
		public object this[int index]
		{
			get
			{
				if (index != 0)
				{
					return this.GetAtSlow(index);
				}
				return this._arg0;
			}
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x0003AF1D File Offset: 0x0003911D
		private object GetAtSlow(int index)
		{
			if (index == 1)
			{
				return this._arg1;
			}
			if (index == 2)
			{
				return this._arg2;
			}
			return this._args[index];
		}

		// Token: 0x040012A8 RID: 4776
		private static readonly object[] s_oneArgArray = new object[1];

		// Token: 0x040012A9 RID: 4777
		private static readonly object[] s_twoArgArray = new object[2];

		// Token: 0x040012AA RID: 4778
		private static readonly object[] s_threeArgArray = new object[3];

		// Token: 0x040012AB RID: 4779
		private readonly object _arg0;

		// Token: 0x040012AC RID: 4780
		private readonly object _arg1;

		// Token: 0x040012AD RID: 4781
		private readonly object _arg2;

		// Token: 0x040012AE RID: 4782
		private readonly object[] _args;
	}
}
