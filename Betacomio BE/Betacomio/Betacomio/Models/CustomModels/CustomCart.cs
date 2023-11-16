namespace Betacomio.Models.CustomModels
{
    public class CustomCart
    {
        public int userID { get; set; }
        public string cartItems { get; set; }  //questo è l'array trasformato in string Base64
    }
}
