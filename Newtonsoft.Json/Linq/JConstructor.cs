using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000B7 RID: 183
	[NullableContext(1)]
	[Nullable(0)]
	public class JConstructor : JContainer
	{
		// Token: 0x0600098E RID: 2446 RVA: 0x000274A8 File Offset: 0x000256A8
		public override async Task WriteToAsync(JsonWriter writer, CancellationToken cancellationToken, params JsonConverter[] converters)
		{
			await writer.WriteStartConstructorAsync(this._name ?? string.Empty, cancellationToken).ConfigureAwait(false);
			for (int i = 0; i < this._values.Count; i++)
			{
				await this._values[i].WriteToAsync(writer, cancellationToken, converters).ConfigureAwait(false);
			}
			await writer.WriteEndConstructorAsync(cancellationToken).ConfigureAwait(false);
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x00027503 File Offset: 0x00025703
		public new static Task<JConstructor> LoadAsync(JsonReader reader, CancellationToken cancellationToken = default(CancellationToken))
		{
			return JConstructor.LoadAsync(reader, null, cancellationToken);
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x00027510 File Offset: 0x00025710
		public new static async Task<JConstructor> LoadAsync(JsonReader reader, [Nullable(2)] JsonLoadSettings settings, CancellationToken cancellationToken = default(CancellationToken))
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
					throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader.");
				}
			}
			await reader.MoveToContentAsync(cancellationToken).ConfigureAwait(false);
			if (reader.TokenType != JsonToken.StartConstructor)
			{
				throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader. Current JsonReader item is not a constructor: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JConstructor c = new JConstructor((string)reader.Value);
			c.SetLineInfo(reader as IJsonLineInfo, settings);
			await c.ReadTokenFromAsync(reader, settings, cancellationToken).ConfigureAwait(false);
			return c;
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000991 RID: 2449 RVA: 0x00027563 File Offset: 0x00025763
		protected override IList<JToken> ChildrenTokens
		{
			get
			{
				return this._values;
			}
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x0002756B File Offset: 0x0002576B
		[NullableContext(2)]
		internal override int IndexOfItem(JToken item)
		{
			if (item == null)
			{
				return -1;
			}
			return this._values.IndexOfReference(item);
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x00027580 File Offset: 0x00025780
		internal override void MergeItem(object content, [Nullable(2)] JsonMergeSettings settings)
		{
			JConstructor jconstructor = content as JConstructor;
			if (jconstructor == null)
			{
				return;
			}
			if (jconstructor.Name != null)
			{
				this.Name = jconstructor.Name;
			}
			JContainer.MergeEnumerableContent(this, jconstructor, settings);
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000994 RID: 2452 RVA: 0x000275B4 File Offset: 0x000257B4
		// (set) Token: 0x06000995 RID: 2453 RVA: 0x000275BC File Offset: 0x000257BC
		[Nullable(2)]
		public string Name
		{
			[NullableContext(2)]
			get
			{
				return this._name;
			}
			[NullableContext(2)]
			set
			{
				this._name = value;
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000996 RID: 2454 RVA: 0x000275C5 File Offset: 0x000257C5
		public override JTokenType Type
		{
			get
			{
				return JTokenType.Constructor;
			}
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x000275C8 File Offset: 0x000257C8
		public JConstructor()
		{
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x000275DB File Offset: 0x000257DB
		public JConstructor(JConstructor other)
			: base(other, null)
		{
			this._name = other.Name;
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x000275FC File Offset: 0x000257FC
		internal JConstructor(JConstructor other, [Nullable(2)] JsonCloneSettings settings)
			: base(other, settings)
		{
			this._name = other.Name;
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x0002761D File Offset: 0x0002581D
		public JConstructor(string name, params object[] content)
			: this(name, content)
		{
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x00027627 File Offset: 0x00025827
		public JConstructor(string name, object content)
			: this(name)
		{
			this.Add(content);
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x00027637 File Offset: 0x00025837
		public JConstructor(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException("Constructor name cannot be empty.", "name");
			}
			this._name = name;
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x00027678 File Offset: 0x00025878
		internal override bool DeepEquals(JToken node)
		{
			JConstructor jconstructor = node as JConstructor;
			return jconstructor != null && this._name == jconstructor.Name && base.ContentsEqual(jconstructor);
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x000276AB File Offset: 0x000258AB
		internal override JToken CloneToken([Nullable(2)] JsonCloneSettings settings = null)
		{
			return new JConstructor(this, settings);
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x000276B4 File Offset: 0x000258B4
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WriteStartConstructor(this._name);
			int count = this._values.Count;
			for (int i = 0; i < count; i++)
			{
				this._values[i].WriteTo(writer, converters);
			}
			writer.WriteEndConstructor();
		}

		// Token: 0x170001C1 RID: 449
		[Nullable(2)]
		public override JToken this[object key]
		{
			[return: Nullable(2)]
			get
			{
				ValidationUtils.ArgumentNotNull(key, "key");
				if (key is int)
				{
					int num = (int)key;
					return this.GetItem(num);
				}
				throw new ArgumentException("Accessed JConstructor values with invalid key value: {0}. Argument position index expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
			}
			[param: Nullable(2)]
			set
			{
				ValidationUtils.ArgumentNotNull(key, "key");
				if (key is int)
				{
					int num = (int)key;
					this.SetItem(num, value);
					return;
				}
				throw new ArgumentException("Set JConstructor values with invalid key value: {0}. Argument position index expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
			}
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x00027798 File Offset: 0x00025998
		internal override int GetDeepHashCode()
		{
			string name = this._name;
			return ((name != null) ? name.GetHashCode() : 0) ^ base.ContentsHashCode();
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x000277B3 File Offset: 0x000259B3
		public new static JConstructor Load(JsonReader reader)
		{
			return JConstructor.Load(reader, null);
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x000277BC File Offset: 0x000259BC
		public new static JConstructor Load(JsonReader reader, [Nullable(2)] JsonLoadSettings settings)
		{
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader.");
			}
			reader.MoveToContent();
			if (reader.TokenType != JsonToken.StartConstructor)
			{
				throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader. Current JsonReader item is not a constructor: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JConstructor jconstructor = new JConstructor((string)reader.Value);
			jconstructor.SetLineInfo(reader as IJsonLineInfo, settings);
			jconstructor.ReadTokenFrom(reader, settings);
			return jconstructor;
		}

		// Token: 0x04000373 RID: 883
		[Nullable(2)]
		private string _name;

		// Token: 0x04000374 RID: 884
		private readonly List<JToken> _values = new List<JToken>();
	}
}
