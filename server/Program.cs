﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server {
    public class Program {
        public async static Task Main(string[] args) {
            await Server.StartServer();
        }
    }
}