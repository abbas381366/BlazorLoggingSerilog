using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorLoggingSeriLog.DbModels;

public class DIMPersonelType
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int ID { get; set; }
    public string Title { get; set; }
}
