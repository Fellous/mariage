using System;

namespace mariage.Models;

public class Mariages
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string ThemeTable { get; set; }
    public int RabbinId { get; set; }
}
