#language: pt-BR

Funcionalidade: O manipulador de comando aplica o comando na respectiva entidade endereçada e armazena no repositório de eventos os eventos acarretados por tal operação
	Como um componente de software
	Desejo enviar um comando para o manipulador responsável pelo mesmo
	Para que o manipulador de comando responsável possa receber meu comando e aplicá-lo na respectiva entidade endereçada e salvar os eventos acarretados por tal operação
	
@OmanipuladorDeComandoAplicaOComandoNaRespectivaEntidadeEndereçadaEarmazenaNoRepositórioDeEventosOsEventosAcarretadosPorTalOperação
Cenário: O manipulador de comando aplica o comando na versão esperada da respectiva entidade endereçada e armazena no repositório de eventos os eventos acarretados por tal operação
	Dado que exista um encaminhador de comandos
	E que exista um publicador de comandos
	E que exista um encaminhador de eventos
	E que exista um publicador de eventos
	E que exista um repositório de eventos
	E que exista um serviço consolidador de usuários a partir do repositório de eventos
	E que exista um manipulador de comando de criação de usuários 
	E que exista um manipulador de comando de modificação de email de usuários 
	E que o manipulador de comando de criação de usuários tenha sido configurado para receber comandos que sejam do tipo CREATE_USER do encaminhador de comandos 
	E que o manipulador de comando de modificação de email de usuários tenha sido configurado para receber comandos que sejam do tipo CHANGE_USER_EMAIL do encaminhador de comandos 
	E que tenha sido publicado no publicador de comandos o comando CREATE_USER com id 'id1', nome 'JOAO', email 'joao@microsoft.com.br' e referente ao registro versão '-1'
	Quando for publicado no publicador de comandos o comando CHANGE_USER_EMAIL com id 'id1', email 'joao@concert.com.br' e referente ao registro versão '0' 
	Então o evento USER_EMAIL_CHANGED com id 'id1', nome 'JOAO', email 'joao@concert.com.br' e referente ao registro versão '1' deve ser publicado no publicador de eventos
	E o evento USER_EMAIL_CHANGED com id 'id1', nome 'JOAO', email 'joao@concert.com.br' e referente ao registro versão '1' deve estar armazenado no repositório de eventos 
	E o evento mais atualizado do usuário de id 'id1' no repositório de eventos deve ser a versão '1'

@OmanipuladorDeComandoAplicaOComandoNaRespectivaEntidadeEndereçadaEarmazenaNoRepositórioDeEventosOsEventosAcarretadosPorTalOperação
Cenário: O manipulador de comando aplica o comando na versão superior da respectiva entidade endereçada 
	Dado que exista um encaminhador de comandos
	E que exista um publicador de comandos
	E que exista um encaminhador de eventos
	E que exista um publicador de eventos
	E que exista um repositório de eventos
	E que exista um serviço consolidador de usuários a partir do repositório de eventos
	E que exista um manipulador de comando de criação de usuários 
	E que exista um manipulador de comando de modificação de email de usuários 
	E que o manipulador de comando de criação de usuários tenha sido configurado para receber comandos que sejam do tipo CREATE_USER do encaminhador de comandos 
	E que o manipulador de comando de modificação de email de usuários tenha sido configurado para receber comandos que sejam do tipo CHANGE_USER_EMAIL do encaminhador de comandos 
	E que tenha sido publicado no publicador de comandos o comando CREATE_USER com id 'id1', nome 'JOAO', email 'joao@microsoft.com.br' e referente ao registro versão '-1'
	Quando for publicado no publicador de comandos o comando CHANGE_USER_EMAIL com id 'id1', email 'joao@concert.com.br' e referente ao registro versão '2' 
	Então o evento USER_EMAIL_CHANGED com id 'id1', nome 'JOAO', email 'joao@concert.com.br' e referente ao registro versão '3' não deve ser publicado no publicador de eventos
	E o evento USER_EMAIL_CHANGED com id 'id1', nome 'JOAO', email 'joao@concert.com.br' e referente ao registro versão '3' não deve estar armazenado no repositório de eventos 
	E o evento mais atualizado do usuário de id 'id1' no repositório de eventos deve ser a versão '0'

@OmanipuladorDeComandoAplicaOComandoNaRespectivaEntidadeEndereçadaEarmazenaNoRepositórioDeEventosOsEventosAcarretadosPorTalOperação
Cenário: O manipulador de comando aplica o comando na versão inferior da respectiva entidade endereçada 
	Dado que exista um encaminhador de comandos
	E que exista um publicador de comandos
	E que exista um encaminhador de eventos
	E que exista um publicador de eventos
	E que exista um repositório de eventos
	E que exista um serviço consolidador de usuários a partir do repositório de eventos
	E que exista um manipulador de comando de criação de usuários 
	E que exista um manipulador de comando de modificação de email de usuários 
	E que o manipulador de comando de criação de usuários tenha sido configurado para receber comandos que sejam do tipo CREATE_USER do encaminhador de comandos 
	E que o manipulador de comando de modificação de email de usuários tenha sido configurado para receber comandos que sejam do tipo CHANGE_USER_EMAIL do encaminhador de comandos 
	E que tenha sido publicado no publicador de comandos o comando CREATE_USER com id 'id1', nome 'JOAO', email 'joao@microsoft.com.br' e referente ao registro versão '-1'
	Quando for publicado no publicador de comandos o comando CHANGE_USER_EMAIL com id 'id1', email 'joao@concert.com.br' e referente ao registro versão '-1' 
	Então o evento USER_EMAIL_CHANGED com id 'id1', nome 'JOAO', email 'joao@concert.com.br' e referente ao registro versão '0' não deve ser publicado no publicador de eventos
	E o evento USER_EMAIL_CHANGED com id 'id1', nome 'JOAO', email 'joao@concert.com.br' e referente ao registro versão '0' não deve estar armazenado no repositório de eventos 
	E o evento mais atualizado do usuário de id 'id1' no repositório de eventos deve ser a versão '0'
		