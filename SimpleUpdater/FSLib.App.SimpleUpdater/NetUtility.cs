namespace FSLib.App.SimpleUpdater
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Security;

    internal class NetUtility
    {
        static HashSet<string> _ignoreDomainCert = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        static NetUtility()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) =>
            {
                var request = (HttpWebRequest)sender;
                if (_ignoreDomainCert.Contains(request.Host))
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
#if NET20 || NET35 || NET40
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
            if (!_ignoreDomainCert.Contains(host))
                _ignoreDomainCert.Add(host);
        }
    }
}
