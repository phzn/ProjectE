using System;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000C1 RID: 193
	public class JsonMergeSettings
	{
		// Token: 0x06000AA7 RID: 2727 RVA: 0x0002A029 File Offset: 0x00028229
		public JsonMergeSettings()
		{
			this._propertyNameComparison = StringComparison.Ordinal;
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000AA8 RID: 2728 RVA: 0x0002A038 File Offset: 0x00028238
		// (set) Token: 0x06000AA9 RID: 2729 RVA: 0x0002A040 File Offset: 0x00028240
		public MergeArrayHandling MergeArrayHandling
		{
			get
			{
				return this._mergeArrayHandling;
			}
			set
			{
				if (value < MergeArrayHandling.Concat || value > MergeArrayHandling.Merge)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._mergeArrayHandling = value;
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000AAA RID: 2730 RVA: 0x0002A05C File Offset: 0x0002825C
		// (set) Token: 0x06000AAB RID: 2731 RVA: 0x0002A064 File Offset: 0x00028264
		public MergeNullValueHandling MergeNullValueHandling
		{
			get
			{
				return this._mergeNullValueHandling;
			}
			set
			{
				if (value < MergeNullValueHandling.Ignore || value > MergeNullValueHandling.Merge)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._mergeNullValueHandling = value;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000AAC RID: 2732 RVA: 0x0002A080 File Offset: 0x00028280
		// (set) Token: 0x06000AAD RID: 2733 RVA: 0x0002A088 File Offset: 0x00028288
		public StringComparison PropertyNameComparison
		{
			get
			{
				return this._propertyNameComparison;
			}
			set
			{
				if (value < StringComparison.CurrentCulture || value > StringComparison.OrdinalIgnoreCase)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._propertyNameComparison = value;
			}
		}

		// Token: 0x04000388 RID: 904
		private MergeArrayHandling _mergeArrayHandling;

		// Token: 0x04000389 RID: 905
		private MergeNullValueHandling _mergeNullValueHandling;

		// Token: 0x0400038A RID: 906
		private StringComparison _propertyNameComparison;
	}
}
