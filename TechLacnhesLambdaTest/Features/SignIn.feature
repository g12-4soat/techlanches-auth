#language: pt-BR

Funcionalidade: Sign in de usuário

Cenário: Autenticando um usuário
	Dado que os dados do usuário são válido
	Quando autentica o usuário 
	Entao retorna o resultado sucesso na autenticação

Cenário: Falha ao tentar autenticar um usuário não confirmado
	Dado que os dados do usuário são válido
	Quando autentica o usuário não confirmado
	Entao retorna o resultado falha por usuário não confirmado

Cenário: Falha ao tentar autenticar um usuário não autorizado
	Dado que os dados do usuário são válido
	Quando autentica o usuário não autorizado
	Entao retorna o resultado falha por usuário não autorizado

Cenário: Falha ao tentar autenticar um usuário não encontrado
	Dado que os dados do usuário são válido
	Quando autentica o usuário não encontrado
	Entao retorna o resultado falha por usuário não encontrado

Cenário: Retorno da autenticação de usuário inválido
	Dado que os dados do usuário são válido
	Quando autentica o usuário inválido
	Entao retorna o resultado falha na autenticação

#Exemplo: 
#    | Cpf   | Email              | Nome         |
#	| 958.315.760-00  | test@example.com  | Test User |