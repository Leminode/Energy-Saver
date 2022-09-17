﻿using System.ComponentModel;

namespace Energy_Saver.Pages
{
    public class Records
    {
        public int Id { get; set; }

        [DisplayName("Month")]
        public string Month { get; set; }

        [DisplayName("Gas Price")]
        public string GasPrice { get; set; }

        [DisplayName("Gas Amount")]
        public string GasAmount { get; set; }
    }
}
