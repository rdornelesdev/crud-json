using System.Text.Json;
using caminhoArquivo;
using UsuarioModel;
using AuthModel;
using Security;
using main;
using System.Runtime.InteropServices;

namespace AuthServiceInstance;

class AuthService : Caminho
{

    public void selectAuth()
    {
        Console.WriteLine("----------");
        Console.WriteLine("Seja bem-vindo!");
        Console.WriteLine("----------");

        string[] opts = ["Login", "Cadastro"];

        int idx = 0;
        foreach (string x in opts)
        {
            Console.WriteLine($"{idx + 1}. {x}");
            idx++;
        }

        Console.Write("Selecione uma opção: ");
        string loginCadastro = Console.ReadLine()!;

        if (int.TryParse(loginCadastro, out int loginCadastroCast) && loginCadastroCast > 0 && loginCadastroCast <= opts.Length)
        {
            switch (loginCadastroCast)
            {
                case 1:
                    Auth();
                    break;

                case 2:
                    AuthCreate();
                    break;
            }
        }
        else
        {
            Console.WriteLine("❌ Selecione corretamente!");
        }

    }

    public void Auth()
    {
        string jsonCad = File.ReadAllText(cadastroUsuariosJson);
        var dadosJsonCadastro = JsonSerializer.Deserialize<List<UsuarioAuth>>(jsonCad) ?? [];

        Console.WriteLine("----------");
        Console.WriteLine("Login");
        Console.WriteLine("----------");

        Console.Write("Digite o usuário: ");
        string usuario = Console.ReadLine()!;

        Console.Write("Digite a senha: ");
        string senha = Console.ReadLine()!;

        if (!string.IsNullOrWhiteSpace(usuario) && !string.IsNullOrWhiteSpace(senha))
        {
            var usuarioEncontrado = dadosJsonCadastro.FirstOrDefault(x => x.Usuario == usuario);

            if (usuarioEncontrado is not null && PasswordHasher.VerifyPassword(senha, usuarioEncontrado.Senha ?? string.Empty))
            {
                Console.Clear();
                Console.WriteLine($"✅ Sucesso! Seja bem-víndo, {usuario}!");
                Main main = new();
                main.MainFnc();
            }
            else
            {
                Console.WriteLine("Usuário ou senha incorretos.");
            }
        }
    }

    public void AuthCreate()
    {
        Console.WriteLine("----------");
        Console.WriteLine("Cadastro");
        Console.WriteLine("----------");

        Console.Write("Usuário: ");
        string usuarioCad = Console.ReadLine()!;
        Console.Write("Senha: ");
        string senhaCad = Console.ReadLine()!;

        if (!string.IsNullOrWhiteSpace(usuarioCad) && usuarioCad.All(char.IsLetter) && !string.IsNullOrWhiteSpace(senhaCad))
        {
            string jsonCad = File.ReadAllText(cadastroUsuariosJson);
            var dadosJsonCad = JsonSerializer.Deserialize<List<UsuarioAuth>>(jsonCad) ?? [];

            dadosJsonCad.Add(new UsuarioAuth
            {
                Id = Guid.NewGuid(),
                Usuario = usuarioCad,
                Senha = PasswordHasher.HashPassword(senhaCad)
            });

            string atualizaJson = JsonSerializer.Serialize(dadosJsonCad, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(cadastroUsuariosJson, atualizaJson);

            Console.Clear();
            Console.WriteLine("✅ Cadastro criado com sucesso!");
            Auth();
        }
        else
        {
            Console.WriteLine("❌ Falha no login! Tente novamente.");
        }
    }
}