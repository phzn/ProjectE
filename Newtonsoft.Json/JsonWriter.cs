using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json
{
	// Token: 0x02000032 RID: 50
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class JsonWriter : IDisposable
	{
		// Token: 0x06000350 RID: 848 RVA: 0x0000D880 File Offset: 0x0000BA80
		internal Task AutoCompleteAsync(JsonToken tokenBeingWritten, CancellationToken cancellationToken)
		{
			JsonWriter.State currentState = this._currentState;
			JsonWriter.State state = JsonWriter.StateArray[(int)tokenBeingWritten][(int)currentState];
			if (state == JsonWriter.State.Error)
			{
				throw JsonWriterException.Create(this, "Token {0} in state {1} would result in an invalid JSON object.".FormatWith(CultureInfo.InvariantCulture, tokenBeingWritten.ToString(), currentState.ToString()), null);
			}
			this._currentState = state;
			if (this._formatting == Formatting.Indented)
			{
				switch (currentState)
				{
				case JsonWriter.State.Start:
					goto IL_00F3;
				case JsonWriter.State.Property:
					return this.WriteIndentSpaceAsync(cancellationToken);
				case JsonWriter.State.Object:
					if (tokenBeingWritten == JsonToken.PropertyName)
					{
						return this.AutoCompleteAsync(cancellationToken);
					}
					if (tokenBeingWritten != JsonToken.Comment)
					{
						return this.WriteValueDelimiterAsync(cancellationToken);
					}
					goto IL_00F3;
				case JsonWriter.State.ArrayStart:
				case JsonWriter.State.ConstructorStart:
					return this.WriteIndentAsync(cancellationToken);
				case JsonWriter.State.Array:
				case JsonWriter.State.Constructor:
					if (tokenBeingWritten != JsonToken.Comment)
					{
						return this.AutoCompleteAsync(cancellationToken);
					}
					return this.WriteIndentAsync(cancellationToken);
				}
				if (tokenBeingWritten == JsonToken.PropertyName)
				{
					return this.WriteIndentAsync(cancellationToken);
				}
			}
			else if (tokenBeingWritten != JsonToken.Comment)
			{
				switch (currentState)
				{
				case JsonWriter.State.Object:
				case JsonWriter.State.Array:
				case JsonWriter.State.Constructor:
					return this.WriteValueDelimiterAsync(cancellationToken);
				}
			}
			IL_00F3:
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000D988 File Offset: 0x0000BB88
		private async Task AutoCompleteAsync(CancellationToken cancellationToken)
		{
			await this.WriteValueDelimiterAsync(cancellationToken).ConfigureAwait(false);
			await this.WriteIndentAsync(cancellationToken).ConfigureAwait(false);
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000D9D3 File Offset: 0x0000BBD3
		public virtual Task CloseAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.Close();
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000D9F0 File Offset: 0x0000BBF0
		public virtual Task FlushAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.Flush();
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000DA0D File Offset: 0x0000BC0D
		protected virtual Task WriteEndAsync(JsonToken token, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteEnd(token);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000DA2B File Offset: 0x0000BC2B
		protected virtual Task WriteIndentAsync(CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteIndent();
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000DA48 File Offset: 0x0000BC48
		protected virtual Task WriteValueDelimiterAsync(CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValueDelimiter();
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000DA65 File Offset: 0x0000BC65
		protected virtual Task WriteIndentSpaceAsync(CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteIndentSpace();
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000DA82 File Offset: 0x0000BC82
		public virtual Task WriteRawAsync([Nullable(2)] string json, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteRaw(json);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000DAA0 File Offset: 0x0000BCA0
		public virtual Task WriteEndAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteEnd();
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000DAC0 File Offset: 0x0000BCC0
		internal Task WriteEndInternalAsync(CancellationToken cancellationToken)
		{
			JsonContainerType jsonContainerType = this.Peek();
			switch (jsonContainerType)
			{
			case JsonContainerType.Object:
				return this.WriteEndObjectAsync(cancellationToken);
			case JsonContainerType.Array:
				return this.WriteEndArrayAsync(cancellationToken);
			case JsonContainerType.Constructor:
				return this.WriteEndConstructorAsync(cancellationToken);
			default:
				if (cancellationToken.IsCancellationRequested)
				{
					return cancellationToken.FromCanceled();
				}
				throw JsonWriterException.Create(this, "Unexpected type when writing end: " + jsonContainerType.ToString(), null);
			}
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000DB30 File Offset: 0x0000BD30
		internal Task InternalWriteEndAsync(JsonContainerType type, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			int num = this.CalculateLevelsToComplete(type);
			while (num-- > 0)
			{
				JsonToken closeTokenForType = this.GetCloseTokenForType(this.Pop());
				Task task;
				if (this._currentState == JsonWriter.State.Property)
				{
					task = this.WriteNullAsync(cancellationToken);
					if (!task.IsCompletedSuccessfully())
					{
						return this.<InternalWriteEndAsync>g__AwaitProperty|11_0(task, num, closeTokenForType, cancellationToken);
					}
				}
				if (this._formatting == Formatting.Indented && this._currentState != JsonWriter.State.ObjectStart && this._currentState != JsonWriter.State.ArrayStart)
				{
					task = this.WriteIndentAsync(cancellationToken);
					if (!task.IsCompletedSuccessfully())
					{
						return this.<InternalWriteEndAsync>g__AwaitIndent|11_1(task, num, closeTokenForType, cancellationToken);
					}
				}
				task = this.WriteEndAsync(closeTokenForType, cancellationToken);
				if (!task.IsCompletedSuccessfully())
				{
					return this.<InternalWriteEndAsync>g__AwaitEnd|11_2(task, num, cancellationToken);
				}
				this.UpdateCurrentState();
			}
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000DBF2 File Offset: 0x0000BDF2
		public virtual Task WriteEndArrayAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteEndArray();
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000DC0F File Offset: 0x0000BE0F
		public virtual Task WriteEndConstructorAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteEndConstructor();
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000DC2C File Offset: 0x0000BE2C
		public virtual Task WriteEndObjectAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteEndObject();
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000DC49 File Offset: 0x0000BE49
		public virtual Task WriteNullAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteNull();
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000DC66 File Offset: 0x0000BE66
		public virtual Task WritePropertyNameAsync(string name, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WritePropertyName(name);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000DC84 File Offset: 0x0000BE84
		public virtual Task WritePropertyNameAsync(string name, bool escape, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WritePropertyName(name, escape);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000DCA3 File Offset: 0x0000BEA3
		internal Task InternalWritePropertyNameAsync(string name, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this._currentPosition.PropertyName = name;
			return this.AutoCompleteAsync(JsonToken.PropertyName, cancellationToken);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000DCC9 File Offset: 0x0000BEC9
		public virtual Task WriteStartArrayAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteStartArray();
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000DCE8 File Offset: 0x0000BEE8
		internal async Task InternalWriteStartAsync(JsonToken token, JsonContainerType container, CancellationToken cancellationToken)
		{
			this.UpdateScopeWithFinishedValue();
			await this.AutoCompleteAsync(token, cancellationToken).ConfigureAwait(false);
			this.Push(container);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000DD43 File Offset: 0x0000BF43
		public virtual Task WriteCommentAsync([Nullable(2)] string text, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteComment(text);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000DD61 File Offset: 0x0000BF61
		internal Task InternalWriteCommentAsync(CancellationToken cancellationToken)
		{
			return this.AutoCompleteAsync(JsonToken.Comment, cancellationToken);
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000DD6B File Offset: 0x0000BF6B
		public virtual Task WriteRawValueAsync([Nullable(2)] string json, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteRawValue(json);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000DD89 File Offset: 0x0000BF89
		public virtual Task WriteStartConstructorAsync(string name, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteStartConstructor(name);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000DDA7 File Offset: 0x0000BFA7
		public virtual Task WriteStartObjectAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteStartObject();
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000DDC4 File Offset: 0x0000BFC4
		public Task WriteTokenAsync(JsonReader reader, CancellationToken cancellationToken = default(CancellationToken))
		{
			return this.WriteTokenAsync(reader, true, cancellationToken);
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000DDCF File Offset: 0x0000BFCF
		public Task WriteTokenAsync(JsonReader reader, bool writeChildren, CancellationToken cancellationToken = default(CancellationToken))
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			return this.WriteTokenAsync(reader, writeChildren, true, true, cancellationToken);
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000DDE7 File Offset: 0x0000BFE7
		public Task WriteTokenAsync(JsonToken token, CancellationToken cancellationToken = default(CancellationToken))
		{
			return this.WriteTokenAsync(token, null, cancellationToken);
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000DDF4 File Offset: 0x0000BFF4
		public Task WriteTokenAsync(JsonToken token, [Nullable(2)] object value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			switch (token)
			{
			case JsonToken.None:
				return AsyncUtils.CompletedTask;
			case JsonToken.StartObject:
				return this.WriteStartObjectAsync(cancellationToken);
			case JsonToken.StartArray:
				return this.WriteStartArrayAsync(cancellationToken);
			case JsonToken.StartConstructor:
				ValidationUtils.ArgumentNotNull(value, "value");
				return this.WriteStartConstructorAsync(value.ToString(), cancellationToken);
			case JsonToken.PropertyName:
				ValidationUtils.ArgumentNotNull(value, "value");
				return this.WritePropertyNameAsync(value.ToString(), cancellationToken);
			case JsonToken.Comment:
				return this.WriteCommentAsync((value != null) ? value.ToString() : null, cancellationToken);
			case JsonToken.Raw:
				return this.WriteRawValueAsync((value != null) ? value.ToString() : null, cancellationToken);
			case JsonToken.Integer:
				ValidationUtils.ArgumentNotNull(value, "value");
				if (value is BigInteger)
				{
					BigInteger bigInteger = (BigInteger)value;
					return this.WriteValueAsync(bigInteger, cancellationToken);
				}
				return this.WriteValueAsync(Convert.ToInt64(value, CultureInfo.InvariantCulture), cancellationToken);
			case JsonToken.Float:
				ValidationUtils.ArgumentNotNull(value, "value");
				if (value is decimal)
				{
					decimal num = (decimal)value;
					return this.WriteValueAsync(num, cancellationToken);
				}
				if (value is double)
				{
					double num2 = (double)value;
					return this.WriteValueAsync(num2, cancellationToken);
				}
				if (value is float)
				{
					float num3 = (float)value;
					return this.WriteValueAsync(num3, cancellationToken);
				}
				return this.WriteValueAsync(Convert.ToDouble(value, CultureInfo.InvariantCulture), cancellationToken);
			case JsonToken.String:
				ValidationUtils.ArgumentNotNull(value, "value");
				return this.WriteValueAsync(value.ToString(), cancellationToken);
			case JsonToken.Boolean:
				ValidationUtils.ArgumentNotNull(value, "value");
				return this.WriteValueAsync(Convert.ToBoolean(value, CultureInfo.InvariantCulture), cancellationToken);
			case JsonToken.Null:
				return this.WriteNullAsync(cancellationToken);
			case JsonToken.Undefined:
				return this.WriteUndefinedAsync(cancellationToken);
			case JsonToken.EndObject:
				return this.WriteEndObjectAsync(cancellationToken);
			case JsonToken.EndArray:
				return this.WriteEndArrayAsync(cancellationToken);
			case JsonToken.EndConstructor:
				return this.WriteEndConstructorAsync(cancellationToken);
			case JsonToken.Date:
				ValidationUtils.ArgumentNotNull(value, "value");
				if (value is DateTimeOffset)
				{
					DateTimeOffset dateTimeOffset = (DateTimeOffset)value;
					return this.WriteValueAsync(dateTimeOffset, cancellationToken);
				}
				return this.WriteValueAsync(Convert.ToDateTime(value, CultureInfo.InvariantCulture), cancellationToken);
			case JsonToken.Bytes:
				ValidationUtils.ArgumentNotNull(value, "value");
				if (value is Guid)
				{
					Guid guid = (Guid)value;
					return this.WriteValueAsync(guid, cancellationToken);
				}
				return this.WriteValueAsync((byte[])value, cancellationToken);
			default:
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException("token", token, "Unexpected token type.");
			}
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000E058 File Offset: 0x0000C258
		internal virtual async Task WriteTokenAsync(JsonReader reader, bool writeChildren, bool writeDateConstructorAsDate, bool writeComments, CancellationToken cancellationToken)
		{
			int initialDepth = this.CalculateWriteTokenInitialDepth(reader);
			for (;;)
			{
				if (!writeDateConstructorAsDate || reader.TokenType != JsonToken.StartConstructor)
				{
					goto IL_00E7;
				}
				object value = reader.Value;
				if (!string.Equals((value != null) ? value.ToString() : null, "Date", StringComparison.Ordinal))
				{
					goto IL_00E7;
				}
				await this.WriteConstructorDateAsync(reader, cancellationToken).ConfigureAwait(false);
				IL_0180:
				bool flag = initialDepth - 1 < reader.Depth - (JsonTokenUtils.IsEndToken(reader.TokenType) ? 1 : 0) && writeChildren;
				if (flag)
				{
					flag = await reader.ReadAsync(cancellationToken).ConfigureAwait(false);
				}
				if (!flag)
				{
					break;
				}
				continue;
				IL_00E7:
				if (writeComments || reader.TokenType != JsonToken.Comment)
				{
					await this.WriteTokenAsync(reader.TokenType, reader.Value, cancellationToken).ConfigureAwait(false);
					goto IL_0180;
				}
				goto IL_0180;
			}
			if (this.IsWriteTokenIncomplete(reader, writeChildren, initialDepth))
			{
				throw JsonWriterException.Create(this, "Unexpected end when reading token.", null);
			}
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000E0C8 File Offset: 0x0000C2C8
		internal async Task WriteTokenSyncReadingAsync(JsonReader reader, CancellationToken cancellationToken)
		{
			int initialDepth = this.CalculateWriteTokenInitialDepth(reader);
			for (;;)
			{
				if (reader.TokenType != JsonToken.StartConstructor)
				{
					goto IL_0066;
				}
				object value = reader.Value;
				if (!string.Equals((value != null) ? value.ToString() : null, "Date", StringComparison.Ordinal))
				{
					goto IL_0066;
				}
				this.WriteConstructorDate(reader);
				IL_0082:
				bool flag = initialDepth - 1 < reader.Depth - (JsonTokenUtils.IsEndToken(reader.TokenType) ? 1 : 0);
				if (flag)
				{
					flag = await reader.ReadAsync(cancellationToken).ConfigureAwait(false);
				}
				if (!flag)
				{
					break;
				}
				continue;
				IL_0066:
				this.WriteToken(reader.TokenType, reader.Value);
				goto IL_0082;
			}
			if (initialDepth < this.CalculateWriteTokenFinalDepth(reader))
			{
				throw JsonWriterException.Create(this, "Unexpected end when reading token.", null);
			}
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0000E11C File Offset: 0x0000C31C
		private async Task WriteConstructorDateAsync(JsonReader reader, CancellationToken cancellationToken)
		{
			ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = reader.ReadAsync(cancellationToken).ConfigureAwait(false).GetAwaiter();
			ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
			if (!configuredTaskAwaiter.IsCompleted)
			{
				await configuredTaskAwaiter;
				configuredTaskAwaiter = configuredTaskAwaiter2;
				configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
			}
			if (!configuredTaskAwaiter.GetResult())
			{
				throw JsonWriterException.Create(this, "Unexpected end when reading date constructor.", null);
			}
			if (reader.TokenType != JsonToken.Integer)
			{
				throw JsonWriterException.Create(this, "Unexpected token when reading date constructor. Expected Integer, got " + reader.TokenType.ToString(), null);
			}
			DateTime date = DateTimeUtils.ConvertJavaScriptTicksToDateTime((long)reader.Value);
			configuredTaskAwaiter = reader.ReadAsync(cancellationToken).ConfigureAwait(false).GetAwaiter();
			if (!configuredTaskAwaiter.IsCompleted)
			{
				await configuredTaskAwaiter;
				configuredTaskAwaiter = configuredTaskAwaiter2;
				configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
			}
			if (!configuredTaskAwaiter.GetResult())
			{
				throw JsonWriterException.Create(this, "Unexpected end when reading date constructor.", null);
			}
			if (reader.TokenType != JsonToken.EndConstructor)
			{
				throw JsonWriterException.Create(this, "Unexpected token when reading date constructor. Expected EndConstructor, got " + reader.TokenType.ToString(), null);
			}
			await this.WriteValueAsync(date, cancellationToken).ConfigureAwait(false);
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0000E16F File Offset: 0x0000C36F
		public virtual Task WriteValueAsync(bool value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0000E18D File Offset: 0x0000C38D
		public virtual Task WriteValueAsync(bool? value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0000E1AB File Offset: 0x0000C3AB
		public virtual Task WriteValueAsync(byte value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000E1C9 File Offset: 0x0000C3C9
		public virtual Task WriteValueAsync(byte? value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000E1E7 File Offset: 0x0000C3E7
		public virtual Task WriteValueAsync([Nullable(2)] byte[] value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000E205 File Offset: 0x0000C405
		public virtual Task WriteValueAsync(char value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000E223 File Offset: 0x0000C423
		public virtual Task WriteValueAsync(char? value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000E241 File Offset: 0x0000C441
		public virtual Task WriteValueAsync(DateTime value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0000E25F File Offset: 0x0000C45F
		public virtual Task WriteValueAsync(DateTime? value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000E27D File Offset: 0x0000C47D
		public virtual Task WriteValueAsync(DateTimeOffset value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000E29B File Offset: 0x0000C49B
		public virtual Task WriteValueAsync(DateTimeOffset? value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0000E2B9 File Offset: 0x0000C4B9
		public virtual Task WriteValueAsync(decimal value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0000E2D7 File Offset: 0x0000C4D7
		public virtual Task WriteValueAsync(decimal? value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0000E2F5 File Offset: 0x0000C4F5
		public virtual Task WriteValueAsync(double value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0000E313 File Offset: 0x0000C513
		public virtual Task WriteValueAsync(double? value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0000E331 File Offset: 0x0000C531
		public virtual Task WriteValueAsync(float value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000E34F File Offset: 0x0000C54F
		public virtual Task WriteValueAsync(float? value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000E36D File Offset: 0x0000C56D
		public virtual Task WriteValueAsync(Guid value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000E38B File Offset: 0x0000C58B
		public virtual Task WriteValueAsync(Guid? value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0000E3A9 File Offset: 0x0000C5A9
		public virtual Task WriteValueAsync(int value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0000E3C7 File Offset: 0x0000C5C7
		public virtual Task WriteValueAsync(int? value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0000E3E5 File Offset: 0x0000C5E5
		public virtual Task WriteValueAsync(long value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0000E403 File Offset: 0x0000C603
		public virtual Task WriteValueAsync(long? value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0000E421 File Offset: 0x0000C621
		public virtual Task WriteValueAsync([Nullable(2)] object value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0000E43F File Offset: 0x0000C63F
		[CLSCompliant(false)]
		public virtual Task WriteValueAsync(sbyte value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0000E45D File Offset: 0x0000C65D
		[CLSCompliant(false)]
		public virtual Task WriteValueAsync(sbyte? value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0000E47B File Offset: 0x0000C67B
		public virtual Task WriteValueAsync(short value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0000E499 File Offset: 0x0000C699
		public virtual Task WriteValueAsync(short? value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000E4B7 File Offset: 0x0000C6B7
		public virtual Task WriteValueAsync([Nullable(2)] string value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000E4D5 File Offset: 0x0000C6D5
		public virtual Task WriteValueAsync(TimeSpan value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0000E4F3 File Offset: 0x0000C6F3
		public virtual Task WriteValueAsync(TimeSpan? value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0000E511 File Offset: 0x0000C711
		[CLSCompliant(false)]
		public virtual Task WriteValueAsync(uint value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0000E52F File Offset: 0x0000C72F
		[CLSCompliant(false)]
		public virtual Task WriteValueAsync(uint? value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0000E54D File Offset: 0x0000C74D
		[CLSCompliant(false)]
		public virtual Task WriteValueAsync(ulong value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0000E56B File Offset: 0x0000C76B
		[CLSCompliant(false)]
		public virtual Task WriteValueAsync(ulong? value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0000E589 File Offset: 0x0000C789
		public virtual Task WriteValueAsync([Nullable(2)] Uri value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0000E5A7 File Offset: 0x0000C7A7
		[CLSCompliant(false)]
		public virtual Task WriteValueAsync(ushort value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0000E5C5 File Offset: 0x0000C7C5
		[CLSCompliant(false)]
		public virtual Task WriteValueAsync(ushort? value, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteValue(value);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000E5E3 File Offset: 0x0000C7E3
		public virtual Task WriteUndefinedAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteUndefined();
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0000E600 File Offset: 0x0000C800
		public virtual Task WriteWhitespaceAsync(string ws, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.WriteWhitespace(ws);
			return AsyncUtils.CompletedTask;
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0000E61E File Offset: 0x0000C81E
		internal Task InternalWriteValueAsync(JsonToken token, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			this.UpdateScopeWithFinishedValue();
			return this.AutoCompleteAsync(token, cancellationToken);
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0000E640 File Offset: 0x0000C840
		protected Task SetWriteStateAsync(JsonToken token, object value, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return cancellationToken.FromCanceled();
			}
			switch (token)
			{
			case JsonToken.StartObject:
				return this.InternalWriteStartAsync(token, JsonContainerType.Object, cancellationToken);
			case JsonToken.StartArray:
				return this.InternalWriteStartAsync(token, JsonContainerType.Array, cancellationToken);
			case JsonToken.StartConstructor:
				return this.InternalWriteStartAsync(token, JsonContainerType.Constructor, cancellationToken);
			case JsonToken.PropertyName:
			{
				string text = value as string;
				if (text == null)
				{
					throw new ArgumentException("A name is required when setting property name state.", "value");
				}
				return this.InternalWritePropertyNameAsync(text, cancellationToken);
			}
			case JsonToken.Comment:
				return this.InternalWriteCommentAsync(cancellationToken);
			case JsonToken.Raw:
				return AsyncUtils.CompletedTask;
			case JsonToken.Integer:
			case JsonToken.Float:
			case JsonToken.String:
			case JsonToken.Boolean:
			case JsonToken.Null:
			case JsonToken.Undefined:
			case JsonToken.Date:
			case JsonToken.Bytes:
				return this.InternalWriteValueAsync(token, cancellationToken);
			case JsonToken.EndObject:
				return this.InternalWriteEndAsync(JsonContainerType.Object, cancellationToken);
			case JsonToken.EndArray:
				return this.InternalWriteEndAsync(JsonContainerType.Array, cancellationToken);
			case JsonToken.EndConstructor:
				return this.InternalWriteEndAsync(JsonContainerType.Constructor, cancellationToken);
			default:
				throw new ArgumentOutOfRangeException("token");
			}
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0000E728 File Offset: 0x0000C928
		internal static Task WriteValueAsync(JsonWriter writer, PrimitiveTypeCode typeCode, object value, CancellationToken cancellationToken)
		{
			for (;;)
			{
				switch (typeCode)
				{
				case PrimitiveTypeCode.Char:
					goto IL_00AD;
				case PrimitiveTypeCode.CharNullable:
					goto IL_00BB;
				case PrimitiveTypeCode.Boolean:
					goto IL_00DC;
				case PrimitiveTypeCode.BooleanNullable:
					goto IL_00EA;
				case PrimitiveTypeCode.SByte:
					goto IL_010B;
				case PrimitiveTypeCode.SByteNullable:
					goto IL_0119;
				case PrimitiveTypeCode.Int16:
					goto IL_013A;
				case PrimitiveTypeCode.Int16Nullable:
					goto IL_0148;
				case PrimitiveTypeCode.UInt16:
					goto IL_016A;
				case PrimitiveTypeCode.UInt16Nullable:
					goto IL_0178;
				case PrimitiveTypeCode.Int32:
					goto IL_019A;
				case PrimitiveTypeCode.Int32Nullable:
					goto IL_01A8;
				case PrimitiveTypeCode.Byte:
					goto IL_01CA;
				case PrimitiveTypeCode.ByteNullable:
					goto IL_01D8;
				case PrimitiveTypeCode.UInt32:
					goto IL_01FA;
				case PrimitiveTypeCode.UInt32Nullable:
					goto IL_0208;
				case PrimitiveTypeCode.Int64:
					goto IL_022A;
				case PrimitiveTypeCode.Int64Nullable:
					goto IL_0238;
				case PrimitiveTypeCode.UInt64:
					goto IL_025A;
				case PrimitiveTypeCode.UInt64Nullable:
					goto IL_0268;
				case PrimitiveTypeCode.Single:
					goto IL_028A;
				case PrimitiveTypeCode.SingleNullable:
					goto IL_0298;
				case PrimitiveTypeCode.Double:
					goto IL_02BA;
				case PrimitiveTypeCode.DoubleNullable:
					goto IL_02C8;
				case PrimitiveTypeCode.DateTime:
					goto IL_02EA;
				case PrimitiveTypeCode.DateTimeNullable:
					goto IL_02F8;
				case PrimitiveTypeCode.DateTimeOffset:
					goto IL_031A;
				case PrimitiveTypeCode.DateTimeOffsetNullable:
					goto IL_0328;
				case PrimitiveTypeCode.Decimal:
					goto IL_034A;
				case PrimitiveTypeCode.DecimalNullable:
					goto IL_0358;
				case PrimitiveTypeCode.Guid:
					goto IL_037A;
				case PrimitiveTypeCode.GuidNullable:
					goto IL_0388;
				case PrimitiveTypeCode.TimeSpan:
					goto IL_03AA;
				case PrimitiveTypeCode.TimeSpanNullable:
					goto IL_03B8;
				case PrimitiveTypeCode.BigInteger:
					goto IL_03DA;
				case PrimitiveTypeCode.BigIntegerNullable:
					goto IL_03ED;
				case PrimitiveTypeCode.Uri:
					goto IL_0414;
				case PrimitiveTypeCode.String:
					goto IL_0422;
				case PrimitiveTypeCode.Bytes:
					goto IL_0430;
				case PrimitiveTypeCode.DBNull:
					goto IL_043E;
				default:
				{
					IConvertible convertible = value as IConvertible;
					if (convertible == null)
					{
						goto IL_045F;
					}
					JsonWriter.ResolveConvertibleValue(convertible, out typeCode, out value);
					break;
				}
				}
			}
			IL_00AD:
			return writer.WriteValueAsync((char)value, cancellationToken);
			IL_00BB:
			return writer.WriteValueAsync((value == null) ? null : new char?((char)value), cancellationToken);
			IL_00DC:
			return writer.WriteValueAsync((bool)value, cancellationToken);
			IL_00EA:
			return writer.WriteValueAsync((value == null) ? null : new bool?((bool)value), cancellationToken);
			IL_010B:
			return writer.WriteValueAsync((sbyte)value, cancellationToken);
			IL_0119:
			return writer.WriteValueAsync((value == null) ? null : new sbyte?((sbyte)value), cancellationToken);
			IL_013A:
			return writer.WriteValueAsync((short)value, cancellationToken);
			IL_0148:
			return writer.WriteValueAsync((value == null) ? null : new short?((short)value), cancellationToken);
			IL_016A:
			return writer.WriteValueAsync((ushort)value, cancellationToken);
			IL_0178:
			return writer.WriteValueAsync((value == null) ? null : new ushort?((ushort)value), cancellationToken);
			IL_019A:
			return writer.WriteValueAsync((int)value, cancellationToken);
			IL_01A8:
			return writer.WriteValueAsync((value == null) ? null : new int?((int)value), cancellationToken);
			IL_01CA:
			return writer.WriteValueAsync((byte)value, cancellationToken);
			IL_01D8:
			return writer.WriteValueAsync((value == null) ? null : new byte?((byte)value), cancellationToken);
			IL_01FA:
			return writer.WriteValueAsync((uint)value, cancellationToken);
			IL_0208:
			return writer.WriteValueAsync((value == null) ? null : new uint?((uint)value), cancellationToken);
			IL_022A:
			return writer.WriteValueAsync((long)value, cancellationToken);
			IL_0238:
			return writer.WriteValueAsync((value == null) ? null : new long?((long)value), cancellationToken);
			IL_025A:
			return writer.WriteValueAsync((ulong)value, cancellationToken);
			IL_0268:
			return writer.WriteValueAsync((value == null) ? null : new ulong?((ulong)value), cancellationToken);
			IL_028A:
			return writer.WriteValueAsync((float)value, cancellationToken);
			IL_0298:
			return writer.WriteValueAsync((value == null) ? null : new float?((float)value), cancellationToken);
			IL_02BA:
			return writer.WriteValueAsync((double)value, cancellationToken);
			IL_02C8:
			return writer.WriteValueAsync((value == null) ? null : new double?((double)value), cancellationToken);
			IL_02EA:
			return writer.WriteValueAsync((DateTime)value, cancellationToken);
			IL_02F8:
			return writer.WriteValueAsync((value == null) ? null : new DateTime?((DateTime)value), cancellationToken);
			IL_031A:
			return writer.WriteValueAsync((DateTimeOffset)value, cancellationToken);
			IL_0328:
			return writer.WriteValueAsync((value == null) ? null : new DateTimeOffset?((DateTimeOffset)value), cancellationToken);
			IL_034A:
			return writer.WriteValueAsync((decimal)value, cancellationToken);
			IL_0358:
			return writer.WriteValueAsync((value == null) ? null : new decimal?((decimal)value), cancellationToken);
			IL_037A:
			return writer.WriteValueAsync((Guid)value, cancellationToken);
			IL_0388:
			return writer.WriteValueAsync((value == null) ? null : new Guid?((Guid)value), cancellationToken);
			IL_03AA:
			return writer.WriteValueAsync((TimeSpan)value, cancellationToken);
			IL_03B8:
			return writer.WriteValueAsync((value == null) ? null : new TimeSpan?((TimeSpan)value), cancellationToken);
			IL_03DA:
			return writer.WriteValueAsync((BigInteger)value, cancellationToken);
			IL_03ED:
			return writer.WriteValueAsync((value == null) ? null : new BigInteger?((BigInteger)value), cancellationToken);
			IL_0414:
			return writer.WriteValueAsync((Uri)value, cancellationToken);
			IL_0422:
			return writer.WriteValueAsync((string)value, cancellationToken);
			IL_0430:
			return writer.WriteValueAsync((byte[])value, cancellationToken);
			IL_043E:
			return writer.WriteNullAsync(cancellationToken);
			IL_045F:
			if (value == null)
			{
				return writer.WriteNullAsync(cancellationToken);
			}
			throw JsonWriter.CreateUnsupportedTypeException(writer, value);
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0000EBA8 File Offset: 0x0000CDA8
		internal static JsonWriter.State[][] BuildStateArray()
		{
			List<JsonWriter.State[]> list = JsonWriter.StateArrayTemplate.ToList<JsonWriter.State[]>();
			JsonWriter.State[] array = JsonWriter.StateArrayTemplate[0];
			JsonWriter.State[] array2 = JsonWriter.StateArrayTemplate[7];
			foreach (ulong num in EnumUtils.GetEnumValuesAndNames(typeof(JsonToken)).Values)
			{
				if (list.Count <= (int)num)
				{
					JsonToken jsonToken = (JsonToken)num;
					if (jsonToken - JsonToken.Integer <= 5 || jsonToken - JsonToken.Date <= 1)
					{
						list.Add(array2);
					}
					else
					{
						list.Add(array);
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600039E RID: 926 RVA: 0x0000ECFE File Offset: 0x0000CEFE
		// (set) Token: 0x0600039F RID: 927 RVA: 0x0000ED06 File Offset: 0x0000CF06
		public bool CloseOutput { get; set; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x0000ED0F File Offset: 0x0000CF0F
		// (set) Token: 0x060003A1 RID: 929 RVA: 0x0000ED17 File Offset: 0x0000CF17
		public bool AutoCompleteOnClose { get; set; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x0000ED20 File Offset: 0x0000CF20
		protected internal int Top
		{
			get
			{
				List<JsonPosition> stack = this._stack;
				int num = ((stack != null) ? stack.Count : 0);
				if (this.Peek() != JsonContainerType.None)
				{
					num++;
				}
				return num;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x0000ED50 File Offset: 0x0000CF50
		public WriteState WriteState
		{
			get
			{
				switch (this._currentState)
				{
				case JsonWriter.State.Start:
					return WriteState.Start;
				case JsonWriter.State.Property:
					return WriteState.Property;
				case JsonWriter.State.ObjectStart:
				case JsonWriter.State.Object:
					return WriteState.Object;
				case JsonWriter.State.ArrayStart:
				case JsonWriter.State.Array:
					return WriteState.Array;
				case JsonWriter.State.ConstructorStart:
				case JsonWriter.State.Constructor:
					return WriteState.Constructor;
				case JsonWriter.State.Closed:
					return WriteState.Closed;
				case JsonWriter.State.Error:
					return WriteState.Error;
				default:
					throw JsonWriterException.Create(this, "Invalid state: " + this._currentState.ToString(), null);
				}
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x0000EDC4 File Offset: 0x0000CFC4
		internal string ContainerPath
		{
			get
			{
				if (this._currentPosition.Type == JsonContainerType.None || this._stack == null)
				{
					return string.Empty;
				}
				return JsonPosition.BuildPath(this._stack, null);
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x0000EE00 File Offset: 0x0000D000
		public string Path
		{
			get
			{
				if (this._currentPosition.Type == JsonContainerType.None)
				{
					return string.Empty;
				}
				JsonPosition? jsonPosition = ((this._currentState != JsonWriter.State.ArrayStart && this._currentState != JsonWriter.State.ConstructorStart && this._currentState != JsonWriter.State.ObjectStart) ? new JsonPosition?(this._currentPosition) : null);
				return JsonPosition.BuildPath(this._stack, jsonPosition);
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060003A6 RID: 934 RVA: 0x0000EE66 File Offset: 0x0000D066
		// (set) Token: 0x060003A7 RID: 935 RVA: 0x0000EE6E File Offset: 0x0000D06E
		public Formatting Formatting
		{
			get
			{
				return this._formatting;
			}
			set
			{
				if (value < Formatting.None || value > Formatting.Indented)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._formatting = value;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x0000EE8A File Offset: 0x0000D08A
		// (set) Token: 0x060003A9 RID: 937 RVA: 0x0000EE92 File Offset: 0x0000D092
		public DateFormatHandling DateFormatHandling
		{
			get
			{
				return this._dateFormatHandling;
			}
			set
			{
				if (value < DateFormatHandling.IsoDateFormat || value > DateFormatHandling.MicrosoftDateFormat)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._dateFormatHandling = value;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060003AA RID: 938 RVA: 0x0000EEAE File Offset: 0x0000D0AE
		// (set) Token: 0x060003AB RID: 939 RVA: 0x0000EEB6 File Offset: 0x0000D0B6
		public DateTimeZoneHandling DateTimeZoneHandling
		{
			get
			{
				return this._dateTimeZoneHandling;
			}
			set
			{
				if (value < DateTimeZoneHandling.Local || value > DateTimeZoneHandling.RoundtripKind)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._dateTimeZoneHandling = value;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060003AC RID: 940 RVA: 0x0000EED2 File Offset: 0x0000D0D2
		// (set) Token: 0x060003AD RID: 941 RVA: 0x0000EEDA File Offset: 0x0000D0DA
		public StringEscapeHandling StringEscapeHandling
		{
			get
			{
				return this._stringEscapeHandling;
			}
			set
			{
				if (value < StringEscapeHandling.Default || value > StringEscapeHandling.EscapeHtml)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._stringEscapeHandling = value;
				this.OnStringEscapeHandlingChanged();
			}
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0000EEFC File Offset: 0x0000D0FC
		internal virtual void OnStringEscapeHandlingChanged()
		{
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060003AF RID: 943 RVA: 0x0000EEFE File Offset: 0x0000D0FE
		// (set) Token: 0x060003B0 RID: 944 RVA: 0x0000EF06 File Offset: 0x0000D106
		public FloatFormatHandling FloatFormatHandling
		{
			get
			{
				return this._floatFormatHandling;
			}
			set
			{
				if (value < FloatFormatHandling.String || value > FloatFormatHandling.DefaultValue)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._floatFormatHandling = value;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x0000EF22 File Offset: 0x0000D122
		// (set) Token: 0x060003B2 RID: 946 RVA: 0x0000EF2A File Offset: 0x0000D12A
		[Nullable(2)]
		public string DateFormatString
		{
			[NullableContext(2)]
			get
			{
				return this._dateFormatString;
			}
			[NullableContext(2)]
			set
			{
				this._dateFormatString = value;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x0000EF33 File Offset: 0x0000D133
		// (set) Token: 0x060003B4 RID: 948 RVA: 0x0000EF44 File Offset: 0x0000D144
		public CultureInfo Culture
		{
			get
			{
				return this._culture ?? CultureInfo.InvariantCulture;
			}
			set
			{
				this._culture = value;
			}
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0000EF4D File Offset: 0x0000D14D
		protected JsonWriter()
		{
			this._currentState = JsonWriter.State.Start;
			this._formatting = Formatting.None;
			this._dateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
			this.CloseOutput = true;
			this.AutoCompleteOnClose = true;
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0000EF78 File Offset: 0x0000D178
		internal void UpdateScopeWithFinishedValue()
		{
			if (this._currentPosition.HasIndex)
			{
				this._currentPosition.Position = this._currentPosition.Position + 1;
			}
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0000EF97 File Offset: 0x0000D197
		private void Push(JsonContainerType value)
		{
			if (this._currentPosition.Type != JsonContainerType.None)
			{
				if (this._stack == null)
				{
					this._stack = new List<JsonPosition>();
				}
				this._stack.Add(this._currentPosition);
			}
			this._currentPosition = new JsonPosition(value);
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0000EFD8 File Offset: 0x0000D1D8
		private JsonContainerType Pop()
		{
			ref JsonPosition currentPosition = this._currentPosition;
			if (this._stack != null && this._stack.Count > 0)
			{
				this._currentPosition = this._stack[this._stack.Count - 1];
				this._stack.RemoveAt(this._stack.Count - 1);
			}
			else
			{
				this._currentPosition = default(JsonPosition);
			}
			return currentPosition.Type;
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0000F04A File Offset: 0x0000D24A
		private JsonContainerType Peek()
		{
			return this._currentPosition.Type;
		}

		// Token: 0x060003BA RID: 954
		public abstract void Flush();

		// Token: 0x060003BB RID: 955 RVA: 0x0000F057 File Offset: 0x0000D257
		public virtual void Close()
		{
			if (this.AutoCompleteOnClose)
			{
				this.AutoCompleteAll();
			}
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0000F067 File Offset: 0x0000D267
		public virtual void WriteStartObject()
		{
			this.InternalWriteStart(JsonToken.StartObject, JsonContainerType.Object);
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000F071 File Offset: 0x0000D271
		public virtual void WriteEndObject()
		{
			this.InternalWriteEnd(JsonContainerType.Object);
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000F07A File Offset: 0x0000D27A
		public virtual void WriteStartArray()
		{
			this.InternalWriteStart(JsonToken.StartArray, JsonContainerType.Array);
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000F084 File Offset: 0x0000D284
		public virtual void WriteEndArray()
		{
			this.InternalWriteEnd(JsonContainerType.Array);
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0000F08D File Offset: 0x0000D28D
		public virtual void WriteStartConstructor(string name)
		{
			this.InternalWriteStart(JsonToken.StartConstructor, JsonContainerType.Constructor);
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0000F097 File Offset: 0x0000D297
		public virtual void WriteEndConstructor()
		{
			this.InternalWriteEnd(JsonContainerType.Constructor);
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0000F0A0 File Offset: 0x0000D2A0
		public virtual void WritePropertyName(string name)
		{
			this.InternalWritePropertyName(name);
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000F0A9 File Offset: 0x0000D2A9
		public virtual void WritePropertyName(string name, bool escape)
		{
			this.WritePropertyName(name);
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000F0B2 File Offset: 0x0000D2B2
		public virtual void WriteEnd()
		{
			this.WriteEnd(this.Peek());
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0000F0C0 File Offset: 0x0000D2C0
		public void WriteToken(JsonReader reader)
		{
			this.WriteToken(reader, true);
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0000F0CA File Offset: 0x0000D2CA
		public void WriteToken(JsonReader reader, bool writeChildren)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			this.WriteToken(reader, writeChildren, true, true);
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0000F0E4 File Offset: 0x0000D2E4
		[NullableContext(2)]
		public void WriteToken(JsonToken token, object value)
		{
			switch (token)
			{
			case JsonToken.None:
				return;
			case JsonToken.StartObject:
				this.WriteStartObject();
				return;
			case JsonToken.StartArray:
				this.WriteStartArray();
				return;
			case JsonToken.StartConstructor:
				ValidationUtils.ArgumentNotNull(value, "value");
				this.WriteStartConstructor(value.ToString());
				return;
			case JsonToken.PropertyName:
				ValidationUtils.ArgumentNotNull(value, "value");
				this.WritePropertyName(value.ToString());
				return;
			case JsonToken.Comment:
				this.WriteComment((value != null) ? value.ToString() : null);
				return;
			case JsonToken.Raw:
				this.WriteRawValue((value != null) ? value.ToString() : null);
				return;
			case JsonToken.Integer:
				ValidationUtils.ArgumentNotNull(value, "value");
				if (value is BigInteger)
				{
					BigInteger bigInteger = (BigInteger)value;
					this.WriteValue(bigInteger);
					return;
				}
				this.WriteValue(Convert.ToInt64(value, CultureInfo.InvariantCulture));
				return;
			case JsonToken.Float:
				ValidationUtils.ArgumentNotNull(value, "value");
				if (value is decimal)
				{
					decimal num = (decimal)value;
					this.WriteValue(num);
					return;
				}
				if (value is double)
				{
					double num2 = (double)value;
					this.WriteValue(num2);
					return;
				}
				if (value is float)
				{
					float num3 = (float)value;
					this.WriteValue(num3);
					return;
				}
				this.WriteValue(Convert.ToDouble(value, CultureInfo.InvariantCulture));
				return;
			case JsonToken.String:
				this.WriteValue((value != null) ? value.ToString() : null);
				return;
			case JsonToken.Boolean:
				ValidationUtils.ArgumentNotNull(value, "value");
				this.WriteValue(Convert.ToBoolean(value, CultureInfo.InvariantCulture));
				return;
			case JsonToken.Null:
				this.WriteNull();
				return;
			case JsonToken.Undefined:
				this.WriteUndefined();
				return;
			case JsonToken.EndObject:
				this.WriteEndObject();
				return;
			case JsonToken.EndArray:
				this.WriteEndArray();
				return;
			case JsonToken.EndConstructor:
				this.WriteEndConstructor();
				return;
			case JsonToken.Date:
				ValidationUtils.ArgumentNotNull(value, "value");
				if (value is DateTimeOffset)
				{
					DateTimeOffset dateTimeOffset = (DateTimeOffset)value;
					this.WriteValue(dateTimeOffset);
					return;
				}
				this.WriteValue(Convert.ToDateTime(value, CultureInfo.InvariantCulture));
				return;
			case JsonToken.Bytes:
				ValidationUtils.ArgumentNotNull(value, "value");
				if (value is Guid)
				{
					Guid guid = (Guid)value;
					this.WriteValue(guid);
					return;
				}
				this.WriteValue((byte[])value);
				return;
			default:
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException("token", token, "Unexpected token type.");
			}
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000F313 File Offset: 0x0000D513
		public void WriteToken(JsonToken token)
		{
			this.WriteToken(token, null);
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0000F320 File Offset: 0x0000D520
		internal virtual void WriteToken(JsonReader reader, bool writeChildren, bool writeDateConstructorAsDate, bool writeComments)
		{
			int num = this.CalculateWriteTokenInitialDepth(reader);
			for (;;)
			{
				if (!writeDateConstructorAsDate || reader.TokenType != JsonToken.StartConstructor)
				{
					goto IL_003C;
				}
				object value = reader.Value;
				if (!string.Equals((value != null) ? value.ToString() : null, "Date", StringComparison.Ordinal))
				{
					goto IL_003C;
				}
				this.WriteConstructorDate(reader);
				IL_005B:
				if (num - 1 >= reader.Depth - (JsonTokenUtils.IsEndToken(reader.TokenType) ? 1 : 0) || !writeChildren || !reader.Read())
				{
					break;
				}
				continue;
				IL_003C:
				if (writeComments || reader.TokenType != JsonToken.Comment)
				{
					this.WriteToken(reader.TokenType, reader.Value);
					goto IL_005B;
				}
				goto IL_005B;
			}
			if (this.IsWriteTokenIncomplete(reader, writeChildren, num))
			{
				throw JsonWriterException.Create(this, "Unexpected end when reading token.", null);
			}
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0000F3CC File Offset: 0x0000D5CC
		private bool IsWriteTokenIncomplete(JsonReader reader, bool writeChildren, int initialDepth)
		{
			int num = this.CalculateWriteTokenFinalDepth(reader);
			return initialDepth < num || (writeChildren && initialDepth == num && JsonTokenUtils.IsStartToken(reader.TokenType));
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0000F3FC File Offset: 0x0000D5FC
		private int CalculateWriteTokenInitialDepth(JsonReader reader)
		{
			JsonToken tokenType = reader.TokenType;
			if (tokenType == JsonToken.None)
			{
				return -1;
			}
			if (!JsonTokenUtils.IsStartToken(tokenType))
			{
				return reader.Depth + 1;
			}
			return reader.Depth;
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0000F42C File Offset: 0x0000D62C
		private int CalculateWriteTokenFinalDepth(JsonReader reader)
		{
			JsonToken tokenType = reader.TokenType;
			if (tokenType == JsonToken.None)
			{
				return -1;
			}
			if (!JsonTokenUtils.IsEndToken(tokenType))
			{
				return reader.Depth;
			}
			return reader.Depth - 1;
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0000F45C File Offset: 0x0000D65C
		private void WriteConstructorDate(JsonReader reader)
		{
			DateTime dateTime;
			string text;
			if (!JavaScriptUtils.TryGetDateFromConstructorJson(reader, out dateTime, out text))
			{
				throw JsonWriterException.Create(this, text, null);
			}
			this.WriteValue(dateTime);
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0000F488 File Offset: 0x0000D688
		private void WriteEnd(JsonContainerType type)
		{
			switch (type)
			{
			case JsonContainerType.Object:
				this.WriteEndObject();
				return;
			case JsonContainerType.Array:
				this.WriteEndArray();
				return;
			case JsonContainerType.Constructor:
				this.WriteEndConstructor();
				return;
			default:
				throw JsonWriterException.Create(this, "Unexpected type when writing end: " + type.ToString(), null);
			}
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0000F4DE File Offset: 0x0000D6DE
		private void AutoCompleteAll()
		{
			while (this.Top > 0)
			{
				this.WriteEnd();
			}
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0000F4F1 File Offset: 0x0000D6F1
		private JsonToken GetCloseTokenForType(JsonContainerType type)
		{
			switch (type)
			{
			case JsonContainerType.Object:
				return JsonToken.EndObject;
			case JsonContainerType.Array:
				return JsonToken.EndArray;
			case JsonContainerType.Constructor:
				return JsonToken.EndConstructor;
			default:
				throw JsonWriterException.Create(this, "No close token for type: " + type.ToString(), null);
			}
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0000F530 File Offset: 0x0000D730
		private void AutoCompleteClose(JsonContainerType type)
		{
			int num = this.CalculateLevelsToComplete(type);
			for (int i = 0; i < num; i++)
			{
				JsonToken closeTokenForType = this.GetCloseTokenForType(this.Pop());
				if (this._currentState == JsonWriter.State.Property)
				{
					this.WriteNull();
				}
				if (this._formatting == Formatting.Indented && this._currentState != JsonWriter.State.ObjectStart && this._currentState != JsonWriter.State.ArrayStart)
				{
					this.WriteIndent();
				}
				this.WriteEnd(closeTokenForType);
				this.UpdateCurrentState();
			}
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0000F59C File Offset: 0x0000D79C
		private int CalculateLevelsToComplete(JsonContainerType type)
		{
			int num = 0;
			if (this._currentPosition.Type == type)
			{
				num = 1;
			}
			else
			{
				int num2 = this.Top - 2;
				for (int i = num2; i >= 0; i--)
				{
					int num3 = num2 - i;
					if (this._stack[num3].Type == type)
					{
						num = i + 2;
						break;
					}
				}
			}
			if (num == 0)
			{
				throw JsonWriterException.Create(this, "No token to close.", null);
			}
			return num;
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0000F604 File Offset: 0x0000D804
		private void UpdateCurrentState()
		{
			JsonContainerType jsonContainerType = this.Peek();
			switch (jsonContainerType)
			{
			case JsonContainerType.None:
				this._currentState = JsonWriter.State.Start;
				return;
			case JsonContainerType.Object:
				this._currentState = JsonWriter.State.Object;
				return;
			case JsonContainerType.Array:
				this._currentState = JsonWriter.State.Array;
				return;
			case JsonContainerType.Constructor:
				this._currentState = JsonWriter.State.Array;
				return;
			default:
				throw JsonWriterException.Create(this, "Unknown JsonType: " + jsonContainerType.ToString(), null);
			}
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0000F66E File Offset: 0x0000D86E
		protected virtual void WriteEnd(JsonToken token)
		{
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0000F670 File Offset: 0x0000D870
		protected virtual void WriteIndent()
		{
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0000F672 File Offset: 0x0000D872
		protected virtual void WriteValueDelimiter()
		{
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0000F674 File Offset: 0x0000D874
		protected virtual void WriteIndentSpace()
		{
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0000F678 File Offset: 0x0000D878
		internal void AutoComplete(JsonToken tokenBeingWritten)
		{
			JsonWriter.State state = JsonWriter.StateArray[(int)tokenBeingWritten][(int)this._currentState];
			if (state == JsonWriter.State.Error)
			{
				throw JsonWriterException.Create(this, "Token {0} in state {1} would result in an invalid JSON object.".FormatWith(CultureInfo.InvariantCulture, tokenBeingWritten.ToString(), this._currentState.ToString()), null);
			}
			if ((this._currentState == JsonWriter.State.Object || this._currentState == JsonWriter.State.Array || this._currentState == JsonWriter.State.Constructor) && tokenBeingWritten != JsonToken.Comment)
			{
				this.WriteValueDelimiter();
			}
			if (this._formatting == Formatting.Indented)
			{
				if (this._currentState == JsonWriter.State.Property)
				{
					this.WriteIndentSpace();
				}
				if (this._currentState == JsonWriter.State.Array || this._currentState == JsonWriter.State.ArrayStart || this._currentState == JsonWriter.State.Constructor || this._currentState == JsonWriter.State.ConstructorStart || (tokenBeingWritten == JsonToken.PropertyName && this._currentState != JsonWriter.State.Start))
				{
					this.WriteIndent();
				}
			}
			this._currentState = state;
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0000F748 File Offset: 0x0000D948
		public virtual void WriteNull()
		{
			this.InternalWriteValue(JsonToken.Null);
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0000F752 File Offset: 0x0000D952
		public virtual void WriteUndefined()
		{
			this.InternalWriteValue(JsonToken.Undefined);
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0000F75C File Offset: 0x0000D95C
		[NullableContext(2)]
		public virtual void WriteRaw(string json)
		{
			this.InternalWriteRaw();
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0000F764 File Offset: 0x0000D964
		[NullableContext(2)]
		public virtual void WriteRawValue(string json)
		{
			this.UpdateScopeWithFinishedValue();
			this.AutoComplete(JsonToken.Undefined);
			this.WriteRaw(json);
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0000F77B File Offset: 0x0000D97B
		[NullableContext(2)]
		public virtual void WriteValue(string value)
		{
			this.InternalWriteValue(JsonToken.String);
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0000F785 File Offset: 0x0000D985
		public virtual void WriteValue(int value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0000F78E File Offset: 0x0000D98E
		[CLSCompliant(false)]
		public virtual void WriteValue(uint value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0000F797 File Offset: 0x0000D997
		public virtual void WriteValue(long value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0000F7A0 File Offset: 0x0000D9A0
		[CLSCompliant(false)]
		public virtual void WriteValue(ulong value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0000F7A9 File Offset: 0x0000D9A9
		public virtual void WriteValue(float value)
		{
			this.InternalWriteValue(JsonToken.Float);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0000F7B2 File Offset: 0x0000D9B2
		public virtual void WriteValue(double value)
		{
			this.InternalWriteValue(JsonToken.Float);
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000F7BB File Offset: 0x0000D9BB
		public virtual void WriteValue(bool value)
		{
			this.InternalWriteValue(JsonToken.Boolean);
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0000F7C5 File Offset: 0x0000D9C5
		public virtual void WriteValue(short value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0000F7CE File Offset: 0x0000D9CE
		[CLSCompliant(false)]
		public virtual void WriteValue(ushort value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0000F7D7 File Offset: 0x0000D9D7
		public virtual void WriteValue(char value)
		{
			this.InternalWriteValue(JsonToken.String);
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0000F7E1 File Offset: 0x0000D9E1
		public virtual void WriteValue(byte value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0000F7EA File Offset: 0x0000D9EA
		[CLSCompliant(false)]
		public virtual void WriteValue(sbyte value)
		{
			this.InternalWriteValue(JsonToken.Integer);
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0000F7F3 File Offset: 0x0000D9F3
		public virtual void WriteValue(decimal value)
		{
			this.InternalWriteValue(JsonToken.Float);
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0000F7FC File Offset: 0x0000D9FC
		public virtual void WriteValue(DateTime value)
		{
			this.InternalWriteValue(JsonToken.Date);
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0000F806 File Offset: 0x0000DA06
		public virtual void WriteValue(DateTimeOffset value)
		{
			this.InternalWriteValue(JsonToken.Date);
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0000F810 File Offset: 0x0000DA10
		public virtual void WriteValue(Guid value)
		{
			this.InternalWriteValue(JsonToken.String);
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0000F81A File Offset: 0x0000DA1A
		public virtual void WriteValue(TimeSpan value)
		{
			this.InternalWriteValue(JsonToken.String);
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0000F824 File Offset: 0x0000DA24
		public virtual void WriteValue(int? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000F843 File Offset: 0x0000DA43
		[CLSCompliant(false)]
		public virtual void WriteValue(uint? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0000F862 File Offset: 0x0000DA62
		public virtual void WriteValue(long? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000F881 File Offset: 0x0000DA81
		[CLSCompliant(false)]
		public virtual void WriteValue(ulong? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0000F8A0 File Offset: 0x0000DAA0
		public virtual void WriteValue(float? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000F8BF File Offset: 0x0000DABF
		public virtual void WriteValue(double? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0000F8DE File Offset: 0x0000DADE
		public virtual void WriteValue(bool? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0000F8FD File Offset: 0x0000DAFD
		public virtual void WriteValue(short? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0000F91C File Offset: 0x0000DB1C
		[CLSCompliant(false)]
		public virtual void WriteValue(ushort? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0000F93B File Offset: 0x0000DB3B
		public virtual void WriteValue(char? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000F95A File Offset: 0x0000DB5A
		public virtual void WriteValue(byte? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0000F979 File Offset: 0x0000DB79
		[CLSCompliant(false)]
		public virtual void WriteValue(sbyte? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0000F998 File Offset: 0x0000DB98
		public virtual void WriteValue(decimal? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000F9B7 File Offset: 0x0000DBB7
		public virtual void WriteValue(DateTime? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0000F9D6 File Offset: 0x0000DBD6
		public virtual void WriteValue(DateTimeOffset? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0000F9F5 File Offset: 0x0000DBF5
		public virtual void WriteValue(Guid? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0000FA14 File Offset: 0x0000DC14
		public virtual void WriteValue(TimeSpan? value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.WriteValue(value.GetValueOrDefault());
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0000FA33 File Offset: 0x0000DC33
		[NullableContext(2)]
		public virtual void WriteValue(byte[] value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.InternalWriteValue(JsonToken.Bytes);
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0000FA47 File Offset: 0x0000DC47
		[NullableContext(2)]
		public virtual void WriteValue(Uri value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			this.InternalWriteValue(JsonToken.String);
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0000FA61 File Offset: 0x0000DC61
		[NullableContext(2)]
		public virtual void WriteValue(object value)
		{
			if (value == null)
			{
				this.WriteNull();
				return;
			}
			if (value is BigInteger)
			{
				throw JsonWriter.CreateUnsupportedTypeException(this, value);
			}
			JsonWriter.WriteValue(this, ConvertUtils.GetTypeCode(value.GetType()), value);
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0000FA8F File Offset: 0x0000DC8F
		[NullableContext(2)]
		public virtual void WriteComment(string text)
		{
			this.InternalWriteComment();
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0000FA97 File Offset: 0x0000DC97
		public virtual void WriteWhitespace(string ws)
		{
			this.InternalWriteWhitespace(ws);
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0000FAA0 File Offset: 0x0000DCA0
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0000FAAF File Offset: 0x0000DCAF
		protected virtual void Dispose(bool disposing)
		{
			if (this._currentState != JsonWriter.State.Closed && disposing)
			{
				this.Close();
			}
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0000FAC8 File Offset: 0x0000DCC8
		internal static void WriteValue(JsonWriter writer, PrimitiveTypeCode typeCode, object value)
		{
			for (;;)
			{
				switch (typeCode)
				{
				case PrimitiveTypeCode.Char:
					goto IL_00AD;
				case PrimitiveTypeCode.CharNullable:
					goto IL_00BA;
				case PrimitiveTypeCode.Boolean:
					goto IL_00DA;
				case PrimitiveTypeCode.BooleanNullable:
					goto IL_00E7;
				case PrimitiveTypeCode.SByte:
					goto IL_0107;
				case PrimitiveTypeCode.SByteNullable:
					goto IL_0114;
				case PrimitiveTypeCode.Int16:
					goto IL_0134;
				case PrimitiveTypeCode.Int16Nullable:
					goto IL_0141;
				case PrimitiveTypeCode.UInt16:
					goto IL_0162;
				case PrimitiveTypeCode.UInt16Nullable:
					goto IL_016F;
				case PrimitiveTypeCode.Int32:
					goto IL_0190;
				case PrimitiveTypeCode.Int32Nullable:
					goto IL_019D;
				case PrimitiveTypeCode.Byte:
					goto IL_01BE;
				case PrimitiveTypeCode.ByteNullable:
					goto IL_01CB;
				case PrimitiveTypeCode.UInt32:
					goto IL_01EC;
				case PrimitiveTypeCode.UInt32Nullable:
					goto IL_01F9;
				case PrimitiveTypeCode.Int64:
					goto IL_021A;
				case PrimitiveTypeCode.Int64Nullable:
					goto IL_0227;
				case PrimitiveTypeCode.UInt64:
					goto IL_0248;
				case PrimitiveTypeCode.UInt64Nullable:
					goto IL_0255;
				case PrimitiveTypeCode.Single:
					goto IL_0276;
				case PrimitiveTypeCode.SingleNullable:
					goto IL_0283;
				case PrimitiveTypeCode.Double:
					goto IL_02A4;
				case PrimitiveTypeCode.DoubleNullable:
					goto IL_02B1;
				case PrimitiveTypeCode.DateTime:
					goto IL_02D2;
				case PrimitiveTypeCode.DateTimeNullable:
					goto IL_02DF;
				case PrimitiveTypeCode.DateTimeOffset:
					goto IL_0300;
				case PrimitiveTypeCode.DateTimeOffsetNullable:
					goto IL_030D;
				case PrimitiveTypeCode.Decimal:
					goto IL_032E;
				case PrimitiveTypeCode.DecimalNullable:
					goto IL_033B;
				case PrimitiveTypeCode.Guid:
					goto IL_035C;
				case PrimitiveTypeCode.GuidNullable:
					goto IL_0369;
				case PrimitiveTypeCode.TimeSpan:
					goto IL_038A;
				case PrimitiveTypeCode.TimeSpanNullable:
					goto IL_0397;
				case PrimitiveTypeCode.BigInteger:
					goto IL_03B8;
				case PrimitiveTypeCode.BigIntegerNullable:
					goto IL_03CA;
				case PrimitiveTypeCode.Uri:
					goto IL_03F0;
				case PrimitiveTypeCode.String:
					goto IL_03FD;
				case PrimitiveTypeCode.Bytes:
					goto IL_040A;
				case PrimitiveTypeCode.DBNull:
					goto IL_0417;
				default:
				{
					IConvertible convertible = value as IConvertible;
					if (convertible == null)
					{
						goto IL_0437;
					}
					JsonWriter.ResolveConvertibleValue(convertible, out typeCode, out value);
					break;
				}
				}
			}
			IL_00AD:
			writer.WriteValue((char)value);
			return;
			IL_00BA:
			writer.WriteValue((value == null) ? null : new char?((char)value));
			return;
			IL_00DA:
			writer.WriteValue((bool)value);
			return;
			IL_00E7:
			writer.WriteValue((value == null) ? null : new bool?((bool)value));
			return;
			IL_0107:
			writer.WriteValue((sbyte)value);
			return;
			IL_0114:
			writer.WriteValue((value == null) ? null : new sbyte?((sbyte)value));
			return;
			IL_0134:
			writer.WriteValue((short)value);
			return;
			IL_0141:
			writer.WriteValue((value == null) ? null : new short?((short)value));
			return;
			IL_0162:
			writer.WriteValue((ushort)value);
			return;
			IL_016F:
			writer.WriteValue((value == null) ? null : new ushort?((ushort)value));
			return;
			IL_0190:
			writer.WriteValue((int)value);
			return;
			IL_019D:
			writer.WriteValue((value == null) ? null : new int?((int)value));
			return;
			IL_01BE:
			writer.WriteValue((byte)value);
			return;
			IL_01CB:
			writer.WriteValue((value == null) ? null : new byte?((byte)value));
			return;
			IL_01EC:
			writer.WriteValue((uint)value);
			return;
			IL_01F9:
			writer.WriteValue((value == null) ? null : new uint?((uint)value));
			return;
			IL_021A:
			writer.WriteValue((long)value);
			return;
			IL_0227:
			writer.WriteValue((value == null) ? null : new long?((long)value));
			return;
			IL_0248:
			writer.WriteValue((ulong)value);
			return;
			IL_0255:
			writer.WriteValue((value == null) ? null : new ulong?((ulong)value));
			return;
			IL_0276:
			writer.WriteValue((float)value);
			return;
			IL_0283:
			writer.WriteValue((value == null) ? null : new float?((float)value));
			return;
			IL_02A4:
			writer.WriteValue((double)value);
			return;
			IL_02B1:
			writer.WriteValue((value == null) ? null : new double?((double)value));
			return;
			IL_02D2:
			writer.WriteValue((DateTime)value);
			return;
			IL_02DF:
			writer.WriteValue((value == null) ? null : new DateTime?((DateTime)value));
			return;
			IL_0300:
			writer.WriteValue((DateTimeOffset)value);
			return;
			IL_030D:
			writer.WriteValue((value == null) ? null : new DateTimeOffset?((DateTimeOffset)value));
			return;
			IL_032E:
			writer.WriteValue((decimal)value);
			return;
			IL_033B:
			writer.WriteValue((value == null) ? null : new decimal?((decimal)value));
			return;
			IL_035C:
			writer.WriteValue((Guid)value);
			return;
			IL_0369:
			writer.WriteValue((value == null) ? null : new Guid?((Guid)value));
			return;
			IL_038A:
			writer.WriteValue((TimeSpan)value);
			return;
			IL_0397:
			writer.WriteValue((value == null) ? null : new TimeSpan?((TimeSpan)value));
			return;
			IL_03B8:
			writer.WriteValue((BigInteger)value);
			return;
			IL_03CA:
			writer.WriteValue((value == null) ? null : new BigInteger?((BigInteger)value));
			return;
			IL_03F0:
			writer.WriteValue((Uri)value);
			return;
			IL_03FD:
			writer.WriteValue((string)value);
			return;
			IL_040A:
			writer.WriteValue((byte[])value);
			return;
			IL_0417:
			writer.WriteNull();
			return;
			IL_0437:
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			throw JsonWriter.CreateUnsupportedTypeException(writer, value);
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0000FF20 File Offset: 0x0000E120
		private static void ResolveConvertibleValue(IConvertible convertible, out PrimitiveTypeCode typeCode, out object value)
		{
			TypeInformation typeInformation = ConvertUtils.GetTypeInformation(convertible);
			typeCode = ((typeInformation.TypeCode == PrimitiveTypeCode.Object) ? PrimitiveTypeCode.String : typeInformation.TypeCode);
			Type type = ((typeInformation.TypeCode == PrimitiveTypeCode.Object) ? typeof(string) : typeInformation.Type);
			value = convertible.ToType(type, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0000FF73 File Offset: 0x0000E173
		private static JsonWriterException CreateUnsupportedTypeException(JsonWriter writer, object value)
		{
			return JsonWriterException.Create(writer, "Unsupported type: {0}. Use the JsonSerializer class to get the object's JSON representation.".FormatWith(CultureInfo.InvariantCulture, value.GetType()), null);
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0000FF94 File Offset: 0x0000E194
		protected void SetWriteState(JsonToken token, object value)
		{
			switch (token)
			{
			case JsonToken.StartObject:
				this.InternalWriteStart(token, JsonContainerType.Object);
				return;
			case JsonToken.StartArray:
				this.InternalWriteStart(token, JsonContainerType.Array);
				return;
			case JsonToken.StartConstructor:
				this.InternalWriteStart(token, JsonContainerType.Constructor);
				return;
			case JsonToken.PropertyName:
			{
				string text = value as string;
				if (text == null)
				{
					throw new ArgumentException("A name is required when setting property name state.", "value");
				}
				this.InternalWritePropertyName(text);
				return;
			}
			case JsonToken.Comment:
				this.InternalWriteComment();
				return;
			case JsonToken.Raw:
				this.InternalWriteRaw();
				return;
			case JsonToken.Integer:
			case JsonToken.Float:
			case JsonToken.String:
			case JsonToken.Boolean:
			case JsonToken.Null:
			case JsonToken.Undefined:
			case JsonToken.Date:
			case JsonToken.Bytes:
				this.InternalWriteValue(token);
				return;
			case JsonToken.EndObject:
				this.InternalWriteEnd(JsonContainerType.Object);
				return;
			case JsonToken.EndArray:
				this.InternalWriteEnd(JsonContainerType.Array);
				return;
			case JsonToken.EndConstructor:
				this.InternalWriteEnd(JsonContainerType.Constructor);
				return;
			default:
				throw new ArgumentOutOfRangeException("token");
			}
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x00010064 File Offset: 0x0000E264
		internal void InternalWriteEnd(JsonContainerType container)
		{
			this.AutoCompleteClose(container);
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0001006D File Offset: 0x0000E26D
		internal void InternalWritePropertyName(string name)
		{
			this._currentPosition.PropertyName = name;
			this.AutoComplete(JsonToken.PropertyName);
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00010082 File Offset: 0x0000E282
		internal void InternalWriteRaw()
		{
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x00010084 File Offset: 0x0000E284
		internal void InternalWriteStart(JsonToken token, JsonContainerType container)
		{
			this.UpdateScopeWithFinishedValue();
			this.AutoComplete(token);
			this.Push(container);
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0001009A File Offset: 0x0000E29A
		internal void InternalWriteValue(JsonToken token)
		{
			this.UpdateScopeWithFinishedValue();
			this.AutoComplete(token);
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x000100A9 File Offset: 0x0000E2A9
		internal void InternalWriteWhitespace(string ws)
		{
			if (ws != null && !StringUtils.IsWhiteSpace(ws))
			{
				throw JsonWriterException.Create(this, "Only white space characters should be used.", null);
			}
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x000100C3 File Offset: 0x0000E2C3
		internal void InternalWriteComment()
		{
			this.AutoComplete(JsonToken.Comment);
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x000100CC File Offset: 0x0000E2CC
		[CompilerGenerated]
		private async Task <InternalWriteEndAsync>g__AwaitProperty|11_0(Task task, int LevelsToComplete, JsonToken token, CancellationToken CancellationToken)
		{
			await task.ConfigureAwait(false);
			if (this._formatting == Formatting.Indented && this._currentState != JsonWriter.State.ObjectStart && this._currentState != JsonWriter.State.ArrayStart)
			{
				await this.WriteIndentAsync(CancellationToken).ConfigureAwait(false);
			}
			await this.WriteEndAsync(token, CancellationToken).ConfigureAwait(false);
			this.UpdateCurrentState();
			await this.<InternalWriteEndAsync>g__AwaitRemaining|11_3(LevelsToComplete, CancellationToken).ConfigureAwait(false);
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00010130 File Offset: 0x0000E330
		[CompilerGenerated]
		private async Task <InternalWriteEndAsync>g__AwaitIndent|11_1(Task task, int LevelsToComplete, JsonToken token, CancellationToken CancellationToken)
		{
			await task.ConfigureAwait(false);
			await this.WriteEndAsync(token, CancellationToken).ConfigureAwait(false);
			this.UpdateCurrentState();
			await this.<InternalWriteEndAsync>g__AwaitRemaining|11_3(LevelsToComplete, CancellationToken).ConfigureAwait(false);
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x00010194 File Offset: 0x0000E394
		[CompilerGenerated]
		private async Task <InternalWriteEndAsync>g__AwaitEnd|11_2(Task task, int LevelsToComplete, CancellationToken CancellationToken)
		{
			await task.ConfigureAwait(false);
			this.UpdateCurrentState();
			await this.<InternalWriteEndAsync>g__AwaitRemaining|11_3(LevelsToComplete, CancellationToken).ConfigureAwait(false);
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x000101F0 File Offset: 0x0000E3F0
		[CompilerGenerated]
		private async Task <InternalWriteEndAsync>g__AwaitRemaining|11_3(int LevelsToComplete, CancellationToken CancellationToken)
		{
			while (LevelsToComplete-- > 0)
			{
				JsonToken token = this.GetCloseTokenForType(this.Pop());
				if (this._currentState == JsonWriter.State.Property)
				{
					await this.WriteNullAsync(CancellationToken).ConfigureAwait(false);
				}
				if (this._formatting == Formatting.Indented && this._currentState != JsonWriter.State.ObjectStart && this._currentState != JsonWriter.State.ArrayStart)
				{
					await this.WriteIndentAsync(CancellationToken).ConfigureAwait(false);
				}
				await this.WriteEndAsync(token, CancellationToken).ConfigureAwait(false);
				this.UpdateCurrentState();
			}
		}

		// Token: 0x04000107 RID: 263
		private static readonly JsonWriter.State[][] StateArray = JsonWriter.BuildStateArray();

		// Token: 0x04000108 RID: 264
		internal static readonly JsonWriter.State[][] StateArrayTemplate = new JsonWriter.State[][]
		{
			new JsonWriter.State[]
			{
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.ObjectStart,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.ArrayStart,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.ConstructorStart,
				JsonWriter.State.ConstructorStart,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.ConstructorStart,
				JsonWriter.State.ConstructorStart,
				JsonWriter.State.ConstructorStart,
				JsonWriter.State.ConstructorStart,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.Property,
				JsonWriter.State.Error,
				JsonWriter.State.Property,
				JsonWriter.State.Property,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.Start,
				JsonWriter.State.Property,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.Object,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.Array,
				JsonWriter.State.Constructor,
				JsonWriter.State.Constructor,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.Start,
				JsonWriter.State.Property,
				JsonWriter.State.ObjectStart,
				JsonWriter.State.Object,
				JsonWriter.State.ArrayStart,
				JsonWriter.State.Array,
				JsonWriter.State.Constructor,
				JsonWriter.State.Constructor,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			},
			new JsonWriter.State[]
			{
				JsonWriter.State.Start,
				JsonWriter.State.Object,
				JsonWriter.State.Error,
				JsonWriter.State.Error,
				JsonWriter.State.Array,
				JsonWriter.State.Array,
				JsonWriter.State.Constructor,
				JsonWriter.State.Constructor,
				JsonWriter.State.Error,
				JsonWriter.State.Error
			}
		};

		// Token: 0x04000109 RID: 265
		[Nullable(2)]
		private List<JsonPosition> _stack;

		// Token: 0x0400010A RID: 266
		private JsonPosition _currentPosition;

		// Token: 0x0400010B RID: 267
		private JsonWriter.State _currentState;

		// Token: 0x0400010C RID: 268
		private Formatting _formatting;

		// Token: 0x0400010F RID: 271
		private DateFormatHandling _dateFormatHandling;

		// Token: 0x04000110 RID: 272
		private DateTimeZoneHandling _dateTimeZoneHandling;

		// Token: 0x04000111 RID: 273
		private StringEscapeHandling _stringEscapeHandling;

		// Token: 0x04000112 RID: 274
		private FloatFormatHandling _floatFormatHandling;

		// Token: 0x04000113 RID: 275
		[Nullable(2)]
		private string _dateFormatString;

		// Token: 0x04000114 RID: 276
		[Nullable(2)]
		private CultureInfo _culture;

		// Token: 0x02000156 RID: 342
		[NullableContext(0)]
		internal enum State
		{
			// Token: 0x04000635 RID: 1589
			Start,
			// Token: 0x04000636 RID: 1590
			Property,
			// Token: 0x04000637 RID: 1591
			ObjectStart,
			// Token: 0x04000638 RID: 1592
			Object,
			// Token: 0x04000639 RID: 1593
			ArrayStart,
			// Token: 0x0400063A RID: 1594
			Array,
			// Token: 0x0400063B RID: 1595
			ConstructorStart,
			// Token: 0x0400063C RID: 1596
			Constructor,
			// Token: 0x0400063D RID: 1597
			Closed,
			// Token: 0x0400063E RID: 1598
			Error
		}
	}
}
