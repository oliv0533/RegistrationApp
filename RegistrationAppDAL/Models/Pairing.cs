namespace RegistrationAppDAL.Models
{
    public class Pairing
    {
        public ApplicationUser Male { get; set; }

        public ApplicationUser Female { get; set; }

        public Pairing(ApplicationUser male, ApplicationUser female)
        {
            Male = male;
            Female = female;
        }
    }
}