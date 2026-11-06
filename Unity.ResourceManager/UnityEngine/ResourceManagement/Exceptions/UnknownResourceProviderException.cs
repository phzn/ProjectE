using System;
using System.Runtime.Serialization;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace UnityEngine.ResourceManagement.Exceptions
{
	// Token: 0x0200000F RID: 15
	public class UnknownResourceProviderException : ResourceManagerException
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00004054 File Offset: 0x00002254
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x0000405C File Offset: 0x0000225C
		public IResourceLocation Location { get; private set; }

		// Token: 0x060000A1 RID: 161 RVA: 0x00004065 File Offset: 0x00002265
		public UnknownResourceProviderException(IResourceLocation location)
		{
			this.Location = location;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004074 File Offset: 0x00002274
		public UnknownResourceProviderException()
		{
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0000407C File Offset: 0x0000227C
		public UnknownResourceProviderException(string message)
			: base(message)
		{
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00004085 File Offset: 0x00002285
		public UnknownResourceProviderException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x0000408F File Offset: 0x0000228F
		protected UnknownResourceProviderException(SerializationInfo message, StreamingContext context)
			: base(message, context)
		{
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x0000409C File Offset: 0x0000229C
		public override string Message
		{
			get
			{
				string[] array = new string[5];
				array[0] = base.Message;
				array[1] = ", ProviderId=";
				array[2] = this.Location.ProviderId;
				array[3] = ", Location=";
				int num = 4;
				IResourceLocation location = this.Location;
				array[num] = ((location != null) ? location.ToString() : null);
				return string.Concat(array);
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000040F0 File Offset: 0x000022F0
		public override string ToString()
		{
			return this.Message;
		}
	}
}
