using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace System.Net.Http.Headers
{
	/// <summary>A collection of headers and their values as defined in RFC 2616.</summary>
	// Token: 0x02000045 RID: 69
	public abstract class HttpHeaders : IEnumerable<KeyValuePair<string, IEnumerable<string>>>, IEnumerable
	{
		// Token: 0x06000264 RID: 612 RVA: 0x0000904C File Offset: 0x0000724C
		static HttpHeaders()
		{
			HeaderInfo[] array = new HeaderInfo[]
			{
				HeaderInfo.CreateMulti<MediaTypeWithQualityHeaderValue>("Accept", new TryParseListDelegate<MediaTypeWithQualityHeaderValue>(MediaTypeWithQualityHeaderValue.TryParse), HttpHeaderKind.Request, 1, ", "),
				HeaderInfo.CreateMulti<StringWithQualityHeaderValue>("Accept-Charset", new TryParseListDelegate<StringWithQualityHeaderValue>(StringWithQualityHeaderValue.TryParse), HttpHeaderKind.Request, 1, ", "),
				HeaderInfo.CreateMulti<StringWithQualityHeaderValue>("Accept-Encoding", new TryParseListDelegate<StringWithQualityHeaderValue>(StringWithQualityHeaderValue.TryParse), HttpHeaderKind.Request, 1, ", "),
				HeaderInfo.CreateMulti<StringWithQualityHeaderValue>("Accept-Language", new TryParseListDelegate<StringWithQualityHeaderValue>(StringWithQualityHeaderValue.TryParse), HttpHeaderKind.Request, 1, ", "),
				HeaderInfo.CreateMulti<string>("Accept-Ranges", new TryParseListDelegate<string>(CollectionParser.TryParse), HttpHeaderKind.Response, 1, ", "),
				HeaderInfo.CreateSingle<TimeSpan>("Age", new TryParseDelegate<TimeSpan>(Parser.TimeSpanSeconds.TryParse), HttpHeaderKind.Response, null),
				HeaderInfo.CreateMulti<string>("Allow", new TryParseListDelegate<string>(CollectionParser.TryParse), HttpHeaderKind.Content, 0, ", "),
				HeaderInfo.CreateSingle<AuthenticationHeaderValue>("Authorization", new TryParseDelegate<AuthenticationHeaderValue>(AuthenticationHeaderValue.TryParse), HttpHeaderKind.Request, null),
				HeaderInfo.CreateSingle<CacheControlHeaderValue>("Cache-Control", new TryParseDelegate<CacheControlHeaderValue>(CacheControlHeaderValue.TryParse), HttpHeaderKind.Request | HttpHeaderKind.Response, null),
				HeaderInfo.CreateMulti<string>("Connection", new TryParseListDelegate<string>(CollectionParser.TryParse), HttpHeaderKind.Request | HttpHeaderKind.Response, 1, ", "),
				HeaderInfo.CreateSingle<ContentDispositionHeaderValue>("Content-Disposition", new TryParseDelegate<ContentDispositionHeaderValue>(ContentDispositionHeaderValue.TryParse), HttpHeaderKind.Content, null),
				HeaderInfo.CreateMulti<string>("Content-Encoding", new TryParseListDelegate<string>(CollectionParser.TryParse), HttpHeaderKind.Content, 1, ", "),
				HeaderInfo.CreateMulti<string>("Content-Language", new TryParseListDelegate<string>(CollectionParser.TryParse), HttpHeaderKind.Content, 1, ", "),
				HeaderInfo.CreateSingle<long>("Content-Length", new TryParseDelegate<long>(Parser.Long.TryParse), HttpHeaderKind.Content, null),
				HeaderInfo.CreateSingle<Uri>("Content-Location", new TryParseDelegate<Uri>(Parser.Uri.TryParse), HttpHeaderKind.Content, null),
				HeaderInfo.CreateSingle<byte[]>("Content-MD5", new TryParseDelegate<byte[]>(Parser.MD5.TryParse), HttpHeaderKind.Content, null),
				HeaderInfo.CreateSingle<ContentRangeHeaderValue>("Content-Range", new TryParseDelegate<ContentRangeHeaderValue>(ContentRangeHeaderValue.TryParse), HttpHeaderKind.Content, null),
				HeaderInfo.CreateSingle<MediaTypeHeaderValue>("Content-Type", new TryParseDelegate<MediaTypeHeaderValue>(MediaTypeHeaderValue.TryParse), HttpHeaderKind.Content, null),
				HeaderInfo.CreateSingle<DateTimeOffset>("Date", new TryParseDelegate<DateTimeOffset>(Parser.DateTime.TryParse), HttpHeaderKind.Request | HttpHeaderKind.Response, Parser.DateTime.ToString),
				HeaderInfo.CreateSingle<EntityTagHeaderValue>("ETag", new TryParseDelegate<EntityTagHeaderValue>(EntityTagHeaderValue.TryParse), HttpHeaderKind.Response, null),
				HeaderInfo.CreateMulti<NameValueWithParametersHeaderValue>("Expect", new TryParseListDelegate<NameValueWithParametersHeaderValue>(NameValueWithParametersHeaderValue.TryParse), HttpHeaderKind.Request, 1, ", "),
				HeaderInfo.CreateSingle<DateTimeOffset>("Expires", new TryParseDelegate<DateTimeOffset>(Parser.DateTime.TryParse), HttpHeaderKind.Content, Parser.DateTime.ToString),
				HeaderInfo.CreateSingle<string>("From", new TryParseDelegate<string>(Parser.EmailAddress.TryParse), HttpHeaderKind.Request, null),
				HeaderInfo.CreateSingle<string>("Host", new TryParseDelegate<string>(Parser.Host.TryParse), HttpHeaderKind.Request, null),
				HeaderInfo.CreateMulti<EntityTagHeaderValue>("If-Match", new TryParseListDelegate<EntityTagHeaderValue>(EntityTagHeaderValue.TryParse), HttpHeaderKind.Request, 1, ", "),
				HeaderInfo.CreateSingle<DateTimeOffset>("If-Modified-Since", new TryParseDelegate<DateTimeOffset>(Parser.DateTime.TryParse), HttpHeaderKind.Request, Parser.DateTime.ToString),
				HeaderInfo.CreateMulti<EntityTagHeaderValue>("If-None-Match", new TryParseListDelegate<EntityTagHeaderValue>(EntityTagHeaderValue.TryParse), HttpHeaderKind.Request, 1, ", "),
				HeaderInfo.CreateSingle<RangeConditionHeaderValue>("If-Range", new TryParseDelegate<RangeConditionHeaderValue>(RangeConditionHeaderValue.TryParse), HttpHeaderKind.Request, null),
				HeaderInfo.CreateSingle<DateTimeOffset>("If-Unmodified-Since", new TryParseDelegate<DateTimeOffset>(Parser.DateTime.TryParse), HttpHeaderKind.Request, Parser.DateTime.ToString),
				HeaderInfo.CreateSingle<DateTimeOffset>("Last-Modified", new TryParseDelegate<DateTimeOffset>(Parser.DateTime.TryParse), HttpHeaderKind.Content, Parser.DateTime.ToString),
				HeaderInfo.CreateSingle<Uri>("Location", new TryParseDelegate<Uri>(Parser.Uri.TryParse), HttpHeaderKind.Response, null),
				HeaderInfo.CreateSingle<int>("Max-Forwards", new TryParseDelegate<int>(Parser.Int.TryParse), HttpHeaderKind.Request, null),
				HeaderInfo.CreateMulti<NameValueHeaderValue>("Pragma", new TryParseListDelegate<NameValueHeaderValue>(NameValueHeaderValue.TryParsePragma), HttpHeaderKind.Request | HttpHeaderKind.Response, 1, ", "),
				HeaderInfo.CreateMulti<AuthenticationHeaderValue>("Proxy-Authenticate", new TryParseListDelegate<AuthenticationHeaderValue>(AuthenticationHeaderValue.TryParse), HttpHeaderKind.Response, 1, ", "),
				HeaderInfo.CreateSingle<AuthenticationHeaderValue>("Proxy-Authorization", new TryParseDelegate<AuthenticationHeaderValue>(AuthenticationHeaderValue.TryParse), HttpHeaderKind.Request, null),
				HeaderInfo.CreateSingle<RangeHeaderValue>("Range", new TryParseDelegate<RangeHeaderValue>(RangeHeaderValue.TryParse), HttpHeaderKind.Request, null),
				HeaderInfo.CreateSingle<Uri>("Referer", new TryParseDelegate<Uri>(Parser.Uri.TryParse), HttpHeaderKind.Request, null),
				HeaderInfo.CreateSingle<RetryConditionHeaderValue>("Retry-After", new TryParseDelegate<RetryConditionHeaderValue>(RetryConditionHeaderValue.TryParse), HttpHeaderKind.Response, null),
				HeaderInfo.CreateMulti<ProductInfoHeaderValue>("Server", new TryParseListDelegate<ProductInfoHeaderValue>(ProductInfoHeaderValue.TryParse), HttpHeaderKind.Response, 1, " "),
				HeaderInfo.CreateMulti<TransferCodingWithQualityHeaderValue>("TE", new TryParseListDelegate<TransferCodingWithQualityHeaderValue>(TransferCodingWithQualityHeaderValue.TryParse), HttpHeaderKind.Request, 0, ", "),
				HeaderInfo.CreateMulti<string>("Trailer", new TryParseListDelegate<string>(CollectionParser.TryParse), HttpHeaderKind.Request | HttpHeaderKind.Response, 1, ", "),
				HeaderInfo.CreateMulti<TransferCodingHeaderValue>("Transfer-Encoding", new TryParseListDelegate<TransferCodingHeaderValue>(TransferCodingHeaderValue.TryParse), HttpHeaderKind.Request | HttpHeaderKind.Response, 1, ", "),
				HeaderInfo.CreateMulti<ProductHeaderValue>("Upgrade", new TryParseListDelegate<ProductHeaderValue>(ProductHeaderValue.TryParse), HttpHeaderKind.Request | HttpHeaderKind.Response, 1, ", "),
				HeaderInfo.CreateMulti<ProductInfoHeaderValue>("User-Agent", new TryParseListDelegate<ProductInfoHeaderValue>(ProductInfoHeaderValue.TryParse), HttpHeaderKind.Request, 1, " "),
				HeaderInfo.CreateMulti<string>("Vary", new TryParseListDelegate<string>(CollectionParser.TryParse), HttpHeaderKind.Response, 1, ", "),
				HeaderInfo.CreateMulti<ViaHeaderValue>("Via", new TryParseListDelegate<ViaHeaderValue>(ViaHeaderValue.TryParse), HttpHeaderKind.Request | HttpHeaderKind.Response, 1, ", "),
				HeaderInfo.CreateMulti<WarningHeaderValue>("Warning", new TryParseListDelegate<WarningHeaderValue>(WarningHeaderValue.TryParse), HttpHeaderKind.Request | HttpHeaderKind.Response, 1, ", "),
				HeaderInfo.CreateMulti<AuthenticationHeaderValue>("WWW-Authenticate", new TryParseListDelegate<AuthenticationHeaderValue>(AuthenticationHeaderValue.TryParse), HttpHeaderKind.Response, 1, ", ")
			};
			HttpHeaders.known_headers = new Dictionary<string, HeaderInfo>(StringComparer.OrdinalIgnoreCase);
			foreach (HeaderInfo headerInfo in array)
			{
				HttpHeaders.known_headers.Add(headerInfo.Name, headerInfo);
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.Headers.HttpHeaders" /> class.</summary>
		// Token: 0x06000265 RID: 613 RVA: 0x00009656 File Offset: 0x00007856
		protected HttpHeaders()
		{
			this.headers = new Dictionary<string, HttpHeaders.HeaderBucket>(StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000966E File Offset: 0x0000786E
		internal HttpHeaders(HttpHeaderKind headerKind)
			: this()
		{
			this.HeaderKind = headerKind;
		}

		/// <summary>Adds the specified header and its value into the <see cref="T:System.Net.Http.Headers.HttpHeaders" /> collection.</summary>
		/// <param name="name">The header to add to the collection.</param>
		/// <param name="value">The content of the header.</param>
		// Token: 0x06000267 RID: 615 RVA: 0x0000967D File Offset: 0x0000787D
		public void Add(string name, string value)
		{
			this.Add(name, new string[] { value });
		}

		/// <summary>Adds the specified header and its values into the <see cref="T:System.Net.Http.Headers.HttpHeaders" /> collection.</summary>
		/// <param name="name">The header to add to the collection.</param>
		/// <param name="values">A list of header values to add to the collection.</param>
		// Token: 0x06000268 RID: 616 RVA: 0x00009690 File Offset: 0x00007890
		public void Add(string name, IEnumerable<string> values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			this.AddInternal(name, values, this.CheckName(name), false);
		}

		// Token: 0x06000269 RID: 617 RVA: 0x000096B1 File Offset: 0x000078B1
		internal bool AddValue(string value, HeaderInfo headerInfo, bool ignoreInvalid)
		{
			return this.AddInternal(headerInfo.Name, new string[] { value }, headerInfo, ignoreInvalid);
		}

		// Token: 0x0600026A RID: 618 RVA: 0x000096CC File Offset: 0x000078CC
		private bool AddInternal(string name, IEnumerable<string> values, HeaderInfo headerInfo, bool ignoreInvalid)
		{
			HttpHeaders.HeaderBucket headerBucket;
			this.headers.TryGetValue(name, out headerBucket);
			bool flag = true;
			foreach (string text in values)
			{
				bool flag2 = headerBucket == null;
				if (headerInfo != null)
				{
					object obj;
					if (!headerInfo.TryParse(text, out obj))
					{
						if (ignoreInvalid)
						{
							flag = false;
							continue;
						}
						throw new FormatException("Could not parse value for header '" + name + "'");
					}
					else if (headerInfo.AllowsMany)
					{
						if (headerBucket == null)
						{
							headerBucket = new HttpHeaders.HeaderBucket(headerInfo.CreateCollection(this), headerInfo.CustomToString);
						}
						headerInfo.AddToCollection(headerBucket.Parsed, obj);
					}
					else
					{
						if (headerBucket != null)
						{
							throw new FormatException();
						}
						headerBucket = new HttpHeaders.HeaderBucket(obj, headerInfo.CustomToString);
					}
				}
				else
				{
					if (headerBucket == null)
					{
						headerBucket = new HttpHeaders.HeaderBucket(null, null);
					}
					headerBucket.Values.Add(text ?? string.Empty);
				}
				if (flag2)
				{
					this.headers.Add(name, headerBucket);
				}
			}
			return flag;
		}

		/// <summary>Returns a value that indicates whether the specified header and its value were added to the <see cref="T:System.Net.Http.Headers.HttpHeaders" /> collection without validating the provided information.</summary>
		/// <param name="name">The header to add to the collection.</param>
		/// <param name="value">The content of the header.</param>
		/// <returns>
		///   <see langword="true" /> if the specified header <paramref name="name" /> and <paramref name="value" /> could be added to the collection; otherwise <see langword="false" />.</returns>
		// Token: 0x0600026B RID: 619 RVA: 0x000097D4 File Offset: 0x000079D4
		public bool TryAddWithoutValidation(string name, string value)
		{
			return this.TryAddWithoutValidation(name, new string[] { value });
		}

		/// <summary>Returns a value that indicates whether the specified header and its values were added to the <see cref="T:System.Net.Http.Headers.HttpHeaders" /> collection without validating the provided information.</summary>
		/// <param name="name">The header to add to the collection.</param>
		/// <param name="values">The values of the header.</param>
		/// <returns>
		///   <see langword="true" /> if the specified header <paramref name="name" /> and <paramref name="values" /> could be added to the collection; otherwise <see langword="false" />.</returns>
		// Token: 0x0600026C RID: 620 RVA: 0x000097E8 File Offset: 0x000079E8
		public bool TryAddWithoutValidation(string name, IEnumerable<string> values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			HeaderInfo headerInfo;
			if (!this.TryCheckName(name, out headerInfo))
			{
				return false;
			}
			this.AddInternal(name, values, null, true);
			return true;
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000981C File Offset: 0x00007A1C
		private HeaderInfo CheckName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException("name");
			}
			Parser.Token.Check(name);
			HeaderInfo headerInfo;
			if (!HttpHeaders.known_headers.TryGetValue(name, out headerInfo) || (headerInfo.HeaderKind & this.HeaderKind) != HttpHeaderKind.None)
			{
				return headerInfo;
			}
			if (this.HeaderKind != HttpHeaderKind.None && ((this.HeaderKind | headerInfo.HeaderKind) & HttpHeaderKind.Content) != HttpHeaderKind.None)
			{
				throw new InvalidOperationException(name);
			}
			return null;
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00009884 File Offset: 0x00007A84
		private bool TryCheckName(string name, out HeaderInfo headerInfo)
		{
			if (!Parser.Token.TryCheck(name))
			{
				headerInfo = null;
				return false;
			}
			return !HttpHeaders.known_headers.TryGetValue(name, out headerInfo) || (headerInfo.HeaderKind & this.HeaderKind) != HttpHeaderKind.None || this.HeaderKind == HttpHeaderKind.None || ((this.HeaderKind | headerInfo.HeaderKind) & HttpHeaderKind.Content) == HttpHeaderKind.None;
		}

		/// <summary>Removes all headers from the <see cref="T:System.Net.Http.Headers.HttpHeaders" /> collection.</summary>
		// Token: 0x0600026F RID: 623 RVA: 0x000098D9 File Offset: 0x00007AD9
		public void Clear()
		{
			this.connectionclose = null;
			this.transferEncodingChunked = null;
			this.headers.Clear();
		}

		/// <summary>Returns if  a specific header exists in the <see cref="T:System.Net.Http.Headers.HttpHeaders" /> collection.</summary>
		/// <param name="name">The specific header.</param>
		/// <returns>
		///   <see langword="true" /> is the specified header exists in the collection; otherwise <see langword="false" />.</returns>
		// Token: 0x06000270 RID: 624 RVA: 0x000098FE File Offset: 0x00007AFE
		public bool Contains(string name)
		{
			this.CheckName(name);
			return this.headers.ContainsKey(name);
		}

		/// <summary>Returns an enumerator that can iterate through the <see cref="T:System.Net.Http.Headers.HttpHeaders" /> instance.</summary>
		/// <returns>An enumerator for the <see cref="T:System.Net.Http.Headers.HttpHeaders" />.</returns>
		// Token: 0x06000271 RID: 625 RVA: 0x00009914 File Offset: 0x00007B14
		public IEnumerator<KeyValuePair<string, IEnumerable<string>>> GetEnumerator()
		{
			foreach (KeyValuePair<string, HttpHeaders.HeaderBucket> keyValuePair in this.headers)
			{
				HttpHeaders.HeaderBucket headerBucket = this.headers[keyValuePair.Key];
				HeaderInfo headerInfo;
				HttpHeaders.known_headers.TryGetValue(keyValuePair.Key, out headerInfo);
				List<string> allHeaderValues = this.GetAllHeaderValues(headerBucket, headerInfo);
				if (allHeaderValues != null)
				{
					yield return new KeyValuePair<string, IEnumerable<string>>(keyValuePair.Key, allHeaderValues);
				}
			}
			Dictionary<string, HttpHeaders.HeaderBucket>.Enumerator enumerator = default(Dictionary<string, HttpHeaders.HeaderBucket>.Enumerator);
			yield break;
			yield break;
		}

		/// <summary>Gets an enumerator that can iterate through a <see cref="T:System.Net.Http.Headers.HttpHeaders" />.</summary>
		/// <returns>An instance of an implementation of an <see cref="T:System.Collections.IEnumerator" /> that can iterate through a <see cref="T:System.Net.Http.Headers.HttpHeaders" />.</returns>
		// Token: 0x06000272 RID: 626 RVA: 0x00009923 File Offset: 0x00007B23
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>Returns all header values for a specified header stored in the <see cref="T:System.Net.Http.Headers.HttpHeaders" /> collection.</summary>
		/// <param name="name">The specified header to return values for.</param>
		/// <returns>An array of header strings.</returns>
		/// <exception cref="T:System.InvalidOperationException">The header cannot be found.</exception>
		// Token: 0x06000273 RID: 627 RVA: 0x0000992C File Offset: 0x00007B2C
		public IEnumerable<string> GetValues(string name)
		{
			this.CheckName(name);
			IEnumerable<string> enumerable;
			if (!this.TryGetValues(name, out enumerable))
			{
				throw new InvalidOperationException();
			}
			return enumerable;
		}

		/// <summary>Removes the specified header from the <see cref="T:System.Net.Http.Headers.HttpHeaders" /> collection.</summary>
		/// <param name="name">The name of the header to remove from the collection.</param>
		/// <returns>Returns <see cref="T:System.Boolean" />.</returns>
		// Token: 0x06000274 RID: 628 RVA: 0x00009953 File Offset: 0x00007B53
		public bool Remove(string name)
		{
			this.CheckName(name);
			return this.headers.Remove(name);
		}

		/// <summary>Return if a specified header and specified values are stored in the <see cref="T:System.Net.Http.Headers.HttpHeaders" /> collection.</summary>
		/// <param name="name">The specified header.</param>
		/// <param name="values">The specified header values.</param>
		/// <returns>
		///   <see langword="true" /> is the specified header <paramref name="name" /> and <see langword="values" /> are stored in the collection; otherwise <see langword="false" />.</returns>
		// Token: 0x06000275 RID: 629 RVA: 0x0000996C File Offset: 0x00007B6C
		public bool TryGetValues(string name, out IEnumerable<string> values)
		{
			HeaderInfo headerInfo;
			if (!this.TryCheckName(name, out headerInfo))
			{
				values = null;
				return false;
			}
			HttpHeaders.HeaderBucket headerBucket;
			if (!this.headers.TryGetValue(name, out headerBucket))
			{
				values = null;
				return false;
			}
			values = this.GetAllHeaderValues(headerBucket, headerInfo);
			return true;
		}

		// Token: 0x06000276 RID: 630 RVA: 0x000099AC File Offset: 0x00007BAC
		internal static string GetSingleHeaderString(string key, IEnumerable<string> values)
		{
			string text = ",";
			HeaderInfo headerInfo;
			if (HttpHeaders.known_headers.TryGetValue(key, out headerInfo) && headerInfo.AllowsMany)
			{
				text = headerInfo.Separator;
			}
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (string text2 in values)
			{
				if (!flag)
				{
					stringBuilder.Append(text);
					if (text != " ")
					{
						stringBuilder.Append(" ");
					}
				}
				stringBuilder.Append(text2);
				flag = false;
			}
			if (flag)
			{
				return null;
			}
			return stringBuilder.ToString();
		}

		/// <summary>Returns a string that represents the current <see cref="T:System.Net.Http.Headers.HttpHeaders" /> object.</summary>
		/// <returns>A string that represents the current object.</returns>
		// Token: 0x06000277 RID: 631 RVA: 0x00009A5C File Offset: 0x00007C5C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, IEnumerable<string>> keyValuePair in this)
			{
				stringBuilder.Append(keyValuePair.Key);
				stringBuilder.Append(": ");
				stringBuilder.Append(HttpHeaders.GetSingleHeaderString(keyValuePair.Key, keyValuePair.Value));
				stringBuilder.Append("\r\n");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000278 RID: 632 RVA: 0x00009AEC File Offset: 0x00007CEC
		internal void AddOrRemove(string name, string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				this.Remove(name);
				return;
			}
			this.SetValue<string>(name, value, null);
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00009B08 File Offset: 0x00007D08
		internal void AddOrRemove<T>(string name, T value, Func<object, string> converter = null) where T : class
		{
			if (value == null)
			{
				this.Remove(name);
				return;
			}
			this.SetValue<T>(name, value, converter);
		}

		// Token: 0x0600027A RID: 634 RVA: 0x00009B24 File Offset: 0x00007D24
		internal void AddOrRemove<T>(string name, T? value) where T : struct
		{
			this.AddOrRemove<T>(name, value, null);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x00009B2F File Offset: 0x00007D2F
		internal void AddOrRemove<T>(string name, T? value, Func<object, string> converter) where T : struct
		{
			if (value == null)
			{
				this.Remove(name);
				return;
			}
			this.SetValue<T?>(name, value, converter);
		}

		// Token: 0x0600027C RID: 636 RVA: 0x00009B4C File Offset: 0x00007D4C
		private List<string> GetAllHeaderValues(HttpHeaders.HeaderBucket bucket, HeaderInfo headerInfo)
		{
			List<string> list = null;
			if (headerInfo != null && headerInfo.AllowsMany)
			{
				list = headerInfo.ToStringCollection(bucket.Parsed);
			}
			else if (bucket.Parsed != null)
			{
				string text = bucket.ParsedToString();
				if (!string.IsNullOrEmpty(text))
				{
					list = new List<string>();
					list.Add(text);
				}
			}
			if (bucket.HasStringValues)
			{
				if (list == null)
				{
					list = new List<string>();
				}
				list.AddRange(bucket.Values);
			}
			return list;
		}

		// Token: 0x0600027D RID: 637 RVA: 0x00009BB8 File Offset: 0x00007DB8
		internal static HttpHeaderKind GetKnownHeaderKind(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException("name");
			}
			HeaderInfo headerInfo;
			if (HttpHeaders.known_headers.TryGetValue(name, out headerInfo))
			{
				return headerInfo.HeaderKind;
			}
			return HttpHeaderKind.None;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00009BF0 File Offset: 0x00007DF0
		internal T GetValue<T>(string name)
		{
			HttpHeaders.HeaderBucket headerBucket;
			if (!this.headers.TryGetValue(name, out headerBucket))
			{
				return default(T);
			}
			if (headerBucket.HasStringValues)
			{
				object obj;
				if (!HttpHeaders.known_headers[name].TryParse(headerBucket.Values[0], out obj))
				{
					if (!(typeof(T) == typeof(string)))
					{
						return default(T);
					}
					return (T)((object)headerBucket.Values[0]);
				}
				else
				{
					headerBucket.Parsed = obj;
					headerBucket.Values = null;
				}
			}
			return (T)((object)headerBucket.Parsed);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00009C90 File Offset: 0x00007E90
		internal HttpHeaderValueCollection<T> GetValues<T>(string name) where T : class
		{
			HttpHeaders.HeaderBucket headerBucket;
			if (!this.headers.TryGetValue(name, out headerBucket))
			{
				HeaderInfo headerInfo = HttpHeaders.known_headers[name];
				headerBucket = new HttpHeaders.HeaderBucket(new HttpHeaderValueCollection<T>(this, headerInfo), headerInfo.CustomToString);
				this.headers.Add(name, headerBucket);
			}
			HttpHeaderValueCollection<T> httpHeaderValueCollection = (HttpHeaderValueCollection<T>)headerBucket.Parsed;
			if (headerBucket.HasStringValues)
			{
				HeaderInfo headerInfo2 = HttpHeaders.known_headers[name];
				if (httpHeaderValueCollection == null)
				{
					httpHeaderValueCollection = (headerBucket.Parsed = new HttpHeaderValueCollection<T>(this, headerInfo2));
				}
				for (int i = 0; i < headerBucket.Values.Count; i++)
				{
					string text = headerBucket.Values[i];
					object obj;
					if (!headerInfo2.TryParse(text, out obj))
					{
						httpHeaderValueCollection.AddInvalidValue(text);
					}
					else
					{
						headerInfo2.AddToCollection(httpHeaderValueCollection, obj);
					}
				}
				headerBucket.Values.Clear();
			}
			return httpHeaderValueCollection;
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00009D5F File Offset: 0x00007F5F
		internal void SetValue<T>(string name, T value, Func<object, string> toStringConverter = null)
		{
			this.headers[name] = new HttpHeaders.HeaderBucket(value, toStringConverter);
		}

		// Token: 0x04000106 RID: 262
		private static readonly Dictionary<string, HeaderInfo> known_headers;

		// Token: 0x04000107 RID: 263
		private readonly Dictionary<string, HttpHeaders.HeaderBucket> headers;

		// Token: 0x04000108 RID: 264
		private readonly HttpHeaderKind HeaderKind;

		// Token: 0x04000109 RID: 265
		internal bool? connectionclose;

		// Token: 0x0400010A RID: 266
		internal bool? transferEncodingChunked;

		// Token: 0x02000046 RID: 70
		private class HeaderBucket
		{
			// Token: 0x06000281 RID: 641 RVA: 0x00009D79 File Offset: 0x00007F79
			public HeaderBucket(object parsed, Func<object, string> converter)
			{
				this.Parsed = parsed;
				this.CustomToString = converter;
			}

			// Token: 0x1700009A RID: 154
			// (get) Token: 0x06000282 RID: 642 RVA: 0x00009D8F File Offset: 0x00007F8F
			public bool HasStringValues
			{
				get
				{
					return this.values != null && this.values.Count > 0;
				}
			}

			// Token: 0x1700009B RID: 155
			// (get) Token: 0x06000283 RID: 643 RVA: 0x00009DAC File Offset: 0x00007FAC
			// (set) Token: 0x06000284 RID: 644 RVA: 0x00009DD1 File Offset: 0x00007FD1
			public List<string> Values
			{
				get
				{
					List<string> list;
					if ((list = this.values) == null)
					{
						list = (this.values = new List<string>());
					}
					return list;
				}
				set
				{
					this.values = value;
				}
			}

			// Token: 0x06000285 RID: 645 RVA: 0x00009DDA File Offset: 0x00007FDA
			public string ParsedToString()
			{
				if (this.Parsed == null)
				{
					return null;
				}
				if (this.CustomToString != null)
				{
					return this.CustomToString(this.Parsed);
				}
				return this.Parsed.ToString();
			}

			// Token: 0x0400010B RID: 267
			public object Parsed;

			// Token: 0x0400010C RID: 268
			private List<string> values;

			// Token: 0x0400010D RID: 269
			public readonly Func<object, string> CustomToString;
		}
	}
}
