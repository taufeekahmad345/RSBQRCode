using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DF_WebModule.Models
{
    public class UserDetailsModel
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        [Required]
        public long RoleId { get; set; }
        public string Role { get; set; }
        public string DOB { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [DisplayName("Allow Documentation")]
        public string AllowDocumentation { get; set; }
        public string Address { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public List<Role> roles { get; set; }
        public List<Menus> menus { get; set; }
        public List<DocumentListModel> documentLists { get; set; }
        public List<BOMColumnList> bOMColumnLists { get; set; }
    }

    public class BOMColumnList
    {
        public int Id { get; set; }
        public string ColumnName { get; set; }
        public bool IsSelected { get; set; }
    }
    public class DocumentListModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }
    public class Role
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class Menus
    {
        public int id { get; set; }
        public string MenuName { get; set; }
        public bool selecte { get; set; }
    }
}