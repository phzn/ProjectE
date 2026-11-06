using System;

namespace Unity.Properties
{
	// Token: 0x0200001D RID: 29
	public class DelegateProperty<TContainer, TValue> : Property<TContainer, TValue>
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00003015 File Offset: 0x00001215
		public override string Name { get; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600006B RID: 107 RVA: 0x0000301D File Offset: 0x0000121D
		public override bool IsReadOnly
		{
			get
			{
				return this.m_Setter == null;
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003028 File Offset: 0x00001228
		public DelegateProperty(string name, PropertyGetter<TContainer, TValue> getter, PropertySetter<TContainer, TValue> setter = null)
		{
			this.Name = name;
			if (getter == null)
			{
				throw new ArgumentException("getter");
			}
			this.m_Getter = getter;
			this.m_Setter = setter;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003058 File Offset: 0x00001258
		public override TValue GetValue(ref TContainer container)
		{
			return this.m_Getter(ref container);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003078 File Offset: 0x00001278
		public override void SetValue(ref TContainer container, TValue value)
		{
			bool isReadOnly = this.IsReadOnly;
			if (isReadOnly)
			{
				throw new InvalidOperationException("Property is ReadOnly.");
			}
			this.m_Setter(ref container, value);
		}

		// Token: 0x0400002A RID: 42
		private readonly PropertyGetter<TContainer, TValue> m_Getter;

		// Token: 0x0400002B RID: 43
		private readonly PropertySetter<TContainer, TValue> m_Setter;
	}
}
