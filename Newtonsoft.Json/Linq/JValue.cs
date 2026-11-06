using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Globalization;
using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000C8 RID: 200
	[NullableContext(2)]
	[Nullable(0)]
	public class JValue : JToken, IEquatable<JValue>, IFormattable, IComparable, IComparable<JValue>, IConvertible
	{
		// Token: 0x06000B90 RID: 2960 RVA: 0x0002D148 File Offset: 0x0002B348
		[NullableContext(1)]
		public override Task WriteToAsync(JsonWriter writer, CancellationToken cancellationToken, params JsonConverter[] converters)
		{
			if (converters != null && converters.Length != 0 && this._value != null)
			{
				JsonConverter matchingConverter = JsonSerializer.GetMatchingConverter(converters, this._value.GetType());
				if (matchingConverter != null && matchingConverter.CanWrite)
				{
					matchingConverter.WriteJson(writer, this._value, JsonSerializer.CreateDefault());
					return AsyncUtils.CompletedTask;
				}
			}
			switch (this._valueType)
			{
			case JTokenType.Comment:
			{
				object value = this._value;
				return writer.WriteCommentAsync((value != null) ? value.ToString() : null, cancellationToken);
			}
			case JTokenType.Integer:
			{
				object obj = this._value;
				if (obj is int)
				{
					int num = (int)obj;
					return writer.WriteValueAsync(num, cancellationToken);
				}
				obj = this._value;
				if (obj is long)
				{
					long num2 = (long)obj;
					return writer.WriteValueAsync(num2, cancellationToken);
				}
				obj = this._value;
				if (obj is ulong)
				{
					ulong num3 = (ulong)obj;
					return writer.WriteValueAsync(num3, cancellationToken);
				}
				obj = this._value;
				if (obj is BigInteger)
				{
					BigInteger bigInteger = (BigInteger)obj;
					return writer.WriteValueAsync(bigInteger, cancellationToken);
				}
				return writer.WriteValueAsync(Convert.ToInt64(this._value, CultureInfo.InvariantCulture), cancellationToken);
			}
			case JTokenType.Float:
			{
				object obj = this._value;
				if (obj is decimal)
				{
					decimal num4 = (decimal)obj;
					return writer.WriteValueAsync(num4, cancellationToken);
				}
				obj = this._value;
				if (obj is double)
				{
					double num5 = (double)obj;
					return writer.WriteValueAsync(num5, cancellationToken);
				}
				obj = this._value;
				if (obj is float)
				{
					float num6 = (float)obj;
					return writer.WriteValueAsync(num6, cancellationToken);
				}
				return writer.WriteValueAsync(Convert.ToDouble(this._value, CultureInfo.InvariantCulture), cancellationToken);
			}
			case JTokenType.String:
			{
				object value2 = this._value;
				return writer.WriteValueAsync((value2 != null) ? value2.ToString() : null, cancellationToken);
			}
			case JTokenType.Boolean:
				return writer.WriteValueAsync(Convert.ToBoolean(this._value, CultureInfo.InvariantCulture), cancellationToken);
			case JTokenType.Null:
				return writer.WriteNullAsync(cancellationToken);
			case JTokenType.Undefined:
				return writer.WriteUndefinedAsync(cancellationToken);
			case JTokenType.Date:
			{
				object obj = this._value;
				if (obj is DateTimeOffset)
				{
					DateTimeOffset dateTimeOffset = (DateTimeOffset)obj;
					return writer.WriteValueAsync(dateTimeOffset, cancellationToken);
				}
				return writer.WriteValueAsync(Convert.ToDateTime(this._value, CultureInfo.InvariantCulture), cancellationToken);
			}
			case JTokenType.Raw:
			{
				object value3 = this._value;
				return writer.WriteRawValueAsync((value3 != null) ? value3.ToString() : null, cancellationToken);
			}
			case JTokenType.Bytes:
				return writer.WriteValueAsync((byte[])this._value, cancellationToken);
			case JTokenType.Guid:
				return writer.WriteValueAsync((this._value != null) ? ((Guid?)this._value) : null, cancellationToken);
			case JTokenType.Uri:
				return writer.WriteValueAsync((Uri)this._value, cancellationToken);
			case JTokenType.TimeSpan:
				return writer.WriteValueAsync((this._value != null) ? ((TimeSpan?)this._value) : null, cancellationToken);
			default:
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException("Type", this._valueType, "Unexpected token type.");
			}
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x0002D44E File Offset: 0x0002B64E
		internal JValue(object value, JTokenType type)
		{
			this._value = value;
			this._valueType = type;
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x0002D464 File Offset: 0x0002B664
		[NullableContext(1)]
		internal JValue(JValue other, [Nullable(2)] JsonCloneSettings settings)
			: this(other.Value, other.Type)
		{
			if (settings == null || settings.CopyAnnotations)
			{
				base.CopyAnnotations(this, other);
			}
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x0002D48E File Offset: 0x0002B68E
		[NullableContext(1)]
		public JValue(JValue other)
			: this(other.Value, other.Type)
		{
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x0002D4A2 File Offset: 0x0002B6A2
		public JValue(long value)
			: this(BoxedPrimitives.Get(value), JTokenType.Integer)
		{
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x0002D4B1 File Offset: 0x0002B6B1
		public JValue(decimal value)
			: this(BoxedPrimitives.Get(value), JTokenType.Float)
		{
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x0002D4C0 File Offset: 0x0002B6C0
		public JValue(char value)
			: this(value, JTokenType.String)
		{
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x0002D4CF File Offset: 0x0002B6CF
		[CLSCompliant(false)]
		public JValue(ulong value)
			: this(value, JTokenType.Integer)
		{
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x0002D4DE File Offset: 0x0002B6DE
		public JValue(double value)
			: this(BoxedPrimitives.Get(value), JTokenType.Float)
		{
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x0002D4ED File Offset: 0x0002B6ED
		public JValue(float value)
			: this(value, JTokenType.Float)
		{
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x0002D4FC File Offset: 0x0002B6FC
		public JValue(DateTime value)
			: this(value, JTokenType.Date)
		{
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x0002D50C File Offset: 0x0002B70C
		public JValue(DateTimeOffset value)
			: this(value, JTokenType.Date)
		{
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x0002D51C File Offset: 0x0002B71C
		public JValue(bool value)
			: this(BoxedPrimitives.Get(value), JTokenType.Boolean)
		{
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x0002D52C File Offset: 0x0002B72C
		public JValue(string value)
			: this(value, JTokenType.String)
		{
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x0002D536 File Offset: 0x0002B736
		public JValue(Guid value)
			: this(value, JTokenType.Guid)
		{
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x0002D546 File Offset: 0x0002B746
		public JValue(Uri value)
			: this(value, (value != null) ? JTokenType.Uri : JTokenType.Null)
		{
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x0002D55E File Offset: 0x0002B75E
		public JValue(TimeSpan value)
			: this(value, JTokenType.TimeSpan)
		{
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x0002D570 File Offset: 0x0002B770
		public JValue(object value)
			: this(value, JValue.GetValueType(null, value))
		{
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x0002D594 File Offset: 0x0002B794
		[NullableContext(1)]
		internal override bool DeepEquals(JToken node)
		{
			JValue jvalue = node as JValue;
			return jvalue != null && (jvalue == this || JValue.ValuesEquals(this, jvalue));
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000BA3 RID: 2979 RVA: 0x0002D5BA File Offset: 0x0002B7BA
		public override bool HasValues
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x0002D5C0 File Offset: 0x0002B7C0
		[NullableContext(1)]
		private static int CompareBigInteger(BigInteger i1, object i2)
		{
			int num = i1.CompareTo(ConvertUtils.ToBigInteger(i2));
			if (num != 0)
			{
				return num;
			}
			if (i2 is decimal)
			{
				decimal num2 = (decimal)i2;
				return 0m.CompareTo(Math.Abs(num2 - Math.Truncate(num2)));
			}
			if (i2 is double || i2 is float)
			{
				double num3 = Convert.ToDouble(i2, CultureInfo.InvariantCulture);
				return 0.0.CompareTo(Math.Abs(num3 - Math.Truncate(num3)));
			}
			return num;
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x0002D64C File Offset: 0x0002B84C
		internal static int Compare(JTokenType valueType, object objA, object objB)
		{
			if (objA == objB)
			{
				return 0;
			}
			if (objB == null)
			{
				return 1;
			}
			if (objA == null)
			{
				return -1;
			}
			switch (valueType)
			{
			case JTokenType.Comment:
			case JTokenType.String:
			case JTokenType.Raw:
			{
				string text = Convert.ToString(objA, CultureInfo.InvariantCulture);
				string text2 = Convert.ToString(objB, CultureInfo.InvariantCulture);
				return string.CompareOrdinal(text, text2);
			}
			case JTokenType.Integer:
				if (objA is BigInteger)
				{
					BigInteger bigInteger = (BigInteger)objA;
					return JValue.CompareBigInteger(bigInteger, objB);
				}
				if (objB is BigInteger)
				{
					BigInteger bigInteger2 = (BigInteger)objB;
					return -JValue.CompareBigInteger(bigInteger2, objA);
				}
				if (objA is ulong || objB is ulong || objA is decimal || objB is decimal)
				{
					return Convert.ToDecimal(objA, CultureInfo.InvariantCulture).CompareTo(Convert.ToDecimal(objB, CultureInfo.InvariantCulture));
				}
				if (objA is float || objB is float || objA is double || objB is double)
				{
					return JValue.CompareFloat(objA, objB);
				}
				return Convert.ToInt64(objA, CultureInfo.InvariantCulture).CompareTo(Convert.ToInt64(objB, CultureInfo.InvariantCulture));
			case JTokenType.Float:
				if (objA is BigInteger)
				{
					BigInteger bigInteger3 = (BigInteger)objA;
					return JValue.CompareBigInteger(bigInteger3, objB);
				}
				if (objB is BigInteger)
				{
					BigInteger bigInteger4 = (BigInteger)objB;
					return -JValue.CompareBigInteger(bigInteger4, objA);
				}
				if (objA is ulong || objB is ulong || objA is decimal || objB is decimal)
				{
					return Convert.ToDecimal(objA, CultureInfo.InvariantCulture).CompareTo(Convert.ToDecimal(objB, CultureInfo.InvariantCulture));
				}
				return JValue.CompareFloat(objA, objB);
			case JTokenType.Boolean:
			{
				bool flag = Convert.ToBoolean(objA, CultureInfo.InvariantCulture);
				bool flag2 = Convert.ToBoolean(objB, CultureInfo.InvariantCulture);
				return flag.CompareTo(flag2);
			}
			case JTokenType.Date:
			{
				if (objA is DateTime)
				{
					DateTime dateTime = (DateTime)objA;
					DateTime dateTime2;
					if (objB is DateTimeOffset)
					{
						dateTime2 = ((DateTimeOffset)objB).DateTime;
					}
					else
					{
						dateTime2 = Convert.ToDateTime(objB, CultureInfo.InvariantCulture);
					}
					return dateTime.CompareTo(dateTime2);
				}
				DateTimeOffset dateTimeOffset = (DateTimeOffset)objA;
				DateTimeOffset dateTimeOffset2;
				if (objB is DateTimeOffset)
				{
					dateTimeOffset2 = (DateTimeOffset)objB;
				}
				else
				{
					dateTimeOffset2 = new DateTimeOffset(Convert.ToDateTime(objB, CultureInfo.InvariantCulture));
				}
				return dateTimeOffset.CompareTo(dateTimeOffset2);
			}
			case JTokenType.Bytes:
			{
				byte[] array = objB as byte[];
				if (array == null)
				{
					throw new ArgumentException("Object must be of type byte[].");
				}
				return MiscellaneousUtils.ByteArrayCompare(objA as byte[], array);
			}
			case JTokenType.Guid:
			{
				if (!(objB is Guid))
				{
					throw new ArgumentException("Object must be of type Guid.");
				}
				Guid guid = (Guid)objA;
				Guid guid2 = (Guid)objB;
				return guid.CompareTo(guid2);
			}
			case JTokenType.Uri:
			{
				Uri uri = objB as Uri;
				if (uri == null)
				{
					throw new ArgumentException("Object must be of type Uri.");
				}
				Uri uri2 = (Uri)objA;
				return Comparer<string>.Default.Compare(uri2.ToString(), uri.ToString());
			}
			case JTokenType.TimeSpan:
			{
				if (!(objB is TimeSpan))
				{
					throw new ArgumentException("Object must be of type TimeSpan.");
				}
				TimeSpan timeSpan = (TimeSpan)objA;
				TimeSpan timeSpan2 = (TimeSpan)objB;
				return timeSpan.CompareTo(timeSpan2);
			}
			}
			throw MiscellaneousUtils.CreateArgumentOutOfRangeException("valueType", valueType, "Unexpected value type: {0}".FormatWith(CultureInfo.InvariantCulture, valueType));
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x0002D978 File Offset: 0x0002BB78
		[NullableContext(1)]
		private static int CompareFloat(object objA, object objB)
		{
			double num = Convert.ToDouble(objA, CultureInfo.InvariantCulture);
			double num2 = Convert.ToDouble(objB, CultureInfo.InvariantCulture);
			if (MathUtils.ApproxEquals(num, num2))
			{
				return 0;
			}
			return num.CompareTo(num2);
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x0002D9B0 File Offset: 0x0002BBB0
		private static bool Operation(ExpressionType operation, object objA, object objB, out object result)
		{
			if ((objA is string || objB is string) && (operation == ExpressionType.Add || operation == ExpressionType.AddAssign))
			{
				result = ((objA != null) ? objA.ToString() : null) + ((objB != null) ? objB.ToString() : null);
				return true;
			}
			if (objA is BigInteger || objB is BigInteger)
			{
				if (objA == null || objB == null)
				{
					result = null;
					return true;
				}
				BigInteger bigInteger = ConvertUtils.ToBigInteger(objA);
				BigInteger bigInteger2 = ConvertUtils.ToBigInteger(objB);
				if (operation <= ExpressionType.Subtract)
				{
					if (operation <= ExpressionType.Divide)
					{
						if (operation != ExpressionType.Add)
						{
							if (operation != ExpressionType.Divide)
							{
								goto IL_0393;
							}
							goto IL_00DE;
						}
					}
					else
					{
						if (operation == ExpressionType.Multiply)
						{
							goto IL_00CE;
						}
						if (operation != ExpressionType.Subtract)
						{
							goto IL_0393;
						}
						goto IL_00BE;
					}
				}
				else if (operation <= ExpressionType.DivideAssign)
				{
					if (operation != ExpressionType.AddAssign)
					{
						if (operation != ExpressionType.DivideAssign)
						{
							goto IL_0393;
						}
						goto IL_00DE;
					}
				}
				else
				{
					if (operation == ExpressionType.MultiplyAssign)
					{
						goto IL_00CE;
					}
					if (operation != ExpressionType.SubtractAssign)
					{
						goto IL_0393;
					}
					goto IL_00BE;
				}
				result = bigInteger + bigInteger2;
				return true;
				IL_00BE:
				result = bigInteger - bigInteger2;
				return true;
				IL_00CE:
				result = bigInteger * bigInteger2;
				return true;
				IL_00DE:
				result = bigInteger / bigInteger2;
				return true;
			}
			else if (objA is ulong || objB is ulong || objA is decimal || objB is decimal)
			{
				if (objA == null || objB == null)
				{
					result = null;
					return true;
				}
				decimal num = Convert.ToDecimal(objA, CultureInfo.InvariantCulture);
				decimal num2 = Convert.ToDecimal(objB, CultureInfo.InvariantCulture);
				if (operation <= ExpressionType.Subtract)
				{
					if (operation <= ExpressionType.Divide)
					{
						if (operation != ExpressionType.Add)
						{
							if (operation != ExpressionType.Divide)
							{
								goto IL_0393;
							}
							goto IL_01AD;
						}
					}
					else
					{
						if (operation == ExpressionType.Multiply)
						{
							goto IL_019D;
						}
						if (operation != ExpressionType.Subtract)
						{
							goto IL_0393;
						}
						goto IL_018D;
					}
				}
				else if (operation <= ExpressionType.DivideAssign)
				{
					if (operation != ExpressionType.AddAssign)
					{
						if (operation != ExpressionType.DivideAssign)
						{
							goto IL_0393;
						}
						goto IL_01AD;
					}
				}
				else
				{
					if (operation == ExpressionType.MultiplyAssign)
					{
						goto IL_019D;
					}
					if (operation != ExpressionType.SubtractAssign)
					{
						goto IL_0393;
					}
					goto IL_018D;
				}
				result = num + num2;
				return true;
				IL_018D:
				result = num - num2;
				return true;
				IL_019D:
				result = num * num2;
				return true;
				IL_01AD:
				result = num / num2;
				return true;
			}
			else if (objA is float || objB is float || objA is double || objB is double)
			{
				if (objA == null || objB == null)
				{
					result = null;
					return true;
				}
				double num3 = Convert.ToDouble(objA, CultureInfo.InvariantCulture);
				double num4 = Convert.ToDouble(objB, CultureInfo.InvariantCulture);
				if (operation <= ExpressionType.Subtract)
				{
					if (operation <= ExpressionType.Divide)
					{
						if (operation != ExpressionType.Add)
						{
							if (operation != ExpressionType.Divide)
							{
								goto IL_0393;
							}
							goto IL_0278;
						}
					}
					else
					{
						if (operation == ExpressionType.Multiply)
						{
							goto IL_026A;
						}
						if (operation != ExpressionType.Subtract)
						{
							goto IL_0393;
						}
						goto IL_025C;
					}
				}
				else if (operation <= ExpressionType.DivideAssign)
				{
					if (operation != ExpressionType.AddAssign)
					{
						if (operation != ExpressionType.DivideAssign)
						{
							goto IL_0393;
						}
						goto IL_0278;
					}
				}
				else
				{
					if (operation == ExpressionType.MultiplyAssign)
					{
						goto IL_026A;
					}
					if (operation != ExpressionType.SubtractAssign)
					{
						goto IL_0393;
					}
					goto IL_025C;
				}
				result = num3 + num4;
				return true;
				IL_025C:
				result = num3 - num4;
				return true;
				IL_026A:
				result = num3 * num4;
				return true;
				IL_0278:
				result = num3 / num4;
				return true;
			}
			else if (objA is int || objA is uint || objA is long || objA is short || objA is ushort || objA is sbyte || objA is byte || objB is int || objB is uint || objB is long || objB is short || objB is ushort || objB is sbyte || objB is byte)
			{
				if (objA == null || objB == null)
				{
					result = null;
					return true;
				}
				long num5 = Convert.ToInt64(objA, CultureInfo.InvariantCulture);
				long num6 = Convert.ToInt64(objB, CultureInfo.InvariantCulture);
				if (operation <= ExpressionType.Subtract)
				{
					if (operation <= ExpressionType.Divide)
					{
						if (operation != ExpressionType.Add)
						{
							if (operation != ExpressionType.Divide)
							{
								goto IL_0393;
							}
							goto IL_0385;
						}
					}
					else
					{
						if (operation == ExpressionType.Multiply)
						{
							goto IL_0377;
						}
						if (operation != ExpressionType.Subtract)
						{
							goto IL_0393;
						}
						goto IL_0369;
					}
				}
				else if (operation <= ExpressionType.DivideAssign)
				{
					if (operation != ExpressionType.AddAssign)
					{
						if (operation != ExpressionType.DivideAssign)
						{
							goto IL_0393;
						}
						goto IL_0385;
					}
				}
				else
				{
					if (operation == ExpressionType.MultiplyAssign)
					{
						goto IL_0377;
					}
					if (operation != ExpressionType.SubtractAssign)
					{
						goto IL_0393;
					}
					goto IL_0369;
				}
				result = num5 + num6;
				return true;
				IL_0369:
				result = num5 - num6;
				return true;
				IL_0377:
				result = num5 * num6;
				return true;
				IL_0385:
				result = num5 / num6;
				return true;
			}
			IL_0393:
			result = null;
			return false;
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x0002DD54 File Offset: 0x0002BF54
		[NullableContext(1)]
		internal override JToken CloneToken([Nullable(2)] JsonCloneSettings settings)
		{
			return new JValue(this, settings);
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x0002DD5D File Offset: 0x0002BF5D
		[NullableContext(1)]
		public static JValue CreateComment([Nullable(2)] string value)
		{
			return new JValue(value, JTokenType.Comment);
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x0002DD66 File Offset: 0x0002BF66
		[NullableContext(1)]
		public static JValue CreateString([Nullable(2)] string value)
		{
			return new JValue(value, JTokenType.String);
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x0002DD6F File Offset: 0x0002BF6F
		[NullableContext(1)]
		public static JValue CreateNull()
		{
			return new JValue(null, JTokenType.Null);
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x0002DD79 File Offset: 0x0002BF79
		[NullableContext(1)]
		public static JValue CreateUndefined()
		{
			return new JValue(null, JTokenType.Undefined);
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x0002DD84 File Offset: 0x0002BF84
		private static JTokenType GetValueType(JTokenType? current, object value)
		{
			if (value == null)
			{
				return JTokenType.Null;
			}
			if (value == DBNull.Value)
			{
				return JTokenType.Null;
			}
			if (value is string)
			{
				return JValue.GetStringValueType(current);
			}
			if (value is long || value is int || value is short || value is sbyte || value is ulong || value is uint || value is ushort || value is byte)
			{
				return JTokenType.Integer;
			}
			if (value is Enum)
			{
				return JTokenType.Integer;
			}
			if (value is BigInteger)
			{
				return JTokenType.Integer;
			}
			if (value is double || value is float || value is decimal)
			{
				return JTokenType.Float;
			}
			if (value is DateTime)
			{
				return JTokenType.Date;
			}
			if (value is DateTimeOffset)
			{
				return JTokenType.Date;
			}
			if (value is byte[])
			{
				return JTokenType.Bytes;
			}
			if (value is bool)
			{
				return JTokenType.Boolean;
			}
			if (value is Guid)
			{
				return JTokenType.Guid;
			}
			if (value is Uri)
			{
				return JTokenType.Uri;
			}
			if (value is TimeSpan)
			{
				return JTokenType.TimeSpan;
			}
			throw new ArgumentException("Could not determine JSON object type for type {0}.".FormatWith(CultureInfo.InvariantCulture, value.GetType()));
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x0002DE88 File Offset: 0x0002C088
		private static JTokenType GetStringValueType(JTokenType? current)
		{
			if (current == null)
			{
				return JTokenType.String;
			}
			JTokenType valueOrDefault = current.GetValueOrDefault();
			if (valueOrDefault == JTokenType.Comment || valueOrDefault == JTokenType.String || valueOrDefault == JTokenType.Raw)
			{
				return current.GetValueOrDefault();
			}
			return JTokenType.String;
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000BAF RID: 2991 RVA: 0x0002DEBE File Offset: 0x0002C0BE
		public override JTokenType Type
		{
			get
			{
				return this._valueType;
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000BB0 RID: 2992 RVA: 0x0002DEC6 File Offset: 0x0002C0C6
		// (set) Token: 0x06000BB1 RID: 2993 RVA: 0x0002DED0 File Offset: 0x0002C0D0
		public new object Value
		{
			get
			{
				return this._value;
			}
			set
			{
				object value2 = this._value;
				Type type = ((value2 != null) ? value2.GetType() : null);
				Type type2 = ((value != null) ? value.GetType() : null);
				if (type != type2)
				{
					this._valueType = JValue.GetValueType(new JTokenType?(this._valueType), value);
				}
				this._value = value;
			}
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x0002DF24 File Offset: 0x0002C124
		[NullableContext(1)]
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			if (converters != null && converters.Length != 0 && this._value != null)
			{
				JsonConverter matchingConverter = JsonSerializer.GetMatchingConverter(converters, this._value.GetType());
				if (matchingConverter != null && matchingConverter.CanWrite)
				{
					matchingConverter.WriteJson(writer, this._value, JsonSerializer.CreateDefault());
					return;
				}
			}
			switch (this._valueType)
			{
			case JTokenType.Comment:
			{
				object value = this._value;
				writer.WriteComment((value != null) ? value.ToString() : null);
				return;
			}
			case JTokenType.Integer:
			{
				object obj = this._value;
				if (obj is int)
				{
					int num = (int)obj;
					writer.WriteValue(num);
					return;
				}
				obj = this._value;
				if (obj is long)
				{
					long num2 = (long)obj;
					writer.WriteValue(num2);
					return;
				}
				obj = this._value;
				if (obj is ulong)
				{
					ulong num3 = (ulong)obj;
					writer.WriteValue(num3);
					return;
				}
				obj = this._value;
				if (obj is BigInteger)
				{
					BigInteger bigInteger = (BigInteger)obj;
					writer.WriteValue(bigInteger);
					return;
				}
				writer.WriteValue(Convert.ToInt64(this._value, CultureInfo.InvariantCulture));
				return;
			}
			case JTokenType.Float:
			{
				object obj = this._value;
				if (obj is decimal)
				{
					decimal num4 = (decimal)obj;
					writer.WriteValue(num4);
					return;
				}
				obj = this._value;
				if (obj is double)
				{
					double num5 = (double)obj;
					writer.WriteValue(num5);
					return;
				}
				obj = this._value;
				if (obj is float)
				{
					float num6 = (float)obj;
					writer.WriteValue(num6);
					return;
				}
				writer.WriteValue(Convert.ToDouble(this._value, CultureInfo.InvariantCulture));
				return;
			}
			case JTokenType.String:
			{
				object value2 = this._value;
				writer.WriteValue((value2 != null) ? value2.ToString() : null);
				return;
			}
			case JTokenType.Boolean:
				writer.WriteValue(Convert.ToBoolean(this._value, CultureInfo.InvariantCulture));
				return;
			case JTokenType.Null:
				writer.WriteNull();
				return;
			case JTokenType.Undefined:
				writer.WriteUndefined();
				return;
			case JTokenType.Date:
			{
				object obj = this._value;
				if (obj is DateTimeOffset)
				{
					DateTimeOffset dateTimeOffset = (DateTimeOffset)obj;
					writer.WriteValue(dateTimeOffset);
					return;
				}
				writer.WriteValue(Convert.ToDateTime(this._value, CultureInfo.InvariantCulture));
				return;
			}
			case JTokenType.Raw:
			{
				object value3 = this._value;
				writer.WriteRawValue((value3 != null) ? value3.ToString() : null);
				return;
			}
			case JTokenType.Bytes:
				writer.WriteValue((byte[])this._value);
				return;
			case JTokenType.Guid:
				writer.WriteValue((this._value != null) ? ((Guid?)this._value) : null);
				return;
			case JTokenType.Uri:
				writer.WriteValue((Uri)this._value);
				return;
			case JTokenType.TimeSpan:
				writer.WriteValue((this._value != null) ? ((TimeSpan?)this._value) : null);
				return;
			default:
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException("Type", this._valueType, "Unexpected token type.");
			}
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x0002E210 File Offset: 0x0002C410
		internal override int GetDeepHashCode()
		{
			int num = ((this._value != null) ? this._value.GetHashCode() : 0);
			int valueType = (int)this._valueType;
			return valueType.GetHashCode() ^ num;
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x0002E244 File Offset: 0x0002C444
		[NullableContext(1)]
		private static bool ValuesEquals(JValue v1, JValue v2)
		{
			return v1 == v2 || (v1._valueType == v2._valueType && JValue.Compare(v1._valueType, v1._value, v2._value) == 0);
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x0002E276 File Offset: 0x0002C476
		public bool Equals(JValue other)
		{
			return other != null && JValue.ValuesEquals(this, other);
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x0002E284 File Offset: 0x0002C484
		public override bool Equals(object obj)
		{
			JValue jvalue = obj as JValue;
			return jvalue != null && this.Equals(jvalue);
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x0002E2A4 File Offset: 0x0002C4A4
		public override int GetHashCode()
		{
			if (this._value == null)
			{
				return 0;
			}
			return this._value.GetHashCode();
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x0002E2BB File Offset: 0x0002C4BB
		[NullableContext(1)]
		public override string ToString()
		{
			if (this._value == null)
			{
				return string.Empty;
			}
			return this._value.ToString();
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x0002E2D6 File Offset: 0x0002C4D6
		[NullableContext(1)]
		public string ToString(string format)
		{
			return this.ToString(format, CultureInfo.CurrentCulture);
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x0002E2E4 File Offset: 0x0002C4E4
		[NullableContext(1)]
		public string ToString([Nullable(2)] IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x0002E2F0 File Offset: 0x0002C4F0
		[return: Nullable(1)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			if (this._value == null)
			{
				return string.Empty;
			}
			IFormattable formattable = this._value as IFormattable;
			if (formattable != null)
			{
				return formattable.ToString(format, formatProvider);
			}
			return this._value.ToString();
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x0002E32E File Offset: 0x0002C52E
		[NullableContext(1)]
		protected override DynamicMetaObject GetMetaObject(Expression parameter)
		{
			return new DynamicProxyMetaObject<JValue>(parameter, this, new JValue.JValueDynamicProxy());
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x0002E33C File Offset: 0x0002C53C
		int IComparable.CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}
			JValue jvalue = obj as JValue;
			object obj2;
			JTokenType jtokenType;
			if (jvalue != null)
			{
				obj2 = jvalue.Value;
				jtokenType = ((this._valueType == JTokenType.String && this._valueType != jvalue._valueType) ? jvalue._valueType : this._valueType);
			}
			else
			{
				obj2 = obj;
				jtokenType = this._valueType;
			}
			return JValue.Compare(jtokenType, this._value, obj2);
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x0002E39D File Offset: 0x0002C59D
		public int CompareTo(JValue obj)
		{
			if (obj == null)
			{
				return 1;
			}
			return JValue.Compare((this._valueType == JTokenType.String && this._valueType != obj._valueType) ? obj._valueType : this._valueType, this._value, obj._value);
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x0002E3DC File Offset: 0x0002C5DC
		TypeCode IConvertible.GetTypeCode()
		{
			if (this._value == null)
			{
				return TypeCode.Empty;
			}
			IConvertible convertible = this._value as IConvertible;
			if (convertible != null)
			{
				return convertible.GetTypeCode();
			}
			return TypeCode.Object;
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x0002E40A File Offset: 0x0002C60A
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return (bool)this;
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x0002E412 File Offset: 0x0002C612
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return (char)this;
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x0002E41A File Offset: 0x0002C61A
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return (sbyte)this;
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x0002E422 File Offset: 0x0002C622
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return (byte)this;
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x0002E42A File Offset: 0x0002C62A
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return (short)this;
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x0002E432 File Offset: 0x0002C632
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return (ushort)this;
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x0002E43A File Offset: 0x0002C63A
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return (int)this;
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x0002E442 File Offset: 0x0002C642
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return (uint)this;
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x0002E44A File Offset: 0x0002C64A
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return (long)this;
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x0002E452 File Offset: 0x0002C652
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return (ulong)this;
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x0002E45A File Offset: 0x0002C65A
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return (float)this;
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x0002E463 File Offset: 0x0002C663
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return (double)this;
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x0002E46C File Offset: 0x0002C66C
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return (decimal)this;
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x0002E474 File Offset: 0x0002C674
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			return (DateTime)this;
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x0002E47C File Offset: 0x0002C67C
		[NullableContext(1)]
		object IConvertible.ToType(Type conversionType, [Nullable(2)] IFormatProvider provider)
		{
			return base.ToObject(conversionType);
		}

		// Token: 0x040003B7 RID: 951
		private JTokenType _valueType;

		// Token: 0x040003B8 RID: 952
		private object _value;

		// Token: 0x020001D2 RID: 466
		[NullableContext(1)]
		[Nullable(new byte[] { 0, 1 })]
		private class JValueDynamicProxy : DynamicProxy<JValue>
		{
			// Token: 0x06000FDA RID: 4058 RVA: 0x000453E4 File Offset: 0x000435E4
			public override bool TryConvert(JValue instance, ConvertBinder binder, [Nullable(2)] [NotNullWhen(true)] out object result)
			{
				if (binder.Type == typeof(JValue) || binder.Type == typeof(JToken))
				{
					result = instance;
					return true;
				}
				object value = instance.Value;
				if (value == null)
				{
					result = null;
					return ReflectionUtils.IsNullable(binder.Type);
				}
				result = ConvertUtils.Convert(value, CultureInfo.InvariantCulture, binder.Type);
				return true;
			}

			// Token: 0x06000FDB RID: 4059 RVA: 0x00045454 File Offset: 0x00043654
			public override bool TryBinaryOperation(JValue instance, BinaryOperationBinder binder, object arg, [Nullable(2)] [NotNullWhen(true)] out object result)
			{
				JValue jvalue = arg as JValue;
				object obj = ((jvalue != null) ? jvalue.Value : arg);
				ExpressionType operation = binder.Operation;
				if (operation <= ExpressionType.NotEqual)
				{
					if (operation <= ExpressionType.LessThanOrEqual)
					{
						if (operation != ExpressionType.Add)
						{
							switch (operation)
							{
							case ExpressionType.Divide:
								break;
							case ExpressionType.Equal:
								result = JValue.Compare(instance.Type, instance.Value, obj) == 0;
								return true;
							case ExpressionType.ExclusiveOr:
							case ExpressionType.Invoke:
							case ExpressionType.Lambda:
							case ExpressionType.LeftShift:
								goto IL_018D;
							case ExpressionType.GreaterThan:
								result = JValue.Compare(instance.Type, instance.Value, obj) > 0;
								return true;
							case ExpressionType.GreaterThanOrEqual:
								result = JValue.Compare(instance.Type, instance.Value, obj) >= 0;
								return true;
							case ExpressionType.LessThan:
								result = JValue.Compare(instance.Type, instance.Value, obj) < 0;
								return true;
							case ExpressionType.LessThanOrEqual:
								result = JValue.Compare(instance.Type, instance.Value, obj) <= 0;
								return true;
							default:
								goto IL_018D;
							}
						}
					}
					else if (operation != ExpressionType.Multiply)
					{
						if (operation != ExpressionType.NotEqual)
						{
							goto IL_018D;
						}
						result = JValue.Compare(instance.Type, instance.Value, obj) != 0;
						return true;
					}
				}
				else if (operation <= ExpressionType.AddAssign)
				{
					if (operation != ExpressionType.Subtract && operation != ExpressionType.AddAssign)
					{
						goto IL_018D;
					}
				}
				else if (operation != ExpressionType.DivideAssign && operation != ExpressionType.MultiplyAssign && operation != ExpressionType.SubtractAssign)
				{
					goto IL_018D;
				}
				if (JValue.Operation(binder.Operation, instance.Value, obj, out result))
				{
					result = new JValue(result);
					return true;
				}
				IL_018D:
				result = null;
				return false;
			}
		}
	}
}
