using System;
using System.Runtime.Serialization.Diagnostics.Application;
using System.Security;
using System.Xml;

namespace System.Runtime.Serialization.Json
{
	// Token: 0x02000177 RID: 375
	internal sealed class JsonFormatReaderGenerator
	{
		// Token: 0x0600136F RID: 4975 RVA: 0x0004B4E4 File Offset: 0x000496E4
		[SecurityCritical]
		public JsonFormatReaderGenerator()
		{
			this.helper = new JsonFormatReaderGenerator.CriticalHelper();
		}

		// Token: 0x06001370 RID: 4976 RVA: 0x0004B4F8 File Offset: 0x000496F8
		[SecurityCritical]
		public JsonFormatClassReaderDelegate GenerateClassReader(ClassDataContract classContract)
		{
			JsonFormatClassReaderDelegate jsonFormatClassReaderDelegate;
			try
			{
				if (TD.DCJsonGenReaderStartIsEnabled())
				{
					TD.DCJsonGenReaderStart("Class", classContract.UnderlyingType.FullName);
				}
				jsonFormatClassReaderDelegate = this.helper.GenerateClassReader(classContract);
			}
			finally
			{
				if (TD.DCJsonGenReaderStopIsEnabled())
				{
					TD.DCJsonGenReaderStop();
				}
			}
			return jsonFormatClassReaderDelegate;
		}

		// Token: 0x06001371 RID: 4977 RVA: 0x0004B550 File Offset: 0x00049750
		[SecurityCritical]
		public JsonFormatCollectionReaderDelegate GenerateCollectionReader(CollectionDataContract collectionContract)
		{
			JsonFormatCollectionReaderDelegate jsonFormatCollectionReaderDelegate;
			try
			{
				if (TD.DCJsonGenReaderStartIsEnabled())
				{
					TD.DCJsonGenReaderStart("Collection", collectionContract.StableName.Name);
				}
				jsonFormatCollectionReaderDelegate = this.helper.GenerateCollectionReader(collectionContract);
			}
			finally
			{
				if (TD.DCJsonGenReaderStopIsEnabled())
				{
					TD.DCJsonGenReaderStop();
				}
			}
			return jsonFormatCollectionReaderDelegate;
		}

		// Token: 0x06001372 RID: 4978 RVA: 0x0004B5A8 File Offset: 0x000497A8
		[SecurityCritical]
		public JsonFormatGetOnlyCollectionReaderDelegate GenerateGetOnlyCollectionReader(CollectionDataContract collectionContract)
		{
			JsonFormatGetOnlyCollectionReaderDelegate jsonFormatGetOnlyCollectionReaderDelegate;
			try
			{
				if (TD.DCJsonGenReaderStartIsEnabled())
				{
					TD.DCJsonGenReaderStart("GetOnlyCollection", collectionContract.UnderlyingType.FullName);
				}
				jsonFormatGetOnlyCollectionReaderDelegate = this.helper.GenerateGetOnlyCollectionReader(collectionContract);
			}
			finally
			{
				if (TD.DCJsonGenReaderStopIsEnabled())
				{
					TD.DCJsonGenReaderStop();
				}
			}
			return jsonFormatGetOnlyCollectionReaderDelegate;
		}

		// Token: 0x040009B6 RID: 2486
		[SecurityCritical]
		private JsonFormatReaderGenerator.CriticalHelper helper;

		// Token: 0x02000178 RID: 376
		private class CriticalHelper
		{
			// Token: 0x06001373 RID: 4979 RVA: 0x0004B600 File Offset: 0x00049800
			internal JsonFormatClassReaderDelegate GenerateClassReader(ClassDataContract classContract)
			{
				return (XmlReaderDelegator xr, XmlObjectSerializerReadContextComplexJson ctx, XmlDictionaryString emptyDictionaryString, XmlDictionaryString[] memberNames) => new JsonFormatReaderInterpreter(classContract).ReadFromJson(xr, ctx, emptyDictionaryString, memberNames);
			}

			// Token: 0x06001374 RID: 4980 RVA: 0x0004B619 File Offset: 0x00049819
			internal JsonFormatCollectionReaderDelegate GenerateCollectionReader(CollectionDataContract collectionContract)
			{
				return (XmlReaderDelegator xr, XmlObjectSerializerReadContextComplexJson ctx, XmlDictionaryString emptyDS, XmlDictionaryString inm, CollectionDataContract cc) => new JsonFormatReaderInterpreter(collectionContract, false).ReadCollectionFromJson(xr, ctx, emptyDS, inm, cc);
			}

			// Token: 0x06001375 RID: 4981 RVA: 0x0004B632 File Offset: 0x00049832
			internal JsonFormatGetOnlyCollectionReaderDelegate GenerateGetOnlyCollectionReader(CollectionDataContract collectionContract)
			{
				return delegate(XmlReaderDelegator xr, XmlObjectSerializerReadContextComplexJson ctx, XmlDictionaryString emptyDS, XmlDictionaryString inm, CollectionDataContract cc)
				{
					new JsonFormatReaderInterpreter(collectionContract, true).ReadGetOnlyCollectionFromJson(xr, ctx, emptyDS, inm, cc);
				};
			}
		}
	}
}
