using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DF_WebModule.Models
{
    public class MenuModel
    {
        public int Id { get; set; }
        public string MenuText { get; set; }
        public int ParentMenuId { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string Classtext { get; set; }
        public string Stylesheet { get; set; }
        public string DisplayOrder { get; set; }
        public string HtmlText { get; set; }
        public string Route { get; set; }
        public List<MenuModel> submenu { get; set; }

    }

    public partial class MenuMaster
    {
        public int Id { get; set; }
        public string MenuText { get; set; }
        public string Description { get; set; }
        public Nullable<int> ParentID { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }

        public bool IsChecked { get; set; }
        public List<MenuMaster> menus { get; set; }      
    }

}