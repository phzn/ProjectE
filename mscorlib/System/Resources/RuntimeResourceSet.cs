using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace System.Resources
{
	// Token: 0x02000860 RID: 2144
	internal sealed class RuntimeResourceSet : ResourceSet, IEnumerable
	{
		// Token: 0x0600474D RID: 18253 RVA: 0x000E8300 File Offset: 0x000E6500
		internal RuntimeResourceSet(string fileName)
			: base(false)
		{
			this._resCache = new Dictionary<string, ResourceLocator>(FastResourceComparer.Default);
			Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			this._defaultReader = new ResourceReader(stream, this._resCache);
			this.Reader = this._defaultReader;
		}

		// Token: 0x0600474E RID: 18254 RVA: 0x000E834C File Offset: 0x000E654C
		internal RuntimeResourceSet(Stream stream)
			: base(false)
		{
			this._resCache = new Dictionary<string, ResourceLocator>(FastResourceComparer.Default);
			this._defaultReader = new ResourceReader(stream, this._resCache);
			this.Reader = this._defaultReader;
		}

		// Token: 0x0600474F RID: 18255 RVA: 0x000E8384 File Offset: 0x000E6584
		protected override void Dispose(bool disposing)
		{
			if (this.Reader == null)
			{
				return;
			}
			if (disposing)
			{
				IResourceReader reader = this.Reader;
				lock (reader)
				{
					this._resCache = null;
					if (this._defaultReader != null)
					{
						this._defaultReader.Close();
						this._defaultReader = null;
					}
					this._caseInsensitiveTable = null;
					base.Dispose(disposing);
					return;
				}
			}
			this._resCache = null;
			this._caseInsensitiveTable = null;
			this._defaultReader = null;
			base.Dispose(disposing);
		}

		// Token: 0x06004750 RID: 18256 RVA: 0x000E8418 File Offset: 0x000E6618
		public override IDictionaryEnumerator GetEnumerator()
		{
			return this.GetEnumeratorHelper();
		}

		// Token: 0x06004751 RID: 18257 RVA: 0x000E8418 File Offset: 0x000E6618
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumeratorHelper();
		}

		// Token: 0x06004752 RID: 18258 RVA: 0x000E8420 File Offset: 0x000E6620
		private IDictionaryEnumerator GetEnumeratorHelper()
		{
			IResourceReader reader = this.Reader;
			if (reader == null || this._resCache == null)
			{
				throw new ObjectDisposedException(null, "Cannot access a closed resource set.");
			}
			return reader.GetEnumerator();
		}

		// Token: 0x06004753 RID: 18259 RVA: 0x000E8444 File Offset: 0x000E6644
		public override string GetString(string key)
		{
			return (string)this.GetObject(key, false, true);
		}

		// Token: 0x06004754 RID: 18260 RVA: 0x000E8454 File Offset: 0x000E6654
		public override string GetString(string key, bool ignoreCase)
		{
			return (string)this.GetObject(key, ignoreCase, true);
		}

		// Token: 0x06004755 RID: 18261 RVA: 0x000E8464 File Offset: 0x000E6664
		public override object GetObject(string key)
		{
			return this.GetObject(key, false, false);
		}

		// Token: 0x06004756 RID: 18262 RVA: 0x000E846F File Offset: 0x000E666F
		public override object GetObject(string key, bool ignoreCase)
		{
			return this.GetObject(key, ignoreCase, false);
		}

		// Token: 0x06004757 RID: 18263 RVA: 0x000E847C File Offset: 0x000E667C
		private object GetObject(string key, bool ignoreCase, bool isString)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (this.Reader == null || this._resCache == null)
			{
				throw new ObjectDisposedException(null, "Cannot access a closed resource set.");
			}
			object obj = null;
			IResourceReader reader = this.Reader;
			object obj3;
			lock (reader)
			{
				if (this.Reader == null)
				{
					throw new ObjectDisposedException(null, "Cannot access a closed resource set.");
				}
				ResourceLocator resourceLocator;
				if (this._defaultReader != null)
				{
					int num = -1;
					if (this._resCache.TryGetValue(key, out resourceLocator))
					{
						obj = resourceLocator.Value;
						num = resourceLocator.DataPosition;
					}
					if (num == -1 && obj == null)
					{
						num = this._defaultReader.FindPosForResource(key);
					}
					if (num != -1 && obj == null)
					{
						ResourceTypeCode resourceTypeCode;
						if (isString)
						{
							obj = this._defaultReader.LoadString(num);
							resourceTypeCode = ResourceTypeCode.String;
						}
						else
						{
							obj = this._defaultReader.LoadObject(num, out resourceTypeCode);
						}
						resourceLocator = new ResourceLocator(num, ResourceLocator.CanCache(resourceTypeCode) ? obj : null);
						Dictionary<string, ResourceLocator> resCache = this._resCache;
						lock (resCache)
						{
							this._resCache[key] = resourceLocator;
						}
					}
					if (obj != null || !ignoreCase)
					{
						return obj;
					}
				}
				if (!this._haveReadFromReader)
				{
					if (ignoreCase && this._caseInsensitiveTable == null)
					{
						this._caseInsensitiveTable = new Dictionary<string, ResourceLocator>(StringComparer.OrdinalIgnoreCase);
					}
					if (this._defaultReader == null)
					{
						IDictionaryEnumerator enumerator = this.Reader.GetEnumerator();
						while (enumerator.MoveNext())
						{
							DictionaryEntry entry = enumerator.Entry;
							string text = (string)entry.Key;
							ResourceLocator resourceLocator2 = new ResourceLocator(-1, entry.Value);
							this._resCache.Add(text, resourceLocator2);
							if (ignoreCase)
							{
								this._caseInsensitiveTable.Add(text, resourceLocator2);
							}
						}
						if (!ignoreCase)
						{
							this.Reader.Close();
						}
					}
					else
					{
						ResourceReader.ResourceEnumerator enumeratorInternal = this._defaultReader.GetEnumeratorInternal();
						while (enumeratorInternal.MoveNext())
						{
							string text2 = (string)enumeratorInternal.Key;
							int dataPosition = enumeratorInternal.DataPosition;
							ResourceLocator resourceLocator3 = new ResourceLocator(dataPosition, null);
							this._caseInsensitiveTable.Add(text2, resourceLocator3);
						}
					}
					this._haveReadFromReader = true;
				}
				object obj2 = null;
				bool flag3 = false;
				bool flag4 = false;
				if (this._defaultReader != null && this._resCache.TryGetValue(key, out resourceLocator))
				{
					flag3 = true;
					obj2 = this.ResolveResourceLocator(resourceLocator, key, this._resCache, flag4);
				}
				if (!flag3 && ignoreCase && this._caseInsensitiveTable.TryGetValue(key, out resourceLocator))
				{
					flag4 = true;
					obj2 = this.ResolveResourceLocator(resourceLocator, key, this._resCache, flag4);
				}
				obj3 = obj2;
			}
			return obj3;
		}

		// Token: 0x06004758 RID: 18264 RVA: 0x000E8738 File Offset: 0x000E6938
		private object ResolveResourceLocator(ResourceLocator resLocation, string key, Dictionary<string, ResourceLocator> copyOfCache, bool keyInWrongCase)
		{
			object obj = resLocation.Value;
			if (obj == null)
			{
				IResourceReader reader = this.Reader;
				ResourceTypeCode resourceTypeCode;
				lock (reader)
				{
					obj = this._defaultReader.LoadObject(resLocation.DataPosition, out resourceTypeCode);
				}
				if (!keyInWrongCase && ResourceLocator.CanCache(resourceTypeCode))
				{
					resLocation.Value = obj;
					copyOfCache[key] = resLocation;
				}
			}
			return obj;
		}

		// Token: 0x04002DC2 RID: 11714
		internal const int Version = 2;

		// Token: 0x04002DC3 RID: 11715
		private Dictionary<string, ResourceLocator> _resCache;

		// Token: 0x04002DC4 RID: 11716
		private ResourceReader _defaultReader;

		// Token: 0x04002DC5 RID: 11717
		private Dictionary<string, ResourceLocator> _caseInsensitiveTable;

		// Token: 0x04002DC6 RID: 11718
		private bool _haveReadFromReader;
	}
}
