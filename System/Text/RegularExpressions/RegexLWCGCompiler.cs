using System;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace System.Text.RegularExpressions
{
	// Token: 0x02000205 RID: 517
	internal sealed class RegexLWCGCompiler : RegexCompiler
	{
		// Token: 0x06000E97 RID: 3735 RVA: 0x0003FE00 File Offset: 0x0003E000
		public RegexRunnerFactory FactoryInstanceFromCode(RegexCode code, RegexOptions options)
		{
			this._code = code;
			this._codes = code.Codes;
			this._strings = code.Strings;
			this._fcPrefix = code.FCPrefix;
			this._bmPrefix = code.BMPrefix;
			this._anchors = code.Anchors;
			this._trackcount = code.TrackCount;
			this._options = options;
			string text = Interlocked.Increment(ref RegexLWCGCompiler.s_regexCount).ToString(CultureInfo.InvariantCulture);
			DynamicMethod dynamicMethod = this.DefineDynamicMethod("Go" + text, null, typeof(CompiledRegexRunner));
			base.GenerateGo();
			DynamicMethod dynamicMethod2 = this.DefineDynamicMethod("FindFirstChar" + text, typeof(bool), typeof(CompiledRegexRunner));
			base.GenerateFindFirstChar();
			DynamicMethod dynamicMethod3 = this.DefineDynamicMethod("InitTrackCount" + text, null, typeof(CompiledRegexRunner));
			base.GenerateInitTrackCount();
			return new CompiledRegexRunnerFactory(dynamicMethod, dynamicMethod2, dynamicMethod3);
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x0003FEF4 File Offset: 0x0003E0F4
		public DynamicMethod DefineDynamicMethod(string methname, Type returntype, Type hostType)
		{
			MethodAttributes methodAttributes = MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static;
			CallingConventions callingConventions = CallingConventions.Standard;
			DynamicMethod dynamicMethod = new DynamicMethod(methname, methodAttributes, callingConventions, returntype, RegexLWCGCompiler.s_paramTypes, hostType, false);
			this._ilg = dynamicMethod.GetILGenerator();
			return dynamicMethod;
		}

		// Token: 0x04000908 RID: 2312
		private static int s_regexCount = 0;

		// Token: 0x04000909 RID: 2313
		private static Type[] s_paramTypes = new Type[] { typeof(RegexRunner) };
	}
}
