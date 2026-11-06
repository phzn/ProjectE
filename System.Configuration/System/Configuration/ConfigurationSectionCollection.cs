using System;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Unity;

namespace System.Configuration
{
	/// <summary>Represents a collection of related sections within a configuration file.</summary>
	// Token: 0x02000031 RID: 49
	[Serializable]
	public sealed class ConfigurationSectionCollection : NameObjectCollectionBase
	{
		// Token: 0x060001B5 RID: 437 RVA: 0x00006CF7 File Offset: 0x00004EF7
		internal ConfigurationSectionCollection(Configuration config, SectionGroupInfo group)
			: base(StringComparer.Ordinal)
		{
			this.config = config;
			this.group = group;
		}

		/// <summary>Gets the keys to all <see cref="T:System.Configuration.ConfigurationSection" /> objects contained in this <see cref="T:System.Configuration.ConfigurationSectionCollection" /> object.</summary>
		/// <returns>A <see cref="T:System.Collections.Specialized.NameObjectCollectionBase.KeysCollection" /> object that contains the keys of all sections in this collection.</returns>
		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x00006D12 File Offset: 0x00004F12
		public override NameObjectCollectionBase.KeysCollection Keys
		{
			get
			{
				return this.group.Sections.Keys;
			}
		}

		/// <summary>Gets the number of sections in this <see cref="T:System.Configuration.ConfigurationSectionCollection" /> object.</summary>
		/// <returns>An integer that represents the number of sections in the collection.</returns>
		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x00006D24 File Offset: 0x00004F24
		public override int Count
		{
			get
			{
				return this.group.Sections.Count;
			}
		}

		/// <summary>Gets the specified <see cref="T:System.Configuration.ConfigurationSection" /> object.</summary>
		/// <param name="name">The name of the <see cref="T:System.Configuration.ConfigurationSection" /> object to be returned.</param>
		/// <returns>The <see cref="T:System.Configuration.ConfigurationSection" /> object with the specified name.</returns>
		// Token: 0x17000082 RID: 130
		public ConfigurationSection this[string name]
		{
			get
			{
				ConfigurationSection configurationSection = base.BaseGet(name) as ConfigurationSection;
				if (configurationSection == null)
				{
					SectionInfo sectionInfo = this.group.Sections[name] as SectionInfo;
					if (sectionInfo == null)
					{
						return null;
					}
					configurationSection = this.config.GetSectionInstance(sectionInfo, true);
					if (configurationSection == null)
					{
						return null;
					}
					object obj = ConfigurationSectionCollection.lockObject;
					lock (obj)
					{
						base.BaseSet(name, configurationSection);
					}
				}
				return configurationSection;
			}
		}

		/// <summary>Gets the specified <see cref="T:System.Configuration.ConfigurationSection" /> object.</summary>
		/// <param name="index">The index of the <see cref="T:System.Configuration.ConfigurationSection" /> object to be returned.</param>
		/// <returns>The <see cref="T:System.Configuration.ConfigurationSection" /> object at the specified index.</returns>
		// Token: 0x17000083 RID: 131
		public ConfigurationSection this[int index]
		{
			get
			{
				return this[this.GetKey(index)];
			}
		}

		/// <summary>Adds a <see cref="T:System.Configuration.ConfigurationSection" /> object to the <see cref="T:System.Configuration.ConfigurationSectionCollection" /> object.</summary>
		/// <param name="name">The name of the section to be added.</param>
		/// <param name="section">The section to be added.</param>
		// Token: 0x060001BA RID: 442 RVA: 0x00006DCB File Offset: 0x00004FCB
		public void Add(string name, ConfigurationSection section)
		{
			this.config.CreateSection(this.group, name, section);
		}

		/// <summary>Clears this <see cref="T:System.Configuration.ConfigurationSectionCollection" /> object.</summary>
		// Token: 0x060001BB RID: 443 RVA: 0x00006DE0 File Offset: 0x00004FE0
		public void Clear()
		{
			if (this.group.Sections != null)
			{
				foreach (object obj in this.group.Sections)
				{
					ConfigInfo configInfo = (ConfigInfo)obj;
					this.config.RemoveConfigInfo(configInfo);
				}
			}
		}

		/// <summary>Copies this <see cref="T:System.Configuration.ConfigurationSectionCollection" /> object to an array.</summary>
		/// <param name="array">The array to copy the <see cref="T:System.Configuration.ConfigurationSectionCollection" /> object to.</param>
		/// <param name="index">The index location at which to begin copying.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The length of <paramref name="array" /> is less than the value of <see cref="P:System.Configuration.ConfigurationSectionCollection.Count" /> plus <paramref name="index" />.</exception>
		// Token: 0x060001BC RID: 444 RVA: 0x00006E50 File Offset: 0x00005050
		public void CopyTo(ConfigurationSection[] array, int index)
		{
			for (int i = 0; i < this.group.Sections.Count; i++)
			{
				array[i + index] = this[i];
			}
		}

		/// <summary>Gets the specified <see cref="T:System.Configuration.ConfigurationSection" /> object contained in this <see cref="T:System.Configuration.ConfigurationSectionCollection" /> object.</summary>
		/// <param name="index">The index of the <see cref="T:System.Configuration.ConfigurationSection" /> object to be returned.</param>
		/// <returns>The <see cref="T:System.Configuration.ConfigurationSection" /> object at the specified index.</returns>
		// Token: 0x060001BD RID: 445 RVA: 0x00006E84 File Offset: 0x00005084
		public ConfigurationSection Get(int index)
		{
			return this[index];
		}

		/// <summary>Gets the specified <see cref="T:System.Configuration.ConfigurationSection" /> object contained in this <see cref="T:System.Configuration.ConfigurationSectionCollection" /> object.</summary>
		/// <param name="name">The name of the <see cref="T:System.Configuration.ConfigurationSection" /> object to be returned.</param>
		/// <returns>The <see cref="T:System.Configuration.ConfigurationSection" /> object with the specified name.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> is null or an empty string ("").</exception>
		// Token: 0x060001BE RID: 446 RVA: 0x00006E8D File Offset: 0x0000508D
		public ConfigurationSection Get(string name)
		{
			return this[name];
		}

		/// <summary>Gets an enumerator that can iterate through this <see cref="T:System.Configuration.ConfigurationSectionCollection" /> object.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through this <see cref="T:System.Configuration.ConfigurationSectionCollection" /> object.</returns>
		// Token: 0x060001BF RID: 447 RVA: 0x00006E96 File Offset: 0x00005096
		public override IEnumerator GetEnumerator()
		{
			foreach (object obj in this.group.Sections.AllKeys)
			{
				string text = (string)obj;
				yield return this[text];
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		/// <summary>Gets the key of the specified <see cref="T:System.Configuration.ConfigurationSection" /> object contained in this <see cref="T:System.Configuration.ConfigurationSectionCollection" /> object.</summary>
		/// <param name="index">The index of the <see cref="T:System.Configuration.ConfigurationSection" /> object whose key is to be returned.</param>
		/// <returns>The key of the <see cref="T:System.Configuration.ConfigurationSection" /> object at the specified index.</returns>
		// Token: 0x060001C0 RID: 448 RVA: 0x00006EA5 File Offset: 0x000050A5
		public string GetKey(int index)
		{
			return this.group.Sections.GetKey(index);
		}

		/// <summary>Removes the specified <see cref="T:System.Configuration.ConfigurationSection" /> object from this <see cref="T:System.Configuration.ConfigurationSectionCollection" /> object.</summary>
		/// <param name="name">The name of the section to be removed.</param>
		// Token: 0x060001C1 RID: 449 RVA: 0x00006EB8 File Offset: 0x000050B8
		public void Remove(string name)
		{
			SectionInfo sectionInfo = this.group.Sections[name] as SectionInfo;
			if (sectionInfo != null)
			{
				this.config.RemoveConfigInfo(sectionInfo);
			}
		}

		/// <summary>Removes the specified <see cref="T:System.Configuration.ConfigurationSection" /> object from this <see cref="T:System.Configuration.ConfigurationSectionCollection" /> object.</summary>
		/// <param name="index">The index of the section to be removed.</param>
		// Token: 0x060001C2 RID: 450 RVA: 0x00006EEC File Offset: 0x000050EC
		public void RemoveAt(int index)
		{
			SectionInfo sectionInfo = this.group.Sections[index] as SectionInfo;
			this.config.RemoveConfigInfo(sectionInfo);
		}

		/// <summary>Used by the system during serialization.</summary>
		/// <param name="info">The applicable <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object.</param>
		/// <param name="context">The applicable <see cref="T:System.Runtime.Serialization.StreamingContext" /> object.</param>
		// Token: 0x060001C3 RID: 451 RVA: 0x0000371B File Offset: 0x0000191B
		[MonoTODO]
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00003518 File Offset: 0x00001718
		internal ConfigurationSectionCollection()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x040000C0 RID: 192
		private SectionGroupInfo group;

		// Token: 0x040000C1 RID: 193
		private Configuration config;

		// Token: 0x040000C2 RID: 194
		private static readonly object lockObject = new object();
	}
}
