using System;
using Unity.Collections;
using UnityEngine;

namespace Unity.Entities
{
	// Token: 0x0200018B RID: 395
	[GenerateTestsForBurstCompatibility]
	internal struct EntityName
	{
		// Token: 0x060009D2 RID: 2514 RVA: 0x000315DD File Offset: 0x0002F7DD
		public void ToFixedString(ref FixedString64Bytes value)
		{
			EntityNameStorage.GetFixedString(this.Index, ref value);
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x000315EC File Offset: 0x0002F7EC
		[ExcludeFromBurstCompatTesting("Returns managed string")]
		public override string ToString()
		{
			FixedString64Bytes fixedString64Bytes = default(FixedString64Bytes);
			this.ToFixedString(ref fixedString64Bytes);
			return fixedString64Bytes.ToString();
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x00031618 File Offset: 0x0002F818
		public void SetFixedString(in FixedString64Bytes value)
		{
			int orCreateIndex = EntityNameStorage.GetOrCreateIndex(in value);
			if (orCreateIndex >= 0)
			{
				this.Index = orCreateIndex;
				return;
			}
			if (EntityNameStorage.s_State.Data.hasLoggedError == 0)
			{
				Debug.LogError(EntityNameStorage.s_State.Data.kMaxEntriesMsg);
				ref EntityNameStorage.State data = ref EntityNameStorage.s_State.Data;
				data.hasLoggedError += 1;
			}
		}

		// Token: 0x040004AF RID: 1199
		internal int Index;
	}
}
