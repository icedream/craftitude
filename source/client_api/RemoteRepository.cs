#region Imports (12)

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using YamlDotNet;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;
using YamlDotNet.RepresentationModel.Serialization;

#endregion Imports (12)

namespace Minova.ClientApi
{


    internal class Distribution
    {
        #region Properties of Distribution (3)

        public string DistributionID { get; private set; }

        internal Uri DistributionUrl { get { return Repository.GetUri(string.Format("{0}/", DistributionID)); } }

        internal Repository Repository { get; private set; }

        #endregion Properties of Distribution (3)

        #region Constructors of Distribution (1)

        public Distribution(Repository parentRepository, string distributionName)
        {
            Repository = parentRepository;
            DistributionID = distributionName;

            // TODO: Repository validation with exception on fail
        }

        #endregion Constructors of Distribution (1)

        #region Methods of Distribution (2)

        public DistributionPackageList GetAvailablePackages(string since)
        {
            YamlSerializer s = new YamlSerializer(typeof(DistributionPackageList));
            HttpWebRequest wr = (HttpWebRequest)HttpWebRequest.Create(GetUri("packages.yml?since=" + since));
            var wresp = wr.GetResponse();
            DistributionPackageList ret = null;
            using (var ws = wresp.GetResponseStream())
            {
                using (var wsr = new StreamReader(ws))
                {
                    ret = (DistributionPackageList)s.Deserialize(wsr);
                }
            }
            return ret;
        }

        internal Uri GetUri(string relativePath)
        {
            return new Uri(DistributionUrl, relativePath);
        }

        #endregion Methods of Distribution (2)
    }

    public class DistributionPackageList : List<DistributionPackageListEntry>
    {
    }

    public class DistributionPackageListEntry
    {
        #region Properties of DistributionPackageListEntry (2)

        public string CurrentVersion { get; internal set; }

        public PackageMetadata Metadata { get; internal set; }

        #endregion Properties of DistributionPackageListEntry (2)
    }

    public class PackageMetadata
    {
        #region Properties of PackageMetadata (10)

        public string[] Dependencies { get; private set; }

        public string Description { get; private set; }

        public string[] Developers { get; private set; }

        public string ID { get; internal set; }

        public string License { get; private set; }

        [YamlIgnore()]
        public string LicenseNameOrText { get { return string.Join("; ", License.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries).Where(licenseEntry => !Uri.IsWellFormedUriString(licenseEntry, UriKind.Absolute))); } }

        [YamlIgnore()]
        public string LicenseUrl { get { return License.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries).SingleOrDefault(licenseEntry => Uri.IsWellFormedUriString(licenseEntry, UriKind.Absolute)); } }

        public string[] Maintainers { get; private set; }

        public string Name { get; private set; }

        public bool PlatformDependence { get; private set; }

        #endregion Properties of PackageMetadata (10)
    }

    internal class Repository
    {
        #region Properties of Repository (1)

        public Uri RepositoryUrl { get; private set; }

        #endregion Properties of Repository (1)

        #region Methods of Repository (2)

        public Distribution GetDistribution(string dist)
        {
            return new Distribution(this, dist);
        }

        internal Uri GetUri(string relativePath)
        {
            return new Uri(RepositoryUrl, relativePath);
        }

        #endregion Methods of Repository (2)
    }
}
