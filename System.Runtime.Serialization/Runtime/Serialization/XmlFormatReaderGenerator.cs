using System;
using System.Runtime.Serialization.Diagnostics.Application;
using System.Security;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x0200013A RID: 314
	internal sealed class XmlFormatReaderGenerator
	{
		// Token: 0x06000FA6 RID: 4006 RVA: 0x0003E961 File Offset: 0x0003CB61
		[SecurityCritical]
		public XmlFormatReaderGenerator()
		{
			this.helper = new XmlFormatReaderGenerator.CriticalHelper();
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x0003E974 File Offset: 0x0003CB74
		[SecurityCritical]
		public XmlFormatClassReaderDelegate GenerateClassReader(ClassDataContract classContract)
		{
			XmlFormatClassReaderDelegate xmlFormatClassReaderDelegate;
			try
			{
				if (TD.DCGenReaderStartIsEnabled())
				{
					TD.DCGenReaderStart("Class", classContract.UnderlyingType.FullName);
				}
				xmlFormatClassReaderDelegate = this.helper.GenerateClassReader(classContract);
			}
			finally
			{
				if (TD.DCGenReaderStopIsEnabled())
				{
					TD.DCGenReaderStop();
				}
			}
			return xmlFormatClassReaderDelegate;
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x0003E9CC File Offset: 0x0003CBCC
		[SecurityCritical]
		public XmlFormatCollectionReaderDelegate GenerateCollectionReader(CollectionDataContract collectionContract)
		{
			XmlFormatCollectionReaderDelegate xmlFormatCollectionReaderDelegate;
			try
			{
				if (TD.DCGenReaderStartIsEnabled())
				{
					TD.DCGenReaderStart("Collection", collectionContract.UnderlyingType.FullName);
				}
				xmlFormatCollectionReaderDelegate = this.helper.GenerateCollectionReader(collectionContract);
			}
			finally
			{
				if (TD.DCGenReaderStopIsEnabled())
				{
					TD.DCGenReaderStop();
				}
			}
			return xmlFormatCollectionReaderDelegate;
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x0003EA24 File Offset: 0x0003CC24
		[SecurityCritical]
		public XmlFormatGetOnlyCollectionReaderDelegate GenerateGetOnlyCollectionReader(CollectionDataContract collectionContract)
		{
			XmlFormatGetOnlyCollectionReaderDelegate xmlFormatGetOnlyCollectionReaderDelegate;
			try
			{
				if (TD.DCGenReaderStartIsEnabled())
				{
					TD.DCGenReaderStart("GetOnlyCollection", collectionContract.UnderlyingType.FullName);
				}
				xmlFormatGetOnlyCollectionReaderDelegate = this.helper.GenerateGetOnlyCollectionReader(collectionContract);
			}
			finally
			{
				if (TD.DCGenReaderStopIsEnabled())
				{
					TD.DCGenReaderStop();
				}
			}
			return xmlFormatGetOnlyCollectionReaderDelegate;
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x0003EA7C File Offset: 0x0003CC7C
		[SecuritySafeCritical]
		internal static object UnsafeGetUninitializedObject(int id)
		{
			return FormatterServices.GetUninitializedObject(DataContract.GetDataContractForInitialization(id).TypeForInitialization);
		}

		// Token: 0x040006EB RID: 1771
		[SecurityCritical]
		private XmlFormatReaderGenerator.CriticalHelper helper;

		// Token: 0x0200013B RID: 315
		private class CriticalHelper
		{
			// Token: 0x06000FAB RID: 4011 RVA: 0x0003EA8E File Offset: 0x0003CC8E
			internal XmlFormatClassReaderDelegate GenerateClassReader(ClassDataContract classContract)
			{
				return (XmlReaderDelegator xr, XmlObjectSerializerReadContext ctx, XmlDictionaryString[] memberNames, XmlDictionaryString[] memberNamespaces) => new XmlFormatReaderInterpreter(classContract).ReadFromXml(xr, ctx, memberNames, memberNamespaces);
			}

			// Token: 0x06000FAC RID: 4012 RVA: 0x0003EAA7 File Offset: 0x0003CCA7
			internal XmlFormatCollectionReaderDelegate GenerateCollectionReader(CollectionDataContract collectionContract)
			{
				return (XmlReaderDelegator xr, XmlObjectSerializerReadContext ctx, XmlDictionaryString inm, XmlDictionaryString ins, CollectionDataContract cc) => new XmlFormatReaderInterpreter(collectionContract, false).ReadCollectionFromXml(xr, ctx, inm, ins, cc);
			}

			// Token: 0x06000FAD RID: 4013 RVA: 0x0003EAC0 File Offset: 0x0003CCC0
			internal XmlFormatGetOnlyCollectionReaderDelegate GenerateGetOnlyCollectionReader(CollectionDataContract collectionContract)
			{
				return delegate(XmlReaderDelegator xr, XmlObjectSerializerReadContext ctx, XmlDictionaryString inm, XmlDictionaryString ins, CollectionDataContract cc)
				{
					new XmlFormatReaderInterpreter(collectionContract, true).ReadGetOnlyCollectionFromXml(xr, ctx, inm, ins, cc);
				};
			}
		}
	}
}
