#language: pt-BR

Funcionalidade: Consolidar eventos do repositório de eventos
	Como um componente de software
	Desejo consolidar uma entidade a partir de um conjunto de eventos associados a esse e armazenados dentro do repositório de eventos
	
@ConsolidarEventosDoRepositórioDeEventos
Cenário: Consolidar uma entidade a partir de um conjunto de eventos associados a esse e armazenados dentro do repositório de eventos
	Dado que exista um repositório de eventos
	E que exista um serviço consolidador de usuários a partir do repositório de eventos
	Quando o evento USER_CREATED com id 'id1', nome 'JOAO', email 'joao@microsoft.com.br' e referente ao registro versão '0', disparado a partir da execução do comando CREATE_USER com id 'id1', nome 'JOAO', email 'joao@microsoft.com.br' e referente ao registro versão '-1', for salvo no repositório de eventos
	E o evento USER_EMAIL_CHANGED com id 'id1', nome 'JOAO', email 'joao@ibm.com.br' e referente ao registro versão '1', disparado a partir da execução do comando CHANGE_EMAIL com id 'id1', email 'joao@ibm.com.br' e referente ao registro versão '0', for salvo no repositório de eventos
	E o evento USER_EMAIL_CHANGED com id 'id1', nome 'JOAO', email 'joao@concert.com.br' e referente ao registro versão '2', disparado a partir da execução do comando CHANGE_EMAIL com id 'id1', email 'joao@concert.com.br' e referente ao registro versão '1', for salvo no repositório de eventos
	Então o usuário de id 'id1', nome 'JOAO', email 'joao@concert.com.br' e referente ao registro versão '2' deve estar armazenado no repositório de usuários

@ConsolidarEventosDoRepositórioDeEventos
Cenário: Consolidar uma entidade com ausência de eventos associados a esse e armazenados dentro do repositório de eventos
	Dado que exista um repositório de eventos
	E que exista um serviço consolidador de usuários a partir do repositório de eventos
	Quando for consultado o usuário de id 'id1' dentro do repositório de agregado raiz usuário
	Então uma exceção do tipo ENTITY_NOT_FOUND deve ser lançada

@ConsolidarEventosDoRepositórioDeEventos
Cenário: Consolidar entidades distintas a partir de um conjunto de eventos associados a esses e armazenados dentro do repositório de eventos
	Dado que exista um repositório de eventos
	E que exista um serviço consolidador de usuários a partir do repositório de eventos
	Quando o evento USER_CREATED com id 'id1', nome 'JOAO', email 'joao@microsoft.com.br' e referente ao registro versão '0', disparado a partir da execução do comando CREATE_USER com id 'id1', nome 'JOAO', email 'joao@microsoft.com.br' e referente ao registro versão '-1', for salvo no repositório de eventos
	E o evento USER_CREATED com id 'id2', nome 'PEDRO', email 'pedro@concert.com.br' e referente ao registro versão '0', disparado a partir da execução do comando CREATE_USER com id 'id2', nome 'PEDRO', email 'pedro@microsoft.com.br' e referente ao registro versão '-1', for salvo no repositório de eventos
	E o evento USER_EMAIL_CHANGED com id 'id1', nome 'JOAO', email 'joao@concert.com.br' e referente ao registro versão '1', disparado a partir da execução do comando CHANGE_EMAIL com id 'id1', email 'joao@concert.com.br' e referente ao registro versão '0', for salvo no repositório de eventos
	Então o usuário de id 'id1', nome 'JOAO', email 'joao@concert.com.br' e referente ao registro versão '1' deve estar armazenado no repositório de usuários
	E o usuário de id 'id2', nome 'PEDRO', email 'pedro@concert.com.br' e referente ao registro versão '0' deve estar armazenado no repositório de usuários

