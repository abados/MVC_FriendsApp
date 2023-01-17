using System.Data.Entity;

namespace MyFriends.Models
{
    public class DataLayer:DbContext
    {
        private static DataLayer data;

        private DataLayer(): base("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=WorkersMVC;Data Source=localhost\\SQLEXPRESS")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DataLayer>());

            //כאשר מסד הנתונים ריק, נפעיל את הפונקציה הזורעת
            if (Friends.Count() == 0) seed();
        }

        //פונקציה הזורעת את מסד הנתונים בפעם הראשונה
        private void seed()
        {
            //יצירת חבר ראשונה בטבלה
            Friend friend = new Friend
            {
                FirstName = "חי",
                LastName = "עבאדי",
                City = "רמת ישי",
                //Date = DateTime.Now.AddYears(-31)


            };
            // הוספת החברה לטבלה
            Friends.Add(friend);
            //שמירת שינויים
            SaveChanges();
        }

        //קישור למודל הפנימי
        public static DataLayer Data
        {
            get
            {
                if (data == null) data= new DataLayer();
                return data;
                
            }
           
        }

        //טבלת חברים
        public DbSet<Friend> Friends { get;set; }
        //טבלת תמונות
        public DbSet<Image> Images { get;set; }

    }
}
