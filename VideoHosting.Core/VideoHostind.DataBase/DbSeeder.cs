using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VideoHosting.Services.Entities;

namespace VideoHostind.DataBase
{
    public  class DbSeeder
    {
        public async Task Seed(UserManager<UserLogin> userManager, RoleManager<UserRole> roleManager, DataBaseContext context)
        {
            UserRole role1 = new UserRole { Name = "Admin", Description = "Can delete video, users, comentaries" };
            UserRole role2 = new UserRole { Name = "User", Description = "Can add video, comentaries, likes" };
            UserRole role3 = new UserRole { Name = "TsarBatushka", Description = "Can do everything that admin and can add,delete them" };

            await roleManager.CreateAsync(role2);
            await roleManager.CreateAsync(role1);
            await roleManager.CreateAsync(role3);

            Country Ukraine = new Country { CountryName = "Ukraine" };
            Country Belarus = new Country { CountryName = "Belarus" };
            Country Russia = new Country { CountryName = "Russia" };
            Country German = new Country { CountryName = "German" };
            Country US = new Country { CountryName = "US" };

            context.Countries.AddRange(new Country[] { Ukraine, Belarus, Russia, German, US });
            context.SaveChanges();


            UserLogin user1 = new UserLogin { Email = "dyshkant2804@ukr.net", UserName = "AllaDyshkant", PhoneNumber = "380683925657" };
            UserProfile userProfile1 = new UserProfile()
            {
                Name = "Alla",
                Surname = "Dyshkant",
                Birthday = new DateTime(1977, 5, 15),
                DateOfCreation = DateTime.Now,
                UserLogin = user1,
                TempPassword = 0,
                Sex = false,
            };
            user1.UserProfile = userProfile1;
            var r1 = await userManager.CreateAsync(user1, "Dyshkant2804");
            user1.UserName = user1.Id;
            var r11 = await userManager.UpdateAsync(user1);

            UserLogin user2 = new UserLogin { Email = "dyshkant280400@ukr.net", UserName = "OlegDyshkant", PhoneNumber = "380683925658" };
             UserProfile userProfile2 = new UserProfile()
            {
                Name = "Oleg",
                Surname = "Dyshkant",
                Birthday = new DateTime(1974, 6, 29),
                DateOfCreation = DateTime.Now,
                UserLogin = user2,
                TempPassword = 0,
                Sex = true
            };
            user2.UserProfile = userProfile2;
            var r2 = await userManager.CreateAsync(user2, "Dyshkant280400");
            user2.UserName = user2.Id;
            var r22 = await userManager.UpdateAsync(user2);

            UserLogin admin = new UserLogin { Email = "vivavictro28@ukr.net", UserName = "Tsar", PhoneNumber = "380682819737" };
             UserProfile adminProfile = new UserProfile()
            {
                Name = "Victor",
                Surname = "Dyshkant",
                Birthday = new DateTime(2000, 4, 28),
                DateOfCreation = DateTime.Now,
                TempPassword = 0,
                Sex = true
            };
            admin.UserProfile = adminProfile;
            var r3 = await userManager.CreateAsync(admin, "Qwerty280400");
            admin.UserName = admin.Id;
            var r33 =  userManager.UpdateAsync(admin);

            
           
           

            //context.UserProfiles.Add(userProfile1);
            //context.UserProfiles.Add(userProfile2);
            //context.UserProfiles.Add(adminProfile);


            await userManager.AddToRoleAsync(admin, role1.Name);
            await userManager.AddToRoleAsync(admin, role2.Name);
            await userManager.AddToRoleAsync(admin, role3.Name);

            await userManager.AddToRoleAsync(user1, role2.Name);
            await userManager.AddToRoleAsync(user2, role2.Name);

            await context.SaveChangesAsync();
        }
    }
}
