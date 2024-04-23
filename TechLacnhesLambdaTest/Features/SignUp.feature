#language: pt-BR
Funcionalidade: Sign up de usuário

#Cenário: Usuáro válido deve ser cadastrado
Cenário: Cadastrando um usuário
	Dado que os dados do usuário são válidos 
	E seja diferente do usuário techlanches
	E seja inexistente
	Quando cadastra o usuário 
	E confirma o usuário
	Entao retorna o resultado sucesso

Cenário: Usuário já cadastrado
	Dado que os dados do usuário são válidos 
	E seja diferente do usuário techlanches
	E seja existente
	Quando cadastra o usuário
	Entao retorna o resultado falha de usuário existente

Cenário: Retorno do cadastro de usuário inválido
	Dado que os dados do usuário são válidos 
	E seja diferente do usuário techlanches
	E seja inexistente
	Quando o retorno do cadastro de usuário é diferente do status code OK
	Entao retorna o resultado falha por status code

Cenário: Falha ao tentar cadastrar um usuário
	Dado que os dados do usuário são válidos 
	E seja diferente do usuário techlanches
	E seja inexistente
	Quando houver falha ao cadastrar o usuário 
	Entao retorna o resultado falha

Cenário: Falha ao tentar confirmar usuário
	Dado que os dados do usuário são válidos 
	E seja diferente do usuário techlanches
	E seja inexistente
	E o usuário esteja cadastrado
	Quando houver falha ao confirmar o usuário 
	Entao retorna o resultado falha na confirmação do usuário