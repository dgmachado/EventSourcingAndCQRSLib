using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace EventSourcingAndCQRS.Tests
{
    sealed partial class StepDefinition
    {
        private StepsExecutor StepsExecutor { get; set; }

        [BeforeScenario("EnvioDeComandoParaOManipuladorResponsável")]
        [BeforeScenario("OmanipuladorDeComandoAplicaOComandoNaRespectivaEntidadeEndereçadaEarmazenaNoRepositórioDeEventosOsEventosAcarretadosPorTalOperação")]
        [BeforeScenario("ArmazenarEntidadeNoRepositórioDeAgregadoRaiz")]
        [BeforeScenario("ArmazenarEventoNoRepositórioDeEventos")]
        [BeforeScenario("ConsolidarEventosDoRepositórioDeEventos")]
        [BeforeScenario("OcriadorDeVisãoMaterializadaConsolidaAsEntidadesEmUmRepositórioDeLeituraUtilizandoEventosCapturados")]
        [BeforeScenario("ArmazenarDadosDesnomalizados")]
        public void Setup()
        {
            StepsExecutor = new StepsExecutor();
            StepsExecutor.InitializeDependencyResolverContainer();
            StepsExecutor.InstantiateDomainEventsDispatcher();
        }

        [AfterScenario("EnvioDeComandoParaOManipuladorResponsável")]
        [AfterScenario("OmanipuladorDeComandoAplicaOComandoNaRespectivaEntidadeEndereçadaEarmazenaNoRepositórioDeEventosOsEventosAcarretadosPorTalOperação")]
        [AfterScenario("ArmazenarEntidadeNoRepositórioDeAgregadoRaiz")]
        [AfterScenario("ArmazenarEventoNoRepositórioDeEventos")]
        [AfterScenario("ConsolidarEventosDoRepositórioDeEventos")]
        [AfterScenario("OcriadorDeVisãoMaterializadaConsolidaAsEntidadesEmUmRepositórioDeLeituraUtilizandoEventosCapturados")]
        [AfterScenario("ArmazenarDadosDesnomalizados")]
        public void TearDown()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (StepsExecutor != null)
            {
                StepsExecutor.Dispose();
            }
        }
    }
}
