namespace FSLib.App.SimpleUpdater
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Security;

    internal class NetUtility
    {
        static Dictionary<string, bool> _ignoreDomainCert = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

        static NetUtility()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) =>
            {
                var request = (HttpWebRequest)sender;
#if NET20
                var host = request.Address.Host;
#else
                var host = request.Host;
#endif
                if (_ignoreDomainCert.ContainsKey(host))
                {
                    return true;
                }

                return errors == SslPolicyErrors.None;
            };
        }

        public static void EnableCompatibility()
        {
            if (Environment.OSVersion.Version.Major > 5)
            {
#if NET20 || NET35 || NET40 || NET45
                ServicePointManager.SecurityProtocol =
                    SecurityProtocolType.Ssl3
                    | SecurityProtocolType.Tls
                    | (SecurityProtocolType)3072 //TLS 1.2
                    | (SecurityProtocolType)768; //TLS 1.1;
#endif
            }
        }

        internal static void AddIgnoreDomainCertForUrl(string url) => AddIgnoreDomainCertForUrl(url);

        internal static void AddIgnoreDomainCertForUrl(Uri uri)
        {
            var host = uri.Host;
            if (!_ignoreDomainCert.ContainsKey(host))
                _ignoreDomainCert.Add(host, true);
        }
    }
}