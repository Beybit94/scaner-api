using Data.Model.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Model
{
    [Table("Users")]
    public class Users:Entity
    {
        public int UserId { get; set; }
        public int UserDivisionId { get; set; }
        public string UserFirstName { get; set; }
        public string UserSecondName { get; set; }
        public string UserMiddleName { get; set; }
        public string UserGuid { get; set; }
    }
}
