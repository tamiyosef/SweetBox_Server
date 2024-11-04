using AppServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppServer.Models;
using AppServer.DTO;
using Microsoft.EntityFrameworkCore;


namespace AppServer.Controllers
{
    [Route("api")]
    [ApiController]
    public class SweetBoxController : ControllerBase
    {

        // הגדרת הקישור למסד הנתונים
        private SweetBoxDBContext context;

        //הגדרת סביבת העבודה
        private IWebHostEnvironment WebHostenvironment;


        // הגדרת פעולת בניית המחלקה 
        public SweetBoxController(SweetBoxDBContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.WebHostenvironment = webHostEnvironment;
        }

        // הגדרת פעולת התחברות
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginInfo loginInfo)

        {
            try
            {

                //LogOut any user that is already logged in
                HttpContext.Session.Clear();

                // קבלת פרטי המשתמש ממסד הנתונים
                Models.User user = context.Users.FirstOrDefault(u => u.Email == loginInfo.Email);

                // בדיקה האם המשתמש קיים
                if (user == null)
                {
                    return NotFound();
                }
                if (user.Password != loginInfo.Password)
                {
                    return Unauthorized();
                }
                // החזרת פרטי המשתמש
                HttpContext.Session.SetString("LoggedInUser", user.Email);
                DTO.User DTO_User = new DTO.User(user);

                return Ok(DTO_User);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] DTO.User user_dto)
        {
            if (user_dto == null)
            {
                return BadRequest("Invalid user data.");
            }

            // יצירת יוזר חדש בהתבסס על הקלט מהמשתמש
            Models.User modeluser = new Models.User
            {
                FullName = user_dto.FullName,
                Email = user_dto.Email,
                Password = user_dto.Password,
                UserType = user_dto.UserType,
                DateCreated = DateTime.Now
            };

            // בדיקת תקינות אימייל ייחודי
            var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Email == user_dto.Email);
            if (existingUser != null)
            {
                return Conflict("User with this email already exists.");
            }

            // הוספת המשתמש למסד הנתונים
            context.Users.Add(modeluser);
            await context.SaveChangesAsync(); // שמירת השינויים במסד הנתונים
            return Ok(modeluser.UserId);
        }


        [HttpGet("sellers")]
        public async Task<IActionResult> GetSellers()
        {
            try
            {
                // שליפת רשימת המוכרים ממסד הנתונים
                var sellersList = await context.Sellers.ToListAsync();

                // בדיקה אם יש מוכרים במערכת
                if (sellersList == null || sellersList.Count == 0)
                {
                    return NotFound("No sellers found.");
                }

                return Ok(sellersList); // החזרת הרשימה כלקוח JSON
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("products/{sellerId}")]
        public async Task<IActionResult> GetProductsBySellerId(int sellerId)
        {
            try
            {
                // שליפת מוצרים של המוכר המבוקש מתוך מסד הנתונים
                var products = await context.Products
                                            .Where(p => p.SellerId == sellerId)
                                            .ToListAsync();

                // בדיקה אם נמצאו מוצרים
                if (products == null || !products.Any())
                {
                    return NotFound($"No products found for seller with ID {sellerId}.");
                }

                // החזרת המוצרים ללקוח בפורמט JSON
                return Ok(products);
            }
            catch (Exception ex)
            {
                // טיפול בשגיאה במקרה הצורך
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("RegisterSeller")]
        public async Task<IActionResult> RegisterSeller([FromBody] DTO.Seller seller_dto)
        {
            try
            {   
                // בדיקת תקינות הקלט
                if (seller_dto == null)
                {
                    return BadRequest("Invalid seller data.");
                }

                // מיפוי ה-DTO למודל של ה-Seller
                Models.Seller modelSeller = new Models.Seller
                {
                    SellerId = seller_dto.SellerId, // מזהה המוכר הוא ה-UserId
                    BusinessName = seller_dto.BusinessName,
                    BusinessAddress = seller_dto.BusinessAddress,
                    BusinessPhone = seller_dto.BusinessPhone,
                    ProfilePicture = seller_dto.ProfilePicture,
                    Description = seller_dto.Description
                };

                // הוספת המוכר למסד הנתונים
                context.Sellers.Add(modelSeller);
                await context.SaveChangesAsync(); // שמירת השינויים במסד הנתונים
                return Ok("Seller registered successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("sellers/{sellerId}")]
        public async Task<IActionResult> GetSellerById(int sellerId)
        {
            var seller = await context.Sellers.FirstOrDefaultAsync(s => s.SellerId == sellerId);

            if (seller == null)
            {
                return NotFound($"Seller with ID {sellerId} not found.");
            }

            return Ok(seller);
        }

        [HttpPut("sellers/{sellerId}")]
        public async Task<IActionResult> UpdateSeller(int sellerId, [FromBody] DTO.Seller sellerDto)
        {
            try
            {
                // בדיקה אם הנתונים שהתקבלו מהלקוח תקינים
                if (sellerDto == null)
                {
                    return BadRequest("Invalid seller data.");
                }

                // שליפת המוכר ממסד הנתונים לפי ה-ID שהתקבל מהנתיב
                var existingSeller = await context.Sellers.FirstOrDefaultAsync(s => s.SellerId == sellerId);

                if (existingSeller == null)
                {
                    return NotFound($"Seller with ID {sellerId} not found.");
                }

                // עדכון פרטי המוכר תוך כדי התעלמות מה-SellerId בגוף הבקשה
                existingSeller.BusinessName = sellerDto.BusinessName;
                existingSeller.BusinessAddress = sellerDto.BusinessAddress;
                existingSeller.BusinessPhone = sellerDto.BusinessPhone;
                existingSeller.ProfilePicture = sellerDto.ProfilePicture;
                existingSeller.Description = sellerDto.Description;

                // שמירת השינויים במסד הנתונים
                await context.SaveChangesAsync();

                return NoContent(); // החזרת תגובה ללא תוכן (מוצלח)
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("UpdateUserDetails")]
        public async Task<IActionResult> UpdateUserDetails([FromBody] DTO.User userDto)
        {
            try
            {
                // Find the user in the database using the UserId
                var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == userDto.UserId);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                // Update the user's details
                user.FullName = userDto.FullName;
                user.Email = userDto.Email;
                user.UserType = userDto.UserType;

                // Save the changes to the database
                await context.SaveChangesAsync();

                return Ok("User details updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



    }
}

    