# CRUD App
Aplicação de console em C# que implementa operações CRUD de usuários com autenticação segura e armazenamento em arquivos JSON. Desenvolvido para demonstrar boas práticas de arquitetura, segurança e manipulação de dados.

---

## 🚀 Principais Funcionalidades
- **CRUD Completo:** Criação, leitura, atualização e exclusão de usuários.
- **Autenticação Segura:** Implementação de hashing de senhas (PBKDF2/SHA-256) para proteger credenciais.
- **Persistência de Dados:** Armazenamento local estruturado em arquivos JSON.
- **Arquitetura Organizada:** Lógica de negócio separada em serviços e modelos para melhor manutenção.

---

## 🏗️ Estrutura do Projeto
- `Program.cs` / `Main.cs`: Ponto de entrada da aplicação.
- `Services/`: Lógica de aplicação (AuthService, UsuarioService, etc.).
- `Security/`: Classe `PasswordHasher.cs` para segurança de senhas.
- `Models/`: Classes de domínio (Usuario, UsuarioAuth, etc.).
- `Data/`: Arquivos JSON de persistência.

---

## 🐳 Como rodar com Docker
Se você não quer instalar o .NET na sua máquina, pode rodar o projeto usando Docker:

1. Certifique-se de ter o [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado.
2. Na pasta do projeto, execute:
   ```bash
   docker build -t crud-app .
   docker run -it --rm -v $(pwd)/Data:/app/Data crud-app
   ```
   Caso prefira usar o orquestrador (ideal para terminais nativos do sistema):
   ```bash
   docker-compose up --build --attach crud-app
   ```

---
*Projeto desenvolvido para fins de aprendizado.*