using System;

namespace mariage.Models;

public class Mariage
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string ThemeTable { get; set; }
    public int RabbinId { get; set; }
}
