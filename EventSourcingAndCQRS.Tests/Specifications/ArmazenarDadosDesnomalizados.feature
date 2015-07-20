#language: pt-BR

Funcionalidade: Armazenar dados desnomalizados
	Como um componente de software
	Desejo armazenar eventos no repositório de eventos

@ArmazenarDadosDesnomalizados
Cenário: Armazenar dados desnomalizados
	Dado que exista um encaminhador de comandos
	E que exista um publicador de comandos
	E que exista um encaminhador de eventos
	E que exista um publicador de eventos
	E que exista um repositório de eventos
	E que exista um serviço consolidador de usuários a partir do repositório de eventos
	E que exista um manipulador de comando de criação de usuários 
	E que o manipulador de comando de criação de usuários tenha sido configurado para receber comandos que sejam do tipo CREATE_USER do encaminhador de comandos 
	E que exista um repositório de contabilizações acerca das informações de contas
	E que exista um serviço contabilizador de total de contas criadas por dia
	Quando for publicado no publicador de comandos o comando CREATE_USER com id 'id1', nome 'JOAO', email 'joao@concert.com.br' e referente ao registro versão '-1'
	E for publicado no publicador de comandos o comando CREATE_USER com id 'id2', nome 'PEDRO', email 'pedro@concert.com.br' e referente ao registro versão '-1'
	E for publicado no publicador de comandos o comando CREATE_USER com id 'id3', nome 'TATIANA', email 'tatiana@concert.com.br' e referente ao registro versão '-1'
	E for publicado no publicador de comandos o comando CREATE_USER com id 'id4', nome 'FERNANDO', email 'fernando@concert.com.br' e referente ao registro versão '-1'
	Então o total de contas criadas no dia de hoje foram 4 contas





