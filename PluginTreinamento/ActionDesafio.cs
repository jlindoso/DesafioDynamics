using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PluginTreinamento
{
    public class ActionDesafio : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context =
                (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            IOrganizationServiceFactory organizationServiceFactory =
                (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

            IOrganizationService serviceAdmin =
                organizationServiceFactory.CreateOrganizationService(null);


            ITracingService trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            var cep = context.InputParameters["CepInput"];
            trace.Trace($"Cep informado: {cep}");

            var viaCepUrl = $"https://viacep.com.br/ws/{cep}/json/";
            var result = string.Empty;
            using(WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Encoding = Encoding.UTF8;
                result = client.DownloadString(viaCepUrl);
            }
            context.OutputParameters["ResultadoCEP"] = result;
            trace.Trace($"Reultado: {result}");

        }
    }
}
