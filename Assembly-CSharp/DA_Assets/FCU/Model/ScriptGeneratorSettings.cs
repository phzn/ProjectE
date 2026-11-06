using System;
using System.IO;
using DA_Assets.DAI;
using UnityEngine;

namespace DA_Assets.FCU.Model
{
	// Token: 0x02000385 RID: 901
	[Serializable]
	public class ScriptGeneratorSettings : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x0600178D RID: 6029 RVA: 0x000974A6 File Offset: 0x000956A6
		// (set) Token: 0x0600178E RID: 6030 RVA: 0x000974AE File Offset: 0x000956AE
		public FieldSerializationMode SerializationMode
		{
			get
			{
				return this.serializationMode;
			}
			set
			{
				base.SetValue<FieldSerializationMode>(ref this.serializationMode, value);
			}
		}

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x0600178F RID: 6031 RVA: 0x000974BD File Offset: 0x000956BD
		// (set) Token: 0x06001790 RID: 6032 RVA: 0x000974C5 File Offset: 0x000956C5
		public string Namespace
		{
			get
			{
				return this.@namespace;
			}
			set
			{
				base.SetValue<string>(ref this.@namespace, value);
			}
		}

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x06001791 RID: 6033 RVA: 0x000974D4 File Offset: 0x000956D4
		// (set) Token: 0x06001792 RID: 6034 RVA: 0x000974DC File Offset: 0x000956DC
		public string BaseClass
		{
			get
			{
				return this.baseClass;
			}
			set
			{
				base.SetValue<string>(ref this.baseClass, value);
			}
		}

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x06001793 RID: 6035 RVA: 0x000974EB File Offset: 0x000956EB
		// (set) Token: 0x06001794 RID: 6036 RVA: 0x000974F3 File Offset: 0x000956F3
		public string OutputPath
		{
			get
			{
				return this.outputPath;
			}
			set
			{
				base.SetValue<string>(ref this.outputPath, value);
			}
		}

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x06001795 RID: 6037 RVA: 0x00097502 File Offset: 0x00095702
		// (set) Token: 0x06001796 RID: 6038 RVA: 0x0009750A File Offset: 0x0009570A
		public int FieldNameMaxLenght
		{
			get
			{
				return this.fieldNameMaxLenght;
			}
			set
			{
				base.SetValue<int>(ref this.fieldNameMaxLenght, value);
			}
		}

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x06001797 RID: 6039 RVA: 0x00097519 File Offset: 0x00095719
		// (set) Token: 0x06001798 RID: 6040 RVA: 0x00097521 File Offset: 0x00095721
		public int MethodNameMaxLenght
		{
			get
			{
				return this.methodNameMaxLenght;
			}
			set
			{
				base.SetValue<int>(ref this.methodNameMaxLenght, value);
			}
		}

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x06001799 RID: 6041 RVA: 0x00097530 File Offset: 0x00095730
		// (set) Token: 0x0600179A RID: 6042 RVA: 0x00097538 File Offset: 0x00095738
		public int ClassNameMaxLenght
		{
			get
			{
				return this.classNameMaxLenght;
			}
			set
			{
				base.SetValue<int>(ref this.classNameMaxLenght, value);
			}
		}

		// Token: 0x0400190E RID: 6414
		[SerializeField]
		private FieldSerializationMode serializationMode;

		// Token: 0x0400190F RID: 6415
		[SerializeField]
		private string @namespace = "MyNamespace";

		// Token: 0x04001910 RID: 6416
		[SerializeField]
		private string baseClass = "MonoBehaviour";

		// Token: 0x04001911 RID: 6417
		[SerializeField]
		private string outputPath = Path.Combine("Assets", "GeneratedScripts");

		// Token: 0x04001912 RID: 6418
		[SerializeField]
		private int fieldNameMaxLenght = 16;

		// Token: 0x04001913 RID: 6419
		[SerializeField]
		private int methodNameMaxLenght = 16;

		// Token: 0x04001914 RID: 6420
		[SerializeField]
		private int classNameMaxLenght = 16;
	}
}
