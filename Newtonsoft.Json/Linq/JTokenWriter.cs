using System;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000C7 RID: 199
	[NullableContext(2)]
	[Nullable(0)]
	public class JTokenWriter : JsonWriter
	{
		// Token: 0x06000B66 RID: 2918 RVA: 0x0002CC21 File Offset: 0x0002AE21
		[NullableContext(1)]
		internal override Task WriteTokenAsync(JsonReader reader, bool writeChildren, bool writeDateConstructorAsDate, bool writeComments, CancellationToken cancellationToken)
		{
			if (reader is JTokenReader)
			{
				this.WriteToken(reader, writeChildren, writeDateConstructorAsDate, writeComments);
				return AsyncUtils.CompletedTask;
			}
			return base.WriteTokenSyncReadingAsync(reader, cancellationToken);
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000B67 RID: 2919 RVA: 0x0002CC45 File Offset: 0x0002AE45
		public JToken CurrentToken
		{
			get
			{
				return this._current;
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000B68 RID: 2920 RVA: 0x0002CC4D File Offset: 0x0002AE4D
		public JToken Token
		{
			get
			{
				if (this._token != null)
				{
					return this._token;
				}
				return this._value;
			}
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x0002CC64 File Offset: 0x0002AE64
		[NullableContext(1)]
		public JTokenWriter(JContainer container)
		{
			ValidationUtils.ArgumentNotNull(container, "container");
			this._token = container;
			this._parent = container;
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x0002CC85 File Offset: 0x0002AE85
		public JTokenWriter()
		{
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x0002CC8D File Offset: 0x0002AE8D
		public override void Flush()
		{
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x0002CC8F File Offset: 0x0002AE8F
		public override void Close()
		{
			base.Close();
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x0002CC97 File Offset: 0x0002AE97
		public override void WriteStartObject()
		{
			base.WriteStartObject();
			this.AddParent(new JObject());
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x0002CCAA File Offset: 0x0002AEAA
		[NullableContext(1)]
		private void AddParent(JContainer container)
		{
			if (this._parent == null)
			{
				this._token = container;
			}
			else
			{
				this._parent.AddAndSkipParentCheck(container);
			}
			this._parent = container;
			this._current = container;
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x0002CCD8 File Offset: 0x0002AED8
		private void RemoveParent()
		{
			this._current = this._parent;
			this._parent = this._parent.Parent;
			if (this._parent != null && this._parent.Type == JTokenType.Property)
			{
				this._parent = this._parent.Parent;
			}
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x0002CD29 File Offset: 0x0002AF29
		public override void WriteStartArray()
		{
			base.WriteStartArray();
			this.AddParent(new JArray());
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x0002CD3C File Offset: 0x0002AF3C
		[NullableContext(1)]
		public override void WriteStartConstructor(string name)
		{
			base.WriteStartConstructor(name);
			this.AddParent(new JConstructor(name));
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x0002CD51 File Offset: 0x0002AF51
		protected override void WriteEnd(JsonToken token)
		{
			this.RemoveParent();
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x0002CD59 File Offset: 0x0002AF59
		[NullableContext(1)]
		public override void WritePropertyName(string name)
		{
			JObject jobject = this._parent as JObject;
			if (jobject != null)
			{
				jobject.Remove(name);
			}
			this.AddParent(new JProperty(name));
			base.WritePropertyName(name);
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x0002CD86 File Offset: 0x0002AF86
		private void AddRawValue(object value, JTokenType type, JsonToken token)
		{
			this.AddJValue(new JValue(value, type), token);
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x0002CD98 File Offset: 0x0002AF98
		internal void AddJValue(JValue value, JsonToken token)
		{
			if (this._parent != null)
			{
				if (this._parent.TryAdd(value))
				{
					this._current = this._parent.Last;
					if (this._parent.Type == JTokenType.Property)
					{
						this._parent = this._parent.Parent;
						return;
					}
				}
			}
			else
			{
				this._value = value ?? JValue.CreateNull();
				this._current = this._value;
			}
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x0002CE08 File Offset: 0x0002B008
		public override void WriteValue(object value)
		{
			if (value is BigInteger)
			{
				base.InternalWriteValue(JsonToken.Integer);
				this.AddRawValue(value, JTokenType.Integer, JsonToken.Integer);
				return;
			}
			base.WriteValue(value);
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x0002CE2A File Offset: 0x0002B02A
		public override void WriteNull()
		{
			base.WriteNull();
			this.AddJValue(JValue.CreateNull(), JsonToken.Null);
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x0002CE3F File Offset: 0x0002B03F
		public override void WriteUndefined()
		{
			base.WriteUndefined();
			this.AddJValue(JValue.CreateUndefined(), JsonToken.Undefined);
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x0002CE54 File Offset: 0x0002B054
		public override void WriteRaw(string json)
		{
			base.WriteRaw(json);
			this.AddJValue(new JRaw(json), JsonToken.Raw);
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x0002CE6A File Offset: 0x0002B06A
		public override void WriteComment(string text)
		{
			base.WriteComment(text);
			this.AddJValue(JValue.CreateComment(text), JsonToken.Comment);
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x0002CE80 File Offset: 0x0002B080
		public override void WriteValue(string value)
		{
			base.WriteValue(value);
			this.AddJValue(new JValue(value), JsonToken.String);
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x0002CE97 File Offset: 0x0002B097
		public override void WriteValue(int value)
		{
			base.WriteValue(value);
			this.AddRawValue(value, JTokenType.Integer, JsonToken.Integer);
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x0002CEAE File Offset: 0x0002B0AE
		[CLSCompliant(false)]
		public override void WriteValue(uint value)
		{
			base.WriteValue(value);
			this.AddRawValue(value, JTokenType.Integer, JsonToken.Integer);
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x0002CEC5 File Offset: 0x0002B0C5
		public override void WriteValue(long value)
		{
			base.WriteValue(value);
			this.AddJValue(new JValue(value), JsonToken.Integer);
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x0002CEDB File Offset: 0x0002B0DB
		[CLSCompliant(false)]
		public override void WriteValue(ulong value)
		{
			base.WriteValue(value);
			this.AddJValue(new JValue(value), JsonToken.Integer);
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x0002CEF1 File Offset: 0x0002B0F1
		public override void WriteValue(float value)
		{
			base.WriteValue(value);
			this.AddJValue(new JValue(value), JsonToken.Float);
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x0002CF07 File Offset: 0x0002B107
		public override void WriteValue(double value)
		{
			base.WriteValue(value);
			this.AddJValue(new JValue(value), JsonToken.Float);
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x0002CF1D File Offset: 0x0002B11D
		public override void WriteValue(bool value)
		{
			base.WriteValue(value);
			this.AddJValue(new JValue(value), JsonToken.Boolean);
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x0002CF34 File Offset: 0x0002B134
		public override void WriteValue(short value)
		{
			base.WriteValue(value);
			this.AddRawValue(value, JTokenType.Integer, JsonToken.Integer);
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x0002CF4B File Offset: 0x0002B14B
		[CLSCompliant(false)]
		public override void WriteValue(ushort value)
		{
			base.WriteValue(value);
			this.AddRawValue(value, JTokenType.Integer, JsonToken.Integer);
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x0002CF64 File Offset: 0x0002B164
		public override void WriteValue(char value)
		{
			base.WriteValue(value);
			string text = value.ToString(CultureInfo.InvariantCulture);
			this.AddJValue(new JValue(text), JsonToken.String);
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x0002CF93 File Offset: 0x0002B193
		public override void WriteValue(byte value)
		{
			base.WriteValue(value);
			this.AddRawValue(value, JTokenType.Integer, JsonToken.Integer);
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x0002CFAA File Offset: 0x0002B1AA
		[CLSCompliant(false)]
		public override void WriteValue(sbyte value)
		{
			base.WriteValue(value);
			this.AddRawValue(value, JTokenType.Integer, JsonToken.Integer);
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x0002CFC1 File Offset: 0x0002B1C1
		public override void WriteValue(decimal value)
		{
			base.WriteValue(value);
			this.AddJValue(new JValue(value), JsonToken.Float);
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x0002CFD7 File Offset: 0x0002B1D7
		public override void WriteValue(DateTime value)
		{
			base.WriteValue(value);
			value = DateTimeUtils.EnsureDateTime(value, base.DateTimeZoneHandling);
			this.AddJValue(new JValue(value), JsonToken.Date);
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x0002CFFC File Offset: 0x0002B1FC
		public override void WriteValue(DateTimeOffset value)
		{
			base.WriteValue(value);
			this.AddJValue(new JValue(value), JsonToken.Date);
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x0002D013 File Offset: 0x0002B213
		public override void WriteValue(byte[] value)
		{
			base.WriteValue(value);
			this.AddJValue(new JValue(value, JTokenType.Bytes), JsonToken.Bytes);
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x0002D02C File Offset: 0x0002B22C
		public override void WriteValue(TimeSpan value)
		{
			base.WriteValue(value);
			this.AddJValue(new JValue(value), JsonToken.String);
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x0002D043 File Offset: 0x0002B243
		public override void WriteValue(Guid value)
		{
			base.WriteValue(value);
			this.AddJValue(new JValue(value), JsonToken.String);
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x0002D05A File Offset: 0x0002B25A
		public override void WriteValue(Uri value)
		{
			base.WriteValue(value);
			this.AddJValue(new JValue(value), JsonToken.String);
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x0002D074 File Offset: 0x0002B274
		[NullableContext(1)]
		internal override void WriteToken(JsonReader reader, bool writeChildren, bool writeDateConstructorAsDate, bool writeComments)
		{
			JTokenReader jtokenReader = reader as JTokenReader;
			if (jtokenReader == null || !writeChildren || !writeDateConstructorAsDate || !writeComments)
			{
				base.WriteToken(reader, writeChildren, writeDateConstructorAsDate, writeComments);
				return;
			}
			if (jtokenReader.TokenType == JsonToken.None && !jtokenReader.Read())
			{
				return;
			}
			JToken jtoken = jtokenReader.CurrentToken.CloneToken(null);
			if (this._parent != null)
			{
				this._parent.Add(jtoken);
				this._current = this._parent.Last;
				if (this._parent.Type == JTokenType.Property)
				{
					this._parent = this._parent.Parent;
					base.InternalWriteValue(JsonToken.Null);
				}
			}
			else
			{
				this._current = jtoken;
				if (this._token == null && this._value == null)
				{
					this._token = jtoken as JContainer;
					this._value = jtoken as JValue;
				}
			}
			jtokenReader.Skip();
		}

		// Token: 0x040003B3 RID: 947
		private JContainer _token;

		// Token: 0x040003B4 RID: 948
		private JContainer _parent;

		// Token: 0x040003B5 RID: 949
		private JValue _value;

		// Token: 0x040003B6 RID: 950
		private JToken _current;
	}
}
