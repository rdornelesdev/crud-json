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

        if (!string.IsNullOrWhiteSpace(nomeUsuario))
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

        if(!string.IsNullOrWhiteSpace(nomeUsuario) && nomeFormat.All(char.IsLetter) && idadeCast > 0 && idadeCast < 120 && !string.IsNullOrWhiteSpace(email) && validarEmail.IsValid(email))
        {
            // adiciona no array (memória)
            usuarios.Add(new Usuario { Id = Guid.NewGuid(), Nome = nomeFormat, Idade = idadeCast, Email = email, DataCadastro = DateTime.Now });

            // adiciona no usuarios.json
            string jsonAtualizado = JsonSerializer.Serialize(usuarios, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(arquivo, jsonAtualizado);

            Console.Clear();

            Console.WriteLine("✅ Usuário adicionado com sucesso!");

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

    public void AlterarUsuario()
    {
        
    }

    public void ListarUsuarios()
    {
        
    }

    public void ExcluirUsuario()
    {
        
    }
}