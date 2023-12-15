using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginTreinamento
{
    public class PluginAccountPreOperation : IPlugin
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

           
            if (context.InputParameters.Contains("Target")&& context.InputParameters["Target"] is Entity)
            {
                Entity entidadeContexto = (Entity)context.InputParameters["Target"];
                if (entidadeContexto.LogicalName == "account")
                {
                    if (entidadeContexto.Attributes.Contains("telephone1"))
                    {
                        var phone1 = entidadeContexto["telephone1"].ToString();

                        var FetchContact = $"<?xml version='1.0'?>" +
                                               " <fetch distinct='false' mapping='logical' output-format='xml-platform' version='1.0'>" +
                                               " <entity name='contact'>" +
                                               " <attribute name='fullname'/>" +
                                               " <attribute name='telephone1'/>" +
                                               " <attribute name='contactid'/>" +
                                               " <order descending='false' attribute='fullname'/>" +
                                               " <filter type='and'>" +
                                               $" <condition attribute='telephone1' value='{phone1}' operator='eq' />" +
                                               "</filter>" +
                                               "</entity>" +
                                               "</fetch>";

                        trace.Trace($"FetchContact: {FetchContact}");
                        var primaryContact = serviceAdmin.RetrieveMultiple(new FetchExpression(FetchContact));
                        if (primaryContact.Entities.Count > 0)
                        {
                            foreach(var entityContact in primaryContact.Entities)
                            {
                                entidadeContexto["primarycontactid"] = new EntityReference("contact", entityContact.Id);
                            }
                        }
                    }
                }

            }
        
        
        
        }
    }
}
