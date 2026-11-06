using System;
using System.Security.Cryptography;
using System.Text;

namespace System.Net
{
	// Token: 0x02000675 RID: 1653
	internal class DigestSession
	{
		// Token: 0x06003400 RID: 13312 RVA: 0x000B5585 File Offset: 0x000B3785
		public DigestSession()
		{
			this._nc = 1;
			this.lastUse = DateTime.UtcNow;
		}

		// Token: 0x17000A80 RID: 2688
		// (get) Token: 0x06003401 RID: 13313 RVA: 0x000B559F File Offset: 0x000B379F
		public string Algorithm
		{
			get
			{
				return this.parser.Algorithm;
			}
		}

		// Token: 0x17000A81 RID: 2689
		// (get) Token: 0x06003402 RID: 13314 RVA: 0x000B55AC File Offset: 0x000B37AC
		public string Realm
		{
			get
			{
				return this.parser.Realm;
			}
		}

		// Token: 0x17000A82 RID: 2690
		// (get) Token: 0x06003403 RID: 13315 RVA: 0x000B55B9 File Offset: 0x000B37B9
		public string Nonce
		{
			get
			{
				return this.parser.Nonce;
			}
		}

		// Token: 0x17000A83 RID: 2691
		// (get) Token: 0x06003404 RID: 13316 RVA: 0x000B55C6 File Offset: 0x000B37C6
		public string Opaque
		{
			get
			{
				return this.parser.Opaque;
			}
		}

		// Token: 0x17000A84 RID: 2692
		// (get) Token: 0x06003405 RID: 13317 RVA: 0x000B55D3 File Offset: 0x000B37D3
		public string QOP
		{
			get
			{
				return this.parser.QOP;
			}
		}

		// Token: 0x17000A85 RID: 2693
		// (get) Token: 0x06003406 RID: 13318 RVA: 0x000B55E0 File Offset: 0x000B37E0
		public string CNonce
		{
			get
			{
				if (this._cnonce == null)
				{
					byte[] array = new byte[15];
					DigestSession.rng.GetBytes(array);
					this._cnonce = Convert.ToBase64String(array);
					Array.Clear(array, 0, array.Length);
				}
				return this._cnonce;
			}
		}

		// Token: 0x06003407 RID: 13319 RVA: 0x000B5624 File Offset: 0x000B3824
		public bool Parse(string challenge)
		{
			this.parser = new DigestHeaderParser(challenge);
			if (!this.parser.Parse())
			{
				return false;
			}
			if (this.parser.Algorithm == null || this.parser.Algorithm.ToUpper().StartsWith("MD5"))
			{
				this.hash = MD5.Create();
			}
			return true;
		}

		// Token: 0x06003408 RID: 13320 RVA: 0x000B5684 File Offset: 0x000B3884
		private string HashToHexString(string toBeHashed)
		{
			if (this.hash == null)
			{
				return null;
			}
			this.hash.Initialize();
			byte[] array = this.hash.ComputeHash(Encoding.ASCII.GetBytes(toBeHashed));
			StringBuilder stringBuilder = new StringBuilder();
			foreach (byte b in array)
			{
				stringBuilder.Append(b.ToString("x2"));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06003409 RID: 13321 RVA: 0x000B56F0 File Offset: 0x000B38F0
		private string HA1(string username, string password)
		{
			string text = string.Format("{0}:{1}:{2}", username, this.Realm, password);
			if (this.Algorithm != null && this.Algorithm.ToLower() == "md5-sess")
			{
				text = string.Format("{0}:{1}:{2}", this.HashToHexString(text), this.Nonce, this.CNonce);
			}
			return this.HashToHexString(text);
		}

		// Token: 0x0600340A RID: 13322 RVA: 0x000B5754 File Offset: 0x000B3954
		private string HA2(HttpWebRequest webRequest)
		{
			string text = string.Format("{0}:{1}", webRequest.Method, webRequest.RequestUri.PathAndQuery);
			this.QOP == "auth-int";
			return this.HashToHexString(text);
		}

		// Token: 0x0600340B RID: 13323 RVA: 0x000B5798 File Offset: 0x000B3998
		private string Response(string username, string password, HttpWebRequest webRequest)
		{
			string text = string.Format("{0}:{1}:", this.HA1(username, password), this.Nonce);
			if (this.QOP != null)
			{
				text += string.Format("{0}:{1}:{2}:", this._nc.ToString("X8"), this.CNonce, this.QOP);
			}
			text += this.HA2(webRequest);
			return this.HashToHexString(text);
		}

		// Token: 0x0600340C RID: 13324 RVA: 0x000B5808 File Offset: 0x000B3A08
		public Authorization Authenticate(WebRequest webRequest, ICredentials credentials)
		{
			if (this.parser == null)
			{
				throw new InvalidOperationException();
			}
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			if (httpWebRequest == null)
			{
				return null;
			}
			this.lastUse = DateTime.UtcNow;
			NetworkCredential credential = credentials.GetCredential(httpWebRequest.RequestUri, "digest");
			if (credential == null)
			{
				return null;
			}
			string userName = credential.UserName;
			if (userName == null || userName == "")
			{
				return null;
			}
			string password = credential.Password;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("Digest username=\"{0}\", ", userName);
			stringBuilder.AppendFormat("realm=\"{0}\", ", this.Realm);
			stringBuilder.AppendFormat("nonce=\"{0}\", ", this.Nonce);
			stringBuilder.AppendFormat("uri=\"{0}\", ", httpWebRequest.Address.PathAndQuery);
			if (this.Algorithm != null)
			{
				stringBuilder.AppendFormat("algorithm=\"{0}\", ", this.Algorithm);
			}
			stringBuilder.AppendFormat("response=\"{0}\", ", this.Response(userName, password, httpWebRequest));
			if (this.QOP != null)
			{
				stringBuilder.AppendFormat("qop=\"{0}\", ", this.QOP);
			}
			lock (this)
			{
				if (this.QOP != null)
				{
					stringBuilder.AppendFormat("nc={0:X8}, ", this._nc);
					this._nc++;
				}
			}
			if (this.CNonce != null)
			{
				stringBuilder.AppendFormat("cnonce=\"{0}\", ", this.CNonce);
			}
			if (this.Opaque != null)
			{
				stringBuilder.AppendFormat("opaque=\"{0}\", ", this.Opaque);
			}
			stringBuilder.Length -= 2;
			return new Authorization(stringBuilder.ToString());
		}

		// Token: 0x17000A86 RID: 2694
		// (get) Token: 0x0600340D RID: 13325 RVA: 0x000B59BC File Offset: 0x000B3BBC
		public DateTime LastUse
		{
			get
			{
				return this.lastUse;
			}
		}

		// Token: 0x04001E7C RID: 7804
		private static RandomNumberGenerator rng = RandomNumberGenerator.Create();

		// Token: 0x04001E7D RID: 7805
		private DateTime lastUse;

		// Token: 0x04001E7E RID: 7806
		private int _nc;

		// Token: 0x04001E7F RID: 7807
		private HashAlgorithm hash;

		// Token: 0x04001E80 RID: 7808
		private DigestHeaderParser parser;

		// Token: 0x04001E81 RID: 7809
		private string _cnonce;
	}
}
