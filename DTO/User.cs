using AppServer.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AppServer.DTO
{
    public class User


    {
        // נשאיר את השדות הרלוונטיים להעברת הנתונים ללקוח בלבד
        public int UserId { get; set; } 

        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string UserType { get; set; } = null!;


        // בנאי ברירת מחדל לצורך דסיריאליזציה מה-JSON
        public User() { }

        //פעולה בונה שתיצור אובייקוט משתמש מסוג מחלקת DTO 
        public User(Models.User user)
        {
            this.UserId = user.UserId;
            this.FullName = user.FullName;
            this.Email = user.Email;
            this.Password = user.Password;
            this.UserType = user.UserType;
            //this.ProfilePicture = user.ProfilePicture;
        }
    }
}
