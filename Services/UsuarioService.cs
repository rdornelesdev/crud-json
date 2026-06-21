using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using UsuarioModel;

class UsuarioService
{
    public string arquivo = "Data/usuarios.json";

    public void AdicionarUsuario(List<Usuario> usuarios)
    {
        // Na criação vai gerar o ID e a data de cadastro de forma automática.
        Console.Write("Digite um nome: ");
        string nomeUsuario = Console.ReadLine()!;

        if (string.IsNullOrWhiteSpace(nomeUsuario))
        {
            Console.WriteLine("Nome inválido!");
            return;
        }

        Console.Write("Digite a idade: ");
        string idade = Console.ReadLine()!;
        int.TryParse(idade, out int idadeCast);

        Console.Write("Digite o e-mail: ");
        string email = Console.ReadLine()!;
        var validarEmail = new EmailAddressAttribute();

        string nomeFormat = char.ToUpper(nomeUsuario[0]) + nomeUsuario.Substring(1).ToLower();

        if (!string.IsNullOrWhiteSpace(nomeUsuario) && nomeFormat.All(char.IsLetter) && idadeCast > 0 && idadeCast < 120 && !string.IsNullOrWhiteSpace(email) && validarEmail.IsValid(email))
        {
            // adiciona no array (memória)
            usuarios.Add(new Usuario { Id = Guid.NewGuid(), Nome = nomeFormat, Idade = idadeCast, Email = email, DataCadastro = DateTime.Now });

            // adiciona no usuarios.json
            string jsonAtualizado = JsonSerializer.Serialize(usuarios, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(arquivo, jsonAtualizado);

            Console.Clear();

            Console.WriteLine("✅ Usuário adicionado com sucesso!");

            // for each no "usuarios" devido que o List<"Usuario"> pega as props de Usuario.cs, consequentemente capturamos os values da lista 
            foreach (var u in usuarios)
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
        // lê o json 
        string json = File.ReadAllText(arquivo);
        // deserializa o json no formato do List<Usuario>
        var dadosJson = JsonSerializer.Deserialize<List<Usuario>>(json) ?? [];

        bool validation = true;
        while (validation)
        {
            if (dadosJson.Count > 0)
            {
                // exibe os usuários de forma númerada
                for (int i = 0; i < dadosJson.Count; i++)
                {
                    Console.WriteLine($"{i + 1} - {dadosJson[i].Nome} | {dadosJson[i].Idade} | {dadosJson[i].Email}");
                }

                Console.Write("Qual número deseja alterar?");
                string selecao = Console.ReadLine()!;

                if (int.TryParse(selecao, out int selecaoCast) && selecaoCast >= 1 && selecaoCast <= dadosJson.Count)
                {
                    Console.Clear();

                    int indice = selecaoCast - 1;
                    Console.WriteLine($"Você selecionou: {dadosJson[indice].Nome} | {dadosJson[indice].Idade} | {dadosJson[indice].Email}");

                    Console.Write("Nome: ");
                    string alteracaoNome = Console.ReadLine()!;
                    string nomeFormat = char.ToUpper(alteracaoNome[0]) + alteracaoNome.Substring(1).ToLower();

                    Console.Write("Idade: ");
                    string alteracaoIdade = Console.ReadLine()!;
                    int.TryParse(alteracaoIdade, out int idadeCast);

                    Console.Write("Email: ");
                    string alteracaoEmail = Console.ReadLine()!;
                    var validacaoEmail = new EmailAddressAttribute();

                    if (!string.IsNullOrWhiteSpace(nomeFormat) && nomeFormat.All(char.IsLetter) && idadeCast > 1 && validacaoEmail.IsValid(alteracaoEmail))
                    {
                        // altera os dados
                        dadosJson[indice].Nome = nomeFormat;
                        dadosJson[indice].Idade = idadeCast;
                        dadosJson[indice].Email = alteracaoEmail;
                        dadosJson[indice].DataAlteracao = DateTime.Now;

                        // salva a lista atualizada no usuarios.json
                        string novoJson = JsonSerializer.Serialize(dadosJson, new JsonSerializerOptions { WriteIndented = true });
                        File.WriteAllText(arquivo, novoJson);

                        Console.WriteLine("✅ Nome alterado e salvo com sucesso!");
                        validation = false;
                    }
                    else
                    {
                        Console.WriteLine("❌ Preencha os campos corretamente.");
                    }
                }
                else
                {
                    Console.WriteLine("❌ Seleção inválida. Tente novamente");
                }
            }
            else
            {
                Console.WriteLine("Nenhum usuário encontrado.");
                validation = false;
            }
        }
    }

    public void ListarUsuarios(List<Usuario> usuarios)
    {
        Console.WriteLine("----------");
        Console.WriteLine("Usuários");
        Console.WriteLine("----------");

        if (File.Exists(arquivo))
        {
            string json = File.ReadAllText(arquivo);
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
        string json = File.ReadAllText(arquivo);
        var dadosJson = JsonSerializer.Deserialize<List<Usuario>>(json) ?? [];
        // para manter que o usuarios vai refletir apenas dados atuais, sem manter dados duplicados ou desatualizados, o usuarios é limpado e usado o addrange para copiar os usuarios do arquivo para a list
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
                        File.WriteAllText(arquivo, salvaNovoJson);

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