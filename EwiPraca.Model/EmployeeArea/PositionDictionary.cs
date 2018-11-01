﻿using EwiPraca.Model.Base;
using EwiPraca.Model.UserArea;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EwiPraca.Model
{
    public class PositionDictionary : BaseModel
    {
        public int Id { get; set; }

        [Required]
        [Index(IsUnique = true)]
        public int UserCompanyId { get; set; }
        public virtual UserCompany UserCompany { get; set; }

        public virtual List<PositionDictionaryValue> Values { get; set; }
    }
}
