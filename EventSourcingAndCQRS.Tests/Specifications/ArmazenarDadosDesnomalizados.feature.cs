﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.34209
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace EventSourcingAndCQRS.Tests.Specifications
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Armazenar dados desnomalizados")]
    public partial class ArmazenarDadosDesnomalizadosFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "ArmazenarDadosDesnomalizados.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("pt-BR"), "Armazenar dados desnomalizados", "Como um componente de software\r\nDesejo armazenar eventos no repositório de evento" +
                    "s", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Armazenar dados desnomalizados")]
        [NUnit.Framework.CategoryAttribute("ArmazenarDadosDesnomalizados")]
        public virtual void ArmazenarDadosDesnomalizados()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Armazenar dados desnomalizados", new string[] {
                        "ArmazenarDadosDesnomalizados"});
#line 8
this.ScenarioSetup(scenarioInfo);
#line 9
 testRunner.Given("que exista um encaminhador de comandos", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Dado ");
#line 10
 testRunner.And("que exista um publicador de comandos", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 11
 testRunner.And("que exista um encaminhador de eventos", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 12
 testRunner.And("que exista um publicador de eventos", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 13
 testRunner.And("que exista um repositório de eventos", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 14
 testRunner.And("que exista um serviço consolidador de usuários a partir do repositório de eventos" +
                    "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 15
 testRunner.And("que exista um manipulador de comando de criação de usuários", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 16
 testRunner.And("que o manipulador de comando de criação de usuários tenha sido configurado para r" +
                    "eceber comandos que sejam do tipo CREATE_USER do encaminhador de comandos", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 17
 testRunner.And("que exista um repositório de contabilizações acerca das informações de contas", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 18
 testRunner.And("que exista um serviço contabilizador de total de contas criadas por dia", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 19
 testRunner.When("for publicado no publicador de comandos o comando CREATE_USER com id \'id1\', nome " +
                    "\'JOAO\', email \'joao@concert.com.br\' e referente ao registro versão \'-1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Quando ");
#line 20
 testRunner.And("for publicado no publicador de comandos o comando CREATE_USER com id \'id2\', nome " +
                    "\'PEDRO\', email \'pedro@concert.com.br\' e referente ao registro versão \'-1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 21
 testRunner.And("for publicado no publicador de comandos o comando CREATE_USER com id \'id3\', nome " +
                    "\'TATIANA\', email \'tatiana@concert.com.br\' e referente ao registro versão \'-1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 22
 testRunner.And("for publicado no publicador de comandos o comando CREATE_USER com id \'id4\', nome " +
                    "\'FERNANDO\', email \'fernando@concert.com.br\' e referente ao registro versão \'-1\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "E ");
#line 23
 testRunner.Then("o total de contas criadas no dia de hoje foram 4 contas", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Então ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
