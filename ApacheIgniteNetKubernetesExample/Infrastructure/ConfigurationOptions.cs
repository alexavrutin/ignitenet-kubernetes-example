using System;
using System.Collections.Generic;
using System.Text;

namespace UtilityClick.ProductService.Infrastructure
{
    /// <summary>
    /// Defines the schema for service's configuration options. 
    /// </summary>
    public class ConfigurationOptions
    {
        public ConfigurationOptions()
        {

        }
        /// <summary>
        /// Connection string for the storage account used by the service.
        /// </summary>
        public string AzureStorageAccount { get; set; }

        /// <summary>
        /// Connection string for the service bus. 
        /// </summary>
        public string ServiceBusConnectionString { get; set; }

        /// <summary>
        /// Topic where the ProductService has to publish its events.
        /// </summary>
        public string ServiceBusTopicName { get; set; }

        /// <summary>
        /// Container for the temporary files. 
        /// </summary>
        public string TempFileContainerName { get; set; }

        public string DefaultLoggerName
        {
            get
            {
                return Program.SERVICE_NAME;
            }
        }

        public string Environment { get; set; }

        /// <summary>
        /// Time-to-live for uploaded but uncommitted files (in minutes).
        /// </summary>
        public int TempFileTtlMin { get; set; }

        public QuoteServiceOptions QuoteService { get; set; }

        public class QuoteServiceOptions
        {
            public string Name { get; set; }

            public string FlatFileManagerName { get; set; }

            public string HhMatrixFileManagerName { get; set; }

            public string NhhMatrixFileManagerName { get; set; }
        }
    }
}
