# CRUD App
Descrição
▪︎ Aplicando console em C# que implementa operações CRUD de usuários com autenticação e armazenamento em arquivos JSON. Ideal para aprendizado e exemplos de arquitetura pequena.

Principais funcionalidades
▪︎ Criar, ler, atualizar e excluir usuários.
▪︎ Autenticação com hashing de senhas.
▪︎ Persistência em arquivos JSON (dados locais).
▪︎ Serviços organizados (ex.: serviço de usuários, serviço de autenticação).

Requisitos
▪︎ .NET 8 SDK ou superior.
▪︎ Sistema operacional: Linux / macOS / Windows

Estrutura do projeto (resumo)
▪︎ Program.cs / Main.cs -> ponto de entrada.
▪︎ Services -> lógica de aplicação (AuthService, UsuarioService, etc).
▪︎ PasswordHasher.cs -> hash de senhas e verificação.
▪︎ Models -> classes de domínio (Usuario, UsuarioAuth, etc.).
▪︎ Data -> arquivos JSON de persistência (ex.: usuários).

Uso 
▪︎ Criar usuários: siga as instruções interativas ao executar a aplicação.
▪︎ Login: use o fluxo de autenticação fornecido pelo AuthService.
▪︎ Local de dados: os usuários são gravados em arquivos JSON dentro da pasta "Data".
