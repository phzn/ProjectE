using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000BE RID: 190
	[NullableContext(1)]
	[Nullable(0)]
	public class JRaw : JValue
	{
		// Token: 0x06000A96 RID: 2710 RVA: 0x00029E90 File Offset: 0x00028090
		public static async Task<JRaw> CreateAsync(JsonReader reader, CancellationToken cancellationToken = default(CancellationToken))
		{
			JRaw jraw;
			using (StringWriter sw = new StringWriter(CultureInfo.InvariantCulture))
			{
				using (JsonTextWriter jsonWriter = new JsonTextWriter(sw))
				{
					await jsonWriter.WriteTokenSyncReadingAsync(reader, cancellationToken).ConfigureAwait(false);
					jraw = new JRaw(sw.ToString());
				}
			}
			return jraw;
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x00029EDB File Offset: 0x000280DB
		public JRaw(JRaw other)
			: base(other, null)
		{
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x00029EE5 File Offset: 0x000280E5
		internal JRaw(JRaw other, [Nullable(2)] JsonCloneSettings settings)
			: base(other, settings)
		{
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x00029EEF File Offset: 0x000280EF
		[NullableContext(2)]
		public JRaw(object rawJson)
			: base(rawJson, JTokenType.Raw)
		{
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x00029EFC File Offset: 0x000280FC
		public static JRaw Create(JsonReader reader)
		{
			JRaw jraw;
			using (StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture))
			{
				using (JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter))
				{
					jsonTextWriter.WriteToken(reader);
					jraw = new JRaw(stringWriter.ToString());
				}
			}
			return jraw;
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x00029F64 File Offset: 0x00028164
		internal override JToken CloneToken([Nullable(2)] JsonCloneSettings settings)
		{
			return new JRaw(this, settings);
		}
	}
}
