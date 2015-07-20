#language: pt-BR

Funcionalidade: Armazenar eventos no repositório de eventos
	Como um componente de software
	Desejo armazenar eventos no repositório de eventos

@ArmazenarEventoNoRepositórioDeEventos
Cenário: Armazenar novo evento com versão esperada
	Dado que exista um repositório de eventos
	E o evento USER_CREATED com id 'id1', nome 'JOAO', email 'joao@microsoft.com.br' e referente ao registro versão '0', disparado a partir da execução do comando CREATE_USER com id 'id1', nome 'JOAO', email 'joao@microsoft.com.br' e referente ao registro versão '-1', esteja salvo no repositório de eventos
	Quando o evento USER_EMAIL_CHANGED com id 'id1', nome 'JOAO', email 'joao@microsoft.com.br' e referente ao registro versão '1', disparado a partir da execução do comando CHANGE_EMAIL com id 'id1', email 'joao@microsoft.com.br' e referente ao registro versão '0', for salvo no repositório de eventos
	Então o evento mais atualizado do usuário de id 'id1' no repositório de eventos deve ser a versão '1'

@ArmazenarEventoNoRepositórioDeEventos
Cenário: Armazenar novo evento com versão superior
	Dado que exista um repositório de eventos
	E o evento USER_CREATED com id 'id1', nome 'JOAO', email 'joao@microsoft.com.br' e referente ao registro versão '0', disparado a partir da execução do comando CREATE_USER com id 'id1', nome 'JOAO', email 'joao@microsoft.com.br' e referente ao registro versão '-1', esteja salvo no repositório de eventos
	Quando o evento USER_EMAIL_CHANGED com id 'id1', nome 'JOAO', email 'joao@microsoft.com.br' e referente ao registro versão '2', disparado a partir da execução do comando CHANGE_EMAIL com id 'id1', email 'joao@microsoft.com.br' e referente ao registro versão '1', for salvo no repositório de eventos
	Então o evento USER_EMAIL_CHANGED com id 'id1', nome 'JOAO', email 'joao@microsoft.com.br' e referente ao registro versão '2' não deve estar armazenado no repositório de eventos 
	E uma exceção do tipo CONCURRENCY_EXCEPTION deve ser lançada para o id 'id1'
	
@ArmazenarEventoNoRepositórioDeEventos
Cenário: Armazenar novo evento com versão inferior
	Dado que exista um repositório de eventos
	E o evento USER_CREATED com id 'id1', nome 'JOAO', email 'joao@microsoft.com.br' e referente ao registro versão '0', disparado a partir da execução do comando CREATE_USER com id 'id1', nome 'JOAO', email 'joao@microsoft.com.br' e referente ao registro versão '-1', esteja salvo no repositório de eventos
	Quando o evento USER_EMAIL_CHANGED com id 'id1', nome 'JOAO', email 'joao@microsoft.com.br' e referente ao registro versão '-1', disparado a partir da execução do comando CHANGE_EMAIL com id 'id1', email 'joao@microsoft.com.br' e referente ao registro versão '-1', for salvo no repositório de eventos
	Então o evento USER_EMAIL_CHANGED com id 'id1', nome 'JOAO', email 'joao@microsoft.com.br' e referente ao registro versão '-1' não deve estar armazenado no repositório de eventos 
	E uma exceção do tipo CONCURRENCY_EXCEPTION deve ser lançada para o id 'id1'

@ArmazenarEventoNoRepositórioDeEventos
Cenário: Armazenar snapshot da entidade no repositório de eventos 
	Dado que exista um repositório de eventos
	E que exista um serviço consolidador de usuários a partir do repositório de eventos
	E que exista um encaminhador de eventos
	E que exista um publicador de eventos
	E que exista um encaminhador de comandos
	E que exista um publicador de comandos	
	E que exista um manipulador de comando de criação de usuários 
	E que exista um manipulador de comando de modificação de email de usuários 
	E que exista um manipulador de comando de criação de snapshot de usuários 
	E que o manipulador de comando de criação de usuários tenha sido configurado para receber comandos que sejam do tipo CREATE_USER do encaminhador de comandos 
	E que o manipulador de comando de modificação de email de usuários tenha sido configurado para receber comandos que sejam do tipo CHANGE_USER_EMAIL do encaminhador de comandos 
	E que o manipulador de comando de criação de snapshot de usuários tenha sido configurado para receber comandos que sejam do tipo TAKE_USER_SNAPSHOT do encaminhador de comandos 
	E que tenha sido publicado no publicador de comandos o comando CREATE_USER com id 'id1', nome 'JOAO', email 'joao@microsoft.com.br' e referente ao registro versão '-1'
	E que tenha sido publicado no publicador de comandos o comando CHANGE_USER_EMAIL com id 'id1', email 'joao@concert.com.br' e referente ao registro versão '0'
	E que tenha sido publicado no publicador de comandos o comando CHANGE_USER_EMAIL com id 'id1', email 'joao.machado@concert.com.br' e referente ao registro versão '1'
	E que tenha sido publicado no publicador de comandos o comando CHANGE_USER_EMAIL com id 'id1', email 'joaosm@concert.com.br' e referente ao registro versão '2'
	Quando for publicado no publicador de comandos o comando TAKE_USER_SNAPSHOT com id 'id1'
	Então o usuário de id 'id1', nome 'JOAO', email 'joaosm@concert.com.br' e referente ao registro versão '4' deve estar armazenado no repositório de usuários
	E o evento USER_CREATED com id 'id1', nome 'JOAO', email 'joao@microsoft.com.br' e referente ao registro versão '0' deve estar armazenado no repositório de eventos 
	E o evento USER_EMAIL_CHANGED com id 'id1', nome 'JOAO', email 'joao@concert.com.br' e referente ao registro versão '1' deve estar armazenado no repositório de eventos 
	E o evento USER_EMAIL_CHANGED com id 'id1', nome 'JOAO', email 'joao.machado@concert.com.br' e referente ao registro versão '2' deve estar armazenado no repositório de eventos 
	E o evento USER_EMAIL_CHANGED com id 'id1', nome 'JOAO', email 'joaosm@concert.com.br' e referente ao registro versão '3' deve estar armazenado no repositório de eventos 
	E o evento de snapshot USER_SNAPSHOT com id 'id1', nome 'JOAO', email 'joaosm@concert.com.br' e referente ao registro versão '4' deve estar armazenado no repositório de eventos 















