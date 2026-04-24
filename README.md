Adicionar a informação de login com JWT ao `README.md` é uma ótima forma de documentar como a autenticação funciona na sua API.

Aqui está o `README.md` atualizado com a seção de autenticação via JWT.

---

# 🚀 Aplicativo ASP.NET Core (.NET 8)

Esta é uma aplicação API moderna e robusta desenvolvida em **ASP.NET Core** com o **.NET 8**, a versão mais recente e performática da plataforma. Projetada com uma arquitetura que prioriza a escalabilidade, manutenibilidade e desempenho, a solução integra tecnologias de ponta para gerenciar o fluxo de dados, persistência e testes de forma eficiente.

---

### ✨ Visão Geral

A aplicação é construída com o princípio de **CQRS (Command Query Responsibility Segregation)**, utilizando o **MediatR** para desacoplar a lógica de negócio e os comandos/consultas. O login e a autenticação são gerenciados por **JSON Web Tokens (JWT)**, garantindo a segurança das requisições. Os dados são persistidos em um banco de dados **PostgreSQL**, e o **Redis** é empregado para implementar uma camada de _caching_ de alta velocidade. O mapeamento de objetos é facilitado pelo **AutoMapper**, enquanto a qualidade do código é garantida através de testes unitários com **XUnit** e **NSubstitute**.

---

### ⚙️ Tecnologias Utilizadas

- **.NET 8**: A versão mais recente da plataforma .NET, que traz melhorias de performance e novas funcionalidades.
- **ASP.NET Core**: Framework para construir a API.
- **JSON Web Tokens (JWT)**: Padrão para autenticação segura.
- **PostgreSQL**: Banco de dados relacional para persistência de dados.
- **Redis**: Sistema de cache em memória para otimizar o acesso aos dados.
- **MediatR**: Biblioteca para implementação de um padrão de _mediator_, facilitando o CQRS e a comunicação entre componentes.
- **AutoMapper**: Ferramenta para simplificar o mapeamento entre objetos.
- **XUnit**: Framework de testes unitários.
- **NSubstitute**: Framework para criação de _mocks_ e _stubs_ em testes, permitindo simular dependências.
- **Docker Compose**: Ferramenta para orquestrar e gerenciar a execução de múltiplos contêineres Docker, simplificando a configuração do ambiente de desenvolvimento.

---

### 🖥️ Como Rodar Localmente

Com o arquivo `docker-compose.yml` na raiz do projeto, a configuração e execução de todos os serviços se torna um processo simples e consistente.

#### 1\. Pré-requisitos

Certifique-se de que o **Docker** e o **Docker Compose** estão instalados e em execução em sua máquina.

#### 2\. Executando a Aplicação

1.  Abra o terminal na raiz do projeto onde o arquivo `docker-compose.yml` está localizado.
2.  Execute o comando a seguir para construir as imagens e iniciar todos os serviços (API, PostgreSQL, Redis):
    ```bash
    docker-compose up
    ```
    - Este comando lerá o arquivo `docker-compose.yml`, construirá as imagens necessárias (se não existirem) e iniciará os contêineres para cada serviço.

#### Executando em Segundo Plano

Se preferir que os contêineres rodem em segundo plano para liberar o seu terminal, adicione a flag `-d`:

```bash
docker-compose up -d
```

#### Parando os Serviços

Para parar todos os contêineres e remover as redes criadas pelo Docker Compose, use o comando:

```bash
docker-compose down
```

### 🔐 Autenticação com JWT

A API utiliza o padrão JWT para autenticação. Para acessar os endpoints protegidos, o cliente deve seguir os seguintes passos:

1.  Fazer uma requisição `POST` para o endpoint de login, fornecendo as credenciais (nome de usuário/email e senha).
2.  Em caso de sucesso, a API retornará um token JWT.
3.  Incluir este token no cabeçalho `Authorization` de todas as requisições subsequentes para os endpoints protegidos, no formato:
    ```
    Authorization: Bearer [seu_token_jwt]
    ```
    Isso permitirá que a API valide a identidade do utilizador e conceda acesso aos recursos.

### 🧪 Testes

Para executá-los:

1.  Abra o terminal na raiz do projeto.
2.  Execute o comando:
    ```bash
    dotnet test
    ```
    Isso executará todos os testes definidos nos projetos de teste (_XUnit_) e usará _NSubstitute_ para simular as dependências.
