# Usando RabbitMQ 

Projeto modelo para novos projetos

## Introdução

Essas instruções fornecerão uma cópia do projeto em execução na sua máquina local para fins de desenvolvimento e teste.
Consulte implantação para obter notas sobre como implantar o projeto em um sistema ativo.

### Prerequisitos

O que você precisa para baixar, rodar e disponibilizar.

* Dotnet core 3.1
* Vagrant
* VirtualBox
* IDE de sua preferência 

### Instalação

Após a execução do pre requisitos, segue um passo a passo de como rodar localmente.

Clonar o repositório

```
git clone git@github.com:robsonpedroso/x.git
```

Rodar o comando `vagrant up` para subir o server com o RabbitMQ instalado
 - IP do servidor `192.168.33.10`
	- Porta do dashboard: 15672
 - Usuário: `admin`
 - Senha: `123`


Abra a solução com o seu IDE (no meu caso Visual Studio) e compile.
 - Pode ser feito pelo bash, terminal ou cmd através do comando `dotnet build`

Abra a solução com o Visual Studio e compile.
Sete o Projeto default como a API e execute (F5).


Chame a URL abaixo pelo navegador para verificar se esta ok.

```
http://localhost:4201/api/v1/ping
```

Se ele retornar ok (conforme exemplo abaixo)

```
{
    "content": "pong",
    "status": "OK",
    "messages": []
}
```
## Diretórios

1. `_docs` - Contem o arquivo Readme.md e caso necessário outras documentações para suporte a execução e manutenção da aplicação.
2. `api` - Projeto da API
3. `core` - Estrutura padrão do DDD contendo os projetos `Application`, `Domain` e `Infra`
4. `tools` - Ferramentas para ajudar no desenvolvimento, no caso foi usado algumas extensions para facilitar a implementação da API e dos retornos.
5. `worker` - Worker para processar as filas

### Padrão de Tecnologia utilizado

Utilizamos o padrão do DDD mais simplificado para trabalhar com os projetos.

Os contratos não são utilizados no projeto de `Application` devido ser 1 por 1, caso haja algum caso que seja necessário fazer uma diferenciação de aplicação, ai sim criamos a interface para essa diferenciação

Já no `Domain` e no `Infra` utilizamos normalmente os contratos (interfaces), pois sabemos que muitas das vezes precisamos modificar os serviços, seja por causa de alguma integração ou ferramenta utilizada que foi necessário mudar o padrão de conexões e chamadas entre elas.

O mapeamento das interfaces são feito automaticamente com reflection para evitar o trabalho e possíveis erros de esquecimento.
Esse reflection se encontra numa extension no projeto `tools/WebApi` e é chamado no startup da aplicação.
Caso seja necessário passar alguma interface externa ou manualmente mesmo, esse metodo aceita um action ficando mais fácil utilizar.

Exemplo da utilização se encontra no Statup (veja abaixo):
```
	services.AddServiceMappingsFromAssemblies<BaseApplication, IBaseService, InfraServices>(srv =>
    {
        srv.AddSingleton<Config>();
    });
```

O retorno da API foi modificado através de um wrapper e filtro no startup da API.
O padrão de conversão do json é `SnakeCaseNamingStrategy`.
Para facilitar a visualização do json de resultado utilizei o [Json Viewer Online](http://jsonviewer.stack.hu/)

## Execução dos testes

Não foi gerado

## Publicação

Não foi gerado

## Versionamento

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/your/project/tags). 

```
Given a version number MAJOR.MINOR.PATCH, increment the:

MAJOR version when you make incompatible API changes,
MINOR version when you add functionality in a backwards compatible manner, and
PATCH version when you make backwards compatible bug fixes.
Additional labels for pre-release and build metadata are available as extensions to the MAJOR.MINOR.PATCH format.
```

## Autores

* **Robson Pedroso** - *Projeto inicial* - [RobsonPedroso](https://github.com/robsonpedroso)

## Licença

[MIT](https://gist.github.com/robsonpedroso/98dc906d5896711f07a9cffbcc2776ea)

## Ferramentas

* [RabbitMQ](https://www.rabbitmq.com/getstarted.html)
* [Vagrant](https://www.vagrantup.com/)
* [Dotnet](https://dotnet.microsoft.com/download)
* [VirtualBox](https://www.virtualbox.org/)
