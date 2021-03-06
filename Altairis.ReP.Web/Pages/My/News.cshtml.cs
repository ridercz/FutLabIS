using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Altairis.ReP.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Altairis.ReP.Web.Pages.My {
    public class NewsModel : PageModel {
        private readonly RepDbContext dc;

        public NewsModel(RepDbContext dc) {
            this.dc = dc ?? throw new ArgumentNullException(nameof(dc));
        }

        public IEnumerable<NewsMessage> Messages { get; set; }

        public async Task OnGetAsync() {
            this.Messages = await this.dc.NewsMessages.OrderByDescending(x => x.Date).ToListAsync();
        }

    }
}
