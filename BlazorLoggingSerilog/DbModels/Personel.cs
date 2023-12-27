using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorLoggingSeriLog.DbModels;

public class Personel
{
    [Key]
    public string CodeMeli { get; set; }
    public string Name { get; set; }
    public string Family { get; set; }
    public int IDDimPersonelType { get; set; }
    [ForeignKey(nameof(IDDimPersonelType))]
    public DIMPersonelType DIMPersonelType { get; set; }


}
