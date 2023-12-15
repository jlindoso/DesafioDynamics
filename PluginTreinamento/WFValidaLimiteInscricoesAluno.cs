using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using Microsoft.Xrm.Sdk;

namespace PluginTreinamento
{
    public class WFValidaLimiteInscricoesAluno : CodeActivity
    {
        #region Parameters

        [Input("Usuario")]
        [ReferenceTarget("systemuser")]
        public InArgument<EntityReference> UsuarioEntrada { get; set; }

        [Input("AlunoXCursoDisponivel")]
        [ReferenceTarget("curso_alunoxcursodisponivel")]
        public InArgument<EntityReference> RegistroContexto { get; set; }

        [Output("saida")]
        public OutArgument<string> saida { get; set; }

        #endregion
        protected override void Execute(CodeActivityContext executionContext)
        {
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService organizationService = serviceFactory.CreateOrganizationService(context.UserId);
            ITracingService trace = executionContext.GetExtension<ITracingService>();

            Guid id = context.PrimaryEntityId;

            trace.Trace($"Id Aluno: {id}");

            Entity alunoCurso = organizationService.Retrieve("curso_alunoxcursodisponivel", id, new Microsoft.Xrm.Sdk.Query.ColumnSet("curso_cursoselecionado", "dio_periodo", "dio_datainicio"));

            Guid idCurso = Guid.Empty;
            var periodo = string.Empty;

            var dataInicio = new DateTime();

            idCurso = ((EntityReference)alunoCurso.Attributes["curso_cursoselecionado"]).Id;

            if (alunoCurso.Attributes.Contains("dio_periodo"))
            {
                periodo = ((OptionSetValue)alunoCurso["dio_periodo"]).Value == 914300000 ? "DIURNO" : "NOTURNO";
            }
            if (alunoCurso.Attributes.Contains("dio_datainicio"))
            {
                dataInicio  = (DateTime)alunoCurso["dio_datainicio"];


            }
            if (idCurso != Guid.Empty)
            {
                Entity cursos = organizationService.Retrieve("curso_cursosdisponiveis", idCurso, new Microsoft.Xrm.Sdk.Query.ColumnSet("dio_duracao"));
                int horasDuracao = 0;
                if(cursos!=null && cursos.Attributes.Contains("dio_duracao"))
                {
                    horasDuracao = Convert.ToInt32(cursos.Attributes["dio_duracao"].ToString());
                }
                var diasNescessarios = 0;
                diasNescessarios = periodo.Equals("DIURNO")? horasDuracao / 8: horasDuracao / 4;
                if (diasNescessarios > 0)
                {
                    for(int i = 0; i<diasNescessarios; i++)
                    {
                        if(dataInicio.ToString("ddd").Equals("Sat") && periodo.Equals("NOTURNO"))
                        {
                            dataInicio = dataInicio.AddDays(2);
                        }
                        Entity calendario = new Entity("dio_calendario");
                        calendario["dio_name"] = $"Aula do dia {dataInicio}";
                        calendario["dio_data"] = dataInicio;
                        calendario["dio_alunoxcursodisponivel"] = new EntityReference("curso_alunoxcursodisponivel", id);
                        organizationService.Create(calendario);
                        dataInicio = dataInicio.AddDays(1);
                    }
                }
            }
        }
    }
}
