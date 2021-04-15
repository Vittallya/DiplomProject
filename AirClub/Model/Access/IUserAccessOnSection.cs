using System;
using System.Collections.Generic;
using System.Text;

namespace AirClub.Model.Access
{
    public interface IUserAccessOnSection
    {
        int AccessIndex { get; set; }
        int Value { get; set; }
    }

    public class UserAccessOnSection : IUserAccessOnSection
    {
        public int AccessIndex { get; set; }
        public int Value { get; set; }

        public UserAccessOnSection() { }

        public UserAccessOnSection(int value, int index): this()
        {
            AccessIndex = index;
            Value = value;
        }
    }
    public enum AccessSectionsIndex
    {
        ReadEditEmployee = 101,
        AddEmployee,
        ReadEditSpecial,
        AddSpecial,
        ReadEditClients,
        AddClients,
        ReadEditService,
        AddService,
        ReadEditTour,
        AddTour,
        ReadEditTransfer,
        AddTransfer,
        ReadEditInsurance,
        AddInsurance,
        ReadEditReservation,
        AddReservation,
        ReadEditStorage,
        AddStorage,
        ReadEditParnters,
        AddParnters

    }

}
