using System;
using Unity.Properties;

namespace Unity.Serialization
{
	// Token: 0x02000012 RID: 18
	internal struct PropertyWrapper<T> : IPropertyWrapper
	{
		// Token: 0x06000023 RID: 35 RVA: 0x000025E6 File Offset: 0x000007E6
		public PropertyWrapper(T value)
		{
			if (!Unity.Properties.PropertyBag.Exists<PropertyWrapper<T>>())
			{
				Unity.Properties.PropertyBag.Register<PropertyWrapper<T>>(new PropertyWrapper<T>.PropertyBag());
			}
			this.Value = value;
		}

		// Token: 0x0400000E RID: 14
		public T Value;

		// Token: 0x02000013 RID: 19
		private class PropertyBag : ContainerPropertyBag<PropertyWrapper<T>>, IPropertyWrapper
		{
			// Token: 0x17000002 RID: 2
			// (get) Token: 0x06000024 RID: 36 RVA: 0x00002600 File Offset: 0x00000800
			private PropertyWrapper<T>.Property Property { get; } = new PropertyWrapper<T>.Property();

			// Token: 0x06000025 RID: 37 RVA: 0x00002608 File Offset: 0x00000808
			public PropertyBag()
			{
				base.AddProperty<T>(this.Property);
			}
		}

		// Token: 0x02000014 RID: 20
		private class Property : Property<PropertyWrapper<T>, T>, IPropertyWrapper
		{
			// Token: 0x17000003 RID: 3
			// (get) Token: 0x06000026 RID: 38 RVA: 0x00002627 File Offset: 0x00000827
			public override string Name
			{
				get
				{
					return "Value";
				}
			}

			// Token: 0x17000004 RID: 4
			// (get) Token: 0x06000027 RID: 39 RVA: 0x0000262E File Offset: 0x0000082E
			public override bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06000028 RID: 40 RVA: 0x00002631 File Offset: 0x00000831
			public override T GetValue(ref PropertyWrapper<T> container)
			{
				return container.Value;
			}

			// Token: 0x06000029 RID: 41 RVA: 0x00002639 File Offset: 0x00000839
			public override void SetValue(ref PropertyWrapper<T> container, T value)
			{
				container.Value = value;
			}
		}
	}
}
