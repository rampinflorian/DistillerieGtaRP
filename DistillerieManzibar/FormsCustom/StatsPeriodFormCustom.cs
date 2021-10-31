using System;
using System.ComponentModel.DataAnnotations;

namespace DistillerieManzibar.FormsCustom
{
    public class StatsPeriodFormCustom
    {
        [DataType(DataType.Date)]public DateTime? Start { get; set; }
        [DataType(DataType.Date)]public DateTime? Stop { get; set; }
    }
}