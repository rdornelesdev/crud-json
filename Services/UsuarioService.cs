using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text.Json;
using caminhoArquivo;
using UsuarioModel;

class UsuarioService : Caminho
{
    public void AdicionarUsuario(List<Usuario> usuarios)
    {
        // carrega dados atuais do arquivo
        List<Usuario> dadosJson = new();
        if (File.Exists(usuariosJson))
        {
            string existente = File.ReadAllText(usuariosJson);
            dadosJson = JsonSerializer.Deserialize<List<Usuario>>(existente) ?? new();
        }

        // Na criação vai gerar o ID e a data de cadastro de forma automática.
        Console.Write("Digite um nome: ");
        string nomeUsuario = Console.ReadLine()!;

        // Coloca a primeira letra de cada palavra em maiúscula
        string nomeFormat = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(nomeUsuario.Trim().ToLower());

        Console.Write("Digite a idade: ");
        string idade = Console.ReadLine()!;
        int.TryParse(idade, out int idadeCast);

        Console.Write("Digite o e-mail: ");
        string email = Console.ReadLine()!;
        var validarEmail = new EmailAddressAttribute();

        bool condNome = !string.IsNullOrWhiteSpace(nomeFormat) && Regex.IsMatch(nomeFormat, @"^[a-zA-ZÀ-ÿ\s]+$");
        bool condIdade = idadeCast > 0 && idadeCast < 120;
        bool condEmail = validarEmail.IsValid(email);

        if (condNome && condIdade && condEmail)
        {
            var novo = new Usuario { Id = Guid.NewGuid(), Nome = nomeFormat, Idade = idadeCast, Email = email, DataCadastro = DateTime.Now };
            dadosJson.Add(novo);

            string jsonAtualizado = JsonSerializer.Serialize(dadosJson, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(usuariosJson, jsonAtualizado);

            usuarios.Clear();
            usuarios.AddRange(dadosJson);

            Console.Clear();
            Console.WriteLine("✅ Usuário adicionado com sucesso!");

            foreach (var u in dadosJson)
            {
                Console.WriteLine($"Nome: {u.Nome} | Idade: {u.Idade} | E-mail: {u.Email} | Data de Cadastro: {u.DataCadastro}");
            }
        }
        else
        {
            Console.WriteLine("❌ Preencha os campos corretamente!");
        }
    }

    public void AlterarUsuario(List<Usuario> usuarios)
    {
        // lê o json (com checagem)
        List<Usuario> dadosJson = new List<Usuario>();
        if (File.Exists(usuariosJson))
        {
            string json = File.ReadAllText(usuariosJson);
            dadosJson = JsonSerializer.Deserialize<List<Usuario>>(json) ?? new List<Usuario>();
        }

        if (dadosJson.Count == 0)
        {
            Console.WriteLine("Nenhum usuário encontrado.");
            return;
        }

        // exibe e seleciona
        for (int i = 0; i < dadosJson.Count; i++)
            Console.WriteLine($"{i + 1} - {dadosJson[i].Nome} | {dadosJson[i].Idade} | {dadosJson[i].Email}");

        Console.Write("Qual número deseja alterar? ");
        string selecao = Console.ReadLine()!;
        if (!int.TryParse(selecao, out int selecaoCast) || selecaoCast < 1 || selecaoCast > dadosJson.Count)
        {
            Console.WriteLine("❌ Seleção inválida. Tente novamente");
            return;
        }

        int indice = selecaoCast - 1;

        Console.Clear();
        Console.WriteLine($"Você selecionou: {dadosJson[indice].Nome} | {dadosJson[indice].Idade} | {dadosJson[indice].Email}");

        Console.Write("Nome (pressione Enter para manter): ");
        string alteracaoNome = Console.ReadLine() ?? string.Empty;
        string? nomeFormat = string.IsNullOrWhiteSpace(alteracaoNome)
            ? dadosJson[indice].Nome
            : CultureInfo.CurrentCulture.TextInfo.ToTitleCase(alteracaoNome.Trim().ToLower());

        Console.Write("Idade (pressione Enter para manter): ");
        string alteracaoIdade = Console.ReadLine()!;
        int idadeCast;
        if (string.IsNullOrWhiteSpace(alteracaoIdade))
        {
            idadeCast = dadosJson[indice].Idade;
        }
        else if (!int.TryParse(alteracaoIdade, out idadeCast))
        {
            Console.WriteLine("Idade inválida.");
            return;
        }

        Console.Write("Email (pressione Enter para manter): ");
        string alteracaoEmail = Console.ReadLine()!;
        if (string.IsNullOrWhiteSpace(alteracaoEmail))
        {
            alteracaoEmail = dadosJson[indice].Email ?? string.Empty;
        }
        var validacaoEmail = new EmailAddressAttribute();

        // Agrupando condições corretamente: exigir que NOME seja válido E a idade esteja no intervalo E o email seja válido
        bool condNome = Regex.IsMatch(nomeFormat!, @"^[a-zA-ZÀ-ÿ\s]+$");
        bool condIdade = idadeCast >= 1 && idadeCast <= 120;
        bool condEmail = validacaoEmail.IsValid(alteracaoEmail);

        if (condNome && condIdade && condEmail)
        {
            dadosJson[indice].Nome = nomeFormat;
            dadosJson[indice].Idade = idadeCast;
            dadosJson[indice].Email = alteracaoEmail;
            dadosJson[indice].DataAlteracao = DateTime.Now;

            string novoJson = JsonSerializer.Serialize(dadosJson, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(usuariosJson, novoJson);

            // atualizar a lista em memória passada como parâmetro
            usuarios.Clear();
            usuarios.AddRange(dadosJson);

            Console.WriteLine("✅ Nome alterado e salvo com sucesso!");
        }
        else
        {
            Console.WriteLine("❌ Preencha os campos corretamente.");
        }
    }

    public void ListarUsuarios(List<Usuario> usuarios)
    {
        Console.WriteLine("----------");
        Console.WriteLine("Usuários");
        Console.WriteLine("----------");

        if (File.Exists(usuariosJson))
        {
            string json = File.ReadAllText(usuariosJson);
            var dadosJson = JsonSerializer.Deserialize<List<Usuario>>(json);

            if (dadosJson != null)
            {
                foreach (var j in dadosJson)
                {
                    Console.WriteLine($"Nome: {j.Nome} | Idade: {j.Idade} | E-mail: {j.Email} | Data de Cadastro: {j.DataCadastro}");
                }
            }
            else
            {
                Console.WriteLine("❌ Nenhum usuário encontrado.");
            }
        }
    }

    public void ExcluirUsuario(List<Usuario> usuarios)
    {
        string json = File.ReadAllText(usuariosJson);
        var dadosJson = JsonSerializer.Deserialize<List<Usuario>>(json) ?? [];
        // para manter que o usuarios vai refletir apenas dados atuais, sem manter dados duplicados ou desatualizados, o usuarios é limpado e usado o addrange para copiar os usuarios do usuariosJson para a list
        usuarios.Clear();
        usuarios.AddRange(dadosJson);

        bool validacao = true;
        while (validacao)
        {
            if (dadosJson.Count > 0)
            {
                for (var y = 0; y < dadosJson.Count; y++)
                {
                    Console.WriteLine($"{y + 1} - {dadosJson[y].Nome}");
                }

                Console.WriteLine("Qual usuário você deseja remover (número)?");
                string numeroEscolhido = Console.ReadLine()!;
                int.TryParse(numeroEscolhido, out int numeroEscolhidoCast);

                int indice = numeroEscolhidoCast - 1;

                if (numeroEscolhidoCast >= 1 && numeroEscolhidoCast <= usuarios.Count)
                {
                    try
                    {
                        usuarios.RemoveAt(indice);

                        string salvaNovoJson = JsonSerializer.Serialize(usuarios, new JsonSerializerOptions { WriteIndented = true });
                        File.WriteAllText(usuariosJson, salvaNovoJson);

                        Console.WriteLine("✅ Usuário excluído com sucesso!");

                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        Console.WriteLine("❌ Usuário inexistente.");
                    }
                }
                else
                {
                    Console.WriteLine("❌ Selecione um usuário correto.");
                }

                validacao = false;
            }
            else
            {
                Console.WriteLine("Nenhum usuário encontrado.");
                validacao = false;
            }
        }
    }
}