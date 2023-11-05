namespace Classcharts_app
{
    internal class LoginResponse
    {
        public int success { get ; set; }
        public meta meta { get; set; }

    }
    internal class meta
    {
        public string session_id { get; set; }
    }
}
