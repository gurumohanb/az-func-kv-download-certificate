using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using Azure;
using System.Security.Cryptography.X509Certificates;

namespace BGW_BNP_CERT_TEST
{
    public static class bgw_bnp_get_pem_cert
    {
        [FunctionName("bgw_bnp_get_pem_cert")]
        public static async Task<Response<X509Certificate2>> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {


            log.LogInformation("Reading certificate from Azure Key Vault.");

            string keyVaultName = Environment.GetEnvironmentVariable("KEY_VAULT_NAME");
            string certificateName = Environment.GetEnvironmentVariable("CERTIFICATE_NAME");

            var kvUri = $"https://{keyVaultName}.vault.azure.net/";

            var credential = new DefaultAzureCredential();

            log.LogInformation("Initialize the certificate client");

            var certificateClient = new CertificateClient(new Uri(kvUri), credential);

            log.LogInformation("Retrieve the certificate");

            try
            {
                return await certificateClient.DownloadCertificateAsync(certificateName);
            }
            catch (Exception ex)
            {
                log.LogError(certificateName, ex);
                throw;
            }
            //var certificate = certificateResponse.Value;

            //return new OkObjectResult(new
            //{
            //    CertificateName = certificate.Name,
            //    Thumbprint = certificate.Properties.X509Thumbprint
            //});
        }
    }
}
