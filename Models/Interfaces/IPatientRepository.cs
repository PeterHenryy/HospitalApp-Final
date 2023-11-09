using HospitalApp.Models.Doctors;
using System.Collections.Generic;

namespace HospitalApp.Models.Interfaces
{
    public interface IPatientRepository
    {
        List<Doctor> GetDoctorsDropdown(); 
        bool CreateCreditCard(CreditCard creditCard);
        bool DeleteCreditCard(int creditCardID);
        CreditCard GetCreditCardByID(int cardID);
        List<CreditCard> GetSpecificUserCards(int userID);
    }
}
