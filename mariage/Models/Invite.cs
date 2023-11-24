namespace mariage.Models;

public enum TypeInvite
{
    FamilleMarie,
    AmisParentsMarie,
    AmisMarie,
    FamilleMariee,
    AmisParentsMariee,
    AmisMariee
}

public class Invite
{
    public int Id { get; set; }
    public string Nom { get; set; }
    public string Prenom { get; set; }
    public TypeInvite Type { get; set; } // Utilisation de l'enum ici
    public int MariageId { get; set; } // Clé étrangère vers la table Mariage si nécessaire
}
