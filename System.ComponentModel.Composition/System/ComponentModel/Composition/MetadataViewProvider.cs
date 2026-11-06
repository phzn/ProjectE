using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Microsoft.Internal;

namespace System.ComponentModel.Composition
{
	// Token: 0x0200004F RID: 79
	internal static class MetadataViewProvider
	{
		// Token: 0x0600021E RID: 542 RVA: 0x000066EC File Offset: 0x000048EC
		public static TMetadataView GetMetadataView<TMetadataView>(IDictionary<string, object> metadata)
		{
			Assumes.NotNull<IDictionary<string, object>>(metadata);
			Type typeFromHandle = typeof(TMetadataView);
			if (typeFromHandle.IsAssignableFrom(typeof(IDictionary<string, object>)))
			{
				return (TMetadataView)((object)metadata);
			}
			Type type;
			if (typeFromHandle.IsInterface)
			{
				if (!typeFromHandle.IsAttributeDefined<MetadataViewImplementationAttribute>())
				{
					try
					{
						type = MetadataViewGenerator.GenerateView(typeFromHandle);
						goto IL_00C5;
					}
					catch (TypeLoadException ex)
					{
						throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Strings.NotSupportedInterfaceMetadataView, typeFromHandle.FullName), ex);
					}
				}
				type = typeFromHandle.GetFirstAttribute<MetadataViewImplementationAttribute>().ImplementationType;
				if (type == null)
				{
					throw new CompositionContractMismatchException(string.Format(CultureInfo.CurrentCulture, Strings.ContractMismatch_MetadataViewImplementationCanNotBeNull, typeFromHandle.FullName, type.FullName));
				}
				if (!typeFromHandle.IsAssignableFrom(type))
				{
					throw new CompositionContractMismatchException(string.Format(CultureInfo.CurrentCulture, Strings.ContractMismatch_MetadataViewImplementationDoesNotImplementViewInterface, typeFromHandle.FullName, type.FullName));
				}
			}
			else
			{
				type = typeFromHandle;
			}
			IL_00C5:
			TMetadataView tmetadataView;
			try
			{
				tmetadataView = (TMetadataView)((object)type.SafeCreateInstance(new object[] { metadata }));
			}
			catch (MissingMethodException ex2)
			{
				throw new CompositionContractMismatchException(string.Format(CultureInfo.CurrentCulture, Strings.CompositionException_MetadataViewInvalidConstructor, type.AssemblyQualifiedName), ex2);
			}
			catch (TargetInvocationException ex3)
			{
				if (typeFromHandle.IsInterface)
				{
					if (ex3.InnerException.GetType() == typeof(InvalidCastException))
					{
						throw new CompositionContractMismatchException(string.Format(CultureInfo.CurrentCulture, Strings.ContractMismatch_InvalidCastOnMetadataField, new object[]
						{
							ex3.InnerException.Data["MetadataViewType"],
							ex3.InnerException.Data["MetadataItemKey"],
							ex3.InnerException.Data["MetadataItemValue"],
							ex3.InnerException.Data["MetadataItemSourceType"],
							ex3.InnerException.Data["MetadataItemTargetType"]
						}), ex3);
					}
					if (ex3.InnerException.GetType() == typeof(NullReferenceException))
					{
						throw new CompositionContractMismatchException(string.Format(CultureInfo.CurrentCulture, Strings.ContractMismatch_NullReferenceOnMetadataField, ex3.InnerException.Data["MetadataViewType"], ex3.InnerException.Data["MetadataItemKey"], ex3.InnerException.Data["MetadataItemTargetType"]), ex3);
					}
				}
				throw;
			}
			return tmetadataView;
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00006988 File Offset: 0x00004B88
		public static bool IsViewTypeValid(Type metadataViewType)
		{
			Assumes.NotNull<Type>(metadataViewType);
			return ExportServices.IsDefaultMetadataViewType(metadataViewType) || metadataViewType.IsInterface || ExportServices.IsDictionaryConstructorViewType(metadataViewType);
		}
	}
}
