using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace BasiclyIdentity.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View("Index");
        }

        [Authorize]
        public IActionResult Secert()
        {
            return View("Secert");
        }

        public IActionResult Login()
        {
            //Claim类似于身份证的某条内容，一条内容对应一条Claim.例如：民族：汉、籍贯：浙江杭州 此处用的是学校的学生证
            var schoolClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"李雷"),//姓名
                new Claim(ClaimTypes.SerialNumber,"0001"),//学号
                new Claim("Gender","男"),//性别
            };

            //Claim类似于身份证的某条内容，一条内容对应一条Claim.例如：民族：汉、籍贯：浙江杭州 此处用的是社会上的驾照
            var drivePass = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"李雷"),//姓名
                new Claim(ClaimTypes.SerialNumber,"浙A00000"),//车牌号
                new Claim("Driver","GoodJob"),//开车技术怎么样...随便写的
            };

            //ClaimsIdentity 类似于身份证、学生证。它是有一条或者多条Claim组合而成。这样就是组成了一个学生证和驾照
            var schoolIdentity = new ClaimsIdentity(schoolClaims, "school");
            var govIdentity = new ClaimsIdentity(drivePass, "gov");

            //claimsPrincipal相当于一个人，你可以指定这个人持有那些ClaimsIdentity（证件）,我指定他持有schoolIdentity、govIdentity那么他就是
            //在学校里是学生，在社会上是一名好司机
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(new[] { schoolIdentity, govIdentity });

            //HttpContext上下文登录。会根据你在StartUp.cs文件中配置的services.AddAuthentication("CookiesAuth").AddCookie("CookiesAuth")进行操作
            //此处就是增加了Cookies
            HttpContext.SignInAsync(claimsPrincipal);
            //HttpContext上下文登出,会清楚Cookies
            //HttpContext.SignOutAsync()
            return View("Index");
        }
    }
}