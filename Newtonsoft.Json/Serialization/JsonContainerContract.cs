using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000081 RID: 129
	[NullableContext(2)]
	[Nullable(0)]
	public class JsonContainerContract : JsonContract
	{
		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x0600067C RID: 1660 RVA: 0x0001B632 File Offset: 0x00019832
		// (set) Token: 0x0600067D RID: 1661 RVA: 0x0001B63A File Offset: 0x0001983A
		internal JsonContract ItemContract
		{
			get
			{
				return this._itemContract;
			}
			set
			{
				this._itemContract = value;
				if (this._itemContract != null)
				{
					this._finalItemContract = (this._itemContract.UnderlyingType.IsSealed() ? this._itemContract : null);
					return;
				}
				this._finalItemContract = null;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x0600067E RID: 1662 RVA: 0x0001B674 File Offset: 0x00019874
		internal JsonContract FinalItemContract
		{
			get
			{
				return this._finalItemContract;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x0600067F RID: 1663 RVA: 0x0001B67C File Offset: 0x0001987C
		// (set) Token: 0x06000680 RID: 1664 RVA: 0x0001B684 File Offset: 0x00019884
		public JsonConverter ItemConverter { get; set; }

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000681 RID: 1665 RVA: 0x0001B68D File Offset: 0x0001988D
		// (set) Token: 0x06000682 RID: 1666 RVA: 0x0001B695 File Offset: 0x00019895
		public bool? ItemIsReference { get; set; }

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000683 RID: 1667 RVA: 0x0001B69E File Offset: 0x0001989E
		// (set) Token: 0x06000684 RID: 1668 RVA: 0x0001B6A6 File Offset: 0x000198A6
		public ReferenceLoopHandling? ItemReferenceLoopHandling { get; set; }

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000685 RID: 1669 RVA: 0x0001B6AF File Offset: 0x000198AF
		// (set) Token: 0x06000686 RID: 1670 RVA: 0x0001B6B7 File Offset: 0x000198B7
		public TypeNameHandling? ItemTypeNameHandling { get; set; }

		// Token: 0x06000687 RID: 1671 RVA: 0x0001B6C0 File Offset: 0x000198C0
		[NullableContext(1)]
		internal JsonContainerContract(Type underlyingType)
			: base(underlyingType)
		{
			JsonContainerAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>(underlyingType);
			if (cachedAttribute != null)
			{
				if (cachedAttribute.ItemConverterType != null)
				{
					this.ItemConverter = JsonTypeReflector.CreateJsonConverterInstance(cachedAttribute.ItemConverterType, cachedAttribute.ItemConverterParameters);
				}
				this.ItemIsReference = cachedAttribute._itemIsReference;
				this.ItemReferenceLoopHandling = cachedAttribute._itemReferenceLoopHandling;
				this.ItemTypeNameHandling = cachedAttribute._itemTypeNameHandling;
			}
		}

		// Token: 0x04000252 RID: 594
		private JsonContract _itemContract;

		// Token: 0x04000253 RID: 595
		private JsonContract _finalItemContract;
	}
}
