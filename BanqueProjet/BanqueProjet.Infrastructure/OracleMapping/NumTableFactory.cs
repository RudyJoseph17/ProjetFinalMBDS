//using Oracle.ManagedDataAccess.Client;
//using Oracle.ManagedDataAccess.Types;
//using System;

//namespace BanqueProjet.Infrastructure.OracleMapping
//{
//    public class NumTableFactory
//        : IOracleArrayTypeFactory
//    {
//        [OracleArrayMapping()]
//        public decimal[] Array { get; set; }

//        // Obligatoire pour IOracleArrayTypeFactory
//        public Array CreateArray(int numElems)
//            => new decimal[numElems];

//        public Array CreateStatusArray(int numElems)
//            => null;
//    }
//}
