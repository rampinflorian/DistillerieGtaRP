using System;
using System.ComponentModel.DataAnnotations;

namespace DistillerieGtaRP.FormsCustom
{
    public class StatsPeriodFormCustom
    {
        [DataType(DataType.Date)]public DateTime? Start { get; set; }
        [DataType(DataType.Date)]public DateTime? Stop { get; set; }
    }
}