using System;
using System.Collections.Generic;

namespace Unity.Properties
{
	// Token: 0x02000083 RID: 131
	internal readonly struct ConversionRegistry : IEqualityComparer<ConversionRegistry>
	{
		// Token: 0x06000213 RID: 531 RVA: 0x00007110 File Offset: 0x00005310
		private ConversionRegistry(Dictionary<ConversionRegistry.ConverterKey, Delegate> storage)
		{
			this.m_Converters = storage;
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000214 RID: 532 RVA: 0x0000711A File Offset: 0x0000531A
		public int ConverterCount
		{
			get
			{
				Dictionary<ConversionRegistry.ConverterKey, Delegate> converters = this.m_Converters;
				return (converters != null) ? converters.Count : 0;
			}
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00007130 File Offset: 0x00005330
		public static ConversionRegistry Create()
		{
			return new ConversionRegistry(new Dictionary<ConversionRegistry.ConverterKey, Delegate>(ConversionRegistry.Comparer));
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00007151 File Offset: 0x00005351
		public void Register(Type source, Type destination, Delegate converter)
		{
			Dictionary<ConversionRegistry.ConverterKey, Delegate> converters = this.m_Converters;
			ConversionRegistry.ConverterKey converterKey = new ConversionRegistry.ConverterKey(source, destination);
			if (converter == null)
			{
				throw new ArgumentException("converter");
			}
			converters[converterKey] = converter;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00007177 File Offset: 0x00005377
		public void Unregister(Type source, Type destination)
		{
			this.m_Converters.Remove(new ConversionRegistry.ConverterKey(source, destination));
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00007190 File Offset: 0x00005390
		public Delegate GetConverter(Type source, Type destination)
		{
			ConversionRegistry.ConverterKey converterKey = new ConversionRegistry.ConverterKey(source, destination);
			Delegate @delegate;
			return this.m_Converters.TryGetValue(converterKey, out @delegate) ? @delegate : null;
		}

		// Token: 0x06000219 RID: 537 RVA: 0x000071C0 File Offset: 0x000053C0
		public bool TryGetConverter(Type source, Type destination, out Delegate converter)
		{
			converter = this.GetConverter(source, destination);
			return converter != null;
		}

		// Token: 0x0600021A RID: 538 RVA: 0x000071E4 File Offset: 0x000053E4
		public void GetAllTypesConvertingToType(Type type, List<Type> result)
		{
			foreach (ConversionRegistry.ConverterKey converterKey in this.m_Converters.Keys)
			{
				bool flag = converterKey.DestinationType == type;
				if (flag)
				{
					result.Add(converterKey.SourceType);
				}
			}
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00007258 File Offset: 0x00005458
		public bool Equals(ConversionRegistry x, ConversionRegistry y)
		{
			return x.m_Converters == y.m_Converters;
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00007278 File Offset: 0x00005478
		public int GetHashCode(ConversionRegistry obj)
		{
			return (obj.m_Converters != null) ? obj.m_Converters.GetHashCode() : 0;
		}

		// Token: 0x040000A6 RID: 166
		private static readonly ConversionRegistry.ConverterKeyComparer Comparer = new ConversionRegistry.ConverterKeyComparer();

		// Token: 0x040000A7 RID: 167
		private readonly Dictionary<ConversionRegistry.ConverterKey, Delegate> m_Converters;

		// Token: 0x02000084 RID: 132
		private class ConverterKeyComparer : IEqualityComparer<ConversionRegistry.ConverterKey>
		{
			// Token: 0x0600021E RID: 542 RVA: 0x000072AC File Offset: 0x000054AC
			public bool Equals(ConversionRegistry.ConverterKey x, ConversionRegistry.ConverterKey y)
			{
				return x.SourceType == y.SourceType && x.DestinationType == y.DestinationType;
			}

			// Token: 0x0600021F RID: 543 RVA: 0x000072E8 File Offset: 0x000054E8
			public int GetHashCode(ConversionRegistry.ConverterKey obj)
			{
				return (((obj.SourceType != null) ? obj.SourceType.GetHashCode() : 0) * 397) ^ ((obj.DestinationType != null) ? obj.DestinationType.GetHashCode() : 0);
			}
		}

		// Token: 0x02000085 RID: 133
		private readonly struct ConverterKey
		{
			// Token: 0x06000221 RID: 545 RVA: 0x00007339 File Offset: 0x00005539
			public ConverterKey(Type source, Type destination)
			{
				this.SourceType = source;
				this.DestinationType = destination;
			}

			// Token: 0x040000A8 RID: 168
			public readonly Type SourceType;

			// Token: 0x040000A9 RID: 169
			public readonly Type DestinationType;
		}
	}
}
