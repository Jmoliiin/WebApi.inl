namespace e_shop.Models.Helpers
{
    public class PasswordModel
    {
        public PasswordModel()
        {
        }

        public PasswordModel(string passWord)
        {
            PassWord = passWord;
        }

        public string PassWord { get; set; }
    }
}
