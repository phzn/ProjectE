using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000BB RID: 187
	[NullableContext(1)]
	[Nullable(0)]
	public class JProperty : JContainer
	{
		// Token: 0x06000A59 RID: 2649 RVA: 0x000295B4 File Offset: 0x000277B4
		public override Task WriteToAsync(JsonWriter writer, CancellationToken cancellationToken, params JsonConverter[] converters)
		{
			Task task = writer.WritePropertyNameAsync(this._name, cancellationToken);
			if (task.IsCompletedSuccessfully())
			{
				return this.WriteValueAsync(writer, cancellationToken, converters);
			}
			return this.WriteToAsync(task, writer, cancellationToken, converters);
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x000295EC File Offset: 0x000277EC
		private async Task WriteToAsync(Task task, JsonWriter writer, CancellationToken cancellationToken, params JsonConverter[] converters)
		{
			await task.ConfigureAwait(false);
			await this.WriteValueAsync(writer, cancellationToken, converters).ConfigureAwait(false);
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x00029650 File Offset: 0x00027850
		private Task WriteValueAsync(JsonWriter writer, CancellationToken cancellationToken, JsonConverter[] converters)
		{
			JToken value = this.Value;
			if (value == null)
			{
				return writer.WriteNullAsync(cancellationToken);
			}
			return value.WriteToAsync(writer, cancellationToken, converters);
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x00029678 File Offset: 0x00027878
		public new static Task<JProperty> LoadAsync(JsonReader reader, CancellationToken cancellationToken = default(CancellationToken))
		{
			return JProperty.LoadAsync(reader, null, cancellationToken);
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x00029684 File Offset: 0x00027884
		public new static async Task<JProperty> LoadAsync(JsonReader reader, [Nullable(2)] JsonLoadSettings settings, CancellationToken cancellationToken = default(CancellationToken))
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
					throw JsonReaderException.Create(reader, "Error reading JProperty from JsonReader.");
				}
			}
			await reader.MoveToContentAsync(cancellationToken).ConfigureAwait(false);
			if (reader.TokenType != JsonToken.PropertyName)
			{
				throw JsonReaderException.Create(reader, "Error reading JProperty from JsonReader. Current JsonReader item is not a property: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JProperty p = new JProperty((string)reader.Value);
			p.SetLineInfo(reader as IJsonLineInfo, settings);
			await p.ReadTokenFromAsync(reader, settings, cancellationToken).ConfigureAwait(false);
			return p;
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000A5E RID: 2654 RVA: 0x000296D7 File Offset: 0x000278D7
		protected override IList<JToken> ChildrenTokens
		{
			get
			{
				return this._content;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000A5F RID: 2655 RVA: 0x000296DF File Offset: 0x000278DF
		public string Name
		{
			[DebuggerStepThrough]
			get
			{
				return this._name;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000A60 RID: 2656 RVA: 0x000296E7 File Offset: 0x000278E7
		// (set) Token: 0x06000A61 RID: 2657 RVA: 0x000296F4 File Offset: 0x000278F4
		public new JToken Value
		{
			[DebuggerStepThrough]
			get
			{
				return this._content._token;
			}
			set
			{
				base.CheckReentrancy();
				JToken jtoken = value ?? JValue.CreateNull();
				if (this._content._token == null)
				{
					this.InsertItem(0, jtoken, false, true);
					return;
				}
				this.SetItem(0, jtoken);
			}
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x00029733 File Offset: 0x00027933
		public JProperty(JProperty other)
			: base(other, null)
		{
			this._name = other.Name;
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x00029754 File Offset: 0x00027954
		internal JProperty(JProperty other, [Nullable(2)] JsonCloneSettings settings)
			: base(other, settings)
		{
			this._name = other.Name;
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x00029775 File Offset: 0x00027975
		internal override JToken GetItem(int index)
		{
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			return this.Value;
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x00029788 File Offset: 0x00027988
		[NullableContext(2)]
		internal override void SetItem(int index, JToken item)
		{
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (JContainer.IsTokenUnchanged(this.Value, item))
			{
				return;
			}
			JObject jobject = (JObject)base.Parent;
			if (jobject != null)
			{
				jobject.InternalPropertyChanging(this);
			}
			base.SetItem(0, item);
			JObject jobject2 = (JObject)base.Parent;
			if (jobject2 == null)
			{
				return;
			}
			jobject2.InternalPropertyChanged(this);
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x000297E2 File Offset: 0x000279E2
		[NullableContext(2)]
		internal override bool RemoveItem(JToken item)
		{
			throw new JsonException("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x00029802 File Offset: 0x00027A02
		internal override void RemoveItemAt(int index)
		{
			throw new JsonException("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x00029822 File Offset: 0x00027A22
		[NullableContext(2)]
		internal override int IndexOfItem(JToken item)
		{
			if (item == null)
			{
				return -1;
			}
			return this._content.IndexOf(item);
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x00029838 File Offset: 0x00027A38
		[NullableContext(2)]
		internal override bool InsertItem(int index, JToken item, bool skipParentCheck, bool copyAnnotations)
		{
			if (item != null && item.Type == JTokenType.Comment)
			{
				return false;
			}
			if (this.Value != null)
			{
				throw new JsonException("{0} cannot have multiple values.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
			}
			return base.InsertItem(0, item, false, copyAnnotations);
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x00029885 File Offset: 0x00027A85
		[NullableContext(2)]
		internal override bool ContainsItem(JToken item)
		{
			return this.Value == item;
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x00029890 File Offset: 0x00027A90
		internal override void MergeItem(object content, [Nullable(2)] JsonMergeSettings settings)
		{
			JProperty jproperty = content as JProperty;
			JToken jtoken = ((jproperty != null) ? jproperty.Value : null);
			if (jtoken != null && jtoken.Type != JTokenType.Null)
			{
				this.Value = jtoken;
			}
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x000298C4 File Offset: 0x00027AC4
		internal override void ClearItems()
		{
			throw new JsonException("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x000298E4 File Offset: 0x00027AE4
		internal override bool DeepEquals(JToken node)
		{
			JProperty jproperty = node as JProperty;
			return jproperty != null && this._name == jproperty.Name && base.ContentsEqual(jproperty);
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x00029917 File Offset: 0x00027B17
		internal override JToken CloneToken([Nullable(2)] JsonCloneSettings settings)
		{
			return new JProperty(this, settings);
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000A6F RID: 2671 RVA: 0x00029920 File Offset: 0x00027B20
		public override JTokenType Type
		{
			[DebuggerStepThrough]
			get
			{
				return JTokenType.Property;
			}
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x00029923 File Offset: 0x00027B23
		internal JProperty(string name)
		{
			ValidationUtils.ArgumentNotNull(name, "name");
			this._name = name;
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x00029948 File Offset: 0x00027B48
		public JProperty(string name, params object[] content)
			: this(name, content)
		{
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x00029954 File Offset: 0x00027B54
		public JProperty(string name, [Nullable(2)] object content)
		{
			ValidationUtils.ArgumentNotNull(name, "name");
			this._name = name;
			this.Value = (base.IsMultiContent(content) ? new JArray(content) : JContainer.CreateFromContent(content));
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x000299A4 File Offset: 0x00027BA4
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WritePropertyName(this._name);
			JToken value = this.Value;
			if (value != null)
			{
				value.WriteTo(writer, converters);
				return;
			}
			writer.WriteNull();
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x000299D6 File Offset: 0x00027BD6
		internal override int GetDeepHashCode()
		{
			int hashCode = this._name.GetHashCode();
			JToken value = this.Value;
			return hashCode ^ ((value != null) ? value.GetDeepHashCode() : 0);
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x000299F6 File Offset: 0x00027BF6
		public new static JProperty Load(JsonReader reader)
		{
			return JProperty.Load(reader, null);
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x00029A00 File Offset: 0x00027C00
		public new static JProperty Load(JsonReader reader, [Nullable(2)] JsonLoadSettings settings)
		{
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw JsonReaderException.Create(reader, "Error reading JProperty from JsonReader.");
			}
			reader.MoveToContent();
			if (reader.TokenType != JsonToken.PropertyName)
			{
				throw JsonReaderException.Create(reader, "Error reading JProperty from JsonReader. Current JsonReader item is not a property: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JProperty jproperty = new JProperty((string)reader.Value);
			jproperty.SetLineInfo(reader as IJsonLineInfo, settings);
			jproperty.ReadTokenFrom(reader, settings);
			return jproperty;
		}

		// Token: 0x0400037F RID: 895
		private readonly JProperty.JPropertyList _content = new JProperty.JPropertyList();

		// Token: 0x04000380 RID: 896
		private readonly string _name;

		// Token: 0x020001C7 RID: 455
		[Nullable(0)]
		private class JPropertyList : IList<JToken>, ICollection<JToken>, IEnumerable<JToken>, IEnumerable
		{
			// Token: 0x06000F9A RID: 3994 RVA: 0x0004434E File Offset: 0x0004254E
			public IEnumerator<JToken> GetEnumerator()
			{
				if (this._token != null)
				{
					yield return this._token;
				}
				yield break;
			}

			// Token: 0x06000F9B RID: 3995 RVA: 0x0004435D File Offset: 0x0004255D
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x06000F9C RID: 3996 RVA: 0x00044365 File Offset: 0x00042565
			public void Add(JToken item)
			{
				this._token = item;
			}

			// Token: 0x06000F9D RID: 3997 RVA: 0x0004436E File Offset: 0x0004256E
			public void Clear()
			{
				this._token = null;
			}

			// Token: 0x06000F9E RID: 3998 RVA: 0x00044377 File Offset: 0x00042577
			public bool Contains(JToken item)
			{
				return this._token == item;
			}

			// Token: 0x06000F9F RID: 3999 RVA: 0x00044382 File Offset: 0x00042582
			public void CopyTo(JToken[] array, int arrayIndex)
			{
				if (this._token != null)
				{
					array[arrayIndex] = this._token;
				}
			}

			// Token: 0x06000FA0 RID: 4000 RVA: 0x00044395 File Offset: 0x00042595
			public bool Remove(JToken item)
			{
				if (this._token == item)
				{
					this._token = null;
					return true;
				}
				return false;
			}

			// Token: 0x1700029F RID: 671
			// (get) Token: 0x06000FA1 RID: 4001 RVA: 0x000443AA File Offset: 0x000425AA
			public int Count
			{
				get
				{
					if (this._token == null)
					{
						return 0;
					}
					return 1;
				}
			}

			// Token: 0x170002A0 RID: 672
			// (get) Token: 0x06000FA2 RID: 4002 RVA: 0x000443B7 File Offset: 0x000425B7
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06000FA3 RID: 4003 RVA: 0x000443BA File Offset: 0x000425BA
			public int IndexOf(JToken item)
			{
				if (this._token != item)
				{
					return -1;
				}
				return 0;
			}

			// Token: 0x06000FA4 RID: 4004 RVA: 0x000443C8 File Offset: 0x000425C8
			public void Insert(int index, JToken item)
			{
				if (index == 0)
				{
					this._token = item;
				}
			}

			// Token: 0x06000FA5 RID: 4005 RVA: 0x000443D4 File Offset: 0x000425D4
			public void RemoveAt(int index)
			{
				if (index == 0)
				{
					this._token = null;
				}
			}

			// Token: 0x170002A1 RID: 673
			public JToken this[int index]
			{
				get
				{
					if (index != 0)
					{
						throw new IndexOutOfRangeException();
					}
					return this._token;
				}
				set
				{
					if (index != 0)
					{
						throw new IndexOutOfRangeException();
					}
					this._token = value;
				}
			}

			// Token: 0x040007C4 RID: 1988
			[Nullable(2)]
			internal JToken _token;
		}
	}
}
