using System;
using System.Collections.Generic;

namespace MoreMountains.Tools
{
	// Token: 0x020000F5 RID: 245
	public class MMCircularList<T> : List<T>
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600051F RID: 1311 RVA: 0x000174CA File Offset: 0x000156CA
		// (set) Token: 0x06000520 RID: 1312 RVA: 0x000174D2 File Offset: 0x000156D2
		public int CurrentIndex
		{
			get
			{
				return this.GetCurrentIndex();
			}
			set
			{
				this._currentIndex = value;
			}
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x000174DB File Offset: 0x000156DB
		protected virtual int GetCurrentIndex()
		{
			if (this._currentIndex > base.Count - 1)
			{
				this._currentIndex = 0;
			}
			if (this._currentIndex < 0)
			{
				this._currentIndex = base.Count - 1;
			}
			return this._currentIndex;
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000522 RID: 1314 RVA: 0x00017511 File Offset: 0x00015711
		public virtual T Current
		{
			get
			{
				return base[this.CurrentIndex];
			}
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0001751F File Offset: 0x0001571F
		public virtual void IncrementCurrentIndex()
		{
			this._currentIndex++;
			this.GetCurrentIndex();
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00017536 File Offset: 0x00015736
		public virtual void DecrementCurrentIndex()
		{
			this._currentIndex--;
			this.GetCurrentIndex();
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000525 RID: 1317 RVA: 0x0001754D File Offset: 0x0001574D
		public virtual int PreviousIndex
		{
			get
			{
				if (this._currentIndex != 0)
				{
					return this._currentIndex - 1;
				}
				return base.Count - 1;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x00017568 File Offset: 0x00015768
		public virtual int NextIndex
		{
			get
			{
				if (this._currentIndex != base.Count - 1)
				{
					return this._currentIndex + 1;
				}
				return 0;
			}
		}

		// Token: 0x0400063B RID: 1595
		private int _currentIndex;
	}
}
