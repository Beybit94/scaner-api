using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScanerApi.Models
{
    public class PdfViewModels
    {
        public List<GoodsModel> goods = new List<GoodsModel>();
        public DifferencesModel differences = new DifferencesModel();
    }
}