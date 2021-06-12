﻿using DepremsizHayat.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.Business.IService
{
    public interface IAnalyseRequestAnswerService
    {
        BaseResponse ReplyRequest(string requestId, string answer, int score);
    }
}