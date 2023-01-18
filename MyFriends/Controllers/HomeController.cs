using Microsoft.AspNetCore.Mvc;
using MyFriends.Models;
using System.Diagnostics;
using System.Data.Entity;
using MyFriends.ViewsModels;

namespace MyFriends.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        //הפונקציה שעולה ראשונה
        public IActionResult Index()
        {
            //טעינת רשימת החברים ממסד הנתונים כולל התמונות שלהם
            List<Friend> friends =DataLayer.Data.Friends.Include(f=>f.Images).ToList();
            return View(friends);
        }

        //פונקציה ליצירת חבר חדש
        public IActionResult Create()
        {
            //שליחה לדף תצוגה של מודל מוכן המכיל גם חבר חדש וגם מקום לתמונה
            return View(new VMFriendWithImage());
        }

        //פונקציה המקבלת את הטופס המלאה מהמשתמש
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(VMFriendWithImage VM)
        {
            //1. הוספת החבר החדש לטבלה של החברים
            DataLayer.Data.Friends.Add(VM.Friend);
            //2. הוספת התמונה לחבר
            VM.Friend.AddImage(VM.File);
            //3. שמירת הנתונים במסד הנתונים
            DataLayer.Data.SaveChanges();
            //4. צריך להחליט האם עוברים מכן לרשימה הכללית או לפרטי החבר הנוכחי

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            //אם לא קיבלנו תעודת זהות
            if (id == null) { return RedirectToAction(nameof(Index)); }
            Friend friend = DataLayer.Data.Friends.Include(f => f.Images).FirstOrDefault(f => f.ID == id);
            //Friend friend2 = DataLayer.Data.Friends.ToList().Find(f=>f.ID==id); Option 2
            //אם לא קיבלנו תעודת זהות
            if (friend == null) { return RedirectToAction("Index"); }
            return View(friend);
        }

        [HttpPost,ValidateAntiForgeryToken]
        public IActionResult Edit(Friend friend)
        {
            if (friend == null) return RedirectToAction("Index");
            Friend friendDB = DataLayer.Data.Friends.ToList().Find(f=>f.ID== friend.ID);
            if (friendDB == null) return RedirectToAction("Index");
            friendDB.FirstName= friend.FirstName;
            friendDB.LastName= friend.LastName;
            friendDB.Email= friend.Email;
            friendDB.City= friend.City;
            friendDB.Street= friend.Street;
            friendDB.Street= friend.Street;
            friendDB.StreetNum= friend.StreetNum;
            friendDB.PhoneNumber= friend.PhoneNumber;
            DataLayer.Data.SaveChanges();
            return View("Details",new VMFriendWithImage { Friend= friendDB });

        }
        public IActionResult deleteImage(int id)
        {
            Image image = DataLayer.Data.Images.Include(f=>f.Friend).FirstOrDefault(i => i.ID == id);
            if(image== null) return RedirectToAction("Index");
            Friend friend = image.Friend;
            friend.Images.Remove(friend.Images.Find(i=>i.ID==id));
            DataLayer.Data.SaveChanges();
            return View("Details", new VMFriendWithImage { Friend = friend });

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //פונקציה להצגת פרטי חבר
        public IActionResult Details(int? id)
        {   //אם לא קיבלנו תעודת זהות
            if(id==null) { return RedirectToAction(nameof(Index)); }
            Friend friend = DataLayer.Data.Friends.Include(f=>f.Images).FirstOrDefault(f=>f.ID==id);
            //Friend friend2 = DataLayer.Data.Friends.ToList().Find(f=>f.ID==id); Option 2
            //אם לא קיבלנו תעודת זהות
            if(friend==null) { return RedirectToAction("Index"); }
            return View(new VMFriendWithImage { Friend=friend});

        }

        //פונקציה המקבלת תמונה עבור חבר קיים במערכת
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult addImage(VMFriendWithImage VM)
        {
            if (VM == null) RedirectToAction("Index");//במידה ולא מתקבל אוביקט - חזרה לדף הבית
            //מציאת החבר במסד הנתונים והבאת התמונות גם עם אינקלוד
            Friend friend = DataLayer.Data.Friends.Include(f=>f.Images).FirstOrDefault(f => f.ID == VM.Friend.ID);
            if(friend==null) RedirectToAction("Index");//במידה ולא מצא את החבר - חזרה לדף הבית
            if (VM.File!=null) {
                friend.AddImage(VM.File);
                DataLayer.Data.SaveChanges();
            }
            return View("Details",new VMFriendWithImage { Friend=friend });
        }
    }
}