namespace UsuarioModel;

class Usuario
{
    public Guid Id { get; set; }
    public string? Nome { get; set; }
    public int Idade { get; set; }
    public string? Email { get; set; }
    public DateTime DataCadastro { get; set; }
    public DateTime? DataAlteracao { get; set; }
}