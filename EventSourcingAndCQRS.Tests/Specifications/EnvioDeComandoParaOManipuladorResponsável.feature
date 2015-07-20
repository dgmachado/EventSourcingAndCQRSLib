#language: pt-BR

Funcionalidade: Envio de comando para o manipulador responsável
	Como um componente de software
	Desejo enviar um comando para o manipulador responsável pelo mesmo
	Para que o manipulador de comando responsável possa executar operações em relação ao pedido realizado
	
@EnvioDeComandoParaOManipuladorResponsável
Cenário: Envio de comando para o manipulador responsável
	Dado que exista um encaminhador de comandos 
	E que exista um publicador de comandos
	E que exista um repositório de eventos
	E que exista um serviço consolidador de usuários a partir do repositório de eventos
	E que exista um manipulador de comando de criação de usuários 
	E que o manipulador de comando de criação de usuários tenha sido configurado para receber comandos que sejam do tipo CREATE_USER do encaminhador de comandos  
	Quando for publicado no publicador de comandos o comando CREATE_USER com id 'id1', nome 'JOAO', email 'joao@concert.com.br' e referente ao registro versão '-1'
	Então o manipulador de comando receberá o comando CREATE_USER com id 'id1' nome 'JOAO', email 'joao@concert.com.br' e referente ao registro versão '-1'
	
