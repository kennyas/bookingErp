using System;

namespace Tornado.Shared.Statics
{
    public class RouteDefaults
    {
        public static Guid RouteId1 = new Guid("7FE1B7EC-1522-4139-A0A8-144372AD63D0");
        public static string RouteName1 = "Berger-Ogba";
        public static string ShortDescription1 = "BRG-OGB";

        public static Guid RouteId2 = new Guid("16883FEC-9651-48C6-81E9-A3FDE4ECD0B3");
        public static string RouteName2 = "Berger-Victoria Island";
        public static string ShortDescription2 = "BRG-VIS";

        public static Guid RouteId3 = new Guid("97F5E279-2ECA-4838-AF7E-061664E96825");
        public static string RouteName3 = "Berger-Ajah";
        public static string ShortDescription3 = "BRG-AJA";
    }

    public class CountryDefaults
    {
        public static Guid CountryId1 = new Guid("185D2E27-319B-43CA-8553-124E358E2345");
        public static string CountryName1 = "Nigeria";
        public static string CountryCode1 = "NGN";
    }

    public class StateDefaults
    {
        public static Guid StateId1 = new Guid("CF2CFEE0-993D-413B-99A8-D3DE01E75B64");
        public static string StateName1 = "Lagos";
        public static string StateCode1 = "LOS";
    }


    public class AreaDefaults
    {
        public static Guid AreaId1 = new Guid("3C57668A-8D81-41C7-9C70-A5CF94910AF6");
        public static string AreaName1 = "Ikeja";
        public static string AreaCode1 = "IKJ";
    } 
    public class PickUpPointDefaults
    {

        public static Guid PPId1 = new Guid("589B7AC3-A7BA-4E72-91CE-326A30A8CBCD");
        public static string PPName1 = "Berger";

        public static Guid PPId2 = new Guid("2BF805B2-4D0A-4829-8873-87569BB17056");
        public static string PPName2 = "FRSC";

        public static Guid PPId3 = new Guid("23A2F524-2BB8-485B-A107-FB00C7CDA2A1");
        public static string PPName3 = "Grammar School";

        public static Guid PPId4 = new Guid("F85920B4-F5E7-412B-AA0A-0CE02C982872");
        public static string PPName4 = "County Hospital";

        public static Guid PPId5 = new Guid("B7A26D56-64B6-4207-9AB0-693421D80CF9");
        public static string PPName5 = "Ogba";

        public static Guid PPId6 = new Guid("A7C61B45-D96B-4FA0-8443-A3F9E54ACC0B");
        public static string PPName6 = "Agege";

        public static Guid PPId7 = new Guid("3A71C629-30A8-42E5-A953-FB105CE2ACB4");
        public static string PPName7 = "Iyana-Oworo";

        public static Guid PPId8 = new Guid("32DBF84B-8E3D-44B4-848A-A4EA50869F07");
        public static string PPName8 = "Victoria Island";

        public static Guid PPId9 = new Guid("E80FE92F-FCE4-46D5-A561-A4E2384E22E9");
        public static string PPName9 = "Lekki";

        public static Guid PPId10 = new Guid("433B43C9-8E59-489A-BB4C-B37A3E1BB6E2");
        public static string PPName10 = "Ajah";
    }

    public class TripDaysDefaults
    {
        public static Guid TripDaysId = new Guid("94058B5E-BEDB-4094-9C90-E3E62EB523BD");
    }
}