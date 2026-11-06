using System;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200005E RID: 94
	internal static class JsonTokenUtils
	{
		// Token: 0x06000545 RID: 1349 RVA: 0x000166BE File Offset: 0x000148BE
		internal static bool IsEndToken(JsonToken token)
		{
			return token - JsonToken.EndObject <= 2;
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x000166CA File Offset: 0x000148CA
		internal static bool IsStartToken(JsonToken token)
		{
			return token - JsonToken.StartObject <= 2;
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x000166D5 File Offset: 0x000148D5
		internal static bool IsPrimitiveToken(JsonToken token)
		{
			return token - JsonToken.Integer <= 5 || token - JsonToken.Date <= 1;
		}
	}
}
