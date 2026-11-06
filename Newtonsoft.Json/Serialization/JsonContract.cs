using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000087 RID: 135
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class JsonContract
	{
		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000698 RID: 1688 RVA: 0x0001B727 File Offset: 0x00019927
		public Type UnderlyingType { get; }

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000699 RID: 1689 RVA: 0x0001B72F File Offset: 0x0001992F
		// (set) Token: 0x0600069A RID: 1690 RVA: 0x0001B738 File Offset: 0x00019938
		public Type CreatedType
		{
			get
			{
				return this._createdType;
			}
			set
			{
				ValidationUtils.ArgumentNotNull(value, "value");
				this._createdType = value;
				this.IsSealed = this._createdType.IsSealed();
				this.IsInstantiable = !this._createdType.IsInterface() && !this._createdType.IsAbstract();
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x0600069B RID: 1691 RVA: 0x0001B78C File Offset: 0x0001998C
		// (set) Token: 0x0600069C RID: 1692 RVA: 0x0001B794 File Offset: 0x00019994
		public bool? IsReference { get; set; }

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600069D RID: 1693 RVA: 0x0001B79D File Offset: 0x0001999D
		// (set) Token: 0x0600069E RID: 1694 RVA: 0x0001B7A5 File Offset: 0x000199A5
		[Nullable(2)]
		public JsonConverter Converter
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x0600069F RID: 1695 RVA: 0x0001B7AE File Offset: 0x000199AE
		// (set) Token: 0x060006A0 RID: 1696 RVA: 0x0001B7B6 File Offset: 0x000199B6
		[Nullable(2)]
		public JsonConverter InternalConverter
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			internal set;
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060006A1 RID: 1697 RVA: 0x0001B7BF File Offset: 0x000199BF
		public IList<SerializationCallback> OnDeserializedCallbacks
		{
			get
			{
				if (this._onDeserializedCallbacks == null)
				{
					this._onDeserializedCallbacks = new List<SerializationCallback>();
				}
				return this._onDeserializedCallbacks;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060006A2 RID: 1698 RVA: 0x0001B7DA File Offset: 0x000199DA
		public IList<SerializationCallback> OnDeserializingCallbacks
		{
			get
			{
				if (this._onDeserializingCallbacks == null)
				{
					this._onDeserializingCallbacks = new List<SerializationCallback>();
				}
				return this._onDeserializingCallbacks;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060006A3 RID: 1699 RVA: 0x0001B7F5 File Offset: 0x000199F5
		public IList<SerializationCallback> OnSerializedCallbacks
		{
			get
			{
				if (this._onSerializedCallbacks == null)
				{
					this._onSerializedCallbacks = new List<SerializationCallback>();
				}
				return this._onSerializedCallbacks;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060006A4 RID: 1700 RVA: 0x0001B810 File Offset: 0x00019A10
		public IList<SerializationCallback> OnSerializingCallbacks
		{
			get
			{
				if (this._onSerializingCallbacks == null)
				{
					this._onSerializingCallbacks = new List<SerializationCallback>();
				}
				return this._onSerializingCallbacks;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060006A5 RID: 1701 RVA: 0x0001B82B File Offset: 0x00019A2B
		public IList<SerializationErrorCallback> OnErrorCallbacks
		{
			get
			{
				if (this._onErrorCallbacks == null)
				{
					this._onErrorCallbacks = new List<SerializationErrorCallback>();
				}
				return this._onErrorCallbacks;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060006A6 RID: 1702 RVA: 0x0001B846 File Offset: 0x00019A46
		// (set) Token: 0x060006A7 RID: 1703 RVA: 0x0001B84E File Offset: 0x00019A4E
		[Nullable(new byte[] { 2, 1 })]
		public Func<object> DefaultCreator
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060006A8 RID: 1704 RVA: 0x0001B857 File Offset: 0x00019A57
		// (set) Token: 0x060006A9 RID: 1705 RVA: 0x0001B85F File Offset: 0x00019A5F
		public bool DefaultCreatorNonPublic { get; set; }

		// Token: 0x060006AA RID: 1706 RVA: 0x0001B868 File Offset: 0x00019A68
		internal JsonContract(Type underlyingType)
		{
			ValidationUtils.ArgumentNotNull(underlyingType, "underlyingType");
			this.UnderlyingType = underlyingType;
			underlyingType = ReflectionUtils.EnsureNotByRefType(underlyingType);
			this.IsNullable = ReflectionUtils.IsNullable(underlyingType);
			this.NonNullableUnderlyingType = ((this.IsNullable && ReflectionUtils.IsNullableType(underlyingType)) ? Nullable.GetUnderlyingType(underlyingType) : underlyingType);
			this._createdType = (this.CreatedType = this.NonNullableUnderlyingType);
			this.IsConvertable = ConvertUtils.IsConvertible(this.NonNullableUnderlyingType);
			this.IsEnum = this.NonNullableUnderlyingType.IsEnum();
			this.InternalReadType = ReadType.Read;
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x0001B900 File Offset: 0x00019B00
		internal void InvokeOnSerializing(object o, StreamingContext context)
		{
			if (this._onSerializingCallbacks != null)
			{
				foreach (SerializationCallback serializationCallback in this._onSerializingCallbacks)
				{
					serializationCallback(o, context);
				}
			}
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x0001B95C File Offset: 0x00019B5C
		internal void InvokeOnSerialized(object o, StreamingContext context)
		{
			if (this._onSerializedCallbacks != null)
			{
				foreach (SerializationCallback serializationCallback in this._onSerializedCallbacks)
				{
					serializationCallback(o, context);
				}
			}
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x0001B9B8 File Offset: 0x00019BB8
		internal void InvokeOnDeserializing(object o, StreamingContext context)
		{
			if (this._onDeserializingCallbacks != null)
			{
				foreach (SerializationCallback serializationCallback in this._onDeserializingCallbacks)
				{
					serializationCallback(o, context);
				}
			}
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x0001BA14 File Offset: 0x00019C14
		internal void InvokeOnDeserialized(object o, StreamingContext context)
		{
			if (this._onDeserializedCallbacks != null)
			{
				foreach (SerializationCallback serializationCallback in this._onDeserializedCallbacks)
				{
					serializationCallback(o, context);
				}
			}
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x0001BA70 File Offset: 0x00019C70
		internal void InvokeOnError(object o, StreamingContext context, ErrorContext errorContext)
		{
			if (this._onErrorCallbacks != null)
			{
				foreach (SerializationErrorCallback serializationErrorCallback in this._onErrorCallbacks)
				{
					serializationErrorCallback(o, context, errorContext);
				}
			}
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x0001BACC File Offset: 0x00019CCC
		internal static SerializationCallback CreateSerializationCallback(MethodInfo callbackMethodInfo)
		{
			return delegate(object o, StreamingContext context)
			{
				callbackMethodInfo.Invoke(o, new object[] { context });
			};
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x0001BAE5 File Offset: 0x00019CE5
		internal static SerializationErrorCallback CreateSerializationErrorCallback(MethodInfo callbackMethodInfo)
		{
			return delegate(object o, StreamingContext context, ErrorContext econtext)
			{
				callbackMethodInfo.Invoke(o, new object[] { context, econtext });
			};
		}

		// Token: 0x04000262 RID: 610
		internal bool IsNullable;

		// Token: 0x04000263 RID: 611
		internal bool IsConvertable;

		// Token: 0x04000264 RID: 612
		internal bool IsEnum;

		// Token: 0x04000265 RID: 613
		internal Type NonNullableUnderlyingType;

		// Token: 0x04000266 RID: 614
		internal ReadType InternalReadType;

		// Token: 0x04000267 RID: 615
		internal JsonContractType ContractType;

		// Token: 0x04000268 RID: 616
		internal bool IsReadOnlyOrFixedSize;

		// Token: 0x04000269 RID: 617
		internal bool IsSealed;

		// Token: 0x0400026A RID: 618
		internal bool IsInstantiable;

		// Token: 0x0400026B RID: 619
		[Nullable(new byte[] { 2, 1 })]
		private List<SerializationCallback> _onDeserializedCallbacks;

		// Token: 0x0400026C RID: 620
		[Nullable(new byte[] { 2, 1 })]
		private List<SerializationCallback> _onDeserializingCallbacks;

		// Token: 0x0400026D RID: 621
		[Nullable(new byte[] { 2, 1 })]
		private List<SerializationCallback> _onSerializedCallbacks;

		// Token: 0x0400026E RID: 622
		[Nullable(new byte[] { 2, 1 })]
		private List<SerializationCallback> _onSerializingCallbacks;

		// Token: 0x0400026F RID: 623
		[Nullable(new byte[] { 2, 1 })]
		private List<SerializationErrorCallback> _onErrorCallbacks;

		// Token: 0x04000270 RID: 624
		private Type _createdType;
	}
}
