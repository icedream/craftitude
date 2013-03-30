#region Imports (5)

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

#endregion Imports (5)

namespace Craftitude.ClientApi
{


    public class CraftitudeClient
    {
        #region Enums of CraftitudeClient (2)

        private List<string> _repositories = new List<string>(new[] {
            "http://repo.craftitude.tk/minecraft/",
            "http://repo.craftitude.tk/libraries/",
            "http://repo.craftitude.tk/mods/"
        });
        private string _targetDirectory = Environment.CurrentDirectory;

        #endregion Enums of CraftitudeClient (2)

        #region Properties of CraftitudeClient (7)

        internal DirectoryInfo CraftitudeDirectoryInfo { get; private set; }

        internal string CraftitudeRepositoriesListFilePath { get { return Path.Combine(CraftitudeDirectoryInfo.FullName, "repositories.list"); } }

        public List<string> Repositories { get { return _repositories; } }

        internal Uri RepositoryEnhancementsUrl { get { return new Uri(RepositoryUrl, "enhanced/"); } }

        public Uri RepositoryUrl { get; private set; }

        public string TargetDirectory { get { return _targetDirectory; } set { _targetDirectory = value; } }

        internal DirectoryInfo TargetDirectoryInfo { get { return new DirectoryInfo(_targetDirectory); } }

        #endregion Properties of CraftitudeClient (7)

        #region Methods of CraftitudeClient (4)

        private void _internalTargetInit()
        {
            CraftitudeDirectoryInfo = TargetDirectoryInfo.CreateSubdirectory("Craftitude");
            if (!File.Exists(CraftitudeRepositoriesListFilePath))
                _saveRepositoriesList();
            else
                _loadRepositoriesList();
        }

        private void _loadRepositoriesList()
        {
            Repositories.Clear();
            Repositories.AddRange(File.ReadAllLines(CraftitudeRepositoriesListFilePath));
        }

        private void _saveRepositoriesList()
        {
            File.WriteAllLines(CraftitudeRepositoriesListFilePath, Repositories);
        }

        public void Save()
        {
            _saveRepositoriesList();
        }

        #endregion Methods of CraftitudeClient (4)
    }
}
