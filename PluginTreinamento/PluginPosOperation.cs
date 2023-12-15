using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginTreinamento
{
    public class PluginPosOperation : IPlugin
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


            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                Entity entidadeContexto = (Entity)context.InputParameters["Target"];

                if (!entidadeContexto.Contains("websiteurl"))
                {
                    throw new InvalidPluginExecutionException("Campo websiteurl principal é obrigatório");
                }
                var Task = new Entity("task");

                Task.Attributes["ownerid"] = new EntityReference("systemuser", context.UserId);
                Task.Attributes["regardingobjectid"] = new EntityReference("account", context.PrimaryEntityId);
                Task.Attributes["subject"] = $"Visite nosso site:{entidadeContexto["websiteurl"]}";
                Task.Attributes["description"] = "TASK CRIADA VIA PLUGIN POST OPERATION";

                serviceAdmin.Create(Task);
                
                
            }


        }
    }
}
