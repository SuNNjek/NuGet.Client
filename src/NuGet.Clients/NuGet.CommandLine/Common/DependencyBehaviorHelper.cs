using System;
using System.Globalization;
using NuGet.Configuration;
using NuGet.Resolver;

namespace NuGet.CommandLine
{
    internal static class DependencyBehaviorHelper
    {
        private static DependencyBehavior TryGetDependencyBehavior(string behaviorStr)
        {
            DependencyBehavior dependencyBehavior;

            if (!Enum.TryParse<DependencyBehavior>(behaviorStr, ignoreCase: true, result: out dependencyBehavior) || !Enum.IsDefined(typeof(DependencyBehavior), dependencyBehavior))
            {
                throw new CommandLineException(string.Format(CultureInfo.CurrentCulture, LocalizedResourceManager.GetString("Error_UnknownDependencyVersion"), behaviorStr));
            }

            return dependencyBehavior;
        }

        public static DependencyBehavior GetDependencyBehavior(DependencyBehavior defaultBehavior,string dependencyVersion, Configuration.ISettings settings)
        {
            // The default dependency behavior when it's not possible to retrieve the behavior from the parameter dependencyVersion or from the setting.
            DependencyBehavior dependencyBehavior = defaultBehavior;

            // Check to see if dependencyVersion parameter is set. Else check for dependencyVersion in .config.
            if (!string.IsNullOrEmpty(dependencyVersion))
            {
                return TryGetDependencyBehavior(dependencyVersion);
            }

            // If the dependencyVersion wasn't provided , try to get the dependencyBehavior from the .config.
            string settingsDependencyVersion = SettingsUtility.GetConfigValue(settings, ConfigurationConstants.DependencyVersion);

            if (!string.IsNullOrEmpty(settingsDependencyVersion))
            {
                dependencyBehavior = TryGetDependencyBehavior(settingsDependencyVersion);
            }

            return dependencyBehavior;
        }

    }
}