
# API ASP.NET com Clean Architecture e TDD
Este é um projeto de API ASP.NET desenvolvido com o propósito de estudar Test-Driven Development (TDD) e implementar os princípios da Clean Architecture. A aplicação é simples e possui uma única entidade chamada "Tarefa". Utiliza um banco de dados SQLite para persistência e apresenta testes unitários abrangendo as diferentes camadas da arquitetura.

## Estrutura do Projeto
O projeto segue uma estrutura de Clean Architecture, que promove a separação clara das responsabilidades em diferentes camadas:

* Domain: Contém a definição da entidade Tarefa e quaisquer lógicas de negócio relacionadas a ela.
* Application: Responsável por orquestrar as operações da aplicação, incluindo a manipulação das entidades e a lógica de aplicação específica.
* Infrastructure: Lida com detalhes de implementação, como acesso a banco de dados e outros recursos externos. Neste projeto, utiliza SQLite como banco de dados.
* Presentation: Camada responsável por expor os endpoints da API e lidar com as requisições HTTP. O TarefaController está localizado aqui.

## Requisitos
.NET Core SDK instalado (versão 7)
Visual Studio ou Visual Studio Code para desenvolvimento
Configuração e Execução
Clone o repositório para sua máquina local.
Certifique-se de ter o .NET Core SDK instalado.
Navegue até o diretório raiz do projeto e execute o comando dotnet build para compilar o projeto.
Execute dotnet run para iniciar a aplicação.
A API estará acessível em http://localhost:5002.

## Testes
Os testes unitários são uma parte essencial deste projeto, garantindo que cada componente da aplicação seja testado de forma isolada e eficaz. Os testes são organizados de acordo com as camadas da arquitetura:

* Domain: Testes para a entidade Tarefa e quaisquer lógicas de negócio relacionadas.
* Application: Testes para o serviço TarefaService que implementa as operações relacionadas a Tarefa.
* Presentation: Testes para o TarefaController, garantindo que os endpoints da API funcionem conforme o esperado.
Para executar os testes, utilize o comando dotnet test.

## Contribuição
Este projeto é destinado principalmente para fins educacionais, para praticar conceitos como TDD e Clean Architecture. No entanto, contribuições são bem-vindas. Se você encontrar problemas ou tiver sugestões de melhorias, sinta-se à vontade para abrir uma issue ou enviar um pull request.
