using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VieshowManager.Models.UserApi.RequestBody
{
    public class RegisterReqBody : GenericReqBody
    {
        [Required]
        public string username { set; get; }

        [Required]
        public string password { set; get; }

        [Required]
        [Compare("password", ErrorMessage = "密碼和確認密碼不相符。")]
        public string confirmPassword { set; get; }
    }
}