﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Domain.Itk.Dispatcher.Models
{
    public class StepItem
    {
        public string QuestionId { get; set; }
        public string QuestionNo { get; set; }
        public int AnswerOrder { get; set; }
    }
}
