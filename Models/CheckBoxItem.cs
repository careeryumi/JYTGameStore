/*
 * CheckBoxItem.cs (Model)
 * JYTGameStore Project
 * 
 *  Revision History
 *      Jeonghwan Ju, 2021.10.26: Created
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JYTGameStore.Models
{
    public class CheckBoxItem
    {
        public int CheckId { get; set; }
        public string CheckTitle { get; set; }
        public bool IsChecked { get; set; }
    }
}
