using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glow.Models
{
    using System;
    using System.Collections.Generic;
    public partial class Player_Info
    {
        public int PL_Membership { get; set; }
        public Nullable<int> PL_Mobile { get; set; }
        

        public int Inf_MembershipNo { get; set; }
        public Nullable<System.DateTime> Inf_Startdate { get; set; }
        public Nullable<System.DateTime> Inf_Enddate { get; set; }
        public Nullable<int> Inf_CaptionName { get; set; }
        public Nullable<int> Inf_FrezzDay { get; set; }
        public Nullable<bool> Inf_Cancel { get; set; }

        public List<Player> pl { get; set; }
        public List<Info> inf { get; set; }

        public virtual ICollection<Caption> Caption { get; set; }
    }
}