//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Glow.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    public partial class News
    {
        [AllowHtml]
        public int id { get; set; }
        public string Newsname_Ar { get; set; }
        public string Newsname { get; set; }
        public string Newstitle_Ar { get; set; }
        public string Newstitle { get; set; }
        public string Newscontent_Ar { get; set; }
        public string Newscontent { get; set; }
        public string NewsImage { get; set; }
        public Nullable<System.DateTime> NewsDate { get; set; }
        public Nullable<bool> NewsDisplay { get; set; }

        public List<News> newss { get; set; }
        public int CurrentPageIndex { get; set; }
        public int PageCount { get; set; }

    }
}