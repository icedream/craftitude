#region Imports (11)

using LuaInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using YaTools.Yaml;
using YaTools.Yaml.AbstractContracts;

#endregion Imports (11)

namespace Craftitude.ClientApi
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

        #region Methods of Distribution (4)

        public DistributionPackageList GetAvailablePackages(string since)
        {
            Uri uri;
            if (!string.IsNullOrEmpty(since))
                uri = GetUri("packages.yml?since=" + since);
            else
                uri = GetUri("packages.yml");
            HttpWebRequest wr = (HttpWebRequest)HttpWebRequest.Create(uri);
            var wresp = wr.GetResponse();
            Hashtable ht = null;
            DistributionPackageList ret = new DistributionPackageList();
            using (var ws = wresp.GetResponseStream())
            using (var wsr = new StreamReader(ws))
                ht = YamlLanguage.StringTo<Hashtable>(wsr.ReadToEnd());
            foreach (string entryName in ht.Keys)
            {
                var entry = ht[entryName] as Hashtable;
                ret.Add(new DistributionPackageListEntry() {
                    CurrentVersion = entry["CurrentVersion"].ToString(),
                    Distribution = this,
                    Metadata = new PackageMetadata() {
                        Data = entry["Metadata"] as Hashtable,
                        ID = entryName
                    }
                });
            }

            return ret;
        }

        public DistributionPackageList GetAvailablePackages(DateTime since)
        {
            return GetAvailablePackages(since.ToString("s"));
        }

        public DistributionPackageList GetAvailablePackages()
        {
            return GetAvailablePackages(null);
        }

        internal Uri GetUri(string relativePath)
        {
            return new Uri(DistributionUrl, relativePath);
        }

        #endregion Methods of Distribution (4)
    }

    public class DistributionPackageList : List<DistributionPackageListEntry>
    {
    }

    public class DistributionPackageListEntry
    {
        #region Properties of DistributionPackageListEntry (3)

        public string CurrentVersion { get; internal set; }

        public Distribution Distribution { get; internal set; }

        public PackageMetadata Metadata { get; internal set; }

        #endregion Properties of DistributionPackageListEntry (3)
    }

    public class PackageMetadata
    {
        #region Properties of PackageMetadata (10)

        public Hashtable Data { get; internal set; }

        public string[] Dependencies { get { return (string[])Data["Dependencies"]; } }

        public string Description { get { return Data["Description"].ToString(); } }

        public string[] Developers { get { return (string[])Data["Developers"]; } }

        public string ID { get; internal set; }

        public string License { get { return Data["License"].ToString(); } }

        public string LicenseNameOrText { get { return string.Join("; ", License.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries).Where(licenseEntry => !Uri.IsWellFormedUriString(licenseEntry, UriKind.Absolute))); } }

        public string LicenseUrl { get { return License.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries).SingleOrDefault(licenseEntry => Uri.IsWellFormedUriString(licenseEntry, UriKind.Absolute)); } }

        public string[] Maintainers { get { return (string[])Data["Maintainers"]; } }

        public string Name { get { return Data["Name"].ToString(); } }

        public bool PlatformDependence { get { return bool.Parse(Data["PlatformDependence"].ToString()); } }

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
