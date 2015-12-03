﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AddressBook.Resources;
using System.Web.Security;
using System.Data.Entity;
using System.Configuration;
using System.Threading.Tasks;

namespace AddressBook.Models
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        public int Type { get; set; }

        [Required]
        public string Header { get; set; }

        [Required]
        public string Body { get; set; }

        public bool isHTML { get; set; }
    }

    public enum EnumMessageType
    {
        Email = 0,
        SMS = 1
    }
}