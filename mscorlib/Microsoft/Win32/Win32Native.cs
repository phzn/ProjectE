using System;
using System.IO;

namespace Microsoft.Win32
{
	// Token: 0x020000AC RID: 172
	internal static class Win32Native
	{
		// Token: 0x0600046F RID: 1135 RVA: 0x000173EE File Offset: 0x000155EE
		public static string GetMessage(int hr)
		{
			return "Error " + hr.ToString();
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00017401 File Offset: 0x00015601
		public static int MakeHRFromErrorCode(int errorCode)
		{
			return -2147024896 | errorCode;
		}

		// Token: 0x04000F81 RID: 3969
		internal const string ADVAPI32 = "advapi32.dll";

		// Token: 0x04000F82 RID: 3970
		internal const int ERROR_SUCCESS = 0;

		// Token: 0x04000F83 RID: 3971
		internal const int ERROR_INVALID_FUNCTION = 1;

		// Token: 0x04000F84 RID: 3972
		internal const int ERROR_FILE_NOT_FOUND = 2;

		// Token: 0x04000F85 RID: 3973
		internal const int ERROR_PATH_NOT_FOUND = 3;

		// Token: 0x04000F86 RID: 3974
		internal const int ERROR_ACCESS_DENIED = 5;

		// Token: 0x04000F87 RID: 3975
		internal const int ERROR_INVALID_HANDLE = 6;

		// Token: 0x04000F88 RID: 3976
		internal const int ERROR_NOT_ENOUGH_MEMORY = 8;

		// Token: 0x04000F89 RID: 3977
		internal const int ERROR_INVALID_DATA = 13;

		// Token: 0x04000F8A RID: 3978
		internal const int ERROR_INVALID_DRIVE = 15;

		// Token: 0x04000F8B RID: 3979
		internal const int ERROR_NO_MORE_FILES = 18;

		// Token: 0x04000F8C RID: 3980
		internal const int ERROR_NOT_READY = 21;

		// Token: 0x04000F8D RID: 3981
		internal const int ERROR_BAD_LENGTH = 24;

		// Token: 0x04000F8E RID: 3982
		internal const int ERROR_SHARING_VIOLATION = 32;

		// Token: 0x04000F8F RID: 3983
		internal const int ERROR_NOT_SUPPORTED = 50;

		// Token: 0x04000F90 RID: 3984
		internal const int ERROR_FILE_EXISTS = 80;

		// Token: 0x04000F91 RID: 3985
		internal const int ERROR_INVALID_PARAMETER = 87;

		// Token: 0x04000F92 RID: 3986
		internal const int ERROR_BROKEN_PIPE = 109;

		// Token: 0x04000F93 RID: 3987
		internal const int ERROR_CALL_NOT_IMPLEMENTED = 120;

		// Token: 0x04000F94 RID: 3988
		internal const int ERROR_INSUFFICIENT_BUFFER = 122;

		// Token: 0x04000F95 RID: 3989
		internal const int ERROR_INVALID_NAME = 123;

		// Token: 0x04000F96 RID: 3990
		internal const int ERROR_BAD_PATHNAME = 161;

		// Token: 0x04000F97 RID: 3991
		internal const int ERROR_ALREADY_EXISTS = 183;

		// Token: 0x04000F98 RID: 3992
		internal const int ERROR_ENVVAR_NOT_FOUND = 203;

		// Token: 0x04000F99 RID: 3993
		internal const int ERROR_FILENAME_EXCED_RANGE = 206;

		// Token: 0x04000F9A RID: 3994
		internal const int ERROR_NO_DATA = 232;

		// Token: 0x04000F9B RID: 3995
		internal const int ERROR_PIPE_NOT_CONNECTED = 233;

		// Token: 0x04000F9C RID: 3996
		internal const int ERROR_MORE_DATA = 234;

		// Token: 0x04000F9D RID: 3997
		internal const int ERROR_DIRECTORY = 267;

		// Token: 0x04000F9E RID: 3998
		internal const int ERROR_OPERATION_ABORTED = 995;

		// Token: 0x04000F9F RID: 3999
		internal const int ERROR_NOT_FOUND = 1168;

		// Token: 0x04000FA0 RID: 4000
		internal const int ERROR_NO_TOKEN = 1008;

		// Token: 0x04000FA1 RID: 4001
		internal const int ERROR_DLL_INIT_FAILED = 1114;

		// Token: 0x04000FA2 RID: 4002
		internal const int ERROR_NON_ACCOUNT_SID = 1257;

		// Token: 0x04000FA3 RID: 4003
		internal const int ERROR_NOT_ALL_ASSIGNED = 1300;

		// Token: 0x04000FA4 RID: 4004
		internal const int ERROR_UNKNOWN_REVISION = 1305;

		// Token: 0x04000FA5 RID: 4005
		internal const int ERROR_INVALID_OWNER = 1307;

		// Token: 0x04000FA6 RID: 4006
		internal const int ERROR_INVALID_PRIMARY_GROUP = 1308;

		// Token: 0x04000FA7 RID: 4007
		internal const int ERROR_NO_SUCH_PRIVILEGE = 1313;

		// Token: 0x04000FA8 RID: 4008
		internal const int ERROR_PRIVILEGE_NOT_HELD = 1314;

		// Token: 0x04000FA9 RID: 4009
		internal const int ERROR_NONE_MAPPED = 1332;

		// Token: 0x04000FAA RID: 4010
		internal const int ERROR_INVALID_ACL = 1336;

		// Token: 0x04000FAB RID: 4011
		internal const int ERROR_INVALID_SID = 1337;

		// Token: 0x04000FAC RID: 4012
		internal const int ERROR_INVALID_SECURITY_DESCR = 1338;

		// Token: 0x04000FAD RID: 4013
		internal const int ERROR_BAD_IMPERSONATION_LEVEL = 1346;

		// Token: 0x04000FAE RID: 4014
		internal const int ERROR_CANT_OPEN_ANONYMOUS = 1347;

		// Token: 0x04000FAF RID: 4015
		internal const int ERROR_NO_SECURITY_ON_OBJECT = 1350;

		// Token: 0x04000FB0 RID: 4016
		internal const int ERROR_TRUSTED_RELATIONSHIP_FAILURE = 1789;

		// Token: 0x04000FB1 RID: 4017
		internal const FileAttributes FILE_ATTRIBUTE_DIRECTORY = FileAttributes.Directory;

		// Token: 0x020000AD RID: 173
		public class SECURITY_ATTRIBUTES
		{
		}

		// Token: 0x020000AE RID: 174
		internal class WIN32_FIND_DATA
		{
			// Token: 0x04000FB2 RID: 4018
			internal int dwFileAttributes;

			// Token: 0x04000FB3 RID: 4019
			internal string cFileName;
		}
	}
}
