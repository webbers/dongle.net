using System;

namespace Dongle.Utils
{
    public static class VersionUtils
    {
        /// <summary>
        /// Compara duas versões
        /// -1 = versao antiga é menor que versao nova
        /// 0  = mesma versão ou versão não reconhecida
        /// 1  = versão antiga é maior que versao nova (downgrade)
        /// </summary>
        public static int CompareWithPreviousVersion(this string newVersion, string oldVersion)
        {
            Version oldVer, newVer;
            if (newVersion == null || oldVersion == null || !Version.TryParse(oldVersion, out oldVer) || !Version.TryParse(newVersion, out newVer))
            {
                return 0;
            }
            return oldVer.CompareTo(newVer);
        }

        /// <summary>
        /// Compara duas versões
        /// true = Versão nova é maior ou igual a versão antiga, ou alguma das duas é vazia
        /// false = Versão nova é menor que a versão antiga (downgrade)
        /// </summary>
        public static bool CompareWithPreviousVersionSplit(this string newVersion, string oldVersion)
        {
            if (string.IsNullOrEmpty(newVersion) || string.IsNullOrEmpty(oldVersion))
            {
                return true;
            }
            var newVersionArray = newVersion.Split('.');
            var oldVersionArray = oldVersion.Split('.');
            for (var i = 0; i < newVersionArray.Length; i++)
            {
                if (i >= oldVersionArray.Length)
                {
                    break;
                }
                long numNewVersion, numOldVersion;
                if (!long.TryParse(newVersionArray[i], out numNewVersion))
                {
                    continue;
                }
                if (!long.TryParse(oldVersionArray[i], out numOldVersion))
                {
                    continue;
                }
                if (numNewVersion < numOldVersion)
                {
                    return false;
                }
                if (numNewVersion > numOldVersion)
                {
                    return true;
                }
            }
            return true;
        }
    }
}
