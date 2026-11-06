using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000B6 RID: 182
	[NullableContext(1)]
	[Nullable(0)]
	public class JArray : JContainer, IList<JToken>, ICollection<JToken>, IEnumerable<JToken>, IEnumerable
	{
		// Token: 0x0600096A RID: 2410 RVA: 0x000270A4 File Offset: 0x000252A4
		public override async Task WriteToAsync(JsonWriter writer, CancellationToken cancellationToken, params JsonConverter[] converters)
		{
			await writer.WriteStartArrayAsync(cancellationToken).ConfigureAwait(false);
			for (int i = 0; i < this._values.Count; i++)
			{
				await this._values[i].WriteToAsync(writer, cancellationToken, converters).ConfigureAwait(false);
			}
			await writer.WriteEndArrayAsync(cancellationToken).ConfigureAwait(false);
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x000270FF File Offset: 0x000252FF
		public new static Task<JArray> LoadAsync(JsonReader reader, CancellationToken cancellationToken = default(CancellationToken))
		{
			return JArray.LoadAsync(reader, null, cancellationToken);
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x0002710C File Offset: 0x0002530C
		public new static async Task<JArray> LoadAsync(JsonReader reader, [Nullable(2)] JsonLoadSettings settings, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (reader.TokenType == JsonToken.None)
			{
				ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = reader.ReadAsync(cancellationToken).ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
				}
				if (!configuredTaskAwaiter.GetResult())
				{
					throw JsonReaderException.Create(reader, "Error reading JArray from JsonReader.");
				}
			}
			await reader.MoveToContentAsync(cancellationToken).ConfigureAwait(false);
			if (reader.TokenType != JsonToken.StartArray)
			{
				throw JsonReaderException.Create(reader, "Error reading JArray from JsonReader. Current JsonReader item is not an array: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JArray a = new JArray();
			a.SetLineInfo(reader as IJsonLineInfo, settings);
			await a.ReadTokenFromAsync(reader, settings, cancellationToken).ConfigureAwait(false);
			return a;
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x0600096D RID: 2413 RVA: 0x0002715F File Offset: 0x0002535F
		protected override IList<JToken> ChildrenTokens
		{
			get
			{
				return this._values;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x0600096E RID: 2414 RVA: 0x00027167 File Offset: 0x00025367
		public override JTokenType Type
		{
			get
			{
				return JTokenType.Array;
			}
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x0002716A File Offset: 0x0002536A
		public JArray()
		{
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x0002717D File Offset: 0x0002537D
		public JArray(JArray other)
			: base(other, null)
		{
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x00027192 File Offset: 0x00025392
		internal JArray(JArray other, [Nullable(2)] JsonCloneSettings settings)
			: base(other, settings)
		{
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x000271A7 File Offset: 0x000253A7
		public JArray(params object[] content)
			: this(content)
		{
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x000271B0 File Offset: 0x000253B0
		public JArray(object content)
		{
			this.Add(content);
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x000271CC File Offset: 0x000253CC
		internal override bool DeepEquals(JToken node)
		{
			JArray jarray = node as JArray;
			return jarray != null && base.ContentsEqual(jarray);
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x000271EC File Offset: 0x000253EC
		internal override JToken CloneToken([Nullable(2)] JsonCloneSettings settings = null)
		{
			return new JArray(this, settings);
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x000271F5 File Offset: 0x000253F5
		public new static JArray Load(JsonReader reader)
		{
			return JArray.Load(reader, null);
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x00027200 File Offset: 0x00025400
		public new static JArray Load(JsonReader reader, [Nullable(2)] JsonLoadSettings settings)
		{
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw JsonReaderException.Create(reader, "Error reading JArray from JsonReader.");
			}
			reader.MoveToContent();
			if (reader.TokenType != JsonToken.StartArray)
			{
				throw JsonReaderException.Create(reader, "Error reading JArray from JsonReader. Current JsonReader item is not an array: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JArray jarray = new JArray();
			jarray.SetLineInfo(reader as IJsonLineInfo, settings);
			jarray.ReadTokenFrom(reader, settings);
			return jarray;
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x00027274 File Offset: 0x00025474
		public new static JArray Parse(string json)
		{
			return JArray.Parse(json, null);
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x00027280 File Offset: 0x00025480
		public new static JArray Parse(string json, [Nullable(2)] JsonLoadSettings settings)
		{
			JArray jarray2;
			using (JsonReader jsonReader = new JsonTextReader(new StringReader(json)))
			{
				JArray jarray = JArray.Load(jsonReader, settings);
				while (jsonReader.Read())
				{
				}
				jarray2 = jarray;
			}
			return jarray2;
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x000272C8 File Offset: 0x000254C8
		public new static JArray FromObject(object o)
		{
			return JArray.FromObject(o, JsonSerializer.CreateDefault());
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x000272D8 File Offset: 0x000254D8
		public new static JArray FromObject(object o, JsonSerializer jsonSerializer)
		{
			JToken jtoken = JToken.FromObjectInternal(o, jsonSerializer);
			if (jtoken.Type != JTokenType.Array)
			{
				throw new ArgumentException("Object serialized to {0}. JArray instance expected.".FormatWith(CultureInfo.InvariantCulture, jtoken.Type));
			}
			return (JArray)jtoken;
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x0002731C File Offset: 0x0002551C
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WriteStartArray();
			for (int i = 0; i < this._values.Count; i++)
			{
				this._values[i].WriteTo(writer, converters);
			}
			writer.WriteEndArray();
		}

		// Token: 0x170001BB RID: 443
		[Nullable(2)]
		public override JToken this[object key]
		{
			[return: Nullable(2)]
			get
			{
				ValidationUtils.ArgumentNotNull(key, "key");
				if (!(key is int))
				{
					throw new ArgumentException("Accessed JArray values with invalid key value: {0}. Int32 array index expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
				}
				return this.GetItem((int)key);
			}
			[param: Nullable(2)]
			set
			{
				ValidationUtils.ArgumentNotNull(key, "key");
				if (!(key is int))
				{
					throw new ArgumentException("Set JArray values with invalid key value: {0}. Int32 array index expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
				}
				this.SetItem((int)key, value);
			}
		}

		// Token: 0x170001BC RID: 444
		public JToken this[int index]
		{
			get
			{
				return this.GetItem(index);
			}
			set
			{
				this.SetItem(index, value);
			}
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x000273EA File Offset: 0x000255EA
		[NullableContext(2)]
		internal override int IndexOfItem(JToken item)
		{
			if (item == null)
			{
				return -1;
			}
			return this._values.IndexOfReference(item);
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x00027400 File Offset: 0x00025600
		internal override void MergeItem(object content, [Nullable(2)] JsonMergeSettings settings)
		{
			IEnumerable enumerable = ((base.IsMultiContent(content) || content is JArray) ? ((IEnumerable)content) : null);
			if (enumerable == null)
			{
				return;
			}
			JContainer.MergeEnumerableContent(this, enumerable, settings);
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x00027434 File Offset: 0x00025634
		public int IndexOf(JToken item)
		{
			return this.IndexOfItem(item);
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x0002743D File Offset: 0x0002563D
		public void Insert(int index, JToken item)
		{
			this.InsertItem(index, item, false, true);
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x0002744A File Offset: 0x0002564A
		public void RemoveAt(int index)
		{
			this.RemoveItemAt(index);
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x00027454 File Offset: 0x00025654
		public IEnumerator<JToken> GetEnumerator()
		{
			return this.Children().GetEnumerator();
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x0002746F File Offset: 0x0002566F
		public void Add(JToken item)
		{
			this.Add(item);
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x00027478 File Offset: 0x00025678
		public void Clear()
		{
			this.ClearItems();
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x00027480 File Offset: 0x00025680
		public bool Contains(JToken item)
		{
			return this.ContainsItem(item);
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x00027489 File Offset: 0x00025689
		public void CopyTo(JToken[] array, int arrayIndex)
		{
			this.CopyItemsTo(array, arrayIndex);
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x0600098B RID: 2443 RVA: 0x00027493 File Offset: 0x00025693
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x00027496 File Offset: 0x00025696
		public bool Remove(JToken item)
		{
			return this.RemoveItem(item);
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x0002749F File Offset: 0x0002569F
		internal override int GetDeepHashCode()
		{
			return base.ContentsHashCode();
		}

		// Token: 0x04000372 RID: 882
		private readonly List<JToken> _values = new List<JToken>();
	}
}
