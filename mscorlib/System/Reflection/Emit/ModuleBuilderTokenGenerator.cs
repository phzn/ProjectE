using System;

namespace System.Reflection.Emit
{
	// Token: 0x02000939 RID: 2361
	internal class ModuleBuilderTokenGenerator : TokenGenerator
	{
		// Token: 0x060051E6 RID: 20966 RVA: 0x001004AF File Offset: 0x000FE6AF
		public ModuleBuilderTokenGenerator(ModuleBuilder mb)
		{
			this.mb = mb;
		}

		// Token: 0x060051E7 RID: 20967 RVA: 0x001004BE File Offset: 0x000FE6BE
		public int GetToken(string str)
		{
			return this.mb.GetToken(str);
		}

		// Token: 0x060051E8 RID: 20968 RVA: 0x001004CC File Offset: 0x000FE6CC
		public int GetToken(MemberInfo member, bool create_open_instance)
		{
			return this.mb.GetToken(member, create_open_instance);
		}

		// Token: 0x060051E9 RID: 20969 RVA: 0x001004DB File Offset: 0x000FE6DB
		public int GetToken(MethodBase method, Type[] opt_param_types)
		{
			return this.mb.GetToken(method, opt_param_types);
		}

		// Token: 0x060051EA RID: 20970 RVA: 0x001004EA File Offset: 0x000FE6EA
		public int GetToken(SignatureHelper helper)
		{
			return this.mb.GetToken(helper);
		}

		// Token: 0x040031FE RID: 12798
		private ModuleBuilder mb;
	}
}
