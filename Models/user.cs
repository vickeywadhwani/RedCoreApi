namespace RedCoreApi.Models
{

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
  

    [Table("user")]
    public partial class user
    {
        public int userid { get; set; }

        [Required]
        [StringLength(50)]
        public string name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string email { get; set; }

        [StringLength(15)]
        public string telephone { get; set; }

        [StringLength(500)]
        public string address { get; set; }

        [StringLength(100)]
        public string city { get; set; }

        [StringLength(5)]
        public string post_code { get; set; }
    }
}
