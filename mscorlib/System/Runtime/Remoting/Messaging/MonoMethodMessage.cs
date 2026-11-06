using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x0200062F RID: 1583
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	internal class MonoMethodMessage : IMethodCallMessage, IMethodMessage, IMessage, IMethodReturnMessage, IInternalMessage
	{
		// Token: 0x06003BBE RID: 15294 RVA: 0x000D03D4 File Offset: 0x000CE5D4
		internal void InitMessage(RuntimeMethodInfo method, object[] out_args)
		{
			this.method = method;
			ParameterInfo[] parametersInternal = method.GetParametersInternal();
			int num = parametersInternal.Length;
			this.args = new object[num];
			this.arg_types = new byte[num];
			this.asyncResult = null;
			this.call_type = CallType.Sync;
			this.names = new string[num];
			for (int i = 0; i < num; i++)
			{
				this.names[i] = parametersInternal[i].Name;
			}
			bool flag = out_args != null;
			int num2 = 0;
			for (int j = 0; j < num; j++)
			{
				bool isOut = parametersInternal[j].IsOut;
				byte b;
				if (parametersInternal[j].ParameterType.IsByRef)
				{
					if (flag)
					{
						this.args[j] = out_args[num2++];
					}
					b = 2;
					if (!isOut)
					{
						b |= 1;
					}
				}
				else
				{
					b = 1;
					if (isOut)
					{
						b |= 4;
					}
				}
				this.arg_types[j] = b;
			}
		}

		// Token: 0x06003BBF RID: 15295 RVA: 0x000D04B5 File Offset: 0x000CE6B5
		public MonoMethodMessage(MethodBase method, object[] out_args)
		{
			if (method != null)
			{
				this.InitMessage((RuntimeMethodInfo)method, out_args);
				return;
			}
			this.args = null;
		}

		// Token: 0x06003BC0 RID: 15296 RVA: 0x000D04DC File Offset: 0x000CE6DC
		internal MonoMethodMessage(MethodInfo minfo, object[] in_args, object[] out_args)
		{
			this.InitMessage((RuntimeMethodInfo)minfo, out_args);
			int num = in_args.Length;
			for (int i = 0; i < num; i++)
			{
				this.args[i] = in_args[i];
			}
		}

		// Token: 0x06003BC1 RID: 15297 RVA: 0x000D0517 File Offset: 0x000CE717
		private static MethodInfo GetMethodInfo(Type type, string methodName)
		{
			MethodInfo methodInfo = type.GetMethod(methodName);
			if (methodInfo == null)
			{
				throw new ArgumentException(string.Format("Could not find '{0}' in {1}", methodName, type), "methodName");
			}
			return methodInfo;
		}

		// Token: 0x06003BC2 RID: 15298 RVA: 0x000D0540 File Offset: 0x000CE740
		public MonoMethodMessage(Type type, string methodName, object[] in_args)
			: this(MonoMethodMessage.GetMethodInfo(type, methodName), in_args, null)
		{
		}

		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x06003BC3 RID: 15299 RVA: 0x000D0551 File Offset: 0x000CE751
		public IDictionary Properties
		{
			get
			{
				if (this.properties == null)
				{
					this.properties = new MCMDictionary(this);
				}
				return this.properties;
			}
		}

		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x06003BC4 RID: 15300 RVA: 0x000D056D File Offset: 0x000CE76D
		public int ArgCount
		{
			get
			{
				if (this.CallType == CallType.EndInvoke)
				{
					return -1;
				}
				if (this.args == null)
				{
					return 0;
				}
				return this.args.Length;
			}
		}

		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x06003BC5 RID: 15301 RVA: 0x000D058C File Offset: 0x000CE78C
		public object[] Args
		{
			get
			{
				return this.args;
			}
		}

		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x06003BC6 RID: 15302 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public bool HasVarArgs
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000910 RID: 2320
		// (get) Token: 0x06003BC7 RID: 15303 RVA: 0x000D0594 File Offset: 0x000CE794
		// (set) Token: 0x06003BC8 RID: 15304 RVA: 0x000D059C File Offset: 0x000CE79C
		public LogicalCallContext LogicalCallContext
		{
			get
			{
				return this.ctx;
			}
			set
			{
				this.ctx = value;
			}
		}

		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x06003BC9 RID: 15305 RVA: 0x000D05A5 File Offset: 0x000CE7A5
		public MethodBase MethodBase
		{
			get
			{
				return this.method;
			}
		}

		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x06003BCA RID: 15306 RVA: 0x000D05AD File Offset: 0x000CE7AD
		public string MethodName
		{
			get
			{
				if (null == this.method)
				{
					return string.Empty;
				}
				return this.method.Name;
			}
		}

		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x06003BCB RID: 15307 RVA: 0x000D05D0 File Offset: 0x000CE7D0
		public object MethodSignature
		{
			get
			{
				if (this.methodSignature == null)
				{
					ParameterInfo[] parameters = this.method.GetParameters();
					this.methodSignature = new Type[parameters.Length];
					for (int i = 0; i < parameters.Length; i++)
					{
						this.methodSignature[i] = parameters[i].ParameterType;
					}
				}
				return this.methodSignature;
			}
		}

		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x06003BCC RID: 15308 RVA: 0x000D0623 File Offset: 0x000CE823
		public string TypeName
		{
			get
			{
				if (null == this.method)
				{
					return string.Empty;
				}
				return this.method.DeclaringType.AssemblyQualifiedName;
			}
		}

		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x06003BCD RID: 15309 RVA: 0x000D0649 File Offset: 0x000CE849
		// (set) Token: 0x06003BCE RID: 15310 RVA: 0x000D0651 File Offset: 0x000CE851
		public string Uri
		{
			get
			{
				return this.uri;
			}
			set
			{
				this.uri = value;
			}
		}

		// Token: 0x06003BCF RID: 15311 RVA: 0x000D065A File Offset: 0x000CE85A
		public object GetArg(int arg_num)
		{
			if (this.args == null)
			{
				return null;
			}
			return this.args[arg_num];
		}

		// Token: 0x06003BD0 RID: 15312 RVA: 0x000D066E File Offset: 0x000CE86E
		public string GetArgName(int arg_num)
		{
			if (this.args == null)
			{
				return string.Empty;
			}
			return this.names[arg_num];
		}

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x06003BD1 RID: 15313 RVA: 0x000D0688 File Offset: 0x000CE888
		public int InArgCount
		{
			get
			{
				if (this.CallType == CallType.EndInvoke)
				{
					return -1;
				}
				if (this.args == null)
				{
					return 0;
				}
				int num = 0;
				byte[] array = this.arg_types;
				for (int i = 0; i < array.Length; i++)
				{
					if ((array[i] & 1) != 0)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x06003BD2 RID: 15314 RVA: 0x000D06D0 File Offset: 0x000CE8D0
		public object[] InArgs
		{
			get
			{
				object[] array = new object[this.InArgCount];
				int num2;
				int num = (num2 = 0);
				byte[] array2 = this.arg_types;
				for (int i = 0; i < array2.Length; i++)
				{
					if ((array2[i] & 1) != 0)
					{
						array[num++] = this.args[num2];
					}
					num2++;
				}
				return array;
			}
		}

		// Token: 0x06003BD3 RID: 15315 RVA: 0x000D0724 File Offset: 0x000CE924
		public object GetInArg(int arg_num)
		{
			int num = 0;
			int num2 = 0;
			byte[] array = this.arg_types;
			for (int i = 0; i < array.Length; i++)
			{
				if ((array[i] & 1) != 0 && num2++ == arg_num)
				{
					return this.args[num];
				}
				num++;
			}
			return null;
		}

		// Token: 0x06003BD4 RID: 15316 RVA: 0x000D0768 File Offset: 0x000CE968
		public string GetInArgName(int arg_num)
		{
			int num = 0;
			int num2 = 0;
			byte[] array = this.arg_types;
			for (int i = 0; i < array.Length; i++)
			{
				if ((array[i] & 1) != 0 && num2++ == arg_num)
				{
					return this.names[num];
				}
				num++;
			}
			return null;
		}

		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x06003BD5 RID: 15317 RVA: 0x000D07AB File Offset: 0x000CE9AB
		public Exception Exception
		{
			get
			{
				return this.exc;
			}
		}

		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x06003BD6 RID: 15318 RVA: 0x000D07B4 File Offset: 0x000CE9B4
		public int OutArgCount
		{
			get
			{
				if (this.args == null)
				{
					return 0;
				}
				int num = 0;
				byte[] array = this.arg_types;
				for (int i = 0; i < array.Length; i++)
				{
					if ((array[i] & 2) != 0)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x06003BD7 RID: 15319 RVA: 0x000D07F0 File Offset: 0x000CE9F0
		public object[] OutArgs
		{
			get
			{
				if (this.args == null)
				{
					return null;
				}
				object[] array = new object[this.OutArgCount];
				int num2;
				int num = (num2 = 0);
				byte[] array2 = this.arg_types;
				for (int i = 0; i < array2.Length; i++)
				{
					if ((array2[i] & 2) != 0)
					{
						array[num++] = this.args[num2];
					}
					num2++;
				}
				return array;
			}
		}

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x06003BD8 RID: 15320 RVA: 0x000D084C File Offset: 0x000CEA4C
		public object ReturnValue
		{
			get
			{
				return this.rval;
			}
		}

		// Token: 0x06003BD9 RID: 15321 RVA: 0x000D0854 File Offset: 0x000CEA54
		public object GetOutArg(int arg_num)
		{
			int num = 0;
			int num2 = 0;
			byte[] array = this.arg_types;
			for (int i = 0; i < array.Length; i++)
			{
				if ((array[i] & 2) != 0 && num2++ == arg_num)
				{
					return this.args[num];
				}
				num++;
			}
			return null;
		}

		// Token: 0x06003BDA RID: 15322 RVA: 0x000D0898 File Offset: 0x000CEA98
		public string GetOutArgName(int arg_num)
		{
			int num = 0;
			int num2 = 0;
			byte[] array = this.arg_types;
			for (int i = 0; i < array.Length; i++)
			{
				if ((array[i] & 2) != 0 && num2++ == arg_num)
				{
					return this.names[num];
				}
				num++;
			}
			return null;
		}

		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x06003BDB RID: 15323 RVA: 0x000D08DB File Offset: 0x000CEADB
		// (set) Token: 0x06003BDC RID: 15324 RVA: 0x000D08E3 File Offset: 0x000CEAE3
		Identity IInternalMessage.TargetIdentity
		{
			get
			{
				return this.identity;
			}
			set
			{
				this.identity = value;
			}
		}

		// Token: 0x06003BDD RID: 15325 RVA: 0x000D08EC File Offset: 0x000CEAEC
		bool IInternalMessage.HasProperties()
		{
			return this.properties != null;
		}

		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x06003BDE RID: 15326 RVA: 0x000D08F7 File Offset: 0x000CEAF7
		public bool IsAsync
		{
			get
			{
				return this.asyncResult != null;
			}
		}

		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x06003BDF RID: 15327 RVA: 0x000D0902 File Offset: 0x000CEB02
		public AsyncResult AsyncResult
		{
			get
			{
				return this.asyncResult;
			}
		}

		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x06003BE0 RID: 15328 RVA: 0x000D090A File Offset: 0x000CEB0A
		internal CallType CallType
		{
			get
			{
				if (this.call_type == CallType.Sync && RemotingServices.IsOneWay(this.method))
				{
					this.call_type = CallType.OneWay;
				}
				return this.call_type;
			}
		}

		// Token: 0x06003BE1 RID: 15329 RVA: 0x000D0930 File Offset: 0x000CEB30
		public bool NeedsOutProcessing(out int outCount)
		{
			bool flag = false;
			outCount = 0;
			foreach (byte b in this.arg_types)
			{
				if ((b & 2) != 0)
				{
					outCount++;
				}
				else if ((b & 4) != 0)
				{
					flag = true;
				}
			}
			return outCount > 0 || flag;
		}

		// Token: 0x040026C4 RID: 9924
		private RuntimeMethodInfo method;

		// Token: 0x040026C5 RID: 9925
		private object[] args;

		// Token: 0x040026C6 RID: 9926
		private string[] names;

		// Token: 0x040026C7 RID: 9927
		private byte[] arg_types;

		// Token: 0x040026C8 RID: 9928
		public LogicalCallContext ctx;

		// Token: 0x040026C9 RID: 9929
		public object rval;

		// Token: 0x040026CA RID: 9930
		public Exception exc;

		// Token: 0x040026CB RID: 9931
		private AsyncResult asyncResult;

		// Token: 0x040026CC RID: 9932
		private CallType call_type;

		// Token: 0x040026CD RID: 9933
		private string uri;

		// Token: 0x040026CE RID: 9934
		private MCMDictionary properties;

		// Token: 0x040026CF RID: 9935
		private Identity identity;

		// Token: 0x040026D0 RID: 9936
		private Type[] methodSignature;
	}
}
