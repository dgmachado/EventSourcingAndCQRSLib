using PostSharp.Patterns.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace EventSourcingAndCQRS.Tests
{
    [Binding]
    [Log(AttributeExclude = true)]
    [LogException(AttributeExclude = true)]
    partial class StepDefinition
    {
        #region :::Give:::

        [Given(@"que exista um encaminhador de comandos")]
        public void DadoQueExistaUmEncaminhadorDeComandos()
        {
            StepsExecutor.InstantiateCommandSubscriber();
        }

        [Given(@"que exista um publicador de comandos")]
        public void DadoQueExistaUmPublicadorDeComandos()
        {
            StepsExecutor.InstantiateCommandPublisher();
        }

        [Given(@"que exista um manipulador de comando de criação de usuários")]
        public void DadoQueExistaUmManipuladorDeComandoDeCriacaoDeUsuarios()
        {
            StepsExecutor.InstantiateCreateUserCommandHandler();
        }

        [Given(@"que exista um repositório de eventos")]
        public void DadoQueExistaUmRepositorioDeEventos()
        {
            StepsExecutor.InstantiateEventStore();
        }

        [Given(@"que exista um serviço consolidador de usuários a partir do repositório de eventos")]
        public void DadoQueExistaUmServicoConsolidadorDeUsuariosAPartirDoRepositorioDeEventos()
        {
            StepsExecutor.InstantiateUserConsolidator();
        }

        [Given(@"que o manipulador de comando de criação de usuários tenha sido configurado para receber comandos que sejam do tipo CREATE_USER do encaminhador de comandos")]
        public void DadoQueOManipuladorDeComandoDeCriacaoDeUsuariosTenhaSidoConfiguradoParaReceberComandosQueSejamDoTipoCREATE_USERDoEncaminhadorDeComandos()
        {
            StepsExecutor.InstantiateCreateUserCommandHandlerSubscriptionId();
            StepsExecutor.ConfigureCreateUserCommandHandlerToSubscribeForCreateUserCommandsFromCommandSubscriber();
        }

        [Given(@"que exista um repositório de contabilizações acerca das informações de contas")]
        public void DadoQueExistaUmRepositorioDeContabilizacoesAcercaDasInformacoesDeContas()
        {
            StepsExecutor.InstantiateDailySummaryAccountsRepository();
        }

        [Given(@"que exista um serviço contabilizador de total de contas criadas por dia")]
        public void DadoQueExistaUmServicoContabilizadorDeTotalDeContasCriadasPorDia()
        {
            StepsExecutor.InstantiateTotalNewAccountsPerDaySumarizer();
            StepsExecutor.ConfigureTotalNewAccountsPerDaySumarizerToSubscribeForUserCreatedFromCommandSubscriber();
        }

        [Given(@"que exista um encaminhador de eventos")]
        public void DadoQueExistaUmEncaminhadorDeEventos()
        {
            StepsExecutor.InstantiateEventSubscriber();
        }

        [Given(@"que exista um publicador de eventos")]
        public void DadoQueExistaUmPublicadorDeEventos()
        {
            StepsExecutor.InstantiateEventPublisher();
        }

        [Given(@"que exista um manipulador de comando de modificação de email de usuários")]
        public void DadoQueExistaUmManipuladorDeComandoDeModificacaoDeEmailDeUsuarios()
        {
            StepsExecutor.InstantiateChangeUserEmailCommandHandler();
        }

        [Given(@"que exista um manipulador de comando de criação de snapshot de usuários")]
        public void DadoQueExistaUmManipuladorDeComandoDeCriacaoDeSnapshotDeUsuarios()
        {
            StepsExecutor.InstantiateTakeUserSnapshotCommandHandler();
        }

        [Given(@"que o manipulador de comando de modificação de email de usuários tenha sido configurado para receber comandos que sejam do tipo CHANGE_USER_EMAIL do encaminhador de comandos")]
        public void DadoQueOManipuladorDeComandoDeModificacaoDeEmailDeUsuariosTenhaSidoConfiguradoParaReceberComandosQueSejamDoTipoCHANGE_USER_EMAILDoEncaminhadorDeComandos()
        {
            StepsExecutor.InstantiateChangeUserEmailCommandHandlerSubscriptionId();
            StepsExecutor.ConfigureChangeUserEmailCommandHandlerToSubscribeForCreateUserCommandsFromCommandSubscriber();
        }

        [Given(@"que o manipulador de comando de criação de snapshot de usuários tenha sido configurado para receber comandos que sejam do tipo TAKE_USER_SNAPSHOT do encaminhador de comandos")]
        public void DadoQueOManipuladorDeComandoDeCriacaoDeSnapshotDeUsuariosTenhaSidoConfiguradoParaReceberComandosQueSejamDoTipoTAKE_USER_SNAPSHOTDoEncaminhadorDeComandos()
        {
            StepsExecutor.InstantiateTakeUserSnapshotCommandHandlerSubscriptionId();
            StepsExecutor.ConfigureTakeUserSnapshotCommandHandlerToSubscribeForTakeUserSnapshotCommandsFromCommandSubscriber();
        }

        [Given(@"que tenha sido publicado no publicador de comandos o comando CREATE_USER com id '(.*)', nome '(.*)', email '(.*)' e referente ao registro versão '(.*)'")]
        public void DadoQueTenhaSidoPublicadoNoPublicadorDeComandosOComandoCREATE_USERComIdNomeEmailEReferenteAoRegistroVersao(string userId, string userName, string userEmail, int version)
        {
            StepsExecutor.SubscribeToCreateUserCommandReceived();
            StepsExecutor.BuildAndPublishCreateUserCommandOnCommandPublisherAndWaitForVerification(userId, userName, userEmail, version);
        }

        [Given(@"o evento USER_CREATED com id '(.*)', nome '(.*)', email '(.*)' e referente ao registro versão '(.*)', disparado a partir da execução do comando CREATE_USER com id '(.*)', nome '(.*)', email '(.*)' e referente ao registro versão '(.*)', esteja salvo no repositório de eventos")]
        public void DadoOEventoUSER_CREATEDComIdNomeEmailEReferenteAoRegistroVersaoDisparadoAPartirDaExecucaoDoComandoCREATE_USERComIdNomeEmailEReferenteAoRegistroVersaoEstejaSalvoNoRepositorioDeEventos(string userCreateUserId, string userCreatedUserName, string userCreatedUserEmail, int userCreatedVersion, string createUserUserId, string createUserUserName, string createUserUserEmail, int createUserVersion)
        {
            StepsExecutor.CreateUserCreatedEventAndSaveOnEventStore(userCreateUserId, userCreatedUserName, userCreatedUserEmail, userCreatedVersion, createUserUserId, createUserUserName, createUserUserEmail, createUserVersion);
        }

        [Given(@"que tenha sido publicado no publicador de comandos o comando CHANGE_USER_EMAIL com id '(.*)', email '(.*)' e referente ao registro versão '(.*)'")]
        public void DadoQueTenhaSidoPublicadoNoPublicadorDeComandosOComandoCHANGE_USER_EMAILComIdEmailEReferenteAoRegistroVersao(string userId, string newUserEmail, int version)
        {
            StepsExecutor.BuildAndPublishChangeUserEmailCommandOnCommandPublisherAndWaitForVerification(userId, newUserEmail, version);
        }

        #endregion

        #region :::When:::
        
        [When(@"o evento USER_CREATED com id '(.*)', nome '(.*)', email '(.*)' e referente ao registro versão '(.*)', disparado a partir da execução do comando CREATE_USER com id '(.*)', nome '(.*)', email '(.*)' e referente ao registro versão '(.*)', for salvo no repositório de eventos")]
        public void QuandoOEventoUSER_CREATEDComIdNomeEmailEReferenteAoRegistroVersaoDisparadoAPartirDaExecucaoDoComandoCREATE_USERComIdNomeEmailEReferenteAoRegistroVersaoForSalvoNoRepositorioDeEventos(string userCreateUserId, string userCreatedUserName, string userCreatedUserEmail, int userCreatedVersion, string createUserUserId, string createUserUserName, string createUserUserEmail, int createUserVersion)
        {
            StepsExecutor.CreateUserCreatedEventAndSaveOnEventStore(userCreateUserId, userCreatedUserName, userCreatedUserEmail, userCreatedVersion, createUserUserId, createUserUserName, createUserUserEmail, createUserVersion);
        }

        [When(@"o evento USER_EMAIL_CHANGED com id '(.*)', nome '(.*)', email '(.*)' e referente ao registro versão '(.*)', disparado a partir da execução do comando CHANGE_EMAIL com id '(.*)', email '(.*)' e referente ao registro versão '(.*)', for salvo no repositório de eventos")]
        public void QuandoOEventoUSER_EMAIL_CHANGEDComIdNomeEmailEReferenteAoRegistroVersaoDisparadoAPartirDaExecucaoDoComandoCHANGE_EMAILComIdEmailEReferenteAoRegistroVersaoForSalvoNoRepositorioDeEventos(string userEmailChangedUserId, string userEmailChangedUserName, string userEmailChangedNewUserEmail, int userEmailChangedVersion, string changeUserEmailUserId, string changeUserEmailNewUserEmail, int changeUserEmailVersion)
        {
            StepsExecutor.CreateUserEmailChangedEventAndSaveOnEventStore(userEmailChangedUserId, userEmailChangedUserName, userEmailChangedNewUserEmail, userEmailChangedVersion, changeUserEmailUserId, changeUserEmailNewUserEmail, changeUserEmailVersion);
        }

        [When(@"for publicado no publicador de comandos o comando CREATE_USER com id '(.*)', nome '(.*)', email '(.*)' e referente ao registro versão '(.*)'")]
        public void QuandoForPublicadoNoPublicadorDeComandosOComandoCREATE_USERComIdNomeEmailEReferenteAoRegistroVersao(string userId, string userName, string userEmail, int version)
        {
            StepsExecutor.SubscribeToCreateUserCommandReceived();
            StepsExecutor.BuildAndPublishCreateUserCommandOnCommandPublisherAndWaitForVerification(userId, userName, userEmail, version);
        }

        [When(@"for publicado no publicador de comandos o comando CHANGE_USER_EMAIL com id '(.*)', email '(.*)' e referente ao registro versão '(.*)'")]
        public void QuandoForPublicadoNoPublicadorDeComandosOComandoCHANGE_USER_EMAILComIdEmailEReferenteAoRegistroVersao(string userId, string newUserEmail, int version)
        {
            StepsExecutor.SubscribeToUserEmailChangedEvent();
            StepsExecutor.BuildAndPublishChangeUserEmailCommandOnCommandPublisherAndWaitForVerification(userId, newUserEmail, version);
        }

        [When(@"for publicado no publicador de comandos o comando TAKE_USER_SNAPSHOT com id '(.*)'")]
        public void QuandoForPublicadoNoPublicadorDeComandosOComandoTAKE_USER_SNAPSHOTComId(string userId)
        {
            StepsExecutor.BuildAndPublishTakeUserSnapshotCommandOnCommandPublisherAndWaitForVerification(userId);
        }

        [When(@"for consultado o usuário de id '(.*)' dentro do repositório de agregado raiz usuário")]
        public void QuandoForConsultadoOUsuarioDeIdDentroDoRepositorioDeAgregadoRaizUsuario(string userId)
        {
            StepsExecutor.FindUserOnRepository(userId);
        }

        #endregion

        #region :::Then:::

        [Then(@"o manipulador de comando receberá o comando CREATE_USER com id '(.*)' nome '(.*)', email '(.*)' e referente ao registro versão '(.*)'")]
        public void EntaoOManipuladorDeComandoReceberaOComandoCREATE_USERComIdNomeEmailEReferenteAoRegistroVersao(string userId, string userName, string userEmail, int version)
        {
            StepsExecutor.AssertThatCommandHandlerReceivedCreateUserCommand(userId, userName, userEmail, version);
        }

        [Then(@"o evento USER_EMAIL_CHANGED com id '(.*)', nome '(.*)', email '(.*)' e referente ao registro versão '(.*)' deve ser publicado no publicador de eventos")]
        public void EntaoOEventoUSER_EMAIL_CHANGEDComIdNomeEmailEReferenteAoRegistroVersaoDeveSerPublicadoNoPublicadorDeEventos(string userId, string userName, string newUserEmail, int version)
        {
            StepsExecutor.AssertThatUserEmailChangedWasPublished(userId, userName, newUserEmail, version);
        }

        [Then(@"o evento USER_EMAIL_CHANGED com id '(.*)', nome '(.*)', email '(.*)' e referente ao registro versão '(.*)' deve estar armazenado no repositório de eventos")]
        public void EntaoOEventoUSER_EMAIL_CHANGEDComIdNomeEmailEReferenteAoRegistroVersaoDeveEstarArmazenadoNoRepositorioDeEventos(string userId, string userName, string newUserEmail, int version)
        {
            StepsExecutor.AssertThatUserEmailChangedEventWasStoredOnEventStore(userId, userName, newUserEmail, version);
        }

        [Then(@"o evento USER_EMAIL_CHANGED com id '(.*)', nome '(.*)', email '(.*)' e referente ao registro versão '(.*)' não deve estar armazenado no repositório de eventos")]
        public void EntaoOEventoUSER_EMAIL_CHANGEDComIdNomeEmailEReferenteAoRegistroVersaoNaoDeveEstarArmazenadoNoRepositorioDeEventos(string userId, string userName, string newUserEmail, int version)
        {
            StepsExecutor.AssertThatUserEmailChangedEventWasnotStoredOnEventStore(userId, userName, newUserEmail, version);
        }

        [Then(@"o evento USER_EMAIL_CHANGED com id '(.*)', nome '(.*)', email '(.*)' e referente ao registro versão '(.*)' não deve ser publicado no publicador de eventos")]
        public void EntaoOEventoUSER_EMAIL_CHANGEDComIdNomeEmailEReferenteAoRegistroVersaoNaoDeveSerPublicadoNoPublicadorDeEventos(string userId, string userName, string newUserEmail, int version)
        {
            StepsExecutor.AssertThatUserEmailChangedWasnotPublishedOnEventPublisher(userId, userName, newUserEmail, version);
        }

        [Then(@"o evento mais atualizado do usuário de id '(.*)' no repositório de eventos deve ser a versão '(.*)'")]
        public void EntaoOEventoMaisAtualizadoDoUsuarioDeIdNoRepositorioDeEventosDeveSerAVersao(string userId, int version)
        {
            StepsExecutor.AssertThatLatestUserEventHasVersion(userId, version);
        }

        [Then(@"o usuário de id '(.*)', nome '(.*)', email '(.*)' e referente ao registro versão '(.*)' deve estar armazenado no repositório de usuários")]
        public void EntaoOUsuarioDeIdNomeEmailEReferenteAoRegistroVersaoDeveEstarArmazenadoNoRepositorioDeUsuarios(string userId, string userName, string userEmail, int version)
        {
            StepsExecutor.AssertThatUserIsSavedRepository(userId, userName, userEmail, version);
        }

        [Then(@"uma exceção do tipo ENTITY_NOT_FOUND deve ser lançada")]
        public void EntaoUmaExcecaoDoTipoENTITY_NOT_FOUNDDeveSerLancada()
        {
            StepsExecutor.AssertThatEntityNotFoundExceptionWasThrew();
        }

        [Then(@"o usuário de id '(.*)' e referente ao registro versão '(.*)' não deve estar armazenado no repositório de usuários")]
        public void EntaoOUsuarioDeIdEReferenteAoRegistroVersaoNaoDeveEstarArmazenadoNoRepositorioDeUsuarios(string userId, int version)
        {
            StepsExecutor.AssertThatUserIsnotSavedRepository(userId, version);
        }

        [Then(@"uma exceção do tipo CONCURRENCY_EXCEPTION deve ser lançada para o id '(.*)'")]
        public void EntaoUmaExcecaoDoTipoCONCURRENCY_EXCEPTIONDeveSerLancadaParaOId(string userId)
        {
            StepsExecutor.AssertThatConcurrencyTransactionExceptionWasThrew(userId);
        }

        [Then(@"o evento USER_CREATED com id '(.*)', nome '(.*)', email '(.*)' e referente ao registro versão '(.*)' deve estar armazenado no repositório de eventos")]
        public void EntaoOEventoUSER_CREATEDComIdNomeEmailEReferenteAoRegistroVersaoDeveEstarArmazenadoNoRepositorioDeEventos(string userId, string userName, string userEmail, int version)
        {
            StepsExecutor.AssertThatUserCreatedEventWasStoredOnEventStore(userId, userName, userEmail, version);
        }

        [Then(@"o evento de snapshot USER_SNAPSHOT com id '(.*)', nome '(.*)', email '(.*)' e referente ao registro versão '(.*)' deve estar armazenado no repositório de eventos")]
        public void EntaoOEventoDeSnapshotUSER_SNAPSHOTComIdNomeEmailEReferenteAoRegistroVersaoDeveEstarArmazenadoNoRepositorioDeEventos(string userId, string userName, string userEmail, int version)
        {
            StepsExecutor.AssertThatUserSnapshotEventWasStoredOnEventStore(userId, userName, userEmail, version);
        }

        [Then(@"o total de contas criadas no dia de hoje foram (.*) contas")]
        public void EntaoOTotalDeContasCriadasNoDiaDeHojeForamContas(int todayTotalNewAccounts)
        {
            StepsExecutor.AssertThatTodayTotalNewAccountsIs(todayTotalNewAccounts);
        }

        #endregion
    }
}
