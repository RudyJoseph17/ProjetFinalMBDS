//using Oracle.ManagedDataAccess.Client;
//using Oracle.ManagedDataAccess.Types;

//namespace BanqueProjet.Infrastructure.OracleMappings;

//[OracleCustomTypeMapping("JOSEPHRUDY.LISTREFASPECTS_LEGAUX_T")]
//public class ListRefAspectsLegauxFactory : /*IOracleCustomType, IOracleCustomTypeFactory,*/ IOracleArrayTypeFactory
//{
//    [OracleArrayMapping()]
//    public decimal[] Array { get; set; }

//    //public IOracleCustomType CreateObject() => new ListRefAspectsLegauxFactory();

//    public Array CreateArray(int numElems) => new decimal[numElems];

//    public Array CreateStatusArray(int numElems) => null;

//    public void FromCustomObject(OracleConnection con, IntPtr pUdt) =>
//        OracleUdt.SetValue(con, pUdt, 0, Array);

//    public void ToCustomObject(OracleConnection con, IntPtr pUdt) =>
//        Array = (decimal[])OracleUdt.GetValue(con, pUdt, 0);

//    //void IOracleCustomType.FromCustomObject(OracleConnection con, object udt)
//    //{
//    //    throw new NotImplementedException();
//    //}

//    //void IOracleCustomType.ToCustomObject(OracleConnection con, object udt)
//    //{
//    //    throw new NotImplementedException();
//    //}
//}
