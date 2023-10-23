namespace mariage.Models;

public class Temoin
{
    public int Id { get; set; }
    public string Nom { get; set; }
    public string Prenom { get; set; }
    public string Type { get; set; } // Encore une fois, envisagez d'utiliser un Enum pour le Type
    public int MariageId { get; set; }
}