using Microsoft.Extensions.Configuration;

namespace Franklin.Tests {
    public class TestHelper {

        /// <summary>
        /// Return configuration from config file or build own.
        /// </summary>
        /// <param name="outputPath"></param>
        /// <returns></returns>
        public static IConfigurationRoot GetIConfigurationRoot(string configPath) {

            return new ConfigurationBuilder()
                    .AddJsonFile("appSettings.test.json") // Picks up the file without explicitly specifying the path.
                    .Build();
        }

    }
}
