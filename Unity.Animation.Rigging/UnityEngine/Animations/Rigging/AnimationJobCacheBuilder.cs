using System;
using System.Collections.Generic;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000007 RID: 7
	public class AnimationJobCacheBuilder
	{
		// Token: 0x0600000C RID: 12 RVA: 0x00002205 File Offset: 0x00000405
		public AnimationJobCacheBuilder()
		{
			this.m_Data = new List<float>();
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002218 File Offset: 0x00000418
		public CacheIndex Add(float v)
		{
			this.m_Data.Add(v);
			return new CacheIndex
			{
				idx = this.m_Data.Count - 1
			};
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002250 File Offset: 0x00000450
		public CacheIndex Add(Vector2 v)
		{
			this.m_Data.Add(v.x);
			this.m_Data.Add(v.y);
			return new CacheIndex
			{
				idx = this.m_Data.Count - 2
			};
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000229C File Offset: 0x0000049C
		public CacheIndex Add(Vector3 v)
		{
			this.m_Data.Add(v.x);
			this.m_Data.Add(v.y);
			this.m_Data.Add(v.z);
			return new CacheIndex
			{
				idx = this.m_Data.Count - 3
			};
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000022FC File Offset: 0x000004FC
		public CacheIndex Add(Vector4 v)
		{
			this.m_Data.Add(v.x);
			this.m_Data.Add(v.y);
			this.m_Data.Add(v.z);
			this.m_Data.Add(v.w);
			return new CacheIndex
			{
				idx = this.m_Data.Count - 4
			};
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0000236A File Offset: 0x0000056A
		public CacheIndex Add(Quaternion v)
		{
			return this.Add(new Vector4(v.x, v.y, v.z, v.w));
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002390 File Offset: 0x00000590
		public CacheIndex Add(AffineTransform tx)
		{
			this.Add(tx.translation);
			this.Add(tx.rotation);
			return new CacheIndex
			{
				idx = this.m_Data.Count - 7
			};
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000023D4 File Offset: 0x000005D4
		public CacheIndex AllocateChunk(int size)
		{
			this.m_Data.AddRange(new float[size]);
			return new CacheIndex
			{
				idx = this.m_Data.Count - size
			};
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000240F File Offset: 0x0000060F
		public void SetValue(CacheIndex index, int offset, float value)
		{
			if (index.idx + offset < this.m_Data.Count)
			{
				this.m_Data[index.idx + offset] = value;
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000243A File Offset: 0x0000063A
		public AnimationJobCache Build()
		{
			return new AnimationJobCache(this.m_Data.ToArray());
		}

		// Token: 0x04000003 RID: 3
		private List<float> m_Data;
	}
}
