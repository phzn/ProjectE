using System;
using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000094 RID: 148
	[NullableContext(1)]
	[Nullable(0)]
	internal class JsonSerializerProxy : JsonSerializer
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060007A3 RID: 1955 RVA: 0x00022249 File Offset: 0x00020449
		// (remove) Token: 0x060007A4 RID: 1956 RVA: 0x00022257 File Offset: 0x00020457
		[Nullable(new byte[] { 2, 1 })]
		public override event EventHandler<ErrorEventArgs> Error
		{
			add
			{
				this._serializer.Error += value;
			}
			remove
			{
				this._serializer.Error -= value;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060007A5 RID: 1957 RVA: 0x00022265 File Offset: 0x00020465
		// (set) Token: 0x060007A6 RID: 1958 RVA: 0x00022272 File Offset: 0x00020472
		[Nullable(2)]
		public override IReferenceResolver ReferenceResolver
		{
			[NullableContext(2)]
			get
			{
				return this._serializer.ReferenceResolver;
			}
			[NullableContext(2)]
			set
			{
				this._serializer.ReferenceResolver = value;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060007A7 RID: 1959 RVA: 0x00022280 File Offset: 0x00020480
		// (set) Token: 0x060007A8 RID: 1960 RVA: 0x0002228D File Offset: 0x0002048D
		[Nullable(2)]
		public override ITraceWriter TraceWriter
		{
			[NullableContext(2)]
			get
			{
				return this._serializer.TraceWriter;
			}
			[NullableContext(2)]
			set
			{
				this._serializer.TraceWriter = value;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060007A9 RID: 1961 RVA: 0x0002229B File Offset: 0x0002049B
		// (set) Token: 0x060007AA RID: 1962 RVA: 0x000222A8 File Offset: 0x000204A8
		[Nullable(2)]
		public override IEqualityComparer EqualityComparer
		{
			[NullableContext(2)]
			get
			{
				return this._serializer.EqualityComparer;
			}
			[NullableContext(2)]
			set
			{
				this._serializer.EqualityComparer = value;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060007AB RID: 1963 RVA: 0x000222B6 File Offset: 0x000204B6
		public override JsonConverterCollection Converters
		{
			get
			{
				return this._serializer.Converters;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060007AC RID: 1964 RVA: 0x000222C3 File Offset: 0x000204C3
		// (set) Token: 0x060007AD RID: 1965 RVA: 0x000222D0 File Offset: 0x000204D0
		public override DefaultValueHandling DefaultValueHandling
		{
			get
			{
				return this._serializer.DefaultValueHandling;
			}
			set
			{
				this._serializer.DefaultValueHandling = value;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060007AE RID: 1966 RVA: 0x000222DE File Offset: 0x000204DE
		// (set) Token: 0x060007AF RID: 1967 RVA: 0x000222EB File Offset: 0x000204EB
		public override IContractResolver ContractResolver
		{
			get
			{
				return this._serializer.ContractResolver;
			}
			set
			{
				this._serializer.ContractResolver = value;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060007B0 RID: 1968 RVA: 0x000222F9 File Offset: 0x000204F9
		// (set) Token: 0x060007B1 RID: 1969 RVA: 0x00022306 File Offset: 0x00020506
		public override MissingMemberHandling MissingMemberHandling
		{
			get
			{
				return this._serializer.MissingMemberHandling;
			}
			set
			{
				this._serializer.MissingMemberHandling = value;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060007B2 RID: 1970 RVA: 0x00022314 File Offset: 0x00020514
		// (set) Token: 0x060007B3 RID: 1971 RVA: 0x00022321 File Offset: 0x00020521
		public override NullValueHandling NullValueHandling
		{
			get
			{
				return this._serializer.NullValueHandling;
			}
			set
			{
				this._serializer.NullValueHandling = value;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060007B4 RID: 1972 RVA: 0x0002232F File Offset: 0x0002052F
		// (set) Token: 0x060007B5 RID: 1973 RVA: 0x0002233C File Offset: 0x0002053C
		public override ObjectCreationHandling ObjectCreationHandling
		{
			get
			{
				return this._serializer.ObjectCreationHandling;
			}
			set
			{
				this._serializer.ObjectCreationHandling = value;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060007B6 RID: 1974 RVA: 0x0002234A File Offset: 0x0002054A
		// (set) Token: 0x060007B7 RID: 1975 RVA: 0x00022357 File Offset: 0x00020557
		public override ReferenceLoopHandling ReferenceLoopHandling
		{
			get
			{
				return this._serializer.ReferenceLoopHandling;
			}
			set
			{
				this._serializer.ReferenceLoopHandling = value;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060007B8 RID: 1976 RVA: 0x00022365 File Offset: 0x00020565
		// (set) Token: 0x060007B9 RID: 1977 RVA: 0x00022372 File Offset: 0x00020572
		public override PreserveReferencesHandling PreserveReferencesHandling
		{
			get
			{
				return this._serializer.PreserveReferencesHandling;
			}
			set
			{
				this._serializer.PreserveReferencesHandling = value;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060007BA RID: 1978 RVA: 0x00022380 File Offset: 0x00020580
		// (set) Token: 0x060007BB RID: 1979 RVA: 0x0002238D File Offset: 0x0002058D
		public override TypeNameHandling TypeNameHandling
		{
			get
			{
				return this._serializer.TypeNameHandling;
			}
			set
			{
				this._serializer.TypeNameHandling = value;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060007BC RID: 1980 RVA: 0x0002239B File Offset: 0x0002059B
		// (set) Token: 0x060007BD RID: 1981 RVA: 0x000223A8 File Offset: 0x000205A8
		public override MetadataPropertyHandling MetadataPropertyHandling
		{
			get
			{
				return this._serializer.MetadataPropertyHandling;
			}
			set
			{
				this._serializer.MetadataPropertyHandling = value;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060007BE RID: 1982 RVA: 0x000223B6 File Offset: 0x000205B6
		// (set) Token: 0x060007BF RID: 1983 RVA: 0x000223C3 File Offset: 0x000205C3
		[Obsolete("TypeNameAssemblyFormat is obsolete. Use TypeNameAssemblyFormatHandling instead.")]
		public override FormatterAssemblyStyle TypeNameAssemblyFormat
		{
			get
			{
				return this._serializer.TypeNameAssemblyFormat;
			}
			set
			{
				this._serializer.TypeNameAssemblyFormat = value;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060007C0 RID: 1984 RVA: 0x000223D1 File Offset: 0x000205D1
		// (set) Token: 0x060007C1 RID: 1985 RVA: 0x000223DE File Offset: 0x000205DE
		public override TypeNameAssemblyFormatHandling TypeNameAssemblyFormatHandling
		{
			get
			{
				return this._serializer.TypeNameAssemblyFormatHandling;
			}
			set
			{
				this._serializer.TypeNameAssemblyFormatHandling = value;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060007C2 RID: 1986 RVA: 0x000223EC File Offset: 0x000205EC
		// (set) Token: 0x060007C3 RID: 1987 RVA: 0x000223F9 File Offset: 0x000205F9
		public override ConstructorHandling ConstructorHandling
		{
			get
			{
				return this._serializer.ConstructorHandling;
			}
			set
			{
				this._serializer.ConstructorHandling = value;
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060007C4 RID: 1988 RVA: 0x00022407 File Offset: 0x00020607
		// (set) Token: 0x060007C5 RID: 1989 RVA: 0x00022414 File Offset: 0x00020614
		[Obsolete("Binder is obsolete. Use SerializationBinder instead.")]
		public override SerializationBinder Binder
		{
			get
			{
				return this._serializer.Binder;
			}
			set
			{
				this._serializer.Binder = value;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060007C6 RID: 1990 RVA: 0x00022422 File Offset: 0x00020622
		// (set) Token: 0x060007C7 RID: 1991 RVA: 0x0002242F File Offset: 0x0002062F
		public override ISerializationBinder SerializationBinder
		{
			get
			{
				return this._serializer.SerializationBinder;
			}
			set
			{
				this._serializer.SerializationBinder = value;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060007C8 RID: 1992 RVA: 0x0002243D File Offset: 0x0002063D
		// (set) Token: 0x060007C9 RID: 1993 RVA: 0x0002244A File Offset: 0x0002064A
		public override StreamingContext Context
		{
			get
			{
				return this._serializer.Context;
			}
			set
			{
				this._serializer.Context = value;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060007CA RID: 1994 RVA: 0x00022458 File Offset: 0x00020658
		// (set) Token: 0x060007CB RID: 1995 RVA: 0x00022465 File Offset: 0x00020665
		public override Formatting Formatting
		{
			get
			{
				return this._serializer.Formatting;
			}
			set
			{
				this._serializer.Formatting = value;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060007CC RID: 1996 RVA: 0x00022473 File Offset: 0x00020673
		// (set) Token: 0x060007CD RID: 1997 RVA: 0x00022480 File Offset: 0x00020680
		public override DateFormatHandling DateFormatHandling
		{
			get
			{
				return this._serializer.DateFormatHandling;
			}
			set
			{
				this._serializer.DateFormatHandling = value;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060007CE RID: 1998 RVA: 0x0002248E File Offset: 0x0002068E
		// (set) Token: 0x060007CF RID: 1999 RVA: 0x0002249B File Offset: 0x0002069B
		public override DateTimeZoneHandling DateTimeZoneHandling
		{
			get
			{
				return this._serializer.DateTimeZoneHandling;
			}
			set
			{
				this._serializer.DateTimeZoneHandling = value;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060007D0 RID: 2000 RVA: 0x000224A9 File Offset: 0x000206A9
		// (set) Token: 0x060007D1 RID: 2001 RVA: 0x000224B6 File Offset: 0x000206B6
		public override DateParseHandling DateParseHandling
		{
			get
			{
				return this._serializer.DateParseHandling;
			}
			set
			{
				this._serializer.DateParseHandling = value;
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060007D2 RID: 2002 RVA: 0x000224C4 File Offset: 0x000206C4
		// (set) Token: 0x060007D3 RID: 2003 RVA: 0x000224D1 File Offset: 0x000206D1
		public override FloatFormatHandling FloatFormatHandling
		{
			get
			{
				return this._serializer.FloatFormatHandling;
			}
			set
			{
				this._serializer.FloatFormatHandling = value;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060007D4 RID: 2004 RVA: 0x000224DF File Offset: 0x000206DF
		// (set) Token: 0x060007D5 RID: 2005 RVA: 0x000224EC File Offset: 0x000206EC
		public override FloatParseHandling FloatParseHandling
		{
			get
			{
				return this._serializer.FloatParseHandling;
			}
			set
			{
				this._serializer.FloatParseHandling = value;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060007D6 RID: 2006 RVA: 0x000224FA File Offset: 0x000206FA
		// (set) Token: 0x060007D7 RID: 2007 RVA: 0x00022507 File Offset: 0x00020707
		public override StringEscapeHandling StringEscapeHandling
		{
			get
			{
				return this._serializer.StringEscapeHandling;
			}
			set
			{
				this._serializer.StringEscapeHandling = value;
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060007D8 RID: 2008 RVA: 0x00022515 File Offset: 0x00020715
		// (set) Token: 0x060007D9 RID: 2009 RVA: 0x00022522 File Offset: 0x00020722
		public override string DateFormatString
		{
			get
			{
				return this._serializer.DateFormatString;
			}
			set
			{
				this._serializer.DateFormatString = value;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060007DA RID: 2010 RVA: 0x00022530 File Offset: 0x00020730
		// (set) Token: 0x060007DB RID: 2011 RVA: 0x0002253D File Offset: 0x0002073D
		public override CultureInfo Culture
		{
			get
			{
				return this._serializer.Culture;
			}
			set
			{
				this._serializer.Culture = value;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060007DC RID: 2012 RVA: 0x0002254B File Offset: 0x0002074B
		// (set) Token: 0x060007DD RID: 2013 RVA: 0x00022558 File Offset: 0x00020758
		public override int? MaxDepth
		{
			get
			{
				return this._serializer.MaxDepth;
			}
			set
			{
				this._serializer.MaxDepth = value;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060007DE RID: 2014 RVA: 0x00022566 File Offset: 0x00020766
		// (set) Token: 0x060007DF RID: 2015 RVA: 0x00022573 File Offset: 0x00020773
		public override bool CheckAdditionalContent
		{
			get
			{
				return this._serializer.CheckAdditionalContent;
			}
			set
			{
				this._serializer.CheckAdditionalContent = value;
			}
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x00022581 File Offset: 0x00020781
		internal JsonSerializerInternalBase GetInternalSerializer()
		{
			if (this._serializerReader != null)
			{
				return this._serializerReader;
			}
			return this._serializerWriter;
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x00022598 File Offset: 0x00020798
		public JsonSerializerProxy(JsonSerializerInternalReader serializerReader)
		{
			ValidationUtils.ArgumentNotNull(serializerReader, "serializerReader");
			this._serializerReader = serializerReader;
			this._serializer = serializerReader.Serializer;
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x000225BE File Offset: 0x000207BE
		public JsonSerializerProxy(JsonSerializerInternalWriter serializerWriter)
		{
			ValidationUtils.ArgumentNotNull(serializerWriter, "serializerWriter");
			this._serializerWriter = serializerWriter;
			this._serializer = serializerWriter.Serializer;
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x000225E4 File Offset: 0x000207E4
		[NullableContext(2)]
		internal override object DeserializeInternal([Nullable(1)] JsonReader reader, Type objectType)
		{
			if (this._serializerReader != null)
			{
				return this._serializerReader.Deserialize(reader, objectType, false);
			}
			return this._serializer.Deserialize(reader, objectType);
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x0002260A File Offset: 0x0002080A
		internal override void PopulateInternal(JsonReader reader, object target)
		{
			if (this._serializerReader != null)
			{
				this._serializerReader.Populate(reader, target);
				return;
			}
			this._serializer.Populate(reader, target);
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x0002262F File Offset: 0x0002082F
		[NullableContext(2)]
		internal override void SerializeInternal([Nullable(1)] JsonWriter jsonWriter, object value, Type rootType)
		{
			if (this._serializerWriter != null)
			{
				this._serializerWriter.Serialize(jsonWriter, value, rootType);
				return;
			}
			this._serializer.Serialize(jsonWriter, value);
		}

		// Token: 0x040002C6 RID: 710
		[Nullable(2)]
		private readonly JsonSerializerInternalReader _serializerReader;

		// Token: 0x040002C7 RID: 711
		[Nullable(2)]
		private readonly JsonSerializerInternalWriter _serializerWriter;

		// Token: 0x040002C8 RID: 712
		internal readonly JsonSerializer _serializer;
	}
}
