using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.ReflectionModel;
using System.Linq;

namespace System.ComponentModel.Composition.Primitives
{
	// Token: 0x0200009A RID: 154
	internal static class PrimitivesServices
	{
		// Token: 0x06000405 RID: 1029 RVA: 0x0000B673 File Offset: 0x00009873
		public static bool IsGeneric(this ComposablePartDefinition part)
		{
			return part.Metadata.GetValue<bool>("System.ComponentModel.Composition.IsGenericPart");
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0000B688 File Offset: 0x00009888
		public static ImportDefinition GetProductImportDefinition(this ImportDefinition import)
		{
			IPartCreatorImportDefinition partCreatorImportDefinition = import as IPartCreatorImportDefinition;
			if (partCreatorImportDefinition != null)
			{
				return partCreatorImportDefinition.ProductImportDefinition;
			}
			return import;
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0000B6A7 File Offset: 0x000098A7
		internal static IEnumerable<string> GetCandidateContractNames(this ImportDefinition import, ComposablePartDefinition part)
		{
			import = import.GetProductImportDefinition();
			string text = import.ContractName;
			string genericContractName = import.Metadata.GetValue<string>("System.ComponentModel.Composition.GenericContractName");
			int[] value = import.Metadata.GetValue<int[]>("System.ComponentModel.Composition.GenericImportParametersOrderMetadataName");
			if (value != null)
			{
				int value2 = part.Metadata.GetValue<int>("System.ComponentModel.Composition.GenericPartArity");
				if (value2 > 0)
				{
					text = GenericServices.GetGenericName(text, value, value2);
				}
			}
			yield return text;
			if (!string.IsNullOrEmpty(genericContractName))
			{
				yield return genericContractName;
			}
			yield break;
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0000B6BE File Offset: 0x000098BE
		internal static bool IsImportDependentOnPart(this ImportDefinition import, ComposablePartDefinition part, ExportDefinition export, bool expandGenerics)
		{
			import = import.GetProductImportDefinition();
			if (expandGenerics)
			{
				return part.GetExports(import).Any<Tuple<ComposablePartDefinition, ExportDefinition>>();
			}
			return PrimitivesServices.TranslateImport(import, part).IsConstraintSatisfiedBy(export);
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0000B6E8 File Offset: 0x000098E8
		private static ImportDefinition TranslateImport(ImportDefinition import, ComposablePartDefinition part)
		{
			ContractBasedImportDefinition contractBasedImportDefinition = import as ContractBasedImportDefinition;
			if (contractBasedImportDefinition == null)
			{
				return import;
			}
			int[] value = contractBasedImportDefinition.Metadata.GetValue<int[]>("System.ComponentModel.Composition.GenericImportParametersOrderMetadataName");
			if (value == null)
			{
				return import;
			}
			int value2 = part.Metadata.GetValue<int>("System.ComponentModel.Composition.GenericPartArity");
			if (value2 == 0)
			{
				return import;
			}
			string genericName = GenericServices.GetGenericName(contractBasedImportDefinition.ContractName, value, value2);
			string genericName2 = GenericServices.GetGenericName(contractBasedImportDefinition.RequiredTypeIdentity, value, value2);
			return new ContractBasedImportDefinition(genericName, genericName2, contractBasedImportDefinition.RequiredMetadata, contractBasedImportDefinition.Cardinality, contractBasedImportDefinition.IsRecomposable, false, contractBasedImportDefinition.RequiredCreationPolicy, contractBasedImportDefinition.Metadata);
		}
	}
}
