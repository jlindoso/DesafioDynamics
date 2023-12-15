using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace PluginTreinamento
{
    public  class PluginAccoundPreValidation: IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = 
                (IPluginExecutionContext) serviceProvider.GetService(typeof(IPluginExecutionContext));

            IOrganizationServiceFactory organizationServiceFactory = 
                (IOrganizationServiceFactory) serviceProvider.GetService(typeof(IOrganizationServiceFactory));

            IOrganizationService serviceAdmin = 
                organizationServiceFactory.CreateOrganizationService(null);


            ITracingService trace = (ITracingService) serviceProvider.GetService(typeof(ITracingService));

            Entity entidadeContexto = null;

            if (context.InputParameters.Contains("Target"))
            {
                entidadeContexto = (Entity)context.InputParameters["Target"];
                trace.Trace($"Entidade do contexto: {entidadeContexto.Attributes.Count}");
                if(entidadeContexto == null)
                {
                    return;
                }
                if (!entidadeContexto.Contains("telephone1"))
                {
                    throw new InvalidPluginExecutionException("O telefone principal é obrigatório");
                }

            }

        }
    }
}
