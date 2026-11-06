using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000A0 RID: 160
	[NullableContext(1)]
	[Nullable(0)]
	internal class TraceJsonReader : JsonReader, IJsonLineInfo
	{
		// Token: 0x06000826 RID: 2086 RVA: 0x00023014 File Offset: 0x00021214
		public TraceJsonReader(JsonReader innerReader)
		{
			this._innerReader = innerReader;
			this._sw = new StringWriter(CultureInfo.InvariantCulture);
			this._sw.Write("Deserialized JSON: " + Environment.NewLine);
			this._textWriter = new JsonTextWriter(this._sw);
			this._textWriter.Formatting = Formatting.Indented;
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x00023075 File Offset: 0x00021275
		public string GetDeserializedJsonMessage()
		{
			return this._sw.ToString();
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x00023082 File Offset: 0x00021282
		public override bool Read()
		{
			bool flag = this._innerReader.Read();
			this.WriteCurrentToken();
			return flag;
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x00023095 File Offset: 0x00021295
		public override int? ReadAsInt32()
		{
			int? num = this._innerReader.ReadAsInt32();
			this.WriteCurrentToken();
			return num;
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x000230A8 File Offset: 0x000212A8
		[NullableContext(2)]
		public override string ReadAsString()
		{
			string text = this._innerReader.ReadAsString();
			this.WriteCurrentToken();
			return text;
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x000230BB File Offset: 0x000212BB
		[NullableContext(2)]
		public override byte[] ReadAsBytes()
		{
			byte[] array = this._innerReader.ReadAsBytes();
			this.WriteCurrentToken();
			return array;
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x000230CE File Offset: 0x000212CE
		public override decimal? ReadAsDecimal()
		{
			decimal? num = this._innerReader.ReadAsDecimal();
			this.WriteCurrentToken();
			return num;
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x000230E1 File Offset: 0x000212E1
		public override double? ReadAsDouble()
		{
			double? num = this._innerReader.ReadAsDouble();
			this.WriteCurrentToken();
			return num;
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x000230F4 File Offset: 0x000212F4
		public override bool? ReadAsBoolean()
		{
			bool? flag = this._innerReader.ReadAsBoolean();
			this.WriteCurrentToken();
			return flag;
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x00023107 File Offset: 0x00021307
		public override DateTime? ReadAsDateTime()
		{
			DateTime? dateTime = this._innerReader.ReadAsDateTime();
			this.WriteCurrentToken();
			return dateTime;
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x0002311A File Offset: 0x0002131A
		public override DateTimeOffset? ReadAsDateTimeOffset()
		{
			DateTimeOffset? dateTimeOffset = this._innerReader.ReadAsDateTimeOffset();
			this.WriteCurrentToken();
			return dateTimeOffset;
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x0002312D File Offset: 0x0002132D
		public void WriteCurrentToken()
		{
			this._textWriter.WriteToken(this._innerReader, false, false, true);
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000832 RID: 2098 RVA: 0x00023143 File Offset: 0x00021343
		public override int Depth
		{
			get
			{
				return this._innerReader.Depth;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000833 RID: 2099 RVA: 0x00023150 File Offset: 0x00021350
		public override string Path
		{
			get
			{
				return this._innerReader.Path;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000834 RID: 2100 RVA: 0x0002315D File Offset: 0x0002135D
		// (set) Token: 0x06000835 RID: 2101 RVA: 0x0002316A File Offset: 0x0002136A
		public override char QuoteChar
		{
			get
			{
				return this._innerReader.QuoteChar;
			}
			protected internal set
			{
				this._innerReader.QuoteChar = value;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000836 RID: 2102 RVA: 0x00023178 File Offset: 0x00021378
		public override JsonToken TokenType
		{
			get
			{
				return this._innerReader.TokenType;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000837 RID: 2103 RVA: 0x00023185 File Offset: 0x00021385
		[Nullable(2)]
		public override object Value
		{
			[NullableContext(2)]
			get
			{
				return this._innerReader.Value;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000838 RID: 2104 RVA: 0x00023192 File Offset: 0x00021392
		[Nullable(2)]
		public override Type ValueType
		{
			[NullableContext(2)]
			get
			{
				return this._innerReader.ValueType;
			}
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x0002319F File Offset: 0x0002139F
		public override void Close()
		{
			this._innerReader.Close();
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x000231AC File Offset: 0x000213AC
		bool IJsonLineInfo.HasLineInfo()
		{
			IJsonLineInfo jsonLineInfo = this._innerReader as IJsonLineInfo;
			return jsonLineInfo != null && jsonLineInfo.HasLineInfo();
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x0600083B RID: 2107 RVA: 0x000231D0 File Offset: 0x000213D0
		int IJsonLineInfo.LineNumber
		{
			get
			{
				IJsonLineInfo jsonLineInfo = this._innerReader as IJsonLineInfo;
				if (jsonLineInfo == null)
				{
					return 0;
				}
				return jsonLineInfo.LineNumber;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x0600083C RID: 2108 RVA: 0x000231F4 File Offset: 0x000213F4
		int IJsonLineInfo.LinePosition
		{
			get
			{
				IJsonLineInfo jsonLineInfo = this._innerReader as IJsonLineInfo;
				if (jsonLineInfo == null)
				{
					return 0;
				}
				return jsonLineInfo.LinePosition;
			}
		}

		// Token: 0x040002DF RID: 735
		private readonly JsonReader _innerReader;

		// Token: 0x040002E0 RID: 736
		private readonly JsonTextWriter _textWriter;

		// Token: 0x040002E1 RID: 737
		private readonly StringWriter _sw;
	}
}
