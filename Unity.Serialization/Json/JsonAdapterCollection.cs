using System;
using System.Collections.Generic;

namespace Unity.Serialization.Json
{
	// Token: 0x0200001F RID: 31
	internal struct JsonAdapterCollection
	{
		// Token: 0x0600006A RID: 106 RVA: 0x00002E34 File Offset: 0x00001034
		public JsonAdapterCollection.Enumerator GetEnumerator(JsonAdapterFilter filter = JsonAdapterFilter.All)
		{
			return new JsonAdapterCollection.Enumerator(filter.HasFlag(JsonAdapterFilter.UserDefined) ? this.UserDefined : null, filter.HasFlag(JsonAdapterFilter.Global) ? this.Global : null, filter.HasFlag(JsonAdapterFilter.Internal) ? this.InternalAdapter : null);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00002E9C File Offset: 0x0000109C
		public static bool ContainsPrimitiveOrStringAdapter(List<IJsonAdapter> adapters)
		{
			if (adapters == null)
			{
				return false;
			}
			foreach (IJsonAdapter jsonAdapter in adapters)
			{
				if (jsonAdapter is IJsonAdapter<sbyte> || jsonAdapter is IJsonAdapter<short> || jsonAdapter is IJsonAdapter<int> || jsonAdapter is IJsonAdapter<long> || jsonAdapter is IJsonAdapter<byte> || jsonAdapter is IJsonAdapter<ushort> || jsonAdapter is IJsonAdapter<uint> || jsonAdapter is IJsonAdapter<ulong> || jsonAdapter is IJsonAdapter<float> || jsonAdapter is IJsonAdapter<double> || jsonAdapter is IJsonAdapter<bool> || jsonAdapter is IJsonAdapter<char> || jsonAdapter is IJsonAdapter<string>)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400001B RID: 27
		public JsonAdapter InternalAdapter;

		// Token: 0x0400001C RID: 28
		public List<IJsonAdapter> Global;

		// Token: 0x0400001D RID: 29
		public List<IJsonAdapter> UserDefined;

		// Token: 0x02000020 RID: 32
		public struct Enumerator
		{
			// Token: 0x0600006C RID: 108 RVA: 0x00002F58 File Offset: 0x00001158
			public Enumerator(List<IJsonAdapter> userDefinedAdapters, List<IJsonAdapter> globalAdapters, JsonAdapter internalAdapter)
			{
				this.m_UserDefinedAdapters = userDefinedAdapters;
				this.m_GlobalAdapters = globalAdapters;
				this.m_InternalAdapter = internalAdapter;
				this.m_Current = null;
				this.m_State = ((userDefinedAdapters != null) ? JsonAdapterCollection.Enumerator.State.User : ((globalAdapters != null) ? JsonAdapterCollection.Enumerator.State.Global : ((internalAdapter != null) ? JsonAdapterCollection.Enumerator.State.Internal : JsonAdapterCollection.Enumerator.State.End)));
				this.m_Index = -1;
			}

			// Token: 0x17000005 RID: 5
			// (get) Token: 0x0600006D RID: 109 RVA: 0x00002F96 File Offset: 0x00001196
			public IJsonAdapter Current
			{
				get
				{
					return this.m_Current;
				}
			}

			// Token: 0x0600006E RID: 110 RVA: 0x00002FA0 File Offset: 0x000011A0
			public bool MoveNext()
			{
				for (;;)
				{
					this.m_Index++;
					switch (this.m_State)
					{
					case JsonAdapterCollection.Enumerator.State.User:
						if (this.m_Index < this.m_UserDefinedAdapters.Count)
						{
							goto Block_1;
						}
						this.m_State = ((this.m_GlobalAdapters != null) ? JsonAdapterCollection.Enumerator.State.Global : ((this.m_InternalAdapter != null) ? JsonAdapterCollection.Enumerator.State.Internal : JsonAdapterCollection.Enumerator.State.End));
						this.m_Index = -1;
						continue;
					case JsonAdapterCollection.Enumerator.State.Global:
						if (this.m_Index < this.m_GlobalAdapters.Count)
						{
							goto Block_4;
						}
						this.m_State = ((this.m_InternalAdapter != null) ? JsonAdapterCollection.Enumerator.State.Internal : JsonAdapterCollection.Enumerator.State.End);
						this.m_Index = -1;
						continue;
					case JsonAdapterCollection.Enumerator.State.Internal:
						goto IL_00CF;
					case JsonAdapterCollection.Enumerator.State.End:
						goto IL_00EB;
					}
					break;
				}
				throw new ArgumentOutOfRangeException();
				Block_1:
				this.m_Current = this.m_UserDefinedAdapters[this.m_Index];
				return true;
				Block_4:
				this.m_Current = this.m_GlobalAdapters[this.m_Index];
				return true;
				IL_00CF:
				this.m_Current = this.m_InternalAdapter;
				this.m_State = JsonAdapterCollection.Enumerator.State.End;
				this.m_Index = -1;
				return true;
				IL_00EB:
				this.m_Index = -1;
				return false;
			}

			// Token: 0x0400001E RID: 30
			private readonly List<IJsonAdapter> m_UserDefinedAdapters;

			// Token: 0x0400001F RID: 31
			private readonly List<IJsonAdapter> m_GlobalAdapters;

			// Token: 0x04000020 RID: 32
			private readonly JsonAdapter m_InternalAdapter;

			// Token: 0x04000021 RID: 33
			private IJsonAdapter m_Current;

			// Token: 0x04000022 RID: 34
			private JsonAdapterCollection.Enumerator.State m_State;

			// Token: 0x04000023 RID: 35
			private int m_Index;

			// Token: 0x02000021 RID: 33
			private enum State
			{
				// Token: 0x04000025 RID: 37
				User,
				// Token: 0x04000026 RID: 38
				Global,
				// Token: 0x04000027 RID: 39
				Internal,
				// Token: 0x04000028 RID: 40
				End
			}
		}
	}
}
