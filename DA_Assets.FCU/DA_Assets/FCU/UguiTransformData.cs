using System;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x02000046 RID: 70
	[Serializable]
	public struct UguiTransformData
	{
		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600018B RID: 395 RVA: 0x0000401F File Offset: 0x0000221F
		// (set) Token: 0x0600018C RID: 396 RVA: 0x00004027 File Offset: 0x00002227
		public Vector2 AnchorMin
		{
			get
			{
				return this.anchorMin;
			}
			set
			{
				this.anchorMin = value;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600018D RID: 397 RVA: 0x00004030 File Offset: 0x00002230
		// (set) Token: 0x0600018E RID: 398 RVA: 0x00004038 File Offset: 0x00002238
		public Vector2 AnchorMax
		{
			get
			{
				return this.anchorMax;
			}
			set
			{
				this.anchorMax = value;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600018F RID: 399 RVA: 0x00004041 File Offset: 0x00002241
		// (set) Token: 0x06000190 RID: 400 RVA: 0x00004049 File Offset: 0x00002249
		public Vector2 AnchoredPosition
		{
			get
			{
				return this.anchoredPosition;
			}
			set
			{
				this.anchoredPosition = value;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000191 RID: 401 RVA: 0x00004052 File Offset: 0x00002252
		// (set) Token: 0x06000192 RID: 402 RVA: 0x0000405A File Offset: 0x0000225A
		public Vector3 SizeDelta
		{
			get
			{
				return this.sizeDelta;
			}
			set
			{
				this.sizeDelta = value;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000193 RID: 403 RVA: 0x00004063 File Offset: 0x00002263
		// (set) Token: 0x06000194 RID: 404 RVA: 0x0000406B File Offset: 0x0000226B
		public Quaternion LocalRotation
		{
			get
			{
				return this.localRotation;
			}
			set
			{
				this.localRotation = value;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000195 RID: 405 RVA: 0x00004074 File Offset: 0x00002274
		// (set) Token: 0x06000196 RID: 406 RVA: 0x0000407C File Offset: 0x0000227C
		public Vector3 LocalPosition
		{
			get
			{
				return this.localPosition;
			}
			set
			{
				this.localPosition = value;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000197 RID: 407 RVA: 0x00004085 File Offset: 0x00002285
		// (set) Token: 0x06000198 RID: 408 RVA: 0x0000408D File Offset: 0x0000228D
		public Vector3 LocalScale
		{
			get
			{
				return this.localScale;
			}
			set
			{
				this.localScale = value;
			}
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00004098 File Offset: 0x00002298
		private UguiTransformData(RectTransform source)
		{
			this.anchorMin = source.anchorMin;
			this.anchorMax = source.anchorMax;
			this.anchoredPosition = source.anchoredPosition;
			this.sizeDelta = source.sizeDelta;
			this.localRotation = source.localRotation;
			this.localScale = source.localScale;
			this.localPosition = source.localPosition;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x000040FE File Offset: 0x000022FE
		public static UguiTransformData Create(RectTransform source)
		{
			return new UguiTransformData(source);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00004108 File Offset: 0x00002308
		public void ApplyTo(RectTransform target)
		{
			target.anchorMin = this.anchorMin;
			target.anchorMax = this.anchorMax;
			target.anchoredPosition = this.anchoredPosition;
			target.sizeDelta = this.sizeDelta;
			target.localRotation = this.localRotation;
			target.localScale = this.localScale;
		}

		// Token: 0x04000365 RID: 869
		[SerializeField]
		private Vector2 anchorMin;

		// Token: 0x04000366 RID: 870
		[SerializeField]
		private Vector2 anchorMax;

		// Token: 0x04000367 RID: 871
		[SerializeField]
		private Vector2 anchoredPosition;

		// Token: 0x04000368 RID: 872
		[SerializeField]
		private Vector3 sizeDelta;

		// Token: 0x04000369 RID: 873
		[SerializeField]
		private Quaternion localRotation;

		// Token: 0x0400036A RID: 874
		[SerializeField]
		private Vector3 localPosition;

		// Token: 0x0400036B RID: 875
		[SerializeField]
		private Vector3 localScale;
	}
}
