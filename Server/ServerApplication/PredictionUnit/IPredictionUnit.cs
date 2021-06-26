using ServerApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApplication.PredictionUnit
{
    public interface IPredictionUnit
    {
        string CurrentPrediction { get; set; }
    }
}
