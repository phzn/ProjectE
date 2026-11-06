using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200008D RID: 141
	[NullableContext(2)]
	[Nullable(0)]
	public class JsonObjectContract : JsonContainerContract
	{
		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060006E1 RID: 1761 RVA: 0x0001C242 File Offset: 0x0001A442
		// (set) Token: 0x060006E2 RID: 1762 RVA: 0x0001C24A File Offset: 0x0001A44A
		public MemberSerialization MemberSerialization { get; set; }

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060006E3 RID: 1763 RVA: 0x0001C253 File Offset: 0x0001A453
		// (set) Token: 0x060006E4 RID: 1764 RVA: 0x0001C25B File Offset: 0x0001A45B
		public MissingMemberHandling? MissingMemberHandling { get; set; }

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060006E5 RID: 1765 RVA: 0x0001C264 File Offset: 0x0001A464
		// (set) Token: 0x060006E6 RID: 1766 RVA: 0x0001C26C File Offset: 0x0001A46C
		public Required? ItemRequired { get; set; }

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060006E7 RID: 1767 RVA: 0x0001C275 File Offset: 0x0001A475
		// (set) Token: 0x060006E8 RID: 1768 RVA: 0x0001C27D File Offset: 0x0001A47D
		public NullValueHandling? ItemNullValueHandling { get; set; }

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060006E9 RID: 1769 RVA: 0x0001C286 File Offset: 0x0001A486
		[Nullable(1)]
		public JsonPropertyCollection Properties
		{
			[NullableContext(1)]
			get;
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060006EA RID: 1770 RVA: 0x0001C28E File Offset: 0x0001A48E
		[Nullable(1)]
		public JsonPropertyCollection CreatorParameters
		{
			[NullableContext(1)]
			get
			{
				if (this._creatorParameters == null)
				{
					this._creatorParameters = new JsonPropertyCollection(base.UnderlyingType);
				}
				return this._creatorParameters;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060006EB RID: 1771 RVA: 0x0001C2AF File Offset: 0x0001A4AF
		// (set) Token: 0x060006EC RID: 1772 RVA: 0x0001C2B7 File Offset: 0x0001A4B7
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
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060006ED RID: 1773 RVA: 0x0001C2C0 File Offset: 0x0001A4C0
		// (set) Token: 0x060006EE RID: 1774 RVA: 0x0001C2C8 File Offset: 0x0001A4C8
		[Nullable(new byte[] { 2, 1 })]
		internal ObjectConstructor<object> ParameterizedCreator
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get
			{
				return this._parameterizedCreator;
			}
			[param: Nullable(new byte[] { 2, 1 })]
			set
			{
				this._parameterizedCreator = value;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060006EF RID: 1775 RVA: 0x0001C2D1 File Offset: 0x0001A4D1
		// (set) Token: 0x060006F0 RID: 1776 RVA: 0x0001C2D9 File Offset: 0x0001A4D9
		public ExtensionDataSetter ExtensionDataSetter { get; set; }

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060006F1 RID: 1777 RVA: 0x0001C2E2 File Offset: 0x0001A4E2
		// (set) Token: 0x060006F2 RID: 1778 RVA: 0x0001C2EA File Offset: 0x0001A4EA
		public ExtensionDataGetter ExtensionDataGetter { get; set; }

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060006F3 RID: 1779 RVA: 0x0001C2F3 File Offset: 0x0001A4F3
		// (set) Token: 0x060006F4 RID: 1780 RVA: 0x0001C2FB File Offset: 0x0001A4FB
		public Type ExtensionDataValueType
		{
			get
			{
				return this._extensionDataValueType;
			}
			set
			{
				this._extensionDataValueType = value;
				this.ExtensionDataIsJToken = value != null && typeof(JToken).IsAssignableFrom(value);
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060006F5 RID: 1781 RVA: 0x0001C326 File Offset: 0x0001A526
		// (set) Token: 0x060006F6 RID: 1782 RVA: 0x0001C32E File Offset: 0x0001A52E
		[Nullable(new byte[] { 2, 1, 1 })]
		public Func<string, string> ExtensionDataNameResolver
		{
			[return: Nullable(new byte[] { 2, 1, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1, 1 })]
			set;
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060006F7 RID: 1783 RVA: 0x0001C338 File Offset: 0x0001A538
		internal bool HasRequiredOrDefaultValueProperties
		{
			get
			{
				if (this._hasRequiredOrDefaultValueProperties == null)
				{
					this._hasRequiredOrDefaultValueProperties = new bool?(false);
					if (this.ItemRequired.GetValueOrDefault(Required.Default) != Required.Default)
					{
						this._hasRequiredOrDefaultValueProperties = new bool?(true);
					}
					else
					{
						foreach (JsonProperty jsonProperty in this.Properties)
						{
							if (jsonProperty.Required == Required.Default)
							{
								DefaultValueHandling? defaultValueHandling = jsonProperty.DefaultValueHandling & DefaultValueHandling.Populate;
								DefaultValueHandling defaultValueHandling2 = DefaultValueHandling.Populate;
								if (!((defaultValueHandling.GetValueOrDefault() == defaultValueHandling2) & (defaultValueHandling != null)))
								{
									continue;
								}
							}
							this._hasRequiredOrDefaultValueProperties = new bool?(true);
							break;
						}
					}
				}
				return this._hasRequiredOrDefaultValueProperties.GetValueOrDefault();
			}
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x0001C424 File Offset: 0x0001A624
		[NullableContext(1)]
		public JsonObjectContract(Type underlyingType)
			: base(underlyingType)
		{
			this.ContractType = JsonContractType.Object;
			this.Properties = new JsonPropertyCollection(base.UnderlyingType);
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0001C445 File Offset: 0x0001A645
		[NullableContext(1)]
		[SecuritySafeCritical]
		internal object GetUninitializedObject()
		{
			if (!JsonTypeReflector.FullyTrusted)
			{
				throw new JsonException("Insufficient permissions. Creating an uninitialized '{0}' type requires full trust.".FormatWith(CultureInfo.InvariantCulture, this.NonNullableUnderlyingType));
			}
			return FormatterServices.GetUninitializedObject(this.NonNullableUnderlyingType);
		}

		// Token: 0x04000294 RID: 660
		internal bool ExtensionDataIsJToken;

		// Token: 0x04000295 RID: 661
		private bool? _hasRequiredOrDefaultValueProperties;

		// Token: 0x04000296 RID: 662
		[Nullable(new byte[] { 2, 1 })]
		private ObjectConstructor<object> _overrideCreator;

		// Token: 0x04000297 RID: 663
		[Nullable(new byte[] { 2, 1 })]
		private ObjectConstructor<object> _parameterizedCreator;

		// Token: 0x04000298 RID: 664
		private JsonPropertyCollection _creatorParameters;

		// Token: 0x04000299 RID: 665
		private Type _extensionDataValueType;
	}
}
