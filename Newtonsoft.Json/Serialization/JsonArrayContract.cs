using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000080 RID: 128
	[NullableContext(2)]
	[Nullable(0)]
	public class JsonArrayContract : JsonContainerContract
	{
		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x0600066C RID: 1644 RVA: 0x0001AF4D File Offset: 0x0001914D
		public Type CollectionItemType { get; }

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x0600066D RID: 1645 RVA: 0x0001AF55 File Offset: 0x00019155
		public bool IsMultidimensionalArray { get; }

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600066E RID: 1646 RVA: 0x0001AF5D File Offset: 0x0001915D
		internal bool IsArray { get; }

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x0600066F RID: 1647 RVA: 0x0001AF65 File Offset: 0x00019165
		internal bool ShouldCreateWrapper { get; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000670 RID: 1648 RVA: 0x0001AF6D File Offset: 0x0001916D
		// (set) Token: 0x06000671 RID: 1649 RVA: 0x0001AF75 File Offset: 0x00019175
		internal bool CanDeserialize { get; private set; }

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000672 RID: 1650 RVA: 0x0001AF7E File Offset: 0x0001917E
		[Nullable(new byte[] { 2, 1 })]
		internal ObjectConstructor<object> ParameterizedCreator
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get
			{
				if (this._parameterizedCreator == null && this._parameterizedConstructor != null)
				{
					this._parameterizedCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(this._parameterizedConstructor);
				}
				return this._parameterizedCreator;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000673 RID: 1651 RVA: 0x0001AFB2 File Offset: 0x000191B2
		// (set) Token: 0x06000674 RID: 1652 RVA: 0x0001AFBA File Offset: 0x000191BA
		[Nullable(new byte[] { 2, 1 })]
		public ObjectConstructor<object> OverrideCreator
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get
			{
				return this._overrideCreator;
			}
			[param: Nullable(new byte[] { 2, 1 })]
			set
			{
				this._overrideCreator = value;
				this.CanDeserialize = true;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000675 RID: 1653 RVA: 0x0001AFCA File Offset: 0x000191CA
		// (set) Token: 0x06000676 RID: 1654 RVA: 0x0001AFD2 File Offset: 0x000191D2
		public bool HasParameterizedCreator { get; set; }

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000677 RID: 1655 RVA: 0x0001AFDB File Offset: 0x000191DB
		internal bool HasParameterizedCreatorInternal
		{
			get
			{
				return this.HasParameterizedCreator || this._parameterizedCreator != null || this._parameterizedConstructor != null;
			}
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x0001AFFC File Offset: 0x000191FC
		[NullableContext(1)]
		public JsonArrayContract(Type underlyingType)
			: base(underlyingType)
		{
			this.ContractType = JsonContractType.Array;
			this.IsArray = base.CreatedType.IsArray || (this.NonNullableUnderlyingType.IsGenericType() && this.NonNullableUnderlyingType.GetGenericTypeDefinition().FullName == "System.Linq.EmptyPartition`1");
			bool flag;
			Type type;
			if (this.IsArray)
			{
				this.CollectionItemType = ReflectionUtils.GetCollectionItemType(base.UnderlyingType);
				this.IsReadOnlyOrFixedSize = true;
				this._genericCollectionDefinitionType = typeof(List<>).MakeGenericType(new Type[] { this.CollectionItemType });
				flag = true;
				this.IsMultidimensionalArray = base.CreatedType.IsArray && base.UnderlyingType.GetArrayRank() > 1;
			}
			else if (typeof(IList).IsAssignableFrom(this.NonNullableUnderlyingType))
			{
				if (ReflectionUtils.ImplementsGenericDefinition(this.NonNullableUnderlyingType, typeof(ICollection<>), out this._genericCollectionDefinitionType))
				{
					this.CollectionItemType = this._genericCollectionDefinitionType.GetGenericArguments()[0];
				}
				else
				{
					this.CollectionItemType = ReflectionUtils.GetCollectionItemType(this.NonNullableUnderlyingType);
				}
				if (this.NonNullableUnderlyingType == typeof(IList))
				{
					base.CreatedType = typeof(List<object>);
				}
				if (this.CollectionItemType != null)
				{
					this._parameterizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(this.NonNullableUnderlyingType, this.CollectionItemType);
				}
				this.IsReadOnlyOrFixedSize = ReflectionUtils.InheritsGenericDefinition(this.NonNullableUnderlyingType, typeof(ReadOnlyCollection<>));
				flag = true;
			}
			else if (ReflectionUtils.ImplementsGenericDefinition(this.NonNullableUnderlyingType, typeof(ICollection<>), out this._genericCollectionDefinitionType))
			{
				this.CollectionItemType = this._genericCollectionDefinitionType.GetGenericArguments()[0];
				if (ReflectionUtils.IsGenericDefinition(this.NonNullableUnderlyingType, typeof(ICollection<>)) || ReflectionUtils.IsGenericDefinition(this.NonNullableUnderlyingType, typeof(IList<>)))
				{
					base.CreatedType = typeof(List<>).MakeGenericType(new Type[] { this.CollectionItemType });
				}
				if (ReflectionUtils.IsGenericDefinition(this.NonNullableUnderlyingType, typeof(ISet<>)))
				{
					base.CreatedType = typeof(HashSet<>).MakeGenericType(new Type[] { this.CollectionItemType });
				}
				this._parameterizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(this.NonNullableUnderlyingType, this.CollectionItemType);
				flag = true;
				this.ShouldCreateWrapper = true;
			}
			else if (ReflectionUtils.ImplementsGenericDefinition(this.NonNullableUnderlyingType, typeof(IReadOnlyCollection<>), out type))
			{
				this.CollectionItemType = type.GetGenericArguments()[0];
				if (ReflectionUtils.IsGenericDefinition(this.NonNullableUnderlyingType, typeof(IReadOnlyCollection<>)) || ReflectionUtils.IsGenericDefinition(this.NonNullableUnderlyingType, typeof(IReadOnlyList<>)))
				{
					base.CreatedType = typeof(ReadOnlyCollection<>).MakeGenericType(new Type[] { this.CollectionItemType });
				}
				this._genericCollectionDefinitionType = typeof(List<>).MakeGenericType(new Type[] { this.CollectionItemType });
				this._parameterizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(base.CreatedType, this.CollectionItemType);
				this.StoreFSharpListCreatorIfNecessary(this.NonNullableUnderlyingType);
				this.IsReadOnlyOrFixedSize = true;
				flag = this.HasParameterizedCreatorInternal;
			}
			else if (ReflectionUtils.ImplementsGenericDefinition(this.NonNullableUnderlyingType, typeof(IEnumerable<>), out type))
			{
				this.CollectionItemType = type.GetGenericArguments()[0];
				if (ReflectionUtils.IsGenericDefinition(base.UnderlyingType, typeof(IEnumerable<>)))
				{
					base.CreatedType = typeof(List<>).MakeGenericType(new Type[] { this.CollectionItemType });
				}
				this._parameterizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(this.NonNullableUnderlyingType, this.CollectionItemType);
				this.StoreFSharpListCreatorIfNecessary(this.NonNullableUnderlyingType);
				if (this.NonNullableUnderlyingType.IsGenericType() && this.NonNullableUnderlyingType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
				{
					this._genericCollectionDefinitionType = type;
					this.IsReadOnlyOrFixedSize = false;
					this.ShouldCreateWrapper = false;
					flag = true;
				}
				else
				{
					this._genericCollectionDefinitionType = typeof(List<>).MakeGenericType(new Type[] { this.CollectionItemType });
					this.IsReadOnlyOrFixedSize = true;
					this.ShouldCreateWrapper = true;
					flag = this.HasParameterizedCreatorInternal;
				}
			}
			else
			{
				flag = false;
				this.ShouldCreateWrapper = true;
			}
			this.CanDeserialize = flag;
			Type type2;
			ObjectConstructor<object> objectConstructor;
			if (this.CollectionItemType != null && ImmutableCollectionsUtils.TryBuildImmutableForArrayContract(this.NonNullableUnderlyingType, this.CollectionItemType, out type2, out objectConstructor))
			{
				base.CreatedType = type2;
				this._parameterizedCreator = objectConstructor;
				this.IsReadOnlyOrFixedSize = true;
				this.CanDeserialize = true;
			}
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x0001B4A4 File Offset: 0x000196A4
		[NullableContext(1)]
		internal IWrappedCollection CreateWrapper(object list)
		{
			if (this._genericWrapperCreator == null)
			{
				this._genericWrapperType = typeof(CollectionWrapper<>).MakeGenericType(new Type[] { this.CollectionItemType });
				Type type;
				if (ReflectionUtils.InheritsGenericDefinition(this._genericCollectionDefinitionType, typeof(List<>)) || this._genericCollectionDefinitionType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
				{
					type = typeof(ICollection<>).MakeGenericType(new Type[] { this.CollectionItemType });
				}
				else
				{
					type = this._genericCollectionDefinitionType;
				}
				ConstructorInfo constructor = this._genericWrapperType.GetConstructor(new Type[] { type });
				this._genericWrapperCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(constructor);
			}
			return (IWrappedCollection)this._genericWrapperCreator(new object[] { list });
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x0001B57C File Offset: 0x0001977C
		[NullableContext(1)]
		internal IList CreateTemporaryCollection()
		{
			if (this._genericTemporaryCollectionCreator == null)
			{
				Type type = ((this.IsMultidimensionalArray || this.CollectionItemType == null) ? typeof(object) : this.CollectionItemType);
				Type type2 = typeof(List<>).MakeGenericType(new Type[] { type });
				this._genericTemporaryCollectionCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(type2);
			}
			return (IList)this._genericTemporaryCollectionCreator();
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x0001B5F5 File Offset: 0x000197F5
		[NullableContext(1)]
		private void StoreFSharpListCreatorIfNecessary(Type underlyingType)
		{
			if (!this.HasParameterizedCreatorInternal && underlyingType.Name == "FSharpList`1")
			{
				FSharpUtils.EnsureInitialized(underlyingType.Assembly());
				this._parameterizedCreator = FSharpUtils.Instance.CreateSeq(this.CollectionItemType);
			}
		}

		// Token: 0x04000247 RID: 583
		private readonly Type _genericCollectionDefinitionType;

		// Token: 0x04000248 RID: 584
		private Type _genericWrapperType;

		// Token: 0x04000249 RID: 585
		[Nullable(new byte[] { 2, 1 })]
		private ObjectConstructor<object> _genericWrapperCreator;

		// Token: 0x0400024A RID: 586
		[Nullable(new byte[] { 2, 1 })]
		private Func<object> _genericTemporaryCollectionCreator;

		// Token: 0x0400024E RID: 590
		private readonly ConstructorInfo _parameterizedConstructor;

		// Token: 0x0400024F RID: 591
		[Nullable(new byte[] { 2, 1 })]
		private ObjectConstructor<object> _parameterizedCreator;

		// Token: 0x04000250 RID: 592
		[Nullable(new byte[] { 2, 1 })]
		private ObjectConstructor<object> _overrideCreator;
	}
}
