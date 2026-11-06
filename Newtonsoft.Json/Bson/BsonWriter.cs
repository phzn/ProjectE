using System;
using System.Globalization;
using System.IO;
using System.Numerics;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x02000110 RID: 272
	[Obsolete("BSON reading and writing has been moved to its own package. See https://www.nuget.org/packages/Newtonsoft.Json.Bson for more details.")]
	public class BsonWriter : JsonWriter
	{
		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000DA4 RID: 3492 RVA: 0x00035EC1 File Offset: 0x000340C1
		// (set) Token: 0x06000DA5 RID: 3493 RVA: 0x00035ECE File Offset: 0x000340CE
		public DateTimeKind DateTimeKindHandling
		{
			get
			{
				return this._writer.DateTimeKindHandling;
			}
			set
			{
				this._writer.DateTimeKindHandling = value;
			}
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x00035EDC File Offset: 0x000340DC
		public BsonWriter(Stream stream)
		{
			ValidationUtils.ArgumentNotNull(stream, "stream");
			this._writer = new BsonBinaryWriter(new BinaryWriter(stream));
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x00035F00 File Offset: 0x00034100
		public BsonWriter(BinaryWriter writer)
		{
			ValidationUtils.ArgumentNotNull(writer, "writer");
			this._writer = new BsonBinaryWriter(writer);
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x00035F1F File Offset: 0x0003411F
		public override void Flush()
		{
			this._writer.Flush();
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x00035F2C File Offset: 0x0003412C
		protected override void WriteEnd(JsonToken token)
		{
			base.WriteEnd(token);
			this.RemoveParent();
			if (base.Top == 0)
			{
				this._writer.WriteToken(this._root);
			}
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x00035F54 File Offset: 0x00034154
		public override void WriteComment(string text)
		{
			throw JsonWriterException.Create(this, "Cannot write JSON comment as BSON.", null);
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x00035F62 File Offset: 0x00034162
		public override void WriteStartConstructor(string name)
		{
			throw JsonWriterException.Create(this, "Cannot write JSON constructor as BSON.", null);
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x00035F70 File Offset: 0x00034170
		public override void WriteRaw(string json)
		{
			throw JsonWriterException.Create(this, "Cannot write raw JSON as BSON.", null);
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x00035F7E File Offset: 0x0003417E
		public override void WriteRawValue(string json)
		{
			throw JsonWriterException.Create(this, "Cannot write raw JSON as BSON.", null);
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x00035F8C File Offset: 0x0003418C
		public override void WriteStartArray()
		{
			base.WriteStartArray();
			this.AddParent(new BsonArray());
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x00035F9F File Offset: 0x0003419F
		public override void WriteStartObject()
		{
			base.WriteStartObject();
			this.AddParent(new BsonObject());
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x00035FB2 File Offset: 0x000341B2
		public override void WritePropertyName(string name)
		{
			base.WritePropertyName(name);
			this._propertyName = name;
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x00035FC2 File Offset: 0x000341C2
		public override void Close()
		{
			base.Close();
			if (base.CloseOutput)
			{
				BsonBinaryWriter writer = this._writer;
				if (writer == null)
				{
					return;
				}
				writer.Close();
			}
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x00035FE2 File Offset: 0x000341E2
		private void AddParent(BsonToken container)
		{
			this.AddToken(container);
			this._parent = container;
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x00035FF2 File Offset: 0x000341F2
		private void RemoveParent()
		{
			this._parent = this._parent.Parent;
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x00036005 File Offset: 0x00034205
		private void AddValue(object value, BsonType type)
		{
			this.AddToken(new BsonValue(value, type));
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x00036014 File Offset: 0x00034214
		internal void AddToken(BsonToken token)
		{
			if (this._parent != null)
			{
				BsonObject bsonObject = this._parent as BsonObject;
				if (bsonObject != null)
				{
					bsonObject.Add(this._propertyName, token);
					this._propertyName = null;
					return;
				}
				((BsonArray)this._parent).Add(token);
				return;
			}
			else
			{
				if (token.Type != BsonType.Object && token.Type != BsonType.Array)
				{
					throw JsonWriterException.Create(this, "Error writing {0} value. BSON must start with an Object or Array.".FormatWith(CultureInfo.InvariantCulture, token.Type), null);
				}
				this._parent = token;
				this._root = token;
				return;
			}
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x000360A4 File Offset: 0x000342A4
		public override void WriteValue(object value)
		{
			if (value is BigInteger)
			{
				BigInteger bigInteger = (BigInteger)value;
				base.SetWriteState(JsonToken.Integer, null);
				this.AddToken(new BsonBinary(bigInteger.ToByteArray(), BsonBinaryType.Binary));
				return;
			}
			base.WriteValue(value);
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x000360E3 File Offset: 0x000342E3
		public override void WriteNull()
		{
			base.WriteNull();
			this.AddToken(BsonEmpty.Null);
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x000360F6 File Offset: 0x000342F6
		public override void WriteUndefined()
		{
			base.WriteUndefined();
			this.AddToken(BsonEmpty.Undefined);
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x00036109 File Offset: 0x00034309
		public override void WriteValue(string value)
		{
			base.WriteValue(value);
			this.AddToken((value == null) ? BsonEmpty.Null : new BsonString(value, true));
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x00036129 File Offset: 0x00034329
		public override void WriteValue(int value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x00036140 File Offset: 0x00034340
		[CLSCompliant(false)]
		public override void WriteValue(uint value)
		{
			if (value > 2147483647U)
			{
				throw JsonWriterException.Create(this, "Value is too large to fit in a signed 32 bit integer. BSON does not support unsigned values.", null);
			}
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x0003616C File Offset: 0x0003436C
		public override void WriteValue(long value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Long);
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x00036183 File Offset: 0x00034383
		[CLSCompliant(false)]
		public override void WriteValue(ulong value)
		{
			if (value > 9223372036854775807UL)
			{
				throw JsonWriterException.Create(this, "Value is too large to fit in a signed 64 bit integer. BSON does not support unsigned values.", null);
			}
			base.WriteValue(value);
			this.AddValue(value, BsonType.Long);
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x000361B3 File Offset: 0x000343B3
		public override void WriteValue(float value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Number);
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x000361C9 File Offset: 0x000343C9
		public override void WriteValue(double value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Number);
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x000361DF File Offset: 0x000343DF
		public override void WriteValue(bool value)
		{
			base.WriteValue(value);
			this.AddToken(value ? BsonBoolean.True : BsonBoolean.False);
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x000361FD File Offset: 0x000343FD
		public override void WriteValue(short value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x00036214 File Offset: 0x00034414
		[CLSCompliant(false)]
		public override void WriteValue(ushort value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x0003622C File Offset: 0x0003442C
		public override void WriteValue(char value)
		{
			base.WriteValue(value);
			string text = value.ToString(CultureInfo.InvariantCulture);
			this.AddToken(new BsonString(text, true));
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x0003625C File Offset: 0x0003445C
		public override void WriteValue(byte value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x00036273 File Offset: 0x00034473
		[CLSCompliant(false)]
		public override void WriteValue(sbyte value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x0003628A File Offset: 0x0003448A
		public override void WriteValue(decimal value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Number);
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x000362A0 File Offset: 0x000344A0
		public override void WriteValue(DateTime value)
		{
			base.WriteValue(value);
			value = DateTimeUtils.EnsureDateTime(value, base.DateTimeZoneHandling);
			this.AddValue(value, BsonType.Date);
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x000362C5 File Offset: 0x000344C5
		public override void WriteValue(DateTimeOffset value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Date);
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x000362DC File Offset: 0x000344DC
		public override void WriteValue(byte[] value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			base.WriteValue(value);
			this.AddToken(new BsonBinary(value, BsonBinaryType.Binary));
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x000362FC File Offset: 0x000344FC
		public override void WriteValue(Guid value)
		{
			base.WriteValue(value);
			this.AddToken(new BsonBinary(value.ToByteArray(), BsonBinaryType.Uuid));
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x00036318 File Offset: 0x00034518
		public override void WriteValue(TimeSpan value)
		{
			base.WriteValue(value);
			this.AddToken(new BsonString(value.ToString(), true));
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x0003633A File Offset: 0x0003453A
		public override void WriteValue(Uri value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			base.WriteValue(value);
			this.AddToken(new BsonString(value.ToString(), true));
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x00036365 File Offset: 0x00034565
		public void WriteObjectId(byte[] value)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			if (value.Length != 12)
			{
				throw JsonWriterException.Create(this, "An object id must be 12 bytes", null);
			}
			base.SetWriteState(JsonToken.Undefined, null);
			this.AddValue(value, BsonType.Oid);
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x00036397 File Offset: 0x00034597
		public void WriteRegex(string pattern, string options)
		{
			ValidationUtils.ArgumentNotNull(pattern, "pattern");
			base.SetWriteState(JsonToken.Undefined, null);
			this.AddToken(new BsonRegex(pattern, options));
		}

		// Token: 0x0400045A RID: 1114
		private readonly BsonBinaryWriter _writer;

		// Token: 0x0400045B RID: 1115
		private BsonToken _root;

		// Token: 0x0400045C RID: 1116
		private BsonToken _parent;

		// Token: 0x0400045D RID: 1117
		private string _propertyName;
	}
}
