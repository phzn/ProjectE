using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020000EE RID: 238
	public abstract class VolumeParameter : ICloneable
	{
		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060007DC RID: 2012 RVA: 0x00026917 File Offset: 0x00024B17
		// (set) Token: 0x060007DD RID: 2013 RVA: 0x0002691F File Offset: 0x00024B1F
		public virtual bool overrideState
		{
			get
			{
				return this.m_OverrideState;
			}
			set
			{
				this.m_OverrideState = value;
			}
		}

		// Token: 0x060007DE RID: 2014
		internal abstract void Interp(VolumeParameter from, VolumeParameter to, float t);

		// Token: 0x060007DF RID: 2015 RVA: 0x00026928 File Offset: 0x00024B28
		public T GetValue<T>()
		{
			return ((VolumeParameter<T>)this).value;
		}

		// Token: 0x060007E0 RID: 2016
		public abstract void SetValue(VolumeParameter parameter);

		// Token: 0x060007E1 RID: 2017 RVA: 0x00026935 File Offset: 0x00024B35
		protected internal virtual void OnEnable()
		{
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x00026937 File Offset: 0x00024B37
		protected internal virtual void OnDisable()
		{
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x00026939 File Offset: 0x00024B39
		public static bool IsObjectParameter(Type type)
		{
			return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ObjectParameter<>)) || (type.BaseType != null && VolumeParameter.IsObjectParameter(type.BaseType));
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x00026977 File Offset: 0x00024B77
		public virtual void Release()
		{
		}

		// Token: 0x060007E5 RID: 2021
		public abstract object Clone();

		// Token: 0x040004D2 RID: 1234
		public const string k_DebuggerDisplay = "{m_Value} ({m_OverrideState})";

		// Token: 0x040004D3 RID: 1235
		[SerializeField]
		protected bool m_OverrideState;
	}
}
