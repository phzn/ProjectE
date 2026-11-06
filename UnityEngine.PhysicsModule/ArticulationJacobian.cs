using System;
using System.Collections.Generic;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200001C RID: 28
	[NativeHeader("Modules/Physics/ArticulationBody.h")]
	public struct ArticulationJacobian
	{
		// Token: 0x0600005C RID: 92 RVA: 0x00002ABC File Offset: 0x00000CBC
		public ArticulationJacobian(int rows, int cols)
		{
			this.rowsCount = rows;
			this.colsCount = cols;
			this.matrixData = new List<float>(rows * cols);
			for (int i = 0; i < rows * cols; i++)
			{
				this.matrixData.Add(0f);
			}
		}

		// Token: 0x17000034 RID: 52
		public float this[int row, int col]
		{
			get
			{
				bool flag = row < 0 || row >= this.rowsCount;
				if (flag)
				{
					throw new IndexOutOfRangeException();
				}
				bool flag2 = col < 0 || col >= this.colsCount;
				if (flag2)
				{
					throw new IndexOutOfRangeException();
				}
				return this.matrixData[row * this.colsCount + col];
			}
			set
			{
				bool flag = row < 0 || row >= this.rowsCount;
				if (flag)
				{
					throw new IndexOutOfRangeException();
				}
				bool flag2 = col < 0 || col >= this.colsCount;
				if (flag2)
				{
					throw new IndexOutOfRangeException();
				}
				this.matrixData[row * this.colsCount + col] = value;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00002BD0 File Offset: 0x00000DD0
		// (set) Token: 0x06000060 RID: 96 RVA: 0x00002BE8 File Offset: 0x00000DE8
		public int rows
		{
			get
			{
				return this.rowsCount;
			}
			set
			{
				this.rowsCount = value;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00002BF4 File Offset: 0x00000DF4
		// (set) Token: 0x06000062 RID: 98 RVA: 0x00002C0C File Offset: 0x00000E0C
		public int columns
		{
			get
			{
				return this.colsCount;
			}
			set
			{
				this.colsCount = value;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00002C18 File Offset: 0x00000E18
		// (set) Token: 0x06000064 RID: 100 RVA: 0x00002C30 File Offset: 0x00000E30
		public List<float> elements
		{
			get
			{
				return this.matrixData;
			}
			set
			{
				this.matrixData = value;
			}
		}

		// Token: 0x0400007C RID: 124
		private int rowsCount;

		// Token: 0x0400007D RID: 125
		private int colsCount;

		// Token: 0x0400007E RID: 126
		private List<float> matrixData;
	}
}
