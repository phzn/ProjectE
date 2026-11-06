using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200008F RID: 143
	[NullableContext(2)]
	[Nullable(0)]
	public class JsonProperty
	{
		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060006FE RID: 1790 RVA: 0x0001C5E0 File Offset: 0x0001A7E0
		// (set) Token: 0x060006FF RID: 1791 RVA: 0x0001C5E8 File Offset: 0x0001A7E8
		internal JsonContract PropertyContract { get; set; }

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x0001C5F1 File Offset: 0x0001A7F1
		// (set) Token: 0x06000701 RID: 1793 RVA: 0x0001C5F9 File Offset: 0x0001A7F9
		public string PropertyName
		{
			get
			{
				return this._propertyName;
			}
			set
			{
				this._propertyName = value;
				this._skipPropertyNameEscape = !JavaScriptUtils.ShouldEscapeJavaScriptString(this._propertyName, JavaScriptUtils.HtmlCharEscapeFlags);
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x0001C61B File Offset: 0x0001A81B
		// (set) Token: 0x06000703 RID: 1795 RVA: 0x0001C623 File Offset: 0x0001A823
		public Type DeclaringType { get; set; }

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000704 RID: 1796 RVA: 0x0001C62C File Offset: 0x0001A82C
		// (set) Token: 0x06000705 RID: 1797 RVA: 0x0001C634 File Offset: 0x0001A834
		public int? Order { get; set; }

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000706 RID: 1798 RVA: 0x0001C63D File Offset: 0x0001A83D
		// (set) Token: 0x06000707 RID: 1799 RVA: 0x0001C645 File Offset: 0x0001A845
		public string UnderlyingName { get; set; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000708 RID: 1800 RVA: 0x0001C64E File Offset: 0x0001A84E
		// (set) Token: 0x06000709 RID: 1801 RVA: 0x0001C656 File Offset: 0x0001A856
		public IValueProvider ValueProvider { get; set; }

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x0600070A RID: 1802 RVA: 0x0001C65F File Offset: 0x0001A85F
		// (set) Token: 0x0600070B RID: 1803 RVA: 0x0001C667 File Offset: 0x0001A867
		public IAttributeProvider AttributeProvider { get; set; }

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x0600070C RID: 1804 RVA: 0x0001C670 File Offset: 0x0001A870
		// (set) Token: 0x0600070D RID: 1805 RVA: 0x0001C678 File Offset: 0x0001A878
		public Type PropertyType
		{
			get
			{
				return this._propertyType;
			}
			set
			{
				if (this._propertyType != value)
				{
					this._propertyType = value;
					this._hasGeneratedDefaultValue = false;
				}
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x0600070E RID: 1806 RVA: 0x0001C696 File Offset: 0x0001A896
		// (set) Token: 0x0600070F RID: 1807 RVA: 0x0001C69E File Offset: 0x0001A89E
		public JsonConverter Converter { get; set; }

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000710 RID: 1808 RVA: 0x0001C6A7 File Offset: 0x0001A8A7
		// (set) Token: 0x06000711 RID: 1809 RVA: 0x0001C6AF File Offset: 0x0001A8AF
		[Obsolete("MemberConverter is obsolete. Use Converter instead.")]
		public JsonConverter MemberConverter
		{
			get
			{
				return this.Converter;
			}
			set
			{
				this.Converter = value;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000712 RID: 1810 RVA: 0x0001C6B8 File Offset: 0x0001A8B8
		// (set) Token: 0x06000713 RID: 1811 RVA: 0x0001C6C0 File Offset: 0x0001A8C0
		public bool Ignored { get; set; }

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000714 RID: 1812 RVA: 0x0001C6C9 File Offset: 0x0001A8C9
		// (set) Token: 0x06000715 RID: 1813 RVA: 0x0001C6D1 File Offset: 0x0001A8D1
		public bool Readable { get; set; }

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000716 RID: 1814 RVA: 0x0001C6DA File Offset: 0x0001A8DA
		// (set) Token: 0x06000717 RID: 1815 RVA: 0x0001C6E2 File Offset: 0x0001A8E2
		public bool Writable { get; set; }

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000718 RID: 1816 RVA: 0x0001C6EB File Offset: 0x0001A8EB
		// (set) Token: 0x06000719 RID: 1817 RVA: 0x0001C6F3 File Offset: 0x0001A8F3
		public bool HasMemberAttribute { get; set; }

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x0600071A RID: 1818 RVA: 0x0001C6FC File Offset: 0x0001A8FC
		// (set) Token: 0x0600071B RID: 1819 RVA: 0x0001C70E File Offset: 0x0001A90E
		public object DefaultValue
		{
			get
			{
				if (!this._hasExplicitDefaultValue)
				{
					return null;
				}
				return this._defaultValue;
			}
			set
			{
				this._hasExplicitDefaultValue = true;
				this._defaultValue = value;
			}
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x0001C71E File Offset: 0x0001A91E
		internal object GetResolvedDefaultValue()
		{
			if (this._propertyType == null)
			{
				return null;
			}
			if (!this._hasExplicitDefaultValue && !this._hasGeneratedDefaultValue)
			{
				this._defaultValue = ReflectionUtils.GetDefaultValue(this._propertyType);
				this._hasGeneratedDefaultValue = true;
			}
			return this._defaultValue;
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600071D RID: 1821 RVA: 0x0001C75E File Offset: 0x0001A95E
		// (set) Token: 0x0600071E RID: 1822 RVA: 0x0001C76B File Offset: 0x0001A96B
		public Required Required
		{
			get
			{
				return this._required.GetValueOrDefault();
			}
			set
			{
				this._required = new Required?(value);
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600071F RID: 1823 RVA: 0x0001C779 File Offset: 0x0001A979
		public bool IsRequiredSpecified
		{
			get
			{
				return this._required != null;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000720 RID: 1824 RVA: 0x0001C786 File Offset: 0x0001A986
		// (set) Token: 0x06000721 RID: 1825 RVA: 0x0001C78E File Offset: 0x0001A98E
		public bool? IsReference { get; set; }

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000722 RID: 1826 RVA: 0x0001C797 File Offset: 0x0001A997
		// (set) Token: 0x06000723 RID: 1827 RVA: 0x0001C79F File Offset: 0x0001A99F
		public NullValueHandling? NullValueHandling { get; set; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000724 RID: 1828 RVA: 0x0001C7A8 File Offset: 0x0001A9A8
		// (set) Token: 0x06000725 RID: 1829 RVA: 0x0001C7B0 File Offset: 0x0001A9B0
		public DefaultValueHandling? DefaultValueHandling { get; set; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000726 RID: 1830 RVA: 0x0001C7B9 File Offset: 0x0001A9B9
		// (set) Token: 0x06000727 RID: 1831 RVA: 0x0001C7C1 File Offset: 0x0001A9C1
		public ReferenceLoopHandling? ReferenceLoopHandling { get; set; }

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000728 RID: 1832 RVA: 0x0001C7CA File Offset: 0x0001A9CA
		// (set) Token: 0x06000729 RID: 1833 RVA: 0x0001C7D2 File Offset: 0x0001A9D2
		public ObjectCreationHandling? ObjectCreationHandling { get; set; }

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x0600072A RID: 1834 RVA: 0x0001C7DB File Offset: 0x0001A9DB
		// (set) Token: 0x0600072B RID: 1835 RVA: 0x0001C7E3 File Offset: 0x0001A9E3
		public TypeNameHandling? TypeNameHandling { get; set; }

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x0600072C RID: 1836 RVA: 0x0001C7EC File Offset: 0x0001A9EC
		// (set) Token: 0x0600072D RID: 1837 RVA: 0x0001C7F4 File Offset: 0x0001A9F4
		[Nullable(new byte[] { 2, 1 })]
		public Predicate<object> ShouldSerialize
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x0600072E RID: 1838 RVA: 0x0001C7FD File Offset: 0x0001A9FD
		// (set) Token: 0x0600072F RID: 1839 RVA: 0x0001C805 File Offset: 0x0001AA05
		[Nullable(new byte[] { 2, 1 })]
		public Predicate<object> ShouldDeserialize
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000730 RID: 1840 RVA: 0x0001C80E File Offset: 0x0001AA0E
		// (set) Token: 0x06000731 RID: 1841 RVA: 0x0001C816 File Offset: 0x0001AA16
		[Nullable(new byte[] { 2, 1 })]
		public Predicate<object> GetIsSpecified
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000732 RID: 1842 RVA: 0x0001C81F File Offset: 0x0001AA1F
		// (set) Token: 0x06000733 RID: 1843 RVA: 0x0001C827 File Offset: 0x0001AA27
		[Nullable(new byte[] { 2, 1, 2 })]
		public Action<object, object> SetIsSpecified
		{
			[return: Nullable(new byte[] { 2, 1, 2 })]
			get;
			[param: Nullable(new byte[] { 2, 1, 2 })]
			set;
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x0001C830 File Offset: 0x0001AA30
		[NullableContext(1)]
		public override string ToString()
		{
			return this.PropertyName ?? string.Empty;
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000735 RID: 1845 RVA: 0x0001C841 File Offset: 0x0001AA41
		// (set) Token: 0x06000736 RID: 1846 RVA: 0x0001C849 File Offset: 0x0001AA49
		public JsonConverter ItemConverter { get; set; }

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000737 RID: 1847 RVA: 0x0001C852 File Offset: 0x0001AA52
		// (set) Token: 0x06000738 RID: 1848 RVA: 0x0001C85A File Offset: 0x0001AA5A
		public bool? ItemIsReference { get; set; }

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000739 RID: 1849 RVA: 0x0001C863 File Offset: 0x0001AA63
		// (set) Token: 0x0600073A RID: 1850 RVA: 0x0001C86B File Offset: 0x0001AA6B
		public TypeNameHandling? ItemTypeNameHandling { get; set; }

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x0600073B RID: 1851 RVA: 0x0001C874 File Offset: 0x0001AA74
		// (set) Token: 0x0600073C RID: 1852 RVA: 0x0001C87C File Offset: 0x0001AA7C
		public ReferenceLoopHandling? ItemReferenceLoopHandling { get; set; }

		// Token: 0x0600073D RID: 1853 RVA: 0x0001C888 File Offset: 0x0001AA88
		[NullableContext(1)]
		internal void WritePropertyName(JsonWriter writer)
		{
			string propertyName = this.PropertyName;
			if (this._skipPropertyNameEscape)
			{
				writer.WritePropertyName(propertyName, false);
				return;
			}
			writer.WritePropertyName(propertyName);
		}

		// Token: 0x0400029C RID: 668
		internal Required? _required;

		// Token: 0x0400029D RID: 669
		internal bool _hasExplicitDefaultValue;

		// Token: 0x0400029E RID: 670
		private object _defaultValue;

		// Token: 0x0400029F RID: 671
		private bool _hasGeneratedDefaultValue;

		// Token: 0x040002A0 RID: 672
		private string _propertyName;

		// Token: 0x040002A1 RID: 673
		internal bool _skipPropertyNameEscape;

		// Token: 0x040002A2 RID: 674
		private Type _propertyType;
	}
}
