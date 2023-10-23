namespace mariage.Models;

public class Invite
{
    public int Id { get; set; }
    public string Nom { get; set; }
    public string Prenom { get; set; }
    public string Type { get; set; } // Vous pouvez envisager d'utiliser un Enum pour le Type
    public int MariageId { get; set; }
}