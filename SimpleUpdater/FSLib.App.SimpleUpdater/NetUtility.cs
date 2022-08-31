namespace FSLib.App.SimpleUpdater
{
	using System.Net;

	internal class NetUtility
	{
		public static void EnableCompatibility()
		{
#if NET20 || NET35 || NET40
			ServicePointManager.SecurityProtocol =
				SecurityProtocolType.Ssl3
				| SecurityProtocolType.Tls
				| (SecurityProtocolType)3072 //TLS 1.2
				| (SecurityProtocolType)768; //TLS 1.1;
#endif
		}
	}
}
