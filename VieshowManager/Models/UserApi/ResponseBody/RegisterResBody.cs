﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VieshowManager.Models.UserApi.ResponseBody
{
    public class RegisterResBody : GenericResBody
    {
        public string token { set; get; }
    }
}