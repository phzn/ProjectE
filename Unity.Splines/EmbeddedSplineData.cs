using System;
using Unity.Mathematics;

namespace UnityEngine.Splines
{
	// Token: 0x0200000C RID: 12
	[Serializable]
	public class EmbeddedSplineData
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00003403 File Offset: 0x00001603
		// (set) Token: 0x06000045 RID: 69 RVA: 0x0000340B File Offset: 0x0000160B
		public SplineContainer Container
		{
			get
			{
				return this.m_Container;
			}
			set
			{
				this.m_Container = value;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00003414 File Offset: 0x00001614
		// (set) Token: 0x06000047 RID: 71 RVA: 0x0000341C File Offset: 0x0000161C
		public int SplineIndex
		{
			get
			{
				return this.m_SplineIndex;
			}
			set
			{
				this.m_SplineIndex = value;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00003425 File Offset: 0x00001625
		// (set) Token: 0x06000049 RID: 73 RVA: 0x0000342D File Offset: 0x0000162D
		public EmbeddedSplineDataType Type
		{
			get
			{
				return this.m_Type;
			}
			set
			{
				this.m_Type = value;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00003436 File Offset: 0x00001636
		// (set) Token: 0x0600004B RID: 75 RVA: 0x0000343E File Offset: 0x0000163E
		public string Key
		{
			get
			{
				return this.m_Key;
			}
			set
			{
				this.m_Key = value;
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003447 File Offset: 0x00001647
		public EmbeddedSplineData()
			: this(null, EmbeddedSplineDataType.Float, null, 0)
		{
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003453 File Offset: 0x00001653
		public EmbeddedSplineData(string key, EmbeddedSplineDataType type, SplineContainer container = null, int splineIndex = 0)
		{
			this.m_Container = container;
			this.m_SplineIndex = splineIndex;
			this.m_Key = key;
			this.m_Type = type;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003478 File Offset: 0x00001678
		public bool TryGetSpline(out Spline spline)
		{
			if (this.Container == null || this.SplineIndex < 0 || this.SplineIndex >= this.Container.Splines.Count)
			{
				spline = null;
			}
			else
			{
				spline = this.Container.Splines[this.SplineIndex];
			}
			return spline != null;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000034D8 File Offset: 0x000016D8
		public bool TryGetFloatData(out SplineData<float> data)
		{
			if (this.Type != EmbeddedSplineDataType.Float)
			{
				throw new InvalidCastException(string.Format("EmbeddedSplineDataType {0} does not match requested SplineData collection: {1}", this.Type, typeof(float)));
			}
			return this.Container.Splines[this.SplineIndex].TryGetFloatData(this.Key, out data);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003538 File Offset: 0x00001738
		public bool TryGetFloat4Data(out SplineData<float4> data)
		{
			if (this.Type != EmbeddedSplineDataType.Float4)
			{
				throw new InvalidCastException(string.Format("EmbeddedSplineDataType {0} does not match requested SplineData collection: {1}", this.Type, typeof(float4)));
			}
			return this.Container.Splines[this.SplineIndex].TryGetFloat4Data(this.Key, out data);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003598 File Offset: 0x00001798
		public bool TryGetIntData(out SplineData<int> data)
		{
			if (this.Type != EmbeddedSplineDataType.Int)
			{
				throw new InvalidCastException(string.Format("EmbeddedSplineDataType {0} does not match requested SplineData collection: {1}", this.Type, typeof(int)));
			}
			return this.Container.Splines[this.SplineIndex].TryGetIntData(this.Key, out data);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000035F4 File Offset: 0x000017F4
		public bool TryGetObjectData(out SplineData<Object> data)
		{
			if (this.Type != EmbeddedSplineDataType.Object)
			{
				throw new InvalidCastException(string.Format("EmbeddedSplineDataType {0} does not match requested SplineData collection: {1}", this.Type, typeof(Object)));
			}
			return this.Container.Splines[this.SplineIndex].TryGetObjectData(this.Key, out data);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003654 File Offset: 0x00001854
		public SplineData<float> GetOrCreateFloatData()
		{
			if (this.Type != EmbeddedSplineDataType.Float)
			{
				throw new InvalidCastException(string.Format("EmbeddedSplineDataType {0} does not match requested SplineData collection: {1}", this.Type, typeof(float)));
			}
			return this.Container.Splines[this.SplineIndex].GetOrCreateFloatData(this.Key);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000036B0 File Offset: 0x000018B0
		public SplineData<float4> GetOrCreateFloat4Data()
		{
			if (this.Type != EmbeddedSplineDataType.Float4)
			{
				throw new InvalidCastException(string.Format("EmbeddedSplineDataType {0} does not match requested SplineData collection: {1}", this.Type, typeof(float4)));
			}
			return this.Container.Splines[this.SplineIndex].GetOrCreateFloat4Data(this.Key);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x0000370C File Offset: 0x0000190C
		public SplineData<int> GetOrCreateIntData()
		{
			if (this.Type != EmbeddedSplineDataType.Int)
			{
				throw new InvalidCastException(string.Format("EmbeddedSplineDataType {0} does not match requested SplineData collection: {1}", this.Type, typeof(int)));
			}
			return this.Container.Splines[this.SplineIndex].GetOrCreateIntData(this.Key);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003768 File Offset: 0x00001968
		public SplineData<Object> GetOrCreateObjectData()
		{
			if (this.Type != EmbeddedSplineDataType.Object)
			{
				throw new InvalidCastException(string.Format("EmbeddedSplineDataType {0} does not match requested SplineData collection: {1}", this.Type, typeof(Object)));
			}
			return this.Container.Splines[this.SplineIndex].GetOrCreateObjectData(this.Key);
		}

		// Token: 0x04000017 RID: 23
		[SerializeField]
		private SplineContainer m_Container;

		// Token: 0x04000018 RID: 24
		[SerializeField]
		private int m_SplineIndex;

		// Token: 0x04000019 RID: 25
		[SerializeField]
		private EmbeddedSplineDataType m_Type;

		// Token: 0x0400001A RID: 26
		[SerializeField]
		private string m_Key;
	}
}
