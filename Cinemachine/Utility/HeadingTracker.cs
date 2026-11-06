using System;
using UnityEngine;

namespace Cinemachine.Utility
{
	// Token: 0x02000066 RID: 102
	public class HeadingTracker
	{
		// Token: 0x060003E1 RID: 993 RVA: 0x00017668 File Offset: 0x00015868
		public HeadingTracker(int filterSize)
		{
			this.mHistory = new HeadingTracker.Item[filterSize];
			float num = (float)filterSize / 5f;
			HeadingTracker.mDecayExponent = -Mathf.Log(2f) / num;
			this.ClearHistory();
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060003E2 RID: 994 RVA: 0x000176B3 File Offset: 0x000158B3
		public int FilterSize
		{
			get
			{
				return this.mHistory.Length;
			}
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x000176C0 File Offset: 0x000158C0
		private void ClearHistory()
		{
			this.mTop = (this.mBottom = (this.mCount = 0));
			this.mWeightSum = 0f;
			this.mHeadingSum = Vector3.zero;
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x000176FC File Offset: 0x000158FC
		private static float Decay(float time)
		{
			return Mathf.Exp(time * HeadingTracker.mDecayExponent);
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0001770C File Offset: 0x0001590C
		public void Add(Vector3 velocity)
		{
			if (this.FilterSize == 0)
			{
				this.mLastGoodHeading = velocity;
				return;
			}
			float magnitude = velocity.magnitude;
			if (magnitude > 0.0001f)
			{
				HeadingTracker.Item item = default(HeadingTracker.Item);
				item.velocity = velocity;
				item.weight = magnitude;
				item.time = CinemachineCore.CurrentTime;
				if (this.mCount == this.FilterSize)
				{
					this.PopBottom();
				}
				this.mCount++;
				this.mHistory[this.mTop] = item;
				int num = this.mTop + 1;
				this.mTop = num;
				if (num == this.FilterSize)
				{
					this.mTop = 0;
				}
				this.mWeightSum *= HeadingTracker.Decay(item.time - this.mWeightTime);
				this.mWeightTime = item.time;
				this.mWeightSum += magnitude;
				this.mHeadingSum += item.velocity;
			}
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00017804 File Offset: 0x00015A04
		private void PopBottom()
		{
			if (this.mCount > 0)
			{
				float currentTime = CinemachineCore.CurrentTime;
				HeadingTracker.Item item = this.mHistory[this.mBottom];
				int num = this.mBottom + 1;
				this.mBottom = num;
				if (num == this.FilterSize)
				{
					this.mBottom = 0;
				}
				this.mCount--;
				float num2 = HeadingTracker.Decay(currentTime - item.time);
				this.mWeightSum -= item.weight * num2;
				this.mHeadingSum -= item.velocity * num2;
				if (this.mWeightSum <= 0.0001f || this.mCount == 0)
				{
					this.ClearHistory();
				}
			}
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x000178BC File Offset: 0x00015ABC
		public void DecayHistory()
		{
			float currentTime = CinemachineCore.CurrentTime;
			float num = HeadingTracker.Decay(currentTime - this.mWeightTime);
			this.mWeightSum *= num;
			this.mWeightTime = currentTime;
			if (this.mWeightSum < 0.0001f)
			{
				this.ClearHistory();
				return;
			}
			this.mHeadingSum *= num;
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x00017918 File Offset: 0x00015B18
		public Vector3 GetReliableHeading()
		{
			if (this.mWeightSum > 0.0001f && (this.mCount == this.mHistory.Length || this.mLastGoodHeading.AlmostZero()))
			{
				Vector3 vector = this.mHeadingSum / this.mWeightSum;
				if (!vector.AlmostZero())
				{
					this.mLastGoodHeading = vector.normalized;
				}
			}
			return this.mLastGoodHeading;
		}

		// Token: 0x0400029A RID: 666
		private HeadingTracker.Item[] mHistory;

		// Token: 0x0400029B RID: 667
		private int mTop;

		// Token: 0x0400029C RID: 668
		private int mBottom;

		// Token: 0x0400029D RID: 669
		private int mCount;

		// Token: 0x0400029E RID: 670
		private Vector3 mHeadingSum;

		// Token: 0x0400029F RID: 671
		private float mWeightSum;

		// Token: 0x040002A0 RID: 672
		private float mWeightTime;

		// Token: 0x040002A1 RID: 673
		private Vector3 mLastGoodHeading = Vector3.zero;

		// Token: 0x040002A2 RID: 674
		private static float mDecayExponent;

		// Token: 0x020000E9 RID: 233
		private struct Item
		{
			// Token: 0x0400049D RID: 1181
			public Vector3 velocity;

			// Token: 0x0400049E RID: 1182
			public float weight;

			// Token: 0x0400049F RID: 1183
			public float time;
		}
	}
}
