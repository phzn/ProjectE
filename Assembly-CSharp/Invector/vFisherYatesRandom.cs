using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x02000457 RID: 1111
	public class vFisherYatesRandom
	{
		// Token: 0x06001BB0 RID: 7088 RVA: 0x000AE594 File Offset: 0x000AC794
		public int Next(int len)
		{
			if (len <= 1)
			{
				return 0;
			}
			if (this.randomIndices == null || this.randomIndices.Length != len)
			{
				this.randomIndices = new int[len];
				for (int i = 0; i < this.randomIndices.Length; i++)
				{
					this.randomIndices[i] = i;
				}
			}
			if (this.randomIndex == 0)
			{
				int num = 0;
				do
				{
					for (int j = 0; j < len - 1; j++)
					{
						int num2 = Random.Range(j, len);
						if (num2 != j)
						{
							int num3 = this.randomIndices[j];
							this.randomIndices[j] = this.randomIndices[num2];
							this.randomIndices[num2] = num3;
						}
					}
				}
				while (this.prevValue == this.randomIndices[0] && ++num < 10);
			}
			int num4 = this.randomIndices[this.randomIndex];
			int num5 = this.randomIndex + 1;
			this.randomIndex = num5;
			if (num5 >= this.randomIndices.Length)
			{
				this.randomIndex = 0;
			}
			this.prevValue = num4;
			return num4;
		}

		// Token: 0x06001BB1 RID: 7089 RVA: 0x000AE684 File Offset: 0x000AC884
		public int Range(int min, int max)
		{
			int num = max - min + 1;
			if (num <= 1)
			{
				return max;
			}
			if (this.randomIndices == null || this.randomIndices.Length != num)
			{
				this.randomIndices = new int[num];
				for (int i = 0; i < this.randomIndices.Length; i++)
				{
					this.randomIndices[i] = min + i;
				}
			}
			if (this.randomIndex == 0)
			{
				int num2 = 0;
				do
				{
					for (int j = 0; j < num - 1; j++)
					{
						int num3 = Random.Range(j, num);
						if (num3 != j)
						{
							int num4 = this.randomIndices[j];
							this.randomIndices[j] = this.randomIndices[num3];
							this.randomIndices[num3] = num4;
						}
					}
				}
				while (this.prevValue == this.randomIndices[0] && ++num2 < 10);
			}
			int num5 = this.randomIndices[this.randomIndex];
			int num6 = this.randomIndex + 1;
			this.randomIndex = num6;
			if (num6 >= this.randomIndices.Length)
			{
				this.randomIndex = 0;
			}
			this.prevValue = num5;
			return num5;
		}

		// Token: 0x04001CF4 RID: 7412
		private int[] randomIndices;

		// Token: 0x04001CF5 RID: 7413
		private int randomIndex;

		// Token: 0x04001CF6 RID: 7414
		private int prevValue = -1;
	}
}
