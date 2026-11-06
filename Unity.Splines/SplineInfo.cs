using System;
using Unity.Mathematics;

namespace UnityEngine.Splines
{
	// Token: 0x02000029 RID: 41
	[Serializable]
	public struct SplineInfo : IEquatable<SplineInfo>, ISerializationCallbackReceiver
	{
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x00009542 File Offset: 0x00007742
		public Object Object
		{
			get
			{
				return this.m_Object;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x0000954A File Offset: 0x0000774A
		// (set) Token: 0x060001F8 RID: 504 RVA: 0x00009561 File Offset: 0x00007761
		public ISplineContainer Container
		{
			get
			{
				return this.m_Container ?? (this.m_Object as ISplineContainer);
			}
			set
			{
				this.m_Container = value;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x0000956C File Offset: 0x0000776C
		public Transform Transform
		{
			get
			{
				Component component = this.Object as Component;
				if (component == null)
				{
					return null;
				}
				return component.transform;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060001FA RID: 506 RVA: 0x00009590 File Offset: 0x00007790
		public Spline Spline
		{
			get
			{
				if (this.Container == null || this.Index <= -1 || this.Index >= this.Container.Splines.Count)
				{
					return null;
				}
				return this.Container.Splines[this.Index];
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060001FB RID: 507 RVA: 0x000095DE File Offset: 0x000077DE
		// (set) Token: 0x060001FC RID: 508 RVA: 0x000095E6 File Offset: 0x000077E6
		public int Index
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

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060001FD RID: 509 RVA: 0x000095EF File Offset: 0x000077EF
		public float4x4 LocalToWorld
		{
			get
			{
				if (!(this.Transform != null))
				{
					return float4x4.identity;
				}
				return this.Transform.localToWorldMatrix;
			}
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00009615 File Offset: 0x00007815
		public SplineInfo(ISplineContainer container, int index)
		{
			this.m_Container = container;
			this.m_Object = container as Object;
			this.m_SplineIndex = index;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00009631 File Offset: 0x00007831
		public bool Equals(SplineInfo other)
		{
			return object.Equals(this.Container, other.Container) && this.Index == other.Index;
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00009658 File Offset: 0x00007858
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is SplineInfo)
			{
				SplineInfo splineInfo = (SplineInfo)obj;
				return this.Equals(splineInfo);
			}
			return false;
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00009682 File Offset: 0x00007882
		public override int GetHashCode()
		{
			return (((this.Container != null) ? this.Container.GetHashCode() : 0) * 397) ^ this.Index;
		}

		// Token: 0x06000202 RID: 514 RVA: 0x000096A8 File Offset: 0x000078A8
		public void OnBeforeSerialize()
		{
			Object @object = this.m_Container as Object;
			if (@object != null)
			{
				this.m_Object = @object;
				this.m_Container = null;
			}
		}

		// Token: 0x06000203 RID: 515 RVA: 0x000096D2 File Offset: 0x000078D2
		public void OnAfterDeserialize()
		{
			if (this.m_Container == null)
			{
				this.m_Container = this.m_Object as ISplineContainer;
			}
		}

		// Token: 0x04000099 RID: 153
		[SerializeField]
		private Object m_Object;

		// Token: 0x0400009A RID: 154
		[SerializeReference]
		private ISplineContainer m_Container;

		// Token: 0x0400009B RID: 155
		[SerializeField]
		private int m_SplineIndex;
	}
}
