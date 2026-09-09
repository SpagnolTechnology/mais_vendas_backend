namespace Crosscutting.Constant
{
    public static class ExternalApiPaths
    {
        public const string Clients = "api/v1/client";
        public const string Billings = "billings";

        public static string ClientById(int id) => $"{Clients}/id/{id}";
    }
}
