using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VieshowManager.Models.UserApi.RequestBody
{
    public class LoginReqBody : GenericReqBody
    {
        [Required]
        public string username { set; get; }

        [Required]
        public string password { set; get; }
    }
}