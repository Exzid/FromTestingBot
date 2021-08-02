﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order
{
    public static class Enums
    {
        public enum UserWait
        {
            NoWait = 0,
            Email,
            Phone
        }
        public enum ErrFormatCode
        {           
            editRate = 1,
            addAdmin,
            removeAdmin,
            rate
        }
    }
}
