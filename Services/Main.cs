using UsuarioModel;

namespace main;

class Main
{


    public void MainFnc()
    {
        List<Usuario> usuario = new();

        bool validacao = true;
        while (validacao)
        {
            string[] opcoes = ["1. Adicionar usuário", "2. Listar usuários", "3. Alterar usuário específico", "4. Remover usuário específico", "5. Sair"];

            Console.WriteLine("----------");
            Console.WriteLine("Painel principal");
            Console.WriteLine("----------");

            foreach (var y in opcoes)
            {
                Console.WriteLine(y);
            }

            Console.WriteLine("Selecione uma opção: ");
            string opcaoSelecionada = Console.ReadLine()!;
            int.TryParse(opcaoSelecionada, out int opcaoSelecionadaCast);

            switch (opcaoSelecionadaCast)
            {
                case 1:
                    Console.Clear();
                    UsuarioService add = new();
                    add.AdicionarUsuario(usuario);
                    break;

                case 2:
                    Console.Clear();
                    UsuarioService list = new();
                    list.ListarUsuarios(usuario);
                    break;

                case 3:
                    Console.Clear();
                    UsuarioService alt = new();
                    alt.AlterarUsuario(usuario);
                    break;

                case 4:
                    Console.Clear();
                    UsuarioService del = new();
                    del.ExcluirUsuario(usuario);
                    break;

                case 5:
                    Console.WriteLine("Saindo...");
                    validacao = false;
                    break;

                default:
                    Console.WriteLine("Opção inválida. Selecione uma opção válida!");
                    break;
            }
        }
    }
}