using System;
using System.Collections.Generic;
using System.Text;
using ETradeApiV1.Client.Services;

namespace ETradeApiV1.Client
{
    public abstract class EtradeApiOptionExtensions
    {
        public abstract EtApiServiceOptions GetFromSql();
    }
}
