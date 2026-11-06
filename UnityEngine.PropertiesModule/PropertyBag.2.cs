using System;
using Unity.Properties.Internal;

namespace Unity.Properties
{
	// Token: 0x02000051 RID: 81
	public abstract class PropertyBag<TContainer> : IPropertyBag<TContainer>, IPropertyBag, IPropertyBagRegister, IConstructor<TContainer>, IConstructor
	{
		// Token: 0x06000179 RID: 377 RVA: 0x00005E7C File Offset: 0x0000407C
		static PropertyBag()
		{
			bool flag = !TypeTraits.IsContainer(typeof(TContainer));
			if (flag)
			{
				throw new InvalidOperationException(string.Format("Failed to create a property bag for Type=[{0}]. The type is not a valid container type.", typeof(TContainer)));
			}
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00005EBB File Offset: 0x000040BB
		void IPropertyBagRegister.Register()
		{
			PropertyBagStore.AddPropertyBag<TContainer>(this);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00005EC8 File Offset: 0x000040C8
		public void Accept(ITypeVisitor visitor)
		{
			bool flag = visitor == null;
			if (flag)
			{
				throw new ArgumentNullException("visitor");
			}
			visitor.Visit<TContainer>();
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00005EF4 File Offset: 0x000040F4
		void IPropertyBag.Accept(IPropertyBagVisitor visitor, ref object container)
		{
			bool flag = container == null;
			if (flag)
			{
				throw new ArgumentNullException("container");
			}
			object obj = container;
			TContainer tcontainer;
			int num;
			if (obj is TContainer)
			{
				tcontainer = (TContainer)((object)obj);
				num = 1;
			}
			else
			{
				num = 0;
			}
			bool flag2 = num == 0;
			if (flag2)
			{
				throw new ArgumentException(string.Format("The given ContainerType=[{0}] does not match the PropertyBagType=[{1}]", container.GetType(), typeof(TContainer)));
			}
			PropertyBag.AcceptWithSpecializedVisitor<TContainer>(this, visitor, ref tcontainer);
			container = tcontainer;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00005F68 File Offset: 0x00004168
		void IPropertyBag<TContainer>.Accept(IPropertyBagVisitor visitor, ref TContainer container)
		{
			visitor.Visit<TContainer>(this, ref container);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00005F74 File Offset: 0x00004174
		PropertyCollection<TContainer> IPropertyBag<TContainer>.GetProperties()
		{
			return this.GetProperties();
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00005F8C File Offset: 0x0000418C
		PropertyCollection<TContainer> IPropertyBag<TContainer>.GetProperties(ref TContainer container)
		{
			return this.GetProperties(ref container);
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000180 RID: 384 RVA: 0x00005FA5 File Offset: 0x000041A5
		InstantiationKind IConstructor.InstantiationKind
		{
			get
			{
				return this.InstantiationKind;
			}
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00005FB0 File Offset: 0x000041B0
		TContainer IConstructor<TContainer>.Instantiate()
		{
			return this.Instantiate();
		}

		// Token: 0x06000182 RID: 386
		public abstract PropertyCollection<TContainer> GetProperties();

		// Token: 0x06000183 RID: 387
		public abstract PropertyCollection<TContainer> GetProperties(ref TContainer container);

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00005FC8 File Offset: 0x000041C8
		protected virtual InstantiationKind InstantiationKind { get; } = InstantiationKind.Activator;

		// Token: 0x06000185 RID: 389 RVA: 0x00005FD0 File Offset: 0x000041D0
		protected virtual TContainer Instantiate()
		{
			return default(TContainer);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00005FEB File Offset: 0x000041EB
		public TContainer CreateInstance()
		{
			return TypeUtility.Instantiate<TContainer>();
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00005FF2 File Offset: 0x000041F2
		public bool TryCreateInstance(out TContainer instance)
		{
			return TypeUtility.TryInstantiate<TContainer>(out instance);
		}
	}
}
