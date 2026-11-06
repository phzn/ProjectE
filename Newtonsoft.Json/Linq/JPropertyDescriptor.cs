using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000BC RID: 188
	[NullableContext(1)]
	[Nullable(0)]
	public class JPropertyDescriptor : PropertyDescriptor
	{
		// Token: 0x06000A77 RID: 2679 RVA: 0x00029A7F File Offset: 0x00027C7F
		public JPropertyDescriptor(string name)
			: base(name, null)
		{
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x00029A89 File Offset: 0x00027C89
		private static JObject CastInstance(object instance)
		{
			return (JObject)instance;
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x00029A91 File Offset: 0x00027C91
		public override bool CanResetValue(object component)
		{
			return false;
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x00029A94 File Offset: 0x00027C94
		[NullableContext(2)]
		public override object GetValue(object component)
		{
			JObject jobject = component as JObject;
			if (jobject == null)
			{
				return null;
			}
			return jobject[this.Name];
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x00029AAD File Offset: 0x00027CAD
		public override void ResetValue(object component)
		{
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x00029AB0 File Offset: 0x00027CB0
		[NullableContext(2)]
		public override void SetValue(object component, object value)
		{
			JObject jobject = component as JObject;
			if (jobject != null)
			{
				JToken jtoken = (value as JToken) ?? new JValue(value);
				jobject[this.Name] = jtoken;
			}
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x00029AE5 File Offset: 0x00027CE5
		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000A7E RID: 2686 RVA: 0x00029AE8 File Offset: 0x00027CE8
		public override Type ComponentType
		{
			get
			{
				return typeof(JObject);
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000A7F RID: 2687 RVA: 0x00029AF4 File Offset: 0x00027CF4
		public override bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000A80 RID: 2688 RVA: 0x00029AF7 File Offset: 0x00027CF7
		public override Type PropertyType
		{
			get
			{
				return typeof(object);
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000A81 RID: 2689 RVA: 0x00029B03 File Offset: 0x00027D03
		protected override int NameHashCode
		{
			get
			{
				return base.NameHashCode;
			}
		}
	}
}
