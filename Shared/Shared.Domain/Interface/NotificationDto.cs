using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Domain.ApplicationUsers
{
    public class NotificationDto
    {
        public int IdNotification { get; set; }
        public string? Titre { get; set; }
        public string? Message { get; set; }
        public string? IdReceveur { get; set; }
        public string? EstLuChar { get; set; }
        public string? EstLuBool { get; set; }
        public DateTime? DateNotification { get; set; }
        public string? Resume { get; set; }
    }
}
