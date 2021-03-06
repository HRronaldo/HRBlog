﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HRBlog.Model.Sys;
using HRBlog.Service.Sys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using HRBlog.Model.CustomModel;

namespace HRBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SysController : Controller
    {
        SysUserService _userService;

        public SysController(IOptions<MyOptions> option)
        {
            _userService = new SysUserService(option.Value.DefaultConnection);
        }
        public IActionResult Index()
        {
            return View();
        }
        [AutoValidateAntiforgeryToken()]
        [HttpPost]
        public IActionResult Index(string oldpassword,string newpwd1,string newpwd2)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return MsgContent("未找到该用户，请联系管理员!");
            }
            if (string.IsNullOrEmpty(oldpassword) || string.IsNullOrEmpty(newpwd1) || string.IsNullOrEmpty(newpwd2))
            {
                return MsgContent("请把修改密码信息填写完整!");
            }

            if (newpwd1 != newpwd2)
            {
                return MsgContent("二次密码输入不相同！");
            }

            oldpassword = Common.Security.MD5Security.MD5Hash(oldpassword);
            int userid= int.Parse(User.Identities.First(u=>u.IsAuthenticated).FindFirst(ClaimTypes.PrimarySid).Value);
            string username =User.Identities.First(u=>u.IsAuthenticated).FindFirst(ClaimTypes.Name).Value;
            var list = _userService.GetList(username, oldpassword);
            if (list == null || list.Count <= 0)
                return MsgContent("原密码错误！");
            bool isSuccess = _userService.Update(new T_SYS_USER() {UserID = userid, Password = Common.Security.MD5Security.MD5Hash(newpwd1)});
            if(isSuccess)
            return Content("<script>alert('修改密码成功！');parent.location.href='/Admin/Login'</script>", "text/html");
            return MsgContent("密码修改失败！");
        }

        private IActionResult MsgContent(string message)
        {
            return Content("<script> alert("+ message + ");</script>" ,"text/html");
        }
    }
}