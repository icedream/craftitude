using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Minova.ClientApi
{
    public class MinovaClient
    {
        public Uri RepositoryUrl { get; private set; }
        internal Uri RepositoryEnhancementsUrl { get { return new Uri(RepositoryUrl, "enhanced/"); } }

        private string _targetDirectory = Environment.CurrentDirectory;
        public string TargetDirectory { get { return _targetDirectory; } set { _targetDirectory = value; } }
        internal DirectoryInfo TargetDirectoryInfo { get { return new DirectoryInfo(_targetDirectory); } }
        internal DirectoryInfo MinovaDirectoryInfo { get; private set; }
        internal string MinovaRepositoriesListFilePath { get { return Path.Combine(MinovaDirectoryInfo.FullName, "repositories.list"); } }

        private List<string> _repositories = new List<string>(new[] {
            "http://repo.minova.tk/minecraft/",
            "http://repo.minova.tk/libraries/",
            "http://repo.minova.tk/mods/"
        });
        public List<string> Repositories { get { return _repositories; } }

        private void _internalTargetInit()
        {
            MinovaDirectoryInfo = TargetDirectoryInfo.CreateSubdirectory("minova");
            if (!File.Exists(MinovaRepositoriesListFilePath))
                _saveRepositoriesList();
            else
                _loadRepositoriesList();
        }

        public void Save()
        {
            _saveRepositoriesList();
        }

        private void _saveRepositoriesList()
        {
            File.WriteAllLines(MinovaRepositoriesListFilePath, Repositories);
        }

        private void _loadRepositoriesList()
        {
            Repositories.Clear();
            Repositories.AddRange(File.ReadAllLines(MinovaRepositoriesListFilePath));
        }
    }
}
